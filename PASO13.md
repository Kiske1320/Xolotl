# Paso 13: arte real de los escenarios

Lee el CLAUDE.md. Copié un script nuevo: ParallaxLayer.cs. Jurgen descargó los packs en Assets/_Xolotl/Art/Terceros/:
- NES_Village_Anokolisa (Free - Classic NES Legacy Pack - Village): tiles 16x16 de pueblo nocturno, iglesia, panteón, interiores, faroles y decoración.
- Graveyard_Segart (2D 16px Graveyard Tileset): tiles 16x16 de cementerio y capas de parallax.

Regla de oro: este paso SOLO cambia lo que se ve. No muevas, no cambies de tamaño ni borres ningún collider, plataforma, veladora, pétalo, olvidado, vecino, texto, LevelExit, FallZone, CameraZone ni ChaseWall. Si algo no se puede hacer sin cambiar la jugabilidad, avísame antes. Todo con comandos del Editor, sin editar .unity, .prefab ni .asset a mano. Haz commit antes de empezar cada nivel.

## Parte A: importación
1. Importa las hojas de ambos packs con 16 PPU, Filter Mode Point, Compression None y sin mipmaps.
2. Corta los tilesets con Grid By Cell Size 16x16 donde haga falta.
3. Revisa qué trae cada pack y hazme un resumen corto (qué sirve para piso, muros, techos, decoración y fondos).

## Parte B: sprites de 9 partes para los bloques
Los bloques del nivel son SpriteRenderer en modo Tiled. Para que se vean como tiles y no como un cuadro repetido, crea sprites de 9 partes (9-slice):
1. Para cada material de abajo, busca en el tileset un bloque de 3x3 tiles (48x48 px) con orilla arriba, abajo y a los lados. Crea un sprite con ese rectángulo, bordes de 16 px en los 4 lados y Mesh Type Full Rect.
2. Si el tileset no trae un 3x3 completo, arma una textura nueva de 48x48 combinando tiles (con un script de editor que copie los píxeles) en Assets/_Xolotl/Art/Tiles/ y córtala igual.
3. Materiales:
   - Tierra_Panteon (de Segart o del panteón de Anokolisa): piso, tumbas y plataformas del nivel 1.
   - Piedra_Cripta: la versión más oscura disponible, para la cripta del nivel 1.
   - Piedra_Pueblo y Techo_Pueblo (de Anokolisa): calles, casas, techos y puestos del nivel 2.
   - Piedra_Iglesia (de Anokolisa): piso, muros, bancas, candelabros y campanario del nivel 3.

## Parte C: aplicar a los niveles
1. En 01_Panteon, 02_Calles y 03_Iglesia, cambia el sprite de cada bloque visible del layer Ground por el material que le toca, en modo Tiled y Tile Mode Continuous, color blanco, sin cambiar su tamaño ni su collider.
2. Las paredes invisibles se quedan invisibles.
3. No cambies los PapelPicado ni los prefabs de veladora y pétalo en este paso.

## Parte D: decoración (sin colliders)
1. Crea las Sorting Layers en este orden: Background, Decor, Default, Foreground. Todo lo de juego se queda en Default.
2. En cada nivel, agrega decoración del pack en la Sorting Layer Decor, sin colliders, detrás del juego:
   - 01_Panteon: lápidas, cruces, rejas, árboles secos y faroles apagados.
   - 02_Calles: fachadas de casas, ventanas, barriles, faroles y letreros.
   - 03_Iglesia: fachada de la iglesia en el atrio; dentro, ventanales, columnas y la cruz del altar.
3. Que la decoración no tape ningún texto de ayuda, pétalo, veladora, olvidado ni la entrada de un camino.
4. Densidad moderada: que se vea vivo, pero que el jugador siempre distinga qué es piso y qué es fondo.

## Parte E: fondos con parallax
1. En cada nivel, crea 2 o 3 capas de fondo en la Sorting Layer Background, cada una con ParallaxLayer (cameraTransform = MainCamera):
   - Lejana (factorX 0.9): cielo nocturno con luna o la capa más lejana de Segart.
   - Media (factorX 0.7): siluetas (árboles en el panteón, casas en las calles, el pueblo y la iglesia a lo lejos en el nivel 3).
   - Cercana opcional (factorX 0.5).
2. Los fondos usan el material Sprite-Unlit-Default con un tinte azul oscuro (entre #1B264F y #2E3B6B), para que se distingan como siluetas en la oscuridad sin competir con el juego. La decoración de la Parte D sí usa Sprite-Lit-Default (reacciona a las luces).
3. Cada capa en modo Tiled, lo bastante ancha para cubrir todo el nivel con su factor, sin que se vea un borde al llegar al final.
4. La cripta y el sótano no llevan parallax: solo un fondo de muro oscuro fijo (Sprite-Lit-Default) dentro de la zona.

## Parte F: título y ofrenda
1. 00_Titulo: el mismo fondo con parallax del panteón (estático) y el suelo con Tierra_Panteon.
2. 04_Ofrenda: muro de fondo de una casa (Anokolisa) detrás de la ofrenda y piso de Piedra_Pueblo.

## Al final
1. Créditos en 00_Titulo: agrega a Anokolisa (Classic NES Legacy Pack - Village) y Segart (2D 16px Graveyard Tileset) si no están.
2. Dale Play en los tres niveles y revisa que todo se juegue igual que antes.
3. Haz un build Web nuevo en Builds/Web.
4. Dime si hay errores y haz commit con el mensaje "PASO13: arte de escenarios".
