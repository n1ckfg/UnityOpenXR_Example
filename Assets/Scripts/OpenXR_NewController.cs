using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Management;
using UnityEngine.XR.OpenXR.Input;

//[RequireComponent(typeof(OpenXR_TrackedObject))]
public class OpenXR_NewController : MonoBehaviour {

    public bool triggerPressed = false;
    public bool padPressed = false;
    public bool gripped = false;
    public bool menuPressed = false;
    public bool triggerDown = false;
    public bool padDown = false;
    public bool gripDown = false;
    public bool menuDown = false;
    public bool triggerUp = false;
    public bool padUp = false;
    public bool gripUp = false;
    public bool menuUp = false;

    public bool padDirUp = false;
    public bool padDirDown = false;
    public bool padDirLeft = false;
    public bool padDirRight = false;
    public bool padDirCenter = false;

    public Vector2 touchpad = new Vector2(0f, 0f);

    [HideInInspector] public Vector3 startPos = Vector3.zero;
    [HideInInspector] public Vector3 endPos = Vector3.zero;
    [HideInInspector] public float triggerVal;

    private OpenXR_TrackedObject trackedObj;
    private OpenXR_Controller.Device device;

    private float touchPadLimit = 0.6f; // 0.7f;

    private void Awake() {
        trackedObj = GetComponent<OpenXR_TrackedObject>();
    }

    private void FixedUpdate() {
        device = OpenXR_Controller.Input((int)trackedObj.index);
    }

    private void Update() {
        //var device = OpenXR_Controller.Input((int) trackedObj.index);

        resetButtons();
        checkTriggerVal();
        checkPadDir();

        if (device.GetTouchDown(OpenXR_Controller.ButtonMask.Trigger)) {
            triggerPressed = true;
            triggerDown = true;
            startPos = transform.position;
        } else if (device.GetTouchUp(OpenXR_Controller.ButtonMask.Trigger)) {
            triggerPressed = false;
            triggerUp = true;
            endPos = transform.position;
        }

        if (device.GetPressDown(OpenXR_Controller.ButtonMask.Touchpad)) {
            padPressed = true;
            padDown = true;
        } else if (device.GetPressUp(OpenXR_Controller.ButtonMask.Touchpad)) {
            padPressed = false;
            padUp = true;
        }

        if (device.GetPressDown(OpenXR_Controller.ButtonMask.Grip)) {
            gripped = true;
            gripDown = true;
        } else if (device.GetPressUp(OpenXR_Controller.ButtonMask.Grip)) {
            gripped = false;
            gripUp = true;
        }

        if (device.GetPressDown(OpenXR_Controller.ButtonMask.ApplicationMenu)) {
            menuPressed = true;
            menuDown = true;
        } else if (device.GetPressUp(OpenXR_Controller.ButtonMask.ApplicationMenu)) {
            menuPressed = false;
            menuUp = true;
        }
    }

    private void resetButtons() {
        triggerDown = false;
        padDown = false;
        gripDown = false;
        menuDown = false;
        triggerUp = false;
        padUp = false;
        gripUp = false;
        menuUp = false;

        padDirUp = false;
        padDirDown = false;
        padDirLeft = false;
        padDirRight = false;
        padDirCenter = true;
    }

    private void checkTriggerVal() {
        triggerVal = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_OpenXR_Trigger).x;
    }

    private void checkPadDir() {
        touchpad = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);

        if (touchpad.y > touchPadLimit) {
            padDirUp = true;
            padDirDown = false;
            padDirCenter = false;
        } else if (touchpad.y < -touchPadLimit) {
            padDirUp = false;
            padDirDown = true;
            padDirCenter = false;
        }

        if (touchpad.x > touchPadLimit) {
            padDirLeft = true;
            padDirRight = false;
            padDirCenter = false;
        } else if (touchpad.x < -touchPadLimit) {
            padDirLeft = false;
            padDirRight = true;
            padDirCenter = false;
        }
    }

    float defaultVibrationVal = 2f;

    public void vibrateController() {
        int ms = (int)defaultVibrationVal * 1000;
        device.TriggerHapticPulse((ushort)ms, Valve.VR.EVRButtonId.k_EButton_OpenXR_Touchpad);
    }

    public void vibrateController(float val) {
        int ms = (int)val * 1000;
        device.TriggerHapticPulse((ushort)ms, Valve.VR.EVRButtonId.k_EButton_OpenXR_Touchpad);
    }

}