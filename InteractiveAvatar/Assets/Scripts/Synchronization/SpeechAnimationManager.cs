using System.Collections.Generic;
using Models;
using UnityEngine;

public class SpeechAnimationManager : MonoBehaviour {
    // Avatar model
    private GameObject newCoach;
    // Unity Animator component for the coach 
    private Animator animator;
    
    // layer for viseme (speech) animation
    private int visemeLayer = 1;

    // whether the speech animation is active
    private bool isSpeaking = false;
    
    // time elapsed since start of last viseme animation
    private float elapsedTime = 0;
    // full text that is currently being spoken
    private string currentText;
    // list of visemes that are currently playing
    private List<Viseme> visemeList;
    // name of currently playing viseme animation
    private string currentVisemeName;
    // length of currently playing viseme animation    
    private float currentVisemeLength = 0;
    // index of currently playing viseme in the list to be played
    private int currentVisemeInList = 0;
    // index of most recent word in the currently spoken text
    private int charIndex = 0;
    // amount of visemes in current set to play
    private int visemeAmount = 0;

    // Unity interface field for visemeTimings script.
    [SerializeField] private VisemeTimings visemeTimings;
    
    // Singleton Instance
    private static SpeechAnimationManager amInstance;

    /// <summary>
    /// Constructs an instance of ApplicationManager if it doesn't exist and
    /// returns the instance if it already exists.
    /// </summary>
    public static SpeechAnimationManager instance
    {
        get
        {
            if (amInstance == null)
            {
                amInstance = GameObject.FindObjectOfType<SpeechAnimationManager>();
                DontDestroyOnLoad(amInstance.gameObject);
            }
            return amInstance;
        }
    }

    /// <summary>
    /// Updates the elapsed time since the last rendered frame. Starts the next
    /// viseme animation if the elapsed time exceeds the duration of the
    /// currently playing viseme animation.
    /// </summary>
    /// <param name="lastFrameDuration">
    /// The elapsed time since the last rendered frame.
    /// </param>
    public void frameUpdate(float lastFrameDuration) {
        // update elapsed time since last frame
        elapsedTime += lastFrameDuration;
        
        // return if no visemes to animate
        if (!isSpeaking) {
            return;
        }

        // if last viseme animation finished
        if (elapsedTime >= currentVisemeLength) {
            // play next viseme animation
            playNextViseme();
            // reset elapsed time
            elapsedTime = 0;
        }
    }

    /// <summary>
    /// Sets the visemes to be played to be the specified list of visemes. The
    /// first viseme from the list is played instantly, further visemes from the
    /// list are played after the currently playing viseme has finished untill
    /// all visemes from the list have been played.
    ///
    /// The frameUpdate function is used to determine when the next animation
    /// should be played.
    /// </summary>
    /// <param name="visList">
    /// Set of visemes to be played.
    /// </param>
    public void playVisemeList(List<Viseme> visList) {
        if (null == visList) {
            return;
        }
        // save list of visemes to play
        visemeList = visList;
        visemeAmount = visemeList.Count;
        if (0 == visemeAmount) {
            return;
        }
        // set current viseme playing
        currentVisemeInList = 0;
        // play first viseme
        playNextViseme();
        isSpeaking = true;
    }

    /// <summary>
    /// Starts animating the next viseme in the current set of visemes.
    ///
    /// Empties the set of visemes and resets the currently playing viseme when
    /// all visemes in the set have been animated.
    /// </summary>
    private void playNextViseme() {
        // if all visemes in the set have been animated
        if (currentVisemeInList >= visemeAmount) {
            stopSpeechAnimation(charIndex);
            return;
        }
        Viseme current = visemeList[currentVisemeInList];
        currentVisemeName = current.getVisemeCode().getName();
        currentVisemeLength = (float) current.getDuration();
        
        // play the found viseme animation
        if (animator != null) {
            animator.CrossFade(currentVisemeName, 0, visemeLayer);
        }
        // increment currently playing viseme
        currentVisemeInList++;
    }
    
    public VisemeTimings getVisemeTimingCalculator()
    {
        return visemeTimings;
    }

    public void setText(string text) {
        // for local access to spoken text.
        currentText = text;
    }

    /// <summary>
    /// Indicates that the next word is being spoken. Specified index is the
    /// index of the first character of that word in the full text string.
    /// 
    /// To be called from the speech animation onboundary event.
    /// </summary>
    /// <param name="charIndex"></param>
    public void onBoundary(int charIndex) {
        this.charIndex = charIndex;
        // TODO start animating the word
				
        // currentIndex specifies the start of the next word in the whole text
				
        // find set of viseme numbers for that word
				
        // call playVisemeList() with the set of visemes for a word
    }

    /// <summary>
    /// Starts animation for speech synthesis.
    ///
    /// To be called as callback from the speech synthesis start event.
    /// </summary>
    /// <param name="charIndex"></param>
    public void startSpeechAnimation(int charIndex) {
        this.charIndex = charIndex;
        isSpeaking = true;
    }

    /// <summary>
    /// Stops all speech animation and clear the current sentence.
    ///
    /// To be called as callback from the speech synthesis stop event.
    /// </summary>
    /// <param name="charIndex"></param>
    public void stopSpeechAnimation(int charIndex) {
        this.charIndex = charIndex;
        // reset the currently playing viseme set
        isSpeaking = false;
        visemeList = null;
        currentVisemeInList = 0;
        // play the silence viseme animation
        if (animator != null) {
            animator.CrossFade("Silence", 0, visemeLayer);
        }
    }

    /// <summary>
    /// Pauses animation for the currently active speech synthesis sentence.
    ///
    /// To be called as callback from the speech synthesis pause event.
    ///
    /// This may be inaccurate as the speech synthesis uses word separation for
    /// pausing, and is quite delayed compared to the animation stopping.
    /// </summary>
    /// <param name="charIndex"></param>
    public void pauseSpeechAnimation(int charIndex) {
        // charIndex will be reset to 0 in event, unsure if text is shortened?
        // this index update might not be sensible without a currentText update
        // but this does not affect results due to simplicity of isSpeaking
        this.charIndex = charIndex;
        // stop speech animation
        isSpeaking = false;
    }

    /// <summary>
    /// Resumes animation for the currently active speech synthesis sentence.
    ///
    /// To be called as callback from the speech synthesis resume event.
    /// </summary>
    /// <param name="charIndex"></param>
    public void resumeSpeechAnimation(int charIndex) {
        // charIndex will be reset to 0 in event, unsure if text is shortened?
        // this index update might not be sensible without a currentText update
        // but this does not affect results due to simplicity of isSpeaking
        this.charIndex = charIndex; 
        // resume speech animation
        isSpeaking = true;
    }

    /// <summary>
    /// Pauses speech synthesis and animation for the current sentence.
    /// 
    /// To be called through TalkingCoachAPI.
    ///
    /// This may not work with every language as it relies on onboundary events
    /// which browsers do not generate for some languages.
    /// </summary>
    public void pauseSpeech() {
        // update remaining text using current position
        currentText = currentText.Substring(charIndex);
        // stop speech synthesis
        TextManager.tmInstance.stopSpeech();
        // stop speech animation
        isSpeaking = false;
    }

    /// <summary>
    /// Resumes speech synthesis and animation for the current sentence.
    ///
    /// To be called through TalkingCoachAPI.
    /// 
    /// To be called after a call to pauseSpeech.  
    /// </summary>
    public void resumeSpeech() {
        // do nothing if speaking (not paused)
        if (isSpeaking) {
            return;
        }
        // restart speech synthesis and animation using remaining text
        TextManager.tmInstance.startSpeech(currentText);
    }

    /// <summary>
    /// Sets the index of the layer in the Animator component that contains the
    /// viseme animations.
    ///
    /// Default speech animation layer index is 1.
    /// </summary>
    /// <param name="layer">index of layer containing viseme animations</param>
    public void setVisemeLayer(int layer) {
        visemeLayer = layer;
    }

    /// <summary>
    /// Loads the coach GameObject for the SpeechAnimation Class.
    /// 
    /// The coach object is assumed to have an Animator component.
    /// This Animator should contain a Speech Animation Layer, which should be
    /// set properly using setVisemeLayer, and should contain the required
    /// viseme Animations.
    /// </summary>
    /// <param name="coach">Current coach GameObject used</param>
    public void loadCoach(GameObject coach) {
        // set the local coach object reference
        newCoach = coach;
        
        // set the local reference for the Animator component attached to coach.
        animator = newCoach.GetComponent<Animator>();
    }

    /// <summary>
    /// Returns the elapsed time since last frame.
    /// </summary>
    /// <returns></returns>
    public float getElapsedTime() {
        return elapsedTime;
    }

    /// <summary>
    /// Returns whether animation is active.
    /// </summary>
    /// <returns></returns>
    public bool getIsSpeaking() {
        return isSpeaking;
    }

    /// <summary>
    /// Returns the current charIndex.
    /// </summary>
    /// <returns></returns>
    public int getCharIndex() {
        return charIndex;
    }

    /// <summary>
    /// Returns the current text.
    /// </summary>
    /// <returns></returns>
    public string getCurrentText() {
        return currentText;
    }
}