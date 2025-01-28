using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessWachinSSG.Model {

	public record class HistoricalRankingEntry(Player Player) : IComparable<HistoricalRankingEntry> {

		public int Points { get { return Wins * 2 + Draws * 1; } }

		public int Wins { get; set; }
		public int Draws { get; set; }
		public int Losses { get; set; }

		public int Titles { get; set; }
		public int Golds { get; set; }
		public int Silvers { get; set; }
		public int Bronzes { get; set; }

		public int CompareTo(HistoricalRankingEntry? other) {
			if (other == null) {
				return -1;
			}

			if (Titles > other.Titles) {
				return -1;
			}

			if (Titles < other.Titles) {
				return 1;
			}

			if (Golds > other.Golds) {
				return -1;
			}

			if (Golds < other.Golds) {
				return 1;
			}

			if (Silvers > other.Silvers) {
				return -1;
			}

			if (Silvers < other.Silvers) {
				return 1;
			}

			if (Bronzes > other.Bronzes) {
				return -1;
			}

			if (Bronzes < other.Bronzes) {
				return 1;
			}

			if (Points > other.Points) {
				return -1;
			}

			if (Points < other.Points) {
				return 1;
			}

			if (Wins > other.Wins) {
				return -1;
			}

			if (Wins < other.Wins) {
				return 1;
			}

			if (Losses > other.Losses) {
				return 1;
			}

			if (Losses < other.Losses) {
				return -1;
			}

			return 0;
		}

	}

}
