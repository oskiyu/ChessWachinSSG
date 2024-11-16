namespace ChessWachinSSG.Data.Dtos {

    /// <summary>
    /// Información sobre una partida.
    /// </summary>
    /// <param name="FirstPlayerId">ID del primer jugador.</param>
    /// <param name="SecondPlayerId">ID del segundo jugador.</param>
    /// <param name="Winner">Resultado de la partida.</param>
    /// <param name="Moves">Número de movimientos de cada jugador.</param>
    /// <param name="Duration">Duración, en segundos.</param>
    /// <param name="DurationType">Tipo de partida, en segundos por jugador.</param>
    /// <param name="Url">Dirección URL de la partida en chess.com.</param>
    /// <param name="Date">Fecha de la partida.</param>
    public record class MatchDto(
        string FirstPlayerId,
        string SecondPlayerId,
        Winner Winner,
        int Moves,
        int Duration,
        int DurationType,
        string Url,
        string Date);

    /// <summary>
    /// Resultado de una partida.
    /// </summary>
    public enum Winner {

        /// <summary>
        /// Ganó el primer jugador.
        /// </summary>
        First,

        /// <summary>
        /// Ganó el segundo jugador.
        /// </summary>
        Second,

        /// <summary>
        /// Empate.
        /// </summary>
        Draw

    }

}
