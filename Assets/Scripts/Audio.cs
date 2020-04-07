using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Audio : MonoBehaviour {

    AudioSource _audioSource;
    public const int NUM_SAMPLES = 512;
    private readonly int _outputLength = (int) Mathf.Ceil(Mathf.Log(NUM_SAMPLES + 1, 2));

    public float[] _samples = new float[NUM_SAMPLES];
    private  float[] _freqBand;
    private float[] _bandBuffer;
    private float[] _bufferDecrease;
    private float[] _freqBandHighest;
    public static float[] _audioBand, _audioBandBuffer;

    // 64 band arrays
    private float[] _freqBand64 = new float[64];
    private float[] _bandBuffer64 = new float[64];
    private float[] _bufferDecrease64 = new float[64];
    private float[] _freqBandHighest64 = new float[64];
    public float[] _audioBand64, audioBandBuffer64; // DEAL WITH THIS BEING NON STATIC! TOMORROW!


    public static float _amplitude, _amplitudeBuffer;
    private float maxAmplitude;
    public static float _lowerAmplitude;


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
        MakeFrequencyBands64();
        BandBuffer();
        BandBuffer64();
        CreateAudioBands();
        CreateAudioBands64();
        CalcAmplitude();
        CalcLowerAmplitude();
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

    void CreateAudioBands64() {
        for (int i = 0; i < 64 - 1; i++) {
            if (_freqBand64[i] > _freqBandHighest64[i]) {
                _freqBandHighest64[i] = _freqBand64[i];
            }
            if (_freqBandHighest64[i] != 0) {
                _audioBand64[i] = (_freqBand64[i] / _freqBandHighest64[i]);
                _audioBandBuffer64[i] = (_bandBuffer64[i] / _freqBandHighest64[i]);
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


    void BandBuffer64() {
        for (int g = 0; g < 64 - 1; g++) {
            if (_freqBand64[g] > _bandBuffer64[g] || _bandBuffer64[g] < 0) {
                _bandBuffer64[g] = _freqBand64[g];
                _bufferDecrease64[g] = 0.005f;
            }
            if (_freqBand64[g] < _bandBuffer64[g]) {
                _bandBuffer64[g] -= _bufferDecrease64[g];
                _bufferDecrease64[g] *= 1.2f;
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

    void MakeFrequencyBands64() {
        int count = 0;
        int samplecount = 1;
        int power = 0;
        for (int i = 0; i < 64; i++) {
            if(i == 16 || i == 32 || i == 40 || i == 48 || i == 56) {
                power++;
                samplecount = (int)Mathf.Pow(2, power);
                if(power == 3) {
                    samplecount -= 2;
                }
            }

            float average = 0;
            for (int j = 0; j < samplecount; j++) {
                average += _samples[count] * (count + 1);
                count++;
            }
            average /= count;
            _freqBand64[i] = average * 80;
        }
    }


    /*
     * Average Amplitude across all bands, calculate and set.
     */
    void CalcAmplitude() {
        float amplitudeSum = 0;
        float amplitudeSumBuffer = 0;
        for(int i = 0; i < _audioBand.Length; i++) {
            amplitudeSum += _audioBand[i];
            amplitudeSumBuffer += _audioBandBuffer[i];
        }
        _amplitude = amplitudeSum / _audioBand.Length;
        _amplitudeBuffer = amplitudeSumBuffer / _audioBandBuffer.Length;
    }

    void CalcLowerAmplitude() {
        float amplitudeSum = 0;
        for (int i = 0; i < 4; i++) {
            amplitudeSum += _audioBandBuffer[i];
        }
        _lowerAmplitude = amplitudeSum / 4;
    }
}
