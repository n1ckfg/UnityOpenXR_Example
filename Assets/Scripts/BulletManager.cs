using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public OpenXR_NewController[] ctls;
    public Bullet prefab;
    public float speed = 0.1f;

    void Update() {
        foreach (OpenXR_NewController ctl in ctls) { 
            if (ctl.triggerDown) {
                Bullet bullet = GameObject.Instantiate(prefab, ctl.transform.position, ctl.transform.rotation).GetComponent<Bullet>();
                bullet.speed = speed;
                Debug.Log("fired");
            }
        }
    }
}
