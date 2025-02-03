namespace ChessWachinSSG.Data.Dtos {

	/// <summary>
	/// Datos de una fase de liga.
	/// </summary>
	/// <param name="Id">Identificador único de la liga.</param>
	/// <param name="Name">Nombre de la liga.</param>
	/// <param name="MatchesPath">Ruta con los datos de la liga.</param>
	/// <param name="QPositions">Número de jugadores que se clasifican para la siguiente ronda.</param>
	/// <param name="DesempateMatchesPath">Path al archivo con las partidas de desempate.</param>
	public record class LeagueDto(string Id, string Name, string MatchesPath, int QPositions, string? DesempateMatchesPath);

}
