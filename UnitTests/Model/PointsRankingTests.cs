using ChessWachinSSG.Model;
using ChessWachinSSG.Data.Dtos;

namespace UnitTests.Model
{

    [TestClass]
    public class PointsRankingTests
    {

        [TestMethod]
        public void SingleWinTest()
        {
            var player1 = new Player("id1", "name1", new("", "", ""), "", "tag1");
            var player2 = new Player("id2", "name2", new("", "", ""), "", "tag2");
            var ranking = new PointsRanking.Builder().ApplyMatch(new(player1, player2, Winner.First, 5, 6, 7, "", "")).Build();

            var ordered = ranking.Ranking;

            Assert.AreEqual(1, ranking.GetPlayerInfo(player1)!.Wins);
            Assert.AreEqual(0, ranking.GetPlayerInfo(player1)!.Draws);
            Assert.AreEqual(0, ranking.GetPlayerInfo(player1)!.Losses);

            Assert.AreEqual(0, ranking.GetPlayerInfo(player2)!.Wins);
            Assert.AreEqual(0, ranking.GetPlayerInfo(player2)!.Draws);
            Assert.AreEqual(1, ranking.GetPlayerInfo(player2)!.Losses);

            Assert.AreEqual(2, ranking.GetPlayerInfo(player1)!.Points);
            Assert.AreEqual(0, ranking.GetPlayerInfo(player2)!.Points);
        }

        [TestMethod]
        public void SingleDrawTest()
        {
            var player1 = new Player("id1", "name1", new("", "", ""), "", "tag1");
            var player2 = new Player("id2", "name2", new("", "", ""), "", "tag2");
            var ranking = new PointsRanking.Builder().ApplyMatch(new(player1, player2, Winner.Draw, 5, 6, 7, "", "")).Build();

            var ordered = ranking.Ranking;

            Assert.AreEqual(0, ranking.GetPlayerInfo(player1)!.Wins);
            Assert.AreEqual(1, ranking.GetPlayerInfo(player1)!.Draws);
            Assert.AreEqual(0, ranking.GetPlayerInfo(player1)!.Losses);

            Assert.AreEqual(0, ranking.GetPlayerInfo(player2)!.Wins);
            Assert.AreEqual(1, ranking.GetPlayerInfo(player2)!.Draws);
            Assert.AreEqual(0, ranking.GetPlayerInfo(player2)!.Losses);

            Assert.AreEqual(1, ranking.GetPlayerInfo(player1)!.Points);
            Assert.AreEqual(1, ranking.GetPlayerInfo(player2)!.Points);
        }

        [TestMethod]
        public void SingleLossTest()
        {
            var player1 = new Player("id1", "name1", new("", "", ""), "", "tag1");
            var player2 = new Player("id2", "name2", new("", "", ""), "", "tag2");
            var ranking = new PointsRanking.Builder().ApplyMatch(new(player1, player2, Winner.Second, 5, 6, 7, "", "")).Build();

            var ordered = ranking.Ranking;

            Assert.AreEqual(0, ranking.GetPlayerInfo(player1)!.Wins);
            Assert.AreEqual(0, ranking.GetPlayerInfo(player1)!.Draws);
            Assert.AreEqual(1, ranking.GetPlayerInfo(player1)!.Losses);

            Assert.AreEqual(1, ranking.GetPlayerInfo(player2)!.Wins);
            Assert.AreEqual(0, ranking.GetPlayerInfo(player2)!.Draws);
            Assert.AreEqual(0, ranking.GetPlayerInfo(player2)!.Losses);

            Assert.AreEqual(0, ranking.GetPlayerInfo(player1)!.Points);
            Assert.AreEqual(2, ranking.GetPlayerInfo(player2)!.Points);
        }

        [TestMethod]
        public void MultipleMatchesTest()
        {
            var player1 = new Player("id1", "name1", new("", "", ""), "", "tag1");
            var player2 = new Player("id2", "name2", new("", "", ""), "", "tag2");
            var matches = new MatchList.Builder()
                .AddMatch(new(player1, player2, Winner.First, 5, 6, 7, "", ""))
                .AddMatch(new(player1, player2, Winner.Draw, 5, 6, 7, "", ""))
                .AddMatch(new(player2, player1, Winner.Second, 5, 6, 7, "", ""))
                .Build();

            var ranking = new PointsRanking.Builder().ApplyAllMatches(matches).Build();

            Assert.AreEqual(0, ranking.GetPlayerPosition(player1));
            Assert.AreEqual(1, ranking.GetPlayerPosition(player2));

            Assert.AreEqual(5, ranking.GetPlayerInfo(player1)!.Points);
            Assert.AreEqual(1, ranking.GetPlayerInfo(player2)!.Points);
        }

        [TestMethod]
        public void NonExistingPlayerTest()
        {
            var ranking = new PointsRanking.Builder().Build();

            Assert.IsNull(ranking.GetPlayerInfo("ID"));
            Assert.IsNull(ranking.GetPlayerInfo(new Player("", "", new("", "", ""), "", "")));

            Assert.AreEqual(-1, ranking.GetPlayerPosition("ID"));
            Assert.AreEqual(-1, ranking.GetPlayerPosition(new Player("", "", new("", "", ""), "", "")));
        }

    }

}
