using UnityEngine;

public class SlingshotCamera : MonoBehaviour
{
    [Header("Targets")]
    public Transform slingshotTarget;        // The Slingshot root / handle center
    public SlingshotInteractions interactions; // Drag handler script

    [Header("Orbit / View controls")]
    public float orbitSpeed = 5f;
    public float minVerticalAngle = -20f;
    public float maxVerticalAngle = 60f;
    public float cameraDistance = 6f;

    [Header("Elastic Pull Effect")]
    public float pullInfluence = 0.3f;
    public float smoothSpeed = 8f;

    private float currentX = 0f;
    private float currentY = 15f; // Initial slightly elevated viewpoint

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;
    }

    void Update()
    {
        // Only allow scene camera orbiting if the player is holding Right Click
        // This stops it from conflicting with left-click rubber band dragging
        if (!interactions.dragging && Input.GetMouseButton(1))
        {
            currentX += Input.GetAxis("Mouse X") * orbitSpeed;
            currentY -= Input.GetAxis("Mouse Y") * orbitSpeed;
            currentY = Mathf.Clamp(currentY, minVerticalAngle, maxVerticalAngle);
        }
    }

    void LateUpdate()
    {
        if (slingshotTarget == null || interactions == null) return;

        // 1. Calculate Orbit Rotation Matrix
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // 2. Base position relative to slingshot center
        Vector3 basePosition = slingshotTarget.position - (rotation * Vector3.forward * cameraDistance);

        // 3. Apply elastic tension offset if pulling
        if (interactions.dragging)
        {
            Vector3 pullOffset = interactions.pouchTarget.position - slingshotTarget.position;
            basePosition += pullOffset * pullInfluence;
        }

        // Smoothly glide camera to destination matrix
        transform.position = Vector3.Lerp(transform.position, basePosition, Time.deltaTime * smoothSpeed);
        
        // Keep looking at the slingshot handle
        transform.LookAt(slingshotTarget.position + Vector3.up * 0.5f);
    }
}




// using UnityEngine;

// public class SlingshotCamera : MonoBehaviour
// {
//     [Header("Targets")]
//     public Transform slingshotHandle; // Reference to the Slingshot Handle bone/center
//     public Transform pouchTarget;    // Reference to the interactive Pouch_Target object

//     [Header("Camera Movement settings")]
//     public Vector3 cameraOffset = new Vector3(0, 1, -5); // Default resting position behind slingshot
//     public float pullInfluence = 0.4f;                  // How much the camera moves with the drag (0 = stationary)
//     public float smoothSpeed = 5f;                       // How smoothly the camera glides

//     private Vector3 standardTargetPosition;

//     void Start()
//     {
//         if (slingshotHandle != null)
//         {
//             // Set the baseline resting location relative to the slingshot
//             standardTargetPosition = slingshotHandle.position + cameraOffset;
//         }
//     }

//     void LateUpdate()
//     {
//         if (slingshotHandle == null || pouchTarget == null) return;

//         // 1. Calculate base target position
//         Vector3 targetCamPos = slingshotHandle.position + cameraOffset;

//         // 2. Add dynamic movement based on where the pouch is being pulled
//         // If pulled back, the camera drifts back with it to frame the tension
//         Vector3 pullOffset = pouchTarget.position - slingshotHandle.position;
//         targetCamPos += pullOffset * pullInfluence;

//         // 3. Smoothly interpolate the camera's position
//         transform.position = Vector3.Lerp(transform.position, targetCamPos, Time.deltaTime * smoothSpeed);

//         // 4. Keep the camera looking steadily at the slingshot core target
//         Vector3 lookTarget = Vector3.Lerp(slingshotHandle.position, pouchTarget.position, 0.3f);
//         transform.LookAt(lookTarget);
//     }
// }