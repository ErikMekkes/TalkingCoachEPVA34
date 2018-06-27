using System.Collections;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

/// <summary>
/// General test suite class.
/// </summary>
[TestFixture]
public class TestSuite {

	private TextManager _textManager;
	private string _testString;
	private float _testFloat;

	/// <summary>
	/// Runs before each test case.
	///
	/// The text manager is instantiated in this way because it is in a test.
	/// It fails if it is instantiated in a normal way.
	/// </summary>
	[SetUp]
	[SuppressMessage("ReSharper", "Unity.IncorrectMonoBehaviourInstantiation")]
	public void SetUp() {
		_textManager = new TextManager();
		// A string which is most likely not yet appearing in the text manager.
		_testString = "testvoice*#!_+.,";
		// A float which is most likely not yet appearing in the text manager.
		_testFloat = 12345f;
	}

	/// <summary>
	/// Test for the setter and getter of the voice field.
	/// </summary>
	[Test]
	public void GetSetVoiceTest() {
		const string message = "Test fails because the teststring is accidentally equal to " +
		                       "the original voice string in the text manager. Fix this by " +
		                       "changing the teststring to a more unique one which succeeds " +
		                       "with a higher probability";
		Assert.AreNotEqual(_testString, _textManager.GetVoice(), message);
		_textManager.SetVoice(_testString);
		Assert.AreEqual(_testString, _textManager.GetVoice());
	}

	/// <summary>
	/// Test for the setter and getter of the language field.
	/// </summary>
	[Test]
	public void GetSetLanguageTest() {
		const string message = "Test fails because the teststring is accidentally equal to " +
		                       "the original language string in the text manager. Fix this by " +
		                       "changing the teststring to a more unique one which succeeds " +
		                       "with a higher probability";
		Assert.AreNotEqual(_testString, _textManager.GetLanguage(), message);
		_textManager.SetLanguage(_testString);
		Assert.AreEqual(_testString, _textManager.GetLanguage());
	}

	/// <summary>
	/// Test for the setter and getter of the speed field.
	/// </summary>
	[Test]
	public void GetSetSpeedTest() {
		const string message = "Test fails because the testfloat is accidentally equal to " +
		                       "the original speed float in the text manager. Fix this by " +
		                       "changing the teststring to a more unique one which succeeds " +
		                       "with a higher probability";
		Assert.AreNotEqual(_testFloat, _textManager.GetSpeed(), message);
		_textManager.SetSpeed(_testFloat);
		Assert.AreEqual(_testFloat, _textManager.GetSpeed());
	}

	/// <summary>
	/// Test for the animation list created after the string is parsed.
	/// </summary>
	[Test]
	public void GetAnimationsTest() {
		const string testString = "This is a test sentence with a {HAPPY} happy emotion and a {SAD} sad emotion.";
		var splitted = testString.Split(' ');
		var result = TextManager.GetAnimations(splitted, 160);
		var anims = new ArrayList {
			new Pair<string, float>("talking", 2.625f),
			new Pair<string, float>("Happy", 1.875f),
			new Pair<string, float>("Sad", 10f)
		};
		//int max = MaxValue(result.Count, anims.Count);
		for (var i = 0; i < result.Count; i++)
		{
			Assert.AreEqual(((Pair<string, float>) result[i]).GetA(), ((Pair<string, float>) anims[i]).GetA());
			Assert.AreEqual(((Pair<string, float>) result[i]).GetB(), ((Pair<string, float>) anims[i]).GetB());
		}
	}
}
