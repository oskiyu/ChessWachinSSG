using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

using System.Text;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag con un historial de partidas.
	/// 
	/// TODO: mergear con Tr_MatchHistory.
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	public class Tr_PlayoffPhaseHistory(IFileReader reader) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var output = new StringBuilder();

			var phaseId = tag.Arguments[0]!;
			var phase = context.GetPlayoffsRound(phaseId);

			if (phase == null) {
				logger.Error($"No se encuentra la fase de playoffs {phaseId}.");
				return string.Empty;
			}

			var htmlTable = reader.GetStream("Sources/playoffs_history_table.html").ReadToEnd();

			var lastDate = string.Empty;

			var matchesWithDate = new List<Match>();
			foreach (var match in phase.Matches.GetAll()) {
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
				{ "cwssg:playoffs:history:entries", new Tr_Inline(output.ToString()) }
			};

			return HtmlBuilder.Process(htmlTable, context, replacers);
		}

		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	}

}
