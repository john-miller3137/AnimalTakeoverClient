using System;
using System.Collections;
using Network;
using Riptide;
using SharedLibrary;
using UnityEngine;

namespace Game
{

    public class OpenCrateController : MonoBehaviour
    {
        public static OpenCrateController Instance { get; private set; }

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

        [SerializeField] private GameObject openCrateParticleEffect,
            woodenCrate,
            lightRays,
            animalPortrait,
            openCrateSound;

        public Sprite[] portraits;

        private SpriteRenderer lightRaySprite, portraitSprite;
        private ParticleSystem openCrateParticleSystem;
        private Animator woodenCrateAnimator;
        private AudioSource openCrateAudio;
        private bool shouldRotateRays;
        public bool crateOpening;

        private void Start()
        {
            openCrateAudio = openCrateSound.GetComponent<AudioSource>();
            woodenCrateAnimator = woodenCrate.GetComponent<Animator>();
            portraitSprite = animalPortrait.GetComponent<SpriteRenderer>();
            lightRaySprite = lightRays.GetComponent<SpriteRenderer>();
            woodenCrateAnimator.enabled = false;
            //StartCoroutine(OpenCrate(17));
        }


        public IEnumerator OpenCrate(int animalId)
        {
            openCrateAudio.Play();
            woodenCrateAnimator.enabled = true;
            woodenCrateAnimator.Play("AnimalCrateShake");
            yield return new WaitForSeconds(1.5f);
            Color c = lightRaySprite.color;
            lightRaySprite.color = new Color(c.r, c.g, c.b, 0);
            woodenCrate.SetActive(false);
            GameObject crateParticles =
                Instantiate(openCrateParticleEffect, new Vector3(0, 1, 0), Quaternion.identity);
            Destroy(crateParticles, 5f);
            portraitSprite.sprite = portraits[animalId];
            animalPortrait.SetActive(true);
            while (lightRaySprite.color.a < 1f)
            {
                c = lightRaySprite.color;
                lightRaySprite.color = new Color(c.r, c.g, c.b, c.a + 4 * Time.deltaTime);
                yield return null;
            }

            shouldRotateRays = true;
            StartCoroutine(rotateRays());
            yield return null;
        }

        public void CloseCrate()
        {
            animalPortrait.SetActive(false);
            woodenCrate.SetActive(true);
            lightRaySprite.color = new Color(Color.white.r, Color.white.g, Color.white.b, 0);
            shouldRotateRays = false;
        }

        private IEnumerator rotateRays()
        {
            while (shouldRotateRays)
            {
                lightRays.transform.Rotate(new Vector3(0, 0, 16*Time.deltaTime)); 
                yield return null;
            }

            yield return null;
        }

        public static void SendCrateOpen()
        {
            Message message =
                Message.Create(MessageSendMode.Reliable, (ushort)MessageResponseCodes.CrateOpenRequest);
            message.AddString(MessageHandlers.Key);
            NetworkManager.Singleton.MainClient.Send(message);
        }
    }
}