using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por los records personales
	/// de un jugador.
	/// </summary>
	/// <param name="reader">Clase lectora de archivos externos.</param>
	/// <param name="player">Jugador.</param>
	public class Tr_ProfileRecords(IFileReader reader, Player player) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/profile_records.html").ReadToEnd();
			var records = context.PlayerRecords[player.Id];

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:profile:records:winstreak", new Tr_Inline($"{records.MaxWinStreak} {(records.IsWinStreakActive ? "(en curso)" : "")}") },
				{ "cwssg:profile:records:undefeatedstreak", new Tr_Inline($"{records.MaxUndefeatedStreak} {(records.IsUndefeatedStreakActive ? "(en curso)" : "")}") },
				{ "cwssg:profile:records:lossstreak", new Tr_Inline($"{records.MaxLossStreak} {(records.IsLossStreakActive ? "(en curso)" : "")}") },

				{ "cwssg:profile:records:maxleaguepoints", new Tr_Inline($"{records.MaxLeaguePoints} ({records.MaxLeaguePointsCompetition?.Name ?? ""})") },
				{ "cwssg:profile:records:maxleaguewins", new Tr_Inline($"{records.MaxLeagueWins} ({records.MaxLeagueWinsCompetition?.Name ?? ""})") }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}
	}
}
