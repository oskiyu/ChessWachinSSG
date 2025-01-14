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
			if (other == null) {
				return -1;
			}

			if (Points > other.Points) {
				return -1;
			}

			if (Points < other.Points) {
				return 1;
			}

			if (Wins > other.Wins) {
				return -1;
			}

			if (Wins < other.Wins) {
				return 1;
			}

			if (Losses > other.Losses) {
				return 1;
			}

			if (Losses < other.Losses) {
				return -1;
			}

			return 0;
		}

	}

}
