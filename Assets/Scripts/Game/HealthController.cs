using System;
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

        private void Start()
        {
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

        public void OnDeadAnimal(int animalId)
        {
            int targetId = animalId;
            if (!GameLogic.Instance.IsPlayerOne)
            {
                targetId = MessageHandlers.SwitchAnimal(animalId);
            }

            switch (targetId)
            {
                case 0: 
                    DoDeath(GameLogic.Instance.myAnimal0);
                    break;
                case 1:
                    DoDeath(GameLogic.Instance.myAnimal1);
                    break;
                case 2:
                    DoDeath(GameLogic.Instance.myAnimal2);
                    break;
                case 3: 
                    DoDeath(GameLogic.Instance.enemyAnimal0);
                    break;
                case 4:
                    DoDeath(GameLogic.Instance.enemyAnimal1);
                    break;
                case 5:
                    DoDeath(GameLogic.Instance.enemyAnimal2);
                    break;
            }
        }

        private void DoDeath(GameObject animal)
        {
            animal.SetActive(false);
        }
    }
    
}