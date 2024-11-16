# ChessWachinSSG

**ChessWachinSSG** (*Chess-Wachín Static Site Generator*) es un generador de archivos HTML para la página oficial del Chess-wachín.

## Descripción

El programa genera la página del torneo, introduciendo en tiempo de ejecución datos como:
- Clasificación histórica (títulos, medallas, puntos, victorias, empates, derrotas).
- Clasificación actual de ELO (tanto en partidas de 10 minutos como en partidas de 3 minutos) (consulta directa a la API de chess.com).
- Información de cada una de las competiciones:
	- Historial (tanto de fase de liga como fase de playoffs):
		- Resultado de partida, tiempo transcurrido, número de movimientos y fecha.
	- Clasificación actualizada de la fase de liga.
	- Cuadro actualizado de playoffs.
	- ELOs de los jugadores al inicio del torneo.
- Información de cada jugador (en página de perfil):
	- Records personales (mejores rachas de victoria, sin perder y de derrotas, mayor cantidad de puntos en fase de liga, mayor canitdad de victorias en fase de liga).
	- Historial completo.
	- Porcentajes de victoria, empate y derrota.
	- Estadísticas al jugar contra cada otro jugador (victorias, empates y derrotas).


- Datos de competiciones y jugadores, en archivos `.json`:
	- Datos de jugadores (nombre, país, foto de perfil) en `Data/players.json`.
	- Datos de países (icono de la bandera) en `Data/countries.json`.
	- Datos de competiciones (nombre, ELOs iniciales, información sobre las fases de liga y playoffs) en `Data/competitions.json`.
		- Cada fase de liga y playoffs almacena sus partidas en un archivo JSON independiente.
- Páginas HTML originales, en las que se indica donde aparecerán los elementos generados en tiempo de ejecución mediante *tags*. En `WebIn/`.
- Plantillas HTML para elementos que se construirán en tiempo de ejecución (como tablas de clasificación o los cuadros de playoffs). En `Sources/`.

La página final se genera en la ruta `WebOut/`, siguiendo la misma estructura que en `WebIn/`. Contendrá todos los archivos de la ruta `WebIn/`, incluso aunque no hayan sido procesados por el programa.

## Funcionamiento interno

El programa comienza leyendo todos los datos de los archivos `.json`. Con esto, el programa es capaz de generar cualquier tipo de estadística.

La página "original" se almacena en la ruta `WebIn/`. Para indicar dónde colocar qué estadísticas/tablas/etc. se insertan unos *tags* customizados (por ejemplo, `<cwssg:footer>` para indicar donde colocar el *footer*, o `<cwssg:league:ranking cw1:liga>` para colocar la tabla de clasificación de la fase de liga del torneo `cw1`).

El programa contiene una serie de clases reemplazadoras (`ITagReplacer`) cuya función es reemplazar un *tag* por un texto HTML. El programa primero analiza el archivo obteniendo todos los *tags*, y después lo comprueba contra un mapa de reemplazadores, reemplazando cada tag según su reemplazador asignado.

Este es un proceso recursivo: un *tag* puede reemplazarse por un texto HTML que a su vez puede tener otros *tags* a reemplazar.

Para permitir la generación de estructuras más complejas en tiempo de ejecución, se pueden introducir de manera puntual nuevos reemplazadores.