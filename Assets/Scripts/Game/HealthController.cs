using System;
using System.Collections;
using Network;
using SharedLibrary;
using SharedLibrary.Library;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HealthController : MonoBehaviour
    {
        private static HealthController instance;
        public static HealthController Instance
    
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<HealthController>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("HealthController");
                        instance = go.AddComponent<HealthController>();
                    }
                }
                return instance;
            }
        }

        [SerializeField] private Sprite[] healthHeadSprites;
        [SerializeField] private GameObject[] healthHeads, healthBars, xpBars;
        private Slider a0Slider, a1Slider, a2Slider, xp0Slider, xp1Slider, xp2Slider;
        public object KOLock;

        private Slider[] animalSliders, animalXpSliders;
        private void Start()
        {
            KOLock = new object();
            animalSliders = new Slider[3];
            animalXpSliders = new Slider[3];
            a0Slider = healthBars[0].GetComponent<Slider>();
            a1Slider = healthBars[1].GetComponent<Slider>();
            a2Slider = healthBars[2].GetComponent<Slider>();
            xp0Slider = xpBars[0].GetComponent<Slider>();
            xp1Slider = xpBars[1].GetComponent<Slider>();
            xp2Slider = xpBars[2].GetComponent<Slider>();
            animalSliders[0] = a0Slider;
            animalSliders[1] = a1Slider;
            animalSliders[2] = a2Slider;
            animalXpSliders[0] = xp0Slider;
            animalXpSliders[1] = xp1Slider;
            animalXpSliders[2] = xp2Slider;
        }

        public void InitializeHeadSprites()
        {
            int hhIdx = 0;
            for (int i = 0; i < healthHeads.Length; i++)
            {
                healthHeads[i].GetComponent<SpriteRenderer>().sprite = null;
            }
            for (int i = 0; i < GameLogic.Instance.numberOfAnimals; i++)
            {
                if (GameHelperMethods.GetPlayerFromAnimal(i,
                        GameLogic.Instance.numberOfAnimals,
                        GameLogic.Instance.numberOfPlayers) == GameLogic.Instance.playerNum)
                {
                    int id = (int)GameLogic.Instance.animalInfo[i].x;
                    healthHeads[hhIdx].GetComponent<SpriteRenderer>().sprite = healthHeadSprites[id];
                    hhIdx++;
                }
            }
        }

        public void UpdateAnimalHealth(int animalId, float ratio)
        {
            int numberOfPlayers = GameLogic.Instance.numberOfPlayers;
            int numberOfAnimals = GameLogic.Instance.numberOfAnimals;
            int playerNum = GameHelperMethods.GetPlayerFromAnimal(animalId, numberOfAnimals, numberOfPlayers);
            int sliderIndex = animalId % (numberOfAnimals / numberOfPlayers);

            if (playerNum == GameLogic.Instance.playerNum)
            {
                animalSliders[sliderIndex].value = ratio;
            }

        }
        public void UpdateAnimalXp(int animalId, float ratio)
        {
            int numberOfPlayers = GameLogic.Instance.numberOfPlayers;
            int numberOfAnimals = GameLogic.Instance.numberOfAnimals;
            int playerNum = GameHelperMethods.GetPlayerFromAnimal(animalId, numberOfAnimals, numberOfPlayers);
            int sliderIndex = animalId % (numberOfAnimals / numberOfPlayers);

            if (playerNum == GameLogic.Instance.playerNum)
            {
                animalXpSliders[sliderIndex].value = ratio;
            }
        }

        public IEnumerator OnDeadAnimal(GameObject animal, int targetId)
        {
            int numOfAnimals = GameLogic.Instance.numberOfAnimals;
            SpriteRenderer sr = animal.transform.GetChild(1).GetComponent<SpriteRenderer>();
            while (sr.color.a > 0)
            {
                Color c = sr.color;
                sr.color = new Color(c.r, c.g, c.b, c.a - Time.deltaTime);
                yield return null;
            }
            animal.SetActive(false);
            yield return new WaitForSeconds(1);
            animal.transform.position = new Vector3(10, 20, 0);
            MoveController.Instance.UpdateDirtTileColliders();
            lock (KOLock)
            {
                if (GameLogic.Instance.IsPlayerOne)
                {
                    if (targetId < numOfAnimals/2)
                    {
                        GameLogic.Instance.myDeadAnimals.Add(targetId);
                        Debug.Log("adding myDead animal " + targetId);
                    }
                    else
                    {
                        GameLogic.Instance.enemyDeadAnimals.Add(targetId);
                        Debug.Log("adding enemyDead animal " + targetId);
                    }
                    
                }
                else
                {
                    if (MessageHandlers.SwitchAnimal(targetId) < numOfAnimals/2)
                    {
                        GameLogic.Instance.myDeadAnimals.Add(MessageHandlers.SwitchAnimal(targetId));
                        Debug.Log("p2adding myDead animal " + MessageHandlers.SwitchAnimal(targetId));
                    }
                    else
                    {
                        GameLogic.Instance.enemyDeadAnimals.Add(MessageHandlers.SwitchAnimal(targetId));
                        Debug.Log("p2adding enemyDead animal " + MessageHandlers.SwitchAnimal(targetId));
                    }
                }
            }

            if (targetId == GameLogic.Instance.selectedAnimal)
            {
                GameLogic.Instance.DeselectAnimal(true);
            } 
            if (targetId == GameLogic.Instance.selectedTargetAnimal)
            {
                GameLogic.Instance.DeselectAnimal(true);
            }
            yield return null;
        }

        public bool SpecialReady(int animalId)
        {
            int numberOfPlayers = GameLogic.Instance.numberOfPlayers;
            int numberOfAnimals = GameLogic.Instance.numberOfAnimals;
            int playerNum = GameHelperMethods.GetPlayerFromAnimal(animalId, numberOfAnimals, numberOfPlayers);
            int sliderIndex = animalId % (numberOfAnimals / numberOfPlayers);

            if (playerNum == GameLogic.Instance.playerNum)
            {
                if (animalXpSliders[sliderIndex].value == 1) return true;
            }

            return false;
        }
        
    }
    
    
}