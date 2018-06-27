using System;
using UnityEngine;

/// <summary>
/// Holds methods which are called in the 'update' method in ApplicationManager.
/// </summary>
public static class Updater {

    public static float Update(AnimatorManager animatorManager, float timeOutTimer, float timeOut, GameObject newCoach) {
        // Handles what happens when the mouse hovers above the screen when TimeOut.
        var newTimeOutTimer = Refresh(animatorManager, timeOutTimer);

        // If timer reaches zero, do something as a timeout.
        TimeOutFunctionality(timeOutTimer, timeOut);

        // Handle button input
        HandleInput(animatorManager);

        // Controlling the avatar
        CharacterMovement(animatorManager, newCoach);

        return newTimeOutTimer;
    }

    private static float Refresh(AnimatorManager animatorManager, float timeOutTimer) {
        const float epsilon = 0.0001f;
        if (!Input.anyKeyDown && Math.Abs(Input.GetAxis("Mouse X")) < epsilon && Math.Abs(Input.GetAxis("Mouse Y")) < epsilon &&
            !animatorManager.GetBoolAnimator("talk")) {
            return timeOutTimer;
        }
        // If the user is active but the state is still in timeout, walk back in screen.
        if (animatorManager.GetBoolAnimator("timeout")) {
            animatorManager.SetBoolAnimator("timeout", false);
            // Do something on activation after timeout, may be inserted later
        }
        // Allow for moving the avatar with controls instead of only with animations.
        if (!animatorManager.GetApplyRootMotionAnimator()
            && animatorManager.GetCurrentAnimatorStateInfoAnimator(0).IsName("idle")) {
            animatorManager.SetApplyRootMotionAnimator(true);
        }

        return 0f;

    }

    private static void CharacterMovement(AnimatorManager animatorManager, GameObject newCoach) {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 100.0f;

        // Run if holding left shift, else walk
        if (Input.GetKey(KeyCode.LeftShift) && z > 0f) {
            animatorManager.SetBoolAnimator("run", true);
            animatorManager.SetBoolAnimator("walk", false);
            z *= 3f;
        }
        else {
            animatorManager.SetBoolAnimator("run", false);
            animatorManager.SetBoolAnimator("walk", z > 0f);
        }

        newCoach.transform.Rotate(0f, x, 0f);
        newCoach.transform.Translate(0f, 0f, z);
    }

    private static void TimeOutFunctionality(float timeOutTimer, float timeOut) {
        if (timeOutTimer > timeOut) {
            // Do something after a timeout, may be inserted later
        }
    }

    private static void HandleInput(AnimatorManager animatorManager) {
        if (Input.GetKeyDown(KeyCode.Space)) {
            animatorManager.CrossfadeAnimator("talking", 0.1f, -1);
        }

        if (Input.GetKeyDown("1")) {
            // This animation is very long so it looks like it is stuck in this state,
            // but it is not.
            animatorManager.PlayOnce("sitting_talking", 0.1f, -1);
        }

        if (Input.GetKeyDown("2")) {
            animatorManager.PlayOnce("Walk", 0.1f, -1);
        }

        if (Input.GetKeyDown("3")) {
            animatorManager.PlayOnce("Run", 0.1f, -1);
        }

        if (Input.GetKeyDown("4")) {
            animatorManager.PlayOnce("Afraid", 0.1f, -1);
        }

        if (Input.GetKeyDown("5")) {
            animatorManager.PlayOnce("Happy", 0.1f, -1);
        }

        if (Input.GetKeyDown("6")) {
            animatorManager.PlayOnce("Sad", 0.1f, -1);
        }

        if (Input.GetKeyDown("7")) {
            animatorManager.PlayOnce("Surprise", 0.1f, -1);
        }

        if (Input.GetKeyDown("8")) {
            animatorManager.PlayOnce("Angry", 0.1f, -1);
        }

        if (Input.GetKeyDown("9")) {
            animatorManager.PlayOnce("Cubeintro", 0f, -1);
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            ApplicationManager.Instance.ChangeCoach();
        }

        if (Input.GetKeyDown(KeyCode.B)) {
            ApplicationManager.Instance.ChangeBackground();
        }
        
        if (Input.GetKeyDown(KeyCode.K)) {
            ApplicationManager.Instance.WalkAway();
        }
        
        if (Input.GetKeyDown(KeyCode.L)) {
            ApplicationManager.Instance.WalkBack();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
            animatorManager.CrossfadeAnimator("Walk", 0.05f, -1);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W)) {
            animatorManager.CrossfadeAnimator("idle", 0.2f, -1);
        }

        // Press R to reset the position and rotation of the avatar.
        if (Input.GetKeyDown(KeyCode.R)) {
            ApplicationManager.Instance.ResetPosition();
        }
    }


}