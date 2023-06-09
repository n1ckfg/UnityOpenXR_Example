using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRController))]
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

    private XRController controller;

    private float touchPadLimit = 0.6f; // 0.7f;

    private void Awake() {
        controller = GetComponent<XRController>();
    }

    private void Update() {
        resetButtons();
        checkTriggerVal();
        checkPadDir();

        // https://docs.unity3d.com/ScriptReference/XR.CommonUsages.html
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool tb_pressed)) {
            if (tb_pressed) {
                triggerPressed = true;
                triggerDown = true;
                startPos = transform.position;
            } else if (triggerPressed) {
                triggerPressed = false;
                triggerUp = true;
                endPos = transform.position;
            }
        }

        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool pad_pressed)) {
            if (pad_pressed) {
                padPressed = true;
                padDown = true;
            } else if (padPressed) {
                padPressed = false;
                padUp = true;
            }
        }

        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool gb_pressed)) {
            if (gb_pressed) {
                gripped = true;
                gripDown = true;
            } else if (gripped) {
                gripped = false;
                gripUp = true;
            }
        }

        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.menuButton, out bool mb_pressed)) {
            if (mb_pressed) {
                menuPressed = true;
                menuDown = true;
            } else if (menuPressed) {
                menuPressed = false;
                menuUp = true;
            }
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
        //triggerVal = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;
    }

    private void checkPadDir() {
        //touchpad = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);

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

    public void vibrateController(float val) {
        int ms = (int)val * 1000;
        //device.TriggerHapticPulse((ushort)ms, Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
    }

    public void vibrateController() {
        vibrateController(2f);
    }

}