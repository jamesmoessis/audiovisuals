using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameParticleControl : MonoBehaviour {
    private float _radius;
    private UnityEngine.VFX.VisualEffect _visualEffect;
    public int band;
    public float levelModifier = 1;

    void Start() {
        _radius = 1;
        _visualEffect = this.GetComponent<UnityEngine.VFX.VisualEffect>();
        _visualEffect.SetFloat("audioLevel", _radius);
    }

    void Update() {
        _radius = Audio._audioBandBuffer[this.band];
        _visualEffect.SetFloat("audioLevel", this.levelModifier*_radius);
    }
}
