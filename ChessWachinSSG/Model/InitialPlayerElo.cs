namespace ChessWachinSSG.Model {

	/// <summary>
	/// Elo de un jugador al inicio de una
	/// competición.
	/// </summary>
	/// <param name="Player">Jugador.</param>
	/// <param name="Elo">ELO.</param>
	public record class InitialPlayerElo(Player Player, int Elo);

}
