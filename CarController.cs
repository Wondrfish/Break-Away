using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float acceleration = 60f;
    public float maxSpeed = 30f;
    public float turnSpeed = 160f;
    public float friction = 2f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Adjust damping 
        rb.linearDamping = 0.8f;
        rb.angularDamping = 2f;
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        // Move the car
        if (Mathf.Abs(moveInput) > 0.1f)
        {
            Vector3 force = transform.forward * moveInput * acceleration;
            rb.AddForce(force, ForceMode.Acceleration);
        }

        // Turn the car only when moving
        if (Mathf.Abs(moveInput) > 0.1f)
        {
            float turn = turnInput * turnSpeed * Time.fixedDeltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }

        // Manual slow down
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            rb.linearVelocity -= rb.linearVelocity.normalized * friction * Time.fixedDeltaTime;
        }

        //  max speed
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Police"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.DamagePlayer();
            }

            Debug.Log("Hit by police!");
        }
    }
}