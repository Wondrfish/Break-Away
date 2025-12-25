using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PoliceChaseAI : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Chase Settings")]
    public float chaseSpeed = 18f;
    public float rotationSpeed = 6f;
    public float detectionRange = 60f;
    public float obstacleAvoidanceRange = 5f;

    [Header("Close Follow Settings")]
    public float closeDistance = 4f;

    private Rigidbody rb;

    // Save both constraint states
    private RigidbodyConstraints normalConstraints;
    private RigidbodyConstraints closeConstraints;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 2f;
        rb.angularDamping = 4f;

        // NORMAL = prevent roll + pitch only
        normalConstraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;

        // CLOSE = prevent roll + pitch + turn (freeze Y)
        closeConstraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezeRotationZ;

        rb.constraints = normalConstraints;
    }

    void FixedUpdate()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > detectionRange) return;

        // CLOSE CHASE MODE — prevent jitter
        if (distance < closeDistance)
        {
            rb.constraints = closeConstraints;   // <— freezes Y rotation when close
            FollowSmooth();
            return;
        }

        // NORMAL MODE
        rb.constraints = normalConstraints;
        ChaseNormal();
    }

    void FollowSmooth()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * 0.4f * Time.fixedDeltaTime));

        rb.MovePosition(rb.position + transform.forward * chaseSpeed * 0.8f * Time.fixedDeltaTime);
    }

    void ChaseNormal()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;

        // Raycast for obstacle avoidance
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out RaycastHit hit, obstacleAvoidanceRange))
        {
            direction += hit.normal * 2f;
        }

        Quaternion targetRot = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));

        rb.MovePosition(rb.position + transform.forward * chaseSpeed * Time.fixedDeltaTime);
    }
}
