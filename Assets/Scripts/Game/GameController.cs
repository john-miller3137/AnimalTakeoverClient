using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Scripts;
using SharedLibrary.ReturnCodes;
using TMPro;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = System.Random;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        private static GameController instance;
        public static GameController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GameController>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("GameController");
                        instance = go.AddComponent<GameController>();
                    }
                }
                return instance;
            }
        }

        [SerializeField] private Sprite[] noShadowSprites;
        [SerializeField] private GameObject ParachutingAnimal;
        [SerializeField] private GameObject moveSound, firingUpSound, launchSound, explosionSound;
        private AudioSource moveSource, firingUpSource, launchSource, explosionSource;
        public GameObject nametag;
        private TextMeshProUGUI nametagText;
        public string player1Name, player2Name;

        [SerializeField] private AudioClip bombDropSound,
            bombExplosionSound,
            firingUpSoundClip,
            launchSoundClip,
            explosionClip;
        private async void Start()
        {
            //await InputLogic.Instance.WaitForSceneLoads();
            nametagText = nametag.GetComponent<TextMeshProUGUI>();
            circleLight = circle.GetComponent<Light2D>();
            sr = outerCircle.GetComponent<SpriteRenderer>();
            sr2 = circle.GetComponent<SpriteRenderer>();
            initialScale = outerCircle.transform.localScale;
            targetScale = initialScale * 3f;  // Set the target scale to 3 times the initial scale
            elapsedTime = 0f;
            moveSource = moveSound.GetComponent<AudioSource>();
            firingUpSource = firingUpSound.GetComponent<AudioSource>();
            launchSource = launchSound.GetComponent<AudioSource>();
            explosionSource = explosionSound.GetComponent<AudioSource>();
            //DoStartCountdown();
            //StartCoroutine(StartupCircleNoDelay());
            //InputLogic.Instance.gameSceneLoaded++;
        }

        private void Update()
        {
            
        }

        public void SetNametagText(String text)
        {
            nametagText.text = text;
        }

        public void PlayMoveSound()
        {
            moveSource.Play();
        }
        public void PlayFiringUpSound()
        {
            firingUpSource.clip = firingUpSoundClip;
            firingUpSource.Play();
        }
        public void StopFiringUpSound()
        {
            firingUpSource.Stop();
        }
        public void PlayLaunchSound()
        {
            launchSource.clip = launchSoundClip;
            launchSource.Play();
        }
        public void PlayDragonFire()
        {
            launchSource.clip = AnimalEffectController.Instance.dragonFireSound;
            launchSource.Play();
        }
        public void StopLaunchSound()
        {
            launchSource.Stop();
        }
        public void PlayExplosionSound()
        {
            explosionSource.Play();
        }
        
        public void PlayBombDropSound()
        {
            launchSource.clip = bombDropSound;
            launchSource.Play();
        }
        public void PlayBombExplosionSound()
        {
            explosionSource.clip = bombExplosionSound;
            if (!explosionSource.isPlaying)
            {
                explosionSource.Play();
            }
            
        }
        
        public void PlayFiringUpFx(AudioClip clip)
        {
            firingUpSource.clip = clip;
            firingUpSource.Play();
        }


        public void DoStartCountdown()
        {
            StartCoroutine(StartCountdown());
        }

        [SerializeField] private GameObject oneText, twoText, threeText, gText, oText, perfectText, perfectParticles,
            inventoryCanvas, gameStartingText, circle, oofText, missText;

        public GameObject canvas;
        public GameObject outerCircle;
        private Vector3 initialScale;
        public SpriteRenderer sr, sr2;
        private Vector3 targetScale;
        private Light2D circleLight;
        private float elapsedTime;
        private bool scalingUp = true;  // Flag to track the current scaling direction
        public bool doScaling;
        private float duration = 0.5f;  // Total duration of each scaling animation
        public bool cA;
        private IEnumerator StartCountdown()
        {
            yield return new WaitForSeconds(.1f);
            StartCoroutine(SpawnGameStarting());
            yield return new WaitForSeconds(6.9f);
            StartCoroutine(SpawnThree());
            yield return new WaitForSeconds(1);
            StartCoroutine(SpawnTwo());
            yield return new WaitForSeconds(1);
            StartCoroutine(SpawnOne());
            yield return new WaitForSeconds(.9f);
            StartCoroutine(SpawnGo());
            yield return null;
        }

        public IEnumerator SpawnMissedHit()
        {
            Random r = new Random();
            int randomInt = r.Next(1, 3);
            GameObject text;
            switch (randomInt)
            {
                case 1:
                    text = Instantiate(oofText, new Vector3(2.8f, -7.7f, 0), Quaternion.identity);
                    
                    break;
                case 2:
                    text = Instantiate(missText, new Vector3(2.8f, -7.7f, 0), Quaternion.identity);
                    break;
                default:
                    text = Instantiate(oofText, new Vector3(2.8f, -7.7f, 0), Quaternion.identity);
                    break;
            }
            text.transform.SetParent(inventoryCanvas.transform);
            Destroy(text, 1f);
            yield return null;
        }
        private IEnumerator SpawnGameStarting()
        {
            GameObject gameStarting = Instantiate(gameStartingText, new Vector3(0, 0, 0), Quaternion.identity);
            gameStarting.transform.SetParent(canvas.transform);
            Destroy(gameStarting, 2);
            yield return null;
        }
        public IEnumerator SpawnPerfect()
        {
            GameObject perfect = Instantiate(perfectText, new Vector3(2.8f, -7.7f, 0), Quaternion.identity);
            GameObject perfectParticles = Instantiate(this.perfectParticles, new Vector3(2.8f, -7.7f, 0), Quaternion.identity);
            perfect.transform.SetParent(inventoryCanvas.transform);
            Destroy(perfect, 1f);
            Destroy(perfectParticles, 1f);
            yield return null;
        }
        private IEnumerator SpawnThree()
        {
            GameObject three = Instantiate(threeText, new Vector3(0, 0, 0), Quaternion.identity);
            three.transform.SetParent(canvas.transform);
            Destroy(three, 1f);
            yield return null;
        } 
        private IEnumerator SpawnTwo()
        {
            GameObject three = Instantiate(twoText, new Vector3(0, 0, 0), Quaternion.identity);
            three.transform.SetParent(canvas.transform);
            Destroy(three, 1f);
            yield return null;
        } 
        private IEnumerator SpawnOne()
        {
            GameObject three = Instantiate(oneText, new Vector3(0, 0, 0), Quaternion.identity);
            three.transform.SetParent(canvas.transform);
            Destroy(three, 1f);
            yield return null;
        } 
        private IEnumerator SpawnGo()
        {
            GameObject g = Instantiate(gText, new Vector3(-1.5f, 0, 0), Quaternion.identity);
            GameObject o = Instantiate(oText, new Vector3(1.5f, 0, 0), Quaternion.identity);
            g.transform.SetParent(canvas.transform);
            o.transform.SetParent(canvas.transform);
            Destroy(g, 1f);
            Destroy(o, 1f);
            yield return null;
        }

        public IEnumerator HideOuterCircle()
        {
            
            yield return null;
        }
        public IEnumerator FadeSpriteToFullAlpha(float t, SpriteRenderer i)
        {
            while (i.color.a < .45f)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
                
                yield return null;
            }
        }
        private IEnumerator DoFadeSprite(float t, SpriteRenderer i)
        { 
            while (i.color.a > 0.0f)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
                yield return null;
            }
            yield return null;
        }
        
        public void DoCircleGrowth(GameObject go)
        {
            if (doScaling)
            {
                if (elapsedTime < duration)
                {
                    // Calculate the current scale based on the easing function
                    float t = elapsedTime / duration;
                    float scaleMultiplier;

                    if (scalingUp)
                    {
                        // Scale is approaching 3
                        scaleMultiplier = Easing.OutCubic(t);
                    }
                    else
                    {
                        // Scale is approaching 1
                        scaleMultiplier = Easing.Anticipate(t);
                    }

                    Vector3 currentScale = Vector3.Lerp(initialScale, targetScale, scaleMultiplier);

                    // Apply the scale to the GameObject
                    go.transform.localScale = currentScale;

                    elapsedTime += Time.deltaTime;
                }
                else
                {
                    // Reverse the scaling direction
                    scalingUp = !scalingUp;
                    elapsedTime = 0f;

                    // Swap initial and target scales
                    (initialScale, targetScale) = (targetScale, initialScale);
                }
            }
        }

        public IEnumerator StopAndFadeCircle()
        {
            cA = false;
            doScaling = false;
            while (circleLight.pointLightOuterRadius > .5f)
            {
                circleLight.pointLightOuterRadius -= Time.deltaTime;
            }
            
            StartCoroutine(DoFadeSprite(.3f, sr));
            yield return null;
        }

        public IEnumerator FlashRed()
        {
            sr2.color = Color.red;
            yield return new WaitForSeconds(.4f);
            Color c = new Color(1, 137 / 255f, 136 / 255f);
            sr2.color = c;
            yield return null;
        }

        public IEnumerator StartupCircle()
        {
            yield return new WaitForSeconds(1);
            while (circleLight.pointLightOuterRadius < 1f)
            {
                circleLight.pointLightOuterRadius += Time.deltaTime;
            }
            
            StartCoroutine(FadeSpriteToFullAlpha(.5f, sr));
            doScaling = true;
            cA = true;
            yield return null;
        }
        public IEnumerator StartupCircleNoDelay()
        {
            while (circleLight.pointLightOuterRadius < 1f)
            {
                circleLight.pointLightOuterRadius += Time.deltaTime;
            }
            
            StartCoroutine(FadeSpriteToFullAlpha(.5f, sr));
            doScaling = true;
            cA = true;
            yield return null;
        }

        public void SetCA(bool value)
        {
            cA = value;
        }

        public void SetDoScaling(bool value)
        {
            doScaling = value;
        }

        public IEnumerator SpawnParachutingAnimals(CheckForCrystalReturn[] crystalReturns, AnimalBoardEntry[] animalBoardEntries)
        {
            List<GameObject> parachutingList = new List<GameObject>();
            yield return new WaitForSeconds(1);
            foreach(Vector3 v in GameLogic.Instance.animalInfo)
            {
                
                if (GameLogic.Instance.IsPlayerOne)
                {
                    GameObject parachutingAnimal = Instantiate(ParachutingAnimal, new Vector3(v.y + 
                            Constants.hor_offset, v.z + Constants.vert_offset, 0),
                        Quaternion.identity);
                    parachutingAnimal.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite =
                        noShadowSprites[(int)v.x];
                    parachutingList.Add(parachutingAnimal);
                }
                else
                {
                    
                    GameObject parachutingAnimal = Instantiate(ParachutingAnimal, new Vector3(
                            GameLogic.flipX((int)v.y) + Constants.hor_offset, 
                            GameLogic.flipY((int)v.z) + Constants.vert_offset, 0),
                        Quaternion.identity);
                    parachutingAnimal.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite =
                        noShadowSprites[(int)v.x];
                    parachutingList.Add(parachutingAnimal);
                }
            }

            yield return new WaitForSeconds(7);
            if (GameLogic.Instance.IsPlayerOne)
            {
                for (int i = 0; i < GameLogic.Instance.numberOfAnimals; i++)
                {
                    CrystalController.Instance.PickupCrystal(i, crystalReturns[i].CrystalId, crystalReturns[i].CrystalKey, (int)parachutingList[i].transform.position.x - Constants.hor_offset, 
                        (int)parachutingList[i].transform.position.y- Constants.vert_offset, 0, crystalReturns[i].SpawnDirt, -1, -1);
                }
            }
            else
            {
                for (int i = 0; i < GameLogic.Instance.numberOfAnimals/2; i++)
                {
                    CrystalController.Instance.PickupCrystal(i, crystalReturns[i+GameLogic.Instance.numberOfAnimals/2].CrystalId, 
                        crystalReturns[i+GameLogic.Instance.numberOfAnimals/2].CrystalKey, 
                        (int)parachutingList[i+GameLogic.Instance.numberOfAnimals/2].transform.position.x - Constants.hor_offset, 
                        (int)parachutingList[i+GameLogic.Instance.numberOfAnimals/2].transform.position.y- Constants.vert_offset, 0, 
                        crystalReturns[i+GameLogic.Instance.numberOfAnimals/2].SpawnDirt, -1, -1);
                }
                for (int i = GameLogic.Instance.numberOfAnimals/2; i < GameLogic.Instance.numberOfAnimals; i++)
                {
                    CrystalController.Instance.PickupCrystal(i, crystalReturns[i-GameLogic.Instance.numberOfAnimals/2].CrystalId, 
                        crystalReturns[i-GameLogic.Instance.numberOfAnimals/2].CrystalKey, 
                        (int)parachutingList[i-GameLogic.Instance.numberOfAnimals/2].transform.position.x - Constants.hor_offset, 
                        (int)parachutingList[i-GameLogic.Instance.numberOfAnimals/2].transform.position.y- Constants.vert_offset, 0, 
                        crystalReturns[i-GameLogic.Instance.numberOfAnimals/2].SpawnDirt, -1, -1);
                }
            }
            if (GameLogic.Instance.IsPlayerOne)
            {
                for(int i = GameLogic.Instance.playerNum * 2; 
                    i < GameLogic.Instance.playerNum * 2 + GameLogic.Instance.numberOfAnimals/GameLogic.Instance.numberOfPlayers; i++)
                {
                    CheckForCrystalReturn crystalReturn = crystalReturns[i];
                    InventoryController.Instance.AddItemToInventoryVoid((int)crystalReturn.ItemId, 
                        crystalReturn.X + Constants.hor_offset, 
                        crystalReturn.Y+Constants.vert_offset);
                }
               // InventoryController.Instance.AddItemToInventoryVoid((int)item0, p1a0x + Constants.hor_offset, p1a0y+Constants.vert_offset);
               // InventoryController.Instance.AddItemToInventoryVoid((int)item1, p1a1x + Constants.hor_offset, p1a1y+Constants.vert_offset);
               // InventoryController.Instance.AddItemToInventoryVoid((int)item2, p1a2x + Constants.hor_offset, p1a2y+Constants.vert_offset);
            }
            else
            {
                for(int i = GameLogic.Instance.playerNum * 2; 
                    i < GameLogic.Instance.playerNum * 2 + GameLogic.Instance.numberOfAnimals/GameLogic.Instance.numberOfPlayers; i++)
                {
                    CheckForCrystalReturn crystalReturn = crystalReturns[i];
                    InventoryController.Instance.AddItemToInventoryVoid((int)crystalReturn.ItemId, 
                        GameLogic.flipX(crystalReturn.X) + Constants.hor_offset, 
                        GameLogic.flipY(crystalReturn.Y)+Constants.vert_offset);
                }
                //InventoryController.Instance.AddItemToInventoryVoid((int)item3, GameLogic.flipX(p2a0x) + Constants.hor_offset, GameLogic.flipY(p2a0y)+Constants.vert_offset);
                ///InventoryController.Instance.AddItemToInventoryVoid((int)item4, GameLogic.flipX(p2a1x) + Constants.hor_offset, GameLogic.flipY(p2a1y)+Constants.vert_offset);
                //InventoryController.Instance.AddItemToInventoryVoid((int)item5, GameLogic.flipX(p2a2x) + Constants.hor_offset, GameLogic.flipY(p2a2y)+Constants.vert_offset);
            }
            yield return new WaitForSeconds(1);
            foreach(GameObject  go in parachutingList)
            {
                Destroy(go);
            }
            MoveController.Instance.UpdateDirtTileColliders();
            StartCoroutine(GameLogic.Instance.enableAnimals());
            yield return null;
        }
    }
}