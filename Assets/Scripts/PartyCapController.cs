using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PartyCapController : MonoBehaviour {
    public GameObject capPrefab;

    private const int NUM_CAPS = 64;
    private GameObject[] apcays = new GameObject[NUM_CAPS];
    private static float baseYScale = 0.05f;

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
    }

    void Update() {
        for(int i = 0; i < NUM_CAPS; i++) {
            apcays[i].transform.localScale = new Vector3(
                apcays[i].transform.localScale.x, 
                baseYScale + 0.1f*AudioPeer.FindObjectOfType<Audio>()._samples[i],
                apcays[i].transform.localScale.z);
        }
    }
}
