using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por el elo inicial
	/// de un jugador.
	/// </summary>
	/// <param name="reader">Clase lectora de archivos externos.</param>
	/// <param name="player">Jugador.</param>
	internal class Tr_InitialEloEntry(IFileReader reader, InitialPlayerElo player) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/initial_elo_table_entry.html").ReadToEnd();

			string elo = player.Elo == 0 ? "-" : player.Elo.ToString();

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:competition:initialelo:name", new Tr_Inline($"<{player.Player.NameTag}>") },
				{ "cwssg:competition:initialelo:elo", new Tr_Inline($"{elo}") }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

	}

}
