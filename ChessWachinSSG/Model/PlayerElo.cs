namespace ChessWachinSSG.Model {

	/// <summary>
	/// Elos de un jugador.
	/// </summary>
	/// <param name="Player">Jugador.</param>
	/// <param name="Rapid">ELO en partidas de 10'. Puede ser null si no ha jugado partidas clasificatorias de 10'.</param>
	/// <param name="Blitz">ELO en partidas de 3'. Puede ser null si no ha jugado partidas clasificatorias de 3'.</param>
	public record class PlayerElo(Player Player, PlayerEloEntry? Rapid, PlayerEloEntry? Blitz);

	/// <summary>
	/// Tipo de partidas clasificatorias.
	/// </summary>
	public enum EloType {

		/// <summary>
		/// De 10'.
		/// </summary>
		Rapid,

		/// <summary>
		/// De 3'.
		/// </summary>
		Blitz

	}

}
