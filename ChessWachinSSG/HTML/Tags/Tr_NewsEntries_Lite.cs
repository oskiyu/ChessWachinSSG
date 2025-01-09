using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

using System.Text;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por todas las noticias.
	/// </summary>
	/// <param name="reader">Lector de archivos externos.</param>
	public class Tr_NewsEntries_Lite(IFileReader reader) : ITagReplacer {

		/// <summary>
		/// Número máximo de noticias mostradas
		/// en la página de inicio.
		/// </summary>
		private const int MaxQuickNews = 3;

		public string Replace(Tag tag, Context context) {
			var output = new StringBuilder();

			int count = 0;
			foreach (var news in context.News) {
				output.Append(new Tr_NewsEntry_Lite(reader, news).Replace(Tag.Empty, context));
				count++;

				if (count == MaxQuickNews) {
					break;
				}
			}

			return output.ToString();
		}

	}

}
