using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityLogoSound : MonoBehaviour {
    public Audio source;
    private Vector3 audioValues;
    private UnityEngine.VFX.VisualEffect visualEffect;

    
    void Start() {
        audioValues = new Vector3(0, 0, 0);
        visualEffect = this.GetComponent<UnityEngine.VFX.VisualEffect>();
    }

    void Update() {
        audioValues.x = source._lowerAmplitude;
        audioValues.y = source._amplitude;
        audioValues.z = source._higherAmplitude;
        visualEffect.SetVector3("AudioLevel", audioValues);
    }
}
