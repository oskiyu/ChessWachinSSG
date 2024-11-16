using System.Text.Json;
using ChessWachinSSG.Data.Dtos;

namespace ChessWachinSSG.Data.Daos {

    /// <summary>
    /// Clase que obtiene los datos de los países
    /// almacenados externamente.
    /// </summary>
    public class CountryDao(Dictionary<string, CountryDto> _countries) {


        /// <summary>
        /// Inicializa la clase, leyendo los datos del
        /// archivo indicado.
        /// </summary>
        /// <param name="path">Ruta del archivo.</param>
        /// <returns>Objeto DAO con los contenidos almacenados
        /// en la ruta.</returns>
        public static CountryDao FromFile(string path, IFileReader reader) => new(LoadCountries(path, reader));

        /// <summary>
        /// Objeto DAO vacío.
        /// </summary>
        public static CountryDao Empty { get => new([]); }


        /// <returns>Lista con todos los objetos.</returns>
        public List<CountryDto> GetAllCountries() => _countries.Values.ToList();

        /// <param name="id">Identificador del país.</param>
        /// <returns>Información sobre el país.</returns>
        public CountryDto GetCountry(string id) => _countries[id];


        /// <summary>
        /// Carga los países a partir de un archivo JSON.
        /// </summary>
        /// <param name="path">Ruta del archivo JSON.</param>
        /// <param name="reader">Clase lectora.</param>
        /// <returns>Diccionario ID del país => país.</returns>
        private static Dictionary<string, CountryDto> LoadCountries(string path, IFileReader reader) {
            var output = new Dictionary<string, CountryDto>();

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
                    string? flagIconPath = DaoMethods.GetJsonString("flagIcon", jsonElement, logger);

                    if (id == null || name == null || flagIconPath == null) {
                        logger.Error($"Detectado país con argumentos incompletos.");
                        continue;
                    }

                    output[id] = new CountryDto(id, name, flagIconPath);
                }
            }

            logger.Trace($"Cargados {output.Count} países.");

            return output;
        }

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    }

}
