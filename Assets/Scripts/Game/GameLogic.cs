using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game;
using Network;
using Riptide;
using Scripts.GameStructure;
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

    [SerializeField] private GameObject gameObjects, greyedOut, possibleTilePrefab,
        leftArrow, rightArrow, healthHead1, healthHead2, healthHead3, camera;//, trash;
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
    private BoxCollider2D bc0, bc1, bc2, bc3, bc4, bc5, leftArrowBC, rightArrowBC, hh1BC, hh2BC, hh3BC;//, cbc0,cbc1,cbc2,cbc3, trashbc;
    private BoxCollider2D buttonCollider, radioCollider;
    private SpriteRenderer csr0, csr1, csr2, csr3, asr0, asr1, asr2, asr3, asr4, asr5;
    [SerializeField] private Sprite redTurnSprite, greenTurnSprite;
    [SerializeField] private GameObject turnIndicator, innerCircle, outerCircle, leftArrowSprite, rightArrowSprite, button,
        radio;
    private GameCardInfo gci0, gci1, gci2, gci3;
    //private Transform selectedCardPos;
    public int selectedAnimal = -1, selectedTargetAnimal = -1;
    private const float circleSizeThreshold = .25f;
    private GameObject selectedAnimalObj;
    public bool isLoaded = false;
    private CameraShake _shake;
    public List<int> myDeadAnimals, enemyDeadAnimals;
    private bool isMyTurn, leftArrowTouched, rightArrowTouched, invSelectActive;
    private GameObject inventory_select, selected_item;
    private int selected_id;

    public Dictionary<byte, GameObject> CrystalMap;
    
    public GameAnimal[] GameAnimals;
    private SpriteRenderer leftArrowSpriteRenderer, rightArrowSpriteRenderer;

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
        radioCollider = radio.GetComponent<BoxCollider2D>();
        myDeadAnimals = new List<int>();
        enemyDeadAnimals = new List<int>();
        _shake = camera.GetComponent<CameraShake>();
        hh1BC = healthHead1.GetComponent<BoxCollider2D>();
        hh2BC = healthHead2.GetComponent<BoxCollider2D>();
        hh3BC = healthHead3.GetComponent<BoxCollider2D>();
        leftArrowBC = leftArrow.GetComponent<BoxCollider2D>();
        rightArrowBC = rightArrow.GetComponent<BoxCollider2D>();
        buttonCollider = button.GetComponent<BoxCollider2D>();
        leftArrowSpriteRenderer = leftArrowSprite.GetComponent<SpriteRenderer>();
        rightArrowSpriteRenderer = rightArrowSprite.GetComponent<SpriteRenderer>();
        GameAnimals = new GameAnimal[6];
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
        //arrows.SetActive(false);
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
                if((tc == bc0 || tc == hh1BC) && !myDeadAnimals.Contains(0))
                {
                    if (selectedAnimal >= 0 )
                    {
                        StartCoroutine(DeselectAnimal(true));
                    }

                    StartCoroutine(DeselectItem());
                    StartCoroutine(SelectAnimal(0));
                }
                else if ((tc == bc1|| tc == hh2BC) && !myDeadAnimals.Contains(1))
                {
                    if (selectedAnimal >= 0)
                    {
                        StartCoroutine(DeselectAnimal(true));
                    }
                    StartCoroutine(DeselectItem());
                    StartCoroutine(SelectAnimal(1));
                }
                else if ((tc == bc2 || tc == hh3BC) && !myDeadAnimals.Contains(2))
                {
                    if (selectedAnimal >= 0)
                    {
                        StartCoroutine(DeselectAnimal(true));
                    }
                    StartCoroutine(DeselectItem());
                    StartCoroutine(SelectAnimal(2));
                }
                else if(tc == bc3)
                {
                    if (selectedAnimal >= 0)
                    {
                        //SendAttackRequest(selectedAnimal, 3);
                        StartCoroutine(DeselectAnimal(false));
                    }
                    StartCoroutine(DeselectItem());
                }
                else if (tc == bc4)
                {
                    if (selectedAnimal >= 0)
                    {
                        
                        StartCoroutine(DeselectAnimal(false));
                    }
                    StartCoroutine(DeselectItem());
                }
                else if (tc == bc5)
                {
                    if (selectedAnimal >= 0)
                    {
                        //SendAttackRequest(selectedAnimal, 5);
                        StartCoroutine(DeselectAnimal(false));
                    }
                    StartCoroutine(DeselectItem());
                }
                else if (tc == buttonCollider)
                {
                    if (GameController.Instance.cA)
                    {
                        if (CheckCircleSize() < circleSizeThreshold)
                        {
                            StartCoroutine(GameController.Instance.SpawnPerfect());
                            SendAttackRequest(selectedAnimal, selectedTargetAnimal);
                            StartCoroutine(DeselectAnimal(false));
                        }
                        else
                        {
                            _shake.Shake(.2f, .075f);
                            StartCoroutine(GameController.Instance.StopAndFadeCircle());
                            StartCoroutine(GameController.Instance.FlashRed());
                            if (isMyTurn)
                            {
                                StartCoroutine(GameController.Instance.StartupCircle());
                            }
                            StartCoroutine(GameController.Instance.SpawnMissedHit());
                            if (!isMyTurn)
                            {
                                StartCoroutine(GameController.Instance.StopAndFadeCircle());
                            }
                        }
                    }
                } else if (tc == leftArrowBC)
                {
                    if (selectedAnimal < 0) return;
                    leftArrowTouched = true;
                    leftArrowSpriteRenderer.color = Color.grey;
                    StartCoroutine(ShiftTargetLeft());
                }else if (tc == rightArrowBC)
                {
                    if (selectedAnimal < 0) return;
                    rightArrowTouched = true;
                    rightArrowSpriteRenderer.color = Color.grey;
                    StartCoroutine(ShiftTargetRight());
                } else if (tc == radioCollider)
                {
                    MusicController.Instance.PlayRadio();
                }
                else
                {
                    bool inventoryItemTouched = false;
                    lock (InventoryController.Instance.inventoryLock)
                    {
                        for (int i = 0; i < InventoryController.Instance.inventoryItems.Count; i++)
                        {
                            GameObject go = InventoryController.Instance.inventoryItems[i];
                            if (!go.IsDestroyed())
                            {
                                if (tc == go.transform.GetChild(0).GetComponent<BoxCollider2D>())
                                {
                                    if (invSelectActive)
                                    {
                                        Destroy(inventory_select);
                                    }
                                    inventoryItemTouched = true;
                                    inventory_select = Instantiate(InventoryController.Instance.inventory_select,
                                        InventoryController.Instance.GetInventorySelectPosition(i), Quaternion.identity);
                                    invSelectActive = true;
                                    selected_item = go;
                                    selected_id = i;
                                }
                            }
                        }
                    }
                    

                    bool dirtTouched = false;

                    if (!inventoryItemTouched)
                    {
                        lock (PlantController.Instance.dirtLock)
                        {
                            for (int i = 0; i < PlantController.Instance.dirtTiles.Count; i++)
                            {
                                GameObject go = PlantController.Instance.dirtTiles[i];
                                if (tc == go.transform.GetChild(0).GetComponent<BoxCollider2D>())
                                {
                                    dirtTouched = true;
                                    if (invSelectActive)
                                    {
                                        PlantController.Instance.PlantSeed(selected_id, go.transform.position.x, go.transform.position.y);
                                        StartCoroutine(DeselectItem());
                                    }
                                }
                            }
                        }
                    }

                    if (!inventoryItemTouched && !dirtTouched)
                    {
                        StartCoroutine(DeselectItem());
                        SelectMoveTile(tc);
                        
                    }
                    
                }
            } else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (leftArrowTouched)
                {
                    leftArrowTouched = false;
                    leftArrowSpriteRenderer.color = Color.white;
                }

                if (rightArrowTouched)
                {
                    rightArrowTouched = false;
                    rightArrowSpriteRenderer.color = Color.white;
                }
            }
        }
        GameController.Instance.DoCircleGrowth(outerCircle);
    }

    private IEnumerator ShiftTargetRight()
    {
        if (selectedAnimal > -1)
        {
            switch (selectedTargetAnimal)
            {
                case 3:
                    if (enemyAnimal1.activeSelf)
                    {
                        selectedTargetAnimal = 4;
                        asr4.color = Color.white;
                        asr3.color = Color.grey;
                        asr5.color = Color.grey;
                    } else if (enemyAnimal2.activeSelf)
                    {
                        selectedTargetAnimal = 5;
                        asr5.color = Color.white;
                        asr4.color = Color.grey;
                        asr3.color = Color.grey;
                    }
                    break;
                case 4:
                    if (enemyAnimal2.activeSelf)
                    {
                        selectedTargetAnimal = 5;
                        asr5.color = Color.white;
                        asr4.color = Color.grey;
                        asr3.color = Color.grey;
                    } else if (enemyAnimal0.activeSelf)
                    {
                        selectedTargetAnimal = 3;
                        asr3.color = Color.white;
                        asr5.color = Color.grey;
                        asr4.color = Color.grey;
                    }
                    break;
                case 5:
                    if (enemyAnimal0.activeSelf)
                    {
                        selectedTargetAnimal = 3;
                        asr3.color = Color.white;
                        asr5.color = Color.grey;
                        asr4.color = Color.grey;
                    } else if (enemyAnimal1.activeSelf)
                    {
                        selectedTargetAnimal = 4;
                        asr4.color = Color.white;
                        asr3.color = Color.grey;
                        asr5.color = Color.grey;
                    }
                    break;
            }
        } 
        yield return null;
    }
    private IEnumerator ShiftTargetLeft()
    {
        if (selectedAnimal > -1)
        {
            switch (selectedTargetAnimal)
            {
                case 3:
                    if (enemyAnimal2.activeSelf)
                    {
                        selectedTargetAnimal = 5;
                        asr5.color = Color.white;
                        asr3.color = Color.grey;
                        asr4.color = Color.grey;
                    } else if (enemyAnimal1.activeSelf)
                    {
                        selectedTargetAnimal = 4;
                        asr4.color = Color.white;
                        asr5.color = Color.grey;
                        asr3.color = Color.grey;
                    }
                    break;
                    
                case 4:
                    if (enemyAnimal0.activeSelf)
                    {
                        selectedTargetAnimal = 3;
                        asr3.color = Color.white;
                        asr4.color = Color.grey;
                        asr5.color = Color.grey;
                    } else if (enemyAnimal2.activeSelf)
                    {
                        selectedTargetAnimal = 5;
                        asr5.color = Color.white;
                        asr3.color = Color.grey;
                        asr4.color = Color.grey;
                    }

                    break;
                case 5:
                    if (enemyAnimal1.activeSelf)
                    {
                        selectedTargetAnimal = 4;
                        asr4.color = Color.white;
                        asr5.color = Color.grey;
                        asr3.color = Color.grey;
                    } else if (enemyAnimal0.activeSelf)
                    {
                        selectedTargetAnimal = 3;
                        asr3.color = Color.white;
                        asr4.color = Color.grey;
                        asr5.color = Color.grey;
                    }
                    break;
            }
        }
        yield return null;
    }

    public void CallDeselectItem()
    {
        StartCoroutine(DeselectItem());
    }

    private IEnumerator DeselectItem()
    {
        if (selected_id > -1)
        {
            invSelectActive = false;
            Destroy(inventory_select);
            selected_id = -1;
        }
        yield return null;
    }

    public void CallDeselectAnimal()
    {
        StartCoroutine(DeselectAnimal(false));
    }
    private IEnumerator DeselectAnimal(bool saveTarget)
    {
        //arrows.SetActive(false);
        greyedOut.SetActive(false);
        asr0.color = Color.white;
        asr1.color = Color.white;
        asr2.color = Color.white;
        if (!saveTarget)
        {
            selectedTargetAnimal = -1;
            asr3.color = Color.white;
            asr4.color = Color.white;
            asr5.color = Color.white;
        }

        selectedAnimal = -1;
       
        selectedAnimalObj = null;
        possibleMoveTilesInitalized = false;
        foreach(GameObject go in possibleMoveTiles)
        {
            Destroy(go);
        }

        StartCoroutine(GameController.Instance.StopAndFadeCircle());
        possibleMoveTiles.RemoveRange(0,possibleMoveTiles.Count);
        yield return null;
    }
    public void SelectMoveTile(Collider2D c)
    {
        if (selectedAnimal < 0) return; 
        foreach(GameObject go in possibleMoveTiles)
        {
            if(go.transform.GetChild(0).GetComponent<BoxCollider2D>() == c)
            {
                Destroy(go);
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
                Debug.Log($"move tile at {moveTileX - x} {moveTileY - y}");
                NetworkManager.Singleton.MainClient.Send(message);
            }
        }
        
        StartCoroutine(DeselectAnimal(false));
    }

    private IEnumerator DoTargetSelect()
    {
        if (selectedTargetAnimal < 0)
        {
            if (enemyAnimal0.activeSelf)
            {
                asr3.color = Color.white;
                asr4.color = Color.grey;
                asr5.color = Color.grey;
                selectedTargetAnimal = 3;
            } else if (enemyAnimal1.activeSelf)
            {
                asr3.color = Color.grey;
                asr4.color = Color.white;
                asr5.color = Color.grey;
                selectedTargetAnimal = 4;
            } else if (enemyAnimal2.activeSelf)
            {
                asr3.color = Color.grey;
                asr4.color = Color.grey;
                asr5.color = Color.white;
                selectedTargetAnimal = 5;
            }
        }
        else
        {
            switch (selectedTargetAnimal)
            {
                case -1:
                    break;
                case 3:
                    if (enemyAnimal0.activeSelf)
                    {
                        asr3.color = Color.white;
                        asr4.color = Color.grey;
                        asr5.color = Color.grey;
                        selectedTargetAnimal = 3;
                    } else if (enemyAnimal1.activeSelf)
                    {
                        asr3.color = Color.grey;
                        asr4.color = Color.white;
                        asr5.color = Color.grey;
                        selectedTargetAnimal = 4;
                    } else if (enemyAnimal2.activeSelf)
                    {
                        asr3.color = Color.grey;
                        asr4.color = Color.grey;
                        asr5.color = Color.white;
                        selectedTargetAnimal = 5;
                    }
                    break;
                case 4:
                    if (enemyAnimal1.activeSelf)
                    {
                        asr3.color = Color.grey;
                        asr4.color = Color.white;
                        asr5.color = Color.grey;
                        selectedTargetAnimal = 4;
                    } else if (enemyAnimal2.activeSelf)
                    {
                        asr3.color = Color.grey;
                        asr4.color = Color.grey;
                        asr5.color = Color.white;
                        selectedTargetAnimal = 5;
                    } else  if (enemyAnimal0.activeSelf)
                    {
                        asr3.color = Color.white;
                        asr4.color = Color.grey;
                        asr5.color = Color.grey;
                        selectedTargetAnimal = 3;
                    } 
                    break;
                case 5:
                    if (enemyAnimal2.activeSelf)
                    {
                        asr3.color = Color.grey;
                        asr4.color = Color.grey;
                        asr5.color = Color.white;
                        selectedTargetAnimal = 5;
                    } else  if (enemyAnimal0.activeSelf)
                    {
                        asr3.color = Color.white;
                        asr4.color = Color.grey;
                        asr5.color = Color.grey;
                        selectedTargetAnimal = 3;
                    } else if (enemyAnimal1.activeSelf)
                    {
                        asr3.color = Color.grey;
                        asr4.color = Color.white;
                        asr5.color = Color.grey;
                        selectedTargetAnimal = 4;
                    } 
                    break;
            }
        }

        yield return null;
    }
    private IEnumerator SelectAnimal(int animalId)
    {
        if (isPlayerOne && GameEventRoutineManager.Instance.IsConflict(animalId)) yield break;
        if (!isPlayerOne && GameEventRoutineManager.Instance.IsConflict(animalId+3)) yield break;
        greyedOut.SetActive(true);
        selectedAnimal = animalId;
        switch (selectedAnimal)
        {
            case 0:
                asr1.color = Color.grey;
                asr2.color = Color.grey;
                
                selectedAnimalObj = myAnimal0;
                
                StartCoroutine(DoTargetSelect());
                break;
            case 1:
                asr0.color = Color.grey;
                asr2.color = Color.grey;
                
                selectedAnimalObj = myAnimal1;
                StartCoroutine(DoTargetSelect());
                break;
            case 2:
                asr1.color = Color.grey;
                asr0.color = Color.grey;
                
                selectedAnimalObj = myAnimal2;
                StartCoroutine(DoTargetSelect());
                break;
        }
        DisplayPossibleMoveTiles();
        if (isMyTurn && !GameController.Instance.doScaling)
        {
            StartCoroutine(GameController.Instance.StartupCircleNoDelay());
        }
        
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
            int min_y_border = -7;
            int max_y_border = 11;
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
                            if (newX <= min_x_border || newX >= max_x_border || newY <= min_y_border || newY >= max_y_border ||
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
    public void StartTransition(byte p1a0id, byte p1a0x, byte p1a0y, byte p1a1id, byte p1a1x, byte p1a1y, byte p1a2id, 
        byte p1a2x, byte p1a2y, byte p2a0id, byte p2a0x, byte p2a0y, byte p2a1id, byte p2a1x, byte p2a1y, byte p2a2id, 
        byte p2a2x, byte p2a2y, (byte,byte)[,] gameBoard, byte a0Id, byte a0Key, byte a1Id, byte a1Key,byte a2Id, byte a2Key,
        byte a3Id, byte a3Key,byte a4Id, byte a4Key,byte a5Id, byte a5Key, bool c0dirt, bool c1dirt,bool c2dirt,bool c3dirt,
        bool c4dirt,bool c5dirt, byte item0, byte item1, byte item2, byte item3, byte item4, byte item5)
    {
        InputLogic.Instance.StopMenuSource();
        GameController.Instance.DoStartCountdown();
        StartCoroutine(disableAnimals());
        for (int i = 0; i < max_animals; i++)
        {
            var ga = new GameAnimal();
            ga.FireCount = 0;
            ga.WaterCount = 0;
            ga.LifeCount = 0;
            ga.DecayCount = 0;
            GameAnimals[i] = ga;
        }
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
        StartCoroutine(GameController.Instance.SpawnParachutingAnimals(a0Id, a0Key, a1Id, a1Key, a2Id, a2Key,
            a3Id, a3Key, a4Id, a4Key, a5Id, a5Key, c0dirt, c1dirt, c2dirt, c3dirt, c4dirt, c5dirt, item0, item1,item2,
            item3,item4,item5, p1a0x, p1a0y, p1a1x, p1a1y, p1a2x, p1a2y, p2a0x, p2a0y,p2a1x, p2a1y, p2a2x, p2a2y));
        enableObjects();
    }

    private void SendAttackRequest(int animalId, int targetId)
    {
        if (animalId < 0 || targetId < 0) return;
        animalId = GetAnimalForPlayer(animalId);
        targetId = GetAnimalForPlayer(targetId);

        if (GameAnimals[animalId].FireCount > 0)
        {
            GameMessages.SendAttackRequest(animalId, targetId, (int)SpiritType.FIRE, GameAnimals[animalId].FireCount);
        } else if (GameAnimals[animalId].WaterCount > 0)
        {
            GameMessages.SendAttackRequest(animalId, targetId, (int)SpiritType.WATER, GameAnimals[animalId].WaterCount);
        } else if (GameAnimals[animalId].LifeCount > 0)
        {
            GameMessages.SendAttackRequest(animalId, targetId, (int)SpiritType.LIFE, GameAnimals[animalId].LifeCount);
        }
        else if (GameAnimals[animalId].DecayCount > 0)
        {
            GameMessages.SendAttackRequest(animalId, targetId, (int)SpiritType.DECAY, GameAnimals[animalId].DecayCount);
        }
        else
        {
            GameMessages.SendAttackRequest(animalId, targetId, 0, 0);
        }
    }

    public void EndGame()
    {
        foreach(GameObject go in CrystalMap.Values)
        {
            Destroy(go);
        }
        CrystalMap.Clear();
        foreach(GameObject go in PlantController.Instance.plantObjects.Values)
        {
            Destroy(go);
        }
        PlantController.Instance.plantObjects.Clear();
        foreach(GameObject go in PlantController.Instance.dirtTiles)
        {
            Destroy(go);
        }
        PlantController.Instance.dirtTiles.Clear();

        for (int i = 0; i < max_animals; i++)
        {
            GameAnimals[i] = null;
            CrystalController.Instance.DestroyChildrenParticles(CrystalController.Instance.GetAnimalNoSwitch(i));
        }
        gameObjects.SetActive(false);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LoginRegisterScene"));
        isLoaded = false;
        IsPlayerOne = false;
        InputLogic.Instance.EndGame();
    }

    public void UpdateTurn(int playerTurn)
    {
        if ((playerTurn == 1 && isPlayerOne) || (playerTurn == 2 && !isPlayerOne))
        {
            TimerController.Instance.StartTimer();
            //turnIndicatorSR.sprite = greenTurnSprite;
            isMyTurn = true;
            if (selectedAnimal > -1)
            {
                GameController.Instance.SetCA(true);
                GameController.Instance.SetDoScaling(true);
                StartCoroutine(GameController.Instance.StartupCircleNoDelay());
            }
            //StartCoroutine(GameController.Instance.StartupCircleNoDelay());
        } 
        else
        {
            TimerController.Instance.SetTimerFull();
            isMyTurn = false;
            GameController.Instance.SetCA(false);
            GameController.Instance.SetDoScaling(false);
            StartCoroutine(GameController.Instance.StopAndFadeCircle());
            StartCoroutine(DeselectAnimal(false));
        }
        
    }
    /*
    //helper functions
    */
    public IEnumerator enableAnimals()
    {
        myAnimal0.SetActive(true);
        myAnimal1.SetActive(true);
        myAnimal2.SetActive(true);
        enemyAnimal0.SetActive(true);
        enemyAnimal1.SetActive(true);
        enemyAnimal2.SetActive(true);
        yield return null;
    }
    public IEnumerator disableAnimals()
    {
        myAnimal0.SetActive(false);
        myAnimal1.SetActive(false);
        myAnimal2.SetActive(false);
        enemyAnimal0.SetActive(false);
        enemyAnimal1.SetActive(false);
        enemyAnimal2.SetActive(false);
        yield return null;
    }
    public void enableObjects()
    {
        gameObjects.SetActive(true);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameScene1"));
    }
    public static int flipX(int x)
    {
        return max_x - x;
    }
    public static int flipY(int y)
    {
        return max_y - y;
    }

    private float CheckCircleSize()
    {
        return Mathf.Pow(outerCircle.transform.localScale.x - innerCircle.transform.localScale.x, 2);
    }
    public int GetAnimalForPlayer(int animalId)
    {
        if (isPlayerOne)
        {
            return animalId;
        }
        
        return (animalId == 0
                ? 3
                : (animalId == 1 ? 4 : (animalId == 2 ? 5 : (animalId == 3 ? 0 : (animalId == 4 ? 1 : 2)))));
    }

}