namespace ChessWachinSSG.Data.Dtos {

    /// <summary>
    /// Información sobre una fase de playoffs.
    /// </summary>
    /// <param name="Id">ID de la fase.</param>
    /// <param name="Name">Nombre de la fase.</param>
    /// <param name="Semifinals1">Primera semifinal. Puede ser null si aún no se ha jugado.</param>
    /// <param name="Semifinals2">Segunda semifinal. Puede ser null si aún no se ha jugado</param>
    /// <param name="Finals">Final. Puede ser null si aún no se ha jugado.</param>
    /// <param name="DefaultDurations">Duración por defecto de todas las partidas de una subfase.</param>
    public record class PlayoffsDto(
        string Id,
        string Name,
        PlayoffsPhaseDto? Semifinals1,
        PlayoffsPhaseDto? Semifinals2,
        PlayoffsPhaseDto? Finals,
        List<string> DefaultDurations);

}
