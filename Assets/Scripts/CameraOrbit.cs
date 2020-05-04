using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour {
    
    public GameObject Turret;//to get the position in worldspace to which this gameObject will rotate around.

    [Header("The axis by which it will rotate around")]
    public Vector3 axis;//by which axis it will rotate. x,y or z.

    [Header("Angle covered per update")]
    public float angle; //or the speed of rotation.


    void Update() {
        //Gets the position of your 'Turret' and rotates this gameObject around it by the 'axis' provided at speed 'angle' in degrees per update 
        transform.RotateAround(Turret.transform.position, axis, angle);
        transform.position = new Vector3(transform.position.x, 2 + Mathf.Sin(0.8f*Time.time), transform.position.z);
        angle = Triangle(-0.4f, 0.4f, 50, 0, Time.time);
    }

    float Triangle(float minLevel, float maxLevel, float period, float phase, float t) {
        float pos = Mathf.Repeat(t - phase, period) / period;

        if (pos < .5f) {
            return Mathf.Lerp(minLevel, maxLevel, pos * 2f);
        } else {
            return Mathf.Lerp(maxLevel, minLevel, (pos - .5f) * 2f);
        }
    }

}
