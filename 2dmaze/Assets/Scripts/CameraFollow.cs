using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform; // Drag your player's transform here in the inspector
    public float smoothSpeed = 0.125f; // Speed for camera smoothing
    public Vector3 offset; // Use this if you want any offset from the player

    private void LateUpdate()
    {
        if (playerTransform == null)
            return;

        Vector3 desiredPosition = playerTransform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
