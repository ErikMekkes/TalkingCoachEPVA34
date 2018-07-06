using System.Diagnostics;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using Debug = UnityEngine.Debug;

/// <inheritdoc />
/// <summary>
/// Manages the speech of the coach.
/// </summary>
public class TextManager : MonoBehaviour {
    private string _voice = "Dutch Female";
    private string _language = "en-US";
	private string[] words;
	// default ESpeak text to phoneme API host, overwritten with loadHostName().
	private string hostName = "http://test.emekkes.nl";

	// Declarations for javascript text to speech callback functions
	public delegate void StartDelegate(int lastword, float elapsedTime);
	public delegate void EndDelegate(int lastword, float elapsedTime);
	public delegate void BoundaryDelegate(int lastword, float elapsedTime);
	public delegate void PauseDelegate(int lastword, float elapsedTime);
	public delegate void ResumeDelegate(int lastword, float elapsedTime);

#if UNITY_WEBGL || UNITY_EDITOR
	/// <summary>
	/// Start speaking a given text in a given voice, executing callbacks at the start and at the end of the speech.
	/// </summary>
	/// <param name="text">The text to pronounce.</param>
	/// <param name="voice">The voice to pronounce in.</param>
	/// <param name="startCallback">The function to call when speech starts.</param>
	/// <param name="endCallback">The function to call when speech ends.</param>
	/// <param name="boundaryCallback">The function to call when a word is finished.</param>
	/// <param name="pauseCallback">The function to call when speech is paused</param>
	/// <param name="resumeCallback">The function to call when speech is resumed</param>
	/// <param name="lang"></param>
	/// <returns>The state of the TTS.</returns>
	[DllImport("__Internal")]
	private static extern string Speak(
		string text,
		string voice,
		StartDelegate startCallback,
		EndDelegate endCallback,
		BoundaryDelegate boundaryCallback,
		PauseDelegate pauseCallback,
		ResumeDelegate resumeCallback,
		string lang
	);

    /// <summary>
    /// Stops the speech of the speech library.
    /// </summary>
    /// <returns></returns>
    [DllImport("__Internal")]
    private static extern string Stop();

    /// <summary>
    /// Gets the library's voices.
    /// </summary>
    /// <returns></returns>
    [DllImport("__Internal")]
    private static extern string getSystemVoices();
		
	/// <summary>
	/// Returns the hostname of the webpage unity is currently loaded on.
	/// </summary>
	/// <returns>Hostname string with protocol prefix</returns>
	[DllImport("__Internal")]
	private static extern string getHostNameString();
#endif

    private static TextManager _instance;

    private static readonly string[] Tags =
    {
        "{GLITCH}", "{SIT}", "{SAD}", "{SURPRISE}", "{HAPPY}", "{DISGUST}", "{AFRAID}", "{ANGRY}", "{FOC-}", "{FOC+}", "{EPI-}", "{DAB}",
        "{COUNTING}", "{LITTLE}", "{LISTENUP}", "{DETERMINED}", "{BIG}", "{YOU}", "{ME}", "{THIS}", "{THAT}", "{WISE}"
    };

    private const int WordsPerMinute = 200;

    private float _speed = 1;

    private static Stopwatch _watch;

    private static int _wordsAmount;


    /// <summary>
    /// Returns the singleton instance of the TextManager or instantiates it if its not
    /// instantiated.
    /// </summary>
    public static TextManager Instance {
        get {
            if (_instance != null) return _instance;
            _instance = FindObjectOfType<TextManager>();
            DontDestroyOnLoad(_instance.gameObject);

            return _instance;
        }
    }

    /// <summary>
    /// Logs all available voices.
    /// </summary>
    public static void GetVoices() {
        Debug.Log("Get Voices");
        Debug.Log(getSystemVoices());
    }

	/// <summary>
	/// Updates local hostname variable with the hostname used of the current
	/// webpage Unity is loaded in. Only useable from within the web page.
	/// </summary>
	public void loadHostName() {
		hostName = getHostNameString();
	}

	/// <summary>
	/// Sets the hostname variable to the specified hostname.
	/// </summary>
	public void setHostName(string hName) {
		hostName = hName;
	}

	/// <summary>
	/// Returns the current hostname string. Make sure it is updated with
	/// loadHostName, defaults to http://test.emekkes.nl otherwise
	/// </summary>
	/// <returns>Hostname string with protocol prefix</returns>
	public string getHostName() {
		return hostName;
	}

    /// <summary>
    /// Sets the currect voice of the coach.
    /// </summary>
    /// <param name="voice"></param>
    public void SetVoice(string voice) {
        _voice = voice;
    }

    /// <summary>
    /// Returns the current voice.
    /// </summary>
    /// <returns></returns>
    public string GetVoice() {
        return _voice;
    }

	public void StartSpeech(string text) {
		if (Application.platform != RuntimePlatform.WebGLPlayer) return;
		// First removes whitespaces left and right of the string,
		// then replaces all multiple white spaces with a single white space.
		// Ater that splits the words on a single white space.
		words = Regex.Replace(text.Trim(), @"\s+", " ").Split(' ');
		
		// text without markers
		var textString = getTextWithoutMarkers(words);
		
		// send original spoken text to SpeechAnimationManager
		SpeechAnimationManager.instance.setText(textString);
		
		StartCoroutine(LipSynchronization.getInstance.synchronize(textString, _language));
	}

	/// <summary>
	/// Should return the input text wihout emotion markers
	///
	/// Input is the array of words produced in StartSpeech.
	/// Markers are probably placed as a separate entry in this index?
	/// </summary>
	/// <param name="words"></param>
	/// <returns></returns>
	public string getTextWithoutMarkers(string[] words) {
		string result = "";
		
		if (!(words[0].StartsWith("{") && words[0].EndsWith("}"))) {
			result += words[0];
		}
		
		int size = words.Length;
		for (int i=1; i<size; i++) {
			// check if it is a marker, ignore if it is.
			if (words[i].StartsWith("{") && words[i].EndsWith("}")) {
				continue;
			}
			// add to result if it is not a marker
			result += " " + words[i];
		}

		return result;
	}

	public void startActualSpeech(string text)
	{
		
		ApplicationManager.Instance.SequenceAnimations(GetAnimations(words, WordsPerMinute * (1 + (1 - _speed) / 4)));
		// start speech, animation started with callback functions
		Speak(text, _voice, 
			callbackStart, callbackEnd, callbackBoundary, 
			callbackPause, callbackResume, _language);
	}
	
	/// <summary>
	/// Halts the speech of the coach.
	/// </summary>
	public static void StopSpeech() {
		if (Application.platform == RuntimePlatform.WebGLPlayer) {
			Stop();
		}
		ApplicationManager.Instance.StopAnimation();
	}

    public void SetLanguage(string lang) {
        _language = lang;
    }

    public string GetLanguage() {
        return _language;
    }

	public void SetSpeed(float speed) {
		_speed = speed;
	}

    public float GetSpeed() {
        return _speed;
    }

    /// <summary>
    /// Creates an array list of all the animations with the time it has to wait after the previous animation.
    /// </summary>
    /// <param name="words"></param>
    /// <param name="wpm"></param>
    /// <returns>ArrayList with pair of the animation as string and time as float.</returns>
    public static ArrayList GetAnimations(string[] words, float wpm) {
        var anims = new ArrayList();

        var wordIndex = 0;
        var previousTaggedWordIndex = 0;
        var taggedWords = 0;
        var nextAnim = "talking";
        for (var i = 0; i < words.Length; i++) {
            //foreach (string word in words) {
            foreach (var t in Tags) {
                if (!words[i].Contains(t)) continue;
                var time = (float)(i - previousTaggedWordIndex) * 60 / wpm;
                        
                anims.Add(new Pair<string, float>(nextAnim, time));

                switch (t) {
                        case "{GLITCH}":
                            nextAnim = "talking";
                            break;
                        case "{SIT}":
                            nextAnim = "sitting_talking";
                            break;
                        case "{SAD}":
                            nextAnim = "Sad";
                            break;
                        case "{HAPPY}":
                            nextAnim = "Happy";
                            break;
                        case "{DISGUST}":
                            nextAnim = "Disgust";
                            break;
                        case "{AFRAID}":
                            nextAnim = "Afraid";
                            break;
                        case "{SURPRISE}":
                            nextAnim = "Surprise";
                            break;
                        case "{ANGRY}":
                            nextAnim = "Angry";
                            break;
                        case "{DAB}":
                            nextAnim = "dab";
                            break;
                        case "{FOC+}":
                            nextAnim = "foc+";
                            break;
                        case "{FOC-}":
                            nextAnim = "foc-";
                            break;
                        case "{EPI-}":
                            nextAnim = "epi-";
                            break;
                        case "{COUNTING}":
                            nextAnim = "counting";
                            break;
                        case "{LITTLE}":
                            nextAnim = "little";
                            break;
                        case "{LISTENUP}":
                            nextAnim = "listenUp";
                            break;
                        case "{DETERMINED}":
                            nextAnim = "determined";
                            break;
                        case "{BIG}":
                            nextAnim = "big";
                            break;
                        case "{YOU}":
                            nextAnim = "you";
                            break;
                        case "{ME}":
                            nextAnim = "me";
                            break;
                        case "{THIS}":
                            nextAnim = "this";
                            break;
                        case "{THAT}":
                            nextAnim = "that";
                            break;
                        case "{WISE}":
                            nextAnim = "wise";
                            break;
                        default:
                            nextAnim = "talking";
                            break;
                }
                        
                words[wordIndex] = "";
                previousTaggedWordIndex = i;
                taggedWords++;
            }

            wordIndex++;
        }
        _wordsAmount = words.Length - taggedWords;   
        anims.Add(new Pair<string, float>(nextAnim, 10));
        return anims;
    }
    
    /// <summary>
    /// Set the boolean talk in the animator to the boolean talk.
    /// </summary>
    /// <param name="talk"></param>
    private static void SetTalk(bool talk) {
        ApplicationManager.Instance.SetTalk(talk);
    }
	
	/// <summary>
	/// Callback function for speech synthesis start event. Attached as event
	/// handler for the javascript Web Speech API.
	/// 
	/// Notifies SpeechManager that speech has started.
	/// </summary>
	/// <param name="charIndex">Index of the character in the text at which the event was triggered.</param>
	/// <param name="elapsedTime">Total time that has elapsed while speaking</param>
	[MonoPInvokeCallback(typeof(StartDelegate))]
	public static void callbackStart(int charIndex, float elapsedTime) {
		_watch = Stopwatch.StartNew();
		SetTalk(true);
		ApplicationManager.Instance.PlayAnimation();
		Debug.Log("callback start at : " + elapsedTime);
		SpeechAnimationManager.instance.startSpeechAnimation(charIndex);
	}
	
	/// <summary>
	/// Callback function for speech synthesis end event. Attached as event
	/// handler for the javascript Web Speech API.
	///
	/// Notifies SpeechManager that speech has stopped.
	/// </summary>
	/// <param name="charIndex">Index of the character in the text at which the event was triggered.</param>
	/// <param name="elapsedTime">Total time that has elapsed while speaking</param>
	[MonoPInvokeCallback(typeof(EndDelegate))]
	public static void callbackEnd(int charIndex, float elapsedTime) {
		SetTalk(false);
		Stop();
		ApplicationManager.Instance.StopAnimation();
		_watch.Stop();
		var wpm = _wordsAmount / (_watch.ElapsedMilliseconds / 60000.0);
		Debug.Log("wpm: " + (int) wpm + " (" + WordsPerMinute + ")");
		Debug.Log("callback end at : " + elapsedTime);
		// Instruct SpeechAnimationManager to stop speech animation
		SpeechAnimationManager.instance.stopSpeechAnimation(charIndex);
	}

	/// <summary>
	/// Callback function for speech synthesis onboundary event. Attached as
	/// event handler for the javascript Web Speech API.
	///
	/// Notifies SpeechManager that the next word is being spoken.
	/// </summary>
	/// <param name="charIndex">Index of the character in the text at which the event was triggered.</param>
	/// <param name="elapsedTime">Total time that has elapsed while speaking</param>
	[MonoPInvokeCallback(typeof(BoundaryDelegate))]
	public static void callbackBoundary(int charIndex, float elapsedTime) {
		// set most recently spoken word for SpeechAnimationManager
		SpeechAnimationManager.instance.onBoundary(charIndex);
	}
    
	/// <summary>
	/// Callback function for speech synthesis pause event. Attached as
	/// event handler for the javascript Web Speech API.
	/// 
	/// Notifies SpeechManager that speech has been paused.
	/// </summary>
	/// <param name="charIndex">Index of the character in the text at which the event was triggered.</param>
	/// <param name="elapsedTime">Total time that has elapsed while speaking</param>
	[MonoPInvokeCallback(typeof(PauseDelegate))]
	public static void callbackPause(int charIndex, float elapsedTime) {
		// Instruct SpeechAnimationManager to pause speech animation
		SpeechAnimationManager.instance.pauseSpeechAnimation(charIndex);
	}
    
	/// <summary>
	/// Callback function for speech synthesis resume event. Attached as
	/// event handler for the javascript Web Speech API.
	/// 
	/// Notifies SpeechManager that speech has been resumed.
	/// </summary>
	/// <param name="charIndex">Index of the character in the text at which the event was triggered.</param>
	/// <param name="elapsedTime">Total time that has elapsed while speaking</param>
	[MonoPInvokeCallback(typeof(ResumeDelegate))]
	public static void callbackResume(int charIndex, float elapsedTime) {
		// Instruct SpeechAnimationManager to resume speech animation
		SpeechAnimationManager.instance.resumeSpeechAnimation(charIndex);
	}
}