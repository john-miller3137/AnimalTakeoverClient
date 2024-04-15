using System;
using Network;
using Riptide;
using TMPro;
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

        [SerializeField] private GameObject stars, mainMenuSong;
        public GameObject searchingText;
        private AudioSource mmSong;
        private TextMeshProUGUI _textMeshProUGUI;
        private float movingSpeed;
        int finishedCount = 0;
        public bool searchingForGame = false;
        private int counterSearch;
        private string searchingString = "searching";
        private bool goingUp = true;
        

        private void Start()
        {
            _textMeshProUGUI = searchingText.GetComponent<TextMeshProUGUI>();
            searchingText.SetActive(false);
            movingSpeed = 2;
            mmSong = mainMenuSong.GetComponent<AudioSource>();
            counterSearch = 0;
            
        }

        private void Update()
        {
            if (searchingForGame)
            {
                if (goingUp)
                {
                    counterSearch++;
                }
                else
                {
                    counterSearch--;
                }

                if (counterSearch <= 0)
                {
                    goingUp = true;
                }

                if (counterSearch > 0 && counterSearch < 100)
                {
                    _textMeshProUGUI.text = "searching";
                }
                else if (counterSearch >= 100 && counterSearch < 200)
                {
                    _textMeshProUGUI.text = "searching.";
                } else if (counterSearch >= 200 && counterSearch < 300)
                {
                    _textMeshProUGUI.text = "searching..";
                } else if (counterSearch >= 300 && counterSearch < 400)
                {
                    _textMeshProUGUI.text = "searching...";
                }else if (counterSearch >= 400)
                {
                    goingUp = false;
                }
            }
            stars.transform.Rotate(Vector3.forward, movingSpeed * Time.deltaTime);
            if (!mmSong.isPlaying && !InputLogic.Instance.isTutorial)
            {
                finishedCount++;
                if (finishedCount > 1)
                {
                    if (mmSong.isActiveAndEnabled)
                    {
                        mmSong.Play();
                    }
                    
                }
            }
            else
            {
                finishedCount = 0;
            }
        }
        
        public void CancelButton()
        {
            InputLogic.Instance.animalInfoButton.SetActive(true);
            InputLogic.Instance.shopButton.SetActive(true);
            InputLogic.Instance.cancelButton.SetActive(false);
            InputLogic.Instance.findGameButton.SetActive(true);
            InputLogic.Instance.findGameTwosButton.SetActive(true);
            InputLogic.Instance.findGameTwosAiButton.SetActive(true);
            MainMenuController.Instance.searchingForGame = false;
            MainMenuController.Instance.searchingText.SetActive(false);
            MessageHandlers.CancelSearch();
        }
    }
}