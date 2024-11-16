using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

using System.Text;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por un historial de partidas.
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	public class Tr_MatchHistory(IFileReader reader) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var output = new StringBuilder();

			var leagueId = tag.Arguments[0]!;
			var league = context.GetLeague(leagueId);

			if (league == null) {
				logger.Error($"No se encuentra la liga {leagueId}.");
				return string.Empty;
			}

			var htmlTable = reader.GetStream("Sources/league_history_table.html").ReadToEnd();

			var lastDate = string.Empty;

			var matchesWithDate = new List<Match>();
			foreach (var match in league.Matches.GetAll()) {
				var date = match.Date;

				if (date != lastDate) {
					if (matchesWithDate.Count > 0) {
						output.Append(new Tr_MatchesDate(reader, lastDate, matchesWithDate).Replace(Tag.Empty, context));
						matchesWithDate = [];
					}
					lastDate = date;
				}

				matchesWithDate.Add(match);
			}

			if (matchesWithDate.Count > 0) {
				output.Append(new Tr_MatchesDate(reader, lastDate, matchesWithDate).Replace(Tag.Empty, context));
			}

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:league:history:entries", new Tr_Inline(output.ToString()) }
			};

			return HtmlBuilder.Process(htmlTable, context, replacers);
		}

		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	}

}
