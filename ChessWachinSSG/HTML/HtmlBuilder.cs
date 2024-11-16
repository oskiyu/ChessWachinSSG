using ChessWachinSSG.HTML.Tags;
using ChessWachinSSG.Model;

using System.Text;

namespace ChessWachinSSG.HTML {

	/// <summary>
	/// Clase que procesa un elemento de texto y 
	/// reemplaza los tags.
	/// </summary>
	public static class HtmlBuilder {

		/// <summary>
		/// Devuelve el texto "text", habiendo reemplazado
		/// los tags.
		/// 
		/// Es un proceso recursivo: los tags de los tags reemplazados
		/// también se aplican.
		/// </summary>
		/// <param name="text">Texto original.</param>
		/// <param name="context">Contexto de Chess-Wachín-</param>
		/// <param name="customTagsMap">Mapa de tags adicionales.</param>
		/// <returns>Texto procesado.</returns>
		public static string Process(string text, Context context, params IReadOnlyDictionary<string, ITagReplacer>[] customTagsMap) {
			var output = new StringBuilder();
			
			var tags = TagReader.ReadAllTags(text);
			int lastTagEndIdx = 0;
			foreach (var tag in tags) {
				if (!HasTagReplacer(tag.Id, customTagsMap.Prepend(context.TagsReplacers))) {
					continue;
				}

				output.Append(text.AsSpan(lastTagEndIdx, tag.FirstCharacterInex - lastTagEndIdx));


				ITagReplacer replacer = GetTagReplacer(tag.Id, customTagsMap.Prepend(context.TagsReplacers));

				logger.Trace($"Tag replaced: tag {tag.Id}, replacer {replacer}.");

				output.Append(Process(replacer.Replace(tag, context), context, customTagsMap));

				lastTagEndIdx = tag.FirstCharacterInex + tag.TotalCharacters;
			}

			output.Append(text.AsSpan(lastTagEndIdx, text.Length - lastTagEndIdx));

			return output.ToString();
		}


		/// <param name="tagId">ID del tag que reemplazrá un replacer.</param>
		/// <param name="maps">Mapas de tags.</param>
		/// <returns>True si existe al menos un tag replacer para el tag indicado.</returns>
		private static bool HasTagReplacer(string tagId, IEnumerable<IReadOnlyDictionary<string, ITagReplacer>> maps)
			=> maps.Any(x => x.ContainsKey(tagId));

		/// <param name="tagId">ID del tag que reemplazrá un replacer. Debe existir al menos un replacer.</param>
		/// <param name="maps">Mapas de tags.</param>
		/// <returns>Replacer para el tag indicado.</returns>
		private static ITagReplacer GetTagReplacer(string tagId, IEnumerable<IReadOnlyDictionary<string, ITagReplacer>> maps)
			=> maps.Last(x => x.ContainsKey(tagId))[tagId];

		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	}

}
