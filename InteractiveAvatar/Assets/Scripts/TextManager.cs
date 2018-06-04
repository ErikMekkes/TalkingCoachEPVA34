using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;
using Debug = UnityEngine.Debug;

/// <summary>
/// Manages the speech of the coach.
/// </summary>
public class TextManager : MonoBehaviour {
    private string _voice = "Dutch Female";
    private string _language = "en-US";

    public delegate void StartDelegate();

    public delegate void EndDelegate();

    Button btn;

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

	public static String[] TAGS = {"{GLITCH}", "{SIT}", "{SAD}", "{SURPRISE}", "{HAPPY}", "{DISGUST}", "{AFRAID}", "{ANGRY}"};

    private static int WordsPerMinute = 200;

    private static Stopwatch Watch;

    private static int wordsAmount;


    /// <summary>
    /// Returns the singleton instance of the TextManager or instantiates it if its not
    /// instantiated.
    /// </summary>
    public static TextManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<TextManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    /// <summary>
    /// Logs all available voices.
    /// </summary>
    public void GetVoices() {
        Debug.Log("Get Voices");
        Debug.Log(getSystemVoices());
    }

    /// <summary>
    /// Sets the currect voice of the coach.
    /// </summary>
    /// <param name="voice"></param>
    public void SetVoice(string voice) {
        this._voice = voice;
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
    public void StartSpeech(String text) {
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            
            // First removes whitespaces left and right of the string,
            // then replaces all multiple white spaces with a single white space.
            // Ater that splits the words on a single white space.
            string[] words = Regex.Replace(text.Trim(), @"\s+", " ").Split(' ');

            ApplicationManager.Instance.SequenceAnimations(GetAnimations(words, WordsPerMinute));
            Speak(String.Join(" ", words), _voice, CallbackStart, CallbackEnd, _language);
        }
    }

    public void setLanguage(string lang) {
        _language = lang;
    }

    /// <summary>
    /// Creates an array list of all the animations with the time it has to wait after the previous animation.
    /// </summary>
    /// <param name="words"></param>
    /// <param name="wpm"></param>
    /// <returns>ArrayList with pair of the animation as string and time as float.</returns>
    public ArrayList GetAnimations(string[] words, int wpm) {
        ArrayList anims = new ArrayList();

        int wordIndex = 0;
        int previousTaggedWordIndex = 0;
        int taggedWords = 0;
        string nextAnim = "talking";
        for (int i = 0; i < words.Length; i++) {
            //foreach (string word in words) {
            foreach (string t in TAGS) {
                if (words[i].Contains(t)) {
                    float time = (float)(i - previousTaggedWordIndex) * 60 / wpm;
                        
                    anims.Add(new Pair<String, float>(nextAnim, time));
                        
                    if (t.Equals("{GLITCH}")) {
                        nextAnim = "talking";
                    }
                    if (t.Equals("{SIT}")) {
                        nextAnim = "sitting_talking";
                    }
                    if (t.Equals("{SAD}")) {
                        nextAnim = "Sad";
                    }
                    if (t.Equals("{HAPPY}")) {
                        nextAnim = "Happy";
                    }
                    if (t.Equals("{DISGUST}")) {
                        nextAnim = "Disgust";
                    }
                    if (t.Equals("{AFRAID}")) {
                        nextAnim = "Afraid";
                    }
                    if (t.Equals("{SURPRISE}")) {
                        nextAnim = "Surprise";
                    }
                    if (t.Equals("{ANGRY}")) {
                        nextAnim = "Angry";
                    }
                        
                    words[wordIndex] = "";
                    previousTaggedWordIndex = i;
                    taggedWords++;
                }
            }

            wordIndex++;
        }
        
        wordsAmount = words.Length - taggedWords;
            
        anims.Add(new Pair<String, float>(nextAnim, 10));
        
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
    public void StopSpeech() {
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
        Watch = Stopwatch.StartNew();
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
        Watch.Stop();
        double wpm = wordsAmount / (Watch.ElapsedMilliseconds / 60000.0);
        Debug.Log("wpm: " + (int) wpm + " (" + WordsPerMinute + ")");
    }
}