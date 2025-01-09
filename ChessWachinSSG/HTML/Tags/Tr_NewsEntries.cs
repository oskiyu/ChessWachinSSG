using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

using System.Text;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por todas las noticias.
	/// </summary>
	/// <param name="reader">Lector de archivos externos.</param>
	public class Tr_NewsEntries(IFileReader reader) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var output = new StringBuilder();

			foreach (var news in context.News) {
				output.Append(new Tr_NewsEntry(reader, news).Replace(Tag.Empty, context));
			}

			return output.ToString();
		}

	}

}
