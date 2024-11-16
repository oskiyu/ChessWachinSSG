using ChessWachinSSG;
using ChessWachinSSG.Data;
using ChessWachinSSG.Model;

using NSubstitute;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests {

	[TestClass]
	public class GlobalsTests {

		[TestMethod]
		public void GetMedals_Finished() {
			Assert.AreEqual(Globals.GoldMedal, Globals.GetLeagueMedal(0, true));
			Assert.AreEqual(Globals.SilverMedal, Globals.GetLeagueMedal(1, true));
			Assert.AreEqual(Globals.BronzeMedal, Globals.GetLeagueMedal(2, true));

			Assert.AreEqual(string.Empty, Globals.GetLeagueMedal(3, true));
			Assert.AreEqual(string.Empty, Globals.GetLeagueMedal(10, true));
			Assert.AreEqual(string.Empty, Globals.GetLeagueMedal(-1, true));
		}

		[TestMethod]
		public void GetMedals_Unfinished() {
			Assert.AreEqual(string.Empty, Globals.GetLeagueMedal(0, false));
			Assert.AreEqual(string.Empty, Globals.GetLeagueMedal(1, false));
			Assert.AreEqual(string.Empty, Globals.GetLeagueMedal(2, false));

			Assert.AreEqual(string.Empty, Globals.GetLeagueMedal(3, false));
			Assert.AreEqual(string.Empty, Globals.GetLeagueMedal(10, false));
			Assert.AreEqual(string.Empty, Globals.GetLeagueMedal(-1, false));
		}

	}

}
