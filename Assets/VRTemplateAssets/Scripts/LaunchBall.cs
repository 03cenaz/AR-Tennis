using UnityEngine;
using UnityEngine.InputSystem; // InputSystem required for controller input
using TMPro;

public class LaunchBall : MonoBehaviour
{
    public GameObject ballPrefab;       // Assign your ball prefab here
    public Transform spawnPoint;        // Assign your BallLauncher transform
    public InputActionProperty spawnAction; // Button input action

    private void OnEnable()
    {
        spawnAction.action.Enable();
    }

    private void OnDisable()
    {
        spawnAction.action.Disable();
    }

    void Update()
    {
      // Debug.Log("Update");
        if (spawnAction.action.WasPressedThisFrame())
        {
           // Debug.Log("Hit X");
            Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}