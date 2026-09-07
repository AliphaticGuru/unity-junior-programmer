using UnityEngine;

public class SlingshotInteractions : MonoBehaviour
{
    [Header("References")]
    public Transform handleCenter;
    public Transform pouchTarget;

    [Header("Movement")]
    public float maxPullDistance = 2f;

    [Header("Projectile Setup")]
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public float releaseForce = 50f;

    private Camera mainCamera;
    private Collider pouchCollider;
    [HideInInspector] public bool dragging;

    private Vector3 idleLocalPosition;
    private float initialScreenDepth;
    private Vector3 dragOffset;

    void Start()
    {
        mainCamera = Camera.main;
        pouchCollider = pouchTarget.GetComponent<Collider>();
        
        // Remember the local resting place of the pouch
        idleLocalPosition = pouchTarget.localPosition;

        if (pouchCollider == null)
        {
            Debug.LogError("Pouch_Target needs a Collider.");
        }
    }

    void Update()
    {
        // -----------------------------------------
        // START DRAG
        // -----------------------------------------
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider == pouchCollider)
                {
                    dragging = true;

                    // Capture how far away the pouch is from the camera along the Z view-axis
                    initialScreenDepth = mainCamera.WorldToScreenPoint(pouchTarget.position).z;

                    // Calculate offset so the pouch doesn't snap abruptly to its pivot point when clicked
                    Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, initialScreenDepth));
                    dragOffset = pouchTarget.position - mouseWorldPos;
                }
            }
        }

        // -----------------------------------------
        // DRAG (Completely Unlocked on All Axes)
        // -----------------------------------------
        if (dragging && Input.GetMouseButton(0))
        {
            // Convert current 2D screen positions smoothly into 3D world space using captured depth matrix
            Vector3 currentMouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, initialScreenDepth);
            Vector3 targetPosition = mainCamera.ScreenToWorldPoint(currentMouseScreenPos) + dragOffset;

            // ---------------------------------
            // LIMIT PULL DISTANCE (Sphere Clamp)
            // ---------------------------------
            Vector3 pull = targetPosition - handleCenter.position;

            if (pull.magnitude > maxPullDistance)
            {
                pull = pull.normalized * maxPullDistance;
                targetPosition = handleCenter.position + pull;
            }

            pouchTarget.position = targetPosition;
        }

        // -----------------------------------------
        // RELEASE
        // -----------------------------------------
        if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;
            
            FireProjectile();
            // Snap back to resting center position smoothly
            pouchTarget.localPosition = idleLocalPosition;
        }
    }

    void FireProjectile()
    {
        if (projectilePrefab == null || spawnPoint == null) return;

        GameObject rock = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = rock.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 fireDirection = handleCenter.position - pouchTarget.position;
            rb.AddForce(fireDirection.normalized * releaseForce, ForceMode.Impulse);

            // NEW: Play launch snap sound and spawn a subtle release dust particle ring
            if (EffectsManager.Instance != null)
            {
                EffectsManager.Instance.PlaySound(EffectsManager.Instance.launchSFX, 0.8f);
                EffectsManager.Instance.SpawnParticles(
                    EffectsManager.Instance.launchParticlesPrefab, 
                    spawnPoint.position, 
                    Quaternion.identity
                );
            }
        }
    }

    public Vector3 GetLaunchVelocity()
    {
        Vector3 fireDirection = handleCenter.position - pouchTarget.position;
        return fireDirection.normalized * releaseForce; 
    }
}




// using UnityEngine;

// public class SlingshotInteractions : MonoBehaviour
// {
//     [Header("Slingshot Setup")]
//     public Transform handleCenter;    // Center point of the wooden frame handle
//     public Transform pouchTarget;    // The empty GameObject your bone script follows
//     public float maxPullDistance = 3.0f; // Limit how far the player can stretch the rubber band
//     public float releaseForce = 50f;     // Speed/force of the shot

//     [Header("Projectile Setup")]
//     public GameObject projectilePrefab; // The rock/ball prefab to launch
//     public Transform spawnPoint;         // Position right inside the pouch

//     private Vector3 idlePosition;
//     private bool isDragging = false;
//     private Camera mainCamera;
//     private Collider pouchCollider;
//     private float initialMouseDepth;
//     private Vector3 cursorOffset;

//     void Start()
//     {
//         mainCamera = Camera.main;
//         // Remember where the pouch sits when resting normally
//         idlePosition = pouchTarget.localPosition;
//         pouchCollider = pouchTarget.GetComponent<Collider>();
//         if (pouchCollider == null)
//         {
//             Debug.LogError("Please add a Collider component to your Pouch Target object!");
//         }
//     }

//     void Update()
//     {
//         // 1. Detect Initial Click
//         if (Input.GetMouseButtonDown(0))
//         {
//             Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
//             if (Physics.Raycast(ray, out RaycastHit hit))
//             {
//                 // Check if the ray hit the pouch target collider
//                 if (hit.collider == pouchCollider || hit.transform == pouchTarget)
//                 {
//                     isDragging = true;

//                     // Capture how far away the pouch is from the camera along the camera's view axis
//                     initialMouseDepth = mainCamera.WorldToScreenPoint(pouchTarget.position).z;
                    
//                     // Calculate exactly where the player clicked relative to the pouch center
//                     Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, initialMouseDepth));
//                     cursorOffset = pouchTarget.position - mouseWorldPos;
//                 }
//             }
//         }

//         // 2. Handle Dragging Mechanics
//         if (isDragging && Input.GetMouseButton(0))
//         {
//             // Convert current 2D screen mouse position into 3D world space using the captured depth
//             Vector3 currentMouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, initialMouseDepth);
//             Vector3 currentMouseWorldPos = mainCamera.ScreenToWorldPoint(currentMouseScreenPos) + cursorOffset;

//             // Calculate the 3D direction vector from the handle center to your mouse
//             Vector3 pullVector = currentMouseWorldPos - handleCenter.position;

//             // Clamp it in a perfect sphere radius around the handle, not a flat plane
//             if (pullVector.magnitude > maxPullDistance)
//             {
//                 pullVector = pullVector.normalized * maxPullDistance;
//             }

//             // Apply the true 3D position
//             pouchTarget.position = handleCenter.position + pullVector;
//         }

//         // 3. Release Trigger
//         if (Input.GetMouseButtonUp(0) && isDragging)
//         {
//             isDragging = false;
//             FireProjectile();
//             pouchTarget.localPosition = idlePosition;
//         }
//     }

//     void FireProjectile()
//     {
//         if (projectilePrefab == null || spawnPoint == null) return;

//         // Instantiate the rock at the pouch position
//         GameObject rock = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
//         Rigidbody rb = rock.GetComponent<Rigidbody>();

//         if (rb != null)
//         {
//             // Calculate direction pointing forward from where it was pulled back
//             Vector3 fireDirection = handleCenter.position - pouchTarget.position;
            
//             // Apply physical force to fling it forward
//             rb.AddForce(fireDirection.normalized * releaseForce, ForceMode.Impulse);
//         }
//     }
// }
