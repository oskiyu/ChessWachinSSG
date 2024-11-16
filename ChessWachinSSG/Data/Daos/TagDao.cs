using ChessWachinSSG.Data.Dtos;
using ChessWachinSSG.HTML.Tags;

using System.Text.Json;

namespace ChessWachinSSG.Data.Daos {

	/// <summary>
	/// Clase que obtiene los datos de los tags
	/// almacenados externamente.
	/// </summary>
	/// <param name="_tags">Mapa texto del tag => tag.</param>
	public class TagDao(Dictionary<string, TagDto> _tags) {

		/// <summary>
		/// Carga los tags a partir de un archivo externo.
		/// </summary>
		/// <param name="path">Ruta del archivo JSON.</param>
		/// <param name="reader">Clase lectora.</param>
		/// <returns>Dao inicializado.</returns>
		public static TagDao FromFile(string path, IFileReader reader) => new(LoadTags(path, reader));

		/// <summary>
		/// Dao vacío.
		/// </summary>
		public static TagDao Empty { get => new([]); }


		/// <returns>Todos los tags cargados.</returns>
		public List<TagDto> GetAllTags() => _tags.Values.ToList();

		/// <returns>Mapa texto del tag => tag.</returns>
		public Dictionary<string, TagDto> GetTagsMap() => _tags;

		/// <param name="id">ID del tag.</param>
		/// <returns>Tag, o null si no se encuentra.</returns>
		public TagDto? GetTag(string id) => _tags.GetValueOrDefault(id);


		/// <summary>
		/// Carga todos los tags de un archivo.
		/// </summary>
		/// <param name="path">Ruta del archivo JSON.</param>
		/// <param name="reader">Clase lectora.</param>
		/// <returns>Mapa ID del tag => tag.</returns>
		private static Dictionary<string, TagDto> LoadTags(string path, IFileReader reader) {
			var output = new Dictionary<string, TagDto>();

			logger.Trace($"Leyendo el archivo {path}.");

			if (!reader.Exists(path)) {
				logger.Error($"El archivo {path} no existe.");
				return output;
			}

			using (var r = reader.GetStream(path)) {
				JsonDocument json = JsonDocument.Parse(r.ReadToEnd());

				var root = json.RootElement;

				foreach (var jsonElement in root.EnumerateArray()) {
					string? tag = DaoMethods.GetJsonString("tag", jsonElement, logger);

					if (tag == null) continue;

					string? replacerData = "";

					TagReplacerType type = TagReplacerType.Inline;

					try {
						replacerData = jsonElement.GetProperty("path").GetString()!;
						type = TagReplacerType.File;
					}
					catch { /* No-op. */ }

					try {
						replacerData = jsonElement.GetProperty("inline").GetString()!;
						type = TagReplacerType.Inline;
					}
					catch { /* No-op. */ }

					try {
						replacerData = jsonElement.GetProperty("internal").GetString()!;
						type = TagReplacerType.Internal;
					}
					catch { /* No-op. */ }

					if (replacerData == null) {
						logger.Error("No se pudo deducir el tipo de TagReplacer. Posibles tipos son path, inline, internal");
						continue;
					}

					output[tag] = new TagDto(tag, type, replacerData);
				}
			}

			logger.Trace($"Cargados {output.Count} tags.");

			return output;
		}

		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	}

}
