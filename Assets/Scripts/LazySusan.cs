using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LazySusan : MonoBehaviour {
    /*
     * This class keeps track of a lone singleton, which 
     * is an index we use to jump to the next scene after
     * a certain amount of time has passed.
     */
    public static int index = 0;
    void Start() {
        index = index + 1;
        if (index >= SceneManager.sceneCountInBuildSettings) {
            index = 0;
        }
    }

    void Update() {
        if (Time.timeSinceLevelLoad > 40) {
            SceneManager.LoadScene(index);
        }
    }
}
