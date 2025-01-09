using System.Text.Json;

using ChessWachinSSG.Data.Dtos;

namespace ChessWachinSSG.Data.Daos {

	/// <summary>
	/// Clase que obtiene los datos de las noticias.
	/// </summary>
	/// <param name="_news">Noticias.</param>
	public class NewsDao(List<NewsDto> _news) {

		/// <summary>
		/// Inicializa el DAO, cargando los datos del archivo indicado.
		/// </summary>
		/// <param name="path">Ruta del archivo.</param>
		/// <param name="reader">Clase lectora.</param>
		public static NewsDao FromFile(string path, IFileReader reader) => new(LoadNews(path, reader));

		/// <summary>
		/// Dao vacío.
		/// </summary>
		public static NewsDao Empty { get => new([]); }

		/// <returns>Todas las noticias.</returns>
		public List<NewsDto> GetAllNews() => _news;


		private static List<NewsDto> LoadNews(string path, IFileReader reader) {
			var output = new List<NewsDto>();

			logger.Trace($"Leyendo el archivo {path}.");

			if (!reader.Exists(path)) {
				logger.Error($"El archivo {path} no existe.");
				return output;
			}

			using (var r = reader.GetStream(path)) {
				JsonDocument json = JsonDocument.Parse(r.ReadToEnd());

				var root = json.RootElement;

				foreach (var jsonElement in root.EnumerateArray()) {
					string? title = DaoMethods.GetJsonString("title", jsonElement, logger);
					string? date = DaoMethods.GetJsonString("date", jsonElement, logger);
					string? text = DaoMethods.GetJsonString("text", jsonElement, logger);
					string? imageSrc = DaoMethods.GetJsonString("image", jsonElement, logger);

					if (title == null || date == null || text == null || imageSrc == null) {
						continue;
					}

					output.Add(new(title, date, text, imageSrc));
				}
			}

			logger.Trace($"Cargadas {output.Count} noticias.");

			output.Reverse();

			return output;
		}

		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	}

}
