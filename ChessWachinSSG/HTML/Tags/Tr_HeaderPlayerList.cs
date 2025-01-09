using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

using System.Text;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Replacer para la lista de jugadores
	/// del header.
	/// </summary>
	public class Tr_HeaderPlayerList : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var output = new StringBuilder();

			var replacers = new Dictionary<string, ITagReplacer>() {
				{ "cwssg:profile:classes", new Tr_Inline("topnav_player") }
			};

			foreach (var (_, player) in context.Players) {
				output.Append(HtmlBuilder.Process($"<{player.NameTag}>", context, replacers));
			}

			return output.ToString();
		}

	}

}
