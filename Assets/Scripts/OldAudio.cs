using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour {
    //const int NUM_BANDS = 8;

    AudioSource _audioSource;
    private float[] _samples = new float[512];
    private float[] _freqBand = new float[8];
    public static float[] _bandBuffer = new float[8];
    float[] _bufferDecrease = new float[8];


    float[] _freqBandHighest = new float[8];
    public static float[] _audioBand = new float[8];
    public static float[] _audioBandBuffer = new float[8];


    // Start is called before the first frame update
    void Start() {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
    }

    void GetSpectrumAudioSource() {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }

    void CreateAudioBands() {
        for (int i = 0; i < 8; i++) {
            if (_freqBand[i] > _freqBandHighest[i]) {
                _freqBandHighest[i] = _freqBand[i];
            }
            if (_freqBandHighest[i] != 0) {
                _audioBand[i] = (_freqBand[i] / _freqBandHighest[i]);
                _audioBandBuffer[i] = (_bandBuffer[i] / _freqBandHighest[i]);
            }

        }
    }

    void BandBuffer() {
        /*
         * if normal (averaged) sample value from _freqBand is more than in the buffer,
         * set the bandbuffer equal trhe freqband. (and we will always use bandbuffer)
         * if the freqBand is less that the bandbuffer, then decrease the bandBuffer, 
         * and decrease faster each consecutive time (1.2 times faster).
         * 
         * So basically it is stopping large decreases in a band from making it appear jerky. 
         * It's not perfect but certainly makes it look better. Increase the value on line 46 
         * to make the bars fall quicker.
         */
        for (int g = 0; g < 8; ++g) {
            if (_freqBand[g] > _bandBuffer[g]) {
                _bandBuffer[g] = _freqBand[g];
                _bufferDecrease[g] = 0.005f;
            }
            if (_freqBand[g] < _bandBuffer[g]) {
                _bandBuffer[g] -= _bufferDecrease[g];
                _bufferDecrease[g] *= 1.2f;
            }
        }
    }

    void MakeFrequencyBands() {
        int count = 0;

        for (int i = 0; i < 8; i++) {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            if (i == 7) {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++) {
                average += _samples[count] * (count + 1);
                //average += _samples[count];
                count++;
            }
            average /= count;
            _freqBand[i] = average * 10;
        }


        /*
         * 22050 / 512 = 43HZ per sample
         * 
         * 0 - 2 = 0 - 86Hz
         * 1 - 4 = 172 Hz - 87 - 258
         * 2 - 8 = 344Hz    250-602
         * 3 - 16 = 688Hz - 603-1290 
         * 4 - 32 = 1376Hz -1291-2666
         * 5         -     2667 - 5418
         * 6               5419 - 10922
         * 7               10923-221930
         * 
         */
    }

}
