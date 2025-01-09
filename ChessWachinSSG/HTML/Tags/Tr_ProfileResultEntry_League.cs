using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {
	
	/// <summary>
	/// Replacer para el resultado de un jugador
	/// en una fase de liga.
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	/// <param name="player">Jugador.</param>
	/// <param name="league">Liga.</param>
	public class Tr_ProfileResultEntry_League(IFileReader reader, Player player, League? league) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/profile_result_entry_league.html").ReadToEnd();

			int? position = league?.Ranking.GetPlayerPosition(player);
			string positionStr = position is not null ? $"{position + 1}" : "-";

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:profile:result:leagueclass", new Tr_Inline(Globals.GetRankingColorClass(position + 1 ?? int.MaxValue)) },
				{ "cwssg:profile:result:league:position", new Tr_Inline(positionStr) },
				{ "cwssg:profile:result:league:medal", new Tr_Inline(Globals.GetLeagueMedal(position ?? int.MaxValue, league?.IsFinished ?? false)) }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

	}

}
