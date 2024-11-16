using ChessWachinSSG.Data;
using ChessWachinSSG.Data.Dtos;
using ChessWachinSSG.Model;

using System.Text;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por las partidas jugadas en 
	/// una fecha en concreto.
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	/// <param name="matches">Lista de partidas.</param>
    public class Tr_MatchesInDate(IFileReader reader, List<Match> matches) : ITagReplacer {

		public string Replace(Tag tag, Context context) {
			var output = new StringBuilder();

			var template = reader.GetStream("Sources/league_history_single_match.html").ReadToEnd();

			foreach (Match match in matches) {
				Dictionary<string, ITagReplacer> replacers = new() {
					{ "cwssg:match:player1", new Tr_Inline($"<{match.First.NameTag}>") },
					{ "cwssg:match:result1class", new Tr_Inline($"\"{GetResultClass1(match.Result)}\"") },
					{ "cwssg:match:player2", new Tr_Inline($"<{match.Second.NameTag}>") },
					{ "cwssg:match:result2class", new Tr_Inline($"\"{GetResultClass2(match.Result)}\"") },
					{ "cwssg:match:moves", new Tr_Inline(match.Moves.ToString()) },
					{ "cwssg:match:duration", new Tr_Inline($"{match.DurationSeconds / 60}m {match.DurationSeconds % 60}s") },
					{ "cwssg:match:link", new Tr_Inline($"\"{match.Url}\"") }
				};

				output.Append(HtmlBuilder.Process(template, context, replacers));
			}

			return output.ToString();
		}


		/// <param name="result">Resultado de una partida.</param>
		/// <returns>Clase CSS para el primer jugador.</returns>
		private static string GetResultClass1(Winner result) {
			return result switch {
				Winner.First => "matchwin",
				Winner.Draw => "matchdraw",
				Winner.Second => "matchloss",
			};
		}

		/// <param name="result">Resultado de una partida.</param>
		/// <returns>Clase CSS para el segundo jugador.</returns>
		private static string GetResultClass2(Winner result) {
			return result switch {
				Winner.First => "matchloss",
				Winner.Draw => "matchdraw",
				Winner.Second => "matchwin",
			};
		}

	}

}
