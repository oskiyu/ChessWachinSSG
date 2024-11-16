using ChessWachinSSG.Data.Dtos;

namespace ChessWachinSSG.Model {

	/// <summary>
	/// Estadísticas de una competición.
	/// </summary>
    public class CompetitionStats {

		/// <summary>
		/// Clase auxiliar para la construcción
		/// de las estadísticas de competición.
		/// </summary>
		public class Builder {

			/// <returns>Instancia construida.</returns>
			public CompetitionStats Build() => instance;

			/// <summary>
			/// Aplica la competición a las estadísticas.
			/// </summary>
			/// <param name="competition">Comeptición.</param>
			public Builder ApplyCompetition(Competition competition) {
				var allMatchesBuilder = new MatchList.Builder();

				allMatchesBuilder.AddAllMatches(competition.LeaguePhase?.Matches ?? new());

				allMatchesBuilder.AddAllMatches(competition.Playoffs?.Semifinals1?.Matches	?? new());
				allMatchesBuilder.AddAllMatches(competition.Playoffs?.Semifinals2?.Matches	?? new());
				allMatchesBuilder.AddAllMatches(competition.Playoffs?.Finals?.Matches		?? new());

				var allMatches = allMatchesBuilder.Build();

				instance.MatchCount = allMatches.GetAll().Count;
				instance.WhiteWins = allMatches.GetAll().Count(x => x.Result == Winner.First);
				instance.Draws = allMatches.GetAll().Count(x => x.Result == Winner.Draw);
				instance.BlackWins = allMatches.GetAll().Count(x => x.Result == Winner.Second);

				instance.AverageMoveCount = instance.MatchCount != 0 ? allMatches.GetAll().Sum(x => x.Moves) / instance.MatchCount : 0;
				instance.AverageGameDuration= instance.MatchCount != 0 ? allMatches.GetAll().Sum(x => x.DurationSeconds) / instance.MatchCount : 0;
				instance.AverageMoveDuration = instance.MatchCount != 0 ? instance.AverageGameDuration / instance.AverageMoveCount : 0;

				return this;
			}

			private readonly CompetitionStats instance = new();

		}

		public int MatchCount { get; private set; }

		public int WhiteWins { get; private set; }
		public int Draws { get; private set; }
		public int BlackWins { get; private set; }

		public int AverageMoveCount { get; private set; }

		/// <summary>
		/// En segundos.
		/// </summary>
		public int AverageGameDuration { get; private set; }

		/// <summary>
		/// En segundos.
		/// </summary>
		public int AverageMoveDuration { get; private set; }

	}

}
