using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

using System.Text;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Reemplaza el tag por todas las entradas
	/// de una tabla de elo.
	/// </summary>
	/// <param name="reader">Clase lectora.</param>
	/// <param name="type">Tipo de partida.</param>
	public class Tr_EloTableEntries(IFileReader reader, EloType type) : ITagReplacer {

		/// <summary>
		/// Compara el elo de dos jugadores.
		/// </summary>
		/// <param name="x">Primer jugador.</param>
		/// <param name="y">Segundo jugador.</param>
		/// <returns>
		/// 0 si van empatados.
		/// -1 si va ganando el primer jugador.
		/// 1 si va ganando el segundo jugador.
		/// </returns>
		private static int Comparer(PlayerEloEntry? x, PlayerEloEntry? y) => (x, y) switch {

			// Si ambos no tienen partidas: empate.
			(null, null) => 0,

			// Si el primero tiene partidas pero el segundo no,
			// gana el primero.
			(_, null) => -1,

			// Si el segundo tiene partidas pero el primero no,
			// gana el segundo.
			(null, _) => 1,

			// Si el primero lleva suficientes partidas pero el segundo no,
			// gana el primero.
			( { TotalMatches: > 10 }, { TotalMatches: < 10 }) => -1,

			// Si el segundo lleva suficientes partidas pero el primero no,
			// gana el segundo.
			( { TotalMatches: < 10 }, { TotalMatches: > 10 }) => 1,

			// Si ambos tienen suficientes partidas, gana el que
			// tenga más ELO.
			_ => y.Elo.CompareTo(x.Elo)

		};

		public string Replace(Tag tag, Context context) {
			var output = new StringBuilder();

			var elos = context.Elos.Values.ToList();
			elos.Sort((x, y) => type switch {
				EloType.Rapid => Comparer(x.Rapid, y.Rapid),
				EloType.Blitz => Comparer(x.Blitz, y.Blitz)
			});

			foreach (var elo in elos) {
				output.AppendLine(
					new Tr_EloTableEntry(
						reader,
						elo.Player,
						type switch {
							EloType.Rapid => elo.Rapid,
							EloType.Blitz => elo.Blitz,
							_ => null
						})
					.Replace(Tag.Empty, context));
			}

			return output.ToString();
		}

	}
}
