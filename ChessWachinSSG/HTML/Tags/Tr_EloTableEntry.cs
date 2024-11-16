using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por el ELO de un jugador
	/// en concreto.
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	/// <param name="player">Jugador.</param>
	/// <param name="elo">ELO. Puede ser null si no ha jugado partidas clasificatorias.</param>
	public class Tr_EloTableEntry(IFileReader reader, Player player, PlayerEloEntry? elo) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/elo_table_entry.html").ReadToEnd();

			Dictionary<string, ITagReplacer> replacers;
			if (elo == null || elo.TotalMatches < 10) {
				replacers = new() {
					{ "cwssg:elo:entry:player", new Tr_Inline($"<{player.NameTag}>") },
					{ "cwssg:elo:entry:elo", new Tr_Inline($"-") },
					{ "cwssg:elo:entry:wins", new Tr_Inline($"-") },
					{ "cwssg:elo:entry:draws", new Tr_Inline($"-") },
					{ "cwssg:elo:entry:losses", new Tr_Inline($"-") },
					{ "cwssg:elo:entry:totalcount", new Tr_Inline($"-") }
				};
			}
			else {
				replacers = new() {
					{ "cwssg:elo:entry:player", new Tr_Inline($"<{player.NameTag}>") },
					{ "cwssg:elo:entry:elo", new Tr_Inline($"{elo.Elo}") },
					{ "cwssg:elo:entry:wins", new Tr_Inline($"{elo.Wins}") },
					{ "cwssg:elo:entry:draws", new Tr_Inline($"{elo.Draws}") },
					{ "cwssg:elo:entry:losses", new Tr_Inline($"{elo.Losses}") },
					{ "cwssg:elo:entry:totalcount", new Tr_Inline($"{elo.TotalMatches}") }
				};
			}

			return HtmlBuilder.Process(template, context, replacers);
		}

	}

}
