namespace ChessWachinSSG.Data {

	/// <summary>
	/// Interfaz de una clase capaz de leer archivos externos.
	/// Para mejorar testeabilidad, permitiendo
	/// hacer mocks.
	/// </summary>
	public interface IFileReader {

		public bool Exists(string path);
		public TextReader GetStream(string path);

	}

	/// <summary>
	/// Clase que lee archivos externos.
	/// </summary>
	public class FileReader : IFileReader {

		public bool Exists(string path) {
			return File.Exists(path);
		}

		public TextReader GetStream(string path) {
			return new StreamReader(path);
		}
	}

}
