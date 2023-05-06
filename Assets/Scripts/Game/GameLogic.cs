using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField]
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

    public GameObject card0, card1, card2, card3, zoomCard, bcard0, bcard1, bcard2, bcard3;
    private int selectedCard = -1;
    public GameObject myAnimal0, myAnimal1, myAnimal2, enemyAnimal0, enemyAnimal1, enemyAnimal2;
    // Start is called before the first frame update
    void Start()
    {
        

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
                if (tc == card3.GetComponent<BoxCollider2D>())
                {
                    StartCoroutine(DeselectCard());
                    StartCoroutine(SelectCard(3));
                }
                else if (tc == card2.GetComponent<BoxCollider2D>())
                {
                    StartCoroutine(DeselectCard());
                    StartCoroutine(SelectCard(2));
                }
                else if (tc == card1.GetComponent<BoxCollider2D>())
                {
                    StartCoroutine(DeselectCard());
                    StartCoroutine(SelectCard(1));
                }
                else if (tc == card0.GetComponent<BoxCollider2D>())
                {
                    StartCoroutine(DeselectCard());
                    StartCoroutine(SelectCard(0));
                }
                else if(tc == myAnimal0.GetComponent<BoxCollider2D>())
                {
                    //
                }
                else if (tc == myAnimal1.GetComponent<BoxCollider2D>())
                {

                }
                else if (tc == myAnimal2.GetComponent<BoxCollider2D>())
                {

                }
                else
                {
                    StartCoroutine(DeselectCard());
                }
            }
        }
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
        if(card != null)
        {
            card.GetComponent<SpriteRenderer>().sortingOrder -= 5;
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
        card.GetComponent<SpriteRenderer>().sortingOrder += 5;
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
}