namespace ChessWachinSSG.Model {

	/// <summary>
	/// Información sobre un país.
	/// </summary>
	/// <param name="Id">ID del país.</param>
	/// <param name="Name">Nombre del país.</param>
	/// <param name="FlagIconPath">Ruta al icono de la bandera del país.</param>
	/// <param name="PlayerCardClass">Clase CSS para mostrar la bandera en la tarjeta de perfil.</param>
	public record class Country(string Id, string Name, string FlagIconPath, string PlayerCardClass);

}
