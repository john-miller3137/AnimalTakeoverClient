using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AttacksController : MonoBehaviour
{
    [SerializeField] private GameObject Camera, WorldLight, FireworkParticlePrefab, testObject;
    private float worldLightIntensity_Default = 1.1f;
    private float worldLightIntensity_Decrease = 0.2f;
    private Light2D worldLight;
    


    private void Start()
    {
        if (WorldLight != null) worldLight = WorldLight.GetComponent<Light2D>();
        if(testObject != null)
        {

        }
    }

    private IEnumerator MoveToPosition(GameObject gameObject, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0;
        Vector3 startingPos = gameObject.transform.position;

        while (elapsedTime < duration)
        {
            gameObject.transform.position = Vector3.Lerp(startingPos, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.position = targetPosition;
    }

    public IEnumerator ScaleOverTime(GameObject gameObject, float finalScaleFactor, float duration)
    {
        float elapsedTime = 0f;
        Vector3 initialScale = gameObject.transform.localScale;
        Vector3 finalScale = Vector3.one * finalScaleFactor;
        Light2D worldLight = WorldLight.GetComponent<Light2D>();

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            worldLight.intensity = worldLightIntensity_Default - worldLightIntensity_Decrease * (t);
            gameObject.transform.localScale = Vector3.Lerp(initialScale, finalScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = finalScale;
        //yield return LaunchObject(targetPosition, launchSpeed);
    }

    public IEnumerator LaunchObject(GameObject gameObject, Vector3 targetPosition, float launchSpeed)
    {
        Vector3 direction = (targetPosition - gameObject.transform.position).normalized;
        while (Vector3.Distance(gameObject.transform.position, targetPosition) > 1f)
        {
            transform.position += direction * launchSpeed * Time.deltaTime;
            yield return null;
        }
        gameObject.transform.position = targetPosition;
        Camera.GetComponent<CameraShake>().Shake();
        //yield return this.GetComponent<ExplosionEffect>().ExplodeCoroutine();
    }

    public IEnumerator ExplodeCoroutine(GameObject gameObjectToDestroy, float explosionForce)
    {
        Vector3 explosionPosition = gameObjectToDestroy.transform.position;
        if(gameObjectToDestroy != null)
        {
            Destroy(gameObjectToDestroy);
        }
        for (int i = 0; i < 12; i++)
        {
            Vector3 direction = Random.insideUnitSphere;
            GameObject particle = Instantiate(FireworkParticlePrefab, explosionPosition, Quaternion.identity);
            Rigidbody2D rb2D = particle.GetComponent<Rigidbody2D>();
            rb2D.AddForce(direction * explosionForce, ForceMode2D.Impulse);
            Light2D light2D = particle.GetComponent<Light2D>();
            if (i < 6)
            {
                light2D.color = new Color(light2D.color.r, light2D.color.g, light2D.color.b, 1);
            }
            else
            {
                light2D.color = new Color(Color.magenta.r, Color.magenta.g, Color.magenta.b, Color.magenta.a);

            }
        }

        worldLight.intensity = worldLightIntensity_Default;
        yield return null;

    }

    public IEnumerator FlashRed(GameObject gameObject)
    {
        if (gameObject != null) {
            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            yield return new WaitForSeconds(1.5f);
            sr.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            sr.color = Color.white;
        }
        
    }

}
