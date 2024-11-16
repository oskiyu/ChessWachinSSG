using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

using System.Text;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por el historial de un jugador
	/// en todas las competiciones..
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	/// <param name="player">Jugador.</param>
	public class Tr_ProfileHistory(IFileReader reader, Player player) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/profile_history.html").ReadToEnd();

			var output = new StringBuilder();

			foreach (var competition in context.Competitions) {
				output.Append(new Tr_ProfileHistoryEntry(reader, player, competition.Value).Replace(Tag.Empty, context));
			}

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:profile:history:entries", new Tr_Inline(output.ToString()) }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

	}

}
