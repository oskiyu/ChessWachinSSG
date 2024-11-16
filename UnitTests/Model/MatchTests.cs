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
	public class MatchTests {

		[TestMethod]
		public void Test() {
			var player1 = new Player("id1", "name1", new("", "", ""), "", "tag1");
			var player2 = new Player("id2", "name2", new("", "", ""), "", "tag2");
			var match = new Match(player1, player2, Winner.First, 5, 6, 7, "url", "date");

			Assert.AreEqual(player1, match.First);
			Assert.AreEqual(player2, match.Second);
			Assert.AreEqual(Winner.First, match.Result);
			Assert.AreEqual(5, match.Moves);
			Assert.AreEqual(6, match.DurationSeconds);
			Assert.AreEqual(7, match.DurationTypeSeconds);
			Assert.AreEqual("url", match.Url);
			Assert.AreEqual("date", match.Date);
		}

	}

}
