using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Server.Game;
using SharedLibrary.Objects;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public class CrystalController : MonoBehaviour
    {
        private static CrystalController instance;
        public static CrystalController Instance
    
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<CrystalController>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("CrystalController");
                        instance = go.AddComponent<CrystalController>();
                    }
                }
                return instance;
            }
        }

        public GameObject fireSpiritPrefab;
        [SerializeField] private GameObject fireCrystalPrefab,
            commonCrystalPrefab, uncommonCrystalPrefab, rareCrystalPrefab, pickupEffectPrefab,
            waterCrystalPrefab, lifeCrystalPrefab, decayCrystalPrefab, waterSpiritPrefab, lifeSpiritPrefab,
            decaySpiritPrefab;
        private const int vert_offset = -6;
        private const int hor_offset = -4;
        private int fireCount= 0, waterCount = 0, lifeCount= 0, decayCount = 0;
        public void SpawnCrystal(byte crystalId, byte crystalKey, int x, int y)
        {
            if(!GameLogic.Instance.gameStarted) return;
            switch (crystalId)
            {
                case 0: //nothing
                    break;
                case 1://fire crystal
                    GameObject go = Instantiate(fireCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = go;
                    UpdateLayerCrystal(GameLogic.Instance.CrystalMap[crystalKey]);
                    break;
                case 2://water crystal
                    GameObject wc = Instantiate(waterCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = wc;
                    UpdateLayerCrystal(GameLogic.Instance.CrystalMap[crystalKey]);
                    break;
                case 3://water crystal
                    GameObject lc = Instantiate(lifeCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = lc;
                    UpdateLayerCrystal(GameLogic.Instance.CrystalMap[crystalKey]);
                    break;
                case 4://water crystal
                    GameObject dc = Instantiate(decayCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = dc;
                    UpdateLayerCrystal(GameLogic.Instance.CrystalMap[crystalKey]);
                    break;
                case 5:
                    GameObject burn3crystal = Instantiate(commonCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = burn3crystal;
                    UpdateLayerCrystal(GameLogic.Instance.CrystalMap[crystalKey]);
                    break;
                case 6:
                    GameObject drown3crystal = Instantiate(commonCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = drown3crystal;
                    UpdateLayerCrystal(GameLogic.Instance.CrystalMap[crystalKey]);
                    break;
                case 7:
                    GameObject breath3crystal = Instantiate(commonCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = breath3crystal;
                    UpdateLayerCrystal(GameLogic.Instance.CrystalMap[crystalKey]);
                    break;
                case 8:
                    GameObject decay3crystal = Instantiate(commonCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = decay3crystal;
                    UpdateLayerCrystal(decay3crystal);
                    break;
                case 26:
                    GameObject burn5crystal = Instantiate(commonCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = burn5crystal;
                    UpdateLayerCrystal(GameLogic.Instance.CrystalMap[crystalKey]);
                    break;
                case 27:
                    GameObject drown5crystal = Instantiate(commonCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = drown5crystal;
                    UpdateLayerCrystal(GameLogic.Instance.CrystalMap[crystalKey]);
                    break;
                case 28:
                    GameObject breath5crystal = Instantiate(commonCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = breath5crystal;
                    UpdateLayerCrystal(GameLogic.Instance.CrystalMap[crystalKey]);
                    break;
                case 29:
                    GameObject decay5crystal = Instantiate(commonCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = decay5crystal;
                    UpdateLayerCrystal(GameLogic.Instance.CrystalMap[crystalKey]);
                    break;
                case 51:
                    GameObject burn10crystal = Instantiate(rareCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = burn10crystal;
                    UpdateLayerCrystal(GameLogic.Instance.CrystalMap[crystalKey]);
                    break;
                case 52:
                    GameObject drown10crystal = Instantiate(rareCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = drown10crystal;
                    UpdateLayerCrystal(GameLogic.Instance.CrystalMap[crystalKey]);
                    break;
                case 53:
                    GameObject breath10crystal = Instantiate(rareCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = breath10crystal;
                    UpdateLayerCrystal(GameLogic.Instance.CrystalMap[crystalKey]);
                    break;
                case 54:
                    GameObject decay10crystal = Instantiate(rareCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = decay10crystal;
                    UpdateLayerCrystal(GameLogic.Instance.CrystalMap[crystalKey]);
                    break;
                case 101:
                    GameObject dirt_tile = Instantiate(PlantController.Instance.dirt, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = dirt_tile;
                    UpdateLayerCrystal(GameLogic.Instance.CrystalMap[crystalKey]);
                    break;
                default:
                    break;
            }
            
        }

        private void PlayCrystalPickupEffect(int x, int y)
        {
            
            GameObject particles = Instantiate(pickupEffectPrefab, new Vector3(x+hor_offset, y+vert_offset, 0), Quaternion.identity);
            Destroy(particles, 1f);
        }

        public void DestroyChildrenParticles(GameObject animal)
        {
            int childCount = animal.transform.GetChild(1).childCount;
            List<GameObject> objectsToDestroy = new List<GameObject>();
            for (int i = 0; i < childCount; i++)
            {
                objectsToDestroy.Add(animal.transform.GetChild(1).GetChild(i).gameObject);
            }

            foreach (GameObject obj in objectsToDestroy)
            {
                obj.transform.SetParent(null);
                Destroy(obj);
            }
        }
        public void PickupCrystal(int animalId, byte crystalId, byte crystalKey, int x, int y, int addedHealth, bool spawnDirt, int tX, int tY)
        {
            
            GameObject animal = GetAnimalNoSwitch(animalId);
            Vector3 animalPos = animal.transform.position;
            Debug.Log(crystalId + " < - crystal ID");
            switch (crystalId)
            {
                case 0 :
                    break;
                case 1:
                    if (GameLogic.Instance.GameAnimals[animalId].WaterCount > 0 || GameLogic.Instance.GameAnimals[animalId].LifeCount > 0 
                        ||  GameLogic.Instance.GameAnimals[animalId].DecayCount > 0)
                    {
                        DestroyChildrenParticles(animal);
                    }

                    
                        GameObject fireSpirit = Instantiate(fireSpiritPrefab, animal.transform.position, Quaternion.identity);
                        fireSpirit.GetComponent<CircularMotion>().centerPoint = animal;
                        GameLogic.Instance.GameAnimals[animalId].FireCount +=1;
                    
                    
                    GameLogic.Instance.GameAnimals[animalId].WaterCount = 0;
                    GameLogic.Instance.GameAnimals[animalId].LifeCount = 0;
                    GameLogic.Instance.GameAnimals[animalId].DecayCount = 0;
                    PlayCrystalPickupEffect(x, y);
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    break;
                case 2:
                    if (GameLogic.Instance.GameAnimals[animalId].FireCount > 0 || GameLogic.Instance.GameAnimals[animalId].LifeCount > 0 
                        ||  GameLogic.Instance.GameAnimals[animalId].DecayCount > 0)
                    {
                        DestroyChildrenParticles(animal);
                    }

                    
                        GameObject waterSpirit = Instantiate(waterSpiritPrefab, animal.transform.position, Quaternion.identity);
                        waterSpirit.GetComponent<CircularMotion>().centerPoint = animal;
                        GameLogic.Instance.GameAnimals[animalId].WaterCount +=1;
                    

                   
                    GameLogic.Instance.GameAnimals[animalId].FireCount = 0;
                    
                    GameLogic.Instance.GameAnimals[animalId].LifeCount = 0;
                    GameLogic.Instance.GameAnimals[animalId].DecayCount = 0;
                    PlayCrystalPickupEffect(x, y);
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    break;
                case 3:
                    if (GameLogic.Instance.GameAnimals[animalId].FireCount > 0 || GameLogic.Instance.GameAnimals[animalId].WaterCount > 0 
                         ||  GameLogic.Instance.GameAnimals[animalId].DecayCount > 0)
                    {
                        DestroyChildrenParticles(animal);
                    }

                   
                        GameObject lifeSpirit = Instantiate(lifeSpiritPrefab, animal.transform.position, Quaternion.identity);
                        lifeSpirit.GetComponent<CircularMotion>().centerPoint = animal;
                        GameLogic.Instance.GameAnimals[animalId].LifeCount +=1;
                    

                    
                    GameLogic.Instance.GameAnimals[animalId].FireCount = 0;
                    GameLogic.Instance.GameAnimals[animalId].WaterCount = 0;
                    GameLogic.Instance.GameAnimals[animalId].DecayCount = 0;
                    PlayCrystalPickupEffect(x, y);
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    break;
                case 4:
                    if (GameLogic.Instance.GameAnimals[animalId].FireCount > 0 || GameLogic.Instance.GameAnimals[animalId].WaterCount > 0 
                                                                               ||  GameLogic.Instance.GameAnimals[animalId].LifeCount > 0)
                    {
                        DestroyChildrenParticles(animal);
                    }

                    
                        GameObject decaySpirit = Instantiate(decaySpiritPrefab, animal.transform.position, Quaternion.identity);
                        decaySpirit.GetComponent<CircularMotion>().centerPoint = animal;
                        GameLogic.Instance.GameAnimals[animalId].DecayCount +=1;
                    
                  
                    GameLogic.Instance.GameAnimals[animalId].FireCount = 0;
                    GameLogic.Instance.GameAnimals[animalId].WaterCount = 0;
                    GameLogic.Instance.GameAnimals[animalId].LifeCount = 0;
                    
                    PlayCrystalPickupEffect(x, y);
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    break;
                case 5:
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    PlayCrystalPickupEffect(x, y);
                    break;
                case 6:
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    PlayCrystalPickupEffect(x, y);
                    break;
                case 7:
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    PlayCrystalPickupEffect(x, y);
                    break;
                case 8:
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    PlayCrystalPickupEffect(x, y);
                    break;
                case 26:
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    PlayCrystalPickupEffect(x, y);
                    break;
                case 27:
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    PlayCrystalPickupEffect(x, y);
                    break;
                case 28:
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    PlayCrystalPickupEffect(x, y);
                    break;
                case 29:
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    PlayCrystalPickupEffect(x, y);
                    break;
                case 51:
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    PlayCrystalPickupEffect(x, y);
                    break;
                case 52:
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    PlayCrystalPickupEffect(x, y);
                    break;
                case 53:
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    PlayCrystalPickupEffect(x, y);
                    break;
                case 54:
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    PlayCrystalPickupEffect(x, y);
                    break;
                case 100:
                    AnimalEffectController.Instance.eggs.Remove(GameLogic.Instance.CrystalMap[crystalKey]);
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    PlayCrystalPickupEffect(x, y);
                   //StartCoroutine(AttacksController.Instance.SpawnHealthText((int)animalPos.x, (int)animalPos.y, addedHealth, Color.green));
                    break;
                case ConstantVars.egg_on_dirt_tile:
                    AnimalEffectController.Instance.eggs.Remove(GameLogic.Instance.CrystalMap[crystalKey]);
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    PlayCrystalPickupEffect(x, y);
                    //StartCoroutine(AttacksController.Instance.SpawnHealthText((int)animalPos.x, (int)animalPos.y, addedHealth, Color.green));
                    break;
                case ConstantVars.clamshell:
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    AnimalEffectController.Instance.shellList.Remove(GameLogic.Instance.CrystalMap[crystalKey]);
                    PlayCrystalPickupEffect(x, y);
                    //StartCoroutine(AttacksController.Instance.SpawnHealthText((int)animalPos.x, (int)animalPos.y, addedHealth, Color.green));
                    break;
                case ConstantVars.clamshell_on_dirt:
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    AnimalEffectController.Instance.shellList.Remove(GameLogic.Instance.CrystalMap[crystalKey]);
                    PlayCrystalPickupEffect(x, y);
                    //StartCoroutine(AttacksController.Instance.SpawnHealthText((int)animalPos.x, (int)animalPos.y, addedHealth, Color.green));
                    break;
                case ConstantVars.bunnyegg:
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    AnimalEffectController.Instance.bunnyEggs.Remove(GameLogic.Instance.CrystalMap[crystalKey]);
                    PlayCrystalPickupEffect(x, y);
                    //StartCoroutine(AttacksController.Instance.SpawnHealthText((int)animalPos.x, (int)animalPos.y, addedHealth, Color.green));
                    break;
                case ConstantVars.bunnyegg_on_dirt:
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    AnimalEffectController.Instance.bunnyEggs.Remove(GameLogic.Instance.CrystalMap[crystalKey]);
                    PlayCrystalPickupEffect(x, y);
                    //StartCoroutine(AttacksController.Instance.SpawnHealthText((int)animalPos.x, (int)animalPos.y, addedHealth, Color.green));
                    break;
                case ConstantVars.dogbone:
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    GameLogic.Instance.CrystalMap.Remove(crystalKey);
                    PlayCrystalPickupEffect(x, y);
                    break;
                case ConstantVars.dogbone_on_dirt:
                    Destroy(GameLogic.Instance.CrystalMap[crystalKey]);
                    GameLogic.Instance.CrystalMap.Remove(crystalKey);
                    PlayCrystalPickupEffect(x, y);
                    break;
                case ConstantVars.teddy_bear:
                    PlayCrystalPickupEffect(x, y);
                    StartCoroutine(AnimalEffectController.Instance.PickupTeddy(animal, animalId, tX, tY));
                    break;
                case ConstantVars.teddy_bear_on_dirt:
                    PlayCrystalPickupEffect(x, y);
                    StartCoroutine(AnimalEffectController.Instance.PickupTeddy(animal, animalId, tX, tY));
                    break;

                
                default:
                    break;
            }

            if (addedHealth > 0)
            {
                StartCoroutine(AttacksController.Instance.SpawnHealthText((int)animalPos.x, (int)animalPos.y, addedHealth, Color.green));
            }

            if (spawnDirt)
            {
                PlantController.Instance.SpawnDirtTile(x, y, false);
            }
        }

        public GameObject GetAnimalNoSwitch(int animalId)
        {
            if(animalId < GameLogic.Instance.numberOfAnimals/2) return GameLogic.Instance.myAnimals[animalId];
            return GameLogic.Instance.enemyAnimals[animalId - GameLogic.Instance.numberOfAnimals/2];
        }

        public void UpdateLayerAnimal(GameObject go)
        {
            go.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 
                -Mathf.RoundToInt(go.transform.position.y);
        }
        public void UpdateLayerAnimalSpecific(GameObject go,int order)
        {
            go.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder =
                order;
        }
        
        public void UpdateLayerCrystal(GameObject go)
        {
            SpriteRenderer sr = go.transform.GetChild(0).GetComponent<SpriteRenderer>();
            sr.sortingOrder = 
                -Mathf.RoundToInt(go.transform.position.y);
            sr.sortingLayerName = "Animals";
        }
    }
}