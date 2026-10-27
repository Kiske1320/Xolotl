# Paso 2: veladoras, derrota y reintento

Lee el CLAUDE.md. Copié dos scripts nuevos en Assets/_Xolotl/Scripts: Veladora.cs y GameManager.cs. Léelos antes de empezar. No cambies XolotlLight ni XolotlController salvo que haya un error de compilación; si es así, avísame antes.

En la escena 00_Prueba, usando comandos del Editor (sin editar archivos .unity a mano):

1. Revisa que los dos scripts compilen sin errores en la Console.
2. Crea un sprite placeholder de veladora de 16x16 (base clara) y otro de flama de 8x8 naranja, con la convención de 16 PPU, Point y sin compresión, en Assets/_Xolotl/Art/Placeholder.
3. Crea tres veladoras con el script Veladora, cada una con: SpriteRenderer de la base, un hijo Flame con el sprite de flama (asignado a flameRenderer), un hijo Light con Light 2D Point naranja #FF9F1C intensidad 1 (asignado a light2D), y un BoxCollider2D en Trigger de 1x2.
   - Veladora_1 encendida (startLit = true) cerca del inicio, en x = 2 sobre el suelo.
   - Veladora_2 APAGADA (startLit = false) sobre Plataforma_2.
   - Veladora_3 encendida en x = 22 sobre el suelo.
4. Crea un GameObject LevelStart en la posición inicial de Xolotl.
5. Crea un GameObject GameManager con el script GameManager y conecta: xolotlLight, xolotlController, levelStart y retryAction (acción Jump de InputSystem_Actions).
6. Crea la pantalla de derrota con uGUI: un Canvas (Screen Space Overlay, Canvas Scaler con Scale With Screen Size 1920x1080) con un Panel negro a pantalla completa que tenga CanvasGroup (asignado a defeatPanel en GameManager) y dos textos TextMeshPro centrados:
   - "La abuela se perdió en la oscuridad" (grande, color crema).
   - "Presiona Espacio para intentarlo de nuevo" (más chico, gris claro).
   Si TextMeshPro pide importar los TMP Essentials, impórtalos.
7. Para probar más rápido, sube temporalmente drainPerSecond de XolotlLight a 0.5 en el Inspector de Xolotl (no en el script).
8. Guarda la escena y dime si hay errores en la Console.
