using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.reddit.com/r/Unity3D/comments/a6rmbh/vr_question_steamvr_2_scale_object_with_both_hands/

public class OpenXR_WorldScale : MonoBehaviour {

    public OpenXR_NewController cltMain;
    public OpenXR_NewController cltAlt;
    public Transform target;

    private bool armed = false;
    private enum SevenMode { BOTH, MAIN, ALT, NONE };
    private SevenMode sevenMode = SevenMode.NONE;
    private Vector3 initialHandPosition1; // first controller
    private Vector3 initialHandPosition2; // second controller
    private Quaternion initialObjectRotation; // target rotation
    private Vector3 initialObjectScale; // target scale
    private Vector3 initialObjectDirection; // direction of target to midpoint of both controllers
    private Transform origParent = null;

    private void Start() {
        origParent = target.parent;
    }

    private void Update() {
        if (cltMain.gripDown || cltAlt.gripDown) {
            target.SetParent(origParent);
            armed = true;
        }

        if (cltMain.gripped && cltAlt.gripped) {
            sevenMode = SevenMode.BOTH;
        } else if (cltMain.gripped && !cltAlt.gripped) {
            sevenMode = SevenMode.MAIN;
        } else if (!cltMain.gripped && cltAlt.gripped) {
            sevenMode = SevenMode.ALT;
        } else if (!cltMain.gripped && !cltAlt.gripped) {
            sevenMode = SevenMode.NONE;
            target.SetParent(origParent);
            armed = false;
            return;
        }

        if (armed) {
            switch (sevenMode) {
                case SevenMode.BOTH:
                    attachTargetBoth();
                    break;
                case SevenMode.MAIN:
                    attachTargetOne(ref cltMain);
                    break;
                case SevenMode.ALT:
                    attachTargetOne(ref cltAlt);
                    break;
            }
            armed = false;
        }

        switch (sevenMode) {
            case SevenMode.BOTH:
                updateTargetBoth();
                break;
        }

    }

    private void attachTargetBoth() {
        initialHandPosition1 = cltMain.transform.position;
        initialHandPosition2 = cltAlt.transform.position;
        initialObjectRotation = target.transform.rotation;
        initialObjectScale = target.transform.localScale;
        initialObjectDirection = target.transform.position - (initialHandPosition1 + initialHandPosition2) * 0.5f;
    }

    private void updateTargetBoth() {
        Vector3 currentHandPosition1 = cltMain.transform.position; // current first hand position
        Vector3 currentHandPosition2 = cltAlt.transform.position; // current second hand position

        Vector3 handDir1 = (initialHandPosition1 - initialHandPosition2).normalized; // direction vector of initial first and second hand position
        Vector3 handDir2 = (currentHandPosition1 - currentHandPosition2).normalized; // direction vector of current first and second hand position 

        Quaternion handRot = Quaternion.FromToRotation(handDir1, handDir2); // calculate rotation based on those two direction vectors

        float currentGrabDistance = Vector3.Distance(currentHandPosition1, currentHandPosition2);
        float initialGrabDistance = Vector3.Distance(initialHandPosition1, initialHandPosition2);
        float p = (currentGrabDistance / initialGrabDistance); // percentage based on the distance of the initial positions and the new positions

        Vector3 newScale = new Vector3(p * initialObjectScale.x, p * initialObjectScale.y, p * initialObjectScale.z); // calculate new object scale with p

        target.transform.rotation = handRot * initialObjectRotation; // add rotation
        target.transform.localScale = newScale; // set new scale

        // set the position of the object to the center of both hands based on the original object direction relative to the new scale and rotation
        target.transform.position = (0.5f * (currentHandPosition1 + currentHandPosition2)) + (handRot * (initialObjectDirection * p));
    }

    private void attachTargetOne(ref OpenXR_NewController ctl) {
        target.SetParent(ctl.transform);
    }

}
