using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TimerController : MonoBehaviour
    {
        private static TimerController instance;
        public static TimerController Instance
    
        {
            get
            {
                if (instance == null)
                {
                    instance = FindFirstObjectByType<TimerController>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("TimerController");
                        instance = go.AddComponent<TimerController>();
                    }
                }
                return instance;
            }
        }


        [SerializeField] private GameObject timerBar;
        private Slider timerSlider;
        public bool doCountdown;
        private float turnTimer;
        private const float turnTime = 11.8f;
        private void Start()
        {
            turnTimer = turnTime;
            timerSlider = timerBar.GetComponent<Slider>();
        }

        private void Update()
        {
            if (doCountdown)
            {
                turnTimer -= Time.deltaTime;
                timerSlider.value = turnTimer / turnTime;
            } 
        }

        public void StartTimer()
        {
            doCountdown = true;
        }
        public void SetTimerFull()
        {
            turnTimer = turnTime;
            doCountdown = false;
            timerSlider.value = 1;
        }
    }
}