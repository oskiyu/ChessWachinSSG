using System.Text.Json;
using ChessWachinSSG.Data.Dtos;

namespace ChessWachinSSG.Data.Daos {

    /// <summary>
    /// Objeto para cargar los datos de las competiciones.
    /// </summary>
    /// <param name="_competitions">Datos.</param>
    public class CompetitionDao(Dictionary<string, CompetitionDto> _competitions) {

        /// <summary>
        /// Carga los datos almacenados en el archivo indicado.
        /// </summary>
        /// <param name="path">Ruta al archivo JSON con los datos.</param>
        public static CompetitionDao FromFile(string path, IFileReader reader) => new(LoadCompetitions(path, reader));

        /// <summary>
        /// Dao vacío.
        /// </summary>
        public static CompetitionDao Empty { get => new([]); }


        /// <returns>Lista con todas las competiciones cargadas.</returns>
        public List<CompetitionDto> GetAllCompetitions() => _competitions.Values.ToList();

        /// <param name="id">Identificador único de la competición.</param>
        /// <returns>Competición indicada.</returns>
        public CompetitionDto GetCompetition(string id) => _competitions[id];


        /// <summary>
        /// Carga las competiciones desde un archivo JSON.
        /// </summary>
        /// <param name="path">Ruta del archivo.</param>
        /// <param name="reader">Clase lectora.</param>
        /// <returns>
        /// Datos leídos.
        /// Vacío si no se encuentra.
        /// </returns>
        private static Dictionary<string, CompetitionDto> LoadCompetitions(string path, IFileReader reader) {
            var output = new Dictionary<string, CompetitionDto>();

            logger.Trace($"Leyendo el archivo {path}.");

            if (!File.Exists(path)) {
                logger.Error($"El archivo {path} no existe.");
                return output;
            }

            using (var r = reader.GetStream(path)) {
                JsonDocument json = JsonDocument.Parse(r.ReadToEnd());

                var root = json.RootElement;

                foreach (var jsonElement in root.EnumerateArray()) {
                    string? id = DaoMethods.GetJsonString("id", jsonElement, logger);
                    string? name = DaoMethods.GetJsonString("name", jsonElement, logger);

                    if (id == null || name == null) {
                        logger.Error("Entrada con parámetros incorrectos.");
                        continue;
                    }

                    var phases = jsonElement.GetProperty("phases");

                    LeagueDto? leagueData = ProcessLeague(phases);
                    PlayoffsDto? playoffsData = ProcessPlayoffs(phases);

                    if (leagueData == null) {
                        logger.Warn($"No se encuentra la fase de liga para la competición {id}.");
                    }

                    if (playoffsData == null) {
                        logger.Warn($"No se encuentra la fase de playoffs para la competición {id}.");
                    }

                    output[id] = new CompetitionDto(id, name, leagueData, playoffsData, GetInitialElos(jsonElement.GetProperty("initial_elos")));
                }

                logger.Trace($"Cargadas {output.Count} competiciones.");
            }

            return output;
        }

        /// <summary>
        /// Obtiene los elos inciales a partir del elemento JSON.
        /// </summary>
        /// <param name="elos">Elemento con la información.</param>
        /// <returns>Información parseada.</returns>
        private static List<PlayerInitialEloDto> GetInitialElos(JsonElement elos) {
            var output = new List<PlayerInitialEloDto>();

            foreach (var entry in elos.EnumerateArray()) {
                var name = DaoMethods.GetJsonString("name", entry, logger);
                var elo = DaoMethods.GetJsonInt("elo", entry, logger);

                if (name == null || elo == null) {
                    logger.Error("Elo inicial sin argumentos correctos.");
                    continue;
                }

                output.Add(new(name, elo!.Value));
            }

            return output;
        }

        /// <summary>
        /// Procesa los datos de una liga.
        /// </summary>
        /// <param name="phases">Elemento JSON con las fases de una competición.</param>
        /// <returns>
        /// Fase de liga. 
        /// 
        /// Si no están todos los argumentos, devuelve null.
        /// </returns>
        private static LeagueDto? ProcessLeague(JsonElement phases) {
            foreach (var phase in phases.EnumerateArray()) if (phase.GetProperty("type").GetString() == "double round league") {
                    string? leagueId = DaoMethods.GetJsonString("id", phase, logger);
                    string? leagueName = DaoMethods.GetJsonString("name", phase, logger);
                    string? leagueMatchesPath = DaoMethods.GetJsonString("matches", phase, logger);
                    int? numQualifications = DaoMethods.GetJsonInt("num_advances", phase, logger);

                    if (leagueId == null || leagueName == null || leagueMatchesPath == null || numQualifications == null) {
                        logger.Error("Fase de liga sin argumentos correctos.");
                        continue;
                    }

                    return new(leagueId, leagueName, leagueMatchesPath, numQualifications.Value);
                }

            return null;
        }

        /// <summary>
        /// Procesa los datos de unos playoffs.
        /// </summary>
        /// <param name="phases">Elemento JSON con las fases de una competición.</param>
        /// <returns>
        /// Fase de playoffs. 
        /// 
        /// Si no están todos los argumentos, devuelve null.
        /// </returns>
        private static PlayoffsDto? ProcessPlayoffs(JsonElement phases) {
            foreach (var phase in phases.EnumerateArray()) if (phase.GetProperty("type").GetString() == "knockout") {
                    string? id = DaoMethods.GetJsonString("id", phase, logger);
                    string? name = DaoMethods.GetJsonString("name", phase, logger);

                    PlayoffsPhaseDto? sf1 = null;
                    PlayoffsPhaseDto? sf2 = null;
                    PlayoffsPhaseDto? finals = null;

                    foreach (var entry in phase.GetProperty("matches").EnumerateArray()) {
                        string? phaseType = DaoMethods.GetJsonString("type", entry, logger);
                        string? phaseId = DaoMethods.GetJsonString("id", entry, logger);
                        string? phaseName = DaoMethods.GetJsonString("name", entry, logger);
                        string? matchesPath = DaoMethods.GetJsonString("matches", entry, logger);

                        if (phaseType == null || phaseId == null || phaseName == null || matchesPath == null) continue;

                        switch (phaseType)
                        {
                            case "sf1":
                                sf1 = new(phaseId, phaseName, matchesPath);
                                break;

                            case "sf2":
                                sf2 = new(phaseId, phaseName, matchesPath);
                                break;

                            case "finals":
                                finals = new(phaseId, phaseName, matchesPath);
                                break;
                        }
                    }

                    if (id == null || name == null || sf1 == null || sf2 == null || finals == null) continue;

                    var durations = new List<string>();
                    foreach (var entry in phase.GetProperty("default_durations").EnumerateArray()) {
                        durations.Add(entry.GetString() ?? "?");
                    }

                    return new(id, name, sf1, sf2, finals, durations);
                }

            return null;
        }

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    }

}
