# ChessWachinSSG

**ChessWachinSSG** (*Chess-Wach�n Static Site Generator*) es un generador de archivos HTML para la p�gina oficial del Chess-wach�n.

## Descripci�n

El programa genera la p�gina del torneo, introduciendo en tiempo de ejecuci�n datos como:
- Clasificaci�n hist�rica (t�tulos, medallas, puntos, victorias, empates, derrotas).
- Clasificaci�n actual de ELO (tanto en partidas de 10 minutos como en partidas de 3 minutos) (consulta directa a la API de chess.com).
- Informaci�n de cada una de las competiciones:
	- Historial (tanto de fase de liga como fase de playoffs):
		- Resultado de partida, tiempo transcurrido, n�mero de movimientos y fecha.
	- Clasificaci�n actualizada de la fase de liga.
	- Cuadro actualizado de playoffs.
	- ELOs de los jugadores al inicio del torneo.
- Informaci�n de cada jugador (en p�gina de perfil):
	- Records personales (mejores rachas de victoria, sin perder y de derrotas, mayor cantidad de puntos en fase de liga, mayor canitdad de victorias en fase de liga).
	- Historial completo.
	- Porcentajes de victoria, empate y derrota.
	- Estad�sticas al jugar contra cada otro jugador (victorias, empates y derrotas).


- Datos de competiciones y jugadores, en archivos `.json`:
	- Datos de jugadores (nombre, pa�s, foto de perfil) en `Data/players.json`.
	- Datos de pa�ses (icono de la bandera) en `Data/countries.json`.
	- Datos de competiciones (nombre, ELOs iniciales, informaci�n sobre las fases de liga y playoffs) en `Data/competitions.json`.
		- Cada fase de liga y playoffs almacena sus partidas en un archivo JSON independiente.
- P�ginas HTML originales, en las que se indica donde aparecer�n los elementos generados en tiempo de ejecuci�n mediante *tags*. En `WebIn/`.
- Plantillas HTML para elementos que se construir�n en tiempo de ejecuci�n (como tablas de clasificaci�n o los cuadros de playoffs). En `Sources/`.

La p�gina final se genera en la ruta `WebOut/`, siguiendo la misma estructura que en `WebIn/`. Contendr� todos los archivos de la ruta `WebIn/`, incluso aunque no hayan sido procesados por el programa.

## Funcionamiento interno

El programa comienza leyendo todos los datos de los archivos `.json`. Con esto, el programa es capaz de generar cualquier tipo de estad�stica.

La p�gina "original" se almacena en la ruta `WebIn/`. Para indicar d�nde colocar qu� estad�sticas/tablas/etc. se insertan unos *tags* customizados (por ejemplo, `<cwssg:footer>` para indicar donde colocar el *footer*, o `<cwssg:league:ranking cw1:liga>` para colocar la tabla de clasificaci�n de la fase de liga del torneo `cw1`).

El programa contiene una serie de clases reemplazadoras (`ITagReplacer`) cuya funci�n es reemplazar un *tag* por un texto HTML. El programa primero analiza el archivo obteniendo todos los *tags*, y despu�s lo comprueba contra un mapa de reemplazadores, reemplazando cada tag seg�n su reemplazador asignado.

Este es un proceso recursivo: un *tag* puede reemplazarse por un texto HTML que a su vez puede tener otros *tags* a reemplazar.

Para permitir la generaci�n de estructuras m�s complejas en tiempo de ejecuci�n, se pueden introducir de manera puntual nuevos reemplazadores.