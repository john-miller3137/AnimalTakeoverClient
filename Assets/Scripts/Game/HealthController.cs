using System;
using System.Collections;
using Network;
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
        [SerializeField] private GameObject[] healthHeads, healthBars;
        private Slider a0Slider, a1Slider, a2Slider;
        private object KOLock;

        private void Start()
        {
            KOLock = new object();
            a0Slider = healthBars[0].GetComponent<Slider>();
            a1Slider = healthBars[1].GetComponent<Slider>();
            a2Slider = healthBars[2].GetComponent<Slider>();
        }

        public void InitializeHeadSprites()
        {
            if (GameLogic.Instance.IsPlayerOne)
            {
                for (int i = 0; i < Constants.max_animals/2; i++)
                {
                    int id = (int)GameLogic.Instance.animalInfo[i].x;
                    healthHeads[i].GetComponent<SpriteRenderer>().sprite = healthHeadSprites[id];
                }
            }
            else
            {
                for (int i = 3; i < Constants.max_animals; i++)
                {
                    int id = (int)GameLogic.Instance.animalInfo[i].x;
                    healthHeads[i-3].GetComponent<SpriteRenderer>().sprite = healthHeadSprites[id];
                }
            }
        }

        public void UpdateAnimalHealth(int animalId, float ratio)
        {
            switch (animalId)
            {
                case 0:
                    a0Slider.value = ratio;
                    break;
                case 1:
                    a1Slider.value = ratio;
                    break;
                case 2:
                    a2Slider.value = ratio;
                    break;
                case 3:
                    a0Slider.value = ratio;
                    break;
                case 4:
                    a1Slider.value = ratio;
                    break;
                case 5:
                    a2Slider.value = ratio;
                    break;
            }
        }

        public IEnumerator OnDeadAnimal(GameObject animal, int targetId)
        {
            animal.SetActive(false);
            yield return new WaitForSeconds(1);
            animal.transform.position = new Vector3(5, 11, 0);
            lock (KOLock)
            {
                if (GameLogic.Instance.IsPlayerOne)
                {
                    if (targetId < 3)
                    {
                        GameLogic.Instance.myDeadAnimals.Add(targetId);
                    }
                    else
                    {
                        GameLogic.Instance.enemyDeadAnimals.Add(targetId);
                    }
                    
                }
                else
                {
                    if (MessageHandlers.SwitchAnimal(targetId) < 3)
                    {
                        GameLogic.Instance.myDeadAnimals.Add(MessageHandlers.SwitchAnimal(targetId));
                    }
                    else
                    {
                        GameLogic.Instance.enemyDeadAnimals.Add(MessageHandlers.SwitchAnimal(targetId));
                    }
                }
            }
           
            yield return null;
        }
        
    }
    
}