namespace ChessWachinSSG.Data.Dtos {

    /// <summary>
    /// Datos de una fase de liga.
    /// </summary>
    /// <param name="id">Identificador único de la liga.</param>
    /// <param name="name">Nombre de la liga.</param>
    /// <param name="matchesPath">Ruta con los datos de la liga.</param>
    /// <param name="qPositions">Número de jugadores que se clasifican para la siguiente ronda.</param>
    public record class LeagueDto(string Id, string Name, string MatchesPath, int QPositions);

}
