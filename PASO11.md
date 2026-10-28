# Paso 11: nombres, primer build Web y página de prueba

Lee el CLAUDE.md. No hay scripts nuevos. Todo con comandos del Editor, sin editar archivos .unity ni .prefab a mano.

## Parte A: la abuela ahora se llama Aurora
1. Busca en TODAS las escenas (00_Titulo, 01_Panteon, 02_Calles, 03_Iglesia, 04_Ofrenda) y en los prefabs cualquier texto que diga "Rosa" y cámbialo por "Aurora". En 04_Ofrenda, la línea de FinalScene "Rosa baja de tu lomo y acaricia tu cabeza." debe quedar "Aurora baja de tu lomo y acaricia tu cabeza." (está guardada en el Inspector del componente FinalScene, no en el script).
2. Actualiza el CLAUDE.md: la abuela se llama Aurora.
3. Dime todos los lugares donde encontraste "Rosa".

## Parte A2: subtítulo
Ya existe en Steam un juego llamado "Xolotl" (Anchor Games, 2022), así que el nombre completo del juego es: **Xólotl: Antes del Amanecer**.
1. En 00_Titulo, cambia el texto del subtítulo (hoy dice "El guía de los muertos") por el subtítulo elegido, más chico que el título.
2. En los créditos (CreditsPanel de 00_Titulo), la primera línea queda "Xólotl: Antes del Amanecer — Kiske Studio".
3. Actualiza el nombre completo en el CLAUDE.md.

## Parte B: build Web
1. Revisa que la plataforma activa sea Web y que en Player Settings estén: Compression Format Gzip, Decompression Fallback activado, solo WebGL 2 en Graphics API.
2. En Player Settings: Company Name "Kiske Studio", Product Name "Xólotl: Antes del Amanecer", resolución por defecto del canvas 960x540, Run In Background activado.
3. Revisa que la lista de escenas del build sea: 00_Titulo, 01_Panteon, 02_Calles, 03_Iglesia, 04_Ofrenda.
4. Agrega la carpeta Builds/ al .gitignore del proyecto (en la raíz, junto a Assets) si no está.
5. Haz el build Web en la carpeta Builds/Web dentro del proyecto (fuera de Assets). Puede tardar varios minutos.
6. Al terminar, dime si hubo errores o advertencias importantes, el tamaño total de la carpeta Builds/Web y confirma que contiene index.html, Build/ y TemplateData/.

No hagas commit de la carpeta Builds.
