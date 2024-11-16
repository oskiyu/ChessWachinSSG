namespace ChessWachinSSG.Model {

	/// <summary>
	/// Información sobre una fase de liga.
	/// </summary>
	public record class League(string Id, MatchList Matches, int NumQualifications) {

		/// <summary>
		/// Ranking de la fase de liga.
		/// </summary>
		public PointsRanking Ranking { get; private init; } = new PointsRanking.Builder().ApplyAllMatches(Matches).Build();


		/// <summary>
		/// Ganador de la fase de liga, si está completa.
		/// </summary>
		public Player Winner { get => Ranking.Ranking[0].Player; }

		/// <summary>
		/// Segundo de la fase de liga, si está completa.
		/// </summary>
		public Player Second { get => Ranking.Ranking[1].Player; }

		/// <summary>
		/// Tercero de la fase de liga, si está completa.
		/// </summary>
		public Player Third { get => Ranking.Ranking[2].Player; }


		/// <summary>
		/// Comprueba si el jugador está clasificado.
		/// </summary>
		/// <param name="player">Jugador.</param>
		/// <returns>True si está clasificado para playoffs, false en caso contrario.</returns>
		public bool IsPlayerQualified(Player player) => Ranking.GetPlayerPosition(player) < NumQualifications;

		/// <summary>
		/// True si la liga está terminada.
		/// </summary>
		public bool IsFinished { get => Matches.GetAll().Count == GetNumMatches(Ranking.Ranking.Count); }


		/// <summary>
		/// Número total de partidas a disputar en
		/// toda la liga.
		/// </summary>
		/// <param name="playerCount">Número de jugadores que participan.</param>
		/// <returns>Número total de partidas.</returns>
		public static int GetNumMatches(int playerCount) {
			int output = 0;

			for (int i = (playerCount - 1) * 2; i >= 0; i -= 2) {
				output += i;
			}

			return output;
		}

	}

}
