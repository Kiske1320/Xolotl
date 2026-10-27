# Paso 4: prefabs y nivel 1 completo en gris (el panteón)

Lee el CLAUDE.md. Copié dos scripts nuevos en Assets/_Xolotl/Scripts: ScreenFader.cs y LevelExit.cs. Léelos antes de empezar. No cambies los scripts existentes salvo que haya un error de compilación; si es así, avísame antes.

Todo con comandos del Editor, sin editar archivos .unity ni .prefab a mano.

## Parte A: prefabs (desde la escena 00_Prueba)
Crea prefabs en Assets/_Xolotl/Prefabs a partir de los objetos que ya funcionan en 00_Prueba, con todas sus referencias:
1. Xolotl (con GroundCheck y Light).
2. MainCamera (con Pixel Perfect Camera y CameraFollow apuntando a Xolotl; si la referencia al jugador no se puede guardar en el prefab, haz que CameraFollow la conecte en la escena).
3. GameManager.
4. UI_Canvas: el Canvas con el panel de derrota. Agrégale además un panel FadePanel negro a pantalla completa, con CanvasGroup y el script ScreenFader, que quede encima de todo (último hijo del Canvas).
5. Veladora (encendida por defecto), Petal, Olvidado, Vecino (HumanoConVela) y PapelPicado (SpectralPlatform de 3x0.5).
Después de crear los prefabs, reemplaza los objetos de 00_Prueba por instancias de los prefabs, dale Play mentalmente revisando que no queden referencias vacías, y guarda.

## Parte B: escena 01_Panteon
Crea la escena Assets/_Xolotl/Scenes/01_Panteon con la plantilla 2D de URP (o duplicando 00_Prueba y borrando el nivel). Debe tener: Global Light 2D (intensidad 0.08, color #1B264F), y una instancia de Xolotl, MainCamera, GameManager y UI_Canvas con sus referencias conectadas (incluye levelStart en GameManager).

Física de referencia: el salto alcanza unas 2.8 unidades de alto y unas 5 de largo. Ningún obstáculo debe medir más de 2 de alto ni ningún hueco más de 4 de ancho. Todo el suelo y las tumbas en layer Ground con Box Collider 2D y SpriteRenderer Tiled (sprite Square gris).

Arma el nivel de izquierda a derecha, unas 115 unidades de largo, en 5 secciones:

**Sección 1, la llegada (x = 0 a 20):**
- Xolotl empieza en x = 2. LevelStart ahí mismo.
- Veladora encendida en x = 5 (zona segura).
- Camino de pétalos: 5 pétalos en línea, uno cada 3 unidades, desde x = 8.
- Texto de ayuda en el mundo (TextMeshPro 3D, color crema #F4E7C5, tamaño legible, en x = 4, y = 3): "A / D: moverse    Espacio: saltar".
- Texto en x = 12, y = 3: "Los pétalos que ponen los vivos alimentan tu fuego".

**Sección 2, las tumbas (x = 20 a 45):**
- Cuatro tumbas (bloques) de alturas entre 1 y 2 para saltar, separadas por tramos oscuros.
- Solo 3 pétalos, para que el jugador sienta que la luz baja.
- Veladora encendida en x = 43 como checkpoint, con texto encima en y = 3: "Toca una veladora encendida para guardar tu camino".

**Sección 3, el primer olvidado (x = 45 a 70):**
- Texto en x = 47, y = 3: "Shift: llamarada. Gasta luz, aleja a los olvidados y enciende veladoras".
- Un Olvidado (lento) en (58, 3).
- Una plataforma de 4x0.5 en (63, 2) con una Veladora APAGADA encima: se enciende con la llamarada y es el checkpoint de esta sección.
- 3 pétalos repartidos.

**Sección 4, el puente de los vivos (x = 70 a 95):**
- El suelo termina en x = 78 y vuelve a empezar en x = 84 (hueco de 6).
- Dos PapelPicado de 3x0.5 cubriendo el hueco (centrados en x = 79.5 y x = 82.5, con la parte de arriba en y = 0).
- FallZone (Vacio) debajo del hueco, en y = -3, de x = 78 a 84.
- Un Vecino con vela caminando de x = 72 a x = 88, con sus PuntoA y PuntoB como objetos sueltos de la escena (no hijos).
- Texto en x = 72, y = 3: "Los puentes de papel picado solo existen con la luz de los vivos".
- Veladora encendida en x = 90.

**Sección 5, la salida (x = 95 a 115):**
- 2 pétalos y un Olvidado más en (102, 3).
- Al final, en x = 112, una reja (bloque alto de 1x3 gris claro, sin collider sólido) con un BoxCollider2D en Trigger y el script LevelExit (nextSceneName = "02_Calles").
- Suelo hasta x = 115 y una pared invisible al final (Box Collider 2D en layer Ground) para no salirse. Pon otra pared invisible al inicio, en x = -1.

**Cámara:** ajusta los límites de CameraFollow para este nivel (de x = -1 a x = 115, sin mostrar nada debajo de y = -2).

## Parte C: escena 02_Calles (solo base)
Crea Assets/_Xolotl/Scenes/02_Calles con Global Light 2D, Xolotl, MainCamera, GameManager, UI_Canvas y un suelo de 30 unidades, más un texto temporal en el mundo: "Nivel 2: próximamente". La llenamos mañana.

## Parte D: Build Profiles
Agrega a la lista de escenas de Build Profiles, en este orden: 01_Panteon y 02_Calles (00_Prueba no va).

Guarda todo y dime si hay errores en la Console.
