using UnityEngine;

public class RacketHitTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            BallCustomPhysics ball = other.GetComponent<BallCustomPhysics>();
            if (ball != null)
            {
                Rigidbody racketRb = GetComponent<Rigidbody>();
                Vector3 swingVelocity = racketRb ? racketRb.linearVelocity : Vector3.forward;

                // Apply velocity boost to ball from racket
                ball.SetVelocity(swingVelocity * 0.8f); // adjust multiplier as needed
            }
        }
    }
}
