using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Replacer para los títulos (competiciones ganadas).
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	/// <param name="colorClass">
	/// Clase de color del texto. 
	/// Puede ser blanco o dorado, plateado o bronzado.</param>
	/// <param name="count">Número de títulos conseguidos.</param>
	public class Tr_ProfileCardTitles(IFileReader reader, string colorClass, int count) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			if (count == 0) {
				return string.Empty;
			}

			var template = reader.GetStream("Sources/profile_card_titles.html").ReadToEnd();

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:player:card:rankingcolorclass", new Tr_Inline(colorClass) },
				{ "cwssg:profile:title:count", new Tr_Inline($"{count}") }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

	}

}
