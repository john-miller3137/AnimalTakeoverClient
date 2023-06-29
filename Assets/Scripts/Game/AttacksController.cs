using System.Collections;
using SharedLibrary;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.GraphicsBuffer;

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

    [SerializeField] private GameObject Camera, WorldLight, FireworkParticlePrefab, FireballParticlePrefab, testObject, testTarget, a0, a1, a2, a3, a4, a5;
    [SerializeField] private GameObject FireAttackPrefab, FireballHitPrefab, LifeForceAttackPrefab, LifeForceHitPrefab;
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
        //DoSpiritAttack(5, 0, SpiritType.LIFE);
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
        yield return null;
        //yield return this.GetComponent<ExplosionEffect>().ExplodeCoroutine();
    }

    public IEnumerator ExplodeCoroutine(GameObject gameObjectToDestroy, GameObject target, SpiritType spiritType)
    {
        GameObject hitParticles;
        Vector3 explosionPosition = target.transform.position;

        Destroy(gameObjectToDestroy);

        switch (spiritType)
        {
            case SpiritType.FIRE:
                hitParticles = Instantiate(FireballHitPrefab, explosionPosition, Quaternion.identity);
                Destroy(hitParticles, 1f);
                break;
            case SpiritType.WATER:
                hitParticles = Instantiate(FireballHitPrefab, explosionPosition, Quaternion.identity);
                Destroy(hitParticles, 1f);
                break;
            case SpiritType.LIFE:
                hitParticles = Instantiate(LifeForceHitPrefab, explosionPosition, Quaternion.identity);
                Destroy(hitParticles, 1f);
                break;
            case SpiritType.DECAY:
                hitParticles = Instantiate(FireballHitPrefab, explosionPosition, Quaternion.identity);
                Destroy(hitParticles, 1f);
                break;
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

        if (sr != null)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            sr.color = Color.white;
        }
        


    }
    private IEnumerator DoLifeAttack(GameObject animal, GameObject target)
    {
        Vector3 animalPos = animal.transform.position;
        byte direction = 0;
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
            direction = 0;
        }
        else
        {
            ballSpawnPos = new Vector3(animalPos.x, animalPos.y-1, 0);
            direction = 2;
        }
        GameObject fireball = Instantiate(LifeForceAttackPrefab, ballSpawnPos, Quaternion.identity);

        fireball.transform.localScale = new Vector3(0, 0, 0);
        yield return StartCoroutine(ScaleOverTime(fireball, maxFireballScale, 1f));
        StartCoroutine(MoveAnimalAttackSequence(animal, direction, .2f));
        yield return MoveToPosition(fireball, targetPos, .7f);
        yield return StartCoroutine(FlashRed(target));
        StartCoroutine(MoveAnimalAttackSequence(target, direction, .15f));
        _cameraShake.Shake();
        yield return StartCoroutine(ExplodeCoroutine(fireball, target, SpiritType.LIFE));
        yield return null;
    }
    private IEnumerator DoFireAttack(GameObject animal, GameObject target)
    {
        Vector3 animalPos = animal.transform.position;
        byte direction = 0;
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
            direction = 0;
        }
        else
        {
            ballSpawnPos = new Vector3(animalPos.x, animalPos.y-1, 0);
            direction = 2;
        }
        GameObject fireball = Instantiate(FireAttackPrefab, ballSpawnPos, Quaternion.identity);

        fireball.transform.localScale = new Vector3(0, 0, 0);
        yield return StartCoroutine(ScaleOverTime(fireball, maxFireballScale, 1f));
        StartCoroutine(MoveAnimalAttackSequence(animal, direction, .2f));
        yield return MoveToPosition(fireball, targetPos, .7f);
        yield return StartCoroutine(FlashRed(target));
        StartCoroutine(MoveAnimalAttackSequence(target, direction, .15f));
        _cameraShake.Shake();
        yield return StartCoroutine(ExplodeCoroutine(fireball, target, SpiritType.FIRE));
        yield return null;
    }
    private IEnumerator DetachBoardLight(GameObject animal, float timePerMove)
    {
        GameObject boardLight = animal.transform.GetChild(3).gameObject;
        boardLight.transform.SetParent(null);
        yield return new WaitForSeconds(timePerMove * 2);
        boardLight.transform.SetParent(animal.transform);
    }
    private IEnumerator MoveAnimalAttackSequence(GameObject animal, byte direction, float timePerMove)
    {
        float distanceToMove = 0.3f;
        Vector3 animalPos = animal.transform.position;
        
        

        switch (direction)
        {
            case 0:
                StartCoroutine(DetachBoardLight(animal, timePerMove));
                yield return MoveToPosition(animal, new Vector3(animalPos.x, animalPos.y + distanceToMove, 0), timePerMove);
                yield return MoveToPosition(animal, animalPos, timePerMove);
                break;
            case 1:
                StartCoroutine(DetachBoardLight(animal, timePerMove));
                yield return MoveToPosition(animal, new Vector3(animalPos.x + distanceToMove, animalPos.y, 0), timePerMove);
                yield return MoveToPosition(animal, animalPos, timePerMove);
                break;
            case 2:
                StartCoroutine(DetachBoardLight(animal, timePerMove));
                yield return MoveToPosition(animal, new Vector3(animalPos.x, animalPos.y - distanceToMove, 0), timePerMove);
                yield return MoveToPosition(animal, animalPos, timePerMove);
                break;
            case 3:
                StartCoroutine(DetachBoardLight(animal, timePerMove));
                yield return MoveToPosition(animal, new Vector3(animalPos.x - distanceToMove, animalPos.y, 0), timePerMove);
                yield return MoveToPosition(animal, animalPos, timePerMove);
                break;
        }
        
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
                DoSpiritAttack(animalId, targetId, SpiritType.FIRE);
                break;
            default:
                break;
        }
    }

    public void DoSpiritAttack(int animalId, int targetId, SpiritType spiritType)
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

        switch (spiritType)
        {
            case SpiritType.FIRE:
                StartCoroutine(DoFireAttack(animal, target));
                break;
            case SpiritType.WATER:
                break;
            case SpiritType.LIFE:
                StartCoroutine(DoLifeAttack(animal, target));
                break;
            case SpiritType.DECAY:
                break;
        }
        
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
