using ChessWachinSSG.Data.Dtos;
using System.Text.Json;

namespace ChessWachinSSG.Model {

	/// <summary>
	/// Clase que obtiene los ELOs actualizados de los
	/// jugadores en chess.com.
	/// </summary>
    public static class PlayerEloRequester {

		/// <summary>
		/// Obtiene los elos de un jugador.
		/// </summary>
		/// <param name="playerName">Nombre del jugador en chess.com.</param>
		/// <param name="playerId">ID del jugador.</param>
		/// <returns>ELOS (async).</returns>
		public static async Task<EloDto> GetPlayerElos(string playerName, string playerId) {
			var json = await GetElo(playerName);

			EloEntryDto? rapid = null;
			EloEntryDto? blitz = null;

			try {
				var rapidInfo = json.RootElement.GetProperty("chess_rapid");

				int elo = rapidInfo.GetProperty("last").GetProperty("rating").GetInt32();
				int wins = rapidInfo.GetProperty("record").GetProperty("win").GetInt32();
				int draws = rapidInfo.GetProperty("record").GetProperty("draw").GetInt32();
				int losses = rapidInfo.GetProperty("record").GetProperty("loss").GetInt32();

				rapid = new EloEntryDto(elo, wins, draws, losses);
			}
			catch {
				logger.Warn($"El jugador {playerName} no ha jugado partidas RAPID.");
			}

			try {
				var blitzInfo = json.RootElement.GetProperty("chess_blitz");

				int elo = blitzInfo.GetProperty("last").GetProperty("rating").GetInt32();
				int wins = blitzInfo.GetProperty("record").GetProperty("win").GetInt32();
				int draws = blitzInfo.GetProperty("record").GetProperty("draw").GetInt32();
				int losses = blitzInfo.GetProperty("record").GetProperty("loss").GetInt32();

				blitz = new EloEntryDto(elo, wins, draws, losses);
			}
			catch {
				logger.Warn($"El jugador {playerName} no ha jugado partidas BLITZ.");
			}

			return new(playerId, rapid, blitz);
		}

		/// <param name="playerName">Nombre del jugador en chess.com.</param>
		/// <returns>Respuesta del servidor.</returns>
		private static async Task<JsonDocument> GetElo(string playerName) {
			sharedClient.DefaultRequestHeaders.Add("User-Agent", "ChessWachinSSG");

			using HttpResponseMessage response = await sharedClient.GetAsync($"player/{playerName}/stats");

			response.EnsureSuccessStatusCode();

			var jsonResponse = await response.Content.ReadAsStringAsync();

			return JsonDocument.Parse(jsonResponse);
		}

		private static readonly HttpClient sharedClient = new() {
			BaseAddress = new Uri("https://api.chess.com/pub/"),
		};

		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	}

}
