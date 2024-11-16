using ChessWachinSSG.Data;
using ChessWachinSSG.Data.Dtos;
using ChessWachinSSG.HTML.Tags;
using ChessWachinSSG.Model;

using System.Diagnostics.Metrics;

namespace UnitTests
{

    [TestClass]
	public class DataClassesTests {

		[TestMethod]
		public void CountryDataTest() {
			var country = new CountryDto("id", "name", "path");

			Assert.AreEqual("id", country.Id);
			Assert.AreEqual("name", country.Name);
			Assert.AreEqual("path", country.FlagIconPath);
		}

		[TestMethod]
		public void MatchDataTest() {
			var match = new MatchDto("firstId", "secondId", Winner.Draw, 5, 6, 7, "url", "date");

			Assert.AreEqual("firstId", match.FirstPlayerId);
			Assert.AreEqual("secondId", match.SecondPlayerId);
			Assert.AreEqual(Winner.Draw, match.Winner);
			Assert.AreEqual(5, match.Moves);
			Assert.AreEqual(6, match.Duration);
			Assert.AreEqual(7, match.DurationType);
			Assert.AreEqual("url", match.Url);
			Assert.AreEqual("date", match.Date);
		}

		[TestMethod]
		public void PlayerDataTest() {
			var player = new PlayerDto("id", "name", "country", "path", "tag");

			Assert.AreEqual("id", player.Id);
			Assert.AreEqual("name", player.Name);
			Assert.AreEqual("country", player.CountryId);
			Assert.AreEqual("path", player.PfpPath);
			Assert.AreEqual("tag", player.NameTag);
		}

		[TestMethod]
		public void TagDataTest() {
			var tag = new TagDto("tag", TagReplacerType.Internal, "data");

			Assert.AreEqual("tag", tag.Tag);
			Assert.AreEqual(TagReplacerType.Internal, tag.ReplacerType);
			Assert.AreEqual("data", tag.TagReplacerData);
		}

	}

}
