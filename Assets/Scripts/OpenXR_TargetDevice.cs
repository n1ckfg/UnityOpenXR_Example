using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenXR_TargetDevice : MonoBehaviour {

    public enum WhichDevice { VIVE, OCULUS };
    public WhichDevice whichDevice = WhichDevice.VIVE;
    public bool rotationCorrection = true;
    public Transform R_controller_root;
    public Transform L_controller_root;

    private Vector3 viveRotOffset = Vector3.zero;
    private Vector3 oculusRotOffset = new Vector3(45f, 0f, 0f);

    private void Start() {
        switch (whichDevice) {
            case WhichDevice.VIVE:
                if (rotationCorrection) {
                    R_controller_root.Rotate(viveRotOffset);
                    L_controller_root.Rotate(viveRotOffset);
                }
                break;
            case WhichDevice.OCULUS:
                if (rotationCorrection) {
                    R_controller_root.Rotate(oculusRotOffset);
                    L_controller_root.Rotate(oculusRotOffset);
                }
                break;
        }
    }

}
