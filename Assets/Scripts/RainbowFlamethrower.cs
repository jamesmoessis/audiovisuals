using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RainbowFlamethrower : MonoBehaviour {
    private ParticleSystem flameStream;
    public int audioBand;


    void Start() {
        flameStream = GetComponent<ParticleSystem>();
        flameStream.startSpeed = 0;
    }

    void Update() {
        flameStream.startSpeed = Audio._audioBand[audioBand]*13;
        flameStream.startSize = Audio._audioBand[audioBand]*9;
    }
}
