using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por la tabla de
	/// clasificación de una liga.
	/// 
	/// El ID de la liga debe estar en el
	/// primer argumento del tag.
	/// </summary>
	/// <param name="reader">Clase lectora de archivos externos.</param>
	public class Tr_LeagueRanking(IFileReader reader) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			if (tag.Arguments.Count == 0) {
				logger.Error($"No se encuentra el argumento leagueId.");
				return string.Empty;
			}

			var league = context.GetLeague(tag.Arguments[0]);

			if (league == null) {
				logger.Error($"No se encuentra la liga {league}.");
				return string.Empty;
			}

			var table = reader.GetStream("Sources/league_table.html").ReadToEnd();

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:league:rankingentries", new Tr_LeagueRankingEntries(league) }
			};
						
			return HtmlBuilder.Process(table, context, replacers.AsReadOnly());
		}

		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	}

}
