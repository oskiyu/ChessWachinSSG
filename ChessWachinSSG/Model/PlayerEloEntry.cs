namespace ChessWachinSSG.Model {

	/// <summary>
	/// ELO en un tipo de partida en concreto.
	/// </summary>
	/// <param name="Elo">Puntos de ELO.</param>
	/// <param name="Wins">Victorias.</param>
	/// <param name="Draws">Empates.</param>
	/// <param name="Losses">Derrotas.</param>
	public record class PlayerEloEntry(int Elo, int Wins, int Draws, int Losses) {

		public int TotalMatches { get => Wins + Draws + Losses; }

	}

}
