using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

/// <inheritdoc />
/// <summary>
/// Configures the scene and loads the coaches and their animations.
/// </summary>
public class ApplicationManager : MonoBehaviour {
    private static ApplicationManager _instance;

    public List<GameObject> CoachPrefabs;
    public GameObject CoachHolder;
    public GameObject BackgroundHolder;

    public Camera AvatarCamera;

    private GameObject _newCoach;
    private Vector3 _startPosition;

    private Sprite[] _backgroundTexture;
    private SpriteRenderer _backgroundSprite;

    private AnimatorManager _animatorManager;

    private Camera[] _cams;
    public float TimeOut; // Time Out Setting in Seconds
    private float _timeOutTimer;

    private int _coachNumber;
    private int _backgroundNumber;

    private IEnumerator _currentAnimatorWaiter;

    public ArrayList AnimSequence;
    
#if UNITY_WEBGL || UNITY_EDITOR
    /// <summary>
    /// Notifies the webpage that unity is loaded
    /// </summary>
    /// <returns></returns>
    [DllImport("__Internal")]
    private static extern string LoadUnity();
#endif

    /// <summary>
    /// Returns the signleton instance of ApplicationManager,
    /// or instantiates it if it is not instantiated.
    /// </summary>
    public static ApplicationManager Instance {
        get {
            if (_instance != null) return _instance;
            _instance = FindObjectOfType<ApplicationManager>();
            DontDestroyOnLoad(_instance.gameObject);

            return _instance;
        }
    }

    public GameObject Get_NewCoach() {
        return _newCoach;
    }

    /// <summary>
    /// Gets run the first frame before any update methods.
    /// </summary>
    private void Start() {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            LoadUnity();
        }

        _cams = Camera.allCameras;
        foreach (var cam in _cams) {
            if (cam.gameObject.name == "InactiveCamera") {
                cam.enabled = false;
            }
        }
        _animatorManager.SetBoolAnimator("run", false);
        _animatorManager.SetBoolAnimator("talk", false);
    }

    public void Set_AnimatorManager(AnimatorManager animatorManager) {
        _animatorManager = animatorManager;
    }

   /// <summary>
   /// Gets run when the script is being loaded.
   /// </summary>
   private void Awake() {
    #if !UNITY_EDITOR && UNITY_WEBGL
		WebGLInput.captureAllKeyboardInput = true;
	#endif
       on_load();
   }

    /// <summary>
    /// Sets the background and calls the load_coach method.
    /// </summary>
    private void on_load() {
        _animatorManager = AnimatorManager.Instance;
        BackgroundHolder = GameObject.Find("background");
        _backgroundSprite = BackgroundHolder.GetComponent<SpriteRenderer>();
        _backgroundTexture = Resources.LoadAll<Sprite>("Textures");
        _backgroundNumber = 2;
        load_background();
        _coachNumber = 5;
        load_coach();
    }

    /// <summary>
    /// Loads the background.
    /// </summary>
    private void load_background() {
        _backgroundSprite.sprite = _backgroundTexture[_backgroundNumber];
    }

    /// <summary>
    /// Zooms the camera in the direction of the avatar.
    /// </summary>
    /// <param name="zoomValue"></param>
    public void ZoomAvatarCamera(int zoomValue) {
        var changeZoom = new Vector3(0, 0, zoomValue);
        AvatarCamera.transform.transform.position += changeZoom;
    }

    /// <summary>
    /// Moves the coach around the scene.
    /// </summary>
    /// <param name="moveHorizontal"></param>
    /// <param name="moveVertical"></param>
    public void MoveCoach(int moveHorizontal, int moveVertical) {
        var changePosition = new Vector3(moveHorizontal, moveVertical, 0);
        _newCoach.transform.position += changePosition;
    }

    /// <summary>
    /// Loads the coach and sets the Animator Controller.
    /// </summary>
    private void load_coach() {
        _newCoach = Instantiate(CoachPrefabs[_coachNumber]);
        _newCoach.transform.parent = CoachHolder.transform;
        _newCoach.transform.localPosition = new Vector3(0, 0, 0);
        _newCoach.transform.localRotation = Quaternion.identity;
        _newCoach.transform.localScale = new Vector3(1, 1, 1);
        _startPosition = _newCoach.GetComponent<Transform>().position;
    }

    /// <summary>
    /// Cycles to all available backgrounds by incrementing the currect index.
    /// </summary>
    public void ChangeBackground() {
        _backgroundNumber = (_backgroundNumber + 1) % _backgroundTexture.Length;
        load_background();
    }

    /// <summary>
    /// Changes the currect coach by cycling through all available coaches.
    /// </summary>
    public void ChangeCoach() {
        var oldCoachPosition = _newCoach.transform.position;
        var oldCoachRotation = _newCoach.transform.rotation;
        _coachNumber = (_coachNumber + 1) % CoachPrefabs.Count;
        Destroy(_newCoach);
        load_coach();
        _animatorManager.LoadAnimator();
        _newCoach.transform.position = oldCoachPosition;
        _newCoach.transform.rotation = oldCoachRotation;
    }

    /// <summary>
    /// Starts a coroutine to sequence animations.
    /// </summary>
    /// <param name="anims">{animation, length} pairs</param>
	public void SequenceAnimations(ArrayList anims) {
        AnimSequence = anims;
        _currentAnimatorWaiter = AnimatorWaiter(anims);
		StartCoroutine(_currentAnimatorWaiter);
	}

    /// <summary>
    /// Sequences the animations in AnimSequence.
    /// </summary>
    /// <param name="anims">{animation, length} pairs</param>
    /// <returns>yield</returns>
    private IEnumerator AnimatorWaiter(IEnumerable anims) {
        if (AnimSequence == null || _animatorManager.WalkedAway()) yield break;
        foreach (Pair<string, float> pair in anims) {
            if (!_animatorManager.GetBoolAnimator("talk")) {
                yield break;
            }
            _animatorManager.CrossfadeAnimator(pair.GetA(), 0.03f, -1);
            _animatorManager.SetBoolAnimator("loop", true);	            
	           
            //Wait for a set amount of seconds before starting the other animation.
            yield return new WaitForSeconds(pair.GetB());

            _animatorManager.SetBoolAnimator("loop", false);
        }
    }
    
    /// <summary>
    /// Called when the 'Speak' button is called in the interface.
    /// Initiates the 'talk' animation.
    /// </summary>
    public void PlayAnimation() {
        if (_animatorManager.WalkedAway()) return;
        _animatorManager.SetBoolAnimator("talk", true);
        _animatorManager.CrossfadeAnimator("talking", 0.03f, -1);
        SequenceAnimations(AnimSequence);
    }

    /// <summary>
    /// Called at the end of a speaking cycle.
    /// Initiates the 'idle' animation.
    /// </summary>
    public void StopAnimation() {
        if (_animatorManager.WalkedAway()) return;
        _animatorManager.SetBoolAnimator("talk", false);
        _animatorManager.CrossfadeAnimator("idle", 0.03f, -1);
        AnimSequence = new ArrayList();
        if (_currentAnimatorWaiter != null) {
            StopCoroutine(_currentAnimatorWaiter);
        }
    }

    /// <summary>
    /// Set the talk parameter of the animator to a new boolean.
    /// </summary>
    /// <param name="talk"></param>
    public void SetTalk(bool talk) {
        _animatorManager.SetBoolAnimator("talk", talk);
    }

    /// <summary>
    /// Reset the position of the avatar to the original position.
    /// </summary>
    public void ResetPosition() {
        _newCoach.GetComponent<Transform>().position = _startPosition;
        _newCoach.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 0f);
    }

    /// <summary>
    /// Let the avatar walk out of the screen.
    /// </summary>
    public void WalkAway() {
        _animatorManager.SetApplyRootMotionAnimator(false);
        _animatorManager.PlayAnimator("OutOfScreen");
        _animatorManager.SetBoolAnimator("walk_away_toggle", true);
    }

    /// <summary>
    /// Let the avatar walk back into the screen.
    /// </summary>
    public void WalkBack() {
        _animatorManager.SetApplyRootMotionAnimator(false);
        _animatorManager.PlayAnimator("IntoScreen");
        _animatorManager.SetBoolAnimator("walk_away_toggle", false);
    }

    /// <summary>
    /// Executes an animation for a certain duration.
    /// If the animation is shorter, it loops untill the time is over.
    ///
    /// This method may be useful in the future, but is currently unused.
    /// </summary>
    /// <param name="animationName">Name of the animation state</param>
    /// <param name="length">Duration you want the animation to play</param>
    /// <returns></returns>
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private IEnumerator ExecuteAnimationWithLength(string animationName, float length) {
        if (_animatorManager.WalkedAway()) yield break;
        _animatorManager.CrossfadeAnimator(animationName, 0.03f, -1);
        _animatorManager.SetBoolAnimator("loop", true);
        yield return new WaitForSeconds(length);
        _animatorManager.SetBoolAnimator("loop", false);
    }

    /// <summary>
    /// Called on each frame.
    /// </summary>
    private void Update() {

        _timeOutTimer += Time.deltaTime;

        _timeOutTimer = Updater.Update(_animatorManager, _timeOutTimer, TimeOut, _newCoach);
    }

    /// <summary>
    /// Disables all keyboard capturing by WebGL, so you can type text in a text field.
    /// This only works in a WebGL environment.
    /// </summary>
    public static void StartTypingTextInput() {
    #if !UNITY_EDITOR && UNITY_WEBGL
        WebGLInput.captureAllKeyboardInput = false;
    #endif
    }

    /// <summary>
    /// Enables keyboard capturing by WebGL, so you can use keypresses to trigger actions.
    /// This only works in a WebGL environment.
    /// </summary>
    public static void StopTypingTextInput() {
    #if !UNITY_EDITOR && UNITY_WEBGL
        WebGLInput.captureAllKeyboardInput = true;
    #endif
    }
}