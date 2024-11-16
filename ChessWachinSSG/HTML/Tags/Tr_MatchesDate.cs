using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por la fecha en la que se jugaron
	/// las siguientes partidas, y por dichas partidas.
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	/// <param name="date">Fecha.</param>
	/// <param name="matches">Lista de partidas en esa fecha.</param>
	public class Tr_MatchesDate(IFileReader reader, string date, List<Match> matches) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:matches:date", new Tr_Inline(date) },
				{ "cwssg:matches:date:entries", new Tr_MatchesInDate(reader, matches) },
			};

			return HtmlBuilder.Process(
				reader.GetStream("Sources/league_history_date_entry.html").ReadToEnd(), 
				context, 
				replacers);
		}

	}

}
