using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PartyCapController : MonoBehaviour {
    public GameObject capPrefab;
    public Audio source;

    private const int NUM_CAPS = 32;
    private GameObject[] apcays = new GameObject[NUM_CAPS];
    private static float baseYScale = 0.05f;
    private GameObject leftPlane;
    private GameObject rightPlane;
    private Color colour;

    void Start() {
        GameObject cap;
        Vector3 moveUpOne = new Vector3(0, 1, 0);
        Vector3 xVector = new Vector3(0, 0, 0);
        float[] xValues = MathUtil.LinSpace(-2.5f, 14f, NUM_CAPS);
        
        for(int i = 0; i < NUM_CAPS; i++) {
            cap = Instantiate(capPrefab);
            cap.transform.position = this.transform.position;
            cap.transform.parent = this.transform; // may need to get rid of this line
            xVector.x = xValues[i];
            cap.transform.position += (1.8f*moveUpOne) + xVector;
            cap.name = "PartyCap" + i;
            cap.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
            cap.transform.localScale *= 2;
            apcays[i] = cap;
        }
        leftPlane = GameObject.Find("LeftPlane");
        rightPlane = GameObject.Find("RightPlane");
        colour = new Color(0.5f, 0.5f, 0.5f);
    }

    void Update() {
        UpdateCapHeights();
        SqueezeCaps();
        //UpdateColour();
    }

    void UpdateCapHeights() {
        for (int i = 0; i < NUM_CAPS; i++) {
            apcays[i].transform.localScale = new Vector3(
                apcays[i].transform.localScale.x,
                baseYScale + 0.13f * source._audioBandBuffer64[i*2],
                apcays[i].transform.localScale.z);
        }
    }

    void SqueezeCaps() {
        // according to left and right wall, let's squeeze them in
        float[] xValues = MathUtil.LinSpace(leftPlane.transform.position.x, rightPlane.transform.position.x, NUM_CAPS);

        for (int i = 0; i < NUM_CAPS; i++) {
            apcays[i].transform.position = new Vector3(xValues[i]+0.15f, 
                apcays[i].transform.position.y, 
                apcays[i].transform.position.z);
        }
    }

    // Currently not working: commenting out for now
    void UpdateColour() {
        colour.r = Audio._lowerAmplitude;
        colour.g = Audio._amplitude;
        colour.b = Audio._amplitudeBuffer;
        colour.r = 1.0f;
        colour.g = 1.0f;
        colour.b = 1.0f;
        Material capMat;
        for(int i = 0; i < NUM_CAPS; i++) {
            // This shader is driving me cray vray 
            //apcays[i].GetComponent<MeshRenderer>().material.SetColor("_EmissiveColor", Color.red); // This just makes it white... WHY?
            apcays[i].GetComponent<MeshRenderer>().material.SetFloat("_EmissionIntensity", Audio._amplitude); // This just makes it white... WHY?
            apcays[i].GetComponent<MeshRenderer>().material.SetFloat("_EmissionIntensity", 0f); // This just makes it white... WHY?
        }
    }
}
