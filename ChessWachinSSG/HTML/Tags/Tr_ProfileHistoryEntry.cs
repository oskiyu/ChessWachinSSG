using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por el historial de partidas
	/// de un jugador en una competición.
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	/// <param name="player">Jugador.</param>
	/// <param name="competition">Competición.</param>
	public class Tr_ProfileHistoryEntry(IFileReader reader, Player player, Competition competition) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/profile_history_entry.html").ReadToEnd();

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:profile:history:entry:cname", new Tr_Inline(competition.Name) },
				{ "cwssg:profile:history:entry:squares", new Tr_Inline(MatchesSquaresGenerator.Generate(player, competition)) }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

	}
}
