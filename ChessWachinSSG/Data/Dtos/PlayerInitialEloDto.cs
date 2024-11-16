namespace ChessWachinSSG.Data.Dtos {

    /// <summary>
    /// Elo inicial de un jugador
    /// al inicio de una competición.
    /// </summary>
    /// <param name="PlayerId">ID del jugador.</param>
    /// <param name="Elo">ELO.</param>
    public record class PlayerInitialEloDto(string PlayerId, int Elo);

}
