using ChessWachinSSG.Model;

namespace ChessWachinSSG {

	public static class Globals {

		public const string GoldMedal = "🥇";
		public const string SilverMedal = "🥈";
		public const string BronzeMedal = "🥉";
		public const string Cup = "🏆";

		public const string GreenBox = "🟩";
		public const string YellowBox = "🟨";
		public const string RedBox = "🟥";

		public static string GetRankingColorClass(int position)
			=> position switch {
				1 => "pfp_ranking_gold",
				2 => "pfp_ranking_silver",
				3 => "pfp_ranking_bronze",
				_ => string.Empty
			};

		public static string ToMinutesSeconds(int seconds) {
			return $"{seconds / 60}m {seconds % 60}s";
		}

		public static string GetLeagueMedal(int playerPosition, bool isFinished) {
			string medal = string.Empty;

			if (isFinished) {
				medal = playerPosition switch {
					0 => GoldMedal,
					1 => SilverMedal,
					2 => BronzeMedal,
					_ => string.Empty
				};
			}

			return medal;
		}

	}

}
