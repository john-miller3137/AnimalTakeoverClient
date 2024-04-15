using System;
using System.Collections;
using SharedLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class EndGameController : MonoBehaviour
    {
        public static EndGameController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this; 
            } 
        }

        [SerializeField] private GameObject endGameObjects,
            winloseText,
            collider,
            portrait1,
            portrait2,
            portrait3,
            damage1,
            damage2,
            damage3,
            subStat1,
            subStat2,
            subStat3;

        private TextMeshProUGUI winloseTextTMP,
            damage1Text,
            damage2Text,
            damage3Text,
            subStat1Text,
            subStat2Text,
            subStat3Text;

        private Image portrait1Img,
            portrait2Img,
            portrait3Img;
        public BoxCollider2D colliderBox;
        private void Start()
        {
            damage1Text = damage1.GetComponent<TextMeshProUGUI>();
            damage2Text = damage2.GetComponent<TextMeshProUGUI>();
            damage3Text = damage3.GetComponent<TextMeshProUGUI>();
            subStat1Text = subStat1.GetComponent<TextMeshProUGUI>();
            subStat2Text = subStat2.GetComponent<TextMeshProUGUI>();
            subStat3Text = subStat3.GetComponent<TextMeshProUGUI>();
            colliderBox = collider.GetComponent<BoxCollider2D>();
            portrait1Img = portrait1.GetComponent<Image>();
            portrait2Img = portrait2.GetComponent<Image>();
            portrait3Img = portrait3.GetComponent<Image>();
            winloseTextTMP = winloseText.GetComponent<TextMeshProUGUI>();
        }

        public void ShowEndGame(EndGameValue egv, byte a0, byte a1, byte a2, int d0, int d1, int d2, String s0, 
            String s1, String s2)
        {
            StartCoroutine(loadEndGameScreen(egv, a0, a1, a2, d0, d1, d2, s0, s1, s2));
        }

        private IEnumerator loadEndGameScreen(EndGameValue egv, byte a0, byte a1, byte a2, int d0, int d1, int d2, String s0,
            String s1, String s2)
        {
            yield return new WaitForSeconds(2.5f);
            yield return TransitionController.Instance.fadeBlackIn(2f, .6f);
            if (egv == EndGameValue.WIN)
            {
                winloseTextTMP.text = "You Win!";
            }
            else if(egv == EndGameValue.LOSE)
            {
                winloseTextTMP.text = "You Lose!";
            }
            else
            {
                winloseTextTMP.text = "You Tie!";
            }

            SetEndGameInfo(a0, a1, a2, d0, d1, d2, s0, s1, s2);
            endGameObjects.SetActive(true);
            yield return null;
        }

        public void RemoveEndGameScreen()
        {
            GameLogic.Instance.EndGame();
            StartCoroutine(TransitionController.Instance.fadeBlackOut(2f));
            endGameObjects.SetActive(false);
            
        }

        public void SetEndGameInfo(byte a0, byte a1, byte a2, int d0, int d1, int d2,
            String s0, String s1, String s2)
        {
            portrait1Img.sprite = OpenCrateController.Instance.portraits[a0];
            portrait2Img.sprite = OpenCrateController.Instance.portraits[a1];
            portrait3Img.sprite = OpenCrateController.Instance.portraits[a2];
            damage1Text.text = "Damage dealt: " + d0;
            damage2Text.text = "Damage dealt: " + d1;
            damage3Text.text = "Damage dealt: " + d2;
            subStat1Text.text = s0;
            subStat2Text.text = s1;
            subStat3Text.text = s2;
        }
    }
}