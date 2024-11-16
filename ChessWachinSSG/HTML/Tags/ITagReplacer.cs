using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

    /// <summary>
    /// Interfaz para clases que generan
    /// el texto por el que se debe reemplazar
    /// un tag.
    /// 
    /// Es un proceso recursivo.
    /// </summary>
    public interface ITagReplacer {

		/// <returns>Texto que reemplazará al tag.</returns>
		public string Replace(Tag tag, Context context);

	}

	/// <summary>
	/// Tipo de tag replacer.
	/// </summary>
	public enum TagReplacerType {

		/// <summary>
		/// Reemplaza el tag por los contenidos
		/// de un archivo externo.
		/// </summary>
		File,

		/// <summary>
		/// Reemplaza el tag por un texto
		/// previamente establecido.
		/// </summary>
		Inline,

		/// <summary>
		/// Implementado en una clase interna
		/// del programa.
		/// </summary>
		Internal

	}

}
