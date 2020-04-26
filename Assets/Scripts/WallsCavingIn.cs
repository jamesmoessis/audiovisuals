using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is designed to be placed on a plane.
 * The plane will move according towards it's normal
 * vector when lower amplitudes are detected. 
 */
public class WallsCavingIn : MonoBehaviour {

    private PrimitiveType plane;
    private Vector3 normal;
    private Vector3 initialPosition;
    private float lastAudioValue;
    public Audio source;

    void Start() {
        normal = transform.up;
        initialPosition = transform.position;
        lastAudioValue = source._amplitude;
        Debug.Log(normal);
    }

    void Update() {
        CaveIn();
    }
    
    /*
     * Walls "breathe" due to a slow sine wave, even with no input.
     * Walls move more quickly inwards when a large bass input is detected.
     * However this movement is limited, or "smoothed" by maximumChange variable.
     */
    void CaveIn() {
        //transform.position = (5*normal*Audio._amplitudeBuffer) + initialPosition;
        float maximumChange = 0.04f;
        float smoothValue;
        float currentAmp = source._lowerAmplitude;
        if (Mathf.Abs(currentAmp - lastAudioValue) > maximumChange) {
            if (currentAmp < lastAudioValue) {
                smoothValue = lastAudioValue - maximumChange;
            } else {
                smoothValue = lastAudioValue + maximumChange;
            }
        } else {
            smoothValue = currentAmp;
        }
        transform.position = 1.5f*normal* (float)( smoothValue  + 0.4*Mathf.Sin(0.8f*Time.time)) + initialPosition;
        this.lastAudioValue = smoothValue;
    }

}
