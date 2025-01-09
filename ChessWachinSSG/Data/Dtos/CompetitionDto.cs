using ChessWachinSSG.Model;

namespace ChessWachinSSG.Data.Dtos {

    /// <summary>
    /// Datos generales de una competición.
    /// </summary>
    /// <param name="Id">Identificador único de la competición.</param>
    /// <param name="Name">Nombre de la competición.</param>
    /// <param name="League">Fase de liga de la competición, si está presente.</param>
    /// <param name="Playoffs">Fase de playoffs de la competición, si está presente.</param>
    /// <param name="InitialElos">ELOs iniciales de los participantes al inicio de la competición.</param>
    /// <param name="Type">Categoría de competición.</param>
    public record class CompetitionDto(
        string Id,
        string Name,
        LeagueDto? League,
        PlayoffsDto? Playoffs,
        List<PlayerInitialEloDto> InitialElos,
        CompetitionType Type);

}
