using System.Text.Json;
using ChessWachinSSG.Data.Dtos;

namespace ChessWachinSSG.Data.Daos {

	/// <summary>
	/// Clase que obtiene los datos de los jugadores
	/// almacenados externamente.
	/// </summary>
	/// <param name="_players">Diccionario ID del jugador => jugador.</param>
	public class PlayerDao(Dictionary<string, PlayerDto> _players) {

        /// <summary>
        /// Carga todos los jugadores a partir de un archivo externo.
        /// </summary>
        /// <param name="path">Ruta del archivo JSON.</param>
        /// <param name="reader">Clase lectora.</param>
        /// <returns>Dao inicializado.</returns>
        public static PlayerDao FromFile(string path, IFileReader reader) => new(LoadPlayers(path, reader));

        /// <summary>
        /// Dao vacío.
        /// </summary>
        public static PlayerDao Empty { get => new([]); }


        /// <returns>Lista con todos los jugadores cargados.</returns>
        public List<PlayerDto> GetAllPlayers() => _players.Values.ToList();

        /// <param name="id">ID del jugador.</param>
        /// <returns>Jugador, o null si no se encuentra.</returns>
        public PlayerDto? GetPlayer(string id) => _players.GetValueOrDefault(id);


        /// <summary>
        /// Carga todos los jugadores de un archivo.
        /// </summary>
        /// <param name="path">Ruta del archivo JSON.</param>
        /// <param name="reader">Clase lectora.</param>
        /// <returns>Mapa ID del jugador => jugador.</returns>
        private static Dictionary<string, PlayerDto> LoadPlayers(string path, IFileReader reader) {
            var output = new Dictionary<string, PlayerDto>();

            logger.Trace($"Leyendo el archivo {path}.");

            if (!reader.Exists(path)) {
                logger.Error($"El archivo {path} no existe.");
                return output;
            }

            using (var r = reader.GetStream(path)) {
                JsonDocument json = JsonDocument.Parse(r.ReadToEnd());

                var root = json.RootElement;

                foreach (var jsonElement in root.EnumerateArray()) {
                    string? id = DaoMethods.GetJsonString("id", jsonElement, logger);
                    string? name = DaoMethods.GetJsonString("name", jsonElement, logger);
                    string? countryId = DaoMethods.GetJsonString("country", jsonElement, logger);
                    string? nameTag = DaoMethods.GetJsonString("nametag", jsonElement, logger);

                    if (id == null || name == null || countryId == null || nameTag == null) {
                        continue;
                    }

                    string? pfpPath = null;
                    try {
                        pfpPath = jsonElement.GetProperty("pfp").GetString();
                    }
                    catch { 
                        // No-op: parámetro opcional.
                    }

                    output[id] = new PlayerDto(id, name, countryId, pfpPath, nameTag);
                }
            }

            logger.Trace($"Cargados {output.Count} juagores.");

            return output;
        }

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    }

}
