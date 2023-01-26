using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.Experimental.XR.Interaction;

#if ENABLE_AR || ENABLE_VR
using UnityEngine.XR;
#endif

public class OpenXR_NewController : MonoBehaviour
{

    private UnityEngine.SpatialTracking.TrackedPoseDriver ctl;

    private void Awake() {
        ctl = GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>();    
    }

    private void Start() {
        Debug.Log(ctl.deviceType);
    }

    private void Update() {
        
    }

}
