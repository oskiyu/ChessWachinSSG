using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

using System.Text;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por la tabla de enfrentamientos
	/// del perfil de un jugador.
	/// </summary>
	/// <param name="reader">Clase de lectura de archivos externos.</param>
	/// <param name="player">Jugador.</param>
	public class Tr_ProfileMatchupsTable(IFileReader reader, Player player) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/profile_matchups_table.html").ReadToEnd();
			var output = new StringBuilder();

			foreach (var (_, p) in context.Players) {
				if (p == player) {
					continue;
				}

				output.Append(new Tr_ProfileMatchupEntry(reader, player, p).Replace(Tag.Empty, context));
			}

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:profile:matchups:entries", new Tr_Inline(output.ToString()) }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

	}

}
