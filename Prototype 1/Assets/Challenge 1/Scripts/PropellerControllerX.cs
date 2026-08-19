using UnityEngine;

public class PropellerControllerX : MonoBehaviour
{
    public float propellerSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 propellerInput;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Spin the propeller at a constant rate
        transform.Rotate(Vector3.forward * propellerSpeed * Time.deltaTime * propellerInput.x);
    }
}
