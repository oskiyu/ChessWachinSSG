namespace ChessWachinSSG.Model {

	/// <summary>
	/// Fase de playoffs.
	/// Se enfrentan dos jugadores.
	/// </summary>
	/// <param name="id">Identificador de la fase.</param>
	/// <param name="name">Nombre de la fase.</param>
	/// <param name="matches">Partidas de la fase.</param>
	public record class PlayoffsRound(string Id, string Name, MatchList Matches) {

		/// <summary>
		/// Establece la competición a la que pertenece.
		/// </summary>
		/// <param name="parent"></param>
		public void SetParent(Playoffs parent) => Parent = parent;

		/// <summary>
		/// Ganador, si está terminado. 
		/// En caso contrario, null.
		/// </summary>
		public Player? Winner { get => Ranking.Ranking.ElementAtOrDefault(0)?.Player; }

		/// <summary>
		/// Segundo, si está terminado. 
		/// En caso contrario, null.
		/// </summary>
		public Player? Second { get => Ranking.Ranking.ElementAtOrDefault(1)?.Player; }

		public PointsRanking Ranking { get {
				ranking ??= new PointsRanking.Builder().ApplyAllMatches(Matches).Build();
				return ranking;
			} }

		public Playoffs? Parent { get; private set; }
		
		private PointsRanking? ranking = null;
	}

	/// <summary>
	/// Tipos de fases posibles en unos playoffs.
	/// </summary>
	public enum PlayoffRoundType {
		Semifinal1,
		Semifinal2,
		Final
	}

}
