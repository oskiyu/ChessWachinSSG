using ChessWachinSSG.Data;
using ChessWachinSSG.Data.Daos;
using ChessWachinSSG.Data.Dtos;

using NSubstitute;

using System.Text;

namespace UnitTests {

	[TestClass]
	public class DataDaoTests {

		[TestMethod]
		public void CountryDao_Test() {
			var reader = Substitute.For<IFileReader>();
			reader.Exists("").Returns(true);
			reader.GetStream("").Returns(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes("""
				[
				    {
				        "id": "es",
				        "name": "España",
				        "flagIcon": "Flags/flagSpain.png"
				    },
				    {
				        "id": "uy",
				        "name": "Uruguay",
				        "flagIcon": "Flags/flagUruguay.png"
				    },
				    {
				        "id": "esmo",
				        "name": "España-Marruecos",
				        "flagIcon": "Flags/flagMoroccoSpain.png"
				    }
				]
				
				"""))));

			var dao = CountryDao.FromFile("", reader);
			var countres = dao.GetAllCountries();

			Assert.AreEqual(3, countres.Count);

			Assert.AreEqual("es", countres[0].Id);
			Assert.AreEqual("uy", countres[1].Id);
			Assert.AreEqual("esmo", countres[2].Id);

			Assert.AreEqual("España", countres[0].Name);
			Assert.AreEqual("Uruguay", countres[1].Name);
			Assert.AreEqual("España-Marruecos", countres[2].Name);

			Assert.AreEqual("Flags/flagSpain.png", countres[0].FlagIconPath);
			Assert.AreEqual("Flags/flagUruguay.png", countres[1].FlagIconPath);
			Assert.AreEqual("Flags/flagMoroccoSpain.png", countres[2].FlagIconPath);
		}


		[TestMethod]
		public void CountryDao_Empty_Test() {
			var dao = CountryDao.Empty;
			Assert.AreEqual(0, dao.GetAllCountries().Count);
		}


		[TestMethod]
		public void MatchDao_Test() {
			var reader = Substitute.For<IFileReader>();
			reader.Exists("").Returns(true);
			reader.GetStream("").Returns(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes("""
				[
					{
						"player1": "Ramboromir",
						"player2": "Chechu_25",
						"result": "1",
						"moves": 56,
						"duration": 776,
						"duration_type": 600,
						"link": "https://www.chess.com/game/live/107538146547",
						"date": "22 abril 2024"
					},
					{
						"player1": "Chechu_25",
						"player2": "MrMhem",
						"result": "D",
						"moves": 58,
						"duration": 826,
						"duration_type": 600,
						"link": "https://www.chess.com/game/live/107538146547",
						"date": "22 abril 2024"
					},
					{
						"player1": "charliecc13",
						"player2": "MrMhem",
						"result": "0",
						"moves": 29,
						"duration": 800,
						"duration_type": 600,
						"link": "https://www.chess.com/game/live/107543587731",
						"date": "22 abril 2024"
					}
				]
				"""))));

			var dao = MatchDao.FromFile("", reader);
			var matches = dao.GetAllMatches();

			Assert.AreEqual(3, matches.Count);

			Assert.AreEqual("Ramboromir", matches[0].FirstPlayerId);
			Assert.AreEqual("Chechu_25", matches[0].SecondPlayerId);
			Assert.AreEqual(Winner.Second, matches[0].Winner);
			Assert.AreEqual(56, matches[0].Moves);
			Assert.AreEqual(776, matches[0].Duration);
			Assert.AreEqual(600, matches[0].DurationType);
			Assert.AreEqual("https://www.chess.com/game/live/107538146547", matches[0].Url);
			Assert.AreEqual("22 abril 2024", matches[0].Date);

			Assert.AreEqual("Chechu_25", matches[1].FirstPlayerId);
			Assert.AreEqual("MrMhem", matches[1].SecondPlayerId);
			Assert.AreEqual(Winner.Draw, matches[1].Winner);
			Assert.AreEqual(58, matches[1].Moves);
			Assert.AreEqual(826, matches[1].Duration);
			Assert.AreEqual(600, matches[1].DurationType);
			Assert.AreEqual("https://www.chess.com/game/live/107538146547", matches[1].Url);
			Assert.AreEqual("22 abril 2024", matches[1].Date);

			Assert.AreEqual("charliecc13", matches[2].FirstPlayerId);
			Assert.AreEqual("MrMhem", matches[2].SecondPlayerId);
			Assert.AreEqual(Winner.First, matches[2].Winner);
			Assert.AreEqual(29, matches[2].Moves);
			Assert.AreEqual(800, matches[2].Duration);
			Assert.AreEqual(600, matches[2].DurationType);
			Assert.AreEqual("https://www.chess.com/game/live/107543587731", matches[2].Url);
			Assert.AreEqual("22 abril 2024", matches[2].Date);
		}

		[TestMethod]
		public void MatchDao_Empty_Test() {
			var dao = MatchDao.Empty;
			Assert.AreEqual(0, dao.GetAllMatches().Count);
		}
		[TestMethod]
		public void PlayerDao_Test() {
			var reader = Substitute.For<IFileReader>();
			reader.Exists("").Returns(true);
			reader.GetStream("").Returns(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes("""
				[
					{
						"id": "oskiyu2",
						"name": "oskiyu2",
						"country": "es",
						"pfp": "pfps/oskiyu.png",
						"nametag": "cwssg:pfl:oskiyu"
					},
					{
						"id": "martilux2580",
						"name": "martilux2580",
						"country": "uy",
						"nametag": "cwssg:pfl:martilux"
					}
				]
				"""))));

			var dao = PlayerDao.FromFile("", reader);
			var players = dao.GetAllPlayers();

			Assert.AreEqual(2, players.Count);

			Assert.AreEqual("oskiyu2", players[0].Id);
			Assert.AreEqual("oskiyu2", players[0].Name);
			Assert.AreEqual("es", players[0].CountryId);
			Assert.AreEqual("pfps/oskiyu.png", players[0].PfpPath!);
			Assert.AreEqual("cwssg:pfl:oskiyu", players[0].NameTag);

			Assert.AreEqual("martilux2580", players[1].Id);
			Assert.AreEqual("martilux2580", players[1].Name);
			Assert.AreEqual("uy", players[1].CountryId);
			Assert.IsNull(players[1].PfpPath);
			Assert.AreEqual("cwssg:pfl:martilux", players[1].NameTag);
		}

		[TestMethod]
		public void PlayerDao_Empty_Test() {
			var dao = PlayerDao.Empty;
			Assert.AreEqual(0, dao.GetAllPlayers().Count);
		}

		[TestMethod]
		public void TagDao_Test() {
			var reader = Substitute.For<IFileReader>();
			reader.Exists("").Returns(true);
			reader.GetStream("").Returns(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes("""
				[
					{
						"tag": "cwssg:footer",
						"path": "Sources/footer.html"
					},
					{
						"tag": "cwssg:league:playerlosses",
						"internal": "league-ranking-entry-playerlosses"
					},

					{
						"tag": "cwssg:inline",
						"inline": "XDDD"
					}
				]
				"""))));

			var dao = TagDao.FromFile("", reader);
			var tags = dao.GetAllTags();

			Assert.AreEqual(3, tags.Count);

			Assert.AreEqual("cwssg:footer", tags[0].Tag);
			Assert.AreEqual("Sources/footer.html", tags[0].TagReplacerData);
			Assert.AreEqual(ChessWachinSSG.HTML.Tags.TagReplacerType.File, tags[0].ReplacerType);

			Assert.AreEqual("cwssg:league:playerlosses", tags[1].Tag);
			Assert.AreEqual("league-ranking-entry-playerlosses", tags[1].TagReplacerData);
			Assert.AreEqual(ChessWachinSSG.HTML.Tags.TagReplacerType.Internal, tags[1].ReplacerType);

			Assert.AreEqual("cwssg:inline", tags[2].Tag);
			Assert.AreEqual("XDDD", tags[2].TagReplacerData);
			Assert.AreEqual(ChessWachinSSG.HTML.Tags.TagReplacerType.Inline, tags[2].ReplacerType);
		}

		[TestMethod]
		public void TagDao_Empty_Test() {
			var dao = TagDao.Empty;
			Assert.AreEqual(0, dao.GetAllTags().Count);
		}

	}

}
