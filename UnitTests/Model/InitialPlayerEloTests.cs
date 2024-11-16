using ChessWachinSSG.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Model {

	[TestClass]
	public class InitialPlayerEloTests {

		[TestMethod]
		public void Test() {
			var player = new Player("", "", new Country("", "", ""), null, "");
			var initialPlayerElo = new InitialPlayerElo(player, 100);

			Assert.AreEqual(player, initialPlayerElo.Player);
			Assert.AreEqual(100, initialPlayerElo.Elo);
		}

	}

}
