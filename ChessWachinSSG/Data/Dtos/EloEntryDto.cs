namespace ChessWachinSSG.Data.Dtos {

    /// <summary>
    /// Elo de un jugador en un tipo de partida en concreto.
    /// </summary>
    /// <param name="Elo">ELO.</param>
    /// <param name="Wins">Victorias.</param>
    /// <param name="Draws">Empates.</param>
    /// <param name="Losses">Derrotas.</param>
    public record class EloEntryDto(int Elo, int Wins, int Draws, int Losses);

}
