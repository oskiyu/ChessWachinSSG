namespace ChessWachinSSG.Data.Dtos {

    /// <summary>
    /// Datos de un jugador.
    /// </summary>
    /// <param name="Id">ID del jugador.</param>
    /// <param name="Name">Nombre del jugador en chess.com.</param>
    /// <param name="CountryId">ID del país del jugador.</param>
    /// <param name="PfpPath">Ruta a la foto de perfil (opcional).</param>
    /// <param name="NameTag">Texto del tag con su nombre.</param>
    public record class PlayerDto(string Id, string Name, string CountryId, string? PfpPath, string NameTag);

}
