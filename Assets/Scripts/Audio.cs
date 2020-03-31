using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Audio : MonoBehaviour {

    AudioSource _audioSource;
    public const int NUM_SAMPLES = 512;
    private readonly int _outputLength = (int) Mathf.Ceil(Mathf.Log(NUM_SAMPLES + 1, 2));

    private float[] _samples = new float[NUM_SAMPLES];
    private float[] _freqBand;
    private float[] _bandBuffer;
    private float[] _bufferDecrease;
    private float[] _freqBandHighest;
    
    // From one to zero, the audio level in each band.
    public static float[] _audioBand = new float[0];
    public static float[] _audioBandBuffer = new float[0];


    // Start is called before the first frame update
    void Start() {
        this._audioSource = GetComponent<AudioSource>();
        this._freqBand =            new float[_outputLength];
        this._bandBuffer =          new float[_outputLength];
        this._bufferDecrease =      new float[_outputLength];
        this._freqBandHighest =     new float[_outputLength];
        _audioBand =                new float[_outputLength];
        _audioBandBuffer =          new float[_outputLength];
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
        for (int i = 0; i < _outputLength-1; i++) {
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
        for (int g = 0; g < this._outputLength-1; g++) {
            if (_freqBand[g] > _bandBuffer[g] || _bandBuffer[g] < 0) {
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
        /*
         * Get the sample data (currently in 512 bands)
         * and convert into logarithmic average. 
         */
        int outputIndex = 0;
        List<float> copy = new List<float>(_samples);
        int length = 1;

        while (outputIndex < this._outputLength) {
            float average = 0;
            for (int i = 0; i < length; i++) {
                if (copy.Count > 0) {
                    average += copy[0];
                    copy.RemoveAt(0);
                }
            }
            average = average / length;
            _freqBand[outputIndex] = average * 10;
            outputIndex++;
            length = length * 2;
        }
    }
}
