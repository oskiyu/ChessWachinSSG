namespace ChessWachinSSG.Model {

	/// <summary>
	/// Fase final de un torneo.
	/// 
	/// Puede no estar completado,
	/// en cuyo caso puede que
	/// las semifinales y la final 
	/// sean nulas.
	/// </summary>
	/// <param name="id">Identificador.</param>
	/// <param name="sf1">Primera semifinal (opcional).</param>
	/// <param name="sf2">Segunda semifinal (opcional).</param>
	/// <param name="final">Final (opcional) (si está, entonces ambas semifinales deben estar estrablecidas).</param>
	/// <param name="defaultDurations">Duraciones por defecto de las partidas.</param>
	public record class Playoffs(string Id, PlayoffsRound? Semifinals1, PlayoffsRound? Semifinals2, PlayoffsRound? Finals, IReadOnlyList<string> DefaultDurations) {

		/// <summary>
		/// Ganador, si está terminado. 
		/// En caso contrario, null.
		/// </summary>
		public Player? Winner { get => Finals?.Ranking.Ranking.ElementAtOrDefault(0)?.Player; }

		/// <summary>
		/// Segundo, si está terminado. 
		/// En caso contrario, null.
		/// </summary>
		public Player? Second { get => Finals?.Ranking.Ranking.ElementAtOrDefault(1)?.Player; }

		/// <param name="id">Identificador de la fase,</param>
		/// <returns>Fase (null si no se encuentra).</returns>
		public PlayoffsRound? GetPhase(string id) {
			if (id == Semifinals1?.Id) return Semifinals1;
			if (id == Semifinals2?.Id) return Semifinals2;
			if (id == Finals?.Id) return Finals;

			return null;
		}

	}

}
