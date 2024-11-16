using ChessWachinSSG.Data.Dtos;

namespace ChessWachinSSG.Model {

    /// <summary>
    /// Partida.
    /// </summary>
    /// <param name="First">Jugador de blancas.</param>
    /// <param name="Second">Jugador de negras.</param>
    /// <param name="Result">Resultado de la partida.</param>
    /// <param name="Moves">Número de movimientos.</param>
    /// <param name="DurationSeconds">Duración de la partida, en segundos.</param>
    /// <param name="DurationTypeSeconds">Tipo de partida, por tiempo por jugador en segundos.</param>
    /// <param name="Url">Dirección de la partida en chess.com.</param>
    /// <param name="Date">Fecha de la partida.</param>
    public record class Match(
        Player First, 
        Player Second, 
        Winner Result, 
        int Moves, 
        int DurationSeconds, 
        int DurationTypeSeconds, 
        string Url, 
        string Date);

}
