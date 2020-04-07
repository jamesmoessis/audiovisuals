using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RainbowFlamethrower : MonoBehaviour {
    private ParticleSystem flameStream;
    private ParticleSystem embers;
    public int audioBand;
    public GameObject emberobj;


    void Start() {
        flameStream = GetComponent<ParticleSystem>();
        flameStream.startSpeed = 0;
        //embers = GetComponentInChildren<ParticleSystem>();
        //embers.enableEmission = true;
        //emberobj = GameObject.Find("FireEmbers");
        //Debug.Log(emberobj);
        //Debug.Log(embers.emission.rateOverTime.constant);
        //embers = emberobj.GetComponent<ParticleSystem>();
        //embers.enableEmission = true;
    }

    void Update() {
        flameStream.startSpeed = Audio._audioBand[audioBand]*13;
        flameStream.startSize = Audio._audioBand[audioBand]*9;
        //embers.emission.S. = 50 + Audio._audioBand[audioBand] * 250;
        
    }
}
