using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por el cuadro
	/// de playoofs, para playoffs
	/// con enfrentamientos al mejor de 7.
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	public class Tr_PlayoffsDraw_Bo7(IFileReader reader) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/playoffs_draw_bo7.html").ReadToEnd();
			var id = tag.Arguments[0];
			var playoffs = context.GetPlayoffs(id);

			if (playoffs == null) {
				return string.Empty;
			}

			Dictionary<string, ITagReplacer> replacers = [];

			replacers.AddReplacersRange(Tr_PlayoffsDrawMethods.GetPlayerReplacers(playoffs.League, playoffs));
			replacers.AddReplacersRange(Tr_PlayoffsDrawMethods.GetResultReplacers(playoffs));
			replacers.AddReplacersRange(Tr_PlayoffsDrawMethods.GetOverallResultReplacers(playoffs));
			replacers.AddReplacersRange(Tr_PlayoffsDrawMethods.GetDurationReplacers(playoffs));

			return HtmlBuilder.Process(template, context, replacers);
		}

	}

}
