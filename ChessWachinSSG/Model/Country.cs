namespace ChessWachinSSG.Model {

	/// <summary>
	/// Informaci�n sobre un pa�s.
	/// </summary>
	/// <param name="Id">ID del pa�s.</param>
	/// <param name="Name">Nombre del pa�s.</param>
	/// <param name="FlagIconPath">Ruta al icono de la bandera del pa�s.</param>
	/// <param name="PlayerCardClass">Clase CSS para mostrar la bandera en la tarjeta de perfil.</param>
	public record class Country(string Id, string Name, string FlagIconPath, string PlayerCardClass);

}
