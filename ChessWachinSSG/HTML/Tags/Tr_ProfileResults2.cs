using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

using System.Text;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Replacer para los resultados de un jugador
	/// en las competiciones en las que ha participado.
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	/// <param name="player">Jugador.</param>
	public class Tr_ProfileResults2(IFileReader reader, Player player) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/profile_results.html").ReadToEnd();

			var output = new StringBuilder();
			foreach (var competition in context.Competitions.Where(x => x.Value.InitialElos.Any(y => y.Player == player)).Reverse()) {
				output.Append(new Tr_ProfileResults2_Entry(reader, player, competition.Value).Replace(Tag.Empty, context));
			}

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:profile:result:entries", new Tr_Inline(output.ToString()) }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

	}

}
