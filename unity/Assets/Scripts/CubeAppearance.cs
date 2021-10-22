using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAppearance : MonoBehaviour
{
    private Color cubeColor;
    public Color GetCubeColor() { return cubeColor; } 

    // Start is called before the first frame update
    void Start()
    {
        ChangeCubeColor(new Color(Random.Range(0.3f, 1.0f), Random.Range(0.3f, 1.0f), Random.Range(0.3f, 1.0f), 1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeCubeColor(Color color)
    {
        cubeColor = color;
        Material mat = GetComponent<Renderer>().material;
        mat.SetColor("_Color", cubeColor);
    }
}
