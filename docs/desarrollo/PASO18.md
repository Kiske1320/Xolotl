# Paso 18: pulido final (partículas, sacudida de cámara y aplastar/estirar)

Lee el CLAUDE.md. Copié 3 scripts nuevos: CameraShake.cs, SquashStretch.cs y ParticleBurst.cs. Haz commit antes de empezar. Escenas y prefabs con comandos del Editor, sin editar .unity, .prefab ni .asset a mano. Puedes editar los .cs indicados solo para agregar las llamadas de abajo. No cambies colliders, posiciones ni valores de jugabilidad. Hoy es el día de entrega: si algo se complica, sáltalo y dime; no rompas nada que ya funciona.

Rendimiento Web: cada ParticleSystem con Max Particles ≤ 40, material Sprite-Unlit-Default (o Particles/Unlit de URP 2D), sin colisiones ni luces en partículas.

## Parte A: sacudida de cámara
1. Agrega CameraShake al prefab MainCamera (globalStrength 1, frequency 30).
2. Llamadas (agrega `CameraShake.Shake(fuerza, duración)`):

| Script | Momento | Fuerza | Duración |
|---|---|---|---|
| XolotlLight | cuando la llamarada SÍ se dispara | 0.08 | 0.15 |
| OlvidadoController | cuando golpea a Xólotl (TryHit) | 0.15 | 0.2 |
| GameManager | al empezar la derrota | 0.25 | 0.35 |
| ChaseWall | al activarse la marea | 0.12 | 0.5 |

3. Revisa que no se note temblor en reposo ni con la pausa, y que CameraFollow siga suave.

## Parte B: aplastar y estirar a Xólotl
1. En el prefab Xolotl, agrega SquashStretch al objeto que tiene el SpriteRenderer del perro.
   - Si el SpriteRenderer está en la raíz (junto al Rigidbody2D y el collider), NO lo pongas ahí: crea un hijo "Visual", mueve el SpriteRenderer y el Animator a ese hijo sin cambiar cómo se ve, y revisa que XolotlController siga encontrando su Animator y su SpriteRenderer (actualiza las referencias). Si eso es riesgoso, sáltate esta parte y dímelo.
2. Asigna controller, body y spriteRenderer. Valores por defecto.
3. Revisa que al aterrizar desde alto se aplaste un instante, que al saltar se estire y que las patas no floten ni se hundan.
4. Revisa que la Aurora del lomo (si es hijo del sprite) se mueva con él sin verse rara; si se ve mal, que Aurora no sea hija del Visual.

## Parte C: partículas
Crea en cada escena de nivel (01, 02, 03) un objeto **VFX** con estos hijos, cada uno con ParticleSystem + ParticleBurst. Simulation Space World, Play On Awake apagado, Emission Rate 0, Sorting Layer Default por encima de los personajes. Hazlos un prefab "VFX" en Assets/_Xolotl/Prefabs para reutilizarlo.

1. **Petalo** (id "petalo", count 10): partículas pequeñas cuadradas de 1 a 2 píxeles color #FF9F1C a #FFD166, salen en círculo (Shape Circle, radio 0.2), velocidad 1.5 a 3, gravedad 0.3, vida 0.4 a 0.7 s, tamaño que se reduce a 0.
   → En Petal, al tomarlo: `ParticleBurst.Play("petalo", transform.position);`
2. **Llamarada** (id "llamarada", count 30): chispas naranjas (#FF9F1C a #FF5E1F) en anillo (Shape Circle, Radius Thickness 0, radio 0.5), velocidad 6 a 9 hacia afuera, sin gravedad, vida 0.3 a 0.5 s, tamaño que se reduce.
   → En XolotlLight, cuando la llamarada SÍ se dispara: `ParticleBurst.Play("llamarada", transform.position);`
3. **Veladora** (id "veladora", count 14): chispas que suben (Shape Cone hacia arriba, ángulo 25), color #FFD166, velocidad 1 a 2.5, vida 0.5 a 0.8 s.
   → En Veladora, al encenderse con la llamarada y al volverse checkpoint por primera vez: `ParticleBurst.Play("veladora", flama o transform.position);`
4. **Foto** (id "foto", count 16): destellos blancos cálidos (#FFF4D6) en todas direcciones, velocidad 1 a 2, vida 0.5 s.
   → En MemoryPhoto, al tomarla: `ParticleBurst.Play("foto", transform.position);`

5. **Brasas de Xólotl** (sin ParticleBurst, continuo): en el prefab Xolotl, un hijo "Brasas" con ParticleSystem en loop, Simulation Space World, Rate over Time 6, partículas de 1 píxel naranja que suben despacio (velocidad 0.3 a 0.8) y se desvanecen en 0.6 a 1 s. Que la cantidad baje cuando la luz es baja: en XolotlLight, en OnLightChanged o en Update, ajusta `emission.rateOverTime = 2 + 6 * LightNormalized` (si es más simple, déjalo fijo en 5).
6. **Niebla de la marea**: si en el PASO17 no se hizo, la niebla de almas de la marea va aquí con las mismas reglas.

## Al final
1. Dale Play desde 00_Titulo hasta 04_Ofrenda: revisa que todo se vea y se sienta mejor sin errores en la consola y sin bajones de FPS.
2. Haz un build Web nuevo y pruébalo en el navegador en pantalla completa. Si va lento, baja a la mitad las partículas (count y Rate) y vuelve a probar.
3. Haz commit con el mensaje "PASO18: pulido (partículas, sacudida, squash)".
