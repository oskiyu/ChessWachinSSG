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
			StringBuilder output = new();

			var playerInfo = context.HistoricalRanking.GetPlayerInfo(player);

			if (playerInfo == null) {
				logger.Error($"El jugador {player.Id} no se encuentra en el ranking histórico.");
				return string.Empty;
			}

			output.Append(new Tr_ProfileCardTitles(reader, colorClass, playerInfo.Titles).Replace(Tag.Empty, context));
			output.Append(new Tr_ProfileCardGolds(reader, colorClass, playerInfo.Golds).Replace(Tag.Empty, context));
			output.Append(new Tr_ProfileCardSilvers(reader, colorClass, playerInfo.Silvers).Replace(Tag.Empty, context));
			output.Append(new Tr_ProfileCardBronzes(reader, colorClass, playerInfo.Bronzes).Replace(Tag.Empty, context));

			return output.ToString();
		}

		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	}

}
