namespace ChessWachinSSG.Model {

	/// <summary>
	/// Clase con los records personales
	/// a lo largo de todas las competiciones.
	/// </summary>
	public class PersonalRecords {

		/// <summary>
		/// Clase auxiliar para la construcción
		/// de los records.
		/// </summary>
		public class Builder {

			public PersonalRecords Build() => instance;

			public void ProcessLeaguePoints(int points, Competition from) {
				if (points > instance.MaxLeaguePoints) {
					instance.MaxLeaguePoints = points;
					instance.MaxLeaguePointsCompetition = from;
				}
			}

			public void ProcessLeagueWins(int wins, Competition from) {
				if (wins > instance.MaxLeagueWins) {
					instance.MaxLeagueWins = wins;
					instance.MaxLeagueWinsCompetition = from;
				}
			}

			public void AddWin() {
				AddUndefeat();
				instance.CurrentWinStreak++;
				if (instance.CurrentWinStreak > instance.MaxWinStreak) {
					instance.MaxWinStreak = instance.CurrentWinStreak;
				}

				instance.CurrentLossStreak = 0;
			}

			public void AddDraw() {
				AddUndefeat();
				instance.CurrentWinStreak = 0;
				instance.CurrentLossStreak = 0;
			}

			public void AddLoss() {
				instance.CurrentLossStreak++;
				if (instance.CurrentLossStreak > instance.MaxLossStreak) {
					instance.MaxLossStreak = instance.CurrentLossStreak;
				}

				instance.CurrentWinStreak = 0;
				instance.CurrentUndefeatedStreak = 0;
			}

			private void AddUndefeat() {
				instance.CurrentUndefeatedStreak++;

				if (instance.CurrentUndefeatedStreak > instance.MaxUndefeatedStreak) {
					instance.MaxUndefeatedStreak = instance.CurrentUndefeatedStreak;
				}
			}

			private readonly PersonalRecords instance = new();

		}

		public bool IsWinStreakActive { get => CurrentWinStreak == MaxWinStreak; }
		public bool IsUndefeatedStreakActive { get => CurrentUndefeatedStreak == MaxUndefeatedStreak; }
		public bool IsLossStreakActive { get => CurrentLossStreak == MaxLossStreak; }

		public int CurrentWinStreak { get; private set; }
		public int MaxWinStreak { get; private set; }

		public int CurrentUndefeatedStreak { get; private set; }
		public int MaxUndefeatedStreak { get; private set; }

		public int CurrentLossStreak { get; private set; }
		public int MaxLossStreak { get; private set; }

		public int MaxLeaguePoints { get; private set; }
		public Competition? MaxLeaguePointsCompetition { get; private set; }

		public int MaxLeagueWins { get; private set; }
		public Competition? MaxLeagueWinsCompetition { get; private set; }

	}
}
