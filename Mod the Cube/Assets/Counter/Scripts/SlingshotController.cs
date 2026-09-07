using UnityEngine;

public class SlingshotController : MonoBehaviour
{
    [Header("Bone References")]
    public Transform leftProngBone;
    public Transform rightProngBone;
    public Transform pouchTarget; 

    private Vector3 leftInitialScale;
    private Vector3 rightInitialScale;
    private float leftInitialLength;
    private float rightInitialLength;

    // Stores the relational orientation delta between bone and target at setup
    private Quaternion leftRotationOffset;
    private Quaternion rightRotationOffset;

    void Start()
    {
        if (leftProngBone == null || rightProngBone == null || pouchTarget == null)
        {
            Debug.LogError("SlingshotController references are not assigned.");
            enabled = false;
            return;
        }

        leftInitialScale = leftProngBone.localScale;
        rightInitialScale = rightProngBone.localScale;

        leftInitialLength = Vector3.Distance(leftProngBone.position, pouchTarget.position);
        rightInitialLength = Vector3.Distance(rightProngBone.position, pouchTarget.position);

        // Calculate and cache the native Blender rotation offset relative to world tracking direction
        Vector3 leftDir = (pouchTarget.position - leftProngBone.position).normalized;
        leftRotationOffset = Quaternion.Inverse(Quaternion.LookRotation(leftDir)) * leftProngBone.rotation;

        Vector3 rightDir = (pouchTarget.position - rightProngBone.position).normalized;
        rightRotationOffset = Quaternion.Inverse(Quaternion.LookRotation(rightDir)) * rightProngBone.rotation;
    }

    void LateUpdate()
    {
        UpdateBand(leftProngBone, pouchTarget.position, leftInitialLength, leftInitialScale, leftRotationOffset);
        UpdateBand(rightProngBone, pouchTarget.position, rightInitialLength, rightInitialScale, rightRotationOffset);
    }

    void UpdateBand(Transform bone, Vector3 targetPosition, float initialLength, Vector3 initialScale, Quaternion rotationOffset)
    {
        Vector3 direction = targetPosition - bone.position;
        float currentLength = direction.magnitude;

        if (currentLength <= 0.0001f) return;

        direction.Normalize();

        // Flawlessly look at the pouch target across all 3D rotational degrees of freedom
        bone.rotation = Quaternion.LookRotation(direction) * rotationOffset;

        // Calculate structural dynamic stretch scale factor
        float scaleFactor = currentLength / initialLength;
        scaleFactor = Mathf.Max(scaleFactor, 0.01f);

        // Blender rig uses Y for length, X/Z for thickness squashing
        bone.localScale = new Vector3(
            initialScale.x / Mathf.Sqrt(scaleFactor),
            initialScale.y * scaleFactor,
            initialScale.z / Mathf.Sqrt(scaleFactor)
        );
    }
}
