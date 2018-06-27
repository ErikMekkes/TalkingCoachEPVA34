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

    private delegate void StartDelegate();

    private delegate void EndDelegate();

#if UNITY_WEBGL || UNITY_EDITOR
    /// <summary>
    /// /The speech method for the coach. This method is linked with the speech library.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="voice"></param>
    /// <param name="startCallback"></param>
    /// <param name="endCallback"></param>
    /// <param name="lang"></param>
    /// <returns></returns>
    [DllImport("__Internal")]
    private static extern string Speak(string text, string voice, StartDelegate startCallback, EndDelegate endCallback, string lang);

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

    /// <summary>
    /// Initializes the speech cycle of the coach using the given text, a voice, a callback start
    /// and a callback end. CallbackStart and -End are called at the beginning and end of the speech
    /// respectively.
    /// </summary>
    /// <param name="text"></param>
	public void StartSpeech(string text) {
        if (Application.platform != RuntimePlatform.WebGLPlayer) return;
        // First removes whitespaces left and right of the string,
        // then replaces all multiple white spaces with a single white space.
        // Ater that splits the words on a single white space.
        var words = Regex.Replace(text.Trim(), @"\s+", " ").Split(' ');

        ApplicationManager.Instance.SequenceAnimations(GetAnimations(words, WordsPerMinute * (1 + (1 - _speed) / 4)));
        Speak(string.Join(" ", words), _voice, CallbackStart, CallbackEnd, _language);
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
    /// Halts the speech of the coach.
    /// </summary>
    public static void StopSpeech() {
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            Stop();
        }
        ApplicationManager.Instance.StopAnimation();
    }

    /// <summary>
    /// Lets the ApplicationManager play the 'talking' animation.
    /// </summary>
    [MonoPInvokeCallback(typeof(StartDelegate))]
    public static void CallbackStart() {
        _watch = Stopwatch.StartNew();
        Debug.Log("callback start");
        SetTalk(true);
        ApplicationManager.Instance.PlayAnimation();
    }

    /// <summary>
    /// Lets the ApplicationManager play the 'idle' animation.
    /// </summary>
    [MonoPInvokeCallback(typeof(EndDelegate))]
    public static void CallbackEnd() {
        Debug.Log("callback ended");
        SetTalk(false);
		Stop();
        ApplicationManager.Instance.StopAnimation();
        _watch.Stop();
        var wpm = _wordsAmount / (_watch.ElapsedMilliseconds / 60000.0);
        Debug.Log("wpm: " + (int) wpm + " (" + WordsPerMinute + ")");
    }
}