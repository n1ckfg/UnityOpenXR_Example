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

    [HideInInspector] public Vector3 startPos = Vector3.zero;
    [HideInInspector] public Vector3 endPos = Vector3.zero;
    [HideInInspector] public float triggerVal;

    private XRController controller;

    private void Awake()
    {
        controller = GetComponent<XRController>();
    }

    private void Update() {
        resetButtons();
        //checkTriggerVal();
        //checkPadDir();

        // https://docs.unity3d.com/ScriptReference/XR.CommonUsages.html
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool tb_pressed) && tb_pressed) {
            triggerPressed = true;
        } else {
            triggerPressed = false;
        }

        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool gb_pressed) && gb_pressed) {
            gripped = true;
        } else {
            gripped = false;
        }

        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.menuButton, out bool mb_pressed) && mb_pressed) {
            menuPressed = true;
        } else {
            menuPressed = false;
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

}