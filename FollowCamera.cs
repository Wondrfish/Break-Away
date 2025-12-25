using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;         
    public Vector3 offset = new Vector3(0f, 20f, -10f); // Top-down offset
    public float smoothSpeed = 5f;   

    void LateUpdate()
    {
        if (target == null) return;

        // 
        Vector3 desiredPosition = target.position + offset;

        // Smooth 
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Always look at the car 
        transform.LookAt(target);
    }
}