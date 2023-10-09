using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Scripts;
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
        private void Start()
        {
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
        }

        private void Update()
        {
            
        }

        public void PlayMoveSound()
        {
            moveSource.Play();
        }
        public void PlayFiringUpSound()
        {
            firingUpSource.Play();
        }
        public void PlayLaunchSound()
        {
            launchSource.Play();
        }
        public void PlayExplosionSound()
        {
            explosionSource.Play();
        }
        


        public void DoStartCountdown()
        {
            StartCoroutine(StartCountdown());
        }

        [SerializeField] private GameObject oneText, twoText, threeText, gText, oText, canvas, perfectText, perfectParticles,
            inventoryCanvas, gameStartingText, outerCircle, circle, oofText, missText;
        private Vector3 initialScale;
        private SpriteRenderer sr, sr2;
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

        public IEnumerator SpawnParachutingAnimals(byte a0Id, byte a0Key, byte a1Id, byte a1Key, byte a2Id, byte a2Key,
            byte a3Id, byte a3Key, byte a4Id, byte a4Key, byte a5Id, byte a5Key, bool c0dirt, bool c1dirt,bool c2dirt,
            bool c3dirt,bool c4dirt,bool c5dirt, byte item0, byte item1, byte item2, byte item3, byte item4, byte item5,
            byte p1a0x, byte p1a0y, byte p1a1x, byte p1a1y, byte p1a2x, byte p1a2y, byte p2a0x, byte p2a0y, byte p2a1x, 
            byte p2a1y, byte p2a2x, byte p2a2y)
        {
            List<GameObject> parachutingList = new List<GameObject>();
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
            Debug.Log($"{a0Id} {a0Key} {a1Id} {a1Key}");
            if (GameLogic.Instance.IsPlayerOne)
            {
                CrystalController.Instance.PickupCrystal(0, a0Id, a0Key, (int)parachutingList[0].transform.position.x - Constants.hor_offset, 
                    (int)parachutingList[0].transform.position.y- Constants.vert_offset, 0, c0dirt);
                CrystalController.Instance.PickupCrystal(1, a1Id, a1Key, (int)parachutingList[1].transform.position.x - Constants.hor_offset, 
                    (int)parachutingList[1].transform.position.y - Constants.vert_offset, 0, c1dirt);
                CrystalController.Instance.PickupCrystal(2, a2Id, a2Key, (int)parachutingList[2].transform.position.x - Constants.hor_offset, 
                    (int)parachutingList[2].transform.position.y - Constants.vert_offset, 0, c2dirt);
                CrystalController.Instance.PickupCrystal(3, a3Id, a3Key, (int)parachutingList[3].transform.position.x - Constants.hor_offset, 
                    (int)parachutingList[3].transform.position.y - Constants.vert_offset, 0, c3dirt);
                CrystalController.Instance.PickupCrystal(4, a4Id, a4Key, (int)parachutingList[4].transform.position.x - Constants.hor_offset, 
                    (int)parachutingList[4].transform.position.y - Constants.vert_offset, 0, c4dirt);
                CrystalController.Instance.PickupCrystal(5, a5Id, a5Key, (int)parachutingList[5].transform.position.x - Constants.hor_offset, 
                    (int)parachutingList[5].transform.position.y - Constants.vert_offset, 0, c5dirt);
            }
            else
            {
                CrystalController.Instance.PickupCrystal(0, a3Id, a3Key, (int)parachutingList[3].transform.position.x - Constants.hor_offset, 
                    (int)parachutingList[3].transform.position.y- Constants.vert_offset, 0, c3dirt);
                CrystalController.Instance.PickupCrystal(1, a4Id, a4Key, (int)parachutingList[4].transform.position.x - Constants.hor_offset, 
                    (int)parachutingList[4].transform.position.y - Constants.vert_offset, 0, c4dirt);
                CrystalController.Instance.PickupCrystal(2, a5Id, a5Key, (int)parachutingList[5].transform.position.x - Constants.hor_offset, 
                    (int)parachutingList[5].transform.position.y - Constants.vert_offset, 0, c5dirt);
                CrystalController.Instance.PickupCrystal(3, a0Id, a0Key, (int)parachutingList[0].transform.position.x - Constants.hor_offset, 
                    (int)parachutingList[0].transform.position.y - Constants.vert_offset, 0, c0dirt);
                CrystalController.Instance.PickupCrystal(4, a1Id, a1Key, (int)parachutingList[1].transform.position.x - Constants.hor_offset, 
                    (int)parachutingList[1].transform.position.y - Constants.vert_offset, 0, c1dirt);
                CrystalController.Instance.PickupCrystal(5, a2Id, a2Key, (int)parachutingList[2].transform.position.x - Constants.hor_offset, 
                    (int)parachutingList[2].transform.position.y - Constants.vert_offset, 0, c2dirt);
            }
            if (GameLogic.Instance.IsPlayerOne)
            {
                InventoryController.Instance.AddItemToInventoryVoid((int)item0, p1a0x + Constants.hor_offset, p1a0y+Constants.vert_offset);
                InventoryController.Instance.AddItemToInventoryVoid((int)item1, p1a1x + Constants.hor_offset, p1a1y+Constants.vert_offset);
                InventoryController.Instance.AddItemToInventoryVoid((int)item2, p1a2x + Constants.hor_offset, p1a2y+Constants.vert_offset);
            }
            else
            {
                InventoryController.Instance.AddItemToInventoryVoid((int)item3, GameLogic.flipX(p2a0x) + Constants.hor_offset, GameLogic.flipY(p2a0y)+Constants.vert_offset);
                InventoryController.Instance.AddItemToInventoryVoid((int)item4, GameLogic.flipX(p2a1x) + Constants.hor_offset, GameLogic.flipY(p2a1y)+Constants.vert_offset);
                InventoryController.Instance.AddItemToInventoryVoid((int)item5, GameLogic.flipX(p2a2x) + Constants.hor_offset, GameLogic.flipY(p2a2y)+Constants.vert_offset);
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