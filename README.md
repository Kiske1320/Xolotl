# 🐕‍🔥 Xólotl: Antes del Amanecer

![Gameplay](docs/gameplay.gif)

> 🏆 **Primer lugar en la Spooky Game Jam de Hybridge (octubre 2026)**
>
> *Un plataformero 2D de Día de Muertos donde tu luz es lo único que mantiene a salvo a la abuela.*

▶️ **[Jugar en el navegador](https://kiske1320.itch.io/xolotl-antes-del-amanecer)** · 🎮 Unity 6000.5.4f1 (2D URP) · ⏱️ Hecho en 4 días · 👤 Desarrollo en solitario (Kiske Studio)

---

## Sobre el juego
Cada noche del 1 de noviembre, las almas cruzan desde el Mictlán para visitar a quienes las recuerdan. Eres **Xólotl**, un xoloitzcuintle de fuego que debe llevar el alma de la abuela Aurora hasta su ofrenda **antes del amanecer**.

Tu fuego se apaga poco a poco. Los pétalos de cempasúchil lo recargan, y las veladoras son tu refugio. Lo que no ilumina tu luz no es seguro: los **olvidados** avanzan en la oscuridad y se congelan bajo la luz, y los puentes de papel picado solo existen mientras los alumbras.

## Controles
| Acción | Teclado | Control |
|---|---|---|
| Moverse | A / D · ← → | Stick izquierdo |
| Saltar | Espacio | A |
| Llamarada | Shift / J | BOTÓN |
| Pausa | P / Esc | Start |

## Mecánicas
- **Radio de luz:** se consume solo (unos 30 s de luz completa) y define qué puedes ver y qué te puede alcanzar
- **Pétalos:** recargan tu fuego
- **Llamarada:** gasta luz para expandir el radio, encender veladoras y empujar a los olvidados
- **Veladoras:** checkpoint, zona segura y recarga; algunas están expuestas al viento
- **Olvidados:** avanzan en la oscuridad y se congelan bajo la luz. La variante del sótano solo se mueve cuando tú te mueves
- **Puentes de papel picado:** solo son sólidos bajo la luz
- **Persecución** en la Iglesia y **3 finales** según las fotos que recolectes

## Lo que hice
- **Diseño y programación:** todo el gameplay en C#: `XolotlController`, `XolotlLight`, `LightSource`, `LightRegistry`, `Veladora`, `Petal`, `OlvidadoController`, `SpectralPlatform`, `GameManager`, cámara por zonas, intro, pausa y escena final
- **Estructura:** 6 escenas (intro, título, Panteón, Calles, Iglesia, Ofrenda), checkpoints, derrota y reintento
- **Pulido:** partículas (pétalos, llamarada, veladoras, brasas), squash & stretch, fondo propio de la Iglesia con vitrales
- **Audio:** música compuesta por mí con apoyo de IA (Suno) y SFX editados en Audacity
- **Flujo de trabajo:** Claude Code conectado a Unity por MCP, desarrollo por pasos documentados y Git + LFS
- **Proceso documentado:** el desarrollo completo, paso a paso, está en [`docs/desarrollo/`](docs/desarrollo/)


## Retos técnicos que resolví
- **Que la luz visual y la luz del gameplay fueran la misma.** Cada fuente de luz (Xólotl, veladoras, vecinos) hereda de `LightSource` y se registra en `LightRegistry`. Cualquier objeto pregunta `LightRegistry.IsLit(punto)`, y así los olvidados se congelan y los puentes se vuelven sólidos exactamente donde el jugador ve luz. El radio del Light 2D (`pointLightOuterRadius`) cambia con la luz restante, con parpadeo y brasas como indicador.
- **Feedback de la llamarada.** El ladrido es un SFX libre recortado y con el pitch bajado, y suena solo cuando la llamarada sí se dispara, sincronizado con el anillo de fuego y las partículas. Así el jugador sabe al instante si gastó luz o no.
- **Alcance en 4 días.** El lunes quedaron las mecánicas base, el martes el juego completo en gris, el miércoles el build web con arte y música, y el jueves audio, pulido y entrega. En lugar de duplicar niveles, alargué cada uno con secciones mecánicamente distintas.

## Créditos
- **Diseño, programación y música:** Jurgen Guerrero (Kiske Studio)
- **Assets gratuitos:** Pixel Dogs (benvictus), Skeleton Day of the Dead (camacebra), 2D 16px Graveyard Tileset (Segart), Moon Graveyard (Anokolisa), Halloween Graphics (finalbossblues), Free Halloween Characters, Dia De Los Muertos (cccreations), Spooky Playtime (davidkbd), Kenney Impact Sounds, fuente Creepster
- **SFX:** OpenGameArt (ladrido; gemidos de olvidados por qubodup)
- **Uso de IA declarado:** música generada con Suno; desarrollo asistido con Claude Code

## Cómo abrir el proyecto
1. Clona el repo: `git clone https://github.com/Kiske1320/Xolotl.git` (requiere Git LFS)
2. Ábrelo con **Unity 6000.5.4f1** desde Unity Hub
3. Abre `00_Intro` (o `00_Titulo`) y presiona Play
