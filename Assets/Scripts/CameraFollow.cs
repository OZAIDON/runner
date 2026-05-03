using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [Header("Following Settings")]
    public Transform target;
    public Vector3 offset;
    public float smoothTime = 0.15f;

    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 shakeOffset = Vector3.zero;

    void LateUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
        transform.position = smoothPosition + shakeOffset;
    }
    public void TriggerShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }
    private IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            shakeOffset = new Vector3(x, y, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        shakeOffset = Vector3.zero;
    }
}
