# Xólotl: Antes del Amanecer — Spooky Game Jam (Hybridge) · Kiske Studio

## Contexto
- Nombre completo del juego: **Xólotl: Antes del Amanecer** (ya existe un "Xolotl" en Steam, de Anchor Games, 2022). La abuela se llama Aurora.
- Juego 2D de plataformas en pixel art, Unity 6.5 (6000.5.4f1), URP 2D, build **solo Web (WebGL)**.
- Jam del 26 al 30 de octubre de 2026. **Entrega real: jueves 29 antes de las 11:00 p. m.**
- Temática: "visitantes del más allá". Xólotl, el perro de fuego que guía a los muertos, cruza al pueblo de los vivos la noche del 1 de noviembre para llevar el alma de la abuela Aurora a su ofrenda.
- Mecánica central: una sola variable, el radio de luz de Xólotl. Se consume con el tiempo, se recarga con pétalos de cempasúchil y se gasta en la llamarada.
- Mecánicas obligatorias de la jam: protagonista sobrenatural, interacción con el mundo humano, gestión de recursos, victoria y derrota, jugabilidad interactiva.

## Convenciones del proyecto
- Scripts en `Assets/_Xolotl/Scripts`. Una clase por archivo, mismo nombre que la clase.
- Comentarios y textos del juego en español.
- Pixel art: 16 Pixels Per Unit, Filter Mode Point, Compression None. Nunca escalar sprites en el Transform.
- Input: `InputActionReference` (acciones Move, Jump y Flare de InputSystem_Actions). No usar PlayerInput con Send Messages.
- Unity 6: usar `rb.linearVelocity`, no `rb.velocity`.
- Todas las luces de juego heredan de `LightSource` y se consultan con `LightRegistry.IsLit(punto)`.
- Layers: Player, Enemy, Ground, Spectral. Player vs Enemy desactivado en la matriz de colisión; el daño de los olvidados va por distancia (la matriz también bloquea triggers).

## Scripts existentes
LightRegistry, LightSource, XolotlLight, XolotlController, Petal, Veladora, GameManager,
OlvidadoController, SpectralPlatform, HumanoConVela, CameraFollow, FallZone,
ScreenFader, LevelExit, MemoryPhoto, UIManager, ChaseWall, PauseMenu, TitleMenu, FinalScene, IntroScreen,
SeguirVolteo (solo visual: el alma de Aurora copia el flipX del perro).

## Prefabs (Assets/_Xolotl/Prefabs)
Xolotl, MainCamera, GameManager, UI_Canvas, Veladora, Petal, Olvidado, Vecino, PapelPicado, MemoryPhoto.
Referencias a objetos de escena (target de CameraFollow, GameManager, PuntoA/B del vecino) se conectan en cada escena.

## Escenas
Build: 00_Titulo, 00_Intro, 01_Panteon, 02_Calles, 03_Iglesia, 04_Ofrenda. 00_Prueba es solo de pruebas y no va en el build. Plataforma activa: Web.

## Pendiente de limpieza antes de entregar
- Borrar el Debug.Log temporal de la llamarada en XolotlController.

## Reglas de diseño ya decididas
- Olvidados: se congelan bajo cualquier luz, pero tocarlos sigue quitando luz; la llamarada los empuja. Sin collider: daño por distancia (Player vs Enemy está desactivado en la matriz, y eso también bloquea triggers).
- Puentes de papel picado: la luz normal de Xólotl no basta; necesitan vela de vecino, veladora o llamarada. Siguen sólidos 0.8 s al perder la luz.
- Caer al vacío (FallZone) = derrota y reintento en la última veladora.
- Al reaparecer: luz llena, los pétalos vuelven, los olvidados regresan a su inicio. Las fotos tomadas NO vuelven.
- Veladoras encendidas recargan luz (1.5/s) cuando Xólotl está cerca. Opción de viento: se apagan y encienden en ciclo.
- Olvidados del sótano (onlyMoveWhenPlayerMoves): solo avanzan mientras Xólotl se mueve.
- Marea de olvidados (ChaseWall): la luz no la detiene; empieza en x = -20 y se activa en triggerX.
- drainPerSecond = 0.2. Salto: ~2.8 de alto y ~5 de largo; obstáculos ≤ 2 de alto y huecos ≤ 4 (los huecos de 6 llevan PapelPicado).

## Pendientes en orden
1. Arte, audio, pulido, build Web.

## Reglas para la IA
- Primero `unity status` para revisar si hay un Editor conectado; si lo hay, modifica escenas con comandos del Editor. Nunca edites a mano el YAML de `.unity`, `.prefab` o `.asset`.
- Si hay errores de compilación, el Editor entra en Safe Mode y no se puede conectar: corrige el C# primero.
- No agregues paquetes ni sistemas nuevos sin preguntar. La rúbrica premia mecánicas bien hechas, no cantidad de sistemas.
- Prioriza que el juego funcione en WebGL: nada de threads, `System.IO` de archivos ni APIs que no corran en navegador.
- Todo uso de IA se declara en los créditos (requisito de la jam).
