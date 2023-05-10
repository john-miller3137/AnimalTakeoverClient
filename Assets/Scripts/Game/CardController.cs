using System.Collections;
using System.Collections.Generic;
using Network;
using Riptide;
using SharedLibrary;
using SharedLibrary.Library;
using SharedLibrary.Objects;
using UnityEngine;

public class CardController : MonoBehaviour
{
    private static CardController instance;
    public static CardController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CardController>();
                if (instance == null)
                {
                    GameObject go = new GameObject("CardController");
                    instance = go.AddComponent<CardController>();
                }
            }
            return instance;
        }
    }

    [SerializeField] private GameObject myAnimal0, myAnimal1, myAnimal2, enemyAnimal0, enemyAnimal1, enemyAnimal2;
    public List<GameObject> myAnimals, enemyAnimals;
    [SerializeField] private List<Sprite> cardSprites;


    private void Awake()
    {
        myAnimals = new List<GameObject>();
        enemyAnimals = new List<GameObject>();
    }

    void Start()
    {
        if (myAnimal0 != null && myAnimal1 != null && myAnimal2 != null)
        {
            myAnimals.Add(myAnimal0);
            myAnimals.Add(myAnimal1);
            myAnimals.Add(myAnimal2);
        }
        if (enemyAnimal0 != null && enemyAnimal1 != null && enemyAnimal2 != null)
        {
            enemyAnimals.Add(enemyAnimal0);
            enemyAnimals.Add(enemyAnimal1);
            enemyAnimals.Add(enemyAnimal2);
        }
    }

    void Update()
    {

    }

    public void UpdateCard(GameCard gameCard, GameObject card)
    {
        if (card != null && gameCard != null)
        {
            SpriteRenderer spriteRenderer = card.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = cardSprites[gameCard.Id];
            GameCardInfo gameCardInfo = card.GetComponent<GameCardInfo>();
            gameCardInfo.Id = gameCard.Id;
            gameCardInfo.XMove = gameCard.XMove;
            gameCardInfo.YMove = gameCard.YMove;
            gameCardInfo.AttackId = gameCard.AttackId;
            gameCardInfo.Type = gameCard.Type;
        }
    }
    public void HandleDrawCard(int gameCardId)
    {
        GameCard gameCard = CardInfo.idToCardMap[gameCardId];
        if(!GameLogic.Instance.card0.activeSelf)
        {
            UpdateCard(gameCard, GameLogic.Instance.card0);
            GameLogic.Instance.card0.SetActive(true);
        } else if(!GameLogic.Instance.card1.activeSelf)
        {
            UpdateCard(gameCard, GameLogic.Instance.card1);
            GameLogic.Instance.card1.SetActive(true);
        }
        else if (!GameLogic.Instance.card2.activeSelf)
        {
            UpdateCard(gameCard, GameLogic.Instance.card2);
            GameLogic.Instance.card2.SetActive(true);
        }
        else if (!GameLogic.Instance.card3.activeSelf)
        {
            UpdateCard(gameCard, GameLogic.Instance.card3);
            GameLogic.Instance.card3.SetActive(true);
        } else
        {
            Debug.Log("Cant draw card");
        }
    }

    public void PlayCard(int roomId, int cardId, int animalId)
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)MessageResponseCodes.CardPlayRequest);
        Debug.Log(cardId);
        message.AddString(MessageHandlers.Key);
        message.AddInt(roomId).AddInt(cardId).AddInt(animalId);
        NetworkManager.Singleton.MainClient.Send(message);
    }
    
}