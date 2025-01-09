namespace ChessWachinSSG.Model {

	/// <summary>
	/// Información sobre una fase de liga.
	/// </summary>
	public record class League(string Id, MatchList Matches, int NumQualifications, string CompetitionId) {

		/// <summary>
		/// Ranking de la fase de liga.
		/// </summary>
		public PointsRanking Ranking { get; private init; } = new PointsRanking.Builder().ApplyAllMatches(Matches).Build();


		/// <summary>
		/// Ganador de la fase de liga, si está completa.
		/// </summary>
		public Player? Winner { get => Ranking.Ranking.Count > 0 ? Ranking.Ranking[0].Player : null; }

		/// <summary>
		/// Segundo de la fase de liga, si está completa.
		/// </summary>
		public Player? Second { get => Ranking.Ranking.Count > 1 ? Ranking.Ranking[1].Player : null; }

		/// <summary>
		/// Tercero de la fase de liga, si está completa.
		/// </summary>
		public Player? Third { get => Ranking.Ranking.Count > 2 ? Ranking.Ranking[2].Player : null; }


		/// <param name="player">Jugador.</param>
		/// <returns>
		/// True si el primer clasificado de la liga es el jugador indicado.
		/// Si player es null, o la competición no contiene al jugador, devuelve false.</returns>
		public bool IsPlayerFirst(Player? player) => Winner == player;

		/// <param name="player">Jugador.</param>
		/// <returns>
		/// True si el segundo clasificado de la liga es el jugador indicado.
		/// Si player es null, o la competición no contiene al jugador, devuelve false.</returns>
		public bool IsPlayerSecond(Player? player) => Second == player;

		/// <param name="player">Jugador.</param>
		/// <returns>
		/// True si el tercer clasificado de la liga es el jugador indicado.
		/// Si player es null, o la competición no contiene al jugador, devuelve false.</returns>
		public bool IsPlayerThird(Player? player) => Third == player;


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
