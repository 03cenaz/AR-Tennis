using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class BallCustomPhysics : MonoBehaviour
{
    [SerializeField] private LayerMask collisionLayers;

    [Header("Velocity Settings")]
    public float maxVelocity = 8f;
    public float minVelocity = 1f;

    [Header("Gravity Settings")]
    public Vector3 gravity = new Vector3(0, -9.81f, 0);
    public float rayOriginOffset = 0.01f;

    private Rigidbody rb;
    private Vector3 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        // Apply gravity
        velocity += gravity * Time.fixedDeltaTime;
        Vector3 displacement = velocity * Time.fixedDeltaTime;

        RaycastHit hit;
        Vector3 rayOrigin = transform.position - velocity.normalized * rayOriginOffset;

        if (Physics.Raycast(rayOrigin, velocity.normalized, out hit, displacement.magnitude + rayOriginOffset, collisionLayers))
        {
            // Reflect direction and reduce speed
            Vector3 reflectDir = Vector3.Reflect(velocity.normalized, hit.normal);
            float newSpeed = velocity.magnitude * 0.75f; // reduce bounce energy

            newSpeed = Mathf.Clamp(newSpeed, minVelocity, maxVelocity);
            velocity = reflectDir * newSpeed;
            velocity.y *= 0.5f; // dampen upward bounce

            // Move to just before the collision point
            transform.position = hit.point - reflectDir * 0.001f;
        }
        else
        {
            transform.position += displacement;
        }

        if (velocity.magnitude < minVelocity)
        {
            velocity = Vector3.zero;
        }

        // Debug visualization
        Debug.DrawRay(rayOrigin, velocity.normalized * 0.5f, Color.cyan, 0.1f);
    }

    public void SetVelocity(Vector3 newVel)
    {
        velocity = Vector3.ClampMagnitude(newVel, maxVelocity);
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Racket"))
        {
            Rigidbody racketRb = other.attachedRigidbody;
            if (racketRb != null)
            {
                Vector3 swingVelocity = racketRb.linearVelocity;

                Vector3 direction = (transform.position - other.transform.position).normalized;
                Vector3 reflected = Vector3.Reflect(velocity.normalized, direction);

                float combinedSpeed = Mathf.Max(swingVelocity.magnitude, velocity.magnitude);
                SetVelocity(reflected * combinedSpeed);
            }
        }
    }
}
