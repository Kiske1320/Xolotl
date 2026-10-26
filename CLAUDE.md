# Xólotl — Spooky Game Jam (Hybridge) · Kiske Studio

## Contexto
- Juego 2D de plataformas en pixel art, Unity 6.5 (6000.5.4f1), URP 2D, build **solo Web (WebGL)**.
- Jam del 26 al 30 de octubre de 2026. **Entrega real: jueves 29 antes de las 11:00 p. m.**
- Temática: "visitantes del más allá". Xólotl, el perro de fuego que guía a los muertos, cruza al pueblo de los vivos la noche del 1 de noviembre para llevar el alma de la abuela Rosa a su ofrenda.
- Mecánica central: una sola variable, el radio de luz de Xólotl. Se consume con el tiempo, se recarga con pétalos de cempasúchil y se gasta en la llamarada.
- Mecánicas obligatorias de la jam: protagonista sobrenatural, interacción con el mundo humano, gestión de recursos, victoria y derrota, jugabilidad interactiva.

## Convenciones del proyecto
- Scripts en `Assets/_Xolotl/Scripts`. Una clase por archivo, mismo nombre que la clase.
- Comentarios y textos del juego en español.
- Pixel art: 16 Pixels Per Unit, Filter Mode Point, Compression None. Nunca escalar sprites en el Transform.
- Input: `InputActionReference` (acciones Move, Jump y Flare de InputSystem_Actions). No usar PlayerInput con Send Messages.
- Unity 6: usar `rb.linearVelocity`, no `rb.velocity`.
- Todas las luces de juego heredan de `LightSource` y se consultan con `LightRegistry.IsLit(punto)`.
- Layers: Player, Enemy, Ground, Spectral. Player vs Enemy desactivado en la matriz de colisión; el daño va por triggers.

## Scripts existentes
LightRegistry, LightSource, XolotlLight, XolotlController, Petal.

## Pendientes en orden
1. Veladora (encendida/apagada, checkpoint), GameManager con derrota y reintento.
2. SpectralPlatform (puentes de papel picado), OlvidadoController, HumanoConVela.
3. Niveles 1–3 en gris, UIManager, título, pausa, FinalScene.
4. Arte, audio, pulido, build Web.

## Reglas para la IA
- Primero `unity status` para revisar si hay un Editor conectado; si lo hay, modifica escenas con comandos del Editor. Nunca edites a mano el YAML de `.unity`, `.prefab` o `.asset`.
- Si hay errores de compilación, el Editor entra en Safe Mode y no se puede conectar: corrige el C# primero.
- No agregues paquetes ni sistemas nuevos sin preguntar. La rúbrica premia mecánicas bien hechas, no cantidad de sistemas.
- Prioriza que el juego funcione en WebGL: nada de threads, `System.IO` de archivos ni APIs que no corran en navegador.
- Todo uso de IA se declara en los créditos (requisito de la jam).
