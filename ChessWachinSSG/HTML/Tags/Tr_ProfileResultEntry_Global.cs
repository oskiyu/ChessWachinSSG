using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {
	
	/// <summary>
	/// Replacer para la posición final de un jugador en una competición.
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	/// <param name="player">Jugador.</param>
	/// <param name="competition">Competición.</param>
	public class Tr_ProfileResultEntry_Global(IFileReader reader, Player player, Competition competition) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/profile_result_entry_global.html").ReadToEnd();

			string cup = competition.Playoffs?.Finals?.Ranking.GetPlayerPosition(player) == 0 ? Globals.Cup : string.Empty;

			int positionInt = int.MaxValue;
			string position = "-";
			if (competition.LeaguePhase is not null) {
				if (competition.LeaguePhase.IsPlayerQualified(player)) {
					// En playoffs

					if (competition.Playoffs?.Winner == player) {
						position = "1";
						positionInt = 0;
					}
					else if (competition.Playoffs?.Second == player) {
						position = "2";
						positionInt = 1;
					}
					else {
						position = $"{competition.LeaguePhase.Ranking.GetPlayerPosition(player) + 1}";
						positionInt = competition.LeaguePhase.Ranking.GetPlayerPosition(player);
					}
				}
                else {
					// Eliminado
					position = $"{competition.LeaguePhase.Ranking.GetPlayerPosition(player) + 1}";
					positionInt = competition.LeaguePhase.Ranking.GetPlayerPosition(player);
				}
            }

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:profile:result:globalresultclass", new Tr_Inline(Globals.GetRankingColorClass(positionInt + 1)) },
				{ "cwssg:profile:result:global:position", new Tr_Inline(position) },
				{ "cwssg:profile:result:global:cup", new Tr_Inline(cup) }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

	}

}
