using UnityEngine;

[RequireComponent(typeof(BallCustomPhysics))]
public class BallDistanceGrabThrow : MonoBehaviour
{
    private BallCustomPhysics ballPhysics;
    private Vector3 lastPosition;
    private Vector3 velocity;

    private void Awake()
    {
        ballPhysics = GetComponent<BallCustomPhysics>();
    }

    private void LateUpdate()
    {
        velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }

    public void OnBallReleased()
    {
        ballPhysics.SetVelocity(velocity);
    }
}