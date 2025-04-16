using UnityEngine;

public class AttachToHand : MonoBehaviour
{
    public Transform handGripPoint; 
    void Start()
    {
        if (handGripPoint != null)
        {
            // Option 1: Exact alignment using current local transform
            transform.SetParent(handGripPoint, false);

            // Option 2: Uncomment this if you want to use manual offsets
            // transform.SetParent(handGripPoint);
            // transform.localPosition = gripOffset;
            // transform.localRotation = gripRotation;
        }
        else
        {
            Debug.LogWarning("AttachToHand: handGripPoint is not assigned.");
        }
    }
}
