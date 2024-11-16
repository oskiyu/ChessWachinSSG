namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Instancia de un tag.
	/// </summary>
	/// <param name="id">ID del tag.</param>
	/// <param name="arguments">Argumentos del tag. Puede estar vacío.</param>
	/// <param name="totalCharacters">
	/// Número de caracteres que ocupa. 
	/// Para el proceso de lectura de archivos HTML.
	/// </param>
	/// <param name="firstCharIndex">
	/// Índice del primer carácter del tag dentro del texto.
	/// Para el proceso de lectura de archivos HTML.
	/// </param>
	public record class Tag(string Id, List<string> Arguments, int TotalCharacters, int FirstCharacterInex) {

		/// <summary>
		/// Tag vacío.
		/// </summary>
		public static Tag Empty { get => new(string.Empty, [], 0, 0); }

	}

}
