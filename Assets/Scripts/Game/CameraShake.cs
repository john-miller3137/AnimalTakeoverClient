using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition;

    public void Shake(float shakeDuration = 0.5f, float shakeMagnitude = 0.1f)
    {
        StartCoroutine(ShakeCoroutine(shakeDuration, shakeMagnitude));
    }

    private IEnumerator ShakeCoroutine(float shakeDuration = 0.5f, float shakeMagnitude = 0.1f)
    {
        originalPosition = transform.localPosition;

        float elapsedTime = 0.0f;

        while (elapsedTime < shakeDuration)
        {
            Vector3 randomPoint = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition = randomPoint;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}