using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por los
	/// elos de los jugadores al inicio del torneo.
	/// </summary>
	/// <param name="reader">Clase lectora de archivos externos.</param>
	public class Tr_InitialEloTable(IFileReader reader) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var competitionId = tag.Arguments[0];
			var competition = context.GetCompetition(competitionId);

			if (competition == null) {
				logger.Error($"No se encuentra la competición {competitionId}.");
				return string.Empty;
			}

			var template = reader.GetStream("Sources/initial_elo_table.html").ReadToEnd();
			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:competition:initialelo:entries", new Tr_InitialEloEntries(reader, competition) }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	}

}
