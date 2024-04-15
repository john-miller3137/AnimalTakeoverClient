using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Authentication;
using System.Threading.Tasks;
using Game;
using Network;
using Riptide;
using Scripts.GameStructure;
using Server.Game;
using SharedLibrary;
using SharedLibrary.Library;
using SharedLibrary.Objects;
using SharedLibrary.ReturnCodes;
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

    [SerializeField] private GameObject greyedOut, possibleTilePrefab,
        leftArrow, rightArrow, healthHead1, healthHead2, healthHead3,
        guiCover,
        specialReadyFx;//, trash;
    //public GameObject card0, card1, card2, card3, zoomCard, bcard0, bcard1, bcard2, bcard3;
    //public GameCard gc0, gc1, gc2, gc3;
    //private int selectedCard = -1;
    public GameObject myAnimal0, myAnimal1, myAnimal2, enemyAnimal0, enemyAnimal1, enemyAnimal2, gameObjects,
        button, outerCircle,camera, myAnimal3, enemyAnimal3;
    public Vector3[] animalInfo;
    public GameObject[] myAnimals, enemyAnimals;
    private static List<GameObject> possibleMoveTiles, greySpaces;
    private bool isPlayerOne;
    private int user_animals_count;
    private const int max_x = 8;
    private const int max_y = 16;
    private const int vert_offset = -6;
    private const int hor_offset = -4;
    private const int max_cards = 4;
    public int numberOfAnimals, playerNum, numberOfPlayers;
    private bool possibleMoveTilesInitalized;
    private BoxCollider2D bc0, bc1, bc3, bc4, bc5, bc6, bc7, leftArrowBC, rightArrowBC, hh1BC, hh2BC, hh3BC;//, cbc0,cbc1,cbc2,cbc3, trashbc;
    public BoxCollider2D buttonCollider, radioCollider;
    private SpriteRenderer csr0, csr1, csr2, csr3, asr0, asr1, asr2, asr3, asr4, asr5, asr6, asr7;
    public BoxCollider2D bc2;
    [SerializeField] private Sprite redTurnSprite, greenTurnSprite;
    [SerializeField] private GameObject turnIndicator, innerCircle, leftArrowSprite, rightArrowSprite,
        radio, greyBox, boardResetFx;
    private GameCardInfo gci0, gci1, gci2, gci3;
    //private Transform selectedCardPos;
    public int selectedAnimal, selectedTargetAnimal;
    private const float circleSizeThreshold = .25f;
    private GameObject selectedAnimalObj;
    public bool isLoaded = false;
    public CameraShake _shake;
    public List<int> myDeadAnimals, enemyDeadAnimals;
    private bool  leftArrowTouched, rightArrowTouched, invSelectActive;
    public bool isMyTurn;
    private GameObject inventory_select, selected_item;
    private int selected_id;
    public bool gameStarted;
    public GameMode currentGameMode;
    private SpriteRenderer[] animalRenderers;

    public Dictionary<byte, GameObject> CrystalMap;
    
    public GameAnimal[] GameAnimals;
    private Camera cameraCam;
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

    private void InitializeObjects()
    {
        myAnimals = new GameObject[numberOfAnimals/2];
        enemyAnimals = new GameObject[numberOfAnimals/2];
        animalRenderers = new SpriteRenderer[numberOfAnimals];
        GameAnimals = new GameAnimal[numberOfAnimals];
        if (currentGameMode == GameMode.DEFAULT)
        {
            enemyAnimals[0] = enemyAnimal0;
            enemyAnimals[1] = enemyAnimal1;
            enemyAnimals[2] = enemyAnimal2;
            myAnimals[0] = myAnimal0;
            myAnimals[1] = myAnimal1;
            myAnimals[2] = myAnimal2;
            animalRenderers[0] = asr0;
            animalRenderers[1] = asr1;
            animalRenderers[2] = asr2;
            animalRenderers[3] = asr3;
            animalRenderers[4] = asr4;
            animalRenderers[5] = asr5;
            myAnimal3.SetActive(false);
            enemyAnimal3.SetActive(false);
        }
        else
        {
            enemyAnimals[0] = enemyAnimal0;
            enemyAnimals[1] = enemyAnimal1;
            enemyAnimals[2] = enemyAnimal2;
            enemyAnimals[3] = enemyAnimal3;
            myAnimals[0] = myAnimal0;
            myAnimals[1] = myAnimal1;
            myAnimals[2] = myAnimal2;
            myAnimals[3] = myAnimal3;
            animalRenderers[0] = asr0;
            animalRenderers[1] = asr1;
            animalRenderers[2] = asr2;
            animalRenderers[3] = asr6;
            animalRenderers[4] = asr3;
            animalRenderers[5] = asr4;
            animalRenderers[6] = asr5;
            animalRenderers[7] = asr7;
            myAnimal3.SetActive(true);
            enemyAnimal3.SetActive(true);
        }
        
    }
    
    // Start is called before the first frame update
    async void Start()
    {
        selectedAnimal = -1;
        selectedTargetAnimal = -1;
        cameraCam = camera.GetComponent<Camera>();
        gameStarted = false;
        greySpaces = new List<GameObject>();
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
        
        possibleMoveTiles = new List<GameObject>();
        CrystalMap = new Dictionary<byte, GameObject>();
        
        bc0 = myAnimal0.GetComponent<BoxCollider2D>();
        bc1 = myAnimal1.GetComponent<BoxCollider2D>();
        bc2 = myAnimal2.GetComponent<BoxCollider2D>();
        bc3 = enemyAnimal0.GetComponent<BoxCollider2D>();
        bc4 = enemyAnimal1.GetComponent<BoxCollider2D>();
        bc5 = enemyAnimal2.GetComponent<BoxCollider2D>();
        bc6 = myAnimal3.GetComponent<BoxCollider2D>();
        bc7 = enemyAnimal3.GetComponent<BoxCollider2D>();
        
        asr0 = myAnimal0.transform.GetChild(1).GetComponent<SpriteRenderer>();
        asr1 = myAnimal1.transform.GetChild(1).GetComponent<SpriteRenderer>();
        asr2 = myAnimal2.transform.GetChild(1).GetComponent<SpriteRenderer>();
        asr3 = enemyAnimal0.transform.GetChild(1).GetComponent<SpriteRenderer>();
        asr4 = enemyAnimal1.transform.GetChild(1).GetComponent<SpriteRenderer>();
        asr5 = enemyAnimal2.transform.GetChild(1).GetComponent<SpriteRenderer>();
        asr6 = myAnimal3.transform.GetChild(1).GetComponent<SpriteRenderer>();
        asr7 = enemyAnimal3.transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isClick = false;
        Touch simulatedTouch = default;
        if (Input.GetMouseButtonDown(0))
        {
            // Convert mouse position to touch position
            Vector2 touchPos = cameraCam.ScreenToWorldPoint(Input.mousePosition);

            simulatedTouch = new Touch
            {
                fingerId = 0, 
                position = Input.mousePosition,
                phase = TouchPhase.Began
            };
            isClick = true;
        } else if (Input.GetMouseButtonUp(0))
        {
            Vector2 touchPos = cameraCam.ScreenToWorldPoint(Input.mousePosition);
            
            simulatedTouch = new Touch
            {
                fingerId = 0,
                position = Input.mousePosition,
                phase = TouchPhase.Ended 
            };
            isClick = true;
        }
        if (Input.touchCount > 0 || isClick)
        {
            Touch touch;
            if (isClick)
            {
                touch = simulatedTouch;
            }
            else
            {
                touch = Input.GetTouch(0);
            }
            Vector2 tp = cameraCam.ScreenToWorldPoint(touch.position);
            if (touch.phase == TouchPhase.Began)
            {
                Collider2D tc = Physics2D.OverlapPoint(tp);
                if (InputLogic.Instance.isTutorial && InputLogic.Instance.tutComplete)
                {
                    TutorialController.Instance.LaunchGame();
                }
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
                }else if ((currentGameMode == GameMode.TWOS) ||(currentGameMode == GameMode.TWOS_AI) &&(tc == bc6) && !myDeadAnimals.Contains(3))
                {
                    if (selectedAnimal >= 0)
                    {
                        StartCoroutine(DeselectAnimal(true));
                    }
                    StartCoroutine(DeselectItem());
                    StartCoroutine(SelectAnimal(3));
                }else if((currentGameMode == GameMode.TWOS) ||(currentGameMode == GameMode.TWOS_AI) && tc == bc7)
                {
                    if (selectedAnimal >= 0)
                    {
                        //SendAttackRequest(selectedAnimal, 3);
                        StartCoroutine(DeselectAnimal(true));
                    }
                    StartCoroutine(DeselectItem());
                }
                else if(tc == bc3)
                {
                    if (selectedAnimal >= 0)
                    {
                        //SendAttackRequest(selectedAnimal, 3);
                        StartCoroutine(DeselectAnimal(true));
                    }
                    StartCoroutine(DeselectItem());
                }
                else if (tc == bc4)
                {
                    if (selectedAnimal >= 0)
                    {
                        
                        StartCoroutine(DeselectAnimal(true));
                    }
                    StartCoroutine(DeselectItem());
                }
                else if (tc == bc5)
                {
                    if (selectedAnimal >= 0)
                    {
                        //SendAttackRequest(selectedAnimal, 5);
                        StartCoroutine(DeselectAnimal(true));
                    }
                    StartCoroutine(DeselectItem());
                }
                else if (tc == TransitionController.Instance.screenCollider && 
                         !OpenCrateController.Instance.crateOpening)
                {
                    StartCoroutine(TransitionController.Instance.CloseCrateOpen());
                }else if (tc == radioCollider)
                {
                    MusicController.Instance.PlayRadio();
                }else if (tc == EndGameController.Instance.colliderBox)
                {
                    EndGameController.Instance.RemoveEndGameScreen();
                }
                else if (tc == buttonCollider)
                {
                    if (GameController.Instance.cA)
                    {
                        if (CheckCircleSize() < circleSizeThreshold)
                        {
                            if (!InputLogic.Instance.isTutorial)
                            {
                                SendAttackRequest(selectedAnimal, selectedTargetAnimal);
                                StartCoroutine(GameLogic.Instance.DeselectAnimal(true));
                            }
                            else
                            {
                                StartCoroutine(TutorialController.Instance.LaunchFireball());
                            }
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
                }  else if (tc == EmoteController.Instance.emoteCollider1)
                {
                    GameMessages.SendEmoteRequest(EmoteCodes.OWL_IMFINE);
                }else if (tc == EmoteController.Instance.emoteCollider2)
                {
                    GameMessages.SendEmoteRequest(EmoteCodes.OWL_SMOKE);
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
                            foreach (GameObject go in PlantController.Instance.dirtTiles.Values)
                            {
                                if (tc == go.transform.GetChild(0).GetComponent<BoxCollider2D>())
                                {
                                    dirtTouched = true;
                                    if (invSelectActive)
                                    {
                                        if (InputLogic.Instance.isTutorial)
                                        {
                                            TutorialController.Instance.PlantSeeds();
                                        }
                                        else
                                        {
                                            PlantController.Instance.PlantSeed(selected_id, go.transform.position.x, go.transform.position.y);
                                        }
                                        
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
        if (selectedAnimal < 0)
        {
            selectedTargetAnimal = numberOfAnimals / 2 + FindFirstActiveEnemyAnimal(0);
            animalRenderers[selectedTargetAnimal].color = Color.white;
        }

        for (int i = numberOfAnimals / 2; i < numberOfAnimals; i++)
        {
            animalRenderers[i].color = Color.grey;
        }

        int idx = selectedTargetAnimal - numberOfAnimals / 2;
        bool foundActiveAnimal = false;

        // Start from the next animal and loop to the end of the array
        for (int i = idx + 1; i < numberOfAnimals / 2; i++)
        {
            if (enemyAnimals[i].activeSelf)
            {
                selectedTargetAnimal = i + numberOfAnimals / 2;
                animalRenderers[selectedTargetAnimal].color = Color.white;
                foundActiveAnimal = true;
                break;
            }
        }

        // If no active animal was found, loop from the start of the array to the current index
        if (!foundActiveAnimal)
        {
            for (int i = 0; i <= idx; i++)
            {
                if (enemyAnimals[i].activeSelf)
                {
                    selectedTargetAnimal = i + numberOfAnimals / 2;
                    animalRenderers[selectedTargetAnimal].color = Color.white;
                    break;
                }
            }
        }

        yield return null;
    }
    private IEnumerator ShiftTargetLeft()
    {
        if (selectedAnimal < 0)
        {
            selectedTargetAnimal = numberOfAnimals / 2 + FindFirstActiveEnemyAnimal(numberOfAnimals / 2 - 1);
            animalRenderers[selectedTargetAnimal].color = Color.white;
        }

        for (int i = numberOfAnimals / 2; i < numberOfAnimals; i++)
        {
            animalRenderers[i].color = Color.grey;
        }

        int idx = selectedTargetAnimal - numberOfAnimals / 2;
        bool foundActiveAnimal = false;

        // Start from the previous animal and loop to the start of the array
        for (int i = idx - 1; i >= 0; i--)
        {
            if (enemyAnimals[i].activeSelf)
            {
                selectedTargetAnimal = i + numberOfAnimals / 2;
                animalRenderers[selectedTargetAnimal].color = Color.white;
                foundActiveAnimal = true;
                break;
            }
        }

        // If no active animal was found, loop from the end of the array to the current index
        if (!foundActiveAnimal)
        {
            for (int i = numberOfAnimals / 2 - 1; i >= idx; i--)
            {
                if (enemyAnimals[i].activeSelf)
                {
                    selectedTargetAnimal = i + numberOfAnimals / 2;
                    animalRenderers[selectedTargetAnimal].color = Color.white;
                    break;
                }
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
        StartCoroutine(DeselectAnimal(true));
    }
    public IEnumerator DeselectAnimal(bool saveTarget)
    {
        //arrows.SetActive(false);
        specialReadyFx.SetActive(false);
        greyedOut.SetActive(false);
        PlantController.Instance.EnableDirtColliders();
        guiCover.GetComponent<SpriteRenderer>().sortingLayerName = "Crystals";
        Color blue = new Color(.5f, .6f, 1, 1);
        foreach (SpriteRenderer sr in animalRenderers)
        {
            if(sr.color != blue) sr.color = Color.white;
        }

        foreach (var g in enemyAnimals)
        {
            CrystalController.Instance.UpdateLayerAnimal(g);
        }
        
        if (!saveTarget)
        {
            selectedTargetAnimal = -1;
            if(asr0.color != blue) asr0.color = Color.white;
            if(asr1.color != blue) asr1.color = Color.white;
            if(asr2.color != blue) asr2.color = Color.white;
            if(asr6.color != blue) asr6.color = Color.white;
        }

        selectedAnimal = -1;
       
        selectedAnimalObj = null;
        possibleMoveTilesInitalized = false;
        foreach(GameObject go in possibleMoveTiles)
        {
            Destroy(go);
        }

        foreach (var go in greySpaces)
        {
            Destroy(go);
        }

        StartCoroutine(GameController.Instance.StopAndFadeCircle());
        possibleMoveTiles.RemoveRange(0,possibleMoveTiles.Count);
       // Debug.Log("sel ani" + selectedTargetAnimal + " bool : " + saveTarget);
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
                //Debug.Log("Move tile selected");
                
               
                if (!InputLogic.Instance.isTutorial)
                {
                    Message message = Message.Create(MessageSendMode.Reliable, (ushort)MessageResponseCodes.AnimalMoveRequest);
                    message.AddString(MessageHandlers.Key);
                    message.AddInt(MessageHandlers.RoomId);
                    if (isPlayerOne)
                    {
                        message.AddInt(selectedAnimal);
                    }
                    else
                    {
                        message.AddInt(selectedAnimal + numberOfAnimals/2);
                    }
                    message.AddInt(moveTileX - x);
                    message.AddInt(moveTileY - y);
                    //Debug.Log($"move tile at {moveTileX - x} {moveTileY - y}");
                    NetworkManager.Singleton.MainClient.Send(message);
                }
                else
                {
                    if (TutorialController.Instance.moveAwayActive)
                    {
                        TutorialController.Instance.moveAwayActive = false;
                        TutorialController.Instance.moveAwayText.SetActive(false);
                        TutorialController.Instance.plantSeeds.SetActive(true);
                    }
                    TutorialController.Instance.hasMoved = true;
                    StartCoroutine(MoveController.Instance.MoveAnimal(5, moveTileX, moveTileY));
                }
                
            }
        }
        
        StartCoroutine(DeselectAnimal(true));
    }
    
    private int FindFirstActiveEnemyAnimal(int startIndex)
    {
        for (int index = startIndex; index < enemyAnimals.Length; index++)
        {
            if (enemyAnimals[index].activeSelf)
            {
                return index;
            }
        }
        for (int index = 0; index < startIndex; index++)
        {
            if (enemyAnimals[index].activeSelf)
            {
                return index;
            }
        }

        return -1; // Return -1 if no active animal is found
    }

    private IEnumerator DoTargetSelect()
    {
        Color blue = new Color(.5f, .6f, 1, 1);
        if (selectedTargetAnimal < 0)
        {
            if (enemyAnimal0.activeSelf)
            {
                if(asr3.color != blue) asr3.color = Color.white;
                if(asr4.color != blue)asr4.color = Color.grey;
                if(asr5.color != blue) asr5.color = Color.grey;
                if(asr7.color != blue) asr7.color = Color.grey;
                if (currentGameMode == GameMode.TWOS || currentGameMode == GameMode.TWOS_AI)
                {
                    selectedTargetAnimal = 4;
                }
                else
                {
                    selectedTargetAnimal = 3;
                }
                
            } else if (enemyAnimal1.activeSelf)
            {
                if(asr3.color != blue)asr3.color = Color.grey;
                if(asr4.color != blue)asr4.color = Color.white;
                if(asr5.color != blue)asr5.color = Color.grey;
                if(asr7.color != blue) asr7.color = Color.grey;
                if (currentGameMode == GameMode.TWOS|| currentGameMode == GameMode.TWOS_AI)
                {
                    selectedTargetAnimal = 5;
                }
                else
                {
                    selectedTargetAnimal = 4;
                }
            } else if (enemyAnimal2.activeSelf)
            {
                if(asr3.color != blue)asr3.color = Color.grey;
                if(asr4.color != blue)asr4.color = Color.grey;
                if(asr5.color != blue)asr5.color = Color.white;
                if(asr7.color != blue) asr7.color = Color.grey;
                if (currentGameMode == GameMode.TWOS|| currentGameMode == GameMode.TWOS_AI)
                {
                    selectedTargetAnimal = 6;
                }
                else
                {
                    selectedTargetAnimal = 5;
                }
            }else if (enemyAnimal3.activeSelf && (currentGameMode == GameMode.TWOS|| currentGameMode == GameMode.TWOS_AI))
            {
                if(asr3.color != blue)asr3.color = Color.grey;
                if(asr4.color != blue)asr4.color = Color.grey;
                if(asr5.color != blue)asr5.color = Color.grey;
                if(asr7.color != blue) asr7.color = Color.white;
                
                selectedTargetAnimal = 7;

            }
        }
        else
        {
            int idx = FindFirstActiveEnemyAnimal(selectedTargetAnimal-numberOfAnimals/2); // 0 -2/3
            if (idx > -1)
            {
                for (int i = numberOfAnimals / 2; i < numberOfAnimals; i++)
                {
                    if (i == idx+ numberOfAnimals / 2)
                    {
                        animalRenderers[i].color = Color.white;
                        selectedTargetAnimal = i;
                    }
                    else
                    {
                        animalRenderers[i].color = Color.grey; 
                    }
                }
            }
            /*switch (selectedTargetAnimal)
            {
                case -1:
                    break;
                case 3:
                    if (enemyAnimal0.activeSelf)
                    {
                        if(asr3.color != blue)asr3.color = Color.white;
                        if(asr4.color != blue)asr4.color = Color.grey;
                        if(asr5.color != blue)asr5.color = Color.grey;
                        if(asr7.color != blue)asr7.color = Color.grey;
                        selectedTargetAnimal = 3;
                    } else if (enemyAnimal1.activeSelf)
                    {
                        if(asr3.color != blue)asr3.color = Color.grey;
                        if(asr4.color != blue)asr4.color = Color.white;
                        if(asr5.color != blue)asr5.color = Color.grey;
                        if(asr7.color != blue)asr7.color = Color.grey;
                        selectedTargetAnimal = 4;
                    } else if (enemyAnimal2.activeSelf)
                    {
                        if(asr3.color != blue)asr3.color = Color.grey;
                        if(asr4.color != blue)asr4.color = Color.grey;
                        if(asr5.color != blue)asr5.color = Color.white;
                        if(asr7.color != blue)asr7.color = Color.grey;
                        selectedTargetAnimal = 5;
                    }else if (enemyAnimal3.activeSelf)
                    {
                        if(asr3.color != blue)asr3.color = Color.grey;
                        if(asr4.color != blue)asr4.color = Color.grey;
                        if(asr5.color != blue)asr5.color = Color.grey;
                        if(asr7.color != blue) asr7.color = Color.white;
                        selectedTargetAnimal = 7;
                    }
                    break;
                case 4:
                    if (enemyAnimal1.activeSelf)
                    {
                        if(asr3.color != blue)asr3.color = Color.grey;
                        if(asr4.color != blue)asr4.color = Color.white;
                        if(asr5.color != blue)asr5.color = Color.grey;
                        if(asr7.color != blue)asr7.color = Color.grey;
                        selectedTargetAnimal = 4;
                    } else if (enemyAnimal2.activeSelf)
                    {
                        if(asr3.color != blue)asr3.color = Color.grey;
                        if(asr4.color != blue)asr4.color = Color.grey;
                        if(asr5.color != blue)asr5.color = Color.white;
                        if(asr7.color != blue)asr7.color = Color.grey;
                        selectedTargetAnimal = 5;
                    } else  if (enemyAnimal0.activeSelf)
                    {
                        if(asr3.color != blue)asr3.color = Color.white;
                        if(asr4.color != blue)asr4.color = Color.grey;
                        if(asr5.color != blue)asr5.color = Color.grey;
                        if(asr7.color != blue)asr7.color = Color.grey;
                        selectedTargetAnimal = 3;
                    } else if (enemyAnimal3.activeSelf)
                    {
                        if(asr3.color != blue)asr3.color = Color.grey;
                        if(asr4.color != blue)asr4.color = Color.grey;
                        if(asr5.color != blue)asr5.color = Color.grey;
                        if(asr7.color != blue) asr7.color = Color.white;
                        selectedTargetAnimal = 7;
                    }
                    break;
                case 5:
                    if (enemyAnimal2.activeSelf)
                    {
                        if(asr3.color != blue)asr3.color = Color.grey;
                        if(asr4.color != blue)asr4.color = Color.grey;
                        if(asr5.color != blue)asr5.color = Color.white;
                        if(asr7.color != blue)asr7.color = Color.grey;
                        selectedTargetAnimal = 5;
                    } else  if (enemyAnimal0.activeSelf)
                    {
                        if(asr3.color != blue)asr3.color = Color.white;
                        if(asr4.color != blue)asr4.color = Color.grey;
                        if(asr5.color != blue)asr5.color = Color.grey;
                        if(asr7.color != blue)asr7.color = Color.grey;
                        selectedTargetAnimal = 3;
                    } else if (enemyAnimal1.activeSelf)
                    {
                        if(asr3.color != blue)asr3.color = Color.grey;
                        if(asr4.color != blue)asr4.color = Color.white;
                        if(asr5.color != blue)asr5.color = Color.grey;
                        if(asr7.color != blue)asr7.color = Color.grey;
                        selectedTargetAnimal = 4;
                    } else if (enemyAnimal3.activeSelf)
                    {
                        if(asr3.color != blue)asr3.color = Color.grey;
                        if(asr4.color != blue)asr4.color = Color.grey;
                        if(asr5.color != blue)asr5.color = Color.grey;
                        if(asr7.color != blue) asr7.color = Color.white;
                        selectedTargetAnimal = 7;
                    }
                    break;
            }*/
        }

        yield return null;
    }
    private IEnumerator SelectAnimal(int animalId)
    {
        if (isPlayerOne && GameEventRoutineManager.Instance.IsConflict(animalId)) yield break;
        if (!isPlayerOne && GameEventRoutineManager.Instance.IsConflict(animalId+numberOfAnimals/2)) yield break;
        greyedOut.SetActive(true);
        PlantController.Instance.DisableDirtColliders();
        guiCover.GetComponent<SpriteRenderer>().sortingLayerName = "PlantEffectsOverlay";
        selectedAnimal = animalId;
        foreach (GameObject g in enemyAnimals)
        {
            CrystalController.Instance.UpdateLayerAnimalSpecific(g, 200);
        }
        Color blue = new Color(.5f, .6f, 1, 1);
        if (HealthController.Instance.SpecialReady(animalId))
        {
            specialReadyFx.SetActive(true);
        }
        switch (selectedAnimal)
        {
            case 0:
                if(asr1.color != blue) asr1.color = Color.grey;
                if(asr2.color != blue) asr2.color = Color.grey;
                if(asr6.color != blue) asr6.color = Color.grey;
                
                selectedAnimalObj = myAnimal0;
                
                StartCoroutine(DoTargetSelect());
                break;
            case 1:
                if(asr0.color != blue) asr0.color = Color.grey;
                if(asr2.color != blue) asr2.color = Color.grey;
                if(asr6.color != blue) asr6.color = Color.grey;
                
                selectedAnimalObj = myAnimal1;
                StartCoroutine(DoTargetSelect());
                break;
            case 2:
                if(asr1.color != blue) asr1.color = Color.grey;
                if(asr0.color != blue)asr0.color = Color.grey;
                if(asr6.color != blue) asr6.color = Color.grey;
                selectedAnimalObj = myAnimal2;
                StartCoroutine(DoTargetSelect());
                break;
            case 3:
                if(asr1.color != blue) asr1.color = Color.grey;
                if(asr0.color != blue)asr0.color = Color.grey;
                if(asr2.color != blue) asr2.color = Color.grey;
                selectedAnimalObj = myAnimal3;
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

    public void DisplayPossibleMoveTiles()
    {
        if (selectedAnimal > -1)
        {
            int max_x_move = 7;
            int max_y_move = 7;
            int min_x_border = -5;
            int max_x_border = 5;
            int min_y_border = -7;
            int max_y_border = 11;
            Tuple<int, int>[] currentAnimalPositions = new Tuple<int, int>[numberOfAnimals/2];
            Tuple<int, int>[] currentEnemyAnimalPositions = new Tuple<int, int>[numberOfAnimals/2];
            int animalId;
            if (isPlayerOne)
            {
                animalId = (int)animalInfo[selectedAnimal].x;
            }
            else
            {
                animalId = (int)animalInfo[selectedAnimal + numberOfAnimals/2].x;
            }

            if (InputLogic.Instance.isTutorial)
            {
                animalId = ConstantVars.cat_id;
                if (!TutorialController.Instance.hasTapped)
                {
                    TutorialController.Instance.hasTapped = true;
                    TutorialController.Instance.tapSquareText.SetActive(true);
                    TutorialController.Instance.tapCatText.SetActive(false);
                }

                if (!GameController.Instance.cA)
                {
                    StartCoroutine(GameController.Instance.StartupCircleNoDelay());
                }
            }
            for(int i = 0; i < numberOfAnimals/2; i++)
            {
                currentAnimalPositions[i] = new Tuple<int, int>(Mathf.RoundToInt(myAnimals[i].transform.position.x), Mathf.RoundToInt(myAnimals[i].transform.position.y));
                currentEnemyAnimalPositions[i] = new Tuple<int, int>(Mathf.RoundToInt(enemyAnimals[i].transform.position.x), Mathf.RoundToInt(enemyAnimals[i].transform.position.y));
            }

            Vector3 selectAnimalPos = selectedAnimalObj.transform.position;
            int x = Mathf.RoundToInt(selectAnimalPos.x);
            int y = Mathf.RoundToInt(selectAnimalPos.y);
            greyedOut.transform.position = selectAnimalPos;
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
                                if (t.Item1 == newX && t.Item2 == newY && !InputLogic.Instance.isTutorial)
                                {
                  
                                    shouldPutTile = false;
                                    //PutGreyBox(x+i, y+j);
                                }
                            }
                            foreach (Tuple<int, int> t in currentEnemyAnimalPositions)
                            {
                                if (t.Item1 == newX && t.Item2 == newY && !InputLogic.Instance.isTutorial)
                                {
                          
                                    shouldPutTile = false;
                                }
                            }
                            if (newX <= min_x_border || newX >= max_x_border || newY <= min_y_border || newY >= max_y_border ||
                                    (shouldPutTile == false))
                            {
                                if(!(i==3 && j == 3))
                                {
                                    PutGreyBox(x+i, y-j);
                                }

                            } else
                            {
                                moveCoords.Add(new Vector3(newX, newY, 0));
                            }
                                
                        }
                        else
                        {
                            PutGreyBox(x+i, y-j);
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

    private void PutGreyBox(int x, int y)
    {
        GameObject go = Instantiate(greyBox, new Vector3(x-3, y+3), Quaternion.identity);
        greySpaces.Add(go);
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
        myAnimal0.transform.rotation = Quaternion.identity;
        myAnimal1.transform.rotation = Quaternion.identity;
        myAnimal2.transform.rotation = Quaternion.identity;
        myAnimal3.transform.rotation = Quaternion.identity;
        enemyAnimal0.transform.rotation = Quaternion.identity;
        enemyAnimal1.transform.rotation = Quaternion.identity;
        enemyAnimal2.transform.rotation = Quaternion.identity;
        enemyAnimal3.transform.rotation = Quaternion.identity;
        /*foreach(GameObject go in myAnimals)
        {
            go.transform.GetChild(0);
        }*/
        if (myAnimal0 != null && myAnimal1 != null && myAnimal2 != null && 
            enemyAnimal0 != null && enemyAnimal1 != null && enemyAnimal2 != null)
        {
            if (isPlayerOne)
            {
                for (int i = 0; i < myAnimals.Length; i++)
                {
                    myAnimals[i].transform.position = new Vector3(animalInfo[i].y + hor_offset, animalInfo[i].z + vert_offset, 0);
                }
                for (int i = 0; i < enemyAnimals.Length; i++)
                {
                    enemyAnimals[i].transform.position = new Vector3(animalInfo[i+numberOfAnimals/2].y + hor_offset, animalInfo[i+numberOfAnimals/2].z + vert_offset, 0);
                }
            }
            else
            {
                for (int i = 0; i < myAnimals.Length; i++)
                {
                    myAnimals[i].transform.position = new Vector3(flipX((int)animalInfo[i+numberOfAnimals/2].y) + hor_offset, 
                        flipY((int)animalInfo[i+numberOfAnimals/2].z) + vert_offset, 0);
                }
                for (int i = 0; i < enemyAnimals.Length; i++)
                {
                    enemyAnimals[i].transform.position = new Vector3(flipX((int)animalInfo[i].y) + hor_offset, 
                        flipY((int)animalInfo[i].z) + vert_offset, 0);
                }
            }
        } 
    }

    private void LoadAnimalSprites()
    {
        if (IsPlayerOne)
        {
            if (currentGameMode == GameMode.DEFAULT)
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
                asr0.sprite = AnimalSpriteArray[(int)animalInfo[0].x*2];
                asr1.sprite = AnimalSpriteArray[(int)animalInfo[1].x*2];
                asr2.sprite = AnimalSpriteArray[(int)animalInfo[2].x*2];
                asr6.sprite = AnimalSpriteArray[(int)animalInfo[3].x*2];
                asr3.sprite = AnimalSpriteArray[(int)animalInfo[4].x*2+1];
                asr4.sprite = AnimalSpriteArray[(int)animalInfo[5].x*2+1];
                asr5.sprite = AnimalSpriteArray[(int)animalInfo[6].x*2+1];
                asr7.sprite = AnimalSpriteArray[(int)animalInfo[7].x*2+1];
            }
            
        }
        else
        {
            if (currentGameMode == GameMode.DEFAULT)
            {
                asr0.sprite = AnimalSpriteArray[(int)animalInfo[3].x*2];
                asr1.sprite = AnimalSpriteArray[(int)animalInfo[4].x*2];
                asr2.sprite = AnimalSpriteArray[(int)animalInfo[5].x*2];
                asr3.sprite = AnimalSpriteArray[(int)animalInfo[0].x*2+1];
                asr4.sprite = AnimalSpriteArray[(int)animalInfo[1].x*2+1];
                asr5.sprite = AnimalSpriteArray[(int)animalInfo[2].x*2+1];
            }
            else
            {
                asr0.sprite = AnimalSpriteArray[(int)animalInfo[4].x*2];
                asr1.sprite = AnimalSpriteArray[(int)animalInfo[5].x*2];
                asr2.sprite = AnimalSpriteArray[(int)animalInfo[6].x*2];
                asr3.sprite = AnimalSpriteArray[(int)animalInfo[0].x*2+1];
                asr4.sprite = AnimalSpriteArray[(int)animalInfo[1].x*2+1];
                asr5.sprite = AnimalSpriteArray[(int)animalInfo[2].x*2+1];
                
                asr6.sprite = AnimalSpriteArray[(int)animalInfo[7].x*2];
                
                asr7.sprite = AnimalSpriteArray[(int)animalInfo[3].x*2+1];
            }
            
        }
        
        
    }

    public void DoBoardReset((byte, byte)[,] gameBoard)
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
        foreach(GameObject go in PlantController.Instance.dirtTiles.Values)
        {
            Destroy(go);
        }
        PlantController.Instance.dirtTiles.Clear();
        foreach(GameObject g in PlantController.Instance.animalItems.Values)
        {
            Destroy(g);
        }
        PlantController.Instance.animalItems.Clear();
        foreach (GameObject g in AnimalEffectController.Instance.shellList)
        {
            Destroy(g);
        }
        foreach (GameObject g in AnimalEffectController.Instance.teddyBearsDictionary.Values)
        {
            Destroy(g);
        }
        AnimalEffectController.Instance.teddyBearsDictionary.Clear();
        foreach (GameObject g in AnimalEffectController.Instance.dinoExtinctLocations.Values)
        {
            Destroy(g);
        }
        AnimalEffectController.Instance.dinoExtinctLocations.Clear();
        foreach (GameObject g in AnimalEffectController.Instance.animalSpecialFx.Values)
        {
            g.transform.SetParent(null);
            Destroy(g);
        }
        AnimalEffectController.Instance.animalSpecialFx.Clear();
        AnimalEffectController.Instance.shellList.Clear();
        InventoryController.Instance.inventoryGameItems.Clear();
        GameEventRoutineManager.Instance.ClearQueues();
        PlantController.Instance.poppyObjects.Clear();
        PlantController.Instance.shieldObjects.Clear();
        
        GameObject fx = Instantiate(boardResetFx, new Vector3(0, 0, 0), Quaternion.identity);
        InitializeCrystals(gameBoard);
        Destroy(fx, 3);
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

    private IEnumerator fadeBlack()
    {
        yield return StartCoroutine(TransitionController.Instance.fadeBlackIn(2, 1f));
        if (!isLoaded)
        {
            isLoaded = true;
            GameObject.FindGameObjectWithTag("LoginDisplay").SetActive(false);
        }
        enableObjects();
        StartCoroutine(TransitionController.Instance.fadeBlackOut(2));
    }
    //each byte represents id, x, or y locaiton p1a0id = player 1 animal 0 id
    public void StartTransition(AnimalBoardEntry[] animalStartingEntries, (byte,byte)[,] gameBoard, CheckForCrystalReturn[] crystalReturns,
        GameMode gameMode, int number_of_animals, int playerNum)
    {
        this.playerNum = playerNum;
        numberOfAnimals = number_of_animals;
        numberOfPlayers = (gameMode == GameMode.TWOS || gameMode == GameMode.TWOS_AI) ? 4 : 2;
        user_animals_count = numberOfAnimals / numberOfPlayers;
        currentGameMode = gameMode;
        animalMoved = new bool[user_animals_count];
        AnimalList = new GameObject[numberOfAnimals];
        animalInfo = new Vector3[numberOfAnimals];
        InitializeObjects();
        AnimalEffectController.Instance.spinEggs = false;
        GameEventRoutineManager.Instance.ClearQueues();
        StartCoroutine(fadeBlack());
        foreach(GameObject g in PlantController.Instance.animalItems.Values)
        {
            Destroy(g);
        }
        PlantController.Instance.animalItems.Clear();
        myDeadAnimals.Clear();
        enemyDeadAnimals.Clear();
        gameStarted = true;
        TimerController.Instance.SetTimerFull();
        if (IsPlayerOne)
        {
            GameController.Instance.SetNametagText(GameController.Instance.player2Name);
        }
        else
        {
            GameController.Instance.SetNametagText(GameController.Instance.player1Name);
        }

        for (int i = 0; i < user_animals_count; i++)
        {
            HealthController.Instance.UpdateAnimalHealth(i, 1);
        }
        for (int i = 0; i < user_animals_count; i++)
        {
            HealthController.Instance.UpdateAnimalXp(i, 0);
        }
        
        
        
        GameController.Instance.DoStartCountdown();
        StartCoroutine(disableAnimals());
        for (int i = 0; i < number_of_animals; i++)
        {
            var ga = new GameAnimal();
            ga.FireCount = 0;
            ga.WaterCount = 0;
            ga.LifeCount = 0;
            ga.DecayCount = 0;
            GameAnimals[i] = ga;
        }
        
        animalInfo = new Vector3[number_of_animals];
        for (int i = 0; i < number_of_animals; i++)
        {
            animalInfo[i] = new Vector3(animalStartingEntries[i].Id, animalStartingEntries[i].X,
                animalStartingEntries[i].Y);
        }
        
        MoveToStartingPosition();
        
        LoadAnimalSprites();
        InitializeCrystals(gameBoard);
        HealthController.Instance.InitializeHeadSprites();
        
        StartCoroutine(GameController.Instance.SpawnParachutingAnimals(crystalReturns, animalStartingEntries));
        
        
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
        gameStarted = false;
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
        foreach(GameObject go in PlantController.Instance.dirtTiles.Values)
        {
            Destroy(go);
        }
        PlantController.Instance.dirtTiles.Clear();
        foreach(GameObject g in PlantController.Instance.animalItems.Values)
        {
            Destroy(g);
        }
        PlantController.Instance.animalItems.Clear();
        foreach (GameObject go in InventoryController.Instance.inventoryItems)
        {
            Destroy(go);
        }
        InventoryController.Instance.inventoryItems.Clear();
        
        foreach (GameObject g in AnimalEffectController.Instance.shellList)
        {
            Destroy(g);
        }
        foreach (GameObject g in AnimalEffectController.Instance.teddyBearsDictionary.Values)
        {
            Destroy(g);
        }
        AnimalEffectController.Instance.teddyBearsDictionary.Clear();
        foreach (GameObject g in AnimalEffectController.Instance.dinoExtinctLocations.Values)
        {
            Destroy(g);
        }
        AnimalEffectController.Instance.dinoExtinctLocations.Clear();
        foreach (GameObject g in AnimalEffectController.Instance.animalSpecialFx.Values)
        {
            g.transform.SetParent(null);
            Destroy(g);
        }
        AnimalEffectController.Instance.animalSpecialFx.Clear();
        AnimalEffectController.Instance.shellList.Clear();
        InventoryController.Instance.inventoryGameItems.Clear();
        GameEventRoutineManager.Instance.ClearQueues();
        PlantController.Instance.poppyObjects.Clear();
        PlantController.Instance.shieldObjects.Clear();
        foreach (var animal in myAnimals)
        {
            animal.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
        }
        foreach (var animal in enemyAnimals)
        {
            animal.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
        }
        myDeadAnimals.Clear();
        enemyDeadAnimals.Clear();
        for (int i = 0; i < numberOfAnimals; i++)
        {
            GameAnimals[i] = null;
            CrystalController.Instance.DestroyChildrenParticles(CrystalController.Instance.GetAnimalNoSwitch(i));
        }
        gameObjects.SetActive(false);
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("LoginRegisterScene"));
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
            StartCoroutine(TransitionController.Instance.doWhiteFlash());
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
            StartCoroutine(TransitionController.Instance.doBlackFlash());
            TimerController.Instance.SetTimerFull();
            isMyTurn = false;
            GameController.Instance.SetCA(false);
            GameController.Instance.SetDoScaling(false);
            StartCoroutine(GameController.Instance.StopAndFadeCircle());
            StartCoroutine(DeselectAnimal(true));
        }
        
    }
    /*
    //helper functions
    */
    public IEnumerator enableAnimals()
    {
        if (GameLogic.Instance.IsPlayerOne)
        {
            Debug.Log("player one");
            for (int i = 0; i < numberOfAnimals/2; i++)
            {
                if (Constants.mySpriteOffsets.TryGetValue((int)animalInfo[i].x, out float val))
                {
                    //Debug.Log("offset added");
                    myAnimals[i].transform.GetChild(1).transform.position =
                        new Vector3(
                            myAnimals[i].transform.position.x,
                            myAnimals[i].transform.position.y + val,
                            0
                        );
                }
            } 
            for (int i = 0; i < numberOfAnimals/2; i++)
            {
                if (Constants.enemySpriteOffsets.TryGetValue((int)animalInfo[i + numberOfAnimals/2].x, out float val))
                {
                    //Debug.Log("offset added");
                    enemyAnimals[i].transform.GetChild(1).transform.position =
                        new Vector3(
                            enemyAnimals[i].transform.position.x,
                            enemyAnimals[i].transform.position.y + val,
                            0
                        );
                }
            
            } 
        }
        else
        {
            for (int i = 0; i <  numberOfAnimals/2; i++)
            {
                if (Constants.mySpriteOffsets.TryGetValue((int)animalInfo[i].x, out float val))
                {
                    //Debug.Log("offset added");
                    enemyAnimals[i].transform.GetChild(1).transform.position =
                        new Vector3(
                            enemyAnimals[i].transform.position.x,
                            enemyAnimals[i].transform.position.y + val,
                            0
                        );
                }
            } 
            for (int i = 0; i <  numberOfAnimals/2; i++)
            {
                if (Constants.enemySpriteOffsets.TryGetValue((int)animalInfo[i +  numberOfAnimals/2].x, out float val))
                {
                    //Debug.Log("offset added");
                    myAnimals[i].transform.GetChild(1).transform.position =
                        new Vector3(
                            myAnimals[i].transform.position.x,
                            myAnimals[i].transform.position.y + val,
                            0
                        );
                }
            
            } 
        }

        foreach (GameObject g in myAnimals)
        {
            g.SetActive(true);
            CrystalController.Instance.UpdateLayerAnimal(g);
        }
        foreach (GameObject g in enemyAnimals)
        {
            g.SetActive(true);
            CrystalController.Instance.UpdateLayerAnimal(g);
        }

        yield return null;
    }
    public IEnumerator disableAnimals()
    {
        myAnimal0.SetActive(false);
        myAnimal1.SetActive(false);
        myAnimal2.SetActive(false);
        myAnimal3.SetActive(false);
        enemyAnimal0.SetActive(false);
        enemyAnimal1.SetActive(false);
        enemyAnimal2.SetActive(false);
        enemyAnimal3.SetActive(false);
        yield return null;
    }
    public void enableObjects()
    {
        gameObjects.SetActive(true);
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameScene1"));
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

        if (currentGameMode == GameMode.DEFAULT)
        {
            return (animalId == 0
                ? 3
                : (animalId == 1 ? 4 : (animalId == 2 ? 5 : (animalId == 3 ? 0 : (animalId == 4 ? 1 : 2)))));
        }
        else
        {
            return (animalId == 0 ? 4 : (animalId == 1 ? 5 : (animalId == 2 ? 6 : (animalId == 3 ? 7 : (animalId == 4 ? 0 : (animalId == 5 ? 1 : (animalId == 6 ? 2 : 3)))))));
        }
        
    }

}