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
    public float floorY = 0f; // Set this to your floor's Y position

    private Rigidbody rb;
    private Vector3 velocity;
    private float ballRadius;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        // Get the scaled radius from the SphereCollider
        ballRadius = GetComponent<SphereCollider>().radius * transform.localScale.x;
    }

    void FixedUpdate()
    {
        // Apply gravity
        velocity += gravity * Time.fixedDeltaTime;
        Vector3 displacement = velocity * Time.fixedDeltaTime;

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, ballRadius, velocity.normalized, out hit, displacement.magnitude + 0.01f, collisionLayers))
        {
            // Reflect direction and dampen speed
            Vector3 reflectDir = Vector3.Reflect(velocity.normalized, hit.normal);
            float newSpeed = velocity.magnitude * 0.95f;

            newSpeed = Mathf.Clamp(newSpeed, minVelocity, maxVelocity);
            velocity = reflectDir * newSpeed;

            // Move to just before the collision point
            transform.position = hit.point - reflectDir * (ballRadius + 0.01f);
        }
        else
        {
            // Move normally
            transform.position += displacement;
        }

        // Floor protection
        if (transform.position.y < floorY + ballRadius)
        {
            transform.position = new Vector3(transform.position.x, floorY + ballRadius, transform.position.z);
            velocity.y = -velocity.y * 0.7f;
        }

        // Stop very slow movement
        if (velocity.magnitude < minVelocity)
        {
            velocity = Vector3.zero;
            return;
        }
    }

    public void SetVelocity(Vector3 newVel)
    {
        velocity = Vector3.ClampMagnitude(newVel, maxVelocity);
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    // Trigger-based backup bounce logic for vertical racket hits
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Racket"))
        {
            Rigidbody racketRb = other.attachedRigidbody;
            if (racketRb != null)
            {
                Vector3 swingVelocity = racketRb.linearVelocity;

                // Calculate direction from racket to ball
                Vector3 direction = (transform.position - other.transform.position).normalized;

                // Reflect current ball velocity using the swing direction
                Vector3 reflected = Vector3.Reflect(velocity.normalized, direction);

                float combinedSpeed = Mathf.Max(swingVelocity.magnitude, velocity.magnitude);
                SetVelocity(reflected * combinedSpeed);
            }
        }
    }
}
