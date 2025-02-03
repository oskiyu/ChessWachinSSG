namespace ChessWachinSSG.Model {

	/// <summary>
	/// Información sobre una competición en concreto.
	/// </summary>
	public record class Competition {

		public Competition(string id, string name, League? league, Playoffs? playoffs, List<InitialPlayerElo> elos, CompetitionType type) {
			Id = id;
			Name = name;
			LeaguePhase = league;
			Playoffs = playoffs;

			elos.Sort((x, y) => y.Elo.CompareTo(x.Elo));
			InitialElos = elos;

			var matchesBuilder = new MatchList.Builder();
			matchesBuilder.AddAllMatches(league?.Matches ?? new());
			matchesBuilder.AddAllMatches(league?.DesempateMatches ?? new());
			matchesBuilder.AddAllMatches(playoffs?.Semifinals1?.Matches ?? new());
			matchesBuilder.AddAllMatches(playoffs?.Semifinals2?.Matches ?? new());
			matchesBuilder.AddAllMatches(playoffs?.Finals?.Matches ?? new());
			AllMatches = matchesBuilder.Build();

			Stats = new CompetitionStats.Builder().ApplyCompetition(this).Build();

			Type = type;
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

		/// <summary>
		/// Estadísticas de toda la competición.
		/// </summary>
		public CompetitionStats Stats { get; private init; }

		/// <summary>
		/// Lista con todas las partidas de la competición,
		/// incluyendo liga y playoffs.
		/// </summary>
		public MatchList AllMatches { get; private init; }

		/// <summary>
		/// Nombre oficial de la competición.
		/// </summary>
		public string Name { get; private init; }

		/// <summary>
		/// Identificador único de la competición.
		/// </summary>
		public string Id { get; private init; }

		/// <summary>
		/// Tipo de competición.
		/// </summary>
		public CompetitionType Type { get; private init; }

	}

	/// <summary>
	/// Tipo de competición.
	/// </summary>
	public enum CompetitionType {

		/// <summary>
		/// Partidas normales de 10 minutos.
		/// Estilo clásico.
		/// </summary>
		Rapid,

		/// <summary>
		/// Partidas normales de 10 minutos.
		/// </summary>
		Blitz,

		/// <summary>
		/// Partidas Fisher-random de 10 minutos.
		/// </summary>
		FreeStyle

	}

}
