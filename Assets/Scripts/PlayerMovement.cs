using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;
    public float sideSpeed = 10f;

    [Header("Jump and Ground")]
    public float jumpForce = 8f;
    public float gravity = -20f;
    private float verticalVelocity;
    private bool isGrounded = true;

    private BoxCollider playerCol;
    private float originalHeight;
    private float originalCenterY;

    private int desiredLane = 1;
    void Start()
    {
        playerCol = GetComponent<BoxCollider>();
        originalHeight = playerCol.size.y;
        originalCenterY = playerCol.center.y;
    }

    void Update()
    {
        Vector3 forwardMove = Vector3.forward * forwardSpeed * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
            desiredLane++;
            if (desiredLane == 3) desiredLane = 2;
            }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            desiredLane--;
            if (desiredLane == -1) desiredLane = 0;
        }
        Vector3 targetPosition = transform.position;
        if (desiredLane == 0)
        {
            targetPosition.x = -laneDistance;
        }
        else if (desiredLane == 1)
        {
            targetPosition.x = 0;
        }
        else if (desiredLane == 2) 
        { 
            targetPosition.x = laneDistance;
        }
        float newX = Mathf.Lerp(transform.position.x, targetPosition.x, Time.deltaTime * sideSpeed);
        if (isGrounded)
        {
            verticalVelocity = -0.1f;

            if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                verticalVelocity = jumpForce;
                isGrounded = false;
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(Slide());
        }
        Vector3 newPosition = new Vector3(newX, transform.position.y + (verticalVelocity * Time.deltaTime), transform.position.z + forwardMove.z);
        transform.position = newPosition;

        if (transform.position.y <= 0.5f)
        {
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            isGrounded = true;
        }
    }
    IEnumerator Slide()
    {
        if (!isGrounded)
        {
            verticalVelocity = -jumpForce;
        }
        playerCol.size = new Vector3(playerCol.size.x, originalHeight / 2, playerCol.size.z);
        playerCol.center = new Vector3(playerCol.center.x, originalCenterY / 2, playerCol.center.z);

        yield return new WaitForSeconds(1f);

        playerCol.size = new Vector3(playerCol.size.x, originalHeight, playerCol.size.z);
        playerCol.center = new Vector3(playerCol.center.x, originalCenterY, playerCol.center.z);
    }
}
