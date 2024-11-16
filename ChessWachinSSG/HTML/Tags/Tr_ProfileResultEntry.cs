using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Cambia el tag por el resultado de
	/// un jugador en una competición en concreto.
	/// </summary>
	/// <param name="reader">Clase lectora de archivos.</param>
	/// <param name="player">Jugador.</param>
	/// <param name="competition">Competición.</param>
	public class Tr_ProfileResultEntry(IFileReader reader, Player player, Competition competition) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			// Comprobar si el jugador participó en la
			// competición en cuestión.
			if ((competition.LeaguePhase?.Ranking.GetPlayerPosition(player) ?? -1) == -1) {
				return string.Empty;
			}

			var template = reader.GetStream("Sources/profile_results_entry.html").ReadToEnd();

			// Ganó si ganó los playoffs.
			var globalResult = GetGlobalResult();
			var leagueResult = GetLeagueResult();
			var playoffsResult = GetPlayoffsResult();			

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:profile:results:entry:cname", new Tr_Inline(competition.Name) },
				{ "cwssg:profile:results:entry:cup", new Tr_Inline(globalResult) },
				{ "cwssg:profile:results:entry:result:league", new Tr_Inline(leagueResult) },
				{ "cwssg:profile:results:entry:result:playoffs", new Tr_Inline(playoffsResult) }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

		/// <returns>
		/// Devuelve una copa + "1º" si el jugador ganó
		/// la competición.
		/// </returns>
		private string GetGlobalResult() 
			=> competition.Playoffs?.Winner == player ? $"{Globals.Cup} 1º" : string.Empty;

		/// <returns>
		/// Posición en liga del jugador, que puede
		/// contener medalla si queda 1º, 2º o 3º.
		/// </returns>
		private string GetLeagueResult() {
			var output = string.Empty;

			if (competition.LeaguePhase != null) {
				var leaguePosition = competition.LeaguePhase.Ranking.GetPlayerPosition(player);

				output = leaguePosition switch {
					0 => $"{Globals.GoldMedal} 1º",
					1 => $"{Globals.SilverMedal} 2º",
					2 => $"{Globals.BronzeMedal} 3º",
					_ => $"{leaguePosition + 1}º"
				};
			}

			return output;
		}

		/// <returns>
		/// Resultado en playoffs, que puede ser 1º,
		/// 2º o "Cae en semifinales".
		/// </returns>
		private string GetPlayoffsResult() {
			var output = string.Empty;

			if (competition.Playoffs != null) {
				bool isInFinal = competition.Playoffs?.Finals?.Ranking.Ranking.Any(x => x.Player == player) ?? false;

				if (isInFinal) {
					output = competition.Playoffs!.Winner == player
						? $"{Globals.GoldMedal} 1º"
						: $"{Globals.SilverMedal} 2º";
				}
				else {
					if (competition.LeaguePhase?.IsPlayerQualified(player) ?? false) {
						output = "Cae en semifinales";
					}
					else {
						output = "No clasificado";
					}
				}
			}

			return output;
		}

	}
}
