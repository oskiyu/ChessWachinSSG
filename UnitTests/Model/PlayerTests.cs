using ChessWachinSSG.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Model {

	[TestClass]
	public class PlayerTests {

		[TestMethod]
		public void WithPfp() {
			var country = new Country("cid", "cname", "fp");
			var player = new Player("id", "name", country, "pfp", "tag");

			Assert.AreEqual("id", player.Id);
			Assert.AreEqual("name", player.Name);
			Assert.AreEqual(country, player.Country);
			Assert.AreEqual("pfp", player.PfpPath);
			Assert.AreEqual("tag", player.NameTag);
		}

		[TestMethod]
		public void WithoutPfp() {
			var country = new Country("cid", "cname", "fp");
			var player = new Player("id", "name", country, null, "tag");

			Assert.AreEqual("id", player.Id);
			Assert.AreEqual("name", player.Name);
			Assert.AreEqual(country, player.Country);
			Assert.AreEqual("tag", player.NameTag);

			Assert.IsNull(player.PfpPath);
		}

	}

}
