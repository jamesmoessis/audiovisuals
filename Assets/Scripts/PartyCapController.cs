using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PartyCapController : MonoBehaviour {
    public GameObject capPrefab;

    private const int NUM_CAPS = 64;
    private GameObject[] apcays = new GameObject[NUM_CAPS];
    private static float baseYScale = 0.05f;
    private GameObject leftPlane;
    private GameObject rightPlane;

    public Audio source;

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
            cap.transform.position += (2f*moveUpOne) + xVector;
            cap.name = "PartyCap" + i;
            apcays[i] = cap;
        }
        leftPlane = GameObject.Find("LeftPlane");
        rightPlane = GameObject.Find("RightPlane");
    }

    void Update() {
        UpdateCapHeights();
        SqueezeCaps();
    }

    void UpdateCapHeights() {
        for (int i = 0; i < NUM_CAPS; i++) {
            Debug.Log(i);
            apcays[i].transform.localScale = new Vector3(
                apcays[i].transform.localScale.x,
                baseYScale + 0.1f * source._audioBandBuffer64[i],
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
}
