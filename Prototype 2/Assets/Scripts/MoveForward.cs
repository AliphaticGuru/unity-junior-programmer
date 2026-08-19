using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed = 40.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move the object forward along the z-axis at a constant rate
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
