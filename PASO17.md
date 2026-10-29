# Paso 17: ladrido, voces de los olvidados, fondo de la iglesia y marea sin rectángulo

Lee el CLAUDE.md. No hay scripts nuevos míos: puedes editar los .cs que se indican, solo para sonido y visuales. Escenas y prefabs con comandos del Editor, sin editar .unity, .prefab ni .asset a mano. No cambies colliders, posiciones de juego ni valores de jugabilidad. No generes ni dibujes sprites nuevos con IA. Haz commit antes de empezar.

## Archivos que agregó Jurgen
- `Assets/_Xolotl/Audio/SFX/Jugador/llamarada_aullido.wav`: un ladrido (de OpenGameArt, CC0).
- `Assets/_Xolotl/Audio/SFX/Olvidados/`: gemidos de fantasma de qubodup (pack Ghost Moans, CC0). Si los nombres son largos, renómbralos `olvidado_1`, `olvidado_2`, etc.
Si alguno no está, dime cuál y sigue con lo demás.
Importa todos como efecto: Decompress On Load, Vorbis 70, Force To Mono.

## Parte A: ladrido en la llamarada
1. En XolotlLight, el sonido de la llamarada debe sonar SOLO cuando TryFlare sí la dispara (no en cooldown ni sin luz). Asigna llamarada_aullido, volumen 0.8. Si no existe el campo, agrégalo: `[SerializeField] AudioClip flareBarkSfx;` y `AudioManager.Play(flareBarkSfx, 0.8f);`.
2. Pitch base cercano a 0.9 (un perro grande y místico) con la variación de ±5 % de AudioManager. Si hace falta, agrega a AudioManager un parámetro opcional de pitch base.

## Parte B: que los olvidados se escuchen
Jurgen no escuchó nada de los olvidados. Primero revisa por qué:
1. ¿El hitSfx del prefab Olvidado tiene asignado golpe_olvidado? ¿Se cambió `AudioSource.PlayClipAtPoint` por `AudioManager.Play`? Corrígelo si no.
2. Gemido de cercanía: en OlvidadoController agrega `[SerializeField] AudioClip[] moanSfx;`. Si Xólotl está a menos de 8 unidades y el olvidado NO está congelado, cada 4 a 8 segundos (al azar) reproduce uno al azar. El volumen baja con la distancia (de 0.5 cerca a 0.1 a 8 unidades). Usa un contador estático para que nunca suenen más de 2 gemidos a la vez en toda la escena.
3. Al ser empujado por la llamarada, un gemido corto (volumen 0.5) para que se sienta el golpe.
4. Asigna los clips en el prefab Olvidado (y en la variante veloz o la del sótano, si existen).
5. Marea (ChaseWall): mientras está activa, cada 1.5 a 3 segundos un gemido al azar a volumen 0.6, además del loop marea_olvidados si existe. Se detiene al reiniciarse.
6. Agrega a los créditos: "Voces de los olvidados: qubodup (OpenGameArt, CC0)" y "Ladrido: (el autor que te diga Jurgen) (OpenGameArt, CC0)".

## Parte C: fondo propio para 03_Iglesia
Hoy el fondo de la iglesia se parece al de las calles. Haz que se distinga a primera vista, siempre oscuro:
1. **Atrio (la parte de afuera):** cambia el tinte del cielo lejano a un morado muy oscuro (cerca de #1A0F24) en vez del azul de las calles. En la capa media pon la silueta de la iglesia y el campanario (del pack Anokolisa) grandes y al centro, sin casas.
2. **Interior:** fondo de muro de piedra de iglesia (Anokolisa) en Sprite-Lit-Default, con columnas y ventanales repetidos. Detrás de 3 o 4 ventanales pon una luz Light2D Freeform o Point muy suave en ámbar o rojo vino (intensidad 0.3 a 0.5, sin LightSource, para que no cuente como luz de juego) que parezca un vitral iluminado por la luna. Que NO congele olvidados ni active papel picado: usa solo Light2D, nunca el componente LightSource ni Veladora.
3. Decoración Decor en la iglesia: velas apagadas, bancas, la cruz del altar y algún cempasúchil en el altar si el pack lo trae.
4. Revisa que la luz de Xólotl siga destacando y que los olvidados se vean claro.

## Parte D: la marea sin el rectángulo grande
Al final de la iglesia, detrás de los olvidados de la marea, se ve un rectángulo grande. Debe desaparecer:
1. Localiza el SpriteRenderer (o Image) grande del objeto ChaseWall / MareaOlvidados y desactívalo. NO borres ni cambies el collider ni el script ChaseWall: la lógica de la persecución se queda igual.
2. Para que se siga sintiendo como una ola: pon más fantasmas detrás del frente (6 a 10, en varias alturas y a diferentes distancias hacia atrás), con alphas de 0.85 al frente a 0.3 al fondo, todos hijos de la marea para que se muevan con ella.
3. Opcional: un ParticleSystem hijo de la marea con partículas pálidas (#DDE6FF, alpha 0.2 a 0.4, tamaño pequeño, material Sprite-Unlit) que salgan hacia arriba y atrás, como niebla de almas. Que no pase de 40 partículas, por el rendimiento en Web.
4. Revisa que se vea bien con la cámara siguiendo a Xólotl y que la marea se siga reiniciando al reaparecer.

## Al final
1. Dale Play en 03_Iglesia completa (atrio, interior y persecución) y en 01_Panteon para escuchar los olvidados.
2. Revisa la consola sin errores.
3. Haz un build Web nuevo.
4. Haz commit con el mensaje "PASO17: ladrido, voces de olvidados, fondo de iglesia y marea".
