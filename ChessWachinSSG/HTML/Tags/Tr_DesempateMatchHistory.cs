using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

using System.Text;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por un historial de partidas de desempate.
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	public class Tr_DesempateMatchHistory(IFileReader reader) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var leagueId = tag.Arguments[0]!;
			var league = context.GetLeague(leagueId);

			if (league == null) {
				logger.Error($"No se encuentra la liga {leagueId}.");
				return string.Empty;
			}

			if (league.DesempateMatches == null) {
				return string.Empty;
			}

			var htmlTable = reader.GetStream("Sources/league_desempate_history_table.html").ReadToEnd();

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:league:history:desempate:entries", new Tr_Inline(GetDesempateMatches(league, context)) }
			};

			return HtmlBuilder.Process(htmlTable, context, replacers);
		}

		string GetDesempateMatches(League league, Context context) {
			if (league.DesempateMatches == null) {
				return string.Empty;
			}

			var output = new StringBuilder();

			foreach (var match in GetMatchesByDate(league.DesempateMatches)) {
				output.Append(match.Replace(Tag.Empty, context));
			}

			return output.ToString();
		}

		IEnumerable<Tr_MatchesDate> GetMatchesByDate(MatchList list) {
			var lastDate = string.Empty;

			List<Match> output = [];

			foreach (var match in list.GetAll()) {
				var date = match.Date;

				if (date != lastDate) {
					if (output.Count > 0) {
						yield return new Tr_MatchesDate(reader, lastDate, output);

						output = [];
					}
					lastDate = date;
				}

				output.Add(match);
			}

			if (output.Count > 0) {
				yield return new Tr_MatchesDate(reader, lastDate, output);
			}
		}

		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	}

}
