using System.Text.Json;
using ChessWachinSSG.Data.Dtos;

namespace ChessWachinSSG.Data.Daos {

	/// <summary>
	/// Clase que obtiene los datos de las partidas
	/// almacenadas externamente.
	/// </summary>
	/// <param name="_matches">Lista de partidas.</param>
	public class MatchDao(List<MatchDto> _matches) {

        /// <summary>
        /// Carga las partidas a partir de
        /// un archivo externo.
        /// </summary>
        /// <param name="path">Ruta del archivo JSON.</param>
        /// <param name="reader">Clase lectora.</param>
        /// <returns>Dao inicializado.</returns>
        public static MatchDao FromFile(string path, IFileReader reader) => new(LoadMatches(path, reader));

        /// <summary>
        /// Dao vacío.
        /// </summary>
        public static MatchDao Empty { get => new([]); }


        /// <returns>Lista con todas las partidas cargadas.</returns>
        public List<MatchDto> GetAllMatches() => _matches;


        /// <summary>
        /// Carga todas las partidas de un archivo JSON.
        /// </summary>
        /// <param name="path">Ruta del archivo.</param>
        /// <param name="reader">Clase lectora.</param>
        /// <returns>Lista con las partidas cargadas.</returns>
        private static List<MatchDto> LoadMatches(string path, IFileReader reader) {
            var output = new List<MatchDto>();

            logger.Trace($"Leyendo el archivo {path}.");

            if (!reader.Exists(path)) {
                logger.Error($"El archivo {path} no existe.");
                return output;
            }

            using (var r = reader.GetStream(path)) {
                JsonDocument json = JsonDocument.Parse(r.ReadToEnd());

                var root = json.RootElement;

                foreach (var jsonElement in root.EnumerateArray()) {
                    string? firstPlayer = DaoMethods.GetJsonString("player1", jsonElement, logger);
                    string? secondPlayer = DaoMethods.GetJsonString("player2", jsonElement, logger);
                    string? resultStr = DaoMethods.GetJsonString("result", jsonElement, logger);

                    int? moves = DaoMethods.GetJsonInt("moves", jsonElement, logger);
                    int? duration = DaoMethods.GetJsonInt("duration", jsonElement, logger);
                    int? durationType = DaoMethods.GetJsonInt("duration_type", jsonElement, logger);
                    string? link = DaoMethods.GetJsonString("link", jsonElement, logger);
                    string? date = DaoMethods.GetJsonString("date", jsonElement, logger);

                    if (firstPlayer == null || secondPlayer == null || resultStr == null || moves == null ||
                        duration == null || durationType == null || link == null || date == null)
                    {
                        logger.Error($"Detectada partida con argumentos incompletos.");
                        continue;
                    }

                    Winner result = resultStr switch {
                        "0" => Winner.First,
                        "1" => Winner.Second,
                        "D" => Winner.Draw,

                        _ => Winner.Draw
					};

                    output.Add(new(
                        firstPlayer, 
                        secondPlayer, 
                        result, 
                        moves.Value, 
                        duration.Value, 
                        durationType.Value, 
                        link, 
                        date));
                }
            }

            logger.Trace($"Cargadas {output.Count} partidas.");

            return output;
        }

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    }

}
