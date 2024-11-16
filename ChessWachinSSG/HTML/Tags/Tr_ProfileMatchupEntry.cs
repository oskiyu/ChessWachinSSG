using ChessWachinSSG.Data;
using ChessWachinSSG.Data.Dtos;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags
{

    /// <summary>
    /// Reemplaza el tag por el resultado
    /// histórico de un jugador contra otro.
    /// </summary>
    /// <param name="reader">Clase de lectura de archivos externos.</param>
    /// <param name="main">Jugador cuya página de perfil se está generando.</param>
    /// <param name="other">Rival.</param>
    public class Tr_ProfileMatchupEntry(IFileReader reader, Player main, Player other) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var template = reader.GetStream("Sources/profile_matchup_entry.html").ReadToEnd();

			int wins = 0;
			int draws = 0;
			int losses = 0;

			var matchesWithPlayers = context.AllMatches.GetAll()
				.Where(x => 
					(x.First == main  && x.Second == other) || 
					(x.First == other && x.Second == main));

			foreach (Match match in matchesWithPlayers) {
				wins += match.Result switch {
					Winner.First => match.First == main ? 1 : 0,
					Winner.Second => match.Second == main ? 1 : 0,
					_ => 0
				};

				draws += match.Result switch {
					Winner.Draw => 1,
					_ => 0
				};

				losses += match.Result switch {
					Winner.First => match.Second == main ? 1 : 0,
					Winner.Second => match.First == main ? 1 : 0,
					_ => 0
				};
			}

			var overall = wins * 2 + draws - losses * 2;

			var overallClass = overall switch {
				> 0 => "matchwin",
				  0 => "matchdraw",
				< 0 => "matchloss"
			};

			Dictionary<string, ITagReplacer> replacers = new() {
				{ "cwssg:profile:matchup:player", new Tr_Inline($"<{other.NameTag}>") },

				{ "cwssg:profile:matchup:wins", new Tr_Inline($"{wins}") },
				{ "cwssg:profile:matchup:draws", new Tr_Inline($"{draws}") },
				{ "cwssg:profile:matchup:losses", new Tr_Inline($"{losses}") },

				{ "cwssg:profile:matchup:class", new Tr_Inline(overallClass) }
			};

			return HtmlBuilder.Process(template, context, replacers);
		}

	}

}
