using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Score Settings")]
    public TextMeshProUGUI scoreText;
    private float score = 0f;

    [Header("Game Speed Settings")]
    public PlayerMovement playerMovement;
    public float speedIncreaseRate = 0.1f;
    public float maxSpeed = 30f;

    void Update()
    {
        if (playerMovement.enabled)
        {
            score = playerMovement.transform.position.z;
            scoreText.text = ((int)score + 12).ToString();

            if (playerMovement.forwardSpeed < maxSpeed)
            {
                playerMovement.forwardSpeed += speedIncreaseRate * Time.deltaTime;
            }
        }
    }
}
