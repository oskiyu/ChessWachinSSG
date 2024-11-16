using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por la posición
	/// de un jugador en el ranking de una liga.
	/// </summary>
	/// <param name="league">Liga.</param>
	/// <param name="player">Jugador.</param>
	public class Tr_LeagueRankingEntry(League league, PlayerScores player) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = File.ReadAllText("Sources/league_table_entry.html");

			string medal = Globals.GetLeagueMedal(league.Ranking.GetPlayerPosition(player.Player), league.IsFinished);

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:league:playername",    new Tr_Inline($"<{player.Player.NameTag}> {medal}") },
				{ "cwssg:league:playerpoints",  new Tr_Inline(player.Points.ToString()) },
				{ "cwssg:league:playerwins",    new Tr_Inline(player.Wins.ToString()) },
				{ "cwssg:league:playerdraws",   new Tr_Inline(player.Draws.ToString()) },
				{ "cwssg:league:playerlosses",  new Tr_Inline(player.Losses.ToString()) },
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

	}
}
