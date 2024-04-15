using System;
using System.Collections;
using SharedLibrary;
using UnityEngine;

namespace Game
{
    public class EmoteController : MonoBehaviour
    {
        public static EmoteController Instance { get; private set; }
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

        [SerializeField] private GameObject[] emoteCollection;

        [SerializeField] private GameObject emoteButton1, emoteButton2;

        public BoxCollider2D emoteCollider1, emoteCollider2;

        private void Start()
        {
            emoteCollider1 = emoteButton1.GetComponent<BoxCollider2D>();
            emoteCollider2 = emoteButton2.GetComponent<BoxCollider2D>();
        }

        public void ShowEmoteVoid(EmoteCodes emoteCode, int playerCode)
        {
            StartCoroutine(ShowEmote(emoteCode, playerCode));
        }
        public IEnumerator ShowEmote(EmoteCodes emoteCode, int playerCode)
        {
            GameObject emote = emoteCollection[(int)emoteCode];
            GameObject emoteInstance;
            if ((playerCode == 1 && GameLogic.Instance.IsPlayerOne) || 
                (playerCode == 2 && !GameLogic.Instance.IsPlayerOne))
            {
               emoteInstance = Instantiate(emote, new Vector3(0, -2, 0), Quaternion.identity);
            }
            else
            {
                emoteInstance = Instantiate(emote, new Vector3(0, 4, 0), Quaternion.identity);
            }

            emoteInstance.transform.localScale = new Vector3(0, 0, 0);
            while (emoteInstance.transform.localScale.x < 1)
            {
                var localScale = emoteInstance.transform.localScale;
                emoteInstance.transform.localScale = new Vector3(localScale.x + Time.deltaTime, localScale.x + Time.deltaTime,
                    localScale.x + Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(.7f);
            while (emoteInstance.transform.localScale.x > 0)
            {
                var localScale = emoteInstance.transform.localScale;
                emoteInstance.transform.localScale = new Vector3(localScale.x - Time.deltaTime, localScale.x - Time.deltaTime,
                    localScale.x - Time.deltaTime);
                yield return null;
            }
            Destroy(emoteInstance);
            
            yield return null;
        }
        
    }
}