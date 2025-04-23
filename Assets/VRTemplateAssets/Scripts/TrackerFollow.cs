using UnityEngine;

public class TrackerFollow : MonoBehaviour
{
    public Transform targetToFollow;
    public Vector3 RacketVelocity { get; private set; }

    private Vector3 lastPosition;

    void Start()
    {
        if (targetToFollow == null)
        {
            Debug.LogWarning("TrackerFollow: targetToFollow not assigned.");
        }

        lastPosition = targetToFollow != null ? targetToFollow.position : transform.position;
    }

    void FixedUpdate()
    {
        if (targetToFollow == null) return;

        Vector3 current = targetToFollow.position;
        RacketVelocity = (current - lastPosition) / Time.fixedDeltaTime;
        lastPosition = current;

        transform.position = current;
        transform.rotation = targetToFollow.rotation;
    }
}