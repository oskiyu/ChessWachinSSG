using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por el contenido de un
	/// perfil de un jugador.
	/// 
	/// El ID del jugador debe estar en el primer
	/// parámetro del tag.
	/// 
	/// Dependencias:
	/// - Tr_Inline
	/// - Tr_ProfileResults
	/// - Tr_ProfileRecords
	/// - ProfileHistoryTagReplacer
	/// - Tr_ProfileMatchupsTable
	/// </summary>
	/// <param name="reader"></param>
	public class Tr_Profile(IFileReader reader) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			if (tag.Arguments.Count == 0) {
				logger.Error($"El tag {tag} no contiene el argumento playerId.");
				return string.Empty;
			}

			var playerId = tag.Arguments[0];

			if (!context.Players.ContainsKey(playerId)) {
				logger.Error($"El jugador {playerId} no se encuentra.");
				return string.Empty;
			}

			var player = context.Players[playerId];

			var template = reader.GetStream("Sources/profile.html").ReadToEnd();
			var globalRanking = context.HistoricalRanking;

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:profile:name", new Tr_Inline(player.Name) },
				{ "cwssg:profile:pfp", new Tr_Inline(player.PfpPath ?? "assets/defaultpfp.svg") },

				{ "cwssg:profile:globalposition", new Tr_Inline($"{globalRanking.GetPlayerPosition(player) + 1}") },

				{ "cwssg:profile:wins", new Tr_Inline($"{globalRanking.GetPlayerInfo(player)?.Wins ?? 0}") },
				{ "cwssg:profile:draws", new Tr_Inline($"{globalRanking.GetPlayerInfo(player)?.Draws ?? 0}") },
				{ "cwssg:profile:losses", new Tr_Inline($"{globalRanking.GetPlayerInfo(player)?.Losses ?? 0}") },

				{ "cwssg:profile:results", new Tr_ProfileResults2(reader, player) },
				{ "cwssg:profile:records", new Tr_ProfileRecords(reader, player) },
				{ "cwssg:profile:history", new Tr_ProfileHistory(reader, player) },
				{ "cwssg:profile:matchups", new Tr_ProfileMatchupsTable(reader, player) },

				{ "cwssg:profile:card", new Tr_PlayerCard(reader, player) }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	}

}
