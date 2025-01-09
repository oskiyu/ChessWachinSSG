using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Replacer para el resultado de un jugador
	/// en una competición en concreto.
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	/// <param name="player">Jugador.</param>
	/// <param name="competition">Competición.</param>
	public class Tr_ProfileResults2_Entry(IFileReader reader, Player player, Competition competition) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/profile_result_line.html").ReadToEnd();

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:profile:result:torunament:type", new Tr_Inline(GetTournamentTypeBackgroundClass(competition.Type)) },
				{ "cwssg:profile:result:torunament:miniimg", new Tr_Inline(GetTournamentTypeMiniImage(competition.Type)) },
				{ "cwssg:profile:result:torunament:name", new Tr_Inline(competition.Name) },

				{ "cwssg:profile:results:tournament:global", new Tr_ProfileResultEntry_Global(reader, player, competition) },
				{ "cwssg:profile:results:tournament:league", new Tr_ProfileResultEntry_League(reader, player, competition.LeaguePhase) },
				{ "cwssg:profile:results:tournament:playoffs", new Tr_ProfileResultEntry_Playoffs(reader, player, competition.Playoffs) }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

		/// <param name="type">Tipo de competición.</param>
		/// <returns>Ruta al icono del tipo.</returns>
		private static string GetTournamentTypeMiniImage(CompetitionType type)
			=> type switch {
				CompetitionType.Rapid => "/assets/ChessWachinMini.png",
				CompetitionType.Blitz => "/assets/BlitzChessWachinMini.png",
				CompetitionType.FreeStyle => "/assets/FreeStyleChessWachinMini.png"
			};

		/// <param name="type">Tipo de competición.</param>
		/// <returns>Clase CSS del tipo.</returns>
		private static string GetTournamentTypeBackgroundClass(CompetitionType type)
			=> type switch {
				CompetitionType.Rapid => "pf_result_type_normal",
				CompetitionType.Blitz => "pf_result_type_blitz",
				CompetitionType.FreeStyle => "pf_result_type_freestyle"
			};

	}

}
