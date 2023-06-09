using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class OpenXR_NewController : MonoBehaviour {

    public XRController controller;  // Reference to the XRController component

    public bool isTriggerPressed = false;  // Boolean variable to track trigger press

    private void Update() {
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed) && triggerPressed) {
            // Trigger button is pressed
            isTriggerPressed = true;
        } else {
            // Trigger button is released
            isTriggerPressed = false;
        }
    }
}