using ChessWachinSSG.Model;

using System.Text;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por las posiciones de todos los jugadores.
	/// </summary>
	/// <param name="league">Liga.</param>
	public class Tr_LeagueRankingEntries(League league) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var output = new StringBuilder();

			HashSet<Player> addedPlayers = [];
			foreach (var playerScore in league.Ranking.Ranking) {
				output.Append(new Tr_LeagueRankingEntry(league, playerScore).Replace(Tag.Empty, context));
				addedPlayers.Add(playerScore.Player);
			}

			foreach (var playerElo in context.Competitions[league.CompetitionId].InitialElos) {
				if (!addedPlayers.Contains(playerElo.Player)) {
					output.Append(new Tr_LeagueRankingEntry(league, new PlayerScores(playerElo.Player)).Replace(Tag.Empty, context));
				}
			}

			return output.ToString();
		}

	}

}
