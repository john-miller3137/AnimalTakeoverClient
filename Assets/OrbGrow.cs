using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OrbGrow : MonoBehaviour
{
    public float scaleFactor = 0.4f;   // The final scale factor to reach
    public float duration = 1f;        // The time it takes to reach the final scale
    public Vector3 targetPosition;
    public float launchSpeed;
    [SerializeField] private GameObject Camera, WorldLight;

    public IEnumerator ScaleOverTime()
    {
        float elapsedTime = 0f;
        Vector3 initialScale = transform.localScale;
        Vector3 finalScale = Vector3.one * scaleFactor;
        Light2D worldLight = WorldLight.GetComponent<Light2D>();

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            worldLight.intensity = 1.1f - 0.2f * (t);
            transform.localScale = Vector3.Lerp(initialScale, finalScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = finalScale;
        yield return LaunchObject(targetPosition, launchSpeed);
    }
    public IEnumerator LaunchObject(Vector3 targetPosition, float launchSpeed)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        while (Vector3.Distance(transform.position, targetPosition) > 1f)
        {
            transform.position += direction * launchSpeed * Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        Camera.GetComponent<CameraShake>().Shake();
        yield return this.GetComponent<ExplosionEffect>().ExplodeCoroutine();
    }
    public void Start()
    {
        StartCoroutine(ScaleOverTime());
    }
}

