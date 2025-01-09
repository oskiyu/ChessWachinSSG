using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {
	
	/// <summary>
	/// Replacer para el resultado de un jugador
	/// en una fase de playoffs.
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	/// <param name="player">Jugador.</param>
	/// <param name="playoffs">Playoffs.</param>
	public class Tr_ProfileResultEntry_Playoffs(IFileReader reader, Player player, Playoffs? playoffs) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/profile_result_entry_playoffs.html").ReadToEnd();

			string position = "-";

            if (playoffs is not null) {
				if (playoffs.Winner == player) {
					position = "1";
				}
				else if (playoffs.Second == player) {
					position = "2";
				}
				else if (playoffs.IsPlayerQualified(player)) {
					position = "3º/4";
				}
			}

			string positionClass = position switch {
				"1" => Globals.GetRankingColorClass(1),
				"2" => Globals.GetRankingColorClass(2),
				_ => Globals.GetRankingColorClass(int.MaxValue)
			};

			string medal = position switch {
				"1" => Globals.GoldMedal,
				"2" => Globals.SilverMedal,
				_ => string.Empty
			};

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:profile:result:playoffsclass", new Tr_Inline(positionClass) },
				{ "cwssg:profile:result:playoffs:position", new Tr_Inline(position) },
				{ "cwssg:profile:result:playoffs:medal", new Tr_Inline(medal) }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

	}

}
