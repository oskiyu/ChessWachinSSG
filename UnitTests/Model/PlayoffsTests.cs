using ChessWachinSSG.Data.Dtos;
using ChessWachinSSG.Model;

namespace UnitTests.Model
{

    [TestClass]
	public class PlayoffsTests {

		[TestMethod]
		public void EmptyTest() {
			var playoffs = new Playoffs("id", null, null, null, []);

			Assert.AreEqual("id", playoffs.Id);
			Assert.AreEqual(0, playoffs.DefaultDurations.Count);

			Assert.IsNull(playoffs.Semifinals1);
			Assert.IsNull(playoffs.Semifinals2);
			Assert.IsNull(playoffs.Finals);

			Assert.IsNull(playoffs.Winner);
			Assert.IsNull(playoffs.Second);

			Assert.IsNull(playoffs.GetPhase(""));
		}

		[TestMethod]
		public void WithSemifinalsTest() {
			var sf1 = new PlayoffsRound("sf1", "Sf1", new MatchList.Builder().Build());
			var sf2 = new PlayoffsRound("sf2", "Sf2", new MatchList.Builder().Build());
			var playoffs = new Playoffs("id", sf1, sf2, null, []);

			Assert.AreEqual("id", playoffs.Id);
			Assert.AreEqual(0, playoffs.DefaultDurations.Count);

			Assert.AreEqual(sf1, playoffs.Semifinals1);
			Assert.AreEqual(sf2, playoffs.Semifinals2);

			Assert.IsNull(playoffs.Finals);

			Assert.IsNull(playoffs.Winner);
			Assert.IsNull(playoffs.Second);

			Assert.AreEqual(sf1, playoffs.GetPhase("sf1"));
			Assert.AreEqual(sf2, playoffs.GetPhase("sf2"));

			Assert.IsNull(playoffs.GetPhase(""));
		}

		[TestMethod]
		public void WithFinalsTest() {
			var player1 = new Player("id1", "name1", new("", "", ""), "", "tag1");
			var player2 = new Player("id2", "name2", new("", "", ""), "", "tag2");
			var matches = new MatchList.Builder()
				.AddMatch(new(player1, player2, Winner.First, 5, 6, 7, "", ""))
				.AddMatch(new(player1, player2, Winner.Draw, 5, 6, 7, "", ""))
				.AddMatch(new(player2, player1, Winner.Second, 5, 6, 7, "", ""))
				.Build();

			var sf1 = new PlayoffsRound("sf1", "Sf1", matches);
			var sf2 = new PlayoffsRound("sf2", "Sf2", matches);
			var final = new PlayoffsRound("final", "Final", matches);
			var playoffs = new Playoffs("id", sf1, sf2, final, []);

			Assert.AreEqual("id", playoffs.Id);
			Assert.AreEqual(0, playoffs.DefaultDurations.Count);

			Assert.AreEqual(sf1, playoffs.Semifinals1);
			Assert.AreEqual(sf2, playoffs.Semifinals2);

			Assert.AreEqual(final, playoffs.Finals);

			Assert.IsNotNull(playoffs.Winner);
			Assert.IsNotNull(playoffs.Second);

			Assert.AreEqual(sf1, playoffs.GetPhase("sf1"));
			Assert.AreEqual(sf2, playoffs.GetPhase("sf2"));
			Assert.AreEqual(final, playoffs.GetPhase("final"));
		}

	}

}
