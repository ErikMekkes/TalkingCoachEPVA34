using System;
using UnityEngine;

/// <summary>
/// An API to let the webpage's JavaScript control the coach.
/// </summary>
public class TalkingCoachApi : MonoBehaviour {
    /// <summary>
    /// Logs all available voices.
    /// </summary>
    public void GetVoices() {
        TextManager.Instance.GetVoices();
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
    public void ConvertToSpeech(String text) {
        TextManager.Instance.StartSpeech(text);
    }

    /// <summary>
    /// Set the language of the talking coach.
    /// </summary>
    /// <param name="language"></param>
    public void setLanguage(string language) {
        TextManager.Instance.setLanguage(language);
    }

    /// <summary>
    /// Halts the speech of the coach.
    /// </summary>
    public void StopSpeech() {
        TextManager.Instance.StopSpeech();
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

    public void StartTypingTextInput() {
        ApplicationManager.Instance.StartTypingTextInput();
    }

    public void StopTypingTextInput() {
        ApplicationManager.Instance.StopTypingTextInput();
    }
}