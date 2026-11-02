# Paso 7: nivel 3 en gris, "La iglesia y la persecución" (03_Iglesia)

Lee el CLAUDE.md. Copié un script nuevo en Assets/_Xolotl/Scripts: ChaseWall.cs (la marea de olvidados de la persecución final). Léelo antes de empezar. No cambies los scripts salvo que haya un error de compilación; si es así, avísame antes.

Todo con comandos del Editor, sin editar archivos .unity ni .prefab a mano.

## Idea del nivel
Es el clímax. Primero la iglesia, donde las veladoras de los vivos son islas de luz en la oscuridad; luego el campanario, una subida vertical; y al final una persecución: una marea de olvidados que la luz ya no detiene, y solo queda correr hasta la casa de la familia.
Duración meta: 3 a 4 minutos.

## Reglas físicas (no romperlas)
- Salto: unas 2.8 de alto y 5 de largo. Ningún escalón de más de 2 de alto ni hueco de más de 4 sin PapelPicado.
- Bloques grises con Box Collider 2D en layer Ground y SpriteRenderer Tiled (sprite Square). Usa un gris azulado para la iglesia.
- Una Veladora que deba sostener un PapelPicado debe estar a menos de 2 unidades de su orilla.
- Todo PapelPicado sobre un hueco lleva un FallZone debajo.

## Base
La escena 03_Iglesia ya existe. Borra el suelo de 30 y el texto "Nivel 3: próximamente". Revisa que Xolotl, MainCamera, GameManager (con levelStart) y UI_Canvas estén conectados. Global Light 2D igual que en los otros niveles.

## Secciones (de izquierda a derecha, unas 170 unidades)

**1. El atrio (x = 0 a 35)**
- Xolotl y LevelStart en x = 2. Veladora encendida en x = 4. 4 pétalos.
- Un Vecino caminando de x = 12 a x = 30.
- Dos olvidados: uno lento en (18, 4) y uno veloz (moveSpeed 3) en (30, 5).
- Texto (TextMeshPro 3D, crema #F4E7C5) en x = 8, y = 3: "Los olvidados se juntan donde nadie reza por ellos".
- Foto 1: sobre la barda del atrio, en una plataforma alta a la que se llega con dos escalones (cerca de x = 25, y = 6).

**2. La nave de la iglesia (x = 35 a 85)**
- Paredes y un techo alto (bloque de techo a y = 12 de x = 35 a 85) para que se sienta interior.
- Bancas: 5 bloques bajos de 3x0.8 en el piso.
- Tres "candelabros": plataformas de 3x0.5 a y = 4, 6 y 4 (cerca de x = 48, 60 y 72), cada una con una Veladora ENCENDIDA encima.
- Entre los candelabros, PapelPicado a la misma altura, colocados a menos de 2 unidades de las veladoras para que queden sólidos: la ruta de arriba es segura, la de abajo está llena de olvidados.
- Tres olvidados lentos en el piso de la nave (x = 50, 62, 75, y = 2).
- Un Vecino rezando junto al altar (x = 80): usa el prefab Vecino con PuntoA y PuntoB en la misma posición para que se quede quieto.
- Foto 2: arriba del candelabro central (x = 60), en un PapelPicado a y = 9 que solo se pisa con la llamarada.

**3. El campanario (x = 85 a 100)**
- Una subida vertical en zigzag de plataformas de 3x0.5 hasta y = 14, con 2 pétalos en el camino.
- Arriba, una Veladora APAGADA (checkpoint si la enciendes) y la Foto 3.
- Por la derecha se baja de nuevo al nivel del suelo (caída libre segura o escalones).

**4. La persecución (x = 100 a 170)**
- Veladora ENCENDIDA en x = 101 (checkpoint justo antes de la persecución).
- Texto en x = 103, y = 3: "¡Corre! La marea de olvidados viene por ella".
- Calle de x = 100 a 170 con saltos pequeños (cajas de 1 de alto), una línea continua de pétalos cada 4 unidades (para que el jugador no tenga que detenerse) y dos huecos de 6 cubiertos con PapelPicado, cada uno con una Veladora ENCENDIDA junto a la orilla para que ya estén sólidos (aquí no hay tiempo de usar la llamarada). FallZone debajo de cada hueco.
- ChaseWall: un GameObject MareaOlvidados en (92, 6) con el script ChaseWall. Hijos: un rectángulo oscuro de 4x20 color #0B0F1E con alpha 0.9 (SpriteRenderer, Order in Layer arriba del nivel) y 5 sprites de Olvidado repartidos sobre su borde derecho (solo SpriteRenderer, sin scripts). Valores: triggerX = 104, startDelay = 0.8, speed = 4.5, maxLead = 12, stopX = 160, catchOffset = 2.
- Al final, en x = 166, la puerta de la casa de la familia (bloque de 2x3 color ocre) con BoxCollider2D en Trigger y LevelExit con nextSceneName = "04_Ofrenda".
- Paredes invisibles en x = -1 y x = 170.

**Cámara:** límites de CameraFollow de x = -1 a x = 170 y hasta y = 15 (el campanario es alto); nada debajo de y = -2.

Guarda todo, dime si hay errores en la Console y dame las posiciones finales de las 3 fotos.
