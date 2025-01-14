using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

using System.Text;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Replacer para insretar las medallas y los trofeos
	/// conseguidos por un jugador en su tarjeta de perfil.
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	/// <param name="player">Jugador.</param>
	/// <param name="colorClass">Clase de color del texto de la tarjeta.</param>
	public class Tr_ProfileLogros(IFileReader reader, Player player, string colorClass) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var cups = 0;
			var golds = 0;
			var silvers = 0;
			var bronzes = 0;
			foreach (var (key, competition) in context.Competitions) {
				if (competition.LeaguePhase != null && competition.LeaguePhase.IsFinished) {
					golds += competition.LeaguePhase.Winner == player ? 1 : 0;
					silvers += competition.LeaguePhase.Second == player ? 1 : 0;
					bronzes += competition.LeaguePhase.Third == player ? 1 : 0;
				}

				if (competition.Playoffs != null) {
					cups += competition.Playoffs.Winner == player ? 1 : 0;
					golds += competition.Playoffs.Winner == player ? 1 : 0;
					silvers += competition.Playoffs.Second == player ? 1 : 0;
				}
			}

			StringBuilder output = new();

			output.Append(new Tr_ProfileCardTitles(reader, colorClass, cups).Replace(Tag.Empty, context));
			output.Append(new Tr_ProfileCardGolds(reader, colorClass, golds).Replace(Tag.Empty, context));
			output.Append(new Tr_ProfileCardSilvers(reader, colorClass, silvers).Replace(Tag.Empty, context));
			output.Append(new Tr_ProfileCardBronzes(reader, colorClass, bronzes).Replace(Tag.Empty, context));

			return output.ToString();
		}

	}

}
