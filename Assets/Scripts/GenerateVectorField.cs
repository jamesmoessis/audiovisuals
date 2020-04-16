using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateVectorField : MonoBehaviour {
    Texture3D texture;

    void Start() {
        texture = CreateTexture3D(64);
        UnityEditor.AssetDatabase.CreateAsset(texture, "Assets/t3d.asset");
        Debug.Log(UnityEditor.AssetDatabase.GetAssetPath(texture));
    }

    Texture3D CreateTexture3D(int size) {
        Color[] colorArray = new Color[size * size * size];
        texture = new Texture3D(size, size, size, TextureFormat.RGB24, true);
        float r = 1 / (size - 1); // they want to make it a value <=1 so I think that's why it's this weird thing. 
        r = 5;

        float r1 = 2f;
        float r2 = 10f;
        float r3 = 15f;
        // Should really create a function for the vector components and have it return something within reasonable bounds 0 - 1. 
        // But how does that translate? Vectors can be negative. 
        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {
                for (int z = 0; z < size; z++) {

                    //colorArray[x + (y * size) + (z * size * size)] = new Color(0.5f, 0.5f, 0.5f, 1);
                    //colorArray[x + (y * size) + (z * size * size)] = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1), 1);
                    //texture.SetPixel(x, y, z, new Color(x * r1, y * r2, z * r3, 1));
                    //texture.SetPixel(x, y, z, new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1), 1));
                    texture.SetPixel(x, y, z, new Color(Mathf.Sin(x), Mathf.Sin(y), Mathf.Sin(z), 1));
                    
                }
            }
        }
        // perhaps simplify by using 3 setpixel(x,y,z) in a loop? 
        // Everything is travelling in the negative direction. Tryin to realise why. 
        //texture.SetPixels(colorArray);
        texture.Apply();
        return texture;
    }
}