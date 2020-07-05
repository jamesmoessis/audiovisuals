using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMusic : MonoBehaviour {
    public Audio source;
    private Vector3 audioValues;
    private UnityEngine.VFX.VisualEffect visualEffect;
    private Gradient startGradient;
    public Gradient highsGradient;


    void Start() {
        visualEffect = this.GetComponent<UnityEngine.VFX.VisualEffect>();
        this.startGradient = visualEffect.GetGradient("HeightFieldColorMap");
        audioValues = new Vector3(0, 0, 0);
        Debug.Log(startGradient.colorKeys);
    }

    void Update() {
        audioValues.x = source._lowerAmplitude;
        audioValues.y = source._amplitude;
        audioValues.z = source._higherAmplitude;
        /*if (audioValues.z > 0.8) {
            visualEffect.SetGradient("HeightFieldColorMap", highsGradient);
        } else {
            visualEffect.SetGradient("HeightFieldColorMap", startGradient);
        }*/
        visualEffect.SetVector3("AudioLevels", audioValues);
    }
}
