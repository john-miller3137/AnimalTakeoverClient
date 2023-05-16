using UnityEngine;
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
        [SerializeField] private GameObject[] healthHeads;
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
    }
    
}