using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movementScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("CRASHHH");
            movementScript.enabled = false;
        }
    }
}
