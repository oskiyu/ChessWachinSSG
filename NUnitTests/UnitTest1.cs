using ChessWachinSSG.HTML.Tags;
using ChessWachinSSG.Model;

namespace NUnitTests {

	public class Tests {

		[Test]
		public void Test1() {
			var exampleText = "<test:texzt>";
			var exampleRemplacer = new InlineTagReplacer("OK");
			Dictionary<string, ITagReplacer> replacers = new(){
				{ "<test:texzt>", exampleRemplacer }
			};

			var result = ChessWachinSSG.HTML.HtmlBuilder.Process(exampleText, Context.Empty, replacers);

			Assert.That(result, Is.EqualTo("OK"));
		}
	}

}