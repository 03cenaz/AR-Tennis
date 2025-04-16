using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TrackerFollow : MonoBehaviour
{
    public Transform targetToFollow;    
    public Transform attachPoint;      

    public Vector3 RacketVelocity { get; private set; }

    private Vector3 lastAttachPos;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        lastAttachPos = attachPoint.position;
    }

    void FixedUpdate()
    {
        // Calculate offset between racket center and attach point
        Vector3 offset = transform.position - attachPoint.position;

        // Move racket so attach point follows controller
        Vector3 desiredPosition = targetToFollow.position + offset;
        rb.MovePosition(desiredPosition);
        rb.MoveRotation(targetToFollow.rotation);

        // Track velocity from attach point
        RacketVelocity = (attachPoint.position - lastAttachPos) / Time.fixedDeltaTime;
        lastAttachPos = attachPoint.position;
    }
}
