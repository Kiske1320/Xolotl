# Paso 12: arte real de los personajes

Lee el CLAUDE.md. Jurgen descargó los packs de arte en Assets/_Xolotl/Art/Terceros/, cada uno en su carpeta:
- PixelDogs (Benvictus): perros de lado, 64x48 px por cuadro, 12 colores, animaciones idle, walk, run, sit, bark.
- Skeleton_Camacebra: esqueleto 16x16 con idle, walk y jump.
- HalloweenGraphics_finalbossblues: incluye un fantasma (Ghost Sprite) y calaveras de Día de Muertos.

No cambies la lógica de los scripts. Puedes agregar a XolotlController SOLO lo necesario para el Animator si falta algo; avísame antes. Todo con comandos del Editor, sin editar .unity, .prefab ni .asset a mano. Haz commit antes de empezar.

## Reglas de importación
- Filter Mode Point, Compression None, sin mipmaps.
- Corta las hojas con el Sprite Editor (Grid By Cell Size) según el tamaño real de cada cuadro. Pivote abajo al centro (Bottom Center) para personajes que pisan el suelo; centro para los que flotan.
- Ajusta el Pixels Per Unit de cada hoja para que el tamaño en el juego sea el indicado abajo. Nunca escales con el Transform.
- Todos los SpriteRenderer de personajes usan el material Sprite-Lit-Default para que reaccionen a las luces 2D.

## Parte A: Xólotl (prefab Xolotl)
1. De PixelDogs, elige el color más oscuro (negro o gris carbón): un xoloitzcuintle es oscuro y sin pelo. Dime cuál elegiste y por qué.
2. Revisa cuánto mide el perro dentro del cuadro de 64x48 (sin el espacio vacío) y ajusta el PPU para que el cuerpo mida alrededor de 1.3 unidades de largo.
3. Pon el sprite en un hijo Visual de Xolotl (no en la raíz), conectado al campo spriteRenderer de XolotlController, y alinea las patas con el GroundCheck.
4. NO cambies el tamaño del collider de juego más allá de lo indispensable: el diseño de niveles depende de él. Si el perro se ve muy distinto al collider, ajusta el collider como máximo a 1.2 x 0.9 y dime los valores finales.
5. Revisa hacia dónde mira el sprite original y configura spriteFacesLeft.
6. Crea un Animator Controller XolotlAnimator con estados Idle (idle de pie), Run (run), Jump (un cuadro de run sostenido) y Flare (bark), y estos parámetros, que XolotlController ya envía: Speed (Float), Grounded (Bool), VerticalSpeed (Float) y Flare (Trigger).
   - Idle a Run cuando Speed > 0.5; Run a Idle cuando Speed < 0.5. Sin Exit Time y sin transición (0).
   - Cualquier estado a Jump cuando Grounded = false; Jump a Idle cuando Grounded = true.
   - Any State a Flare con el trigger Flare; Flare regresa a Idle al terminar (con Exit Time). Flare sin Loop Time.
7. Squash al aterrizar no hace falta en este paso.

## Parte B: el alma de Aurora
1. Agrega un hijo AlmaAurora a Xolotl con el idle del esqueleto de Camacebra, sentado sobre el lomo del perro (ajusta la posición), color blanco con alpha 0.6, una Light 2D Point blanca muy tenue (intensidad 0.3, radio 1) solo visual (sin LightSource), y que se voltee junto con el perro.

## Parte C: olvidados (prefab Olvidado)
1. Usa el fantasma de HalloweenGraphics_finalbossblues. Corta su animación y crea un Animator simple con su ciclo flotando.
2. Tamaño: alrededor de 1.2 unidades de alto. Pivote al centro.
3. Revisa hacia dónde mira y configura spriteFacesLeft.
4. Dale un tono azulado frío (color #B8C8FF) para que contraste con el naranja del fuego.
5. Cambia también los sprites de olvidado de la MareaOlvidados en 03_Iglesia.

## Parte D: escena final y título
1. En 00_Titulo y 04_Ofrenda, cambia el Xólotl de adorno por el sprite del perro elegido, con su animación idle (sit si existe).
2. En 04_Ofrenda, cambia el alma de la abuela por el esqueleto de Camacebra con alpha 0.6.
3. En la ofrenda de 04_Ofrenda, agrega como decoración las calaveras de Día de Muertos de HalloweenGraphics_finalbossblues.

## Al final
Dale Play a 01_Panteon y revisa que Xólotl camine, salte, lance la llamarada con su animación, que el GroundCheck siga funcionando y que no se atore. Guarda, dime si hay errores y haz commit con el mensaje "PASO12: arte de personajes".
