using ChessWachinSSG.HTML;

namespace UnitTests {

	[TestClass]
	public class TagReaderTests {

		[TestMethod]
		public void SingleTag() {
			string tagInner = "alaverga";
			string tagText = $"<{tagInner}>";
			string text = $"{tagText}";

			var tags = TagReader.ReadAllTags(text);

			Assert.AreEqual(1, tags.Count, "Tag count");
			Assert.AreEqual(tagInner, tags[0].Id, "Content");
			Assert.AreEqual(tags[0].TotalCharacters, tagText.Length);
			Assert.AreEqual(0, tags[0].Arguments.Count);
		}

		[TestMethod]
		public void SingleTag_WithOuterSpaces() {
			string tagInner = "alaverga";
			string tagText = $"<{tagInner}>";
			string text = $" {tagText} ";

			var tags = TagReader.ReadAllTags(text);

			Assert.AreEqual(1, tags.Count, "Tag count");
			Assert.AreEqual(tagInner, tags[0].Id, "Content");
			Assert.AreEqual(tags[0].TotalCharacters, tagText.Length);
			Assert.AreEqual(0, tags[0].Arguments.Count);
		}

		[TestMethod]
		public void SingleTag_WithInnerSpaces() {
			string tagInner = "alaverga";
			string tagText = $"< {tagInner} >";
			string text = $"{tagText}";

			var tags = TagReader.ReadAllTags(text);

			Assert.AreEqual(1, tags.Count, "Tag count");
			Assert.AreEqual(tagInner, tags[0].Id, "Content");
			Assert.AreEqual(tags[0].TotalCharacters, tagText.Length);
			Assert.AreEqual(0, tags[0].Arguments.Count);
		}

		[TestMethod]
		public void SingleTag_WithLeftInnerSpace() {
			string tagInner = "alaverga";
			string tagText = $"< {tagInner}>";
			string text = $"{tagText}";

			var tags = TagReader.ReadAllTags(text);

			Assert.AreEqual(1, tags.Count, "Tag count");
			Assert.AreEqual(tagInner, tags[0].Id, "Content");
			Assert.AreEqual(tags[0].TotalCharacters, tagText.Length);
			Assert.AreEqual(0, tags[0].Arguments.Count);
		}

		[TestMethod]
		public void SingleTag_WithRightInnerSpace() {
			string tagInner = "alaverga";
			string tagText = $"<{tagInner} >";
			string text = $"{tagText}";

			var tags = TagReader.ReadAllTags(text);

			Assert.AreEqual(1, tags.Count, "Tag count");
			Assert.AreEqual(tagInner, tags[0].Id, "Content");
			Assert.AreEqual(tags[0].TotalCharacters, tagText.Length);
			Assert.AreEqual(0, tags[0].Arguments.Count);
		}

		[TestMethod]
		public void SingleTag_WithArguments() {
			string tagInner = "alaverga";
			string tagText = $"<{tagInner} arg1 arg2>";
			string text = $"{tagText}";

			var tags = TagReader.ReadAllTags(text);

			Assert.AreEqual(1, tags.Count, "Tag count");
			Assert.AreEqual(tagInner, tags[0].Id, "Content");
			Assert.AreEqual(tags[0].TotalCharacters, tagText.Length);
			Assert.AreEqual(2, tags[0].Arguments.Count);

			Assert.AreEqual("arg1", tags[0].Arguments[0]);
			Assert.AreEqual("arg2", tags[0].Arguments[1]);
		}

		[TestMethod]
		public void TagStartIndex() {
			string text = "abcd<XD>";

			var tags = TagReader.ReadAllTags(text);

			Assert.AreEqual(1, tags.Count, "Tag count");
			Assert.AreEqual(4, tags[0].FirstCharacterInex);
		}

	}

}	
