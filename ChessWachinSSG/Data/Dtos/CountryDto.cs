namespace ChessWachinSSG.Data.Dtos {

    /// <summary>
    /// Información básica sobre un país.
    /// </summary>
    /// <param name="id">Identificador único.</param>
    /// <param name="name">Nombre.</param>
    /// <param name="flagIconPath">Ruta a la imagen de la bandera.</param>
    public record class CountryDto(string Id, string Name, string FlagIconPath);

}
