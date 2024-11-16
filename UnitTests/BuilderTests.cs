using ChessWachinSSG.HTML.Tags;
using ChessWachinSSG.Model;

namespace UnitTests {

	[TestClass]
	public class BuilderTests {

		[TestMethod]
		public void BuilderTest() {
			var exampleText = "<test:text>";
			var exampleRemplacer = new Tr_Inline("OK");
			Dictionary<string, ITagReplacer> replacers = new(){
				{ "test:text", exampleRemplacer }
			};

			var result = ChessWachinSSG.HTML.HtmlBuilder.Process(exampleText, Context.Empty, replacers);

			Assert.AreEqual("OK", result);
		}

		[TestMethod]
		public void RecursiveReplace() {
			var exampleText = "<test:text>";
			var exampleRemplacer = new Tr_Inline("<test2>");
			var exampleRemplacer2 = new Tr_Inline("XD");
			Dictionary<string, ITagReplacer> replacers = new(){
				{ "test:text", exampleRemplacer },
				{ "test2", exampleRemplacer2 }
			};

			var result = ChessWachinSSG.HTML.HtmlBuilder.Process(exampleText, Context.Empty, replacers);

			Assert.AreEqual("XD", result);
		}

	}

}
