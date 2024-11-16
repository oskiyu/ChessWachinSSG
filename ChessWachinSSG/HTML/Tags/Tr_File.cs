using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por el contenido
	/// de un archivo.
	/// </summary>
	/// <param name="path">Ruta del archivo.</param>
	/// <param name="reader">Lector de archivos.</param>
	public class Tr_File(string path, IFileReader reader) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			return value;
		}

		public override string ToString() =>
			$"ChessWachinSSG.HTML.Tags.FileTagReplacer(path: {path})";

		private readonly string value = reader.GetStream(path).ReadToEnd();

	}

}
