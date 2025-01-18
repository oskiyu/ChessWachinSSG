using ChessWachinSSG.Data.Dtos;
using ChessWachinSSG.Data;
using ChessWachinSSG.Model;
using System.Text;
using NLog;

namespace ChessWachinSSG.HTML.Tags {
	
	public class Tr_PorJugar(IFileReader reader) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/partidas_por_jugar.html").ReadToEnd();
			
			if (tag.Arguments.Count == 0) {
				logger.Error($"No se encuentra el argumento leagueId.");
				return string.Empty;
			}

			var league = context.GetLeague(tag.Arguments[0]);

			if (league == null) {
				logger.Error($"No se encuentra la liga {league}.");
				return string.Empty;
			}

			var entries = new StringBuilder();

			foreach (var playerElo in context.Competitions[league.CompetitionId].InitialElos) {
				var player = playerElo.Player;

				entries.Append(new Tr_PorJugar_Entry(reader, player, league).Replace(Tag.Empty, context));
			}

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:partidasporjugar:entries", new Tr_Inline($"{entries}") }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	}

}
