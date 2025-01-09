using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Replacer para la tarjeta del perfil,
	/// con el resumen del jugador y sus logros.
	/// </summary>
	/// <param name="reader"></param>
	/// <param name="player"></param>
	public class Tr_PlayerCard(IFileReader reader, Player player) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/profile_card.html").ReadToEnd();

			var rankingPosition = context.HistoricalRanking.GetPlayerPosition(player) + 1;
			var rankingColorClass = Globals.GetRankingColorClass(rankingPosition);

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:player:card:country", new Tr_Inline(player.Country.PlayerCardClass) },
				{ "cwssg:player:card:pfp", new Tr_Inline($"/{player.PfpPath?? "assets/defaultpfp.svg"}" ) },
				{ "cwssg:player:name", new Tr_Inline(player.Name) },
				{ "cwssg:player:card:ranking", new Tr_Inline($"{rankingPosition}" )},
				{ "cwssg:player:card:rankingcolorclass", new Tr_Inline(rankingColorClass) },
				{ "cwssg:player:card:logros", new Tr_ProfileLogros(reader, player, rankingColorClass) }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

	}

}
