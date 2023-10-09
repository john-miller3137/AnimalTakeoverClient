using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Network;
using Riptide;
using SharedLibrary;
using SharedLibrary.Library;
using SharedLibrary.Objects;
using UnityEngine;

public class CardController : MonoBehaviour
{/*
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
    public int[] gameCards;
    object lockObject = new object();
    private bool _cardsAreShifting = false;
    private const int MaxCards = 4;
    private int _cardCount;
    public GameObject[] gameCardArray;
    private void Awake()
    {
        gameCardArray = new GameObject[4];
        gameCards = new int[4];
        myAnimals = new List<GameObject>();
        enemyAnimals = new List<GameObject>();
    }

    void Start()
    {
        _cardCount = 4;
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

        gameCardArray[0] = GameLogic.Instance.card0;
        gameCardArray[1] = GameLogic.Instance.card1;
        gameCardArray[2] = GameLogic.Instance.card2;
        gameCardArray[3] = GameLogic.Instance.card3;
    }

    void Update()
    {

    }

    public void UpdateCard(GameCard gameCard, GameObject card)
    {
        if (gameCard != null)
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

    private IEnumerator IncrementCardcount()
    {
        _cardCount++;
        Debug.Log("Card Count incremented to " + _cardCount);
        yield return null;
    }
    private IEnumerator MoveDrawnCard(GameObject emptySlot)
    {
        Debug.Log("Cardcount = " +_cardCount);
       // lock (lockObject)
       // {
            switch (_cardCount)
            {
                case 0:
                    break;
                case 1:
                    StartCoroutine(GameLogic.Instance.LerpCardTransform(emptySlot.transform, GameLogic.Instance.bcard0.transform));
                    break;
                case 2:
                    StartCoroutine(GameLogic.Instance.LerpCardTransform(emptySlot.transform, GameLogic.Instance.bcard1.transform));
                    break;
                case 3:
                    StartCoroutine(GameLogic.Instance.LerpCardTransform(emptySlot.transform, GameLogic.Instance.bcard2.transform));
                    break;
                case 4:
                    StartCoroutine(GameLogic.Instance.LerpCardTransform(emptySlot.transform, GameLogic.Instance.bcard3.transform));
                    break;
                default :
                    break;
            }
        //}

        yield return null;
    }

    public void DoDrawCard(int gameCardId, byte handId)
    {
        StartCoroutine(HandleDrawCard(gameCardId, handId));
        
    }
    public IEnumerator HandleDrawCard(int gameCardId, byte handId)
    {

        bool shiftCards = false;
            GameCard gameCard = CardInfo.idToCardMap[gameCardId];
           // lock (lockObject)
            //{
                if (!gameCardArray[0].activeSelf)
                {

                    // gameCards keeps track of location of card either 0 1 2 3
                    yield return IncrementCardcount();

                    gameCardArray[0].GetComponent<SpriteRenderer>().sortingOrder = 4;

                    UpdateCard(gameCard, gameCardArray[0]);
                    gameCardArray[0].SetActive(true);
                    //yield return MoveDrawnCard(GameLogic.Instance.card0);
                    
                    shiftCards = true;
                }
                else if (!gameCardArray[1].activeSelf)
                {

                    yield return IncrementCardcount();
                    gameCardArray[1].GetComponent<SpriteRenderer>().sortingOrder = 4;

                    UpdateCard(gameCard, gameCardArray[1]);
                    gameCardArray[1].SetActive(true);
                    // yield return MoveDrawnCard(GameLogic.Instance.card1);
                    shiftCards = true;

                }
                else if (!gameCardArray[2].activeSelf)
                {

                    yield return IncrementCardcount();

                    gameCardArray[2].GetComponent<SpriteRenderer>().sortingOrder = 4;
                    //StartCoroutine(GameLogic.Instance.DeselectCard());
                    UpdateCard(gameCard, gameCardArray[2]);
                    gameCardArray[2].SetActive(true);
                    //yield return MoveDrawnCard(GameLogic.Instance.card2);
                    shiftCards = true;

                }
                else if (!gameCardArray[3].activeSelf)
                {

                    yield return IncrementCardcount();

                    gameCardArray[3].GetComponent<SpriteRenderer>().sortingOrder = 4;
                    gameCards[3] = handId;
                    //GameLogic.Instance.DeselectCard();
                    UpdateCard(gameCard, gameCardArray[3]);
                    gameCardArray[3].SetActive(true);
                    //yield return MoveDrawnCard(GameLogic.Instance.card3);
                    shiftCards = true;

                }
                else
                {
                    Debug.Log("Cant draw card");
                }
           // }

            if (shiftCards)
            {
                yield return ShiftCards();
            }
            //               gameCardss   server
            // 0 1 2 3  handid=gameId       0 1 2 3     0 1 2 3  
            // 1 2 3 0          3 0 1 2     4 1 2 3
            // 2 3 0 1           2 3 0 1     4 5 2 3
                // foreach(int in gamecards) if int == 0 card0 = cardi
                yield return null;
                
    }

    public void PlayCard(int roomId, int cardId, int animalId, byte handId)
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)MessageResponseCodes.CardPlayRequest);
        Debug.Log(cardId);
        message.AddString(MessageHandlers.Key);
        message.AddInt(roomId).AddInt(cardId);
        message.AddInt(animalId);
        
        message.AddByte(handId);
        NetworkManager.Singleton.MainClient.Send(message);
    }

    public void CallDiscardCard(byte handId)
    {
        StartCoroutine(DiscardCard(handId));
    }

    public void SendDiscardCard(byte handId, int cardId)
    {
        
        StartCoroutine(DiscardCard(handId));
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)MessageResponseCodes.DiscardCard);
        message.AddString(MessageHandlers.Key).AddInt(MessageHandlers.RoomId);
        message.AddInt(cardId);
        NetworkManager.Singleton.MainClient.Send(message);
    }

    
    private IEnumerator DiscardCard(byte handId)
    {
        
            // Shift the cards here...
            GameObject card1, card2, card3;
            bool success;
            GameObject temp;
            Debug.Log($"{handId} being discarded...");
            for (int i = 0; i < MaxCards; i++)
            {
                // lock (lockObject)
                //  {


                if (gameCardArray[i].name == $"Card{handId}")
                {
                    switch (i)
                    {
                        case 0:

                            gameCardArray[i].SetActive(false);
                            temp = gameCardArray[0];
                            gameCardArray[0] = gameCardArray[1]; // 0 1 2 3 -> 1 2 3 0 
                            gameCardArray[1] = gameCardArray[2];
                            gameCardArray[2] = gameCardArray[3];
                            gameCardArray[3] = temp;
                            _cardsAreShifting = true;

                            StartCoroutine(ShiftCards());
                            _cardCount--;


                            break;
                        case 1:
                            gameCardArray[i].SetActive(false);
                            //GameLogic.Instance.card1.transform.position = GameLogic.Instance.bcard3.transform.position;



                            temp = gameCardArray[1];
                            gameCardArray[1] = gameCardArray[2];
                            gameCardArray[2] = gameCardArray[3];
                            gameCardArray[3] = temp;
                            _cardsAreShifting = true;

                            StartCoroutine(ShiftCards());
                            _cardCount--;


                            break;
                        case 2:

                            gameCardArray[i].SetActive(false);
                            //GameLogic.Instance.card2.transform.position = GameLogic.Instance.bcard3.transform.position;



                            temp = gameCardArray[2];
                            gameCardArray[2] = gameCardArray[3];
                            gameCardArray[3] = temp;
                            _cardsAreShifting = true;

                            StartCoroutine(ShiftCards());
                            _cardCount--;


                            break;
                        case 3:
                            gameCardArray[i].SetActive(false);

                            lock (lockObject)
                            {
                                _cardCount--;
                            }

                            break;
                        default:
                            break;
                    }
                }
                // }
            }

            _cardsAreShifting = false;
        yield return null;
    }
    
    private static int SwitchAnimal(int animalId)
    {
        switch (animalId)
        {
            case 0:
                return 3;
            case 1:
                return 4;
            case 2:
                return 5;
            case 3:
                return 0;
            case 4 :
                return 1;
            case 5:
                return 2;
            default:
                return -1;
        }
    }
    
    private IEnumerator ShiftCards()
    {
        Debug.Log($"Debug{gameCardArray[0].name} {gameCardArray[1].name} {gameCardArray[2].name} {gameCardArray[3].name} ");
        lock (lockObject)
        {
            gameCardArray[0].GetComponent<SpriteRenderer>().sortingOrder = 0;
            gameCardArray[1].GetComponent<SpriteRenderer>().sortingOrder = 1;
            gameCardArray[2].GetComponent<SpriteRenderer>().sortingOrder = 2;
            gameCardArray[3].GetComponent<SpriteRenderer>().sortingOrder = 3;
            if (gameCardArray[0].transform.position != GameLogic.Instance.bcard0.transform.position)
            {
                yield return GameLogic.Instance.LerpCardTransform(gameCardArray[0].transform,
                    GameLogic.Instance.bcard0.transform);
            
            }
            
            if (gameCardArray[1].transform.position != GameLogic.Instance.bcard1.transform.position)
            {
                yield return GameLogic.Instance.LerpCardTransform(gameCardArray[1].transform,
                    GameLogic.Instance.bcard1.transform);
           
            }
            
            if (gameCardArray[2].transform.position != GameLogic.Instance.bcard2.transform.position)
            {
                yield return GameLogic.Instance.LerpCardTransform(gameCardArray[2].transform,
                    GameLogic.Instance.bcard2.transform);
            
            }
            
            if (gameCardArray[3].transform.position != GameLogic.Instance.bcard3.transform.position)
            {
                yield return GameLogic.Instance.LerpCardTransform(gameCardArray[3].transform,
                    GameLogic.Instance.bcard3.transform);
            
            }
            
        }
       
       
    }
    */
}