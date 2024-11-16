using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por un texto establecido
	/// en el constructor.
	/// </summary>
	/// <param name="value">Texto final.</param>
	public class Tr_Inline(string value) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			return value;
		}

		public override string ToString() =>
			$"ChessWachinSSG.HTML.Tags.InlineTagReplacer(value: {value})";

	}

}
