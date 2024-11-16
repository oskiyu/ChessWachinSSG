using ChessWachinSSG.Model;

namespace UnitTests.Model
{

    [TestClass]
    public class LeagueTests
    {

        [TestMethod]
        public void MatchCountTest()
        {
            Assert.AreEqual(30, League.GetNumMatches(6));
            Assert.AreEqual(42, League.GetNumMatches(7));
        }

    }

}
