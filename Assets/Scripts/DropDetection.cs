using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DropDetection : MonoBehaviour {
    /*
     * The interval of GetSpectrumData() is 0 - 24000 Hz.
     * Bands are equally divided in the array.
     * So if we give it an array of size 1024, each element
     * will represent a linear band of 24000/1024 = 23.4 Hz.
     * Bassy sounds are about up to 250Hz so we will use the bottom
     * 16 frequency bands, so about up to 374.4 Hz.
     * However, we will give the lower sounds more weight than the 
     * sounds nearer to 374.4 Hz, linearly according to y = -x/8 + 2.
     * 
     * There is some undeterministic behaviour as we take measurements
     * each frame in a circular array, and fps is not constant.
     */

    public bool drop;
    protected float[] samples = new float[1024];
    
    private AudioSource audioSource;
    private float bassRating;
    private float[] bassRatings = new float[360]; // from last ~10s assuming 60fps
    private float lastDropTime;
    private int fc; // frame counter
    
    void Start() {
        audioSource = GetComponent<AudioSource>();
        fc = 0;
        lastDropTime = 0;
        for(int i = 0; i < bassRatings.Length; i++) {
            bassRatings[i] = 0;
        }
    } 

    void Update() {
        drop = false;
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
        float sum = 0;
        for(int j = 0; j < 16; j++) {
            sum += samples[j] * ((-j / 8) + 2); 
        }
        bassRating = sum/16;
        bassRatings[fc] = bassRating;

        // If the average of the last 3 values are more than 5x the average bassRating,
        // and there hasn't been a drop in the last 20s, then it's a drop.
        int l = bassRatings.Length;
        float last3avg = (bassRatings[fc] + bassRatings[mod(fc - 1, l)] + bassRatings[mod(fc - 2, l)]) / 3;
        if (last3avg > bassRatings.Average()*5 && Time.time - lastDropTime > 20) { 
            drop = true;
            lastDropTime = Time.time;
            Debug.Log("DROP THE BASS!!!!!!!!!");
        }

        fc = (fc + 1) % l;
    }


    static int mod(int x, int m) {
        // return a modulo b. Needed because % gives remainder in c#, not modulo.
        return (x % m + m) % m;
    }
}
