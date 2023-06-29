using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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

        [SerializeField] private GameObject fireCrystalPrefab, fireSpiritPrefab,
            commonCrystalPrefab, uncommonCrystalPrefab, rareCrystalPrefab, pickupEffectPrefab,
            waterCrystalPrefab, lifeCrystalPrefab, decayCrystalPrefab;
        private const int vert_offset = -6;
        private const int hor_offset = -4;
        public void SpawnCrystal(byte crystalId, byte crystalKey, int x, int y)
        {
            switch (crystalId)
            {
                case 0: //nothing
                    break;
                case 1://fire crystal
                    GameObject go = Instantiate(fireCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = go;
                    break;
                case 2://water crystal
                    GameObject wc = Instantiate(waterCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = wc;
                    break;
                case 3://water crystal
                    GameObject lc = Instantiate(lifeCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = lc;
                    break;
                case 4://water crystal
                    GameObject dc = Instantiate(decayCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = dc;
                    break;
                case 5:
                    GameObject burn3crystal = Instantiate(commonCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = burn3crystal;
                    break;
                case 6:
                    GameObject drown3crystal = Instantiate(commonCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = drown3crystal;
                    break;
                case 7:
                    GameObject breath3crystal = Instantiate(commonCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = breath3crystal;
                    break;
                case 8:
                    GameObject decay3crystal = Instantiate(commonCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = decay3crystal;
                    break;
                case 26:
                    GameObject burn5crystal = Instantiate(uncommonCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = burn5crystal;
                    break;
                case 27:
                    GameObject drown5crystal = Instantiate(uncommonCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = drown5crystal;
                    break;
                case 28:
                    GameObject breath5crystal = Instantiate(uncommonCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = breath5crystal;
                    break;
                case 29:
                    GameObject decay5crystal = Instantiate(uncommonCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = decay5crystal;
                    break;
                case 51:
                    GameObject burn10crystal = Instantiate(rareCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = burn10crystal;
                    break;
                case 52:
                    GameObject drown10crystal = Instantiate(rareCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = drown10crystal;
                    break;
                case 53:
                    GameObject breath10crystal = Instantiate(rareCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = breath10crystal;
                    break;
                case 54:
                    GameObject decay10crystal = Instantiate(rareCrystalPrefab, new Vector3((float)x, (float)y, 0),
                        Quaternion.identity);
                    GameLogic.Instance.CrystalMap[crystalKey] = decay10crystal;
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

        public void PickupCrystal(int animalId, byte crystalId, byte crystalKey, bool fullCrystal, int x, int y)
        {
            switch (crystalId)
            {
                case 0 :
                    break;
                case 1:
                    GameObject animal = GetAnimalNoSwitch(animalId);
                    GameObject fireSpirit = Instantiate(fireSpiritPrefab, animal.transform.position, Quaternion.identity);
                    fireSpirit.GetComponent<CircularMotion>().centerPoint = animal;
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
                
                default:
                    break;
            }
        }

        private GameObject GetAnimalNoSwitch(int animalId)
        {
            switch (animalId)
            {
                case 0:
                    return GameLogic.Instance.myAnimal0;
                case 1:
                    return GameLogic.Instance.myAnimal1;
                case 2:
                    return GameLogic.Instance.myAnimal2;
                case 3:
                    return GameLogic.Instance.enemyAnimal0;
                case 4:
                    return GameLogic.Instance.enemyAnimal1;
                case 5:
                    return GameLogic.Instance.enemyAnimal2;
            }

            return null;
        }
    }
}