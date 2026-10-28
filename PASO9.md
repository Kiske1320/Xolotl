# Paso 9: una sección nueva por nivel (+50% de duración)

Lee el CLAUDE.md. Reemplacé dos scripts en Assets/_Xolotl/Scripts:
- Veladora.cs: opción de viento (windOnTime, windOffTime, windPhaseOffset). Con viento, la veladora se apaga y se enciende sola en ciclo y tiembla antes de apagarse. Sin viento (0 y 0) funciona como antes.
- OlvidadoController.cs: opción onlyMoveWhenPlayerMoves. Con ella, el olvidado solo avanza mientras Xólotl se mueve.
Léelos antes de empezar. No cambies los scripts salvo que haya un error de compilación; si es así, avísame antes.

Todo con comandos del Editor, sin editar archivos .unity ni .prefab a mano.

## Objetivo
Cada nivel gana una sección nueva de unas 45 unidades que REMEZCLA las mecánicas de una forma nueva (no más de lo mismo). No se agregan fotos (siguen 3 por nivel). Haz commit antes de empezar cada nivel, para poder volver si algo sale mal.

## Reglas (no romperlas)
- Salto: unas 2.8 de alto y 5 de largo. Escalones de máximo 2 y huecos de máximo 4 sin PapelPicado.
- Todo PapelPicado sobre un hueco lleva FallZone debajo (unas 3 unidades abajo del piso de esa zona).
- Al mover secciones, mueve TODO lo que esté en ese rango: suelo, paredes, pétalos, veladoras, olvidados, vecinos con sus PuntoA y PuntoB, textos, LevelExit y FallZones. Al terminar, verifica que ningún objeto haya quedado encimado o flotando.
- Textos de ayuda con el mismo estilo que los existentes (TextMeshPro 3D, crema #F4E7C5), nunca en (0, 0, 0).
- Actualiza los límites de CameraFollow de cada nivel (incluye las zonas bajas nuevas).

## Nivel 1 (01_Panteon): "La cripta"
1. Mueve todo lo que esté de x = 140 en adelante (la sección de salida) 45 unidades a la derecha. La reja queda cerca de x = 202 y el nivel termina cerca de x = 205.
2. Entre x = 140 y x = 185, crea una cripta subterránea:
   - Escalones de bajada desde el suelo (y = 0) hasta un piso de cripta en y = -8, techo de cripta en y = -3, y escalones de subida al final para volver a y = 0.
   - Un pasillo oscuro con tres Veladoras con viento: windOnTime 3, windOffTime 2, con windPhaseOffset de 0, 1.2 y 2.4 (así siempre hay alguna encendida). startLit = true.
   - Un PapelPicado de 3x0.5 a la mitad, sobre un pozo de 4 en el piso de la cripta (FallZone en y = -11), a menos de 2 unidades de una de las veladoras con viento: solo se puede cruzar cuando esa veladora está encendida (o con la llamarada).
   - Un Vecino caminando por el pasillo de la cripta (de la primera a la tercera veladora).
   - Dos olvidados lentos dentro de la cripta.
   - 3 pétalos.
   - Texto al bajar: "El viento de la cripta apaga las veladoras. Espera su luz".
   - Una Veladora encendida SIN viento al salir de la cripta, como checkpoint.

## Nivel 2 (02_Calles): "El mercado"
1. Mueve todo lo que esté de x = 132 en adelante (la sección de salida) 45 unidades a la derecha. La puerta de la iglesia queda cerca de x = 192 y el nivel termina cerca de x = 195.
2. Entre x = 132 y x = 177, crea un mercado:
   - Puestos: 4 plataformas de 4x0.5 a y = 2.5 (techos de los puestos), y entre ellos tiras de PapelPicado a y = 5.
   - Dos huecos de 6 en el suelo (zanjas) cubiertos con PapelPicado, con FallZone debajo.
   - Cuatro Vecinos con rutas que se cruzan: dos caminando a la derecha y dos a la izquierda, con velocidades distintas (0.9, 1.2, 1.5 y 1.8) y startDelay escalonados, de modo que la luz que sostiene los puentes cambia de lugar todo el tiempo.
   - Dos olvidados (uno lento y uno veloz) sobre los puestos.
   - 3 pétalos.
   - Texto al entrar: "En el mercado, los vivos van y vienen. Elige bien a quién seguir".
   - Una Veladora encendida al final del mercado, como checkpoint.

## Nivel 3 (03_Iglesia): "El sótano"
1. Mueve todo lo que esté de x = 100 en adelante (la persecución completa: veladora de inicio, texto, calle, huecos, veladoras, LevelExit y paredes) 45 unidades a la derecha.
2. Ajusta MareaOlvidados: triggerX + 45 (queda cerca de 149) y stopX + 45 (queda cerca de 205). Su posición inicial se queda en x = -20.
3. Entre x = 100 y x = 145, crea el sótano de la iglesia:
   - Bajada desde el nivel del suelo a un piso de sótano en y = -6 y subida al final a y = 0.
   - Cuatro olvidados con onlyMoveWhenPlayerMoves = true, moveSpeed 2.5 y detectionRange 14, repartidos en el sótano.
   - Columnas (bloques de 1x2) que obligan a saltar.
   - Dos Veladoras encendidas sin viento como zonas de descanso, una a la mitad y otra al final.
   - 3 pétalos.
   - Texto al bajar: "En el sótano, los olvidados solo se mueven cuando tú te mueves".

## Al final
Actualiza el CLAUDE.md: en las reglas de diseño agrega la opción de viento de las veladoras, los olvidados del sótano (solo avanzan mientras Xólotl se mueve) y que la marea empieza en x = -20; y corrige la regla de Layers, porque el daño de los olvidados va por distancia, no por triggers.

Guarda todo, dime si hay errores en la Console, y para cada nivel dame: la nueva longitud total, la posición final de la salida y la lista de checkpoints (veladoras) en orden.
