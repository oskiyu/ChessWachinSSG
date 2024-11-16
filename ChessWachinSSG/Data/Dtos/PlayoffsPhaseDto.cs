namespace ChessWachinSSG.Data.Dtos {

    /// <summary>
    /// Información sobre una sub-fase de unos playoffs.
    /// Puede corresponder a una semifinal o a una final.
    /// </summary>
    /// <param name="Id">ID de la sub-fase.</param>
    /// <param name="Name">Nombre de la sub-fase.</param>
    /// <param name="MatchesPath">Ruta en la que se guardan las partidas de la sub-fase.</param>
    public record class PlayoffsPhaseDto(string Id, string Name, string MatchesPath);

}
