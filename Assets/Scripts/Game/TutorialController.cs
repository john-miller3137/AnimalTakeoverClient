using System;
using System.Collections;
using Server.Game;
using SharedLibrary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class TutorialController : MonoBehaviour
    {
        private static TutorialController instance;

        public static TutorialController Instance

        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<TutorialController>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("TutorialController");
                        instance = go.AddComponent<TutorialController>();
                    }
                }

                return instance;
            }
        }
        private GameObject realA2, realC, realOC, realCamera, tempCanvas;
        private BoxCollider2D bc;
        private SpriteRenderer tempCircleSR,tempOCSR;
        public GameObject gameScene;
        private CameraShake _shake;
        public void ResetLinkage()
        {
            GameLogic.Instance._shake = _shake;
            GameController.Instance.canvas = tempCanvas;
            GameLogic.Instance.camera = realCamera;
            GameLogic.Instance.button = realC;
            GameLogic.Instance.outerCircle = realOC;
            GameLogic.Instance.buttonCollider = GameLogic.Instance.button.GetComponent<BoxCollider2D>();
            GameLogic.Instance.myAnimal2 = realA2;
            GameLogic.Instance.bc2 = bc;
            GameController.Instance.sr2 = tempCircleSR;
            GameController.Instance.sr = tempOCSR;
            
            
        }
        
        private async void Start()
        {
            //await InputLogic.Instance.WaitForSceneLoads();
            //await InputLogic.Instance.WaitForSetup();
            
            if (InputLogic.Instance.isTutorial)
            {
                GameLogic.Instance.isMyTurn = true;

                Debug.Log("is tutorial;");
                if (GameLogic.Instance.gameObjects != null)
                {
                    Debug.Log("gameobjects not null unloading gamescene");
                    
                }
                GameLogic.Instance.gameObjects.SetActive(false);
                if (InputLogic.Instance.menuScene != null)
                {
                    Debug.Log("gameobjects not null unloading mainmenu");
                    
                }
                InputLogic.Instance.menuScene.SetActive(false);
                if (TutorialController.Instance.scene != null)
                {
                    TutorialController.Instance.scene.SetActive(true);
                }

                
                
                realC = GameLogic.Instance.button;
                realOC = GameLogic.Instance.outerCircle;
                tempCircleSR = GameController.Instance.sr2;
                tempOCSR = GameController.Instance.sr;

                realCamera = GameLogic.Instance.camera;
                
                GameLogic.Instance.button = innerCircle;
                GameLogic.Instance.buttonCollider = GameLogic.Instance.button.GetComponent<BoxCollider2D>();
                GameLogic.Instance.outerCircle = outerCircle;
                GameController.Instance.sr2 = innerCircle.GetComponent<SpriteRenderer>();
                GameController.Instance.sr = outerCircle.GetComponent<SpriteRenderer>();
                realA2 = GameLogic.Instance.myAnimal2;
                GameLogic.Instance.camera = camera;
                GameLogic.Instance._shake = camera.GetComponent<CameraShake>();
                bc = GameLogic.Instance.bc2;
                GameLogic.Instance.myAnimal2 = cat;
                GameLogic.Instance.bc2 = GameLogic.Instance.myAnimal2.GetComponent<BoxCollider2D>();
                tempCanvas = GameController.Instance.canvas; 
                GameController.Instance.canvas = canvas;
            }
            else
            {
                if (TutorialController.Instance.scene != null)
                {
                    TutorialController.Instance.scene.SetActive(false);
                }

                if (GameLogic.Instance.gameObjects != null)
                {
                    GameLogic.Instance.gameObjects.SetActive(false);
                }

                if (InputLogic.Instance.menuScene != null)
                {
                    InputLogic.Instance.menuScene.SetActive(true);
                }
            }
            if (cat != null)
            {
                catCollider = cat.GetComponent<BoxCollider2D>();
            }

            if (tapCatText != null)
            {
                tapCatText.SetActive(true);
            }

            if (tapSquareText != null)
            {
                tapSquareText.SetActive(false);
            }
            
            seedsPlanted = false;
            attackLaunched = false;
            pickedUpSeeds = false;
            pickedUpFire = false;
            hasFire = false;
        }

        public GameObject tapCatText, tapSquareText, cat, fireCrystal, seedCrystal, moveAwayText, pickupCrystalsText,
            attackText, tutorialCompleteText, plantSeeds, fireSpirit, target, plantInfo;
        private BoxCollider2D catCollider;
        private bool seedsPlanted, pickedUpSeeds, hasFire, pickedUpFire;
        public bool hasMoved, attackLaunched, hasTapped, moveAwayActive;
        public GameObject scene, innerCircle, outerCircle, camera;
        private GameObject dirt, plant;

        public GameObject canvas;

        private void Update()
        {
            if (!InputLogic.Instance.isTutorial) return; 
            
            if (hasMoved)
            {
                hasMoved = false;
                if (tapSquareText != null)
                {
                    tapSquareText.SetActive(false);
                }
                if (!pickedUpFire || !pickedUpSeeds && !pickedUpSeeds)
                {
                    
                    pickupCrystalsText.SetActive(true);
                } 
                
            }

            if (cat != null)
            {
                if (!pickedUpSeeds && Mathf.RoundToInt(cat.transform.position.x) == 0 && Mathf.RoundToInt(cat.transform.position.y) == -5)
                {
                    pickedUpSeeds = true;
                    PickupSeedCrystal();
                }
                if (!pickedUpFire && cat.transform.position.x == 2 && cat.transform.position.y == 5)
                {
                    pickedUpFire = true;
                    hasFire = true;
                    PickupFireCrystal();
                }
            }

        }

        private void PickupSeedCrystal()
        {
            InventoryController.Instance.AddItemToInventoryVoid((int)ItemId.POISON_SEED, 0, -5);
            moveAwayActive = true;
            moveAwayText.SetActive(true);
            attackText.SetActive(false);
            seedCrystal.SetActive(false);
            pickupCrystalsText.SetActive(false);
            PlantController.Instance.SpawnDirtTile(0-Constants.hor_offset, -5-Constants.vert_offset, false);
        }
        private void PickupFireCrystal()
        {
            moveAwayText.SetActive(false);
            plantSeeds.SetActive(false);
            fireSpirit = Instantiate(CrystalController.Instance.fireSpiritPrefab, cat.transform.position, Quaternion.identity);
            fireSpirit.GetComponent<CircularMotion>().centerPoint = cat;
            pickupCrystalsText.SetActive(false);
            attackText.SetActive(true);
            Destroy(fireCrystal);
            
        }

        public void PlantSeeds()
        {
            plant = Instantiate(PlantController.Instance.poisonPlant1, new Vector3(0,
                -5, 0), Quaternion.identity);
            InventoryController.Instance.DoRemoveItemFromInventory((int)ItemId.POISON_SEED);
            plantSeeds.SetActive(false);
            seedsPlanted = true;
            moveAwayText.SetActive(false);
            if (attackLaunched)
            {
                tutorialCompleteText.SetActive(true);
                InputLogic.Instance.tutComplete = true;
            }
            else
            {
                if (hasFire)
                {
                    attackText.SetActive(true);
                }
                else
                {
                    pickupCrystalsText.SetActive(true);
                }
            }
            
            
        }

        public void LaunchGame()
        {
            Destroy(plant);
            scene.SetActive(false);
            InputLogic.Instance.isTutorial = false;
            foreach (GameObject go in PlantController.Instance.dirtTiles.Values)
            {
                Destroy(go);
            }
            PlantController.Instance.dirtTiles.Clear();
            InputLogic.Instance.menuScene.SetActive(true);
            PlayerPrefs.SetInt("tutorial", 1);
            ResetLinkage();
            GameLogic.Instance.isMyTurn = false;
        }

        public IEnumerator LaunchFireball()
        {
            if (hasFire)
            {
                if (seedsPlanted)
                {
                    tutorialCompleteText.SetActive(true);
                    attackText.SetActive(false);
                    InputLogic.Instance.tutComplete = true;
                }
                else
                {
                    if (pickedUpSeeds)
                    {
                        plantSeeds.SetActive(true);
                        attackText.SetActive(false);
                    }
                    else
                    {
                        pickupCrystalsText.SetActive(true);
                        attackText.SetActive(false);
                    }
                }

                attackLaunched = true;
                hasFire = false;
                Destroy(fireSpirit);
                Vector3 catPos = cat.transform.position;
                Vector3 targetPos = target.transform.position;
                Vector3 ballSpawnPos = new Vector3(catPos.x, catPos.y+1, 0);
                GameObject fireball = Instantiate(AttacksController.Instance.FireAttackPrefab, ballSpawnPos, Quaternion.identity);
                GameController.Instance.PlayFiringUpSound();
                fireball.transform.localScale = new Vector3(0, 0, 0);
                yield return StartCoroutine(AttacksController.Instance.ScaleOverTime(fireball, 0.6f, 1f));
                StartCoroutine(AttacksController.Instance.MoveAnimalAttackSequence(cat, 0, .2f));
                GameController.Instance.PlayLaunchSound();
                yield return AttacksController.Instance.MoveToPosition(fireball, targetPos, .1f*Vector3.Distance(catPos, targetPos), true);
                Destroy(fireball, 4f);
                AttacksController.Instance.worldLight.intensity = .9f;
            }

            yield return null;
        }
    }
}