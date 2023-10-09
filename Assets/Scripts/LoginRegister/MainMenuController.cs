using System;
using UnityEngine;

namespace Scripts.LoginRegister
{
    public class MainMenuController : MonoBehaviour
    {
        private static MainMenuController instance;
        public static MainMenuController Instance
    
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<MainMenuController>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("MainMenuController");
                        instance = go.AddComponent<MainMenuController>();
                    }
                }
                return instance;
            }
        }

        [SerializeField] private GameObject stars;
        private int degrees;

        private void Start()
        {
            degrees = 0;
        }

        private void Update()
        {
            stars.transform.Rotate(new Vector3(0, 0, .001f));

        }
    }
}