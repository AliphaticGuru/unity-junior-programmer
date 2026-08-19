using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public Vector3 cubeLocation = new Vector3(3, 4, 1);
    public float cubeScale = 1.3f;
    public float cubeRotation = 10.0f;

    public MeshRenderer Renderer;
    
    void Start()
    {
        Invoke("newColor", 1.8f); 
        transform.position = cubeLocation;
        transform.localScale = Vector3.one * cubeScale;
    }

    void Update()
    {
        transform.Rotate(cubeRotation * Time.deltaTime, 0.0f, 0.0f);
    }

    void newColor()
    {
        
        Material material = Renderer.material;

        Color cubeColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        
        material.color = cubeColor;

        float colorInterval = Random.Range(1.4f, 4.0f);
        Invoke("newColor", colorInterval);
    }
}
