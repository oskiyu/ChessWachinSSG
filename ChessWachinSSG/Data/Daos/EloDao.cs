using System.Text.Json;
using ChessWachinSSG.Data.Dtos;

namespace ChessWachinSSG.Data.Daos {

	/// <summary>
	/// Clase que obtiene los datos de los ELOs
	/// de los jugadores almacenados externamente.
	/// </summary>
	/// <param name="data._elo">Elo de los jugadores.</param>
	/// <param name="data._date">Fecha de la última actualización.</param>
	public class EloDao((List<EloDto> _elos, string _date) data) {

        /// <summary>
        /// Carga los datos de un archivo.
        /// </summary>
        /// <param name="path">Ruta del archivo.</param>
        /// <param name="reader">Clase lectora.</param>
        /// <returns>Dao inicializado con todos los datos del archivo.</returns>
        public static EloDao FromFile(string path, IFileReader reader) => new(LoadElos(path, reader));

        /// <summary>
        /// Dao vacío.
        /// </summary>
        public static EloDao Empty { get => new(([], "")); }


        /// <returns>Lista con todos los elos cargados.</returns>
        public List<EloDto> GetAllElos() => data._elos;

        /// <returns>Fecha de la última actualización de ELOs.</returns>
        public string GetDate() => data._date;


        /// <summary>
        /// Guarda los datos en un archivo JSON.
        /// </summary>
        /// <param name="path">Ruta del archivo JSON.</param>
        public void Save(string path) {
            using var writer = new StreamWriter(path);


            writer.WriteLine("{ \"elos\": ");

            writer.WriteLine("[");

            // Función que genera un objeto JSON
            // para una entrada en concreto.
            var WriteEntry = (string key, EloEntryDto entry) => {
				writer.WriteLine($"\t,\"{key}\": {{");

				writer.WriteLine($"\t\t\"elo\": {entry.Elo},");
				writer.WriteLine($"\t\t\"wins\": {entry.Wins},");
				writer.WriteLine($"\t\t\"draws\": {entry.Draws},");
				writer.WriteLine($"\t\t\"losses\": {entry.Losses}");

				writer.WriteLine("\t}");
			};

            int count = 0;
            foreach (var elo in data._elos) {
                writer.WriteLine("{");

                writer.WriteLine($"\t\"player\": \"{elo.PlayerId}\"");

                if (elo.Rapid != null) {
                    WriteEntry("rapid", elo.Rapid);
				}

                if (elo.Blitz != null) {
					WriteEntry("blitz", elo.Blitz);
				}

                writer.WriteLine("}");

                if (count != data._elos.Count - 1) {
                    writer.Write(",");
                }

                count++;
            }

            writer.WriteLine("],");

            writer.WriteLine($"\"date\": \"{data._date}\"");

            writer.WriteLine("}");
        }

        /// <summary>
        /// Carga los ELOs de un archivo externo.
        /// </summary>
        /// <param name="path">Ruta del archivo externo.</param>
        /// <param name="reader">Clase lectora.</param>
        /// <returns>Lista de ELOs y fecha de última actualización.</returns>
        private static (List<EloDto> elos, string date) LoadElos(string path, IFileReader reader) {
            var output = new List<EloDto>();

            logger.Trace($"Leyendo el archivo {path}.");

            if (!reader.Exists(path)) {
                logger.Error($"El archivo {path} no existe.");
                return (output, string.Empty);
            }

            using var r = reader.GetStream(path);
            JsonDocument json = JsonDocument.Parse(r.ReadToEnd());

            var root = json.RootElement;

            foreach (var jsonElement in root.GetProperty("elos").EnumerateArray()) {
                string? id = DaoMethods.GetJsonString("player", jsonElement, logger);

                if (id == null) {
                    logger.Error($"Detectado elo con argumentos incompletos.");
                    continue;
                }

                EloEntryDto? rapid = null;
                EloEntryDto? blitz = null;

                try {
                    int rapidElo = jsonElement.GetProperty("rapid").GetProperty("elo").GetInt32();
                    int rapidWins = jsonElement.GetProperty("rapid").GetProperty("wins").GetInt32();
                    int rapidDraws = jsonElement.GetProperty("rapid").GetProperty("draws").GetInt32();
                    int rapidLosses = jsonElement.GetProperty("rapid").GetProperty("losses").GetInt32();

                    rapid = new(rapidElo, rapidWins, rapidDraws, rapidLosses);
                }
                catch { 
                    // NO-OP.
                }

                try
                {
                    int blitzElo = jsonElement.GetProperty("blitz").GetProperty("elo").GetInt32();
                    int blitzWins = jsonElement.GetProperty("blitz").GetProperty("wins").GetInt32();
                    int blitzDraws = jsonElement.GetProperty("blitz").GetProperty("draws").GetInt32();
                    int blitzLosses = jsonElement.GetProperty("blitz").GetProperty("losses").GetInt32();

                    blitz = new(blitzElo, blitzWins, blitzDraws, blitzLosses);
                }
                catch {
					// NO-OP.
                }

				output.Add(new(id, rapid, blitz));
            }

            logger.Trace($"Cargados {output.Count} elos.");

            return (output, json.RootElement.GetProperty("date").GetString()!);
        }

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    }

}
