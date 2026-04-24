using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movementScript;
    public GameObject restartUI;
    private bool isDead = false;
     void Update()
    {
        if (isDead && Input.GetKeyDown(KeyCode.Space))
        {
            Restart();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") && !isDead)
        {
            Die();
        }
    }
    void Die()
    {
        isDead = true;
        movementScript.enabled = false;

        if (restartUI != null)
        {
            restartUI.SetActive(true);
        }
    }
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
