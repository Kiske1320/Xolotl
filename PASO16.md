# Paso 16: música y efectos de sonido

Lee el CLAUDE.md. Copié 3 scripts nuevos: AudioManager.cs, MusicManager.cs y MusicPlayer.cs. Haz commit antes de empezar. Cambios de escena y prefabs con comandos del Editor, sin editar .unity, .prefab ni .asset a mano. En este paso SÍ puedes editar los .cs que se indican abajo, solo para agregar sonido. No cambies la jugabilidad.

## Archivos
- Música (de Jurgen, hecha con Suno) en `Assets/_Xolotl/Audio/Musica/`: Musica_Titulo, Musica_Panteon, Musica_Calles, Musica_Iglesia, Musica_Persecucion y Musica_Ofrenda. Si alguna falta o tiene otro nombre, dime cuáles hay antes de seguir.
- Efectos en `Assets/_Xolotl/Audio/SFX/` (carpetas UI, Mundo, Jugador). Algunos del LEEME.md pueden no existir todavía: usa solo los que haya y dime cuáles faltaron. Que el juego funcione igual sin ellos (todo clip vacío se ignora).

## Parte A: importación para Web
1. Música: Load Type Compressed In Memory, Compression Format Vorbis, Quality 50, Load In Background activado, Force To Mono desactivado.
2. Efectos: Load Type Decompress On Load, Vorbis Quality 70, Force To Mono activado.
3. Loops de ambiente (viento, marea_olvidados, luz_baja, si existen): igual que la música pero Quality 40.

## Parte B: música por escena
En cada escena crea un objeto **Musica** con MusicManager (volumen 0.45):

| Escena | music | chaseMusic | ambienceLoop |
|---|---|---|---|
| 00_Titulo | Musica_Titulo | — | — |
| 00_Intro | Musica_Titulo (sigue sin cortarse) | — | — |
| 01_Panteon | Musica_Panteon | — | viento (si existe), 0.15 |
| 02_Calles | Musica_Calles | — | viento (si existe), 0.15 |
| 03_Iglesia | Musica_Iglesia | Musica_Persecucion | — |
| 04_Ofrenda | Musica_Ofrenda | — | — |

En 03_Iglesia revisa que al activarse la ChaseWall cambie a la persecución y que al reaparecer (la marea se reinicia) regrese a la música de la iglesia.

## Parte C: efectos en los scripts
Agrega en cada script un campo `[SerializeField] AudioClip` (o `AudioClip[]`) y llama `AudioManager.Play(clip, volumen)` en el momento indicado. Asigna los clips en los prefabs o en las escenas.

| Script | Momento | Clip | Volumen |
|---|---|---|---|
| Petal | al tomar el pétalo | petalo_1, 2, 3 con `AudioManager.PlayRandom` | 0.6 |
| Veladora | la primera vez que se vuelve checkpoint | checkpoint | 0.7 |
| Veladora | al encenderse con la llamarada | veladora_encender (si no existe, checkpoint) | 0.7 |
| MemoryPhoto | al tomar la foto | foto | 0.8 |
| SpectralPlatform | al pasar de oculto a sólido, solo si Xólotl está a menos de 12 unidades | papel_picado | 0.4 |
| LevelExit | al tocar la salida | salida | 0.7 |
| XolotlController | al aterrizar si venía cayendo a más de 6 de velocidad vertical | aterrizaje | 0.4 |
| XolotlLight | en TryFlare cuando sí se dispara | llamarada_aullido y llamarada_fuego (los que existan) | 0.8 |
| OlvidadoController | ya tiene hitSfx: cambia `AudioSource.PlayClipAtPoint` por `AudioManager.Play(hitSfx, sfxVolume)` y asigna golpe_olvidado | golpe_olvidado | 0.7 |
| GameManager | al empezar la derrota | derrota | 0.8 |
| GameManager | al reaparecer | reaparecer | 0.6 |
| FallZone | al caer | caida (si existe) | 0.7 |
| ChaseWall | mientras está activa, un AudioSource en loop con marea_olvidados (si existe), volumen 0.35; se detiene al reiniciarse | marea_olvidados | 0.35 |

Opcional, solo si existe luz_baja: en XolotlLight un AudioSource en loop que suba de volumen (0 a 0.4) cuando la luz baja de 25 % y se apague al recargar.

## Parte D: interfaz
1. Todos los botones (título, créditos, pausa, final): ui_click al hacer clic (0.5) y ui_hover al pasar el cursor o seleccionar con teclado (0.3). Puedes hacerlo con un script pequeño `UIButtonSound` (IPointerEnterHandler, ISelectHandler y onClick) que agregues a cada botón.
2. PauseMenu: pausa_abrir al pausar y pausa_cerrar al reanudar (0.6).
3. Cerrar créditos: ui_atras (0.5).
4. Revisa que los sonidos de la pausa suenen aunque Time.timeScale sea 0 (AudioSource no depende del timeScale, pero confírmalo).
5. Revisa que el slider de volumen de la pausa afecte música y efectos.

## Parte E: créditos
En el panel de créditos agrega (corto):
- "Música: Jurgen Rodolfo Guerrero Gutiérrez con Suno (IA)". Si ya existe la línea de Suno del PASO15, no la dupliques.
- "Efectos de sonido: Kenney (kenney.nl, CC0)".
- Si hay sonidos de Freesound, agrega "Freesound.org:" y el nombre de cada autor (pregúntame los autores si no están anotados).

## Al final
1. Dale Play desde 00_Titulo hasta 04_Ofrenda. Revisa el volumen: la música debe ir por debajo de los efectos, y los pétalos, la llamarada y los golpes se deben oír claro.
2. Revisa que no haya errores ni advertencias nuevas en la consola.
3. Haz un build Web nuevo y dime cuánto pesa la carpeta Build (antes eran ~17 MB).
4. Haz commit con el mensaje "PASO16: música y efectos de sonido".
