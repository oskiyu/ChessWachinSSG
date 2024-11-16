using ChessWachinSSG.Data.Dtos;
using ChessWachinSSG.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Model
{

    [TestClass]
	public class PlayoffsPhaseTests {

		[TestMethod]
		public void Test() {
			var player1 = new Player("id1", "name1", new("", "", ""), "", "tag1");
			var player2 = new Player("id2", "name2", new("", "", ""), "", "tag2");
			var matches = new MatchList.Builder()
				.AddMatch(new(player1, player2, Winner.First, 5, 6, 7, "", ""))
				.AddMatch(new(player1, player2, Winner.Draw, 5, 6, 7, "", ""))
				.AddMatch(new(player2, player1, Winner.Second, 5, 6, 7, "", ""))
				.Build();

			var playoffsPhase = new PlayoffsRound("id", "name", matches);

			Assert.AreEqual("id", playoffsPhase.Id);
			Assert.AreEqual("name", playoffsPhase.Name);
			Assert.AreEqual(matches, playoffsPhase.Matches);

			var ranking = new PointsRanking.Builder().ApplyAllMatches(matches).Build();
			Assert.AreEqual(ranking.Ranking.Count, playoffsPhase.Ranking.Ranking.Count);
			
			Assert.AreEqual(ranking.GetPlayerPosition(player1), playoffsPhase.Ranking.GetPlayerPosition(player1));
			Assert.AreEqual(ranking.GetPlayerPosition(player2), playoffsPhase.Ranking.GetPlayerPosition(player2));

			Assert.AreEqual(ranking.GetPlayerInfo(player1)!.Points, playoffsPhase.Ranking.GetPlayerInfo(player1)!.Points);
			Assert.AreEqual(ranking.GetPlayerInfo(player2)!.Points, playoffsPhase.Ranking.GetPlayerInfo(player2)!.Points);

			Assert.IsNull(playoffsPhase.Parent);

			var playoffs = new Playoffs("", null, null, null, []);
			playoffsPhase.SetParent(playoffs);
			Assert.AreEqual(playoffs, playoffsPhase.Parent);
		}

	}
}
