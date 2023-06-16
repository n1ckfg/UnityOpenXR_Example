// https://docs.unity3d.com/ScriptReference/XR.CommonUsages.html

using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRController))]
public class OpenXR_NewController : MonoBehaviour {

    public OpenXR_TargetDevice targetDevice;

    public bool triggerPressed = false;
    public bool padPressed = false;
    public bool gripped = false;
    public bool menuPressed = false;
    public bool menu2Pressed = false;
    public bool triggerDown = false;
    public bool padDown = false;
    public bool gripDown = false;
    public bool menuDown = false;
    public bool menu2Down = false;
    public bool triggerUp = false;
    public bool padUp = false;
    public bool gripUp = false;
    public bool menuUp = false;
    public bool menu2Up = false;

    public bool padDirUp = false;
    public bool padDirDown = false;
    public bool padDirLeft = false;
    public bool padDirRight = false;
    public bool padDirCenter = false;

    public Vector2 touchpad = new Vector2(0f, 0f);

    [HideInInspector] public Vector3 startPos = Vector3.zero;
    [HideInInspector] public Vector3 endPos = Vector3.zero;
    [HideInInspector] public float triggerVal = 0f;

    private XRController controller;

    private float touchPadLimit = 0.6f; // 0.7f;
    private float triggerThreshold = 0.3f;

    private void Awake() {
        controller = GetComponent<XRController>();
    }

    private void Update() {
        resetButtons();
        checkTriggerVal();
        checkPadDir();
        
        switch (targetDevice.whichDevice) {
            case OpenXR_TargetDevice.WhichDevice.VIVE:
                if (controller.inputDevice.TryGetFeatureValue(CommonUsages.menuButton, out bool input_menuPressed)) {
                    if (input_menuPressed & !menuPressed) {
                        menuPressed = true;
                        menuDown = true;
                    } else if (!input_menuPressed && menuPressed) {
                        menuPressed = false;
                        menuUp = true;
                    }
                }

                break;

            case OpenXR_TargetDevice.WhichDevice.OCULUS:
                if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool input_primaryPressed)) {
                    if (input_primaryPressed & !menuPressed) {
                        menuPressed = true;
                        menuDown = true;
                    } else if (!input_primaryPressed && menuPressed) {
                        menuPressed = false;
                        menuUp = true;
                    }
                }

                if (controller.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool input_secondaryPressed)) {
                    if (input_secondaryPressed & !menu2Pressed) {
                        menu2Pressed = true;
                        menu2Down = true;
                    } else if (!input_secondaryPressed && menu2Pressed) {
                        menu2Pressed = false;
                        menu2Up = true;
                    }
                }

                break;
        }

        // Note: Vive has built-in trigger threshold, Oculus does not.
        //if (controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool input_triggerPressed)) {
        bool input_triggerPressedByVal = triggerVal > triggerThreshold;
        if (input_triggerPressedByVal && !triggerPressed) {
            triggerPressed = true;
            triggerDown = true;
            startPos = transform.position;
        } else if (!input_triggerPressedByVal && triggerPressed) {
            triggerPressed = false;
            triggerUp = true;
            endPos = transform.position;
        }
        //}

        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool input_padPressed)) {
            if (input_padPressed & !padPressed) {
                padPressed = true;
                padDown = true;
            } else if (!input_padPressed && padPressed) {
                padPressed = false;
                padUp = true;
            }
        }

        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool input_gripped)) {
            if (input_gripped & !gripped) {
                gripped = true;
                gripDown = true;
            } else if (!input_gripped && gripped) {
                gripped = false;
                gripUp = true;
            }
        }
    }

    private void resetButtons() {
        triggerDown = false;
        padDown = false;
        gripDown = false;
        menuDown = false;
        menu2Down = false;
        triggerUp = false;
        padUp = false;
        gripUp = false;
        menuUp = false;
        menu2Up = false;

        padDirUp = false;
        padDirDown = false;
        padDirLeft = false;
        padDirRight = false;
        padDirCenter = true;
    }

    private void checkTriggerVal() {
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float tb_val)) {
            triggerVal = tb_val;
        }
    }

    private void checkPadDir() {
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 pad_dir_val)) {
            touchpad = pad_dir_val;
        }

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

        // Note: Vive counts off-center dpad movement as a press, Oculus does not.
        /*
        switch (targetDevice.whichDevice) {
            case OpenXR_TargetDevice.WhichDevice.OCULUS:
                if (!padDirCenter & !padPressed) {
                    padPressed = true;
                    padDown = true;
                } else if (padDirCenter && padPressed) {
                    padPressed = false;
                    padUp = true;
                }
                break;
        }
        */
    }

        public void vibrateController(float val) {
        int ms = (int)val * 1000;
        //device.TriggerHapticPulse((ushort)ms, Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
    }

    public void vibrateController() {
        vibrateController(2f);
    }

}