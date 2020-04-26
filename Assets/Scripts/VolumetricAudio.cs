using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumetricAudio : MonoBehaviour {
    public Audio source;
    public float maxChange;
    public float multiplier;

    private UnityEngine.VFX.VisualEffect visualEffect;
    private float prevValue;
    

    void Start() {
        visualEffect = this.GetComponent<UnityEngine.VFX.VisualEffect>();
        prevValue = 0;
    }

    void Update() {
        float audioValue = source._lowerAmplitude;
        visualEffect.SetFloat("AudioLevel", audioValue*multiplier);
    }
}
