using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 10f;
    public float height = 5f;
    public float smoothSpeed = 0.125f;
    public float rotationSpeed = 5f;
    public float verticalRotationLimit = 80f;

    private float currentRotationX = 0f;
    private float currentRotationY = 0f;

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned in the CameraFollow script.");
            return;
        }

        currentRotationX += Input.GetAxis("Mouse X") * rotationSpeed;
        currentRotationY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        currentRotationY = Mathf.Clamp(currentRotationY, -verticalRotationLimit, verticalRotationLimit);

        Quaternion rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
        Vector3 offset = rotation * new Vector3(0, height, -distance);
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        transform.LookAt(target);
    }
}
