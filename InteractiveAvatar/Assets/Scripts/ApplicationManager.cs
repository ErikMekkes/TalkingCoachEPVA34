using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
    private CoachType _coachType;

    private IEnumerator _currentAnimatorWaiter;

    public ArrayList AnimSequence;

    /// <summary>
    /// Returns the signleton instance of ApplicationManager,
    /// or instantiates it if it is not instantiated.
    /// </summary>
    public static ApplicationManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<ApplicationManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    public GameObject Get_NewCoach() {
        return _newCoach;
    }

    /// <summary>
    /// Gets run the first frame before any update methods.
    /// </summary>
    void Start() {
        _cams = Camera.allCameras;
        foreach (Camera cam in _cams) {
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
    void Awake() {
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
        Vector3 changeZoom = new Vector3(0, 0, zoomValue);
        AvatarCamera.transform.transform.position += changeZoom;
    }

    /// <summary>
    /// Moves the coach around the scene.
    /// </summary>
    /// <param name="moveHorizontal"></param>
    /// <param name="moveVertical"></param>
    public void MoveCoach(int moveHorizontal, int moveVertical) {
        Vector3 changePosition = new Vector3(moveHorizontal, moveVertical, 0);
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
        Vector3 oldCoachPosition = _newCoach.transform.position;
        Quaternion oldCoachRotation = _newCoach.transform.rotation;
        _coachNumber = (_coachNumber + 1) % CoachPrefabs.Count;
        Destroy(_newCoach);
        load_coach();
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
	IEnumerator AnimatorWaiter(ArrayList anims) {
	    if (AnimSequence != null) {
	        foreach (Pair<String, float> pair in anims) {
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
	}
    
    /// <summary>
    /// Called when the 'Speak' button is called in the interface.
    /// Initiates the 'talk' animation.
    /// </summary>
    public void PlayAnimation() {
        _animatorManager.SetBoolAnimator("talk", true);
        _animatorManager.CrossfadeAnimator("talking", 0.03f, -1);
        SequenceAnimations(AnimSequence);
    }

    /// <summary>
    /// Called at the end of a speaking cycle.
    /// Initiates the 'idle' animation.
    /// </summary>
    public void StopAnimation() {
        _animatorManager.SetBoolAnimator("talk", false);
        _animatorManager.CrossfadeAnimator("idle", 0.03f, -1);
        AnimSequence = new ArrayList();
        if (_currentAnimatorWaiter != null) {
            StopCoroutine(_currentAnimatorWaiter);
        }
    }

    /// <summary>
    /// Waits a given amount of time and then calls the 'idle' animation.
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    IEnumerator WaitForAudioToFinish(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        //animation.PlayQueued(idle);
        _animatorManager.CrossfadeAnimator("idle", 0.03f, -1);
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

    IEnumerator ExecuteAnimationWithLength(String animation, float length) {
        _animatorManager.CrossfadeAnimator(animation, 0.03f, -1);
        _animatorManager.SetBoolAnimator("loop", true);
        yield return new WaitForSeconds(length);
        _animatorManager.SetBoolAnimator("loop", false);
    }

    /// <summary>
    /// Called on each frame.
    /// </summary>
    void Update() {
        _timeOutTimer += Time.deltaTime;
        // If screen is tapped, reset timer
        if (Input.anyKeyDown || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0
                || _animatorManager.GetBoolAnimator("talk")) {
            _timeOutTimer = 0.0f;
            // If the user is active but the state is still in timeout, walk back in screen.
            if (_animatorManager.GetBoolAnimator("timeout")) {
                _animatorManager.SetBoolAnimator("timeout", false);
                _animatorManager.PlayAnimator("IntoScreen");
            }
            // Allow for moving the avatar with controls instead of only with animations.
            if (!_animatorManager.GetApplyRootMotionAnimator()
                    && _animatorManager.GetCurrentAnimatorStateInfoAnimator(0).IsName("idle")) {
                _animatorManager.SetApplyRootMotionAnimator(true);
            }
        }

        // If timer reaches zero, start walking out of the screen.
        if (_timeOutTimer > TimeOut) {
            _animatorManager.SetApplyRootMotionAnimator(false);
            _animatorManager.PlayAnimator("OutOfScreen");
            _animatorManager.SetBoolAnimator("timeout", true);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            _animatorManager.CrossfadeAnimator("talking", 0.1f, -1);
        }

        if (Input.GetKeyDown("1")) {
            // This animation is very long so it looks like it is stuck in this state,
            // but it is not.
            _animatorManager.PlayOnce("sitting_talking", 0.1f, -1);
        }

        if (Input.GetKeyDown("2")) {
            _animatorManager.PlayOnce("Walk", 0.1f, -1);
        }

        if (Input.GetKeyDown("3")) {
            _animatorManager.PlayOnce("Run", 0.1f, -1);
        }
        
        if (Input.GetKeyDown("4")) {
            _animatorManager.PlayOnce("Afraid", 0.1f, -1);
        }
        
        if (Input.GetKeyDown("5")) {
            _animatorManager.PlayOnce("Happy", 0.1f, -1);
        }
        
        if (Input.GetKeyDown("6")) {
            _animatorManager.PlayOnce("Sad", 0.1f, -1);
        }
        
        if (Input.GetKeyDown("7")) {
            _animatorManager.PlayOnce("Surprise", 0.1f, -1);
        }
        
        if (Input.GetKeyDown("8")) {
            _animatorManager.PlayOnce("Angry", 0.1f, -1);
        }
        
        if (Input.GetKeyDown("9")) {
            _animatorManager.PlayOnce("Cubeintro", 0f, -1);
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            ChangeCoach();
        }
        
        if (Input.GetKeyDown(KeyCode.B)) {
            ChangeBackground();
        }

        // Controlling the avatar
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * 100.0f;

        // Run if holding left shift, else walk
        if (Input.GetKey(KeyCode.LeftShift) && z > 0f) {
            _animatorManager.SetBoolAnimator("run", true);
            _animatorManager.SetBoolAnimator("walk", false);
            z *= 3f;
        }
        else {
            _animatorManager.SetBoolAnimator("run", false);
            _animatorManager.SetBoolAnimator("walk", z > 0f);
        }
        
        _newCoach.transform.Rotate(0f, x, 0f);
        _newCoach.transform.Translate(0f, 0f, z);
        
        
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
            _animatorManager.CrossfadeAnimator("Walk", 0.05f, -1);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W)) {
            _animatorManager.CrossfadeAnimator("idle", 0.2f, -1);
        }
        // Press R to reset the position and rotation of the avatar.
        if (Input.GetKeyDown(KeyCode.R)) {
            ResetPosition();
        }
    }

    /// <summary>
    /// Disables all keyboard capturing by WebGL, so you can type text in a text field.
    /// This only works in a WebGL environment.
    /// </summary>
    public void StartTypingTextInput() {
    #if !UNITY_EDITOR && UNITY_WEBGL
        WebGLInput.captureAllKeyboardInput = false;
    #endif
    }

    /// <summary>
    /// Enables keyboard capturing by WebGL, so you can use keypresses to trigger actions.
    /// This only works in a WebGL environment.
    /// </summary>
    public void StopTypingTextInput() {
    #if !UNITY_EDITOR && UNITY_WEBGL
        WebGLInput.captureAllKeyboardInput = true;
    #endif
    }
}