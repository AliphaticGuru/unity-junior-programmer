using UnityEngine;

public class TrajectoryPredictor : MonoBehaviour
{
    [Header("References")]
    public SlingshotInteractions interactions;
    public Transform spawnPoint;
    public Transform landingDot; // Your landing indicator object

    [Header("Simulation Parameters")]
    [Range(10, 100)] public int resolution = 30; // Max ray steps to look forward
    public float stepTime = 0.05f;               // Virtual time step physics delta
    public LayerMask hitLayers;                  // Terrain/Targets layers to collide against

    void Update()
    {
        if (interactions == null || spawnPoint == null || landingDot == null) return;

        // Only show landing dot helper when pulling the band back
        if (interactions.dragging)
        {
            landingDot.gameObject.SetActive(true);
            PredictLandingPoint();
        }
        else
        {
            landingDot.gameObject.SetActive(false);
        }
    }

    void PredictLandingPoint()
    {
        Vector3 currentPosition = spawnPoint.position;
        
        // Initial velocity vector derived from the interaction script physics force formulas
        Vector3 currentVelocity = interactions.GetLaunchVelocity(); 
        
        // Accounting for Unity ForceMode.Impulse calculations: Velocity = Force / Mass.
        // Assuming a standard mass calculation variable of 1.0f. Adjust if your rock has a custom mass.
        float rockMass = 1f; 
        currentVelocity /= rockMass;

        Vector3 nextPosition;

        for (int i = 0; i < resolution; i++)
        {
            // Formula tracking gravity drag over standard acceleration splits
            currentVelocity += Physics.gravity * stepTime;
            nextPosition = currentPosition + currentVelocity * stepTime;

            // Segment Raycast checking if virtual flight line paths intersect objects
            Vector3 direction = nextPosition - currentPosition;
            float distance = direction.magnitude;

            if (Physics.Raycast(currentPosition, direction.normalized, out RaycastHit hit, distance, hitLayers))
            {
                // Surface found! Snap the visual dot to the exact impact terrain point
                landingDot.position = hit.point;
                landingDot.rotation = Quaternion.LookRotation(hit.normal); // Aligns plane flat to ground
                return;
            }

            currentPosition = nextPosition;
        }

        // If the path misses all terrain constraints before resolution limit, hide it safely
        landingDot.gameObject.SetActive(false);
    }
}
