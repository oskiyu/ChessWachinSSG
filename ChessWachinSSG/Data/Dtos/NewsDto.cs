namespace ChessWachinSSG.Data.Dtos {

	/// <summary>
	/// Datos de una noticia.
	/// </summary>
	/// <param name="Title">Título de la noticia.</param>
	/// <param name="Date">Fecha de la noticia.</param>
	/// <param name="Text">Cuerpo de la noticia.</param>
	/// <param name="ImageSource">Ruta a la imagen de la noticia.</param>
	public record class NewsDto(
		string Title,
		string Date,
		string Text,
		string ImageSource);

}
