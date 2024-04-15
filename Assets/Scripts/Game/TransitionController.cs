using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TransitionController : MonoBehaviour
    {
        public static TransitionController Instance { get; private set; }

        [SerializeField] private GameObject transitionBlack, transitionWhite,
            openCrateScene,
            shopScene,
            loginRegisterScene,
            screen,
            animalInfoScene;

        private Image blackOverlayImage, whiteOverlayImage;
        public BoxCollider2D
            screenCollider;
       
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

        private void Start()
        {
            screenCollider = screen.GetComponent<BoxCollider2D>();
            blackOverlayImage = transitionBlack.GetComponent<Image>();
            whiteOverlayImage = transitionWhite.GetComponent<Image>();
            StartCoroutine(fadeBlackOut(2.5f));
        }
        

        public IEnumerator fadeBlackOut(float mod)
        {
            while (blackOverlayImage.color.a > 0)
            {
                Color c = blackOverlayImage.color;
                Color c2 = new Color(c.r, c.g, c.b, c.a - mod*Time.deltaTime);
                blackOverlayImage.color = c2;
                yield return null;
            }

            yield return null;
        }
        public IEnumerator fadeBlackIn(float mod, float range)
        {
            while (blackOverlayImage.color.a < range)
            {
                Color c = blackOverlayImage.color;
                Color c2 = new Color(c.r, c.g, c.b, c.a + mod*Time.deltaTime);
                blackOverlayImage.color = c2;
                yield return null;
            }

            yield return null;
        }
        
        public IEnumerator fadeWhiteOut(float mod)
        {
            while (whiteOverlayImage.color.a > 0)
            {
                Color c = whiteOverlayImage.color;
                Color c2 = new Color(c.r, c.g, c.b, c.a - mod*Time.deltaTime);
                whiteOverlayImage.color = c2;
                yield return null;
            }

            yield return null;
        }
        public IEnumerator fadeWhiteIn(float mod, float range)
        {
            while (whiteOverlayImage.color.a < range)
            {
                Color c = whiteOverlayImage.color;
                Color c2 = new Color(c.r, c.g, c.b, c.a + mod*Time.deltaTime);
                whiteOverlayImage.color = c2;
                yield return null;
            }

            yield return null;
        }

        public IEnumerator doWhiteFlash()
        {
            yield return fadeWhiteIn(2.5f, .25f);
            yield return fadeWhiteOut(2.5f);
        }
        public IEnumerator doBlackFlash()
        {
            yield return fadeBlackIn(1.5f, .15f);
            yield return fadeBlackOut(1.5f);
        }

        public IEnumerator DoShopBack()
        {
            yield return fadeBlackIn(2, 1);
            shopScene.SetActive(false);
            loginRegisterScene.SetActive(true);
            yield return fadeBlackOut(2);
            
            yield return null;
        }
        
        public IEnumerator DoCrateOpen(int animalId)
        {
            OpenCrateController.Instance.crateOpening = true;
            yield return fadeBlackIn(2, 1);
            shopScene.SetActive(false);
            openCrateScene.SetActive(true);
            yield return fadeBlackOut(2);
            yield return OpenCrateController.Instance.OpenCrate(animalId);
            OpenCrateController.Instance.crateOpening = false;
            yield return null;
        }

        public IEnumerator CloseCrateOpen()
        {
            yield return fadeBlackIn(2, 1);
            shopScene.SetActive(true);
            openCrateScene.SetActive(false);
            OpenCrateController.Instance.CloseCrate();
            yield return fadeBlackOut(2);
            yield return null;
        }

        public void DoCrateOpenVoid(int animalId)
        {
            StartCoroutine(DoCrateOpen(animalId));
        }

        public IEnumerator DoShopOpen()
        {
            yield return fadeBlackIn(2, 1);
            shopScene.SetActive(true);
            loginRegisterScene.SetActive(false);
            yield return fadeBlackOut(2);
            yield return null;
        }

        public void DoShopOpenVoid()
        {
            StartCoroutine(DoShopOpen());
        }
        
        public void DoShopCloseVoid()
        {
            StartCoroutine(DoShopBack());
        }
        
        public IEnumerator DoAnimalInfoOpen()
        {
            yield return fadeBlackIn(2, 1);
            animalInfoScene.SetActive(true);
            loginRegisterScene.SetActive(false);
            yield return fadeBlackOut(2);
            yield return null;
        }
        
        public IEnumerator DoAnimalInfoClose()
        {
            yield return fadeBlackIn(2, 1);
            animalInfoScene.SetActive(false);
            loginRegisterScene.SetActive(true);
            yield return fadeBlackOut(2);
            yield return null;
        }

        public void DoAnimalInfoOpenVoid()
        {
            StartCoroutine(DoAnimalInfoOpen());
        }
        public void DoAnimalClose()
        {
            StartCoroutine(DoAnimalInfoClose());
        }
        
    }
}