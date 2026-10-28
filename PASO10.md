# Paso 10: zonas de cámara para la cripta y el sótano

Lee el CLAUDE.md. Reemplacé CameraFollow.cs (ahora acepta límites verticales temporales) y agregué CameraZone.cs. Léelos antes de empezar. No cambies los scripts salvo que haya un error de compilación; si es así, avísame antes.

Todo con comandos del Editor, sin editar archivos .unity ni .prefab a mano.

## Problema
En el PASO9 se bajó minY de CameraFollow en 01_Panteon y 03_Iglesia para ver las zonas subterráneas, y ahora en la superficie se ve mucho vacío debajo del piso.

## Solución
1. En 01_Panteon y 03_Iglesia, regresa minY de CameraFollow al valor que tenía antes del PASO9 (el que dejaba ver solo hasta y = -2 en la superficie; en 01_Panteon era cerca de 6.44).
2. En 01_Panteon, crea un GameObject CameraZone_Cripta con BoxCollider2D en Trigger que cubra toda la cripta, desde el primer escalón de bajada hasta el último de subida, y desde el piso de la cripta hasta unas 3 unidades arriba de la superficie. Agrégale CameraZone y calcula zoneMinY y zoneMaxY para que dentro de la cripta se vea completa (piso de la cripta abajo de la pantalla, sin mostrar nada debajo de y = -10).
3. En 03_Iglesia, haz lo mismo con CameraZone_Sotano para el sótano (sin mostrar nada debajo de y = -8).
4. Guarda, dale Play en ambos niveles y revisa que en la superficie la cámara se vea como antes del PASO9 y que en la cripta y el sótano se vea el piso de abajo. Dime si hay errores en la Console.
