using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movementScript;
    public GameObject restartUI;
    private bool isDead = false;

    [Header("Stumble Settings")]
    public float stumbleTopMargin = 0.6f;
    public float stumbleSpeedPenalty = 0.5f;
    public float stumbleForgivenessTime = 0.5f;
    public float speedRestoreTime = 1f;

    [Header("Referances")]
    public CameraFollow cameraScript;

    private bool isStumbling = false;
    private float stumbleTimer = 0f;
     void Update()
    {
        if (isDead && Input.GetKeyDown(KeyCode.Space))
        {
            Restart();
        }

        if (isStumbling)
        {
            stumbleTimer -= Time.deltaTime;
            if (stumbleTimer <= 0f )
            {
                isStumbling = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") && !isDead)
        {
            float obstacleTopY = other.bounds.max.y;
            float obstacleCenterY = other.bounds.center.y;
            float playerFeetY = transform.position.y - (transform.localScale.y / 2f);
            float subtract = obstacleTopY - playerFeetY;

            if (subtract > 0 && subtract <= stumbleTopMargin && playerFeetY > obstacleCenterY)
            {
                Stumble();
            }
            else
            {
                Die();
            }
        }
    }
    public void Stumble()
    {
        if (isStumbling)
        {
            Die();
            return;
        }
        isStumbling = true;
        stumbleTimer = stumbleForgivenessTime;
        float oldSpeed = movementScript.forwardSpeed;
        movementScript.forwardSpeed *= stumbleSpeedPenalty;
        if (cameraScript != null)
        {
            cameraScript.TriggerShake(0.2f, 0.2f);
        }
        StartCoroutine(RestoreSpeed(oldSpeed));
    }
    IEnumerator RestoreSpeed(float targetSpeed)
    {
        yield return new WaitForSeconds(speedRestoreTime);
        if (!isDead)
        {
            movementScript.forwardSpeed = targetSpeed;
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
