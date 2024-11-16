namespace ChessWachinSSG.Data.Dtos {

    /// <summary>
    /// ELOs de un jugador.
    /// </summary>
    /// <param name="PlayerId">ID del jugador.</param>
    /// <param name="Rapid">
    /// ELO en partidas de 10'.
    /// Puede ser null si no ha jugado ninguna partida clasificatoria.
    /// </param>
    /// <param name="Blitz">
    /// ELO en partidas de 3'.
    /// Puede ser null si no ha jugado ninguna partida clasificatoria.
    /// </param>
    public record class EloDto(string PlayerId, EloEntryDto? Rapid, EloEntryDto? Blitz);

}
