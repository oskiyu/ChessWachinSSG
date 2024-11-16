using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

using System.Text;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por todos los
	/// elos iniciales de la competición.
	/// </summary>
	/// <param name="reader">Lector de archivos externos.</param>
	/// <param name="competition">Competición.</param>
	public class Tr_InitialEloEntries(IFileReader reader, Competition competition) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var output = new StringBuilder();

			foreach (var player in competition.InitialElos) {
				output.Append(new Tr_InitialEloEntry(reader, player).Replace(Tag.Empty, context));
			}

			return output.ToString();
		}

	}

}
