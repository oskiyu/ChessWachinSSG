namespace ChessWachinSSG.Model {

	/// <summary>
	/// Información sobre un país.
	/// </summary>
	/// <param name="Id">ID del país.</param>
	/// <param name="Name">Nombre del país.</param>
	/// <param name="FlagIconPath">Ruta al icono de la bandera del país.</param>
	public record class Country(string Id, string Name, string FlagIconPath);

}
