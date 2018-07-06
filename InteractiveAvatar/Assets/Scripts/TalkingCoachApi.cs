using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/// <inheritdoc />
/// <summary>
/// An API to let the webpage's JavaScript control the coach.
///
/// All methods in this class are recognized as unused. However, this is not true.
/// The methods are called with a SendMessage call from javascript
/// This has the method name as string in a parameter.
/// Therefore it is not explicitly "used" according to the program, but in reality it is.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class TalkingCoachApi : MonoBehaviour {
    /// <summary>
    /// Logs all available voices.
    /// </summary>
    public void GetVoices() {
        TextManager.GetVoices();
    }

    /// <summary>
    /// Sets the current voice of the coach.
    /// </summary>
    /// <param name="voice"></param>
    public void SetVoice(string voice) {
        TextManager.Instance.SetVoice(voice);
    }

    /// <summary>
    /// Lets the coach speak the given text.
    /// </summary>
    /// <param name="text"></param>
    public void ConvertToSpeech(string text) {
		TextManager.Instance.StartSpeech(text);
    }

	public void SetSpeed(float speed) {
		TextManager.Instance.SetSpeed (speed / 50);
	}

    /// <summary>
    /// Set the language of the talking coach.
    /// </summary>
    /// <param name="language"></param>
    public void SetLanguage(string language) {
        TextManager.Instance.SetLanguage(language);
    }

    /// <summary>
    /// Set the hostname for espeak API calls.
    /// Set the hostname for espeak phoneme server API calls.
    /// </summary>
    /// <param name="hName">Hostname with protocal prefix and port suffix.</param>
    public void setPhonemeServerHost(string hName) {
        TextManager.Instance.setPhonemeServerHost(hName);
    }

    /// <summary>
    /// Halts the speech of the coach.
    /// </summary>
    public void StopSpeech() {
        TextManager.StopSpeech();
    }

	/// <summary>
	/// Pauses the speech, relies on onboundary events for accuracy.
	/// Use window.speechSynthesis.pause() if onboundary not available.
	/// </summary>
	public void pauseSpeech() {
		SpeechAnimationManager.instance.pauseSpeech();
	}

	/// <summary>
	/// Resumes the speech, relies on pauseSpeech, no effect if still speaking.
	/// Use window.speechSynthesis.resume() if onboundary not available.
	/// </summary>
	public void resumeSpeech() {
		SpeechAnimationManager.instance.resumeSpeech();
	}

    public void ResetPosition() {
        ApplicationManager.Instance.ResetPosition();
    }  
    
    /// <summary>
    /// Changes the background by cycling through the available backgrounds.
    /// </summary>
    public void ChangeBackground() {
        ApplicationManager.Instance.ChangeBackground();
    }

    /// <summary>
    /// Changes the coach by cycling through the available coaches.
    /// </summary>
    public void ChangeCoach() {
        ApplicationManager.Instance.ChangeCoach();
    }

    /// <summary>
    /// Zooms into the direction of the coach by the given amount.
    /// </summary>
    /// <param name="zoom"></param>
    public void Zoom(int zoom) {
        ApplicationManager.Instance.ZoomAvatarCamera(zoom);
    }

    /// <summary>
    /// Moves the coach a horizontal distance.
    /// </summary>
    /// <param name="horizontal"></param>
    public void MoveAvatarHorizontal(int horizontal) {
        ApplicationManager.Instance.MoveCoach(horizontal, 0);
    }

    /// <summary>
    /// Moves the coach a vertical distance.
    /// </summary>
    /// <param name="vertical"></param>
    public void MoveAvatarVertical(int vertical) {
        ApplicationManager.Instance.MoveCoach(0, vertical);
    }

    /// <summary>
    /// Call for starting typing text input, so webgl stops capturing all keyboard input.
    /// </summary>
    public void StartTypingTextInput() {
        ApplicationManager.StartTypingTextInput();
    }

    /// <summary>
    /// Call for ending typing text input, so webgl starts capturing all keyboard input to control the avatar.
    /// </summary>
    public void StopTypingTextInput() {
        ApplicationManager.StopTypingTextInput();
    }

    /// <summary>
    /// Let the avatar walk out of the screen.
    /// </summary>
    public void WalkAway() {
        ApplicationManager.Instance.WalkAway();
    }

    /// <summary>
    /// Let the avatar walk back into the screen.
    /// </summary>
    public void WalkBack() {
        ApplicationManager.Instance.WalkBack();
    }

    public void Listening() {
        AnimatorManager.Instance.SetBoolAnimator("listening", true);
    }

    public void NotListening() {
        AnimatorManager.Instance.SetBoolAnimator("listening", false);
    }
}