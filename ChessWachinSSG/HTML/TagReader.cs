using ChessWachinSSG.HTML.Tags;

using System.Text;

namespace ChessWachinSSG.HTML {
	
	/// <summary>
	/// Clase que lee todos los tags
	/// de un texto.
	/// </summary>
	public static class TagReader {

		/// <summary>
		/// Lee todos los tags de un texto,
		/// y los devuelve.
		/// </summary>
		/// <param name="source">Texto fuente.</param>
		/// <returns>Lista de tags.</returns>
		public static List<Tag> ReadAllTags(string source) {
			var output = new List<Tag>();

			foreach (var indices in GetTagIndices(source)) {
				output.Add(BuildTag(source, indices));
			}

			return output;
		}

		/// <summary>
		/// Construye un tag a partir de los índices
		/// de inicio y de fin del tag.
		/// </summary>
		/// <param name="text">Texto.</param>
		/// <param name="indices">Índices.</param>
		/// <returns>Tag.</returns>
		private static Tag BuildTag(string text, TagIndices indices) {
			List<string> args = [];

			StringBuilder currentArg = new();

			int i = indices.Start + 1;
			for (; i < indices.End; i++) {
				char c = text[i];

				if (char.IsWhiteSpace(c)) {

					if (currentArg.Length != 0) {
						args.Add(currentArg.ToString());
						currentArg = new();
					}

					continue;
				}

				if (c == '>') {
					if (currentArg.Length != 0) {
						args.Add(currentArg.ToString());
					}

					break;
				}

				currentArg.Append(c);
			}

			return new Tag(args[0], args.Skip(1).ToList(), indices.End - indices.Start, indices.Start);
		}

		/// <summary>
		/// Índices en los que comienza y termina un tag.
		/// </summary>
		/// <param name="start">Índice del comienzo.</param>
		/// <param name="end">Índice final.</param>
		private record struct TagIndices(int Start, int End);

		/// <summary>
		/// Obtiene todos los índices de todos los tags
		/// del texto.
		/// </summary>
		/// <param name="text">Texto a procesar.</param>
		/// <returns>Índices de todos los tags.</returns>
		private static List<TagIndices> GetTagIndices(string text) {
			List<TagIndices> tags = [];
			Stack<int> starts = new();

			for (int i = 0; i < text.Length; i++) {
				if (text[i] == '<') {
					starts.Push(i);
				}

				else if (text[i] == '>') {
					tags.Add(new TagIndices(starts.Pop(), i + 1));
				}
			}

			return tags;
		}

	}

}
