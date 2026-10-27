# Paso 5: fotos de recuerdo, sección de reto e interfaz (nivel 1 más largo)

Lee el CLAUDE.md. Copié dos scripts nuevos en Assets/_Xolotl/Scripts: MemoryPhoto.cs y UIManager.cs. También reemplacé Petal.cs, GameManager.cs y Veladora.cs (los pétalos reaparecen al reaparecer Xólotl, se reaparece con la luz llena y las veladoras encendidas recargan luz). Léelos antes de empezar. No cambies los scripts salvo que haya un error de compilación; si es así, avísame antes.

Todo con comandos del Editor, sin editar archivos .unity ni .prefab a mano.

Objetivo de diseño: el nivel 1 se siente corto. No hay que alargarlo con suelo vacío, sino darle más que hacer: caminos opcionales hacia arriba con las fotos y una sección de reto que combine todo lo aprendido.

## Parte A: revisión rápida de prefabs
1. Revisa que todo compile.
2. En el prefab GameManager, respawnLightFraction = 1. En el prefab Xolotl, drainPerSecond de XolotlLight = 0.2. Si ya tienen esos valores, no los toques.

## Parte B: prefab MemoryPhoto
1. Sprite placeholder Foto de 12x12 en Assets/_Xolotl/Art/Placeholder (marco crema #F4E7C5 con centro sepia), 16 PPU, Point, sin compresión.
2. Prefab MemoryPhoto: SpriteRenderer con ese sprite, CircleCollider2D en Trigger y el script MemoryPhoto. Agrégale un hijo Light con Light 2D Point blanca cálida #FFE9B0, intensidad 0.6 y radio exterior 1.2, para que brille un poco en la oscuridad y se pueda encontrar (esta luz NO usa LightSource, es solo visual).

## Parte C: interfaz en el prefab UI_Canvas
1. Arriba a la izquierda: un ícono de llama (sprite placeholder naranja de 16x16) y a su lado una barra: Image de fondo oscura y encima una Image LightFill tipo Filled, Fill Method Horizontal, Fill Origin Left, de unos 300x24 en la referencia de 1920x1080.
2. Arriba a la derecha: un texto TextMeshPro PhotoText, color crema #F4E7C5, alineado a la derecha.
3. Agrega UIManager al UI_Canvas y conecta lightFill y photoText (xolotlLight lo busca solo). Presiona el menú de contexto Reset del componente UIManager para que se cree el degradado de color, o configura el Gradient de rojo apagado a naranja a amarillo claro.
4. La barra y el texto deben quedar DEBAJO del panel de derrota y del FadePanel en el orden de hijos del Canvas.
5. Aplica los cambios al prefab para que también aparezcan en 02_Calles.

## Parte D: nivel 1 más largo (01_Panteon)

**1. Haz espacio para la sección de reto:** mueve todo lo que esté de x = 95 en adelante (sección 5: pétalos, olvidado, reja con LevelExit, suelo y pared invisible final) 45 unidades a la derecha. La reja queda en x = 157 y el nivel termina en x = 160.

**2. Nueva sección "El tramo olvidado" (x = 95 a 140):**
- Suelo continuo de x = 95 a 115, un hueco de x = 115 a 121 y suelo de nuevo de x = 121 a 140.
- Dos PapelPicado cubriendo el hueco (centrados en x = 116.5 y x = 119.5, con la parte de arriba en y = 0). SIN vecino: se cruza solo con la llamarada.
- FallZone debajo del hueco (de x = 115 a 121, y = -3).
- Texto en x = 108, y = 3: "Sin los vivos cerca, tu llamarada también despierta al papel picado".
- Dos olvidados: uno lento en (102, 3) y uno veloz (moveSpeed 3, detectionRange 12) en (128, 3).
- Solo 3 pétalos en toda la sección.
- Veladora encendida en x = 137 como checkpoint.

**3. Tres caminos opcionales hacia arriba, cada uno con una foto** (plataformas de 3x0.5 en layer Ground; cada escalón a no más de 2.5 de alto y 4 de separación del anterior):
- Foto 1 (sección de las tumbas): escalones en (26, 3.5) y (30, 6); MemoryPhoto en (33, 8.2) y un pétalo de premio en (30, 7).
- Foto 2 (sección del primer olvidado): desde la plataforma de la veladora apagada en (63, 2), escalones en (67, 4.5) y (71, 7); MemoryPhoto en (74, 8.5). Un Olvidado lento flotando en (72, 10) la cuida.
- Foto 3 (sección del puente): un PapelPicado de 3x0.5 en (86, 3) y otro en (90, 5.5), que solo se pisan con la llamarada o cuando pasa el vecino; MemoryPhoto en (93, 7).

**4. Cámara:** actualiza los límites de CameraFollow de 01_Panteon: el nivel va de x = -1 a x = 160 y ahora hay cosas hasta y = 10; que nunca se vea nada debajo de y = -2.

## Parte E
Guarda todo y dime si hay errores en la Console.
