using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

/// <summary>
/// Herramienta de editor del paso 13: corta las hojas de tiles de los packs,
/// arma las texturas de 9 partes de cada material, la hoja de decoración y los fondos de parallax.
/// Solo genera arte en Assets/_Xolotl/Art/Tiles; no toca escenas.
/// </summary>
public static class GeneradorEscenarios
{
    const int T = 16;
    const string Anokolisa = "Assets/_Xolotl/Art/Terceros/NES_Village_Anokolisa/";
    const string Segart = "Assets/_Xolotl/Art/Terceros/Graveyard_Segart/";
    const string Salida = "Assets/_Xolotl/Art/Tiles/";

    [MenuItem("Xolotl/Generar arte de escenarios")]
    public static void GenerarTodo()
    {
        Directory.CreateDirectory(Salida);
        CortarHoja(Anokolisa + "Assets/Tiles.png");
        CortarHoja(Segart + "tileset/tileset.png");
        GenerarMateriales();
        GenerarDecor();
        GenerarFondos();
        AssetDatabase.SaveAssets();
    }

    // ---------- Lectura y escritura de píxeles ----------

    static Texture2D Leer(string ruta)
    {
        var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        tex.LoadImage(File.ReadAllBytes(ruta));
        return tex;
    }

    static Texture2D Nueva(int w, int h)
    {
        var tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
        tex.SetPixels32(new Color32[w * h]);
        return tex;
    }

    // Coordenadas de tile contadas desde arriba a la izquierda, como se ven en la hoja.
    static Color32[] Tile(Texture2D src, int cx, int cy, int w = 1, int h = 1)
    {
        var c = src.GetPixels(cx * T, src.height - (cy + h) * T, w * T, h * T);
        return c.Select(p => (Color32)p).ToArray();
    }

    // Pega un bloque de píxeles con su esquina superior izquierda en (x, yArriba).
    static void Pegar(Texture2D dst, Color32[] px, int w, int h, int x, int yArriba, bool mezclar = false)
    {
        int y0 = dst.height - yArriba - h;
        for (int j = 0; j < h; j++)
            for (int i = 0; i < w; i++)
            {
                int dx = x + i, dy = y0 + j;
                if (dx < 0 || dy < 0 || dx >= dst.width || dy >= dst.height) continue;
                var c = px[j * w + i];
                if (mezclar && c.a == 0) continue;
                dst.SetPixel(dx, dy, c);
            }
    }

    static string Guardar(Texture2D tex, string nombre)
    {
        string ruta = Salida + nombre + ".png";
        File.WriteAllBytes(ruta, tex.EncodeToPNG());
        AssetDatabase.ImportAsset(ruta, ImportAssetOptions.ForceUpdate);
        return ruta;
    }

    // ---------- Importación y sprites ----------

    static void Configurar(TextureImporter ti, bool multiple)
    {
        ti.textureType = TextureImporterType.Sprite;
        ti.spriteImportMode = multiple ? SpriteImportMode.Multiple : SpriteImportMode.Single;
        ti.spritePixelsPerUnit = T;
        ti.filterMode = FilterMode.Point;
        ti.textureCompression = TextureImporterCompression.Uncompressed;
        ti.mipmapEnabled = false;
        ti.alphaIsTransparency = true;
        ti.wrapMode = TextureWrapMode.Clamp;
        var s = new TextureImporterSettings();
        ti.ReadTextureSettings(s);
        s.spriteMeshType = SpriteMeshType.FullRect;
        ti.SetTextureSettings(s);
    }

    static SpriteRect Rect(string nombre, int x, int y, int w, int h, Vector4 borde)
    {
        return new SpriteRect
        {
            name = nombre,
            rect = new Rect(x, y, w, h),
            border = borde,
            alignment = SpriteAlignment.Center,
            pivot = new Vector2(0.5f, 0.5f),
        };
    }

    // Rectángulo en tiles contado desde arriba (para hojas con orden visual).
    static SpriteRect RectTiles(Texture2D tex, string nombre, int cx, int cy, int w, int h)
    {
        return Rect(nombre, cx * T, tex.height - (cy + h) * T, w * T, h * T, Vector4.zero);
    }

    static void DefinirSprites(string ruta, List<SpriteRect> rects)
    {
        var ti = (TextureImporter)AssetImporter.GetAtPath(ruta);
        Configurar(ti, true);
        ti.SaveAndReimport();

        var fabrica = new SpriteDataProviderFactories();
        fabrica.Init();
        var dp = fabrica.GetSpriteEditorDataProviderFromObject(ti);
        dp.InitSpriteEditorDataProvider();

        // Conserva los IDs de los sprites que ya existían para no romper referencias en escenas.
        var previos = dp.GetSpriteRects().ToDictionary(r => r.name, r => r.spriteID);
        foreach (var r in rects)
            r.spriteID = previos.TryGetValue(r.name, out var id) ? id : GUID.Generate();

        dp.SetSpriteRects(rects.ToArray());
        var nombres = dp.GetDataProvider<ISpriteNameFileIdDataProvider>();
        nombres?.SetNameFileIdPairs(rects.Select(r => new SpriteNameFileIdPair(r.name, r.spriteID)));
        dp.Apply();
        ti.SaveAndReimport();
    }

    // Grid By Cell Size 16x16, sin las celdas vacías.
    static void CortarHoja(string ruta)
    {
        var tex = Leer(ruta);
        string baseNombre = Path.GetFileNameWithoutExtension(ruta);
        var rects = new List<SpriteRect>();
        for (int cy = 0; cy < tex.height / T; cy++)
            for (int cx = 0; cx < tex.width / T; cx++)
                if (Tile(tex, cx, cy).Any(p => p.a > 0))
                    rects.Add(RectTiles(tex, $"{baseNombre}_{cx}_{cy}", cx, cy, 1, 1));
        DefinirSprites(ruta, rects);
    }

    // ---------- Materiales de 9 partes ----------

    static void GenerarMateriales()
    {
        var anok = Leer(Anokolisa + "Assets/Tiles.png");
        var seg = Leer(Segart + "tileset/tileset.png");

        // Tierra del panteón: el bloque 3x3 de Segart (pasto arriba, tierra en capas).
        var tierra = Nueva(48, 48);
        Pegar(tierra, Tile(seg, 0, 1, 3, 3), 48, 48, 0, 0);
        GuardarMaterial(tierra, "Tierra_Panteon");

        // Piedra del pueblo: losa gris de la calle sobre ladrillo verde oscuro.
        GuardarMaterial(Componer(anok, new[] { (0, 0), (0, 0), (2, 0) }, new[] { (3, 15), (3, 15), (3, 15) },
            new[] { (3, 15), (3, 15), (3, 15) }, 1f, true), "Piedra_Pueblo");

        // Techo del pueblo: tablón con viga arriba y tablas verticales de madera.
        GuardarMaterial(Componer(anok, new[] { (19, 0), (20, 0), (21, 0) }, new[] { (22, 1), (23, 1), (24, 1) },
            new[] { (22, 2), (23, 2), (24, 2) }, 1f, true), "Techo_Pueblo");

        // Piedra de la iglesia: cornisa de piedra con viga y muro de piedra gris verdosa.
        GuardarMaterial(Componer(anok, new[] { (17, 4), (18, 4), (19, 4) }, new[] { (6, 18), (7, 18), (8, 18) },
            new[] { (6, 18), (7, 18), (8, 18) }, 1f, true), "Piedra_Iglesia");

        // Piedra de la cripta: la versión más oscura (ladrillo verde oscurecido).
        GuardarMaterial(Componer(anok, new[] { (9, 15), (10, 15), (11, 15) }, new[] { (3, 15), (3, 15), (3, 15) },
            new[] { (3, 15), (3, 15), (3, 15) }, 0.6f, true), "Piedra_Cripta");
    }

    static Texture2D Componer(Texture2D src, (int, int)[] arriba, (int, int)[] medio, (int, int)[] abajo, float brillo, bool contorno)
    {
        var tex = Nueva(48, 48);
        var filas = new[] { arriba, medio, abajo };
        for (int f = 0; f < 3; f++)
            for (int c = 0; c < 3; c++)
                Pegar(tex, Tile(src, filas[f][c].Item1, filas[f][c].Item2), T, T, c * T, f * T);

        var px = tex.GetPixels32();
        for (int i = 0; i < px.Length; i++)
        {
            int x = i % 48, y = i / 48;
            var p = px[i];
            p.r = (byte)(p.r * brillo); p.g = (byte)(p.g * brillo); p.b = (byte)(p.b * brillo);
            p.a = 255;
            // Contorno oscuro de 1 px a los lados y abajo para que el bloque se lea como pieza.
            if (contorno && (x == 0 || x == 47 || y == 0)) p = new Color32(8, 8, 14, 255);
            px[i] = p;
        }
        tex.SetPixels32(px);
        return tex;
    }

    // Variantes del material para que los bloques delgados no se aplasten:
    // Bloque (9 partes, alto >= 2), Franja (alto 1 a 2), Fino (alto < 1),
    // Columna / ColumnaFranja / ColumnaFino (ancho < 2) y Centro (relleno sin orillas).
    static void GuardarMaterial(Texture2D tex, string nombre)
    {
        string ruta = Guardar(tex, nombre);
        DefinirSprites(ruta, new List<SpriteRect>
        {
            Rect(nombre + "_Bloque", 0, 0, 48, 48, new Vector4(16, 16, 16, 16)),
            Rect(nombre + "_Franja", 0, 16, 48, 32, new Vector4(16, 0, 16, 16)),
            Rect(nombre + "_Fino", 0, 32, 48, 16, new Vector4(16, 4, 16, 4)),
            Rect(nombre + "_Columna", 16, 0, 16, 48, new Vector4(0, 16, 0, 16)),
            Rect(nombre + "_ColumnaFranja", 16, 16, 16, 32, new Vector4(0, 0, 0, 16)),
            Rect(nombre + "_ColumnaFino", 16, 32, 16, 16, new Vector4(0, 4, 0, 4)),
            Rect(nombre + "_Centro", 16, 16, 16, 16, Vector4.zero),
        });
    }

    // ---------- Decoración ----------

    static void GenerarDecor()
    {
        // La hoja de Social/Assets.png es la misma de Tiles.png a 4x, pero trae piezas extra
        // (cruz del altar, candelabro, púlpito). Se reduce a 1x copiando un píxel de cada 4.
        var grande = Leer(Anokolisa + "Social/Assets.png");
        var hoja = Nueva(grande.width / 4, grande.height / 4);
        for (int y = 0; y < hoja.height; y++)
            for (int x = 0; x < hoja.width; x++)
                hoja.SetPixel(x, y, grande.GetPixel(x * 4, y * 4));
        string ruta = Guardar(hoja, "Anokolisa_Decor");

        var piezas = new (string, int, int, int, int)[]
        {
            ("Fachada_Torre", 0, 1, 6, 9),
            ("Fachada_Gablete", 6, 0, 6, 6),
            ("Fachada_Madera", 17, 0, 5, 4),
            ("Puerta_Arco", 25, 0, 2, 3),
            ("Puerta_Iglesia", 6, 11, 3, 4),
            ("Ventanales", 6, 9, 3, 2),
            ("Ventana", 0, 8, 2, 2),
            ("Ventana_Azul", 2, 19, 1, 2),
            ("Farol", 12, 5, 1, 3),
            ("Troncos", 2, 13, 3, 1),
            ("Maceta", 8, 16, 1, 2),
            ("Olla", 8, 17, 1, 1),
            ("Letrero", 24, 4, 2, 2),
            ("Cruz_Altar", 17, 14, 3, 4),
            ("Candelabro", 20, 10, 2, 2),
            ("Pulpito", 24, 8, 2, 2),
            ("Altar_Mesa", 22, 10, 3, 2),
            ("Columna", 1, 16, 2, 3),
            ("Lapida", 7, 19, 1, 1),
            ("Lapida_Cruz", 8, 19, 1, 1),
            ("Tumba", 9, 19, 1, 1),
            ("Sepulcro", 8, 20, 2, 1),
            ("Reja_Hierro", 15, 0, 2, 4),
            ("Barrotes", 6, 15, 2, 2),
        };
        DefinirSprites(ruta, piezas.Select(p => RectTiles(hoja, p.Item1, p.Item2, p.Item3, p.Item4, p.Item5)).ToList());

        // Farol apagado: la cabeza sin luz sobre el poste del farol encendido.
        var anok = Leer(Anokolisa + "Assets/Tiles.png");
        var farol = Nueva(16, 48);
        Pegar(farol, Tile(anok, 13, 5), T, T, 0, 0);
        Pegar(farol, Tile(anok, 12, 6, 1, 2), T, 32, 0, 16);
        GuardarFondo(farol, "Farol_Apagado");

        // Reja del panteón: tramo de 4 piezas de Segart (orilla, centro, centro, orilla).
        var reja = Nueva(64, 16);
        string[] partes = { "fenceL", "fenceM", "fenceM", "fenceR" };
        for (int i = 0; i < partes.Length; i++)
            Pegar(reja, Leer(Segart + partes[i] + ".png").GetPixels32(), T, T, i * T, 0);
        GuardarFondo(reja, "Reja_Panteon");
    }

    // ---------- Fondos ----------

    static void GenerarFondos()
    {
        var cielo0 = Leer(Segart + "parallax/parallax0.png");
        var nubes = Leer(Segart + "parallax/parallax1.png");
        var tumbas = Leer(Segart + "parallax/parallax2.png");
        var arbol = Leer(Segart + "tree.png");
        var anok = Leer(Anokolisa + "Assets/Tiles.png");

        // Cielo: franjas de color sin repetir la luna, una sola luna y nubes encima.
        int ancho = 768, alto = 352, extraArriba = 64;
        var cielo = Nueva(ancho, alto);
        for (int y = 0; y < alto; y++)
        {
            int ySrc = Mathf.Clamp(y - (alto - extraArriba - 256), 0, 255);
            var c = cielo0.GetPixel(250, ySrc);
            for (int x = 0; x < ancho; x++) cielo.SetPixel(x, y, c);
        }
        // Luna: los píxeles que no son del color de su franja.
        var luna = new Color32[256 * 256];
        for (int y = 0; y < 256; y++)
            for (int x = 0; x < 256; x++)
            {
                var c = (Color32)cielo0.GetPixel(x, y);
                var fondo = (Color32)cielo0.GetPixel(250, y);
                luna[y * 256 + x] = (c.r == fondo.r && c.g == fondo.g && c.b == fondo.b) ? new Color32(0, 0, 0, 0) : c;
            }
        Pegar(cielo, luna, 256, 256, 200, extraArriba, true);
        var nubesPx = nubes.GetPixels32();
        for (int i = 0; i < ancho / 256; i++) Pegar(cielo, nubesPx, 256, 256, i * 256, extraArriba, true);
        GuardarFondo(cielo, "Fondo_Cielo");

        // Panteón a lo lejos: tumbas en silueta y árboles secos, con tierra sólida abajo.
        var panteon = Nueva(512, 320);
        var tumbasPx = tumbas.GetPixels32();
        for (int i = 0; i < 2; i++) Pegar(panteon, tumbasPx, 256, 256, i * 256, 0, true);
        Rellenar(panteon, 0, 256, 512, 64, tumbas.GetPixel(10, 2));
        var arbolPx = arbol.GetPixels32();
        foreach (int x in new[] { 40, 210, 330, 450 }) Pegar(panteon, arbolPx, 48, 64, x, 174 - 62, true);
        GuardarFondo(panteon, "Fondo_Panteon");

        // Pueblo a lo lejos: casas del pack en fila, con su base sólida.
        GuardarFondo(Pueblo(anok, new[] { "torre", "gablete", "madera", "gablete", "torre", "madera" }), "Fondo_Pueblo");
        // Pueblo con la iglesia al fondo: la torre lleva la cruz como campanario.
        GuardarFondo(Pueblo(anok, new[] { "gablete", "iglesia", "madera", "gablete", "torre", "gablete" }), "Fondo_PuebloIglesia");
    }

    static Texture2D Pueblo(Texture2D anok, string[] orden)
    {
        int alto = 160 + 192, suelo = 160;
        var piezas = new List<(Color32[] px, int w, int h)>();
        foreach (var o in orden)
        {
            switch (o)
            {
                case "torre": piezas.Add((Tile(anok, 0, 1, 6, 9), 96, 144)); break;
                case "gablete": piezas.Add((Tile(anok, 6, 0, 6, 6), 96, 96)); break;
                case "madera": piezas.Add((Tile(anok, 17, 0, 5, 4), 80, 64)); break;
                case "iglesia": piezas.Add((Tile(anok, 0, 1, 6, 9), 96, 144)); break;
            }
        }
        int separacion = 24;
        int ancho = piezas.Sum(p => p.w + separacion);
        var tex = Nueva(ancho, alto);
        int x = separacion / 2;
        for (int i = 0; i < piezas.Count; i++)
        {
            var p = piezas[i];
            Pegar(tex, p.px, p.w, p.h, x, suelo - p.h, true);
            if (orden[i] == "iglesia")
            {
                // Cruz sencilla de 3 px de grosor sobre la torre.
                var cruz = new Color32(40, 40, 60, 255);
                Rellenar(tex, x + 46, suelo - p.h - 28, 4, 28, cruz);
                Rellenar(tex, x + 38, suelo - p.h - 20, 20, 4, cruz);
            }
            x += p.w + separacion;
        }
        Rellenar(tex, 0, suelo, ancho, alto - suelo, new Color32(20, 20, 30, 255));
        return tex;
    }

    static void Rellenar(Texture2D tex, int x, int yArriba, int w, int h, Color c)
    {
        var px = Enumerable.Repeat((Color32)c, w * h).ToArray();
        Pegar(tex, px, w, h, x, yArriba);
    }

    static void GuardarFondo(Texture2D tex, string nombre)
    {
        string ruta = Guardar(tex, nombre);
        var ti = (TextureImporter)AssetImporter.GetAtPath(ruta);
        Configurar(ti, false);
        ti.SaveAndReimport();
    }
}
