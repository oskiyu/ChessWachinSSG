using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

using System.Diagnostics.Contracts;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por la tabla de estadísticas
	/// de una competición en concreto.
	/// 
	/// El ID de la competición debe ser el primer
	/// argumento del tag.
	/// </summary>
	/// <param name="reader">Clase lectora de archivos.</param>
	public class Tr_CompetitionStatsTable(IFileReader reader) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			if (tag.Arguments.Count == 0) {
				logger.Error("No se encuentra el argumento competitionId.");
				return string.Empty;
			}

			var tempalte = reader.GetStream("Sources/competition_stats_table.html").ReadToEnd();
			var competitionId = tag.Arguments[0];

			var competition = context.GetCompetition(competitionId);

			if (competition == null) {
				logger.Error($"No se encuentra la competición {competitionId}.");
				return string.Empty;
			}

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:competition:stats:matches", new Tr_Inline(competition.Stats.MatchCount.ToString()) },
				{ "cwssg:competition:stats:whites", new Tr_Inline(competition.Stats.WhiteWins.ToString()) },
				{ "cwssg:competition:stats:draws", new Tr_Inline(competition.Stats.Draws.ToString()) },
				{ "cwssg:competition:stats:blacks", new Tr_Inline(competition.Stats.BlackWins.ToString()) },

				{ "cwssg:competition:stats:avgmoves", new Tr_Inline(competition.Stats.AverageMoveCount.ToString()) },
				{ "cwssg:competition:stats:avgduration", new Tr_Inline(Globals.ToMinutesSeconds(competition.Stats.AverageGameDuration)) },
				{ "cwssg:competition:stats:avgmoveduration", new Tr_Inline($"{competition.Stats.AverageMoveDuration} segundos") }
			};

			return HtmlBuilder.Process(tempalte, context, replacers);
		}

		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	}

}
