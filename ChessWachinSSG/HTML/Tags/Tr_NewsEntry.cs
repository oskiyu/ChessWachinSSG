using ChessWachinSSG.Data;
using ChessWachinSSG.Data.Dtos;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por la noticia indicada.
	/// </summary>
	/// <param name="reader">Clase lectora de archivos externos.</param>
	/// <param name="news">Noticia.</param>
	internal class Tr_NewsEntry(IFileReader reader, NewsDto news) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/news_entry.html").ReadToEnd();

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:news:entry:img", new Tr_Inline(news.ImageSource) },
				{ "cwssg:news:entry:title", new Tr_Inline(news.Title) },
				{ "cwssg:news:entry:date", new Tr_Inline(news.Date) },
				{ "cwssg:news:entry:text", new Tr_Inline(news.Text) }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

	}

}
