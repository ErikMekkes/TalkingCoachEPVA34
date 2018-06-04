using System.Collections;
using NUnit.Framework;

/// <summary>
/// General test suite class.
/// </summary>
public class TestSuite {

	/// <summary>
	/// Test for the setter and getter of the voice.
	/// </summary>
	[Test]
	public void GetSetVoiceTest() {
		TextManager textManager = new TextManager();
		string testvoice = "testvoice*#!_+.,"; // most likely not the same as the current voice of the text manager.
		Assert.AreNotEqual(textManager.GetVoice(), testvoice);
		textManager.SetVoice(testvoice);
		Assert.AreEqual(textManager.GetVoice(), testvoice);
	}

	/// <summary>
	/// Test for the animation list created after the string is parsed.
	/// </summary>
	[Test]
	public void GetAnimationsTest() {
		TextManager textManager = new TextManager();
		string testString = "This is a test sentence with a {HAPPY} happy emotion and a {SAD} sad emotion.";
		string[] splitted = testString.Split(' ');
		ArrayList result = textManager.GetAnimations(splitted, 160);
		ArrayList anims = new ArrayList();
		anims.Add(new Pair<string, float>("talking", 2.625f));
		anims.Add(new Pair<string, float>("Happy", 1.875f));
		anims.Add(new Pair<string, float>("Sad", 10f));
		//int max = MaxValue(result.Count, anims.Count);
		for (int i = 0; i < result.Count; i++)
		{
			Assert.AreEqual(((Pair<string, float>) result[i]).GetA(), ((Pair<string, float>) anims[i]).GetA());
			Assert.AreEqual(((Pair<string, float>) result[i]).GetB(), ((Pair<string, float>) anims[i]).GetB());
		}
	}
}
