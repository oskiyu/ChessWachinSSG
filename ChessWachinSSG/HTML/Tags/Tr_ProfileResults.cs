using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

using System.Text;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por los resultados de un jugador
	/// en las competiciones en las que haya participado.
	/// </summary>
	/// <param name="reader">Lector de archivos externos.</param>
	/// <param name="player">Jugador.</param>
	public class Tr_ProfileResults(IFileReader reader, Player player) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/profile_results_table.html").ReadToEnd();

			var output = new StringBuilder();

			foreach (var competition in context.Competitions) {
				output.Append(new Tr_ProfileResultEntry(reader, player, competition.Value).Replace(Tag.Empty, context));
			}

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:profile:results:entries", new Tr_Inline(output.ToString()) }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

	}

}
