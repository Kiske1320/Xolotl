# Paso 15: pantalla de introducción y créditos de IA

Lee el CLAUDE.md. Copié un script nuevo: IntroScreen.cs. Todo con comandos del Editor, sin editar .unity, .prefab ni .asset a mano. Haz commit antes de empezar. No cambies nada de jugabilidad.

## Parte A: escena 00_Intro
1. Crea la escena `00_Intro` en la misma carpeta que las demás.
2. Cámara con fondo negro (#000000). Usa el mismo Pixel Perfect Camera (480x270) y el XolotlVolume si las otras escenas lo tienen.
3. Canvas (Screen Space - Overlay, Canvas Scaler con Scale With Screen Size 960x540, Match 0.5) con:
   - **StoryText** (TextMeshPro): fuente Pixelify Sans, centrado en medio de la pantalla, ancho máximo ~70% de la pantalla, color crema cálido #F2E6C9, tamaño legible (que una línea de 2 renglones quepa sin cortarse), alineación centrada, Word Wrapping activado.
   - **HintText** (TextMeshPro): Pixelify Sans, pequeño, abajo a la derecha, color #F2E6C9 con alpha 0.6.
   - Opcional: un pequeño brillo naranja (#FF9F1C) o el sprite del perro animado abajo al centro, sin que tape el texto.
   - **Fader**: el mismo panel negro con ScreenFader que usan las otras escenas, encima de todo.
4. Objeto **Intro** con IntroScreen:
   - storyText y hintText → los de arriba.
   - advanceAction → Jump; skipAction → Pause (InputSystem_Actions).
   - nextScene = `01_Panteon`.
   - Deja los textos que trae el script por defecto.
5. Opcional: si existe `Assets/_Xolotl/Audio/SFX/UI/ui_hover.wav`, ponle un AudioSource (volumen 0.25, Play On Awake apagado) y asígnalo a typeSfx.

## Parte B: conectar el flujo
1. Build Settings en este orden: 00_Titulo, 00_Intro, 01_Panteon, 02_Calles, 03_Iglesia, 04_Ofrenda.
2. En 00_Titulo, en TitleMenu cambia `firstLevel` a `00_Intro`.
3. Revisa que "Jugar" siga reiniciando las fotos de recuerdo como antes.

## Parte C: créditos
En el panel de créditos de 00_Titulo, deja una sección así (corta, en Pixelify Sans, que quepa sin cortarse; si no cabe, reduce el tamaño o usa dos columnas):

```
XÓLOTL: ANTES DEL AMANECER
Kiske Studio

Idea, concepto, diseño, dirección y pruebas:
Jurgen Rodolfo Guerrero Gutiérrez

Uso de inteligencia artificial
· Claude (Anthropic): apoyo en programación, armado de escenas en Unity y documentación.
· Suno: música.
Todas las decisiones creativas y de diseño son del autor.
```

Debajo de eso deja los créditos de assets que ya existen (Pixel Dogs, Anokolisa, Segart, Kenney, Pixelify Sans, etc.). Si algún sprite del juego lo generaste tú (Claude Code) en vez de venir de un pack (por ejemplo la veladora o el pétalo provisionales), agrega a la línea de Claude: "y sprites provisionales de veladora y pétalo", y dime cuáles fueron.

## Al final
1. Dale Play desde 00_Titulo: Jugar → intro → Panteón. Revisa que Espacio complete y avance las líneas, que Esc la salte, que los acentos se vean bien y que no haya errores en la consola.
2. Haz un build Web nuevo.
3. Haz commit con el mensaje "PASO15: intro y créditos de IA".
