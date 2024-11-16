using ChessWachinSSG.Data.Dtos;
using ChessWachinSSG.Model;

using System.Text;

namespace ChessWachinSSG.HTML.Tags {

    public static class MatchesSquaresGenerator {

		/// <summary>
		/// Genera un string con cuadrados de colores,
		/// indicando los resultados del jugador en la competición
		/// indicada.
		/// </summary>
		/// <param name="player">Jugador.</param>
		/// <param name="competition">Competición.</param>
		/// <returns>String.</returns>
		public static string Generate(Player player, Competition competition) {
			var output = new StringBuilder();

			var matchesWithPlayer = competition.AllMatches.GetAll().Where(x => x.First == player || x.Second == player);
			foreach (var match in matchesWithPlayer) {
				if (match.First == player) {
					output.Append(match.Result switch {
						Winner.First => Globals.GreenBox,
						Winner.Draw => Globals.YellowBox,
						Winner.Second => Globals.RedBox
					});
				}
				else if (match.Second == player) {
					output.Append(match.Result switch {
						Winner.First => Globals.RedBox,
						Winner.Draw => Globals.YellowBox,
						Winner.Second => Globals.GreenBox
					});
				}
			}

			return output.ToString();
		}

	}

}
