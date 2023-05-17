using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AttacksController : MonoBehaviour
{
    private static AttacksController instance;
    public static AttacksController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AttacksController>();
                if (instance == null)
                {
                    GameObject go = new GameObject("AttacksController");
                    instance = go.AddComponent<AttacksController>();
                }
            }
            return instance;
        }
    }

    [SerializeField] private GameObject Camera, WorldLight, FireworkParticlePrefab, testObject, testTarget, a0, a1, a2, a3, a4, a5;
    [SerializeField] private GameObject FireAttackPrefab;
    private float worldLightIntensity_Default = 1f;
    private float worldLightIntensity_Decrease = 0.2f;
    private Light2D worldLight;
    private CameraShake _cameraShake;
    private SpriteRenderer sr0, sr1, sr2, sr3, sr4, sr5;

    private const float maxFireballScale = 0.6f;


    private void Start()
    {
        sr0 = a0.transform.GetChild(1).GetComponent<SpriteRenderer>();
        sr1 = a1.transform.GetChild(1).GetComponent<SpriteRenderer>();
        sr2 = a2.transform.GetChild(1).GetComponent<SpriteRenderer>();
        sr3 = a3.transform.GetChild(1).GetComponent<SpriteRenderer>();
        sr4 = a4.transform.GetChild(1).GetComponent<SpriteRenderer>();
        sr5 = a5.transform.GetChild(1).GetComponent<SpriteRenderer>();
        if (Camera != null) _cameraShake = Camera.GetComponent<CameraShake>();
        if (WorldLight != null) worldLight = WorldLight.GetComponent<Light2D>();
        
    }

    private IEnumerator MoveToPosition(GameObject animal, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0;
        Vector3 startingPos = animal.transform.position;

        while (elapsedTime < duration)
        {
            animal.transform.position = Vector3.Lerp(startingPos, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.position = targetPosition;
        _cameraShake.Shake();
        
        yield return null;
    }

    public IEnumerator ScaleOverTime(GameObject animal,float finalScaleFactor, float duration)
    {
        float elapsedTime = 0f;
        Vector3 initialScale = animal.transform.localScale;
        Vector3 finalScale = Vector3.one * finalScaleFactor;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            worldLight.intensity = worldLightIntensity_Default - worldLightIntensity_Decrease * (t);
            animal.transform.localScale = Vector3.Lerp(initialScale, finalScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = finalScale;
        yield return null;
        //yield return LaunchObject(targetPosition, launchSpeed);
        
    }

    public IEnumerator LaunchObject(GameObject animal, Vector3 targetPosition, float launchSpeed)
    {
        Vector3 direction = (targetPosition - animal.transform.position).normalized;
        while (Vector3.Distance(animal.transform.position, targetPosition) > 1f)
        {
            transform.position += direction * launchSpeed * Time.deltaTime;
            yield return null;
        }
        animal.transform.position = targetPosition;
        Camera.GetComponent<CameraShake>().Shake();
        yield return null;
        //yield return this.GetComponent<ExplosionEffect>().ExplodeCoroutine();
    }

    public IEnumerator ExplodeCoroutine(GameObject gameObjectToDestroy, float explosionForce)
    {
        Vector3 explosionPosition = gameObjectToDestroy.transform.position;
        Destroy(gameObjectToDestroy);
        
        for (int i = 0; i < 6; i++)
        {
            Vector3 direction = Random.insideUnitSphere;
            GameObject particle = Instantiate(FireworkParticlePrefab, explosionPosition, Quaternion.identity);
            Rigidbody2D rb2D = particle.GetComponent<Rigidbody2D>();
            rb2D.AddForce(direction * explosionForce, ForceMode2D.Impulse);
            Light2D light2D = particle.GetComponent<Light2D>();
            if (i < 3)
            {
                light2D.color = new Color(light2D.color.r, light2D.color.g, light2D.color.b, 1);
            }
            else
            {
                light2D.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 1);

            }
        }

        worldLight.intensity = worldLightIntensity_Default;
        yield return null;

    }

    public IEnumerator FlashRed(GameObject animal)
    {

        SpriteRenderer sr = null;
        if (animal == a0)
        {
            sr = sr0;
        } else if (animal == a1)
        {
            sr = sr1;
        }else if (animal == a2)
        {
            sr = sr2;
        }else if (animal == a3)
        {
            sr = sr3;
        }else if (animal == a4)
        {
            sr = sr4;
        }else if (animal == a5)
        {
            sr = sr5;
        }

        
        sr.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        sr.color = Color.white;


    }

    private IEnumerator DoFireAttack(GameObject animal, GameObject target)
    {
        Vector3 animalPos = animal.transform.position;
        Vector3 targetPos;
        if (target == null)
        {
            targetPos = new Vector3(animalPos.x, animalPos.y + 20, 0);
        }
        else
        {
            targetPos = target.transform.position;
        }
        
        Vector3 ballSpawnPos;
        if (animalPos.y < targetPos.y)
        {
            ballSpawnPos = new Vector3(animalPos.x, animalPos.y+1, 0);
        }
        else
        {
            ballSpawnPos = new Vector3(animalPos.x, animalPos.y-1, 0);
        }
        GameObject fireball = Instantiate(FireAttackPrefab, ballSpawnPos, Quaternion.identity);

        fireball.transform.localScale = new Vector3(0, 0, 0);
        yield return StartCoroutine(ScaleOverTime(fireball, maxFireballScale, 1f));
        yield return MoveToPosition(fireball, targetPos, .7f);
        yield return StartCoroutine(FlashRed(target));
        yield return StartCoroutine(ExplodeCoroutine(fireball, 12f));
        yield return null;
    }

    public void DoAttack(int cardId, int animalId, int targetId)
    {
        switch (cardId)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
                break;
            case 11:
                FireAttack(animalId, targetId);
                break;
            default:
                break;
        }
    }

    public void FireAttack(int animalId, int targetId)
    {
        if (animalId == targetId) return;
        GameObject animal, target;
        animal = GetAnimal(animalId);
        if (targetId == -1)
        {
            target = null;
        }
        else
        {
            target = GetAnimal(targetId);
        }
       
        
        StartCoroutine(DoFireAttack(animal, target));

        
    }

    private GameObject GetAnimal(int animalId)
    {
        switch (animalId)
        {
            case 0:
                if (GameLogic.Instance.IsPlayerOne)
                {
                    return a0;
                }
                else
                {
                    return a3;
                }
                
                
            case 1:
                if (GameLogic.Instance.IsPlayerOne)
                {
                    return a1;
                }
                else
                {
                    return a4;
                }
            case 2:
                if (GameLogic.Instance.IsPlayerOne)
                {
                    return a2;
                }
                else
                {
                    return a5;
                }
            case 3:
                if (GameLogic.Instance.IsPlayerOne)
                {
                    return a3;
                }
                else
                {
                    return a0;
                }
            case 4:
                if (GameLogic.Instance.IsPlayerOne)
                {
                    return a4;
                }
                else
                {
                    return a1;
                }
            case 5:
                if (GameLogic.Instance.IsPlayerOne)
                {
                    return a5;
                }
                else
                {
                    return a2;
                }
            default:
                return null;
        }
    }
}
