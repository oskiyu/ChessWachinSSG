using ChessWachinSSG.Data.Dtos;

namespace ChessWachinSSG.Model {

    /// <summary>
    /// Ranking de jugadores, ordenados
    /// por puntos.
    /// </summary>
    public class PointsRanking {

		/// <summary>
		/// Clase auxiliar para la construcción
		/// del ranking.
		/// </summary>
		public class Builder {

			/// <returns>
			/// Ranking correctamente ordenado.
			/// </returns>
			public PointsRanking Build() {
				instance.Sort();
				return instance;
			}

			/// <summary>
			/// Procesa una partida, actualizando
			/// el ranking.
			/// </summary>
			/// <param name="matchData">Partida.</param>
			/// <returns>Self.</returns>
			public Builder ApplyMatch(Match matchData) {
				if (!instance.scores.ContainsKey(matchData.First.Id)) {
					instance.scores[matchData.First.Id] = new PlayerScores(matchData.First);
				}

				if (!instance.scores.ContainsKey(matchData.Second.Id)) {
					instance.scores[matchData.Second.Id] = new PlayerScores(matchData.Second);
				}

				instance.scores[matchData.First.Id].Wins += matchData.Result == Winner.First ? 1 : 0;
				instance.scores[matchData.First.Id].Draws += matchData.Result == Winner.Draw ? 1 : 0;
				instance.scores[matchData.First.Id].Losses += matchData.Result == Winner.Second ? 1 : 0;

				instance.scores[matchData.Second.Id].Wins += matchData.Result == Winner.Second ? 1 : 0;
				instance.scores[matchData.Second.Id].Draws += matchData.Result == Winner.Draw ? 1 : 0;
				instance.scores[matchData.Second.Id].Losses += matchData.Result == Winner.First ? 1 : 0;

				return this;
			}

			/// <summary>
			/// Procesa todas las partidas indicadas.
			/// </summary>
			/// <param name="matchList">Lista de partidas.</param>
			/// <returns>Self.</returns>
			public Builder ApplyAllMatches(MatchList matchList) {
				foreach (var match in matchList.GetAll()) {
					ApplyMatch(match);
				}

				return this;
			}

			private readonly PointsRanking instance = new();

		}

		/// <summary>
		/// Ordena los jugadores según
		/// la puntuación.
		/// </summary>
		private void Sort() {
			orderedRanking = [.. scores.Values];
			orderedRanking.Sort();
		}

		
		/// <summary>
		/// Información del jugador.
		/// </summary>
		/// <param name="playerId">ID del jugador.</param>
		/// <returns>Información, o null si no existe.</returns>
		public PlayerScores? GetPlayerInfo(string playerId)	=> scores.GetValueOrDefault(playerId);

		/// <summary>
		/// Información del jugador.
		/// </summary>
		/// <param name="player">Jugador.</param>
		/// <returns>Información, o null si no existe.</returns>
		public PlayerScores? GetPlayerInfo(Player player)	=> scores.GetValueOrDefault(player.Id);


		/// <param name="playerId"></param>
		/// <returns>
		/// Clasificación del jugador (siendo 0 el primero),
		/// o -1 si no existe.
		/// </returns>
		public int GetPlayerPosition(string playerId)
			=> orderedRanking.FindIndex(x => x.Player.Id == playerId);

		/// <param name="player">Jugador.</param>
		/// <returns>
		/// Clasificación del jugador (siendo 0 el primero),
		/// o -1 si no existe.
		/// </returns>
		public int GetPlayerPosition(Player player)
			=> orderedRanking.FindIndex(x => x.Player == player);

		public IReadOnlyList<PlayerScores> Ranking { get => orderedRanking; }

		private List<PlayerScores> orderedRanking = [];
		private readonly Dictionary<string, PlayerScores> scores = [];

	}

}
