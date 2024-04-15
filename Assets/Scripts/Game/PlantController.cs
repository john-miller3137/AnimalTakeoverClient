using System;
using System.Collections;
using System.Collections.Generic;
using Riptide;
using Scripts.GameStructure;
using Server.Game;
using SharedLibrary;
using SharedLibrary.Objects;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class PlantController : MonoBehaviour
    {
        private static PlantController instance;
        public static PlantController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<PlantController>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("PlantController");
                        instance = go.AddComponent<PlantController>();
                    }
                }
                return instance;
            }
        }

        public GameObject dirt, poisonPlant1, poisonPlant2, icePlant1, icePlant2, flowerPlant1, poppy2, dandelion2, 
            marigold2, turrentPlant1, turretPlant2, poisonParticles, frozenEffect, ringDisplay, poppyEffect, turretShot,
            shield2, shieldEffect,
            tadpole;
        public Dictionary<(int, int), GameObject> plantObjects, poppyObjects, shieldObjects,
            animalItems, tadpoles, dirtTiles;
        private const int gameBoardOffsetX = 4;
        private const int gameBoardOffsetY = 6;
        public object dirtLock = new object();
        public void SpawnDirtTile(int x, int y, bool enableTC)
        {
            lock (dirtLock)
            {
                if (dirtTiles.ContainsKey((x, y)))
                {
                    Destroy(dirtTiles[(x, y)]);
                    dirtTiles.Remove((x, y));
                }
            }

            if (plantObjects.ContainsKey((x, y)))
            {
                Destroy(plantObjects[(x,y)]);
                plantObjects.Remove((x, y));
            }
            GameObject dirt = Instantiate(this.dirt, new Vector3((float)x + Constants.hor_offset,
                (float)y + Constants.vert_offset), Quaternion.identity);
            if (!enableTC)
            {
                dirt.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
            }
            lock (dirtLock)
            {
                dirtTiles.Add((x,y),dirt);
            }

            if (plantObjects.ContainsKey((x, y)))
            {
                Destroy(plantObjects[(x,y)]);
                plantObjects.Remove((x, y));
            }
        }

        public void DisableDirtColliders()
        {
            lock (dirtLock)
            {
                foreach (GameObject dirt in dirtTiles.Values)
                {
                    dirt.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
        
        public void EnableDirtColliders()
        {
            MoveController.Instance.UpdateDirtTileColliders();
        }

        private void Start()
        {
            animalItems = new Dictionary<(int, int), GameObject>();
            shieldObjects = new Dictionary<(int, int), GameObject>();
            poppyObjects = new Dictionary<(int, int), GameObject>();
            plantObjects = new Dictionary<(int, int), GameObject>();
            dirtTiles = new Dictionary<(int, int), GameObject>();
        }

        private void Update()
        {
            
        }

        public void PlantSeed(int inventoryId, float x, float y)
        {
            lock (InventoryController.Instance.inventoryLock)
            {
                GameItem gi = InventoryController.Instance.inventoryGameItems[inventoryId];


                if (gi.IsSeed)
                {
                    ItemId itemId = gi.ItemId;
                    int newX = Mathf.RoundToInt(x) + gameBoardOffsetX;
                    int newY = Mathf.RoundToInt(y) + gameBoardOffsetY;

                    Debug.Log($"send plant request {newX} {newY}");
                    GameMessages.SendPlantRequest((int)itemId, newX, newY);
                }
            }
        }

        public void DoPlant(int seedId, int plantId, int x, int y, byte playerNum)
        {
            byte playerCode = (byte)GameLogic.Instance.playerNum;
            if (playerNum == playerCode)
            {
                InventoryController.Instance.DoRemoveItemFromInventory(seedId);
            }
            if (GameLogic.Instance.IsPlayerOne)
            {
                switch (plantId)
                {
                    case ConstantVars.ice_plant1_id:
                        GameObject iceplant1 = Instantiate(icePlant1, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        plantObjects.Add((x, y), iceplant1);
                        
                        break;
                    case ConstantVars.poison_plant1_id:
                        GameObject poisonplant1 = Instantiate(poisonPlant1, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        plantObjects.Add((x, y), poisonplant1);
                        
                        break;
                    case ConstantVars.poppy_plant1:
                        GameObject poppy1 = Instantiate(flowerPlant1, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        plantObjects.Add((x, y), poppy1);
                        
                        break;
                    case ConstantVars.dandelion1:
                        GameObject dandelion1 = Instantiate(flowerPlant1, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        plantObjects.Add((x, y), dandelion1);
                        
                        break;
                    case ConstantVars.marigold1:
                        GameObject marigold1 = Instantiate(flowerPlant1, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        plantObjects.Add((x, y), marigold1);
                        
                        break;
                    case ConstantVars.turrent_plant1:
                        GameObject turret1 = Instantiate(turrentPlant1, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        plantObjects.Add((x, y), turret1);
                        
                        break;
                    case ConstantVars.shield1:
                        GameObject shield1 = Instantiate(flowerPlant1, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        plantObjects.Add((x, y), shield1);
                        
                        break;
                    default:
                        break;
                }  
            }
            else
            {
                x = GameLogic.flipX(x);
                y = GameLogic.flipY(y);
                switch (plantId)
                {
                    case ConstantVars.ice_plant1_id:
                        GameObject iceplant1 = Instantiate(icePlant1, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        plantObjects.Add((x, y), iceplant1);
                       
                        break;
                    case ConstantVars.poison_plant1_id:
                        GameObject poisonplant1 = Instantiate(poisonPlant1, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        plantObjects.Add((x, y), poisonplant1);
                        
                        break;
                    case ConstantVars.poppy_plant1:
                        GameObject poppy1 = Instantiate(flowerPlant1, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        plantObjects.Add((x, y), poppy1);
                        
                        break;
                    case ConstantVars.dandelion1:
                        GameObject dandelion1 = Instantiate(flowerPlant1, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        plantObjects.Add((x, y), dandelion1);
                        
                        break;
                    case ConstantVars.marigold1:
                        GameObject marigold1 = Instantiate(flowerPlant1, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        plantObjects.Add((x, y), marigold1);
                       
                        break;
                    case ConstantVars.turrent_plant1:
                        GameObject turret1 = Instantiate(turrentPlant1, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        plantObjects.Add((x, y), turret1);
                        
                        break;
                    case ConstantVars.shield1:
                        GameObject shield1 = Instantiate(flowerPlant1, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        plantObjects.Add((x, y), shield1);
                        
                        break;
                    default:
                        break;
                }  
            }
        }

        public void DoPlantGrowthEffectVoid(byte x, byte y, byte newPlantId, byte animalId, byte statusEffectId,
            bool isKOED)
        {
            StartCoroutine(DoPlantGrowthEffect(x, y, newPlantId, animalId, statusEffectId, isKOED));
        }
        public IEnumerator DoPlantGrowthEffect(byte x, byte y, byte newPlantId, byte animalId, byte statusEffectId, bool isKOED)
        {
            if (x > 100 || y > 100) yield break;
            
            if (GameLogic.Instance.IsPlayerOne)
            {
                
                switch (newPlantId)
                {
                    case ConstantVars.ice_plant2_id:
                        if (plantObjects[(x, y)] != null)
                        {
                            Destroy(plantObjects[((int)x, (int)y)]);
                        }
                        GameObject iceplant2 = Instantiate(icePlant2, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        
                        plantObjects[(x, y)] = iceplant2;
                        CrystalController.Instance.UpdateLayerCrystal(plantObjects[(x,y)]);
                        StartCoroutine(PlayRingEffect(new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0)));
                        break;
                    case ConstantVars.poison_plant2_id:
                        if (plantObjects[(x, y)] != null)
                        {
                            Destroy(plantObjects[((int)x, (int)y)]);
                        }
                        GameObject poisonplant1 = Instantiate(poisonPlant2, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        StartCoroutine(PlayPoisonParticles(new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0)));
                        StartCoroutine(PlayRingEffect(new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0)));
                        plantObjects[(x, y)] = poisonplant1;
                        CrystalController.Instance.UpdateLayerCrystal(plantObjects[(x,y)]);
                        break;
                    case ConstantVars.poppy_plant2:
                        if (plantObjects[(x, y)] != null)
                        {
                            Destroy(plantObjects[((int)x, (int)y)]);
                        }
                        GameObject poppy2 = Instantiate(this.poppy2, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        StartCoroutine(PlayRingEffect(new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0)));
                        plantObjects[(x, y)] = poppy2;
                        poppyObjects[(x, y)] = poppy2;
                        CrystalController.Instance.UpdateLayerCrystal(plantObjects[(x,y)]);
                        break;
                    case ConstantVars.marigold2:
                        if (plantObjects[(x, y)] != null)
                        {
                            Destroy(plantObjects[((int)x, (int)y)]);
                        }
                        GameObject marigold2 = Instantiate(this.marigold2, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        StartCoroutine(PlayRingEffect(new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0)));
                        plantObjects[(x, y)] = marigold2;
                        CrystalController.Instance.UpdateLayerCrystal(plantObjects[(x,y)]);
                        break;
                    case ConstantVars.turrent_plant2:
                        if (plantObjects[(x, y)] != null)
                        {
                            Destroy(plantObjects[((int)x, (int)y)]);
                        }
                        GameObject turret2 = Instantiate(this.turretPlant2, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        StartCoroutine(PlayRingEffect(new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0)));
                        plantObjects[(x, y)] = turret2;
                        CrystalController.Instance.UpdateLayerCrystal(plantObjects[(x,y)]);
                        break;
                    case ConstantVars.dandelion2:
                        if (plantObjects[(x, y)] != null)
                        {
                            Destroy(plantObjects[((int)x, (int)y)]);
                        }
                        GameObject dandelion2 = Instantiate(this.dandelion2, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        StartCoroutine(PlayRingEffect(new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0)));
                        plantObjects[(x, y)] = dandelion2;
                        CrystalController.Instance.UpdateLayerCrystal(plantObjects[(x,y)]);
                        break;
                    case ConstantVars.shield2:
                        if (plantObjects[(x, y)] != null)
                        {
                            Destroy(plantObjects[((int)x, (int)y)]);
                        }
                        GameObject shield2 = Instantiate(this.shield2, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        StartCoroutine(PlayRingEffect(new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0)));
                        plantObjects[(x, y)] = shield2;
                        shieldObjects[(x, y)] = shield2;
                        CrystalController.Instance.UpdateLayerCrystal(plantObjects[(x,y)]);
                        break;
                    case ConstantVars.tadpole:
                        if (animalItems[(x, y)] != null)
                        {
                            Destroy(animalItems[((int)x, (int)y)]);
                        }
                        GameObject tadpole = Instantiate(this.tadpole, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        StartCoroutine(PlayRingEffect(new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0)));
                        animalItems[(x, y)] = tadpole;
                        CrystalController.Instance.UpdateLayerCrystal(animalItems[(x,y)]);
                        AnimalEffectController.Instance.tadpolesList.Add(tadpole);
                        break;
                    case ConstantVars.tadpole_on_dirt:
                        if (animalItems[(x, y)] != null)
                        {
                            Destroy(animalItems[((int)x, (int)y)]);
                        }
                        GameObject tadpole2 = Instantiate(this.tadpole, new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0), Quaternion.identity);
                        StartCoroutine(PlayRingEffect(new Vector3(x + Constants.hor_offset,
                            y + Constants.vert_offset, 0)));
                        animalItems[(x, y)] = tadpole2;
                        CrystalController.Instance.UpdateLayerCrystal(animalItems[(x,y)]);
                        AnimalEffectController.Instance.tadpolesList.Add(tadpole2);
                        break;

                }
                
            }
            else
            {
                int newX = GameLogic.flipX(x);
                int newY = GameLogic.flipY(y);

                switch (newPlantId)
                {
                    case ConstantVars.ice_plant2_id:
                        if (plantObjects[(newX, newY)] != null)
                        {
                            Destroy(plantObjects[(newX, newY)]);
                        }
                        GameObject iceplant1 = Instantiate(icePlant2, new Vector3(newX + Constants.hor_offset,
                            newY + Constants.vert_offset, 0), Quaternion.identity);
                        StartCoroutine(PlayRingEffect(new Vector3(newX + Constants.hor_offset,
                            newY + Constants.vert_offset, 0)));
                        plantObjects[(newX, newY)] = iceplant1;
                        CrystalController.Instance.UpdateLayerCrystal(plantObjects[(newX,newY)]);
                        break;
                    case ConstantVars.poison_plant2_id:
                        if (plantObjects[(newX, newY)] != null)
                        {
                            Destroy(plantObjects[(newX, newY)]);
                        }
                        GameObject poisonplant1 = Instantiate(poisonPlant2, new Vector3(newX + Constants.hor_offset,
                            newY + Constants.vert_offset, 0), Quaternion.identity);
                        StartCoroutine(PlayPoisonParticles(new Vector3(newX + Constants.hor_offset,
                            newY + Constants.vert_offset, 0)));
                        StartCoroutine(PlayRingEffect(new Vector3(newX + Constants.hor_offset,
                            newY + Constants.vert_offset, 0)));
                        plantObjects[(newX, newY)] = poisonplant1;
                        CrystalController.Instance.UpdateLayerCrystal(plantObjects[(newX,newY)]);
                        break;
                    case ConstantVars.poppy_plant2:
                        if (plantObjects[(newX, newY)] != null)
                        {
                            Destroy(plantObjects[(newX, newY)]);
                        }
                        GameObject poppy2 = Instantiate(this.poppy2, new Vector3(newX + Constants.hor_offset,
                            newY + Constants.vert_offset, 0), Quaternion.identity);
                        StartCoroutine(PlayRingEffect(new Vector3(newX + Constants.hor_offset,
                            newY + Constants.vert_offset, 0)));
                        plantObjects[(newX, newY)] = poppy2;
                        poppyObjects[(newX, newY)] = poppy2;
                        CrystalController.Instance.UpdateLayerCrystal(plantObjects[(newX,newY)]);
                        break;
                    case ConstantVars.marigold2:
                        if (plantObjects[(newX, newY)] != null)
                        {
                            Destroy(plantObjects[(newX, newY)]);
                        }
                        GameObject marigold2 = Instantiate(this.marigold2, new Vector3(newX + Constants.hor_offset,
                            newY + Constants.vert_offset, 0), Quaternion.identity);
                        StartCoroutine(PlayRingEffect(new Vector3(newX + Constants.hor_offset,
                            newY + Constants.vert_offset, 0)));
                        plantObjects[(newX, newY)] = marigold2;
                        CrystalController.Instance.UpdateLayerCrystal(plantObjects[(newX,newY)]);
                        break;
                    case ConstantVars.turrent_plant2:
                        //if (statusEffectId != (byte)(StatusEffect.TURRET))
                        //{
                            if (plantObjects[(newX, newY)] != null)
                            {
                                Destroy(plantObjects[(newX, newY)]);
                            }
                            GameObject turret2 = Instantiate(this.turretPlant2, new Vector3(newX + Constants.hor_offset,
                                newY + Constants.vert_offset, 0), Quaternion.identity);
                            StartCoroutine(PlayRingEffect(new Vector3(newX + Constants.hor_offset,
                                newY + Constants.vert_offset, 0)));
                            plantObjects[(newX, newY)] = turret2;
                        CrystalController.Instance.UpdateLayerCrystal(plantObjects[(newX,newY)]);
                       // }
                        break;
                    case ConstantVars.dandelion2:
                        if (plantObjects[(newX, newY)] != null)
                        {
                            Destroy(plantObjects[(newX, newY)]);
                        }
                        GameObject dandelion2 = Instantiate(this.dandelion2, new Vector3(newX + Constants.hor_offset,
                            newY + Constants.vert_offset, 0), Quaternion.identity);
                        StartCoroutine(PlayRingEffect(new Vector3(newX + Constants.hor_offset,
                            newY + Constants.vert_offset, 0)));
                        plantObjects[(newX, newY)] = dandelion2;
                        CrystalController.Instance.UpdateLayerCrystal(plantObjects[(newX,newY)]);
                        break;
                    case ConstantVars.shield2:
                        if (plantObjects[(newX, newY)] != null)
                        {
                            Destroy(plantObjects[(newX, (int)newY)]);
                        }
                        GameObject shield2 = Instantiate(this.shield2, new Vector3(newX + Constants.hor_offset,
                            newY + Constants.vert_offset, 0), Quaternion.identity);
                        StartCoroutine(PlayRingEffect(new Vector3(newX + Constants.hor_offset,
                            newY + Constants.vert_offset, 0)));
                        plantObjects[(newX, newX)] = shield2;
                        shieldObjects[(newX, newY)] = shield2;
                        CrystalController.Instance.UpdateLayerCrystal(plantObjects[(newX,newY)]);
                        break;
                    case ConstantVars.tadpole:
                        if (animalItems[(newX, newY)] != null)
                        {
                            Destroy(animalItems[(newX, newY)]);
                            AnimalEffectController.Instance.tadpolesList.Remove(animalItems[(newX, newY)]);
                        }
                        GameObject tadpole = Instantiate(this.tadpole, new Vector3(newX + Constants.hor_offset,
                            newY + Constants.vert_offset, 0), Quaternion.identity);
                        StartCoroutine(PlayRingEffect(new Vector3(newX + Constants.hor_offset,
                            newY + Constants.vert_offset, 0)));
                        animalItems[(newX, newY)] = tadpole;
                        CrystalController.Instance.UpdateLayerCrystal(animalItems[(newX,newY)]);
                        AnimalEffectController.Instance.tadpolesList.Add(tadpole);
                        break;
                    case ConstantVars.tadpole_on_dirt:
                        if (animalItems[(newX, newY)] != null)
                        {
                            Destroy(animalItems[(newX, newY)]);
                            AnimalEffectController.Instance.tadpolesList.Remove(animalItems[(newX, newY)]);
                        }
                        GameObject tadpole2 = Instantiate(this.tadpole, new Vector3(newX + Constants.hor_offset,
                            newY + Constants.vert_offset, 0), Quaternion.identity);
                        StartCoroutine(PlayRingEffect(new Vector3(newX + Constants.hor_offset,
                            newY + Constants.vert_offset, 0)));
                        animalItems[(newX, newY)] = tadpole2;
                        CrystalController.Instance.UpdateLayerCrystal(animalItems[(newX,newY)]);
                        AnimalEffectController.Instance.tadpolesList.Add(tadpole2);
                        break;

                }
            }

            yield return CreatePlantRoutine(statusEffectId, animalId, x, y, isKOED);
        }

        private IEnumerator CreatePlantRoutine(byte statusEffectId, byte animalId, byte x, byte y, bool isKOED)
        {
            if (statusEffectId != 0)
            {
                GameEventRoutine gameEventParams = new GameEventRoutine();
                gameEventParams.statusEffectId = statusEffectId;
                gameEventParams.animalId = animalId;
                gameEventParams.targetId = -1;
                gameEventParams.x = x;
                gameEventParams.y = y;
                gameEventParams.gameEvent = GameEvent.EndTurnEffects;
                gameEventParams.isKOed = isKOED;
                gameEventParams.execute = PlantEffectRoutine;
                GameEventRoutineManager.Instance.AddRoutine(gameEventParams);
            }

            yield return null;
        }
        public IEnumerator DoPlantEffects(int animalId, byte statusEffectId, int x, int y, bool isKed)
        {
            if(animalId < 0 || animalId > 5) yield break;
            GameObject animal = AttacksController.Instance.GetAnimal(animalId);
            switch (statusEffectId)
            {
                case (byte)StatusEffect.POISON:
                    
                    yield return DoPoisonEffect(animal);
                    break;
                case (byte) StatusEffect.FROZEN:
                    yield return DoFrozenEffect(animal);
                    break;
                case (byte) StatusEffect.THAW:
                    yield return DoThawEffect(animal);
                    break;
                case (byte) StatusEffect.TURRET:
                    yield return DoTurretShot(x, y, animal, isKed, (byte)animalId);
                    break;
                default:
                    break;
                
            }

            yield return null;
        }
        public IEnumerator PlantEffectRoutine(GameEventParams gameEventParams)
        {
            int animalId = gameEventParams.animalId;
            int x = gameEventParams.x;
            int y = gameEventParams.y;
            byte statusEffectId = gameEventParams.statusEffectId;
            bool isKOED = gameEventParams.isKOed;
            yield return DoPlantEffects(animalId, statusEffectId, x, y, isKOED);
        }

        private IEnumerator PlayRingEffect(Vector3 location)
        {
            GameObject go = Instantiate(ringDisplay, location, quaternion.identity);
            yield return new WaitForSeconds(1);
            Destroy(go);
            yield return null;
        }

        private IEnumerator PlayPoisonParticles(Vector3 location)
        {
            GameObject poisonParticles = Instantiate(this.poisonParticles, new Vector3(
                location.x,location.y , 0), Quaternion.identity);
            yield return new WaitForSeconds(1);
            poisonParticles.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
            yield return new WaitForSeconds(1);
            Destroy(poisonParticles);
            yield return null;
        }

        public IEnumerator DoFrozenEffect(GameObject animal)
        {
            Vector3 animalPos = animal.transform.position;
            Color blue = new Color(.5f, .6f, 1, 1);
            SpriteRenderer sr = animal.transform.GetChild(1).GetComponent<SpriteRenderer>();
            GameObject particles = Instantiate(frozenEffect, new Vector3(animalPos.x, animalPos.y, 0),
                Quaternion.identity);
            StartCoroutine(FadeColor(Color.white, blue, sr, .8f));
            yield return new WaitForSeconds(1.5f);
            Destroy(particles);
            
            yield return null;
        }

        public IEnumerator DoThawEffect(GameObject animal)
        {
            SpriteRenderer sr = animal.transform.GetChild(1).GetComponent<SpriteRenderer>();
            Color blue = new Color(.5f, .6f, 1, 1);
            StartCoroutine(FadeColor(blue, Color.white, sr, .8f));
            yield return null;
        }

        public IEnumerator DoPoppyEffect()
        {
            List<GameObject> objectsToDestroy = new List<GameObject>();
            foreach (GameObject poppy in poppyObjects.Values)
            {
                if(poppy == null) continue;
                GameObject go = Instantiate(poppyEffect, poppy.transform.position, quaternion.identity);
                objectsToDestroy.Add(go);
            }

            yield return new WaitForSeconds(1.5f);
            foreach (GameObject effect in objectsToDestroy)
            {
                Destroy(effect);
            }
            yield return null;
        }
        public IEnumerator DoShieldEffect()
        {
            List<GameObject> objectsToDestroy = new List<GameObject>();
            foreach (GameObject shield in shieldObjects.Values)
            {
                if(shield == null) continue;
                GameObject go = Instantiate(shieldEffect, shield.transform.position, quaternion.identity);
                objectsToDestroy.Add(go);
            }

            yield return new WaitForSeconds(1.5f);
            foreach (GameObject effect in objectsToDestroy)
            {
                Destroy(effect);
            }
            yield return null;
        }

        public IEnumerator DoTurretShot(int plantX, int plantY, GameObject animal, bool isKOED, byte animalId)
        {
            
            GameObject turretPlant;
            if (GameLogic.Instance.IsPlayerOne)
            {
                turretPlant = plantObjects[(plantX, plantY)];
                
            }
            else
            {
                turretPlant = plantObjects[(GameLogic.flipX(plantX), GameLogic.flipY(plantY))];
            }
              
            Vector3 animalPos = animal.transform.position;
            Vector3 plantLoc = turretPlant.transform.position;
            Vector3 firePos = new Vector3(plantLoc.x, plantLoc.y + .2f, 0);
            turretPlant.GetComponent<Animator>()?.Play("TurretShootAnimation");
            GameObject bullet = Instantiate(turretShot, firePos, Quaternion.identity);
            yield return AttacksController.Instance.MoveToPosition(
                bullet, animalPos, .15f*Vector3.Distance(animalPos, bullet.transform.position), true);
            StartCoroutine(AttacksController.Instance.SpawnHealthText(
                Mathf.RoundToInt(animalPos.x), Mathf.RoundToInt(animalPos.y), 20, Color.red));
            if (isKOED)
            {
                StartCoroutine(HealthController.Instance.OnDeadAnimal(animal, animalId));
            }

            
            yield return new WaitForSeconds(.8f);
            Destroy(bullet);
            
            

            yield return null;
        }
        

        public IEnumerator DoPoisonEffect(GameObject animal)
        {
            
                Color purple = new Color(.78f, 0, 1f);
                Vector3 animalPos = animal.transform.position;
                StartCoroutine(PlayPoisonParticles(animalPos));

                SpriteRenderer sr = animal.transform.GetChild(1).GetComponent<SpriteRenderer>();
                StartCoroutine(FadeColor(Color.white, purple, sr, .4f));
                yield return new WaitForSeconds(.4f);
                StartCoroutine(FadeColor(purple, Color.white, sr, .2f));
                yield return new WaitForSeconds(.2f);
                StartCoroutine(FadeColor(Color.white, purple, sr, .4f));
                yield return new WaitForSeconds(.4f);
                StartCoroutine(FadeColor(purple, Color.white, sr, .2f));
                yield return new WaitForSeconds(.2f);
            
            
            
            yield return null;
        }
        
        private IEnumerator FadeColor(Color startColor, Color endColor, SpriteRenderer sr,float fadeDuration)
        {
            float elapsedTime = 0f;
        
            while (elapsedTime < fadeDuration)
            {
                Color lerpedColor = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
                sr.color = lerpedColor;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            sr.color = endColor;
        }

        public IEnumerator ReplacePlantsWithDirt()
        {
            List<GameObject> objects = new List<GameObject>(plantObjects.Values);
            foreach(var plant in objects)
            {
                if(plant == null) continue;
                var position = plant.transform.position;
                SpawnDirtTile(Mathf.RoundToInt(position.x) - Constants.hor_offset, 
                    Mathf.RoundToInt(position.y)- Constants.vert_offset, true);
            }
            
            
            plantObjects.Clear();
            yield return null;
        }
    }
}