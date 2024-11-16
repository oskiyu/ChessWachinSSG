using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por la tabla de ELOs.
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	public class Tr_EloTable(IFileReader reader) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/elo_table.html").ReadToEnd();

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:elo:rapid:entries", new Tr_EloTableEntries(reader, EloType.Rapid) },
				{ "cwssg:elo:blitz:entries", new Tr_EloTableEntries(reader, EloType.Blitz) },
				{ "cwssg:elo:updatetime", new Tr_Inline(context.ElosDate) }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

	}

}
