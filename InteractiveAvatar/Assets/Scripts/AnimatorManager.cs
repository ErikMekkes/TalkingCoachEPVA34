using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/// <inheritdoc />
/// <summary>
/// Handles the interaction with the animator object.
/// All methods are virtual because they need to be mocked, and only virtual methods are mockable.
/// </summary>
[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
public class AnimatorManager : MonoBehaviour {
    private static AnimatorManager _instance;

    private Animator _anim;
    
    /// <summary>
    /// Returns the signleton instance of AnimatorManager,
    /// or instantiates it if it is not instantiated.
    /// </summary>
    public static AnimatorManager Instance {
        get {
            if (_instance != null) return _instance;
            _instance = FindObjectOfType<AnimatorManager>();
            DontDestroyOnLoad(_instance.gameObject);

            return _instance;
        }
    }
    
    /// <summary>
    /// Gets run the first frame before any update methods.
    /// </summary>
    private void Start() {
        LoadAnimator();
        _anim.applyRootMotion = true;
    }
    
    /// <summary>
    /// (Re)loads the animator object.
    /// </summary>
    public void LoadAnimator() {
        _anim = ApplicationManager.Instance.Get_NewCoach().GetComponent<Animator>();
    }

    /// <summary>
    /// Getter for a boolean parameter in the animator.
    /// </summary>
    /// <param name="boolName"></param>
    /// <returns></returns>
    public virtual bool GetBoolAnimator(string boolName) {
        return _anim.GetBool(boolName);
    }

    /// <summary>
    /// Setter for a boolean parameter in the animator.
    /// </summary>
    /// <param name="boolName"></param>
    /// <param name="value"></param>
    public virtual void SetBoolAnimator(string boolName, bool value) {
        _anim.SetBool(boolName, value);
    }

    /// <summary>
    /// Initializes a crossfade animation in the animator.
    /// </summary>
    /// <param name="stateName"></param>
    /// <param name="transitionDuration"></param>
    /// <param name="layer"></param>
    public virtual void CrossfadeAnimator(string stateName, float transitionDuration, int layer) {
        _anim.CrossFade(stateName, transitionDuration, layer);
    }

    /// <summary>
    /// Plays a state in the animator.
    /// </summary>
    /// <param name="stateName"></param>
    public virtual void PlayAnimator(string stateName) {
        _anim.Play(stateName);
    }

    /// <summary>
    /// Getter for the applyRootMotion field in the animator.
    /// </summary>
    /// <returns></returns>
    public virtual bool GetApplyRootMotionAnimator() {
        return _anim.applyRootMotion;
    }

    /// <summary>
    /// Setter for the applyRootMotion field in the animator.
    /// </summary>
    /// <param name="applyRootMotionBool"></param>
    public virtual void SetApplyRootMotionAnimator(bool applyRootMotionBool) {
        _anim.applyRootMotion = applyRootMotionBool;
    }

    /// <summary>
    /// Getter for the CurrentAnimatorStateInfo field in the animator.
    /// </summary>
    /// <param name="layerIndex"></param>
    /// <returns></returns>
    public virtual AnimatorStateInfo GetCurrentAnimatorStateInfoAnimator(int layerIndex) {
        return _anim.GetCurrentAnimatorStateInfo(layerIndex);
    }

    /// <summary>
    /// Plays an animaton once.
    /// </summary>
    /// <param name="stateName">Name of the state</param>
    /// <param name="transitionDuration">How long this animation crossfades with the
    /// previous</param>
    /// <param name="layer">The layer</param>
    public virtual void PlayOnce(string stateName, float transitionDuration, int layer) {
        _anim.SetTrigger("play_once");
        CrossfadeAnimator(stateName + "_once", transitionDuration, layer);
    }

    /// <summary>
    /// Returns a boolean for if the avatar is walked away or not.
    /// </summary>
    /// <returns>True if he is out of the screen, false if he is in the screen.</returns>
    public bool WalkedAway() {
        return _anim.GetBool("walk_away_toggle");
    }
}