namespace ChessWachinSSG.Model {
	
	/// <summary>
	/// Jugador de Chess-Wachín.
	/// </summary>
	/// <param name="Id">ID del jugador.</param>
	/// <param name="Name">Nombre del jugador en chess.com.</param>
	/// <param name="Country">País del jugador.</param>
	/// <param name="PfpPath">Ruta a la foto de perfil (opcional).</param>
	/// <param name="NameTag">Tag del jugador.</param>
	public record class Player(string Id, string Name, Country Country, string? PfpPath, string NameTag);

}
