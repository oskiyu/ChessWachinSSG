namespace ChessWachinSSG.Model {

	/// <summary>
	/// Puntuación de un jugador en una fase de competición.
	/// </summary>
	/// <param name="Player">Jugador.</param>
	public record class PlayerScores(Player Player) : IComparable<PlayerScores> {

		/// <summary>
		/// Puntos totales.
		/// </summary>
		public int Points { get { return Wins * 2 + Draws * 1; } }

		public int Wins { get; set; } = 0;
		public int Draws { get; set; } = 0;
		public int Losses { get; set; } = 0;

		public int CompareTo(PlayerScores? other) {
			return -(Points - other?.Points ?? 0);
		}

	}

}
