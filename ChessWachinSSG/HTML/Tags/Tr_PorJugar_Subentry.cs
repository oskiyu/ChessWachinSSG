using ChessWachinSSG.Data.Dtos;
using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {
	
	public class Tr_PorJugar_Subentry(IFileReader reader, Player other, string color) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/partidas_por_jugar_subentry.html").ReadToEnd();

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:partidasporjugar:color", new Tr_Inline($"{color}") },
				{ "cwssg:partidasporjugar:name:sub", new Tr_Inline($"{other.Name}") }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

	}

}
