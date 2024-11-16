using ChessWachinSSG.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessWachinSSG.Model {

	/// <summary>
	/// Clase con una lista de partidas.
	/// </summary>
	public class MatchList {

		/// <summary>
		/// Clase auxiliar para la construcción
		/// de la lista de partidas.
		/// </summary>
		public class Builder {

			/// <summary>
			/// Añade una partida a la lista.
			/// </summary>
			/// <param name="match">Partida.</param>
			public Builder AddMatch(Match match) {
				instance.matches.Add(match);
				return this;
			}

			/// <summary>
			/// Añade una serie de partidas a la lista.
			/// </summary>
			/// <param name="otherList">Lista de partidas.</param>
			public Builder AddAllMatches(MatchList otherList) {
				foreach (Match match in otherList.matches) {
					instance.matches.Add(match);
				}

				return this;
			}

			/// <returns>Lista de partidas construida.</returns>
			public MatchList Build() => instance;

			private readonly MatchList instance = new();

		}
		
		/// <returns>Todas las partidas.</returns>
		public IReadOnlyList<Match> GetAll() => matches;

		/// <param name="index">Índice de la partida.</param>
		/// <returns>Partida, o null si no existe en el índice indicado.</returns>
		public Match? GetAt(int index) {
			if (index >= matches.Count) {
				return null;
			}

			return matches[index];
		}

		private readonly List<Match> matches = [];

	}

}
