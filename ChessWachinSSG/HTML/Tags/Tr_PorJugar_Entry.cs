using ChessWachinSSG.Data.Dtos;
using ChessWachinSSG.Data;
using ChessWachinSSG.Model;
using System.Text;

namespace ChessWachinSSG.HTML.Tags {
	
	public class Tr_PorJugar_Entry(IFileReader reader, Player jugador, League league) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/partidas_por_jugar_entry.html").ReadToEnd();

			var entries = new StringBuilder();

			var matches = league.Matches.GetAll();
			foreach (var playerElo in context.Competitions[league.CompetitionId].InitialElos) {
				var player = playerElo.Player;
				if (player == jugador) {
					continue;
				}

				bool hasWhitesMatch = matches.Any(x => x.First == jugador && x.Second == player);
				bool hasBlacksMatch = matches.Any(x => x.Second == jugador && x.First == player);

				if (hasWhitesMatch && hasBlacksMatch) {
					continue;
				}

				var subentriesForPlayer = new StringBuilder();

				if (!hasWhitesMatch) {
					subentriesForPlayer.Append(new Tr_PorJugar_Subentry(reader, player, "Blancas").Replace(Tag.Empty, context));
				}

				if (!hasBlacksMatch) {
					subentriesForPlayer.Append(new Tr_PorJugar_Subentry(reader, player, "Negras").Replace(Tag.Empty, context));
				}

				entries.Append(subentriesForPlayer);
				entries.Append("<br>");
			}

			if (entries.Length == 0) {
				return string.Empty;
			}

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:partidasporjugar:name:main", new Tr_Inline($"<{jugador.NameTag}>") },
				{ "cwssg:partidasporjugar:subentries", new Tr_Inline($"{entries}") }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

	}

}
