using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

using System.Text;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por un jugador en el 
	/// ranking global.
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	public class Tr_HistoricalRankingEntry(IFileReader reader) : ITagReplacer {

		public const string Id = "cwssg:historical:table:players";

		public string Replace(Tag tag, Context context) {
			var output = new StringBuilder();

			foreach (var playerScore in context.HistoricalRanking.Ranking) {

				var playerData = context.HistoricalRanking.GetPlayerInfo(playerScore.Player.Id);

				if (playerData == null) {
					logger.Error($"No se encuentra el jugador {playerScore.Player.Id}.");
					return string.Empty;
				}

				var template = reader.GetStream("Sources/historical_table_entry.html").ReadToEnd();

				var cups = 0;
				var golds = 0;
				var silvers = 0;
				var bronzes = 0;
				foreach (var (_, competition) in context.Competitions) {
					if (competition.LeaguePhase != null && competition.LeaguePhase.IsFinished) {
						golds += competition.LeaguePhase.IsPlayerFirst(playerScore.Player) ? 1 : 0;
						silvers += competition.LeaguePhase.IsPlayerSecond(playerScore.Player) ? 1 : 0;
						bronzes += competition.LeaguePhase.IsPlayerThird(playerScore.Player) ? 1 : 0;
					}

					if (competition.Playoffs != null) {
						cups += competition.Playoffs.Winner == playerScore.Player ? 1 : 0;
						golds += competition.Playoffs.Winner == playerScore.Player ? 1 : 0;
						silvers += competition.Playoffs.Second == playerScore.Player ? 1 : 0;
					}
				}

				var titlesBuilder = new StringBuilder();
				if (cups > 0) {
					titlesBuilder.Append(Globals.Cup);
					titlesBuilder.Append($" x{cups}");
				}

				var medalsBuilder = new StringBuilder();
				if (golds > 0) {
					medalsBuilder.Append(Globals.GoldMedal);
					medalsBuilder.Append($" x{golds} ");
				}

				if (silvers > 0) {
					medalsBuilder.Append(Globals.SilverMedal);
					medalsBuilder.Append($" x{silvers} ");
				}

				if (bronzes > 0) {
					medalsBuilder.Append(Globals.BronzeMedal);
					medalsBuilder.Append($" x{bronzes}");
				}

				var replacers = new Dictionary<string, ITagReplacer> {
					["cwssg:htable:name"] = new Tr_Inline($"<{playerScore.Player.NameTag}>"),
					["cwssg:htable:titles"] = new Tr_Inline(titlesBuilder.ToString()),
					["cwssg:htable:medals"] = new Tr_Inline(medalsBuilder.ToString()),
					["cwssg:htable:points"] = new Tr_Inline($"{playerData.Points}"),
					["cwssg:htable:wins"] = new Tr_Inline($"{playerData.Wins}"),
					["cwssg:htable:draws"] = new Tr_Inline($"{playerData.Draws}"),
					["cwssg:htable:losses"] = new Tr_Inline($"{playerData.Losses}"),
				};

				output.Append(HtmlBuilder.Process(template, context, replacers));
			}

			return output.ToString();
		}

		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	}

}
