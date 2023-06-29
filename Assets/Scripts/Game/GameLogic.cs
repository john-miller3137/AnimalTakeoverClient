using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game;
using Network;
using Riptide;
using SharedLibrary;
using SharedLibrary.Library;
using SharedLibrary.Objects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameLogic : MonoBehaviour
{
    private static GameLogic instance;
    public static GameLogic Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameLogic>();
                if (instance == null)
                {
                    GameObject go = new GameObject("GameLogic");
                    instance = go.AddComponent<GameLogic>();
                }
            }
            return instance;
        }
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    [SerializeField] private GameObject gameObjects, greyedOut, possibleTilePrefab;//, trash;
    //public GameObject card0, card1, card2, card3, zoomCard, bcard0, bcard1, bcard2, bcard3;
    //public GameCard gc0, gc1, gc2, gc3;
    //private int selectedCard = -1;
    public GameObject myAnimal0, myAnimal1, myAnimal2, enemyAnimal0, enemyAnimal1, enemyAnimal2;
    public Vector3[] animalInfo = new Vector3[max_animals];
    private GameObject[] myAnimals, enemyAnimals;
    private static List<GameObject> possibleMoveTiles;
    private bool isPlayerOne;
    private const int max_animals = 6;
    private const int user_animals_count = 3;
    private const int max_x = 8;
    private const int max_y = 16;
    private const int vert_offset = -6;
    private const int hor_offset = -4;
    private const int max_cards = 4;
    private bool possibleMoveTilesInitalized;
    private BoxCollider2D bc0, bc1, bc2, bc3, bc4, bc5;//, cbc0,cbc1,cbc2,cbc3, trashbc;
    private SpriteRenderer csr0, csr1, csr2, csr3, asr0, asr1, asr2, asr3, asr4, asr5;
    private GameCardInfo gci0, gci1, gci2, gci3;
    //private Transform selectedCardPos;
    private int selectedAnimal = -1;
    private GameObject selectedAnimalObj;
    private bool isLoaded = false;
    public Dictionary<byte, GameObject> CrystalMap;

    public GameObject[] AnimalList;
    private bool[] animalMoved;

    [SerializeField]
    private Sprite[] AnimalSpriteArray;

    public bool IsPlayerOne
    {
        get{ return isPlayerOne; }
        set { isPlayerOne = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        possibleMoveTiles = new List<GameObject>();
        myAnimals = new GameObject[user_animals_count];
        enemyAnimals = new GameObject[user_animals_count];
        animalMoved = new bool[user_animals_count];
        AnimalList = new GameObject[max_animals];
        CrystalMap = new Dictionary<byte, GameObject>();
        Debug.Log("GameLogic Start");
        //trashbc = trash.GetComponent<BoxCollider2D>();
        bc0 = myAnimal0.GetComponent<BoxCollider2D>();
        bc1 = myAnimal1.GetComponent<BoxCollider2D>();
        bc2 = myAnimal2.GetComponent<BoxCollider2D>();
        bc3 = enemyAnimal0.GetComponent<BoxCollider2D>();
        bc4 = enemyAnimal1.GetComponent<BoxCollider2D>();
        bc5 = enemyAnimal2.GetComponent<BoxCollider2D>();
        /*cbc0 = card0.GetComponent<BoxCollider2D>();
        cbc1 = card1.GetComponent<BoxCollider2D>();
        cbc2 = card2.GetComponent<BoxCollider2D>();
        cbc3 = card3.GetComponent<BoxCollider2D>();
        csr0 = card0.GetComponent<SpriteRenderer>();
        csr1 = card1.GetComponent<SpriteRenderer>();
        csr2 = card2.GetComponent<SpriteRenderer>();
        csr3 = card3.GetComponent<SpriteRenderer>();*/
        asr0 = myAnimal0.transform.GetChild(1).GetComponent<SpriteRenderer>();
        asr1 = myAnimal1.transform.GetChild(1).GetComponent<SpriteRenderer>();
        asr2 = myAnimal2.transform.GetChild(1).GetComponent<SpriteRenderer>();
        asr3 = enemyAnimal0.transform.GetChild(1).GetComponent<SpriteRenderer>();
        asr4 = enemyAnimal1.transform.GetChild(1).GetComponent<SpriteRenderer>();
        asr5 = enemyAnimal2.transform.GetChild(1).GetComponent<SpriteRenderer>();
        /*gci0 = card0.GetComponent<GameCardInfo>();
        gci1 = card1.GetComponent<GameCardInfo>();
        gci2 = card2.GetComponent<GameCardInfo>();
        gci3 = card3.GetComponent<GameCardInfo>();*/
        enemyAnimals[0] = enemyAnimal0;
        enemyAnimals[1] = enemyAnimal1;
        enemyAnimals[2] = enemyAnimal2;
        myAnimals[0] = myAnimal0;
        myAnimals[1] = myAnimal1;
        myAnimals[2] = myAnimal2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 tp = Camera.main.ScreenToWorldPoint(touch.position);
            if (touch.phase == TouchPhase.Began)
            {
                Collider2D tc = Physics2D.OverlapPoint(tp);
                /*if (tc == cbc3)
                {
                    //StartCoroutine(SelectCardProcess(3));
                }
                else if (tc == cbc2)
                {
                    //StartCoroutine(SelectCardProcess(2));
                }
                else if (tc == cbc1)
                {
                    //StartCoroutine(SelectCardProcess(1));
                }
                else if (tc == cbc0)
                {
                    //StartCoroutine(SelectCardProcess(0));
                    
                }*/
                if(tc == bc0)
                {
                    if (selectedAnimal >= 0)
                    {
                        StartCoroutine(DeselectAnimal());
                    }
                    StartCoroutine(SelectAnimal(0));
                }
                else if (tc == bc1)
                {
                    if (selectedAnimal >= 0)
                    {
                        StartCoroutine(DeselectAnimal());
                    }
                    StartCoroutine(SelectAnimal(1));
                }
                else if (tc == bc2)
                {
                    if (selectedAnimal >= 0)
                    {
                        StartCoroutine(DeselectAnimal());
                    }
                    StartCoroutine(SelectAnimal(2));
                }
                else if(tc == bc3)
                {
                    if (selectedAnimal >= 0)
                    {
                        StartCoroutine(DeselectAnimal());
                    }
                }
                else if (tc == bc4)
                {
                    if (selectedAnimal >= 0)
                    {
                        StartCoroutine(DeselectAnimal());
                    }
                }
                else if (tc == bc5)
                {
                    if (selectedAnimal >= 0)
                    {
                        StartCoroutine(DeselectAnimal());
                    }
                }
                else
                {
                    StartCoroutine(SelectMoveTile(tc));
                    
                }
            }
        }
    }
    private IEnumerator DeselectAnimal()
    {
        greyedOut.SetActive(false);
        asr0.color = Color.white;
        asr1.color = Color.white;
        asr2.color = Color.white;
        asr3.color = Color.white;
        asr4.color = Color.white;
        asr5.color = Color.white;
        selectedAnimal = -1;
        selectedAnimalObj = null;
        possibleMoveTilesInitalized = false;
        foreach(GameObject go in possibleMoveTiles)
        {
            Destroy(go);
        }
        possibleMoveTiles.RemoveRange(0,possibleMoveTiles.Count);
        yield return null;
    }
    public IEnumerator SelectMoveTile(Collider2D c)
    {
        if (selectedAnimal < 0) yield return null; 
        foreach(GameObject go in possibleMoveTiles)
        {
            if(go.GetComponent<BoxCollider2D>() == c)
            {
                int x = Mathf.RoundToInt(selectedAnimalObj.transform.position.x);
                int y = Mathf.RoundToInt(selectedAnimalObj.transform.position.y);
                int moveTileX = Mathf.RoundToInt(go.transform.position.x);
                int moveTileY = Mathf.RoundToInt(go.transform.position.y);
                //MoveController.Instance.MoveAnimal(selectedAnimal, );
                Debug.Log("Move tile selected");
                
                Message message = Message.Create(MessageSendMode.Reliable, (ushort)MessageResponseCodes.AnimalMoveRequest);
                message.AddString(MessageHandlers.Key);
                message.AddInt(MessageHandlers.RoomId);
                if (isPlayerOne)
                {
                    message.AddInt(selectedAnimal);
                }
                else
                {
                    message.AddInt(selectedAnimal + 3);
                }
                message.AddInt(moveTileX - x);
                message.AddInt(moveTileY - y);
                NetworkManager.Singleton.MainClient.Send(message);
            }
        }
        
        yield return DeselectAnimal();
    }
    private IEnumerator SelectAnimal(int animalId)
    {
        greyedOut.SetActive(true);
        selectedAnimal = animalId;
        switch (selectedAnimal)
        {
            case 0:
                asr1.color = Color.grey;
                asr2.color = Color.grey;
                asr3.color = Color.grey;
                asr4.color = Color.grey;
                asr5.color = Color.grey;
                selectedAnimalObj = myAnimal0;
                break;
            case 1:
                asr0.color = Color.grey;
                asr2.color = Color.grey;
                asr3.color = Color.grey;
                asr4.color = Color.grey;
                asr5.color = Color.grey;
                selectedAnimalObj = myAnimal1;
                break;
            case 2:
                asr1.color = Color.grey;
                asr0.color = Color.grey;
                asr3.color = Color.grey;
                asr4.color = Color.grey;
                asr5.color = Color.grey;
                selectedAnimalObj = myAnimal2;
                break;
        }
        DisplayPossibleMoveTiles();
        yield return null;
    }

    private void DisplayPossibleMoveTiles()
    {
        if (selectedAnimal > -1)
        {
            int max_x_move = 7;
            int max_y_move = 7;
            int min_x_border = -5;
            int max_x_border = 5;
            int min_y_border = -9;
            int max_y_border = 9;
            Tuple<int, int>[] currentAnimalPositions = new Tuple<int, int>[user_animals_count];
            Tuple<int, int>[] currentEnemyAnimalPositions = new Tuple<int, int>[user_animals_count];
            int animalId;
            if (isPlayerOne)
            {
                animalId = (int)animalInfo[selectedAnimal].x;
            }
            else
            {
                animalId = (int)animalInfo[selectedAnimal + 3].x;
            }
            for(int i = 0; i < user_animals_count; i++)
            {
                currentAnimalPositions[i] = new Tuple<int, int>(Mathf.RoundToInt(myAnimals[i].transform.position.x), Mathf.RoundToInt(myAnimals[i].transform.position.y));
                currentEnemyAnimalPositions[i] = new Tuple<int, int>(Mathf.RoundToInt(enemyAnimals[i].transform.position.x), Mathf.RoundToInt(enemyAnimals[i].transform.position.y));
            }
            List<Vector3> moveCoords = new List<Vector3>();
            bool[,] moveSpaces = GameHelperMethods.getMoveSpaces(animalId);
                for(int i = 0; i < max_x_move;i ++)
                {
                    for(int j = 0; j< max_y_move;j++)
                    {
                        if(moveSpaces[i,j])
                        {
                            Vector3 selectAnimalObjPos = selectedAnimalObj.transform.position;
                            int newX =  Mathf.RoundToInt(selectAnimalObjPos.x - (max_x_move/2) + i);
                            int newY = Mathf.RoundToInt(selectAnimalObjPos.y + (max_y_move / 2) - j);
                            bool shouldPutTile = true;
                            foreach(Tuple<int,int> t in currentAnimalPositions)
                            {
                                if (t.Item1 == newX && t.Item2 == newY)
                                {
                  
                                    shouldPutTile = false;
                                }
                            }
                            foreach (Tuple<int, int> t in currentEnemyAnimalPositions)
                            {
                                if (t.Item1 == newX && t.Item2 == newY)
                                {
                          
                                    shouldPutTile = false;
                                }
                            }
                            if (newX <= min_x_border || newX >= max_x_border || newY <= min_y_border || newY >= max_y_border || (newX == -1 && newY == -6) ||
                                (newX == 0 && newY == -6) || (newX == 1 && newY == -6) || (newX == -1 && newY == -7) || (newX == 0 && newY == -7)
                                    || (newX == 1 && newY == -7) || (newX == -1 && newY == -8) || (newX == 0 && newY == -8) || (newX == 1 && newY == -8)
                                    || (newX == -1 && newY == 6) || (newX == 0 && newY == 6) || (newX == 1 && newY == 6) || (newX == -1 && newY == 7)
                                    || (newX == 0 && newY == 7) || (newX == 1 && newY == 7) || (newX == -1 && newY == 8) || (newX == 0 && newY == 8) || (newX == 1 && newY == 8) ||
                                    (shouldPutTile == false))
                            {

  
                            } else
                            {
                                moveCoords.Add(new Vector3(newX, newY, 0));
                            }
                                
                        }
                    }
                }
                if(possibleMoveTilesInitalized == false)
                {
                    possibleMoveTilesInitalized = true;
                    foreach (Vector3 v in moveCoords)
                    {

                        GameObject go = Instantiate(possibleTilePrefab, v, Quaternion.identity);
                        possibleMoveTiles.Add(go);
                            
                    }
                }
        }
    }
    /*
    private IEnumerator SelectCardProcess(int id)
    {
        yield return DeselectCard();
        yield return SelectCard(id);
    }
    
    public IEnumerator DeselectCard()
    {
        GameObject card;
        trash.SetActive(false);
        for (int i = 0; i < max_cards; i++)
        {
            if (CardController.Instance.gameCardArray[i].name == $"Card{selectedCard}")
            {
                switch (i)
                {
                    case 0:
                        StartCoroutine(LerpCardTransform(CardController.Instance.gameCardArray[i].transform, bcard0.transform));
                        break;
                    case 1:
                        StartCoroutine(LerpCardTransform(CardController.Instance.gameCardArray[i].transform, bcard1.transform));
                        break;
                    case 2:
                        StartCoroutine(LerpCardTransform(CardController.Instance.gameCardArray[i].transform, bcard2.transform));
                        break;
                    case 3:
                        StartCoroutine(LerpCardTransform(CardController.Instance.gameCardArray[i].transform, bcard3.transform));
                        break;
                }
            }
        }
        
        switch (selectedCard)
        {
            case 0:
                csr0.sortingOrder -= 5;
                break;
            case 1:
                csr1.sortingOrder -= 5;
                break;
            case 2:
                csr2.sortingOrder -= 5;
                break;
            case 3:
                csr3.sortingOrder -= 5;
                break;
            default:
                card = null;
                break;
        }
        selectedCard = -1;
        yield return null;
        
    }
    private IEnumerator SelectCard(int i)
    {
        GameObject card;
        bool cardInitialized = false;
        selectedCard = i;
        trash.SetActive(true);
        switch (i)
        {
            case 0:
                card = card0;
                cardInitialized = true;
                break;
            case 1:
                card = card1;
                cardInitialized = true;
                break;
            case 2:
                card = card2;
                cardInitialized = true;
                break;
            case 3:
                card = card3;
                cardInitialized = true;
                break;
            default:
                card = null;
                break;
        }

        if (cardInitialized == true)
        {
            StartCoroutine(LerpCardTransform(card.transform, zoomCard.transform));
        }
        switch (i)
        {
            case 0:
                csr0.sortingOrder += 5;
                break;
            case 1:
                csr1.sortingOrder += 5;
                break;
            case 2:
                csr2.sortingOrder += 5;
                break;
            case 3:
                csr3.sortingOrder += 5;
                break;
            default:
                card = null;
                break;
        }
        
        yield return null;

    }
    public IEnumerator LerpCardTransform(Transform cardTransform, Transform targetTransform)
    {
        float elapsedTime = 0f;
        float duration = .2f;

        Vector3 initialPosition = cardTransform.position;
        Quaternion initialRotation = cardTransform.rotation;
        Vector3 initialScale = cardTransform.localScale;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);
            cardTransform.position = Vector3.Lerp(initialPosition, targetTransform.position, t);
            cardTransform.rotation = Quaternion.Lerp(initialRotation, targetTransform.rotation, t);
            cardTransform.localScale = Vector3.Lerp(initialScale, targetTransform.localScale, t);

            yield return null;
        }
    }
    */
    public void MoveToStartingPosition()
    {
        if (myAnimal0 != null && myAnimal1 != null && myAnimal2 != null && enemyAnimal0 != null && enemyAnimal1 != null && enemyAnimal2 != null)
        {
            if (isPlayerOne)
            {
                myAnimal0.transform.position = new Vector3(animalInfo[0].y + hor_offset, animalInfo[0].z + vert_offset, 0);
                myAnimal1.transform.position = new Vector3(animalInfo[1].y + hor_offset, animalInfo[1].z + vert_offset, 0);
                myAnimal2.transform.position = new Vector3(animalInfo[2].y + hor_offset, animalInfo[2].z + vert_offset, 0);

                enemyAnimal0.transform.position = new Vector3(animalInfo[3].y + hor_offset, animalInfo[3].z + vert_offset, 0);
                enemyAnimal1.transform.position = new Vector3(animalInfo[4].y + hor_offset, animalInfo[3].z + vert_offset, 0);
                enemyAnimal2.transform.position = new Vector3(animalInfo[5].y + hor_offset, animalInfo[3].z + vert_offset, 0);

            }
            else
            {
                myAnimal0.transform.position = new Vector3(flipX((int)animalInfo[3].y) + hor_offset, flipY((int)animalInfo[3].z) + vert_offset, 0);
                myAnimal1.transform.position = new Vector3(flipX((int)animalInfo[4].y) + hor_offset, flipY((int)animalInfo[4].z) + vert_offset, 0);
                myAnimal2.transform.position = new Vector3(flipX((int)animalInfo[5].y) + hor_offset, flipY((int)animalInfo[5].z) + vert_offset, 0);

                enemyAnimal0.transform.position = new Vector3(flipX((int)animalInfo[0].y) + hor_offset, flipY((int)animalInfo[0].z) + vert_offset, 0);
                enemyAnimal1.transform.position = new Vector3(flipX((int)animalInfo[1].y) + hor_offset, flipY((int)animalInfo[1].z) + vert_offset, 0);
                enemyAnimal2.transform.position = new Vector3(flipX((int)animalInfo[2].y)+ hor_offset, flipY((int)animalInfo[2].z) + vert_offset, 0);
            }
        } else
        {
            Debug.Log("animals null");
        }
    }

    private void LoadAnimalSprites()
    {
        if (IsPlayerOne)
        {
            asr0.sprite = AnimalSpriteArray[(int)animalInfo[0].x*2];
            asr1.sprite = AnimalSpriteArray[(int)animalInfo[1].x*2];
            asr2.sprite = AnimalSpriteArray[(int)animalInfo[2].x*2];
            asr3.sprite = AnimalSpriteArray[(int)animalInfo[3].x*2+1];
            asr4.sprite = AnimalSpriteArray[(int)animalInfo[4].x*2+1];
            asr5.sprite = AnimalSpriteArray[(int)animalInfo[5].x*2+1];
        }
        else
        {
            asr0.sprite = AnimalSpriteArray[(int)animalInfo[3].x*2];
            asr1.sprite = AnimalSpriteArray[(int)animalInfo[4].x*2];
            asr2.sprite = AnimalSpriteArray[(int)animalInfo[5].x*2];
            asr3.sprite = AnimalSpriteArray[(int)animalInfo[0].x*2+1];
            asr4.sprite = AnimalSpriteArray[(int)animalInfo[1].x*2+1];
            asr5.sprite = AnimalSpriteArray[(int)animalInfo[2].x*2+1];
        }
        
        
    }

    public void InitializeCrystals((byte,byte)[,] gameBoard)
    {
        int rows = gameBoard.GetLength(0);
        int columns = gameBoard.GetLength(1);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                byte crystalId = gameBoard[i, j].Item2;
                byte crystalKey = gameBoard[i, j].Item1;
                CrystalController.Instance.SpawnCrystal(crystalId, crystalKey,  i + hor_offset, j + vert_offset);
            }
        }
    }
    //each byte represents id, x, or y locaiton p1a0id = player 1 animal 0 id
    public void StartTransition(byte p1a0id, byte p1a0x, byte p1a0y, byte p1a1id, byte p1a1x, byte p1a1y, byte p1a2id, byte p1a2x, byte p1a2y, byte p2a0id, byte p2a0x, byte p2a0y, byte p2a1id, byte p2a1x, byte p2a1y, byte p2a2id, byte p2a2x, byte p2a2y, (byte,byte)[,] gameBoard)
    {    
        animalInfo[0] = new Vector3((float)p1a0id, (float)p1a0x, (float)p1a0y);
        animalInfo[1] = new Vector3((float)p1a1id, (float)p1a1x, (float)p1a1y);
        animalInfo[2] = new Vector3((float)p1a2id, (float)p1a2x, (float)p1a2y);
        animalInfo[3] = new Vector3((float)p2a0id, (float)p2a0x, (float)p2a0y);
        animalInfo[4] = new Vector3((float)p2a1id, (float)p2a1x, (float)p2a1y);
        animalInfo[5] = new Vector3((float)p2a2id, (float)p2a2x, (float)p2a2y);
        MoveToStartingPosition();
        LoadAnimalSprites();
        InitializeCrystals(gameBoard);
        HealthController.Instance.InitializeHeadSprites();
        if (!isLoaded)
        {
            isLoaded = true;
            GameObject.FindGameObjectWithTag("LoginDisplay").SetActive(false);
        }
       
        enableObjects();
    }


    /*
    //helper functions
    */
    public void enableObjects()
    {
        gameObjects.SetActive(true);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameScene1"));
    }
    private static int flipX(int x)
    {
        return max_x - x;
    }
    private static int flipY(int y)
    {
        return max_y - y;
    }


}