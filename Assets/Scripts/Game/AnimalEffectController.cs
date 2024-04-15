using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using Network;
using Scripts.GameStructure;
using Server.Game;
using SharedLibrary;
using UnityEngine;
using Object = System.Object;
using Random = System.Random;

namespace Game
{
    public class AnimalEffectController : MonoBehaviour
    {
        private static AnimalEffectController instance;
        public static AnimalEffectController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AnimalEffectController>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("AnimalEffectController");
                        instance = go.AddComponent<AnimalEffectController>();
                    }
                }
                return instance;
            }
        }


        [SerializeField] private GameObject chickenEgg, chickenEggEffect, elephantDirtEffect, dragonFireEffect, clamShell,
            frogEgg,
            test1,test2,
            dogbone,
            dinoExtinctFx,
            teddyBear, 
            bearSpecialFx,
            rhinoPassiveFx,
            whaleHealFx;

        [SerializeField] private Sprite tuxedoBearFront,
            tuxedoBearBack,
            bearNoBearFront,
            bearNoBearBack,
            bearBearFront,
            bearBearBack;

        public GameObject clamshellFx,
            dragonFireFx,
            goatHitFx,
            huskySpecialFx,
            elephantSpecialFx,
            catSpecialFx,
            deerSpecialFx,
            owlSpecialFx,
            rhinoGroundFx,
            rhinoHitFx,
            whaleBubble,
            whaleBubblePop;
        public AudioClip sealFx, sealShellMoveFx,
            dragonFireSound;
        private int circleRadius = 3;
        public List<GameObject> tadpolesList, eggs, shellList, bunnyEggs = new List<GameObject>();

        public Dictionary<int, GameObject> dinoExtinctLocations, animalSpecialFx;
        public Dictionary<(int, int), GameObject> teddyBearsDictionary;
        private Color eggPurple, eggGreen, eggYellow, eggRed, eggOrange, eggBlue;
        public bool spinEggs, spinBunnyEggs = false;
        private float movementSpeed = 200f;
        private float lerpDuration = .2f;
        private void Start()
        {
            //StartCoroutine(DoTadpoleFrogSpecial());
            eggPurple = new Color(174f / 255f, 89f / 255f, 238f / 255f);
            eggRed = new Color(229f / 255f, 49f / 255f, 59f / 255f);
            eggOrange = new Color(236f / 255f, 154f / 255f, 61f / 255f);
            eggYellow = new Color(234f / 255f, 226f / 255f, 36f / 255f);
            eggBlue = new Color(86f / 255f, 158f / 255f, 255f / 255f);
            eggGreen = new Color(111f / 255f, 227f / 255f, 97f / 255f);
            dinoExtinctLocations = new Dictionary<int, GameObject>();
            teddyBearsDictionary = new Dictionary<(int,int), GameObject>();
            animalSpecialFx = new Dictionary<int, GameObject>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                testing();
            }

            if (spinEggs)
            {
                foreach (GameObject egg in eggs)
                {
                    if (egg != null)
                    {
                        egg.transform.Rotate(new Vector3(0, 0, -2000*Time.deltaTime));
                    }
                }
            }
            if (spinBunnyEggs)
            {
                foreach (GameObject egg in bunnyEggs)
                {
                    if (egg != null)
                    {
                        egg.transform.Rotate(new Vector3(0, 0, -2000*Time.deltaTime));
                    }
                }
            }
        }

        public void DoEffect(int effectId, int x, int y, int crystalKey, int newX, int newY, int addedHealth)
        {
            //Debug.Log((PassiveEffectIds)effectId);
            switch (effectId)
            {
                case 0 :
                    break;
                case (int)PassiveEffectIds.ChickenEggEffect:
                    ChickenLayEggEffect(x, y, crystalKey);
                    break;
                case (int)PassiveEffectIds.HedgehogDirtEffect:
                    PlantController.Instance.SpawnDirtTile(x-Constants.hor_offset, y-Constants.vert_offset, true);
                    break;
                case (int) PassiveEffectIds.DeerPlantEffect:
                    StartCoroutine(DeerEatEffect(newX, newY, addedHealth));
                    break;
                case (int) PassiveEffectIds.SealClamShellEffect:
                    StartCoroutine(DoSealEffect(x, y, crystalKey));
                    break;
                case (int) PassiveEffectIds.FrogEggEffect:
                    StartCoroutine(DoFrogEggEffect(x, y, crystalKey));
                    break;
                case (int) PassiveEffectIds.BunnyEggEffect:
                    BunnyLayEggEffect(x, y, crystalKey);
                    break;
                case (int) PassiveEffectIds.HuskyDropBone:
                    HuskyDropBone(x, y, crystalKey);
                    break;
            }

        }
        
        public void HuskyDropBone(int x, int y, int crystalKey)
        {
            GameObject bone = Instantiate(dogbone, new Vector3(x, y, 0), Quaternion.identity);
            GameLogic.Instance.CrystalMap[(byte)(crystalKey)] = bone;
            CrystalController.Instance.UpdateLayerCrystal(bone);
            GameObject effects = Instantiate(chickenEggEffect, new Vector3(x, y, 0), Quaternion.identity);
            Destroy(effects, 2f);
        }
        public void BunnyLayEggEffect(int x, int y, int crystalKey)
        {
            GameObject egg = Instantiate(chickenEgg, new Vector3(x, y, 0), Quaternion.identity);
            egg.transform.GetChild(0).GetComponent<SpriteRenderer>().color = PickRandomEggColor();
            GameLogic.Instance.CrystalMap[(byte)(crystalKey)] = egg;
            CrystalController.Instance.UpdateLayerCrystal(egg);
            bunnyEggs.Add(egg);
            GameObject effects = Instantiate(chickenEggEffect, new Vector3(x, y, 0), Quaternion.identity);
            Destroy(effects, 2f);
        }
        public void ChickenLayEggEffect(int x, int y, int crystalKey)
        {
            GameObject egg = Instantiate(chickenEgg, new Vector3(x, y, 0), Quaternion.identity);
            GameLogic.Instance.CrystalMap[(byte)(crystalKey)] = egg;
            CrystalController.Instance.UpdateLayerCrystal(egg);
            eggs.Add(egg);
            GameObject effects = Instantiate(chickenEggEffect, new Vector3(x, y, 0), Quaternion.identity);
            Destroy(effects, 2f);
        }

        public IEnumerator DoElephantEffect(int animalId, int addedHealth)
        {
            GameObject animal = AttacksController.Instance.GetAnimal(animalId);

            Vector3 animalLoc = animal.transform.position;
            GameObject dirtPArticles = Instantiate(elephantDirtEffect, animalLoc, Quaternion.identity);
            StartCoroutine(AttacksController.Instance.SpawnHealthText(Mathf.RoundToInt(animalLoc.x), Mathf.RoundToInt(animalLoc.y),
                addedHealth, Color.green));
            Destroy(dirtPArticles, 1f);
            
            yield return null;
        }

        public IEnumerator DoDragonEffect(int animalId)
        {
            GameObject animal = AttacksController.Instance.GetAnimal(animalId);

            Vector3 animalLoc = animal.transform.position;
            GameObject fireParticles = Instantiate(dragonFireEffect, animalLoc, Quaternion.identity);
            Destroy(fireParticles, 1f);
            yield return null;
        }

        public IEnumerator DoBearSpecial(int animalId)
        {
            Debug.LogWarning("animalId Bear spec: " + animalId);
            GameObject animal = AttacksController.Instance.GetAnimal(animalId);

            Vector3 animalPos = animal.transform.position;
            GameObject go = Instantiate(bearSpecialFx, animalPos, Quaternion.identity);
            go.transform.SetParent(animal.transform.GetChild(3));
            if ((animalId < 3 && GameLogic.Instance.IsPlayerOne) || 
                (animalId >= 3 && !GameLogic.Instance.IsPlayerOne))
            {
                animal.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = tuxedoBearBack;
            }
            else
            {
                animal.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = tuxedoBearFront;
            }
            
            animalSpecialFx.Add(animalId, go);
            yield return null;
        }

        public IEnumerator DoHippoEffect(int animalId)
        {
            GameObject animal = AttacksController.Instance.GetAnimal(animalId);

            Vector3 animalLoc = animal.transform.position;
            GameObject dirtPArticles = Instantiate(elephantDirtEffect, animalLoc, Quaternion.identity);
            
            Destroy(dirtPArticles, 1f);
            
            yield return null;
        }

        private IEnumerator DeerEatEffect(int x, int y, int addedHealth)
        {
            GameObject plant = PlantController.Instance.plantObjects[(x, y)];
            Destroy(plant);
            PlantController.Instance.plantObjects.Remove((x, y));
            //Debug.Log("plant destroyed at " + x + " " + y);

            StartCoroutine(AttacksController.Instance.SpawnHealthText(x+Constants.hor_offset, y+Constants.vert_offset,
                addedHealth, addedHealth == 0 ? Color.gray : Color.green));
            yield return null;
        }

        private IEnumerator DoSealEffect(int x, int y, int crystalKey)
        {
            GameObject egg = Instantiate(clamShell, new Vector3(x, y, 0), Quaternion.identity);
            GameLogic.Instance.CrystalMap[(byte)(crystalKey)] = egg;
            CrystalController.Instance.UpdateLayerCrystal(egg);
            shellList.Add(egg);
            GameObject effects = Instantiate(chickenEggEffect, new Vector3(x, y, 0), Quaternion.identity);
            Destroy(effects, 2f);
            yield return null;
        }

        private IEnumerator DoFrogEggEffect(int x, int y, int crystalKey)
        {
            GameObject egg = Instantiate(frogEgg, new Vector3(x, y, 0), Quaternion.identity);
            GameObject effects = Instantiate(chickenEggEffect, new Vector3(x, y, 0), Quaternion.identity);
            GameLogic.Instance.CrystalMap[(byte)(crystalKey)] = egg;
            x -= Constants.hor_offset;
            y -= Constants.vert_offset;

            PlantController.Instance.animalItems[(x, y)] = egg;
            CrystalController.Instance.UpdateLayerCrystal(egg);
            
            Destroy(effects, 2f);
            yield return null;
        }

        public IEnumerator MoveObjectsToPoint(List<GameObject> objects, Vector3 targetPoint, float speed)
        {
            if(objects == null) yield break;
            if(objects[0] == null) yield break;
            // Calculate the time it will take for objects to reach the target point
            float totalTime = Vector3.Distance(objects[0].transform.position, targetPoint) / speed;

            List<Vector3> originalPositions = new List<Vector3>();
            foreach (GameObject o in objects)
            {
                originalPositions.Add(o.transform.position);
            }
            // Move objects towards the target point
            float elapsedTime = 0f;
            while (elapsedTime < totalTime)
            {
                for (int i = 0; i< objects.Count; i++)
                {
                    GameObject o = objects[i];
                    o.transform.position = Vector3.Lerp(originalPositions[i], targetPoint, elapsedTime / totalTime);
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure objects reach the exact target point
            foreach (var o in objects)
            {
                o.transform.position = targetPoint;
            }
        }
        

        public IEnumerator DoChickenSpecial(List<GameObject> eggList, GameObject animal, GameObject target, int damage,
            bool isDead, float healthRatio, int targetId, int addedXp, int animalId, float xpRatio )
        {
            List<GameObject> eggListTest = new List<GameObject>(eggList);
            yield return FormCircleofGameObjects(eggListTest, new Vector3(0, 8.2f, 0), .4f);
            yield return StartCoroutine(AttacksController.Instance.DoChickenEggSpecial(animal, target, damage, isDead, healthRatio, targetId,
                addedXp, animalId, xpRatio, eggList.Count, eggList));
            foreach (var VARIABLE in eggListTest)
            {
                eggList.Remove(VARIABLE);
                eggs.Remove(VARIABLE);
            }
            
            spinEggs = false;
            
            yield return null;
        }
        public IEnumerator DoBunnyEggSpecial(List<GameObject> eggList, GameObject animal, GameObject target, int damage,
            bool isDead, float healthRatio, int targetId, int addedXp, int animalId, float xpRatio )
        {
            List<GameObject> eggListTest = new List<GameObject>(eggList);
            yield return FormCircleofGameObjects(eggListTest, new Vector3(0, 8.2f, 0), .4f);
            yield return StartCoroutine(AttacksController.Instance.DoBunnyEggSpecial(animal, target, damage, isDead, healthRatio, targetId,
                addedXp, animalId, xpRatio, eggListTest.Count, eggListTest));
            foreach (var VARIABLE in eggListTest)
            {
                eggList.Remove(VARIABLE);
                bunnyEggs.Remove(VARIABLE);
            }
            
            spinBunnyEggs = false;
            
            yield return null;
        }

        public IEnumerator FormCircleofGameObjects(List<GameObject> gameObjects, Vector3 centerPos, float totalTime)
        {
            
            int tCount = gameObjects.Count;
            if (tCount == 0) yield break;

            // Calculate angle between each tadpole
            float angleStep = 360f / tCount;
            float currentAngle = 0f;

            // Lerping variables
            float lerpProgress = 0f;
            float lerpSpeed = 1f / lerpDuration;

            List<GameObject> objectsToRemove = new List<GameObject>();
            for (int i = 0; i < tCount; i++)
            {
                if (gameObjects[i] != null)
                {
                    gameObjects[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = "Covers";
                }
                else
                {
                    objectsToRemove.Add(gameObjects[i]);
                }

                yield return null;
            }
            foreach(GameObject go in objectsToRemove)
            {
                gameObjects.Remove(go);
            }
            tCount = gameObjects.Count;
            
            totalTime = totalTime/gameObjects.Count;

            List<Vector3> originalPositions = new List<Vector3>();
            foreach (GameObject o in gameObjects)
            {
                originalPositions.Add(o.transform.position);
            }
            // Lerping all tadpoles to their positions simultaneously
            float time = 0;
            while (time < totalTime)
            {
                
                // Position tadpoles in a circle around the center
                for (int i = 0; i < tCount; i++)
                {
                    if (gameObjects[i] == null) continue;
                    float x = Mathf.Sin(Mathf.Deg2Rad * currentAngle) * circleRadius;
                    float y = Mathf.Cos(Mathf.Deg2Rad * currentAngle) * circleRadius;
                    Vector3 targetPosition = new Vector3(centerPos.x + x, centerPos.y + y, 0);

                    gameObjects[i].transform.position = Vector3.Lerp(originalPositions[i], targetPosition, time/totalTime);

                    currentAngle += angleStep;
                    yield return null;
                }

                time += Time.deltaTime;
                yield return null;
            }

        }
        public IEnumerator DoTadpoleFrogSpecial(GameObject animal, GameObject target, int damage,
            bool isDead, float healthRatio, int targetId, int addedXp, int animalId, float xpRatio)
        {
            List<GameObject> tempTadpoles = new List<GameObject>(tadpolesList);
            yield return FormCircleofGameObjects(tempTadpoles, animal.transform.position, .4f);
            yield return StartCoroutine(AttacksController.Instance.DoBigWaterAttack(animal, target, damage, isDead, healthRatio, targetId,
                addedXp, animalId, xpRatio, tempTadpoles.Count, tempTadpoles));
            foreach (GameObject tadpole in tempTadpoles)
            {
                Destroy(tadpole);
                tadpolesList.Remove(tadpole);
            }
            
        }

        public IEnumerator destroyEggs(List<GameObject> eggs)
        {
            foreach (var VARIABLE in eggs)
            {
                Destroy(VARIABLE);
            }

            yield return null;
        }
        public IEnumerator destroyTadpoles(int tCount, Vector3 animalPos)
        {
            for (int i = 0; i < tCount; i++)
            {
                Destroy(tadpolesList[i]);
            }

            yield return null;
        }
        public IEnumerator launchTadpoles(List<GameObject> tadpoles, int tCount, Vector3 animalPos)
        {
            float launchSpeed = 200f;
            float launchTime = 0f;
            while (launchTime < 1f)
            {
                for (int i = 0; i < tCount; i++)
                {
                    if(tadpoles[i] == null) yield break;
                    Vector3 tangent = new Vector3((tadpoles[i].transform.position.x - animalPos.x), (tadpoles[i].transform.position.y-animalPos.y), 0).normalized;

                    Vector3 launchDirection = tangent;
                    

                    {
                        launchTime += Time.deltaTime;
                        tadpoles[i].transform.position += launchDirection * (Time.deltaTime * launchSpeed);
                        yield return null;
                    }

                    yield return null;
                }
            }
            yield return null;
        }
        
        public IEnumerator LaunchObjectsEquidistant(List<GameObject> objects, float speed, float time)
        {
            if (objects == null || objects.Count == 0)
            {
                Debug.LogError("Object list is null or empty!");
                yield break;
            }

            // Calculate the angle between each launched object
            float angleBetweenObjects = 360f / objects.Count;

            // Launch objects equidistant from the center
            for (int i = 0; i < objects.Count; i++)
            {
                // Calculate launch direction based on current angle
                float launchAngle = i * angleBetweenObjects;
                Vector3 launchDirection = Quaternion.Euler(0f, 0f, launchAngle) * Vector3.right;

                // Launch the object
                StartCoroutine(MoveObject(objects[i].transform, launchDirection, speed, time));
            }

            // Wait until all objects reach their destinations
            yield return new WaitForSeconds(speed);
        }

        IEnumerator MoveObject(Transform objectTransform, Vector3 direction, float speed, float length)
        {
            float elapsedTime = 0f;
            Vector3 startPosition = objectTransform.position;
            Vector3 targetPosition = startPosition + direction*20;

            
            while (elapsedTime < length)
            {
                if (objectTransform == null) yield break;
                objectTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / speed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the object reaches the exact target position
            objectTransform.position = targetPosition;
        }

        public IEnumerator BearDropTeddy(GameObject animal, int animalId, int teddyX, int teddyY)
        {
            SpriteRenderer sr = animal.transform.GetChild(1).GetComponent<SpriteRenderer>();
            if ((GameLogic.Instance.IsPlayerOne && animalId < 3) || (!GameLogic.Instance.IsPlayerOne && animalId >= 3)) 
            {
                sr.sprite = bearNoBearBack;
            } else if ((!GameLogic.Instance.IsPlayerOne && animalId < 3) || (GameLogic.Instance.IsPlayerOne && animalId >=3))
                
            {
                sr.sprite = bearNoBearFront;
            }

            GameObject droppedTeddy = Instantiate(teddyBear, new Vector3(teddyX, teddyY, 0), Quaternion.identity);
            teddyBearsDictionary.Add((teddyX, teddyY), droppedTeddy);
            yield return null;
        }

        public IEnumerator PickupTeddy(GameObject animal, int animalId, int tX, int tY)
        {
            if ((tX == -2 && tY == -2) || (tX == Constants.max_x+1 && tY == Constants.max_y+1))
            {
                SpriteRenderer sr = animal.transform.GetChild(1).GetComponent<SpriteRenderer>();
                if (animalId < 3) 
                {
                    sr.sprite = bearBearBack;
                } else
                {
                    sr.sprite = bearBearFront;
                }

                var position = animal.transform.position;
                int x = Mathf.RoundToInt(position.x);
                int y = Mathf.RoundToInt(position.y);
                Destroy(teddyBearsDictionary[(x,y)]);
                teddyBearsDictionary.Remove((x,y));
            }
            else
            {
                var position = animal.transform.position;
                int x = Mathf.RoundToInt(position.x);
                int y = Mathf.RoundToInt(position.y);
                GameObject g = teddyBearsDictionary[(x,y)];
                g.transform.position = new Vector3(tX + Constants.hor_offset, tY + Constants.vert_offset, 0);
                if (!(tX + Constants.hor_offset == x && tY + Constants.vert_offset == y))
                {
                    teddyBearsDictionary.Remove((x,y));
                    teddyBearsDictionary.Add((tX+Constants.hor_offset, tY+Constants.vert_offset), g);
                    Debug.Log("teddy bear added to " + (tX + Constants.hor_offset) + " y " +  (tY + Constants.vert_offset));
                }
                
            }
            
            yield return null;
        }
        
        public IEnumerator spinTadpoles(List<GameObject> tadpoles, Vector3 animalPos, int tCount)
        {
            float currentAngle = 0f;
            float spinCount = .15f;
            // Move tadpoles along the circumference of another circle
            while (spinCount > 0)
            {
                for (int i = 0; i < tCount; i++)
                {
                   if(tadpoles[i] == null) yield break;
                    currentAngle = Mathf.Atan2(tadpoles[i].transform.position.y-animalPos.y, tadpoles[i].transform.position.x - animalPos.x) * Mathf.Rad2Deg;
                    currentAngle += movementSpeed * Time.deltaTime;
                    float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * circleRadius;
                    float y = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * circleRadius;
                    tadpoles[i].transform.position = new Vector3(x+animalPos.x, y + animalPos.y, 0);
                    yield return null;
                }

                spinCount -= Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
        

        public IEnumerator DoEndTurnModifier(GameEventParams gameEventParams)
        {
            int animalId = gameEventParams.animalId;
            int addedHealth = gameEventParams.addedHealth;
            int modifier = gameEventParams.statusEffectId;
            GameObject animal = AttacksController.Instance.GetAnimal(animalId);
            switch ((PassiveEffectIds)modifier)
            {
                case PassiveEffectIds.DinoGoExtinct:
                    yield return DoDinoExtinct(animal, animalId);
                    break;
                case PassiveEffectIds.WhaleHeal:
                    if(addedHealth > 0) yield return DoWhaleHeal(animal, addedHealth);
                    break;
            }

            yield return null;
        }

        public IEnumerator DoWhaleHeal(GameObject animal, int addedHealth)
        {
            Vector3 animalPos = animal.transform.position;
            GameObject greenPArts = Instantiate(whaleHealFx, animalPos, Quaternion.identity);
            Destroy(greenPArts, 3f);
            yield return AttacksController.Instance.SpawnHealthText((int)animalPos.x, (int)animalPos.y, addedHealth, Color.green);
        }

        public IEnumerator DoRhinoPassive(int animalId)
        {
            GameObject animal = AttacksController.Instance.GetAnimal(animalId);
            Vector3 animalPos = animal.transform.position;
            GameObject fx = Instantiate(rhinoPassiveFx, new Vector3(animalPos.x, animalPos.y - .2f, 0), Quaternion.identity);
            Destroy(fx, 1f);
            yield return null;
        }

        private IEnumerator DoDinoExtinct(GameObject animal, int animalId)
        {
            SpriteRenderer sr = animal.transform.GetChild(1).GetComponent<SpriteRenderer>();
            while (sr.color.a > 0)
            {
                Color c = sr.color;
                sr.color = new Color(c.r, c.g, c.b, c.a - Time.deltaTime);
                yield return null;
            }
            animal.SetActive(false);
            AddDeadAnimal(animalId);
            Vector3 animalPos = animal.transform.position;
            GameObject blackHole = Instantiate(dinoExtinctFx, 
                new Vector3(animalPos.x, animalPos.y - .2f, 0), Quaternion.identity);
            dinoExtinctLocations.Add(animalId, blackHole);
            yield return null;
        }

        public IEnumerator DoDinoUnExtinct(int animalId)
        {
            GameObject dino = AttacksController.Instance.GetAnimal(animalId);
            SpriteRenderer sr = dino.transform.GetChild(1).GetComponent<SpriteRenderer>();
            dino.SetActive(true);
            while (sr.color.a < 1)
            {
                Color c = sr.color;
                sr.color = new Color(c.r, c.g, c.b, c.a + Time.deltaTime);
                yield return null;
            }
            RemoveDeadAnimal(animalId);
            dinoExtinctLocations.TryGetValue(animalId, out GameObject blackhole);
            Destroy(blackhole);
            dinoExtinctLocations.Remove(animalId);
            yield return null;
        }
        
        private void RemoveDeadAnimal(int targetId)
        {
            int na = GameLogic.Instance.numberOfAnimals;
            lock (HealthController.Instance.KOLock)
            {
                if (GameLogic.Instance.IsPlayerOne)
                {
                    if (targetId < na/2)
                    {
                        GameLogic.Instance.myDeadAnimals.Remove(targetId);
                    }
                    else
                    {
                        GameLogic.Instance.enemyDeadAnimals.Remove(targetId);
                    }
                    
                }
                else
                {
                    if (MessageHandlers.SwitchAnimal(targetId) < na/2)
                    {
                        GameLogic.Instance.myDeadAnimals.Remove(MessageHandlers.SwitchAnimal(targetId));
                    }
                    else
                    {
                        GameLogic.Instance.enemyDeadAnimals.Remove(MessageHandlers.SwitchAnimal(targetId));
                    }
                }
            }
        }

        private void AddDeadAnimal(int targetId)
        {
            lock (HealthController.Instance.KOLock)
            {
                
                if (GameLogic.Instance.IsPlayerOne)
                {
                    if (targetId < GameLogic.Instance.numberOfAnimals/2)
                    {
                        GameLogic.Instance.myDeadAnimals.Add(targetId);
                        Debug.Log("adding myDead animal " + targetId);
                    }
                    else
                    {
                        GameLogic.Instance.enemyDeadAnimals.Add(targetId);
                        Debug.Log("adding enemy dead animal " + targetId);
                    }
                    
                }
                else
                {
                    if (MessageHandlers.SwitchAnimal(targetId) < GameLogic.Instance.numberOfAnimals/2)
                    {
                        GameLogic.Instance.myDeadAnimals.Add(MessageHandlers.SwitchAnimal(targetId));
                        Debug.Log("p2:adding dead animal " + MessageHandlers.SwitchAnimal(targetId));
                    }
                    else
                    {
                        GameLogic.Instance.enemyDeadAnimals.Add(MessageHandlers.SwitchAnimal(targetId));
                        Debug.Log("p2:adding enemy dead animal " + MessageHandlers.SwitchAnimal(targetId));
                    }
                }
            }
        }
        

        public IEnumerator DoLightFlash(float increase)
        {

            yield return AttacksController.Instance.FlashLightUp(increase);
            yield return AttacksController.Instance.FlashLightDown(increase);
        }

        private void testing()
        {
            //StartCoroutine(AttacksController.Instance.DoRhinoSpecial(test1, test2, 1000,
             // true, 0, 3, 0, 0, 0));
              //StartCoroutine(EmoteController.Instance.ShowEmote(EmoteCodes.OWL_SMOKE, 2));
        }

        public Color PickRandomEggColor()
        {
            Random r = new Random();
            int val = r.Next(6);
            switch (val)
            {
                case 0:
                    return eggRed;
                case 1:
                    return eggOrange;
                case 2:
                    return eggYellow;
                case 3:
                    return eggGreen;
                case 4:
                    return eggBlue;
                case 5:
                    return eggPurple;
                default:
                    return eggGreen;
            }
        }

    }
}