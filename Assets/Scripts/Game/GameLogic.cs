using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Network;
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

    [SerializeField] private GameObject gameObjects;
    public GameObject card0, card1, card2, card3, zoomCard, bcard0, bcard1, bcard2, bcard3;
    public GameCard gc0, gc1, gc2, gc3;
    private int selectedCard = -1;
    public GameObject myAnimal0, myAnimal1, myAnimal2, enemyAnimal0, enemyAnimal1, enemyAnimal2;
    public static Vector3[] animalInfo = new Vector3[max_animals];
    private bool isPlayerOne;
    private const int max_animals = 6;
    private const int max_x = 8;
    private const int max_y = 16;
    private const float vert_offset = -6;
    private const float hor_offset = -4;
    private BoxCollider2D bc0, bc1, bc2, bc3, bc4, bc5, cbc0,cbc1,cbc2,cbc3;
    private SpriteRenderer csr0, csr1, csr2, csr3;
    private GameCardInfo gci0, gci1, gci2, gci3;
    private bool isLoaded = false;

    public bool IsPlayerOne
    {
        get{ return isPlayerOne; }
        set { isPlayerOne = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameLogic Start");
        bc0 = myAnimal0.GetComponent<BoxCollider2D>();
        bc1 = myAnimal1.GetComponent<BoxCollider2D>();
        bc2 = myAnimal2.GetComponent<BoxCollider2D>();
        bc3 = enemyAnimal0.GetComponent<BoxCollider2D>();
        bc4 = enemyAnimal1.GetComponent<BoxCollider2D>();
        bc5 = enemyAnimal2.GetComponent<BoxCollider2D>();
        cbc0 = card0.GetComponent<BoxCollider2D>();
        cbc1 = card1.GetComponent<BoxCollider2D>();
        cbc2 = card2.GetComponent<BoxCollider2D>();
        cbc3 = card3.GetComponent<BoxCollider2D>();
        csr0 = card0.GetComponent<SpriteRenderer>();
        csr1 = card1.GetComponent<SpriteRenderer>();
        csr2 = card2.GetComponent<SpriteRenderer>();
        csr3 = card3.GetComponent<SpriteRenderer>();
        gci0 = card0.GetComponent<GameCardInfo>();
        gci1 = card1.GetComponent<GameCardInfo>();
        gci2 = card2.GetComponent<GameCardInfo>();
        gci3 = card3.GetComponent<GameCardInfo>();
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
                if (tc == cbc3)
                {
                    StartCoroutine(SelectCardProcess(3));
                }
                else if (tc == cbc2)
                {
                    StartCoroutine(SelectCardProcess(2));
                }
                else if (tc == cbc1)
                {
                    StartCoroutine(SelectCardProcess(1));
                }
                else if (tc == cbc0)
                {
                    StartCoroutine(SelectCardProcess(0));
                    
                }
                else if(tc == bc0)
                {
                    if (selectedCard >= 0)
                    {
                        Debug.Log("animal touched;");
                        CardController.Instance.PlayCard(MessageHandlers.RoomId, GetSelectedCardInfo().Id, 0);
                    }
                    
                }
                else if (tc == bc1)
                {
                    if (selectedCard >= 0)
                    {
                        CardController.Instance.PlayCard(MessageHandlers.RoomId, GetSelectedCardInfo().Id, 1);
                    }
                }
                else if (tc == bc2)
                {
                    if (selectedCard >= 0)
                    {
                        CardController.Instance.PlayCard(MessageHandlers.RoomId, GetSelectedCardInfo().Id, 2);
                    }
                }
                else if(tc == bc3)
                {
                    if (selectedCard > 0)
                    {
                       
                    }
                }
                else if (tc == bc4)
                {
                    if (selectedCard > 0)
                    {
                       
                    }
                }
                else if (tc == bc5)
                {
                    if (selectedCard > 0)
                    {
                    }
                } 
                else
                {
                    StartCoroutine(DeselectCard());
                }
            }
        }
    }

    private IEnumerator SelectCardProcess(int id)
    {
        yield return DeselectCard();
        yield return SelectCard(id);
    }
    private IEnumerator DeselectCard()
    {
        GameObject card;
        switch (selectedCard)
        {
            case 0:
                card = card0;
                StartCoroutine(LerpCardTransform(card.transform, bcard0.transform));
                break;
            case 1:
                card = card1;
                StartCoroutine(LerpCardTransform(card.transform, bcard1.transform));
                break;
            case 2:
                card = card2;
                StartCoroutine(LerpCardTransform(card.transform, bcard2.transform));
                break;
            case 3:
                card = card3;
                StartCoroutine(LerpCardTransform(card.transform, bcard3.transform));
                break;
            default:
                card = null;
                break;
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

        selectedCard = i;
        switch (i)
        {
            case 0:
                card = card0;
                break;
            case 1:
                card = card1;
                break;
            case 2:
                card = card2;
                break;
            case 3:
                card = card3;
                break;
            default:
                card = null;
                break;
        }

        if (card != null)
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
    private IEnumerator LerpCardTransform(Transform cardTransform, Transform targetTransform)
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
    //each byte represents id, x, or y locaiton p1a0id = player 1 animal 0 id
    public void StartTransition(byte p1a0id, byte p1a0x, byte p1a0y, byte p1a1id, byte p1a1x, byte p1a1y, byte p1a2id, byte p1a2x, byte p1a2y, byte p2a0id, byte p2a0x, byte p2a0y, byte p2a1id, byte p2a1x, byte p2a1y, byte p2a2id, byte p2a2x, byte p2a2y)
    {    
        animalInfo[0] = new Vector3((float)p1a0id, (float)p1a0x, (float)p1a0y);
        animalInfo[1] = new Vector3((float)p1a1id, (float)p1a1x, (float)p1a1y);
        animalInfo[2] = new Vector3((float)p1a2id, (float)p1a2x, (float)p1a2y);
        animalInfo[3] = new Vector3((float)p2a0id, (float)p2a0x, (float)p2a0y);
        animalInfo[4] = new Vector3((float)p2a1id, (float)p2a1x, (float)p2a1y);
        animalInfo[5] = new Vector3((float)p2a2id, (float)p2a2x, (float)p2a2y);
        MoveToStartingPosition();
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
    }
    private static int flipX(int x)
    {
        return max_x - x;
    }
    private static int flipY(int y)
    {
        return max_y - y;
    }
    private GameCardInfo GetSelectedCardInfo()
    {
        switch(selectedCard)
        {
            case -1:
                return null;
            case 0:
                return gci0;
            case 1:
                return gci1;
            case 2:
                return gci2;
            case 3:
                return gci3;
            default:
                return null;
        }
    }

   
}