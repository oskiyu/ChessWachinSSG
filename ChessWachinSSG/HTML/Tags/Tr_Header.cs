using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Replacer para el header de la página,
	/// que contiene los enlaces a las noticias y
	/// a los perfiles, así como el título.
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	public class Tr_Header(IFileReader reader) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/header.html");

			var replacers = new Dictionary<string, ITagReplacer>() {
				{ "cwssg:header:dropdown:players", new Tr_HeaderPlayerList() }
			};

			return HtmlBuilder.Process(template.ReadToEnd(), context, replacers);
		}

	}

}
