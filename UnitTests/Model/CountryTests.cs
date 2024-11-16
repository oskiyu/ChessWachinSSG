using ChessWachinSSG.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Model {

	[TestClass]
	public class CountryTests {

		[TestMethod]
		public void Test() {
			var country = new Country("id", "name", "fip");

			Assert.AreEqual("id", country.Id);
			Assert.AreEqual("name", country.Name);
			Assert.AreEqual("fip", country.FlagIconPath);
		}

	}

}
