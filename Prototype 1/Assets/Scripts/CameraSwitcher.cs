using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera camera1; // First camera
    public Camera camera2; // Second camera
    private bool isCamera1Active = true; // Tracks which camera is active

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Ensure one camera is enabled, and the other is disabled at the start
        camera1.enabled = true;
        camera2.enabled = false;
    }

    // Update is called once per frame
    void Update()        
    {
        // Check if the C key is pressed
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCameras();
        }
    }

    void SwitchCameras()
    {
        // Toggle cameras
        isCamera1Active = !isCamera1Active;

        camera1.enabled = isCamera1Active;
        camera2.enabled = !isCamera1Active;
    }
    
}
