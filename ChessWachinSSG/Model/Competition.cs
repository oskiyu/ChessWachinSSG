namespace ChessWachinSSG.Model {

	/// <summary>
	/// Información sobre una competición en concreto.
	/// </summary>
	public record class Competition {

		public Competition(string id, string name, League? league, Playoffs? playoffs, List<InitialPlayerElo> elos) {
			Id = id;
			Name = name;
			LeaguePhase = league;
			Playoffs = playoffs;

			elos.Sort((x, y) => y.Elo.CompareTo(x.Elo));
			InitialElos = elos;

			var matchesBuilder = new MatchList.Builder();
			matchesBuilder.AddAllMatches(league?.Matches ?? new());
			matchesBuilder.AddAllMatches(playoffs?.Semifinals1?.Matches ?? new());
			matchesBuilder.AddAllMatches(playoffs?.Semifinals2?.Matches ?? new());
			matchesBuilder.AddAllMatches(playoffs?.Finals?.Matches ?? new());
			AllMatches = matchesBuilder.Build();

			Stats = new CompetitionStats.Builder().ApplyCompetition(this).Build();
		}

		/// <summary>
		/// Fase de liga de la competición.
		/// Puede ser null si aún no ha comenzado.
		/// </summary>
		public League? LeaguePhase { get; private init; }

		/// <summary>
		/// Fase de playoffs de la competición.
		/// Puede ser null si aún no ha comenzado.
		/// </summary>
		public Playoffs? Playoffs { get; private init; }

		/// <summary>
		/// ELOs al inicio de la competición.
		/// </summary>
		public IReadOnlyList<InitialPlayerElo> InitialElos { get; private init; }

		public CompetitionStats Stats { get; private init; }

		public MatchList AllMatches { get; private init; }

		public string Name { get; private init; }

		public string Id { get; private init; }

	}

}
