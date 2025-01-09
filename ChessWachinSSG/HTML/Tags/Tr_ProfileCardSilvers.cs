using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Replacer para las medallas de plata
	/// de una tarjeta de perfil.
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	/// <param name="colorClass">
	/// Clase de color del texto. 
	/// Puede ser blanco o dorado, plateado o bronzado.</param>
	/// <param name="count">Número de medallas de plata conseguidas.</param>
	public class Tr_ProfileCardSilvers(IFileReader reader, string colorClass, int count) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			if (count == 0) {
				return string.Empty;
			}

			var template = reader.GetStream("Sources/profile_card_silver_medals.html").ReadToEnd();

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:player:card:rankingcolorclass", new Tr_Inline(colorClass) },
				{ "cwssg:profile:silver:count", new Tr_Inline($"{count}") }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

	}

}
