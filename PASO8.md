# Paso 8: pausa, pantalla de título y escena final (el juego completo)

Lee el CLAUDE.md. Copié tres scripts nuevos en Assets/_Xolotl/Scripts: PauseMenu.cs, TitleMenu.cs y FinalScene.cs. Léelos antes de empezar.

Todo con comandos del Editor, sin editar archivos .unity ni .prefab a mano.

## Parte A: limpieza de XolotlController (el único cambio de código permitido)
1. Borra el Debug.Log temporal de diagnóstico de la llamarada (el bloque marcado "TEMPORAL").
2. Al inicio de Update(), agrega: `if (Time.timeScale == 0f) return;` para que en pausa no se pueda saltar ni lanzar la llamarada.

## Parte B: pausa
1. En InputSystem_Actions, agrega una acción Pause (Button) al mapa Player con Escape, P y Start del gamepad. Guarda el asset.
2. En el prefab UI_Canvas, agrega un PausePanel (desactivado al inicio): fondo negro con alpha 0.7 a pantalla completa, el título "Pausa" y, en columna, botones "Continuar", "Reiniciar nivel" y "Salir al título", más un Slider "Volumen". Textos en TextMeshPro, color crema #F4E7C5.
3. Agrega PauseMenu al UI_Canvas y conecta pauseAction (Player/Pause), pausePanel y volumeSlider. Conecta los OnClick de los botones con Resume, RestartLevel y QuitToTitle.
4. El PausePanel va encima de la interfaz pero DEBAJO del FadePanel.
5. Revisa que cada escena de nivel (01, 02 y 03) tenga un EventSystem con Input System UI Input Module; si no, agrégalo.
6. Aplica los cambios al prefab.

## Parte C: pantalla de título (00_Titulo)
Crea Assets/_Xolotl/Scenes/00_Titulo:
- Fondo oscuro: Global Light 2D igual que en los niveles y un suelo decorativo.
- Un Xólotl de adorno (solo el sprite placeholder con su Light 2D naranja, sin scripts) y 3 pétalos de adorno flotando (sin el script Petal o con el collider desactivado).
- Canvas (Screen Space Overlay, 1920x1080) con:
  - Título "XÓLOTL" grande, color naranja cempasúchil #FF9F1C.
  - Subtítulo "El guía de los muertos", crema.
  - Botones "Jugar" y "Créditos".
  - Texto pequeño de controles abajo: "A / D: moverse · Espacio: saltar · Shift: llamarada · P: pausa".
  - CreditsPanel (desactivado) con fondo oscuro, el texto de créditos de abajo y un botón "Volver".
  - FadePanel con CanvasGroup y ScreenFader, encima de todo.
- Un GameObject TitleMenu con el script TitleMenu (creditsPanel conectado). OnClick: Jugar → Play; Créditos → ShowCredits(true); Volver → ShowCredits(false).
- EventSystem con Input System UI Input Module.

Texto de créditos (respeta los saltos de línea):
```
XÓLOTL — Kiske Studio
Spooky Game Jam de Hybridge, 2026

Diseño, programación y música: Jurgen Guerrero

Arte de terceros (gratuito, con crédito):
Benvictus (Pixel Dogs) · Camacebra Games · Anokolisa (Classic NES Legacy Pack)
Segart · finalbossblues · CCCreations y Tonbi
Efectos de sonido: Kenney (CC0)

Uso de inteligencia artificial: se usó Claude (Anthropic) para el diseño del juego,
la investigación de assets gratuitos, el código en C# y el armado de escenas en Unity
mediante Claude Code. El arte, la música y los efectos de sonido no fueron generados con IA.
```

## Parte D: escena final (04_Ofrenda)
Crea Assets/_Xolotl/Scenes/04_Ofrenda:
- Global Light 2D un poco más clara que en los niveles (intensidad 0.15).
- Una ofrenda placeholder al centro: tres escalones de bloques (de abajo hacia arriba más angostos), con un "papel picado" arriba (sprites PapelPicado en fila, sin SpectralPlatform) y la foto de la abuela al centro (sprite Foto, sin script).
- 4 veladoras de adorno alrededor de la ofrenda: prefab Veladora encendida (no hay GameManager en esta escena; el script lo tolera).
- Un Xólotl de adorno (sprite placeholder con su luz, sin scripts) a la izquierda de la ofrenda y el alma de la abuela (sprite placeholder blanco translúcido) junto a él.
- Canvas con:
  - StoryText (TextMeshPro) centrado en la parte de abajo, grande, color crema.
  - PhotoCountText debajo, más chico.
  - EndPanel con CanvasGroup: el texto "Fin", "Gracias por jugar" y un botón "Volver al inicio".
  - FadePanel con CanvasGroup y ScreenFader, encima de todo.
- Un GameObject FinalScene con el script FinalScene, conectando storyText, photoCountText y endPanel. El botón "Volver al inicio" llama a FinalScene.GoToTitle.
- EventSystem con Input System UI Input Module.

## Parte E: orden del build
En Build Profiles, la lista de escenas debe quedar exactamente así: 00_Titulo, 01_Panteon, 02_Calles, 03_Iglesia, 04_Ofrenda (00_Prueba no va).

Guarda todo y dime si hay errores en la Console.
