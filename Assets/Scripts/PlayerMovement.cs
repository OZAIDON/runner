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

    [Header("Environment Sensing")]
    public LayerMask groundLayer;

    [Header("Input Buffer")]
    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    private Rigidbody _rb;
    private int desiredLane = 1;
    private float originalScaleY;
    private bool jumpRequest = false;
    private float visualBump = 0f;
    private Coroutine bumpCoroutine;
    private Coroutine slideCoroutine;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        originalScaleY = transform.localScale.y;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (desiredLane < 2)
            {
                if (IsPathClear(1))
                {
                    desiredLane++;
                }
                else
                {
                    if (bumpCoroutine != null)
                    {
                        StopCoroutine(bumpCoroutine);
                    }
                    bumpCoroutine = StartCoroutine(BumpEffect(1));
                }
            } 
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (desiredLane > 0)
            {
                if (IsPathClear(-1))
                {
                    desiredLane--;
                }
                else
                {
                    if (bumpCoroutine != null)
                    {
                        StopCoroutine(bumpCoroutine);
                    }
                    bumpCoroutine = StartCoroutine(BumpEffect(-1));
                }
            }
        }

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f && isGrounded)
        {
            jumpRequest = true;
            jumpBufferCounter = 0f;

            if (transform.localScale.y < originalScaleY)
            {
                StopSlideAndReset();
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (slideCoroutine != null)
            {
                StopCoroutine(slideCoroutine);
            }
            slideCoroutine = StartCoroutine(Slide());
        }
    }
    void FixedUpdate()
    {
        float currentHalfHeight = transform.localScale.y / 2f;
        float dynamicGroundY = currentHalfHeight;

        Vector3 rayStart = new Vector3(_rb.position.x, _rb.position.y + 1f, _rb.position.z);
        RaycastHit hit;
        Vector3 moveDirection = Vector3.forward;

        if (Physics.Raycast(rayStart, Vector3.down, out hit, 10f, groundLayer, QueryTriggerInteraction.Ignore))
        {
            dynamicGroundY = hit.point.y + currentHalfHeight;
            moveDirection = Vector3.ProjectOnPlane(Vector3.forward, hit.normal).normalized;
        }

        if (isGrounded && jumpRequest)
        {
            verticalVelocity = jumpForce;
            isGrounded = false;
            jumpRequest = false;
        }

        float nextY = _rb.position.y;

        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.fixedDeltaTime;
            verticalVelocity = Mathf.Max(verticalVelocity, -50f);
            nextY += verticalVelocity * Time.fixedDeltaTime;

            if (verticalVelocity <= 0 && nextY <= dynamicGroundY)
            {
                nextY = dynamicGroundY;
                verticalVelocity = 0f;
                isGrounded = true;
            }
        }
        else
        {
            if (_rb.position.y - dynamicGroundY > 0.5f)
            {
                isGrounded = false;
            }
            else
            {
                verticalVelocity = 0f;
                nextY = dynamicGroundY;
            }
        }

        float targetX = 0f;
        if (desiredLane == 0) targetX = -laneDistance;
        else if (desiredLane == 1) targetX = 0;
        else if (desiredLane == 2) targetX = laneDistance;

        float newX = Mathf.Lerp(_rb.position.x, targetX + visualBump, Time.fixedDeltaTime * sideSpeed);

        float newZ = _rb.position.z + (moveDirection.z * forwardSpeed * Time.fixedDeltaTime);

        Vector3 newPosition = new Vector3(newX, nextY, newZ);
        _rb.MovePosition(newPosition);
    }

    IEnumerator Slide()
    {
        if (!isGrounded)
        {
            verticalVelocity = -jumpForce;
        }
        transform.localScale = new Vector3(transform.localScale.x, originalScaleY / 2f, transform.localScale.z);

        if (isGrounded)
        {
            _rb.position = new Vector3(_rb.position.x, _rb.position.y - (originalScaleY / 4f), _rb.position.z);
        }
     
        yield return new WaitForSeconds(1f);
        transform.localScale = new Vector3(transform.localScale.x, originalScaleY, transform.localScale.z);
        if (isGrounded)
        {
            _rb.position = new Vector3(_rb.position.x, _rb.position.y + (originalScaleY / 4f), _rb.position.z);
        }
    }
    IEnumerator BumpEffect(int direction)
    {
        float bumpDistance = 0.7f;
        float duration = 0.1f;

        visualBump = direction * bumpDistance;
        yield return new WaitForSeconds(duration);

        visualBump = 0f;
    }
    bool IsPathClear(int direction)
    {
        Vector3 rayDirection = (direction == 1) ? transform.right : -transform.right;

        float currentSurfaceY = 0f;
        RaycastHit downHit;
        Vector3 downRayStart = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);

        if (Physics.Raycast(downRayStart, Vector3.down, out downHit, 10f, groundLayer, QueryTriggerInteraction.Ignore))
        {
            currentSurfaceY = downHit.point.y;
        }
        Vector3 sideRayStart = new Vector3(transform.position.x, currentSurfaceY + 0.5f, transform.position.z);
        RaycastHit sideHit;
        if (Physics.Raycast(sideRayStart, rayDirection, out sideHit, laneDistance, groundLayer, QueryTriggerInteraction.Ignore))
        {
            return false;
        }
        return true;
    }
    void StopSlideAndReset()
    {
        if (slideCoroutine != null)
        {
            StopCoroutine(slideCoroutine);
            slideCoroutine = null;
        }

        if (isGrounded)
        {
            _rb.position = new Vector3(_rb.position.x, _rb.position.y + (originalScaleY / 4f), _rb.position.z);
        }
        transform.localScale = new Vector3(transform.localScale.x, originalScaleY, transform.localScale.z);
    }
}
