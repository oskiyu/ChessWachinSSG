using ChessWachinSSG.Data.Daos;
using ChessWachinSSG.Data.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChessWachinSSG.Model {

	public class HistoricalRanking {

		/// <summary>
		/// Clase auxiliar para la construcción
		/// del ranking.
		/// </summary>
		public class Builder {

			/// <returns>
			/// Ranking correctamente ordenado.
			/// </returns>
			public HistoricalRanking Build() {
				instance.Sort();
				return instance;
			}

			/// <summary>
			/// Procesa una partida, actualizando
			/// el ranking.
			/// </summary>
			/// <param name="matchData">Partida.</param>
			/// <returns>Self.</returns>
			private Builder ApplyMatch(Match matchData) {
				if (!instance.scores.ContainsKey(matchData.First.Id)) {
					instance.scores[matchData.First.Id] = new HistoricalRankingEntry(matchData.First);
				}

				if (!instance.scores.ContainsKey(matchData.Second.Id)) {
					instance.scores[matchData.Second.Id] = new HistoricalRankingEntry(matchData.Second);
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
			/// Procesa una competición, actualizando
			/// el ranking.
			/// </summary>
			/// <param name="matchData">Partida.</param>
			/// <returns>Self.</returns>
			public Builder ApplyCompetition(Competition competition) {
				// Nos aseguramos de que cada jugador tenga su entrada.
				foreach (var player in competition.InitialElos) {
					if (!instance.scores.ContainsKey(player.Player.Id)) {
						instance.scores[player.Player.Id] = new HistoricalRankingEntry(player.Player);
					}
				}

				// Aplicamos las partidas.
				ApplyAllMatches(competition.LeaguePhase?.Matches ?? new MatchList());
				ApplyAllMatches(competition.Playoffs?.Semifinals1?.Matches ?? new MatchList());
				ApplyAllMatches(competition.Playoffs?.Semifinals2?.Matches ?? new MatchList());
				ApplyAllMatches(competition.Playoffs?.Finals?.Matches ?? new MatchList());

				// Comprobamos medallas y títulos.
				if (competition.LeaguePhase != null && competition.LeaguePhase.IsFinished) {
					instance.scores[competition.LeaguePhase.Winner!.Id].Golds++;
					instance.scores[competition.LeaguePhase.Second!.Id].Silvers++;
					instance.scores[competition.LeaguePhase.Third!.Id].Bronzes++;
				}

				if (competition.Playoffs != null && competition.Playoffs.Finals != null) {
					instance.scores[competition.Playoffs.Winner!.Id].Titles++;
					instance.scores[competition.Playoffs.Winner!.Id].Golds++;
					instance.scores[competition.Playoffs.Second!.Id].Silvers++;
				}

				return this;
			}

			/// <summary>
			/// Se asegura de que aparezcan todos los jugadores, aunque no 
			/// hayan participado todavía.
			/// </summary>
			/// <param name="players">Lista con todos los jugadores.</param>
			/// <returns>Self.</returns>
			public Builder WithPlayers(List<Player> players) {
				foreach (Player player in players) {
					if (!instance.scores.ContainsKey(player.Id)) {
						instance.scores[player.Id] = new HistoricalRankingEntry(player);
					}
				}

				return this;
			}

			/// <summary>
			/// Procesa todas las partidas indicadas.
			/// </summary>
			/// <param name="matchList">Lista de partidas.</param>
			/// <returns>Self.</returns>
			private Builder ApplyAllMatches(MatchList matchList) {
				foreach (var match in matchList.GetAll()) {
					ApplyMatch(match);
				}

				return this;
			}

			private readonly HistoricalRanking instance = new();

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
		public HistoricalRankingEntry? GetPlayerInfo(string playerId) => scores.GetValueOrDefault(playerId);

		/// <summary>
		/// Información del jugador.
		/// </summary>
		/// <param name="player">Jugador.</param>
		/// <returns>Información, o null si no existe.</returns>
		public HistoricalRankingEntry? GetPlayerInfo(Player player) => scores.GetValueOrDefault(player.Id);


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

		public IReadOnlyList<HistoricalRankingEntry> Ranking { get => orderedRanking; }

		private List<HistoricalRankingEntry> orderedRanking = [];
		private readonly Dictionary<string, HistoricalRankingEntry> scores = [];

	}
}
