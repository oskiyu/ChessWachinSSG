namespace ChessWachinSSG.Data.Dtos {

    /// <summary>
    /// Información básica sobre un país.
    /// </summary>
    /// <param name="id">Identificador único.</param>
    /// <param name="name">Nombre.</param>
    /// <param name="flagIconPath">Ruta a la imagen de la bandera.</param>
    /// <param name="PlayerCardClass">Clase usada para mostrar la bandera en la tarjeta del perfil.</param>
    public record class CountryDto(string Id, string Name, string FlagIconPath, string PlayerCardClass);

}
