using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
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
        if(myAnimal0 != null && myAnimal1 != null && myAnimal2 != null) {
            myAnimals.Add(myAnimal0);
            myAnimals.Add(myAnimal1);
            myAnimals.Add(myAnimal2);
        }
        if (enemyAnimal0 != null && enemyAnimal1 != null && enemyAnimal2 != null) {
            enemyAnimals.Add(enemyAnimal0);
            enemyAnimals.Add(enemyAnimal1);
            enemyAnimals.Add(enemyAnimal2);
        }
    }

    void Update()
    {
        
    }
}
