# Paso 14: que la oscuridad y la luz se lean (pulido visual)

Lee el CLAUDE.md. No hay scripts nuevos. Este paso SOLO cambia lo que se ve: no muevas ni cambies colliders, posiciones de juego ni valores de los scripts. No generes ni dibujes sprites nuevos. Todo con comandos del Editor, sin editar .unity, .prefab ni .asset a mano. Haz commit antes de empezar.

## Problema
La mecánica central es la luz en la oscuridad, pero los fondos del PASO13 están demasiado claros (cielo y edificios en azul brillante). La luz de Xólotl casi no destaca, los olvidados azules se confunden con el fondo y los pétalos se ven como bolitas cafés.

## Parte A: oscurecer los fondos
1. En 01_Panteon, 02_Calles, 03_Iglesia y 00_Titulo, oscurece las capas de fondo (Sorting Layer Background): el cielo más lejano con un tinte cerca de #0B1022 y las siluetas de casas, árboles e iglesia cerca de #141B33. Deben distinguirse apenas como siluetas.
2. La luna puede quedarse un poco más clara que el cielo, pero sin brillar (que no compita con el fuego).
3. Revisa que la decoración de la Sorting Layer Decor (Sprite-Lit) se vea casi negra fuera de las luces y se revele cuando Xólotl se acerca.
4. En 00_Titulo, oscurece menos que en los niveles (es la portada): que el título siga luciendo.

## Parte B: viñeteado y bloom (post-processing)
1. Activa Post Processing en la MainCamera del prefab y en las cámaras de 00_Titulo y 04_Ofrenda.
2. Crea un Global Volume en cada escena (o un perfil compartido XolotlVolume en Assets/_Xolotl/Settings) con:
   - Vignette: intensidad 0.35, smoothness 0.4, color negro.
   - Bloom: threshold 0.9, intensity 0.6, scatter 0.6.
3. Revisa que el bloom no haga borroso el texto de la interfaz ni los textos de ayuda.
4. Importante para Web: después del cambio haz un build Web y dime si bajó el rendimiento o hay errores. Si el bloom causa problemas en Web, déjalo en 0.3 o quítalo y deja solo la viñeta.

## Parte C: olvidados que se lean
1. En el prefab Olvidado y en los fantasmas de MareaOlvidados: material Sprite-Unlit-Default (brillan solos, como almas), color blanco pálido #DDE6FF con alpha 0.85.
2. Que se distingan claramente del fondo en los tres niveles.
3. El efecto de transparencia al congelarse (alpha bajo en la luz) debe seguir funcionando.

## Parte D: pétalos que se vean
1. En el prefab Petal: material Sprite-Unlit-Default y color naranja cempasúchil #FF9F1C, sin cambiar el sprite.
2. Que se vean desde lejos en la oscuridad, como pequeñas brasas.

## Parte E: color del perro
1. Cambia la hoja de Pixel Dogs al color más oscuro disponible (gris carbón o negro): un xoloitzcuintle es oscuro y sin pelo. Mantén las mismas animaciones, el mismo PPU (24) y los mismos cortes. Aplica también a los perros de adorno de 00_Titulo y 04_Ofrenda.
2. Revisa que se siga viendo bien junto a su luz naranja y dime qué color elegiste.

## Parte F: fuente pixelada
Jurgen descargó la fuente Pixelify Sans (licencia OFL) en Assets/_Xolotl/Art/Fonts/.
1. Crea un TMP Font Asset con ella.
2. Úsala en los botones, el subtítulo, los créditos, la interfaz (contador de recuerdos), la pausa, los textos de ayuda del mundo y los textos de 04_Ofrenda.
3. El título "XÓLOTL" también, si se ve bien en grande; si no, déjalo con la fuente actual y dime.
4. Revisa que los acentos y la ñ se vean bien (á, é, í, ó, ú, ñ, ¡, ¿).
5. Agrega a los créditos: "Fuente: Pixelify Sans (SIL Open Font License)".

## Al final
Dale Play en los tres niveles y revisa que la luz de Xólotl destaque, que los olvidados y los pétalos se vean desde lejos y que se juegue igual que antes. Haz un build Web nuevo. Dime si hay errores y haz commit con el mensaje "PASO14: oscuridad, post-processing y legibilidad".
