using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Network;
using Scripts.GameStructure;
using Server.Game;
using SharedLibrary;
using SharedLibrary.Library;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
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

    public GameObject FireAttackPrefab;
    [SerializeField] private GameObject Camera, WorldLight, FireworkParticlePrefab, FireballParticlePrefab, testObject, testTarget, a0, a1, a2, a3, a4, a5;
    [SerializeField] private GameObject  FireballHitPrefab, LifeForceAttackPrefab, LifeForceHitPrefab,
        WaterAttackPrefab, WaterHitPrefab, DecayAttackPrefab, DecayHitPrefab, HealthTextPrefab,
        PurpleHitPrefab, OrangeHitPrefab;
    private float worldLightIntensity_Default = .9f;
    private float worldLightIntensity_Decrease = 0.2f;
    public Light2D worldLight;
    public CameraShake _cameraShake;
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

    public IEnumerator FlashLightUp(float increase)
    {
        float intensity = worldLight.intensity;
        while (worldLight.intensity < intensity + increase)
        {
            worldLight.intensity += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
    public IEnumerator FlashLightDown(float decrease)
    {
        float intensity = worldLight.intensity;
        while (worldLight.intensity > intensity - decrease)
        {
            worldLight.intensity -= Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    public IEnumerator MoveToPosition(GameObject animal, Vector3 targetPosition, float duration, bool isFireball)
    {
        if (animal == null) yield break;
        float elapsedTime = 0;
        Vector3 startingPos = animal.transform.position;

        bool layerUpdated = false;
        while (elapsedTime < duration)
        {
            if (!layerUpdated && isFireball && elapsedTime > .1f)
            {
                layerUpdated = true;
                UpdateLayer(animal);
            }
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
            if(animal == null) yield break;
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

    public IEnumerator ResetLights()
    {
        worldLight.intensity = worldLightIntensity_Default;
        yield return null;
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
                hitParticles = Instantiate(WaterHitPrefab, explosionPosition, Quaternion.identity);
                Destroy(hitParticles, 1f);
                break;
            case SpiritType.LIFE:
                hitParticles = Instantiate(LifeForceHitPrefab, explosionPosition, Quaternion.identity);
                Destroy(hitParticles, 1f);
                break;
            case SpiritType.DECAY:
                hitParticles = Instantiate(DecayHitPrefab, explosionPosition, Quaternion.identity);
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
    private IEnumerator DoWaterAttack(GameObject animal, GameObject target, int damage, bool isDead, float healthRatio, 
        int targetId, int addedXp, int animalId, float xpRatio)
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
        GameObject fireball = Instantiate(WaterAttackPrefab, ballSpawnPos, Quaternion.identity);
        UpdateLayer(fireball);
        fireball.transform.localScale = new Vector3(0, 0, 0);
        GameController.Instance.PlayFiringUpSound();
        yield return StartCoroutine(ScaleOverTime(fireball, maxFireballScale, 1f));
        StartCoroutine(MoveAnimalAttackSequence(animal, direction, .2f));
        GameController.Instance.PlayLaunchSound();
        yield return MoveToPosition(fireball, targetPos, .1f*Vector3.Distance(animalPos, targetPos), true);
        _cameraShake.Shake();
        StartCoroutine(SpawnHealthText((int) targetPos.x, (int)targetPos.y, damage, Color.red));
        StartCoroutine(updateXp((int) animalPos.x, (int)animalPos.y, addedXp, Color.blue, animalId, xpRatio));
        if (GameLogic.Instance.IsPlayerOne && targetId < 3)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        } else if (!GameLogic.Instance.IsPlayerOne && targetId >= 3 && targetId < 6)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        }
        if (isDead)
        {
            StartCoroutine(HealthController.Instance.OnDeadAnimal(target, targetId));
        }
        else
        {
            StartCoroutine(FlashRed(target));
        }
        StartCoroutine(ExplodeCoroutine(fireball, target, SpiritType.WATER));
        GameController.Instance.PlayExplosionSound();
        yield return MoveAnimalAttackSequence(target, direction, .15f);
        yield return null;
    }
    private IEnumerator DoDecayAttack(GameObject animal, GameObject target, 
        int damage, bool isDead, float healthRatio, int targetId, int addedXp, int animalId, float xpRatio)
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
        GameObject fireball = Instantiate(DecayAttackPrefab, ballSpawnPos, Quaternion.identity);
        UpdateLayer(fireball);
        fireball.transform.localScale = new Vector3(0, 0, 0);
        GameController.Instance.PlayFiringUpSound();
        yield return StartCoroutine(ScaleOverTime(fireball, maxFireballScale, 1f));
        
        StartCoroutine(MoveAnimalAttackSequence(animal, direction, .2f));
        GameController.Instance.PlayLaunchSound();
        yield return MoveToPosition(fireball, targetPos, .1f*Vector3.Distance(animalPos, targetPos), true);
        _cameraShake.Shake();
        StartCoroutine(SpawnHealthText((int) targetPos.x, (int)targetPos.y, damage, Color.red));
        StartCoroutine(updateXp((int) animalPos.x, (int)animalPos.y, addedXp, Color.blue, animalId, xpRatio));
        if (GameLogic.Instance.IsPlayerOne && targetId < 3)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        } else if (!GameLogic.Instance.IsPlayerOne && targetId >= 3 && targetId < 6)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        }
        if (isDead)
        {
            StartCoroutine(HealthController.Instance.OnDeadAnimal(target, targetId));
        }
        else
        {
            StartCoroutine(FlashRed(target));
        }
        StartCoroutine(ExplodeCoroutine(fireball, target, SpiritType.DECAY));
        GameController.Instance.PlayExplosionSound();
        yield return MoveAnimalAttackSequence(target, direction, .15f);
        yield return null;
    }
    private IEnumerator DoLifeAttack(GameObject animal, GameObject target, int damage, bool isDead, float healthRatio, int targetId,
        int addedXp, int animalId, float xpRatio)
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
        UpdateLayer(fireball);
        GameController.Instance.PlayFiringUpSound();
        fireball.transform.localScale = new Vector3(0, 0, 0);
        yield return StartCoroutine(ScaleOverTime(fireball, maxFireballScale, 1f));
        StartCoroutine(MoveAnimalAttackSequence(animal, direction, .2f));
        GameController.Instance.PlayLaunchSound();
        yield return MoveToPosition(fireball, targetPos, .1f*Vector3.Distance(animalPos, targetPos), true);
        _cameraShake.Shake();
        StartCoroutine(SpawnHealthText((int) targetPos.x, (int)targetPos.y, damage, Color.red));
        StartCoroutine(updateXp((int) animalPos.x, (int)animalPos.y, addedXp, Color.blue, animalId, xpRatio));
        
        if (GameLogic.Instance.IsPlayerOne && targetId < 3)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        } else if (!GameLogic.Instance.IsPlayerOne && targetId >= 3 && targetId < 6)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        }
        if (isDead)
        {
            StartCoroutine(HealthController.Instance.OnDeadAnimal(target, targetId));
        }
        else
        {
            StartCoroutine(FlashRed(target));
        }
        StartCoroutine(ExplodeCoroutine(fireball, target, SpiritType.LIFE));
        GameController.Instance.PlayExplosionSound();
        yield return MoveAnimalAttackSequence(target, direction, .15f);
        yield return null;
    }
    private IEnumerator DoFireAttack(GameObject animal, GameObject target, int damage, bool isDead, float healthRatio, 
        int targetId, int addedXp, int animalId, float xpRatio)
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
        UpdateLayer(fireball);
        GameController.Instance.PlayFiringUpSound();
        fireball.transform.localScale = new Vector3(0, 0, 0);
        yield return StartCoroutine(ScaleOverTime(fireball, maxFireballScale, 1f));
        StartCoroutine(MoveAnimalAttackSequence(animal, direction, .2f));
        GameController.Instance.PlayLaunchSound();
        yield return MoveToPosition(fireball, targetPos, .1f*Vector3.Distance(animalPos, targetPos), true);
        _cameraShake.Shake();
        StartCoroutine(SpawnHealthText((int) targetPos.x, (int)targetPos.y, damage, Color.red));
        StartCoroutine(updateXp((int) animalPos.x, (int)animalPos.y, addedXp, Color.blue, animalId, xpRatio));
        
        if (GameLogic.Instance.IsPlayerOne && targetId < 3)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        } else if (!GameLogic.Instance.IsPlayerOne && targetId >= 3 && targetId < 6)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        }
        if (isDead)
        {
            StartCoroutine(HealthController.Instance.OnDeadAnimal(target,targetId));
        }
        else
        {
            StartCoroutine(FlashRed(target));
        }
        StartCoroutine(ExplodeCoroutine(fireball, target, SpiritType.FIRE));
        GameController.Instance.PlayExplosionSound();
        yield return MoveAnimalAttackSequence(target, direction, .15f);
        yield return null;
    }
    private IEnumerator DetachBoardLight(GameObject animal, float timePerMove)
    {
        if (animal == null) yield break;
        GameObject boardLight = animal.transform.GetChild(3).gameObject;
        boardLight.transform.SetParent(null);
        yield return new WaitForSeconds(timePerMove * 2);
        boardLight.transform.SetParent(animal.transform);
        Vector3 animalPos = animal.transform.position;
        boardLight.transform.position = new Vector3(animalPos.x, animalPos.y-0.2f, 0);
    }
    
    public IEnumerator MoveAnimalAttackSequence(GameObject animal, byte direction, float timePerMove)
    {
        float distanceToMove = 0.3f;
        Vector3 animalPos = animal.transform.position;
        
        

        switch (direction)
        {
            case 0:
                StartCoroutine(DetachBoardLight(animal, timePerMove));
                yield return MoveToPosition(animal, new Vector3(animalPos.x, animalPos.y + distanceToMove, 0), timePerMove, false);
                yield return MoveToPosition(animal, animalPos, timePerMove, false);
                break;
            case 1:
                StartCoroutine(DetachBoardLight(animal, timePerMove));
                yield return MoveToPosition(animal, new Vector3(animalPos.x + distanceToMove, animalPos.y, 0), timePerMove, false);
                yield return MoveToPosition(animal, animalPos, timePerMove, false);
                break;
            case 2:
                StartCoroutine(DetachBoardLight(animal, timePerMove));
                yield return MoveToPosition(animal, new Vector3(animalPos.x, animalPos.y - distanceToMove, 0), timePerMove, false);
                yield return MoveToPosition(animal, animalPos, timePerMove, false);
                break;
            case 3:
                StartCoroutine(DetachBoardLight(animal, timePerMove));
                yield return MoveToPosition(animal, new Vector3(animalPos.x - distanceToMove, animalPos.y, 0), timePerMove, false);
                yield return MoveToPosition(animal, animalPos, timePerMove, false);
                break;
        }

        yield return null;
    }
/*
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
                DoSpiritAttack(animalId, targetId, SpiritType.FIRE, 0);
                break;
            default:
                break;
        }
    }

    */

    public IEnumerator SpiritAttackRoutine(GameEventParams gameEventParams)
    {
        int animalId = gameEventParams.animalId;
        int targetId = gameEventParams.targetId;
        SpiritType spiritType = gameEventParams.spiritType;
        int damage = gameEventParams.damage;
        bool isKOed = gameEventParams.isKOed;
        bool spawnNewCrystal = gameEventParams.spawnNewCrystal;
        byte newX = gameEventParams.newX;
        byte newY = gameEventParams.newY;
        byte crystalKey = gameEventParams.crystalKey;
        float healthRatio = gameEventParams.healthRatio;
        int addedXp = gameEventParams.addedXp;
        float xpRatio = gameEventParams.xpRatio;
        bool isDead1 = gameEventParams.isDead1;
        bool isDead2 = gameEventParams.isDead2;
        float healthRatio1 = gameEventParams.healthRatio1;
        float healthRatio2 = gameEventParams.healthRatio2;
        int addedHealth = gameEventParams.addedHealth;
        int playerNum = gameEventParams.playerNum;
        AttackModifiers[] modifiers = gameEventParams.attackModifier;
        int tX = gameEventParams.tX;
        int tY = gameEventParams.tY;
        yield return DoSpiritAttack(animalId, targetId, spiritType, damage, isKOed, spawnNewCrystal, newX, newY,
            crystalKey, healthRatio, modifiers, addedXp, xpRatio, isDead1, isDead2, healthRatio1, healthRatio2, addedHealth,
            tX, tY, playerNum);
    }

    public IEnumerator DoSpiritAttack(int animalId, int targetId, SpiritType spiritType, int damage, bool isDead, bool spawnNewCrystal,
        byte newX, byte newY, byte crystalKey, float healthRatio, AttackModifiers[] modifiers, int addedXp, float xpRatio,
        bool isDead1, bool isDead2, float healthRatio1, float healthRatio2, int addedHealth, int tX, int tY, int playerNum)
    {
        if (animalId == targetId) yield break;
        if(playerNum == GameLogic.Instance.playerNum)
        {
            StartCoroutine(GameController.Instance.SpawnPerfect());
        }

        //Debug.Log("modifiers: " + modifiers);
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
        StartCoroutine(DestroyOrbs(animalId));
        foreach (var modifier in modifiers)
        {
            switch (modifier)
            {
                case AttackModifiers.RHINO_PASSIVE:
                    StartCoroutine(AnimalEffectController.Instance.DoRhinoPassive(animalId));
                    break;
                case AttackModifiers.SHIELD:
                    StartCoroutine(PlantController.Instance.DoShieldEffect());
                    break;
                case AttackModifiers.BEAR_DROPBEAR:
                    if (GameLogic.Instance.IsPlayerOne)
                    {
                        StartCoroutine(AnimalEffectController.Instance.BearDropTeddy(target, targetId, tX + Constants.hor_offset, 
                            tY+Constants.vert_offset));
                    }
                    else
                    {
                        StartCoroutine(AnimalEffectController.Instance.BearDropTeddy(target, targetId, GameLogic.flipX(tX) + Constants.hor_offset, 
                            GameLogic.flipY(tY) +Constants.vert_offset));
                    }
                    
                    break;
                case AttackModifiers.POPPY:
                    StartCoroutine(PlantController.Instance.DoPoppyEffect());
                    break;
                case AttackModifiers.ELEPHANT_HEAL:
                    StartCoroutine(AnimalEffectController.Instance.DoElephantEffect(animalId, addedHealth));
                    break;
                case AttackModifiers.HIPPO_DIRT:
                    StartCoroutine(AnimalEffectController.Instance.DoHippoEffect(animalId));
                    break;
                case AttackModifiers.DRAGON_FIRE:
                    StartCoroutine(AnimalEffectController.Instance.DoDragonEffect(animalId));
                    break;
                case AttackModifiers.BEAR_SPECIAL:
                    StartCoroutine(AnimalEffectController.Instance.DoBearSpecial(animalId));
                    break;
                case AttackModifiers.FROG_SPECIAL:
                    yield return StartCoroutine(AnimalEffectController.Instance.DoTadpoleFrogSpecial(animal, target, damage, 
                        isDead, healthRatio, targetId, addedXp, animalId, xpRatio));
                    if (spawnNewCrystal && GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.WATER, crystalKey, newX +Constants.hor_offset
                            , newY + Constants.vert_offset);
                    } else if (spawnNewCrystal && !GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.WATER, crystalKey, GameLogic.flipX(newX) +Constants.hor_offset
                            , GameLogic.flipY(newY) + Constants.vert_offset);
                    }
                    yield break;
                case AttackModifiers.CHICKEN_SPECIAL:
                    yield return StartCoroutine(AnimalEffectController.Instance.DoChickenSpecial(AnimalEffectController.Instance.eggs,
                        animal, target, damage, 
                        isDead, healthRatio, targetId, addedXp, animalId, xpRatio));
                    if (spawnNewCrystal && GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.FIRE, crystalKey, newX +Constants.hor_offset
                            , newY + Constants.vert_offset);
                    } else if (spawnNewCrystal && !GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.FIRE, crystalKey, GameLogic.flipX(newX) +Constants.hor_offset
                            , GameLogic.flipY(newY) + Constants.vert_offset);
                    }
                    yield break;
                case AttackModifiers.SEAL_SPECIAL:
                    yield return StartCoroutine(DoSealSpecial(AnimalEffectController.Instance.shellList,
                        animal, target, damage, 
                        isDead, healthRatio, targetId, addedXp, animalId, xpRatio));
                    if (spawnNewCrystal && GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.WATER, crystalKey, newX +Constants.hor_offset
                            , newY + Constants.vert_offset);
                    } else if (spawnNewCrystal && !GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.WATER, crystalKey, GameLogic.flipX(newX) +Constants.hor_offset
                            , GameLogic.flipY(newY) + Constants.vert_offset);
                    }
                    yield break;
                    break;
                case AttackModifiers.DRAGON_SPECIAL:
                    yield return StartCoroutine(DoDragonSpecial(
                        animal, target, damage, 
                        isDead, healthRatio, targetId, addedXp, animalId, xpRatio));
                    if (spawnNewCrystal && GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.FIRE, crystalKey, newX +Constants.hor_offset
                            , newY + Constants.vert_offset);
                    } else if (spawnNewCrystal && !GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.FIRE, crystalKey, GameLogic.flipX(newX) +Constants.hor_offset
                            , GameLogic.flipY(newY) + Constants.vert_offset);
                    }
                    yield break;
                    break;
                case AttackModifiers.GOAT_SPECIAL:
                    yield return StartCoroutine(DoGoatSpecial(
                        animal, target, damage, 
                        isDead, healthRatio, targetId, addedXp, animalId, xpRatio));
                    if (spawnNewCrystal && GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.LIFE, crystalKey, newX +Constants.hor_offset
                            , newY + Constants.vert_offset);
                    } else if (spawnNewCrystal && !GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.LIFE, crystalKey, GameLogic.flipX(newX) +Constants.hor_offset
                            , GameLogic.flipY(newY) + Constants.vert_offset);
                    }
                    yield break;
                    break;
                case AttackModifiers.BUNNY_SPECIAL:
                    yield return StartCoroutine(AnimalEffectController.Instance.DoBunnyEggSpecial(AnimalEffectController.Instance.bunnyEggs,
                        animal, target, damage, 
                        isDead, healthRatio, targetId, addedXp, animalId, xpRatio));
                    if (spawnNewCrystal && GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.LIFE, crystalKey, newX +Constants.hor_offset
                            , newY + Constants.vert_offset);
                    } else if (spawnNewCrystal && !GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.LIFE, crystalKey, GameLogic.flipX(newX) +Constants.hor_offset
                            , GameLogic.flipY(newY) + Constants.vert_offset);
                    }
                    yield break;
                case AttackModifiers.HUSKY_SPECIAL:
                    yield return StartCoroutine(DoHuskySpecial(
                        animal, target, damage, 
                        isDead, healthRatio, targetId, addedXp, animalId, xpRatio));
                    if (spawnNewCrystal && GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.LIFE, crystalKey, newX +Constants.hor_offset
                            , newY + Constants.vert_offset);
                    } else if (spawnNewCrystal && !GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.LIFE, crystalKey, GameLogic.flipX(newX) +Constants.hor_offset
                            , GameLogic.flipY(newY) + Constants.vert_offset);
                    }
                    yield break;
                case AttackModifiers.ELEPHANT_SPECIAL:
                    yield return StartCoroutine(DoElephantSingleSpecial(
                        animal, target, damage, 
                        isDead, healthRatio, targetId, addedXp, animalId, xpRatio));
                    if (spawnNewCrystal && GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.DECAY, crystalKey, newX +Constants.hor_offset
                            , newY + Constants.vert_offset);
                    } else if (spawnNewCrystal && !GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.DECAY, crystalKey, GameLogic.flipX(newX) +Constants.hor_offset
                            , GameLogic.flipY(newY) + Constants.vert_offset);
                    }
                    yield break;
                case AttackModifiers.ELEPHANT_ALL_SPECIAL:
                    yield return StartCoroutine(DoElephantAllSpecial(
                        animal, target, damage, 
                        isDead, healthRatio, targetId, addedXp, animalId, xpRatio, healthRatio1, healthRatio2, isDead1, isDead2));
                    if (spawnNewCrystal && GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.DECAY, crystalKey, newX +Constants.hor_offset
                            , newY + Constants.vert_offset);
                    } else if (spawnNewCrystal && !GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.DECAY, crystalKey, GameLogic.flipX(newX) +Constants.hor_offset
                            , GameLogic.flipY(newY) + Constants.vert_offset);
                    }
                    yield break;
                case AttackModifiers.CAT_SPECIAL:
                    yield return StartCoroutine(DoCatSpecial(
                        animal, target, damage, 
                        isDead, healthRatio, targetId, addedXp, animalId, xpRatio));
                    if (spawnNewCrystal && GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.FIRE, crystalKey, newX +Constants.hor_offset
                            , newY + Constants.vert_offset);
                    } else if (spawnNewCrystal && !GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.FIRE, crystalKey, GameLogic.flipX(newX) +Constants.hor_offset
                            , GameLogic.flipY(newY) + Constants.vert_offset);
                    }
                    yield break;
                case AttackModifiers.DEER_SPECIAL:
                    yield return StartCoroutine(DoDeerSpecial(
                        animal, target, damage, 
                        isDead, healthRatio, targetId, addedXp, animalId, xpRatio, addedHealth));
                    if (spawnNewCrystal && GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.LIFE, crystalKey, newX +Constants.hor_offset
                            , newY + Constants.vert_offset);
                    } else if (spawnNewCrystal && !GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.LIFE, crystalKey, GameLogic.flipX(newX) +Constants.hor_offset
                            , GameLogic.flipY(newY) + Constants.vert_offset);
                    }
                    yield break;
                case AttackModifiers.DINO_SPECIAL:
                    yield return StartCoroutine(DoDinoSpecial(
                        animal, target, damage, 
                        isDead, healthRatio, targetId, addedXp, animalId, xpRatio));
                    if (spawnNewCrystal && GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.FIRE, crystalKey, newX +Constants.hor_offset
                            , newY + Constants.vert_offset);
                    } else if (spawnNewCrystal && !GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.FIRE, crystalKey, GameLogic.flipX(newX) +Constants.hor_offset
                            , GameLogic.flipY(newY) + Constants.vert_offset);
                    }
                    yield break;
                case AttackModifiers.OWL_SPECIAL:
                    yield return StartCoroutine(DoOwlSpecial(
                        animal, target, damage, 
                        isDead, healthRatio, targetId, addedXp, animalId, xpRatio));
                    if (spawnNewCrystal && GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.DECAY, crystalKey, newX +Constants.hor_offset
                            , newY + Constants.vert_offset);
                    } else if (spawnNewCrystal && !GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.DECAY, crystalKey, GameLogic.flipX(newX) +Constants.hor_offset
                            , GameLogic.flipY(newY) + Constants.vert_offset);
                    }
                    yield break;
                case AttackModifiers.RHINO_SPECIAL:
                    yield return StartCoroutine(DoRhinoSpecial(
                        animal, target, damage, 
                        isDead, healthRatio, targetId, addedXp, animalId, xpRatio));
                    if (spawnNewCrystal && GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.FIRE, crystalKey, newX +Constants.hor_offset
                            , newY + Constants.vert_offset);
                    } else if (spawnNewCrystal && !GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.FIRE, crystalKey, GameLogic.flipX(newX) +Constants.hor_offset
                            , GameLogic.flipY(newY) + Constants.vert_offset);
                    }
                    yield break;
                case AttackModifiers.WHALE_SPECIAL:
                    yield return StartCoroutine(DoWhaleSpecial(
                        animal, target, damage, 
                        isDead, healthRatio, targetId, addedXp, animalId, xpRatio));
                    if (spawnNewCrystal && GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.WATER, crystalKey, newX +Constants.hor_offset
                            , newY + Constants.vert_offset);
                    } else if (spawnNewCrystal && !GameLogic.Instance.IsPlayerOne)
                    {
                        CrystalController.Instance.SpawnCrystal((byte)SpiritType.WATER, crystalKey, GameLogic.flipX(newX) +Constants.hor_offset
                            , GameLogic.flipY(newY) + Constants.vert_offset);
                    }
                    yield break;

            }
        }
        
        
        switch (spiritType)
        {
            case SpiritType.FIRE:
                yield return DoFireAttack(animal, target, damage, isDead, healthRatio, targetId, addedXp, animalId, xpRatio);
                if (spawnNewCrystal && GameLogic.Instance.IsPlayerOne)
                {
                    CrystalController.Instance.SpawnCrystal((byte)SpiritType.FIRE, crystalKey, newX +Constants.hor_offset
                        , newY + Constants.vert_offset);
                } else if (spawnNewCrystal && !GameLogic.Instance.IsPlayerOne)
                {
                    CrystalController.Instance.SpawnCrystal((byte)SpiritType.FIRE, crystalKey, GameLogic.flipX(newX) +Constants.hor_offset
                        , GameLogic.flipY(newY) + Constants.vert_offset);
                }
                break;
            case SpiritType.WATER:
                yield return DoWaterAttack(animal, target, damage, isDead, healthRatio, targetId, addedXp, animalId, xpRatio);
                if (spawnNewCrystal && GameLogic.Instance.IsPlayerOne)
                {
                    CrystalController.Instance.SpawnCrystal((byte)SpiritType.WATER, crystalKey, newX +Constants.hor_offset
                        , newY + Constants.vert_offset);
                } else if (spawnNewCrystal && !GameLogic.Instance.IsPlayerOne)
                {
                    CrystalController.Instance.SpawnCrystal((byte)SpiritType.WATER, crystalKey, GameLogic.flipX(newX) +Constants.hor_offset
                        , GameLogic.flipY(newY) + Constants.vert_offset);
                }
                break;
            case SpiritType.LIFE:
                yield return (DoLifeAttack(animal, target, damage, isDead, healthRatio, targetId, addedXp, animalId, xpRatio));
                if (spawnNewCrystal && GameLogic.Instance.IsPlayerOne)
                {
                    CrystalController.Instance.SpawnCrystal((byte)SpiritType.LIFE, crystalKey, newX +Constants.hor_offset
                        , newY + Constants.vert_offset);
                } else if (spawnNewCrystal && !GameLogic.Instance.IsPlayerOne)
                {
                    CrystalController.Instance.SpawnCrystal((byte)SpiritType.LIFE, crystalKey, GameLogic.flipX(newX) +Constants.hor_offset
                        , GameLogic.flipY(newY) + Constants.vert_offset);
                }
                break;
            case SpiritType.DECAY:
                yield return (DoDecayAttack(animal, target, damage, isDead, healthRatio, targetId, addedXp, animalId, xpRatio));
                if (spawnNewCrystal && GameLogic.Instance.IsPlayerOne)
                {
                    CrystalController.Instance.SpawnCrystal((byte)SpiritType.DECAY, crystalKey, newX +Constants.hor_offset
                        , newY + Constants.vert_offset);
                } else if (spawnNewCrystal && !GameLogic.Instance.IsPlayerOne)
                {
                    CrystalController.Instance.SpawnCrystal((byte)SpiritType.DECAY, crystalKey, GameLogic.flipX(newX) +Constants.hor_offset
                        , GameLogic.flipY(newY) + Constants.vert_offset);
                }
                break;
        }

        yield return null;
    }

    private IEnumerator DestroyOrbs(int animalId)
    {
        GameObject animal = GetAnimal(animalId);
        int childCount = animal.transform.GetChild(1).childCount;
        List<GameObject> objectsToDestroy = new List<GameObject>();
        for (int i = 0; i < childCount; i++)
        {
            objectsToDestroy.Add(animal.transform.GetChild(1).GetChild(i).gameObject);
        }
        foreach(GameObject go in objectsToDestroy)
        {
            go.transform.SetParent(null);
            Destroy(go);
        }

        yield return null;
    }

    private IEnumerator DestroyOrb(int animalId)
    {
        GameObject animal = GetAnimal(animalId);
        int childCount = animal.transform.GetChild(1).childCount;
        List<GameObject> objectsToDestroy = new List<GameObject>();
        if(childCount > 1) 
        {
            objectsToDestroy.Add(animal.transform.GetChild(1).GetChild(0).gameObject);
        }
        foreach(GameObject go in objectsToDestroy)
        {
            go.transform.SetParent(null);
            Destroy(go);
        }

        yield return null;
    }

    //update GetAnimal to be dynamic. If IsPlayerOne, return myAnimal[animalId] if animalId<numberOfAnimals/2, else return enemyAnimal[animalId-numberofAnimals/2]
    
    public GameObject GetAnimal(int animalId)
    {
        if (GameLogic.Instance.IsPlayerOne)
        {
            if(animalId < GameLogic.Instance.numberOfAnimals/2)
            {
                return GameLogic.Instance.myAnimals[animalId];
            }
            else
            {
                return GameLogic.Instance.enemyAnimals[animalId - GameLogic.Instance.numberOfAnimals/2];
            }
        }
        else
        {
            if(animalId < GameLogic.Instance.numberOfAnimals/2)
            {
                return GameLogic.Instance.enemyAnimals[animalId];
            }
            else
            {
                return GameLogic.Instance.myAnimals[animalId - GameLogic.Instance.numberOfAnimals/2];
            }
        }
        /*
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
        }*/
    }

    public IEnumerator SpawnHealthText(int x, int y, int damage, Color color)
    {
        GameObject text = Instantiate(HealthTextPrefab, new Vector3(x, y + 1, 0), Quaternion.identity);
        TextMeshProUGUI textMeshProUGUI = text.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.text = damage.ToString();
        textMeshProUGUI.color = new Color(color.r, color.g, color.b, color.a);
        textMeshProUGUI.faceColor = new Color(color.r, color.g, color.b, color.a);
        
        Destroy(text, 2);
        yield return null;
    }
    
    public IEnumerator SpawnAddedMove(int x, int y, Color color)
    {
        GameObject text = Instantiate(HealthTextPrefab, new Vector3(x, y + .8f, 0), Quaternion.identity);
        text.transform.localScale = new Vector3(.3f, .3f, .3f);
        TextMeshProUGUI textMeshProUGUI = text.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.text = "+1 move";
        textMeshProUGUI.color = color;
        textMeshProUGUI.faceColor = color;
        
        Destroy(text, 2);
        yield return null;
    }
    private IEnumerator SpawnXpText(int x, int y, int xp, Color color)
    {
        if (xp <= 0) yield break;
        GameObject text = Instantiate(HealthTextPrefab, new Vector3(x, y + .5f, 0), Quaternion.identity);
        text.transform.localScale = new Vector3(.3f, .3f, .3f);
        TextMeshProUGUI textMeshProUGUI = text.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.text = xp.ToString();
        textMeshProUGUI.color = new Color(color.r, color.g, color.b, color.a);
        textMeshProUGUI.faceColor = new Color(color.r, color.g, color.b, color.a);
        
        Destroy(text, 2);
        yield return null;
    }

    public IEnumerator updateXp(int x, int y, int xp, Color color, int animalId, float xpRatio)
    {
        StartCoroutine(SpawnXpText(x, y, xp, Color.blue));
        HealthController.Instance.UpdateAnimalXp(animalId, xpRatio);
        yield return null;
    }

    public void UpdateLayer(GameObject fireball)
    {
        if (fireball == null) return;
        ParticleSystem[] srs = fireball.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem sr in srs)
        {
            sr.GetComponent<Renderer>().sortingLayerName = "PlantEffectsOverlay";
        }
    }
    
    
    
    public IEnumerator DoBigWaterAttack(GameObject animal, GameObject target, int damage, bool isDead, float healthRatio, 
        int targetId, int addedXp, int animalId, float xpRatio, int tCount, List<GameObject> tadpoles)
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
        GameObject fireball = Instantiate(WaterAttackPrefab, ballSpawnPos, Quaternion.identity);
        UpdateLayer(fireball);
        GameObject purp = Instantiate(PurpleHitPrefab, fireball.transform);
        purp.transform.position = new Vector3(0, 0, 0);
        fireball.transform.localScale = new Vector3(0, 0, 0);
        purp.transform.localPosition = new Vector3(0, 0, 0);
        GameController.Instance.PlayFiringUpSound();
        StartCoroutine(AnimalEffectController.Instance.spinTadpoles(tadpoles, animalPos, tCount));
        StartCoroutine(ScaleOverTime(purp, 2, 1.5f));
        yield return StartCoroutine(ScaleOverTime(fireball, 1, 1.5f));
        StartCoroutine(MoveAnimalAttackSequence(animal, direction, .2f));
        GameController.Instance.StopFiringUpSound();
        GameController.Instance.PlayLaunchSound();
        
        StartCoroutine(AnimalEffectController.Instance.launchTadpoles(tadpoles, tCount, animalPos));
        yield return MoveToPosition(fireball, targetPos, .1f*Vector3.Distance(animalPos, targetPos), false);
        _cameraShake.Shake();
        StartCoroutine(AnimalEffectController.Instance.destroyTadpoles(tCount, animalPos));
        StartCoroutine(SpawnHealthText((int) targetPos.x, (int)targetPos.y, damage, Color.red));
        StartCoroutine(updateXp((int) animalPos.x, (int)animalPos.y, addedXp, Color.blue, animalId, xpRatio));
        if (GameLogic.Instance.IsPlayerOne && targetId < 3)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        } else if (!GameLogic.Instance.IsPlayerOne && targetId >= 3 && targetId < 6)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        }
        if (isDead)
        {
            StartCoroutine(HealthController.Instance.OnDeadAnimal(target, targetId));
        }
        else
        {
            StartCoroutine(FlashRed(target));
        }
        StartCoroutine(ExplodeCoroutine(fireball, target, SpiritType.WATER));
        GameController.Instance.PlayExplosionSound();
        Destroy(purp);
        yield return MoveAnimalAttackSequence(target, direction, .15f);
        yield return null;
    }

    public IEnumerator DoBunnyEggSpecial(GameObject animal, GameObject target, int damage, bool isDead,
        float healthRatio,
        int targetId, int addedXp, int animalId, float xpRatio, int tCount, List<GameObject> eggs)
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
            ballSpawnPos = new Vector3(animalPos.x, animalPos.y + 1, 0);
            direction = 0;
        }
        else
        {
            ballSpawnPos = new Vector3(animalPos.x, animalPos.y - 1, 0);
            direction = 2;
        }

        GameController.Instance.PlayFiringUpSound();
        bool oneTime = false;
        AnimalEffectController.Instance.spinBunnyEggs = true;
        bool twoTime = false;
        for (int i = 0; i < tCount; i++)
        {
            GameObject egg = eggs[i];
            GameObject fireball = Instantiate(FireAttackPrefab, egg.transform);
            fireball.transform.position = egg.transform.position;
            GameObject purp = Instantiate(PurpleHitPrefab, fireball.transform);

            if (!oneTime)
            {
                oneTime = true;
                GameController.Instance.StopFiringUpSound();
                GameController.Instance.PlayBombDropSound();
            }

            purp.transform.position = new Vector3(0, 0, 0);
            fireball.transform.localScale = new Vector3(0, 0, 0);
            purp.transform.localPosition = new Vector3(0, 0, 0);
            StartCoroutine(ScaleOverTime(purp, 2.5f, .4f));
            yield return StartCoroutine(ScaleOverTime(fireball, 1, .4f));
            UpdateLayer(fireball);
            if (egg == null) yield break;
            yield return MoveToPosition(egg,
                targetPos, .02f * Vector3.Distance(egg.transform.position, targetPos), false);

            if (!twoTime)
            {
                twoTime = true;
                GameController.Instance.StopLaunchSound();
                GameController.Instance.PlayBombExplosionSound();
            }

            StartCoroutine(ExplodeCoroutine(egg, target, SpiritType.LIFE));

            yield return null;
        }
        StartCoroutine(AnimalEffectController.Instance.destroyEggs(eggs));
        StartCoroutine(SpawnHealthText((int) targetPos.x, (int)targetPos.y, damage, Color.red));
        StartCoroutine(updateXp((int) animalPos.x, (int)animalPos.y, addedXp, Color.blue, animalId, xpRatio));
        if (GameLogic.Instance.IsPlayerOne && targetId < 3)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        } else if (!GameLogic.Instance.IsPlayerOne && targetId >= 3 && targetId < 6)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        }
        if (isDead)
        {
            StartCoroutine(HealthController.Instance.OnDeadAnimal(target, targetId));
        }
        else
        {
            StartCoroutine(FlashRed(target));
        }
        
        GameController.Instance.PlayBombExplosionSound();
        
        yield return MoveAnimalAttackSequence(target, direction, .15f);
    }

    public IEnumerator DoChickenEggSpecial(GameObject animal, GameObject target, int damage, bool isDead, float healthRatio, 
        int targetId, int addedXp, int animalId, float xpRatio, int tCount, List<GameObject> eggs)
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

        GameController.Instance.PlayFiringUpSound();
        bool oneTime = false;
        AnimalEffectController.Instance.spinEggs = true;
        bool twoTime = false;
        for (int i = 0; i < tCount; i++)
        {
            GameObject egg = eggs[i];
            
            GameObject fireball = Instantiate(FireAttackPrefab, egg.transform);
            fireball.transform.position = egg.transform.position;
            GameObject purp = Instantiate(PurpleHitPrefab, fireball.transform);
            
            if (!oneTime)
            {
                oneTime = true;
                GameController.Instance.StopFiringUpSound();
                GameController.Instance.PlayBombDropSound();
            }
            purp.transform.position = new Vector3(0, 0, 0);
            fireball.transform.localScale = new Vector3(0, 0, 0);
            purp.transform.localPosition = new Vector3(0, 0, 0);
            StartCoroutine(ScaleOverTime(purp, 2.5f, .4f));
            yield return StartCoroutine(ScaleOverTime(fireball, 1, .4f));
            UpdateLayer(fireball);
            if (egg == null) yield break;
            yield return MoveToPosition(egg, 
                targetPos, .02f*Vector3.Distance(egg.transform.position, targetPos), false);
            
            if (!twoTime)
            {
                twoTime = true;
                GameController.Instance.StopLaunchSound();
                GameController.Instance.PlayBombExplosionSound();
            }
            StartCoroutine(ExplodeCoroutine(egg, target, SpiritType.FIRE));

            yield return null;
        }
        
        //StartCoroutine(MoveAnimalAttackSequence(animal, direction, .2f));
        
        StartCoroutine(AnimalEffectController.Instance.destroyEggs(eggs));
        StartCoroutine(SpawnHealthText((int) targetPos.x, (int)targetPos.y, damage, Color.red));
        StartCoroutine(updateXp((int) animalPos.x, (int)animalPos.y, addedXp, Color.blue, animalId, xpRatio));
        if (GameLogic.Instance.IsPlayerOne && targetId < 3)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        } else if (!GameLogic.Instance.IsPlayerOne && targetId >= 3 && targetId < 6)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        }
        if (isDead)
        {
            StartCoroutine(HealthController.Instance.OnDeadAnimal(target, targetId));
        }
        else
        {
            StartCoroutine(FlashRed(target));
        }
        
        GameController.Instance.PlayBombExplosionSound();
        
        yield return MoveAnimalAttackSequence(target, direction, .15f);
        yield return null;
    }
    
    public IEnumerator DoSealSpecial(List<GameObject> shellList, GameObject animal, GameObject target, int damage,
            bool isDead, float healthRatio, int targetId, int addedXp, int animalId, float xpRatio)
        {
            List<GameObject> shells = new List<GameObject>(shellList);
            var position = target.transform.position;
            GameController.Instance.PlayFiringUpFx(AnimalEffectController.Instance.sealFx);
            yield return AnimalEffectController.Instance.FormCircleofGameObjects(shells, position, .4f);
            GameController.Instance.PlayFiringUpFx(AnimalEffectController.Instance.sealShellMoveFx);
            yield return StartCoroutine(AnimalEffectController.Instance.MoveObjectsToPoint(shells, position, 4f));
            GameObject fx = Instantiate(AnimalEffectController.Instance.clamshellFx, position, Quaternion.identity);
            AttacksController.Instance._cameraShake.Shake();
            GameController.Instance.PlayExplosionSound();
            
            StartCoroutine(SpawnHealthText((int) position.x, (int)position.y, damage, Color.red));
            StartCoroutine(updateXp((int) position.x, (int)position.y, addedXp, Color.blue, animalId, xpRatio));
        
            if (GameLogic.Instance.IsPlayerOne && targetId < 3)
            {
                HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
            } else if (!GameLogic.Instance.IsPlayerOne && targetId >= 3 && targetId < 6)
            {
                HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
            }
            if (isDead)
            {
                StartCoroutine(HealthController.Instance.OnDeadAnimal(target,targetId));
            }
            else
            {
                StartCoroutine(FlashRed(target));
            }
            StartCoroutine(AnimalEffectController.Instance.DoLightFlash(.2f));
            yield return AnimalEffectController.Instance.LaunchObjectsEquidistant(shells, .2f, 2f);
            foreach (var VARIABLE in shells)
            {
                Destroy(VARIABLE);
                shellList.Remove(VARIABLE);
            }
            Destroy(fx);
        }

    public IEnumerator DoDragonSpecial(GameObject animal, GameObject target, int damage, bool isDead, float healthRatio, 
        int targetId, int addedXp, int animalId, float xpRatio)
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
       // GameController.Instance.PlayFiringUpSound();
        yield return StartCoroutine(MoveAnimalAttackSequence(animal, direction, .2f));
        float angle = (float) GetAngle(animalPos, targetPos);
        GameController.Instance.PlayDragonFire();
        GameObject fireFx = Instantiate(AnimalEffectController.Instance.dragonFireFx, animalPos, Quaternion.identity);
        fireFx.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        _cameraShake.Shake(3f);
        yield return new WaitForSeconds(1.8f);
        StartCoroutine(FlashRed(target));
        StartCoroutine(SpawnHealthText((int) targetPos.x, (int)targetPos.y, damage, Color.red));
        StartCoroutine(updateXp((int) animalPos.x, (int)animalPos.y, addedXp, Color.blue, animalId, xpRatio));
        for (int i = 0; i < 6; i++)
        {
            StartCoroutine(DestroyOrb(i));
            yield return null;
        }

        StartCoroutine(PlantController.Instance.ReplacePlantsWithDirt());
        if (GameLogic.Instance.IsPlayerOne && targetId < 3)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        } else if (!GameLogic.Instance.IsPlayerOne && targetId >= 3 && targetId < 6)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        }
        if (isDead)
        {
            StartCoroutine(HealthController.Instance.OnDeadAnimal(target,targetId));
        }
        Destroy(fireFx, 5f);
        yield return null;
    }
    public IEnumerator DoRhinoSpecial(GameObject animal, GameObject target, int damage, bool isDead, float healthRatio, 
        int targetId, int addedXp, int animalId, float xpRatio)
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
        // GameController.Instance.PlayFiringUpSound();
        var angle = GetAngle(animalPos, targetPos);
        float angleInRadians = (float)(angle+90) * Mathf.Deg2Rad;

        // Calculate the offset in x and y directions
        float offsetX = Mathf.Cos(angleInRadians) * 25;
        float offsetY = Mathf.Sin(angleInRadians) * 25;
        yield return StartCoroutine(RhinoCharge(animal, target, 2f, direction, angle, offsetX, offsetY,
            damage, addedXp, animalId, xpRatio, targetId, healthRatio, isDead));
        yield return null;
    }

    public IEnumerator DoGoatSpecial(GameObject animal, GameObject target, int damage, bool isDead, float healthRatio, 
        int targetId, int addedXp, int animalId, float xpRatio)
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
       // GameController.Instance.PlayFiringUpSound();
       var angle = GetAngle(animalPos, targetPos);
       float angleInRadians = (float)(angle+90) * Mathf.Deg2Rad;

       // Calculate the offset in x and y directions
       float offsetX = Mathf.Cos(angleInRadians) * 25;
       float offsetY = Mathf.Sin(angleInRadians) * 25;
       yield return StartCoroutine(GoatCharge(animal, target, 2f, direction, angle, offsetX, offsetY,
           damage, addedXp, animalId, xpRatio, targetId, healthRatio, isDead));
       yield return null;
    }
    
    public IEnumerator DoOwlSpecial(GameObject animal, GameObject target, int damage, bool isDead, float healthRatio, 
        int targetId, int addedXp, int animalId, float xpRatio)
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
        // GameController.Instance.PlayFiringUpSound();
        
        var timePerMove = .3f; 
        if (animal == null) yield break;
        GameObject boardLight = animal.transform.GetChild(3).gameObject;
        boardLight.transform.SetParent(null);
        float distanceToMove = -.3f;
        
        switch (direction)
        {
            case 0:
                yield return MoveToPosition(animal, new Vector3(animalPos.x, animalPos.y + distanceToMove, 0), timePerMove, false);
 
                break;
            case 1:
                yield return MoveToPosition(animal, new Vector3(animalPos.x + distanceToMove, animalPos.y, 0), timePerMove, false);
                
                break;
            case 2:
                yield return MoveToPosition(animal, new Vector3(animalPos.x, animalPos.y - distanceToMove, 0), timePerMove, false);
                break;
            case 3:
                yield return MoveToPosition(animal, new Vector3(animalPos.x - distanceToMove, animalPos.y, 0), timePerMove, false);
                break;
        }
        yield return MoveToPosition(animal, new Vector3(animalPos.x, animalPos.y + 3),
            .045f*Vector3.Distance(animalPos, new Vector3(animalPos.x, animalPos.y + 3)), false);
        yield return MoveToPosition(animal, new Vector3(animalPos.x, animalPos.y + 2.65f),
            .2f*Vector3.Distance(animalPos, new Vector3(animalPos.x, animalPos.y + 2.65f)), false);

        var position = animal.transform.position;
        var angle = GetAngle(position, targetPos);
        float angleInRadians = (float)(angle+90) * Mathf.Deg2Rad;
        animal.transform.Rotate(0f, 0f,(float)angle);
        yield return MoveToPosition(animal, targetPos,
            .05f*Vector3.Distance(position, targetPos), false);

        // Calculate the offset in x and y directions
        float offsetX = Mathf.Cos(angleInRadians) * 25;
        float offsetY = Mathf.Sin(angleInRadians) * 25;
        _cameraShake.Shake(1.5f);
        Vector3 newTargetPos = new Vector3(offsetX, offsetY);
        
        GameObject fx = Instantiate(AnimalEffectController.Instance.owlSpecialFx, targetPos, Quaternion.identity);
        
        StartCoroutine(DetachBoardLightGoatSpecial(target, 5f, true));
        StartCoroutine(SpawnHealthText((int) targetPos.x, (int)targetPos.y, damage, Color.red));
        StartCoroutine(updateXp((int) animalPos.x, (int)animalPos.y, addedXp, Color.blue, animalId, xpRatio));
        
        position = animalPos;
        if (GameLogic.Instance.IsPlayerOne && targetId < 3)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        } else if (!GameLogic.Instance.IsPlayerOne && targetId >= 3 && targetId < 6)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        }
        if (isDead)
        {
            StartCoroutine(HealthController.Instance.OnDeadAnimal(target,targetId));
        }
        else
        {
            StartCoroutine(FlashRed(target));
        }
        StartCoroutine(MoveToPosition(animal, newTargetPos,
            .05f*Vector3.Distance(targetPos, newTargetPos), false));
        yield return MoveToPosition(target, newTargetPos,
            .05f*Vector3.Distance(targetPos, newTargetPos), false);
        
        boardLight.transform.SetParent(animal.transform);
        
        boardLight.transform.position = new Vector3(animalPos.x, animalPos.y-0.2f, 0);
        boardLight.SetActive(true);
        
        animal.transform.position = position;
        animal.transform.Rotate(0f, 0f,-(float)angle);
        Destroy(fx);
        yield return null;
        yield return null;
    }


    private IEnumerator SpinAnimal(GameObject animal, float length)
    {
        float curTime = 0;
        while (curTime < length)
        {
            if (animal == null) yield break;
            animal.transform.Rotate(0, 0, 20);
            curTime += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }
    
    private IEnumerator DetachBoardLightGoatSpecial(GameObject animal, float delayTime, bool setInactive)
    {
        if (animal == null) yield break;
        GameObject boardLight = animal.transform.GetChild(3).gameObject;
        boardLight.transform.SetParent(null);
        if (setInactive)
        {
            boardLight.SetActive(false);
        }
        yield return new WaitForSeconds(delayTime);
        boardLight.transform.SetParent(animal.transform);
        Vector3 animalPos = animal.transform.position;
        boardLight.transform.position = new Vector3(animalPos.x, animalPos.y-0.2f, 0);
        boardLight.SetActive(true);
    }
    
    private IEnumerator RhinoCharge(GameObject animal, GameObject target, float delayTime, int direction, double angle,
        float offsetX, float offsetY, int damage, int addedXp, int animalId, float xpRatio, int targetId, float healthRatio, bool isDead)
    {
        Vector3 targetPos = target.transform.position;
        var timePerMove = .3f; 
        if (animal == null) yield break;
        GameObject boardLight = animal.transform.GetChild(3).gameObject;
        boardLight.transform.SetParent(null);
        animal.transform.Rotate(0f, 0f,(float)angle);
        float distanceToMove = -.3f;
        Vector3 animalPos = animal.transform.position;
        
        switch (direction)
        {
            case 0:
                yield return MoveToPosition(animal, new Vector3(animalPos.x, animalPos.y + distanceToMove, 0), timePerMove, false);
 
                break;
            case 1:
                yield return MoveToPosition(animal, new Vector3(animalPos.x + distanceToMove, animalPos.y, 0), timePerMove, false);
                
                break;
            case 2:
                yield return MoveToPosition(animal, new Vector3(animalPos.x, animalPos.y - distanceToMove, 0), timePerMove, false);
                break;
            case 3:
                yield return MoveToPosition(animal, new Vector3(animalPos.x - distanceToMove, animalPos.y, 0), timePerMove, false);
                break;
        }
        GameObject groundFx = Instantiate(AnimalEffectController.Instance.rhinoGroundFx, animalPos, Quaternion.identity);
        StartCoroutine(MoveToPosition(groundFx, targetPos, .045f*Vector3.Distance(animalPos, targetPos), false));
        
        yield return MoveToPosition(animal, targetPos, .045f*Vector3.Distance(animalPos, targetPos), false);
        _cameraShake.Shake();
        Destroy(groundFx);

        Vector3 newTargetPos = new Vector3(offsetX, offsetY);
        GameObject fx = Instantiate(AnimalEffectController.Instance.rhinoHitFx, targetPos, Quaternion.identity);
        
        StartCoroutine(DetachBoardLightGoatSpecial(target, 5f, true));
        StartCoroutine(SpawnHealthText((int) targetPos.x, (int)targetPos.y, damage, Color.red));
        StartCoroutine(updateXp((int) animalPos.x, (int)animalPos.y, addedXp, Color.blue, animalId, xpRatio));
        StartCoroutine(SpinAnimal(target, 1f));

        animal.transform.position = animalPos;
        animal.transform.Rotate(0f, 0f,-(float)angle);
        if (GameLogic.Instance.IsPlayerOne && targetId < 3)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        } else if (!GameLogic.Instance.IsPlayerOne && targetId >= 3 && targetId < 6)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        }
        if (isDead)
        {
            StartCoroutine(HealthController.Instance.OnDeadAnimal(target,targetId));
        }
        else
        {
            StartCoroutine(FlashRed(target));
            yield return MoveToPosition(target, newTargetPos, 1f, false);
            target.transform.position = targetPos;
            target.transform.rotation = new Quaternion(0f, 0f,0, 0);
        }
        
        boardLight.transform.SetParent(animal.transform);
        
        boardLight.transform.position = new Vector3(animalPos.x, animalPos.y-0.2f, 0);
        boardLight.SetActive(true);
        Destroy(fx);
        yield return null;
    }
    private IEnumerator GoatCharge(GameObject animal, GameObject target, float delayTime, int direction, double angle,
        float offsetX, float offsetY, int damage, int addedXp, int animalId, float xpRatio, int targetId, float healthRatio, bool isDead)
    {
        Vector3 targetPos = target.transform.position;
        var timePerMove = .3f; 
        if (animal == null) yield break;
        GameObject boardLight = animal.transform.GetChild(3).gameObject;
        boardLight.transform.SetParent(null);
        animal.transform.Rotate(0f, 0f,(float)angle);
        float distanceToMove = -.3f;
        Vector3 animalPos = animal.transform.position;
        
        switch (direction)
        {
            case 0:
                yield return MoveToPosition(animal, new Vector3(animalPos.x, animalPos.y + distanceToMove, 0), timePerMove, false);
 
                break;
            case 1:
                yield return MoveToPosition(animal, new Vector3(animalPos.x + distanceToMove, animalPos.y, 0), timePerMove, false);
                
                break;
            case 2:
                yield return MoveToPosition(animal, new Vector3(animalPos.x, animalPos.y - distanceToMove, 0), timePerMove, false);
                break;
            case 3:
                yield return MoveToPosition(animal, new Vector3(animalPos.x - distanceToMove, animalPos.y, 0), timePerMove, false);
                break;
        }
        yield return MoveToPosition(animal, targetPos, .035f*Vector3.Distance(animalPos, targetPos), false);
        _cameraShake.Shake();
        

        Vector3 newTargetPos = new Vector3(offsetX, offsetY);
        GameObject fx = Instantiate(AnimalEffectController.Instance.goatHitFx, targetPos, Quaternion.identity);
        
        StartCoroutine(DetachBoardLightGoatSpecial(target, 5f, true));
        StartCoroutine(SpawnHealthText((int) targetPos.x, (int)targetPos.y, damage, Color.red));
        StartCoroutine(updateXp((int) animalPos.x, (int)animalPos.y, addedXp, Color.blue, animalId, xpRatio));
        StartCoroutine(SpinAnimal(target, 8f));

        animal.transform.position = animalPos;
        animal.transform.Rotate(0f, 0f,-(float)angle);
        if (GameLogic.Instance.IsPlayerOne && targetId < 3)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        } else if (!GameLogic.Instance.IsPlayerOne && targetId >= 3 && targetId < 6)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        }
        if (isDead)
        {
            StartCoroutine(HealthController.Instance.OnDeadAnimal(target,targetId));
        }
        else
        {
            StartCoroutine(FlashRed(target));
        }
        yield return MoveToPosition(target, newTargetPos, .8f, false);
        
        boardLight.transform.SetParent(animal.transform);
        
        boardLight.transform.position = new Vector3(animalPos.x, animalPos.y-0.2f, 0);
        boardLight.SetActive(true);
        Destroy(fx);
        yield return null;
    }
    
    public IEnumerator DoHuskySpecial(GameObject animal, GameObject target, int damage, bool isDead, float healthRatio, 
        int targetId, int addedXp, int animalId, float xpRatio)
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
       // GameController.Instance.PlayFiringUpSound();
        yield return StartCoroutine(MoveAnimalAttackSequence(animal, direction, .2f));
        float angle = (float) GetAngle(animalPos, targetPos);
        //GameController.Instance.PlayDragonFire();
        GameObject fireFx = Instantiate(AnimalEffectController.Instance.huskySpecialFx, animalPos, Quaternion.identity);
        fireFx.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        _cameraShake.Shake(3f);
        yield return new WaitForSeconds(1.8f);
        StartCoroutine(FlashRed(target));
        StartCoroutine(SpawnHealthText((int) targetPos.x, (int)targetPos.y, damage, Color.red));
        StartCoroutine(updateXp((int) animalPos.x, (int)animalPos.y, addedXp, Color.blue, animalId, xpRatio));
        
        if (GameLogic.Instance.IsPlayerOne && targetId < 3)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        } else if (!GameLogic.Instance.IsPlayerOne && targetId >= 3 && targetId < 6)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        }
        if (isDead)
        {
            StartCoroutine(HealthController.Instance.OnDeadAnimal(target,targetId));
        }
        else
        {
            StartCoroutine(PlantController.Instance.DoFrozenEffect(target));
        }
        Destroy(fireFx, 5f);
        yield return null;
    }
    public IEnumerator DoCatSpecial(GameObject animal, GameObject target, int damage, bool isDead, float healthRatio, 
        int targetId, int addedXp, int animalId, float xpRatio)
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
       // GameController.Instance.PlayFiringUpSound();
        yield return StartCoroutine(MoveAnimalAttackSequence(animal, direction, .2f));
        float angle = (float) GetAngle(animalPos, targetPos);
        //GameController.Instance.PlayDragonFire();
        GameObject fireFx = Instantiate(AnimalEffectController.Instance.catSpecialFx, animalPos, Quaternion.identity);
        fireFx.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        _cameraShake.Shake(3f);
        yield return new WaitForSeconds(1.8f);
        StartCoroutine(FlashRed(target));
        StartCoroutine(SpawnHealthText((int) targetPos.x, (int)targetPos.y, damage, Color.red));
        StartCoroutine(updateXp((int) animalPos.x, (int)animalPos.y, addedXp, Color.blue, animalId, xpRatio));
        
        if (GameLogic.Instance.IsPlayerOne && targetId < 3)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        } else if (!GameLogic.Instance.IsPlayerOne && targetId >= 3 && targetId < 6)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        }
        if (isDead)
        {
            StartCoroutine(HealthController.Instance.OnDeadAnimal(target,targetId));
        }
        Destroy(fireFx, 5f);
        yield return null;
    }
    
    
    private IEnumerator DoDeerSpecial(GameObject animal, GameObject target, int damage, bool isDead, float healthRatio, 
        int targetId, int addedXp, int animalId, float xpRatio, int addedHealth)
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
       // GameController.Instance.PlayFiringUpSound();
        yield return StartCoroutine(MoveAnimalAttackSequence(animal, direction, .2f));
        float angle = (float) GetAngle(animalPos, targetPos);
        //GameController.Instance.PlayDragonFire();
        GameObject fireFx = Instantiate(AnimalEffectController.Instance.deerSpecialFx, animalPos, Quaternion.identity);
        fireFx.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        _cameraShake.Shake(3f);
        StartCoroutine(SpawnHealthText((int) animalPos.x, (int)animalPos.y, addedHealth, Color.green));
        yield return new WaitForSeconds(1.8f);
        StartCoroutine(FlashRed(target));
        StartCoroutine(SpawnHealthText((int) targetPos.x, (int)targetPos.y, damage, Color.red));
        StartCoroutine(updateXp((int) animalPos.x, (int)animalPos.y, addedXp, Color.blue, animalId, xpRatio));
        
        if (GameLogic.Instance.IsPlayerOne && targetId < 3)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        } else if (!GameLogic.Instance.IsPlayerOne && targetId >= 3 && targetId < 6)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        }
        if (isDead)
        {
            StartCoroutine(HealthController.Instance.OnDeadAnimal(target,targetId));
        }
        Destroy(fireFx, 6f);
        yield return null;
    }
    private IEnumerator DoElephantSingleSpecial(GameObject animal, GameObject target, int damage, bool isDead, float healthRatio, 
        int targetId, int addedXp, int animalId, float xpRatio)
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
       // GameController.Instance.PlayFiringUpSound();
        yield return StartCoroutine(MoveAnimalAttackSequence(animal, direction, .2f));
        float angle = (float) GetAngle(animalPos, targetPos);
        //GameController.Instance.PlayDragonFire();
        GameObject fireFx = Instantiate(AnimalEffectController.Instance.elephantSpecialFx, animalPos, Quaternion.identity);
        fireFx.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        _cameraShake.Shake(3f);
        yield return new WaitForSeconds(1.8f);
        StartCoroutine(FlashRed(target));
        StartCoroutine(SpawnHealthText((int) targetPos.x, (int)targetPos.y, damage, Color.red));
        StartCoroutine(updateXp((int) animalPos.x, (int)animalPos.y, addedXp, Color.blue, animalId, xpRatio));
        
        if (GameLogic.Instance.IsPlayerOne && targetId < 3)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        } else if (!GameLogic.Instance.IsPlayerOne && targetId >= 3 && targetId < 6)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        }
        if (isDead)
        {
            StartCoroutine(HealthController.Instance.OnDeadAnimal(target,targetId));
        }
        Destroy(fireFx, 5f);
        yield return null;
    }
    
    private IEnumerator DoElephantAllSpecial(GameObject animal, GameObject target, int damage, bool isDead, float healthRatio, 
        int targetId, int addedXp, int animalId, float xpRatio, float healthRatio1, float healthRatio2, bool isDead1, bool isDead2)
    {
        GameObject go1, go2;
        int targetId1, targetId2;
        
        if (targetId == 1)
        {
            go1 = GetAnimal(2);
            go2 = GetAnimal(0);
            targetId1 = 2;
            targetId2 = 0;
        } else if (targetId == 2)
        {
            go1 = GetAnimal(1);
            go2 = GetAnimal(0);
            targetId1 = 1;
            targetId2 = 0;
        }  else if (targetId == 0)
        {
            go1 = GetAnimal(1);
            go2 = GetAnimal(2);
            targetId1 = 1;
            targetId2 = 2;
        }  else if (targetId == 3)
        {
            go1 = GetAnimal(4);
            go2 = GetAnimal(5);
            targetId1 = 4;
            targetId2 = 5;
        } else if (targetId == 4)
        {
            go1 = GetAnimal(3);
            go2 = GetAnimal(5);
            targetId1 = 3;
            targetId2 = 5;
        }  else
        {
            go1 = GetAnimal(3);
            go2 = GetAnimal(4);
            targetId1 = 3;
            targetId2 = 4;
        }  
        Vector3 animalPos = animal.transform.position;
        byte direction = 0;
        Vector3 targetPos, targetPos1, targetPos2;
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
       // GameController.Instance.PlayFiringUpSound();
       targetPos1 = go1.transform.position;
       targetPos2 = go2.transform.position;
        yield return StartCoroutine(MoveAnimalAttackSequence(animal, direction, .2f));
        float angle = (float) GetAngle(animalPos, targetPos);
        float angle1 = (float) GetAngle(animalPos, targetPos1);
        float angle2 = (float) GetAngle(animalPos, targetPos2);
        //GameController.Instance.PlayDragonFire();
        GameObject fireFx = Instantiate(AnimalEffectController.Instance.elephantSpecialFx, animalPos, Quaternion.identity);
        if (GameLogic.Instance.IsPlayerOne)
        {
            if (!GameLogic.Instance.enemyDeadAnimals.Contains(targetId1))
            {
                GameObject fireFx1 = Instantiate(AnimalEffectController.Instance.elephantSpecialFx, animalPos,
                    Quaternion.identity);
                fireFx1.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle1));
                Destroy(fireFx1, 7f);
            }

            if (!GameLogic.Instance.enemyDeadAnimals.Contains(targetId2))
            {
                GameObject fireFx2 = Instantiate(AnimalEffectController.Instance.elephantSpecialFx, animalPos, Quaternion.identity);
                fireFx2.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle2));
                Destroy(fireFx2, 7f);
            }
        }
        else
        {
            if (!GameLogic.Instance.enemyDeadAnimals.Contains(MessageHandlers.SwitchAnimal(targetId1)))
            {
                GameObject fireFx1 = Instantiate(AnimalEffectController.Instance.elephantSpecialFx, animalPos,
                    Quaternion.identity);
                fireFx1.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle1));
                Destroy(fireFx1, 7f);
            }

            if (!GameLogic.Instance.enemyDeadAnimals.Contains(MessageHandlers.SwitchAnimal(targetId2)))
            {
                GameObject fireFx2 = Instantiate(AnimalEffectController.Instance.elephantSpecialFx, animalPos, Quaternion.identity);
                fireFx2.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle2));
                Destroy(fireFx2, 7f);
            }
        }
        
        fireFx.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        
        _cameraShake.Shake(3f);
        yield return new WaitForSeconds(1.8f);
        StartCoroutine(FlashRed(target));
        StartCoroutine(SpawnHealthText((int) targetPos.x, (int)targetPos.y, damage, Color.red));
        StartCoroutine(FlashRed(go1));
        StartCoroutine(SpawnHealthText((int) targetPos1.x, (int)targetPos1.y, damage, Color.red));
        StartCoroutine(FlashRed(go2));
        StartCoroutine(SpawnHealthText((int) targetPos2.x, (int)targetPos2.y, damage, Color.red));
        StartCoroutine(updateXp((int) animalPos.x, (int)animalPos.y, addedXp, Color.blue, animalId, xpRatio));
        
        if (GameLogic.Instance.IsPlayerOne && targetId < 3)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
            HealthController.Instance.UpdateAnimalHealth(targetId1, healthRatio1);
            HealthController.Instance.UpdateAnimalHealth(targetId2, healthRatio2);
        } else if (!GameLogic.Instance.IsPlayerOne && targetId >= 3 && targetId < 6)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
            HealthController.Instance.UpdateAnimalHealth(targetId1, healthRatio1);
            HealthController.Instance.UpdateAnimalHealth(targetId2, healthRatio2);
        }
        if (isDead)
        {
            StartCoroutine(HealthController.Instance.OnDeadAnimal(target,targetId));
        } 
        if (isDead1)
        {
            StartCoroutine(HealthController.Instance.OnDeadAnimal(go1,targetId1));
        }
        if (isDead2)
        {
            StartCoroutine(HealthController.Instance.OnDeadAnimal(go2,targetId2));
        }
        Destroy(fireFx, 5f);
        yield return null;
    }
    
    private IEnumerator DoWhaleSpecial(GameObject animal, GameObject target, int damage, bool isDead, float healthRatio, 
        int targetId, int addedXp, int animalId, float xpRatio)
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
        GameObject fireball = Instantiate(AnimalEffectController.Instance.whaleBubble, ballSpawnPos, Quaternion.identity);
        UpdateLayer(fireball);
        fireball.transform.localScale = new Vector3(0, 0, 0);
        GameController.Instance.PlayFiringUpSound();
        yield return StartCoroutine(ScaleOverTime(fireball, maxFireballScale, 1f));
        StartCoroutine(MoveAnimalAttackSequence(animal, direction, .2f));
        GameController.Instance.PlayLaunchSound();
        yield return MoveToPosition(fireball, targetPos, .15f*Vector3.Distance(animalPos, targetPos), true);
        _cameraShake.Shake(.65f);
        yield return new WaitForSeconds(.5f);
        Destroy(fireball);
        StartCoroutine(SpawnHealthText((int) targetPos.x, (int)targetPos.y, damage, Color.red));
        StartCoroutine(updateXp((int) animalPos.x, (int)animalPos.y, addedXp, Color.blue, animalId, xpRatio));
        GameObject popfx = Instantiate(AnimalEffectController.Instance.whaleBubblePop, targetPos, Quaternion.identity);
        Destroy(popfx, 2.5f);
        StartCoroutine(ResetLights());
        if (GameLogic.Instance.IsPlayerOne && targetId < 3)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        } else if (!GameLogic.Instance.IsPlayerOne && targetId >= 3 && targetId < 6)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        }
        if (isDead)
        {
            StartCoroutine(HealthController.Instance.OnDeadAnimal(target, targetId));
        }
        else
        {
            StartCoroutine(FlashRed(target));
            yield return StartCoroutine(SpinAnimal(target, .6f));
            target.transform.rotation = Quaternion.identity;
        }
        
        GameController.Instance.PlayExplosionSound();
        
        yield return null;
    }
    
    private IEnumerator DoDinoSpecial(GameObject animal, GameObject target, int damage, bool isDead, float healthRatio, 
        int targetId, int addedXp, int animalId, float xpRatio)
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
        GameObject fireball = Instantiate(FireAttackPrefab, new Vector3(-10, 20, 0), Quaternion.identity);
        fireball.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        UpdateLayer(fireball);
        GameController.Instance.PlayFiringUpSound();
        StartCoroutine(MoveAnimalAttackSequence(animal, direction, .2f));
        GameController.Instance.PlayLaunchSound();
        yield return MoveToPosition(fireball, targetPos, 
            .1f*Vector3.Distance(fireball.transform.position, targetPos), true);
        _cameraShake.Shake();
        StartCoroutine(SpawnHealthText((int) targetPos.x, (int)targetPos.y, damage, Color.red));
        StartCoroutine(updateXp((int) animalPos.x, (int)animalPos.y, addedXp, Color.blue, animalId, xpRatio));
        
        if (GameLogic.Instance.IsPlayerOne && targetId < 3)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        } else if (!GameLogic.Instance.IsPlayerOne && targetId >= 3 && targetId < 6)
        {
            HealthController.Instance.UpdateAnimalHealth(targetId, healthRatio);
        }
        if (isDead)
        {
            StartCoroutine(HealthController.Instance.OnDeadAnimal(target,targetId));
        }
        else
        {
            StartCoroutine(FlashRed(target));
        }
        StartCoroutine(ExplodeCoroutine(fireball, target, SpiritType.FIRE));
        GameController.Instance.PlayExplosionSound();
        yield return MoveAnimalAttackSequence(target, direction, .15f);
        yield return null;
    }
    private double GetAngle(Vector3 pos1, Vector3 pos2)
    {
        Vector2 direction = pos1 - pos2;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        return angle;
    }
}

