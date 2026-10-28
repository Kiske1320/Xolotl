# Paso 6: nivel 2 en gris, "Calles y azoteas" (02_Calles)

Lee el CLAUDE.md. No hay scripts nuevos: este nivel se arma solo con los prefabs que ya existen. No cambies los scripts salvo que haya un error de compilación; si es así, avísame antes.

Todo con comandos del Editor, sin editar archivos .unity ni .prefab a mano.

## Idea del nivel
El nivel 1 enseñó cada mecánica por separado. El nivel 2 las combina con el mundo humano en el centro:
- Encender una veladora apagada con la llamarada deja un puente de papel picado sólido para siempre (la veladora queda iluminándolo).
- Una procesión de vecinos con velas cruza un canal: el jugador avanza dentro de su luz en movimiento.
Duración meta: 3 a 4 minutos buscando las fotos.

## Reglas físicas (no romperlas)
- Salto: unas 2.8 de alto y 5 de largo. Ningún escalón de más de 2 de alto ni hueco de más de 4 sin PapelPicado.
- Casas y techos: bloques grises con Box Collider 2D en layer Ground y SpriteRenderer Tiled (sprite Square). Usa un gris un poco más claro que el del panteón para distinguir el pueblo.
- La luz de una Veladora tiene radio 2.5: una veladora que deba mantener sólido un puente tiene que estar a menos de 2 unidades de su orilla más cercana.
- Todo PapelPicado sobre un hueco lleva un FallZone debajo.

## Base
La escena 02_Calles ya existe con los objetos base. Borra el suelo de 30 unidades y el texto "Nivel 2: próximamente". Revisa que Xolotl, MainCamera, GameManager (con levelStart) y UI_Canvas estén conectados. Global Light 2D igual que en 01_Panteon.

## Secciones (de izquierda a derecha, unas 150 unidades)

**1. La calle (x = 0 a 30)**
- Xolotl y LevelStart en x = 2. Veladora encendida en x = 4.
- Camino de 4 pétalos.
- Un Vecino caminando de x = 10 a x = 26 por la calle.
- Texto (TextMeshPro 3D, color crema #F4E7C5) en x = 8, y = 3: "Sigue a los vivos: su luz te protege".
- Un Olvidado lento en (22, 4) que se congela cuando pasa el vecino.

**2. Las azoteas (x = 30 a 65)**
- Tres casas con techos a diferentes alturas (entre 2 y 4), con cajas o escalones para subir.
- Entre la segunda y la tercera casa, un hueco de 6 cubierto por dos PapelPicado a la altura de los techos.
- Una Veladora APAGADA en el techo de la segunda casa, junto a la orilla del puente (a menos de 2 unidades del primer PapelPicado). Al encenderla con la llamarada, el puente queda sólido y es checkpoint.
- Texto sobre el techo de la segunda casa: "Enciende la veladora y su luz sostendrá el papel picado".
- FallZone en el hueco, debajo de los techos.
- Foto 1: arriba de la tercera casa, en una plataforma a la que se llega con dos escalones extra.

**3. La procesión (x = 65 a 100)**
- Suelo hasta x = 72, un canal (hueco) de x = 72 a x = 84 cubierto por cuatro PapelPicado de 3x0.5 a nivel del suelo, y suelo otra vez desde x = 84.
- FallZone a lo largo del canal.
- Tres Vecinos en fila, caminando de x = 66 a x = 92 y de regreso, con startDelay de 0, 1.5 y 3 segundos para que avancen escalonados.
- Texto en x = 66, y = 3: "La procesión ilumina el camino de los muertos".
- Dos olvidados lentos sobre el canal, en (76, 4) y (80, 4).
- Foto 2: sobre un PapelPicado elevado en (86, 4.5), al que se llega saltando desde la procesión o con la llamarada.
- Veladora encendida en x = 95.

**4. El callejón (x = 100 a 132)**
- Un pasillo con techo bajo (bloque de techo a y = 4.5 sobre todo el callejón), oscuro, con pocos pétalos (3).
- Dos olvidados: uno lento en (110, 2) y uno veloz (moveSpeed 3) en (124, 2).
- Una Veladora APAGADA a la mitad (x = 116): es el checkpoint del callejón si la enciendes.
- Foto 3: por encima del techo del callejón, a la que se llega por una abertura en el techo en x = 120 con escalones.

**5. La salida (x = 132 a 150)**
- 2 pétalos, una Veladora encendida en x = 136.
- Al final, en x = 147, una puerta de iglesia (bloque de 2x3 gris claro) con BoxCollider2D en Trigger y LevelExit con nextSceneName = "03_Iglesia".
- Paredes invisibles en x = -1 y x = 150.

**Cámara:** límites de CameraFollow para este nivel (de x = -1 a x = 150, hasta y = 10, nada debajo de y = -2).

## Escena 03_Iglesia (solo base)
Crea Assets/_Xolotl/Scenes/03_Iglesia con Global Light 2D, Xolotl, MainCamera, GameManager, UI_Canvas, un suelo de 30 unidades y un texto temporal "Nivel 3: próximamente". Agrégala a Build Profiles después de 02_Calles.

Guarda todo, dime si hay errores en la Console y dame una lista de las posiciones finales de las 3 fotos y de las veladoras apagadas.
