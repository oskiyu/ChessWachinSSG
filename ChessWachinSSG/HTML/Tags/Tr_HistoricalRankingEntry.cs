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

				var titlesBuilder = new StringBuilder();
				if (playerData.Titles > 0) {
					titlesBuilder.Append(Globals.Cup);
					titlesBuilder.Append($" x{playerData.Titles}");
				}

				var medalsBuilder = new StringBuilder();
				if (playerData.Golds > 0) {
					medalsBuilder.Append(Globals.GoldMedal);
					medalsBuilder.Append($" x{playerData.Golds} ");
				}

				if (playerData.Silvers > 0) {
					medalsBuilder.Append(Globals.SilverMedal);
					medalsBuilder.Append($" x{playerData.Silvers} ");
				}

				if (playerData.Bronzes > 0) {
					medalsBuilder.Append(Globals.BronzeMedal);
					medalsBuilder.Append($" x{playerData.Bronzes}");
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
