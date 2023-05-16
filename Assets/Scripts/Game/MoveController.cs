using System;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private static MoveController instance;
    public static MoveController Instance
    
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MoveController>();
                if (instance == null)
                {
                    GameObject go = new GameObject("MoveController");
                    instance = go.AddComponent<MoveController>();
                }
            }
            return instance;
        }
    }
    
    [SerializeField] private GameObject Camera, WorldLight, a0, a1, a2, a3, a4, a5;

    
    public void MoveAnimal(int animalId, int xMove, int yMove)
    {
        GameObject animal = GetAnimal(animalId);
        animal.transform.position = new Vector3(xMove, yMove, 0);
        Debug.Log("animal "+ animalId + " moved!");
    }

    private GameObject GetAnimal(int animalId)
    {
        GameObject animal;
        switch (animalId)
        {
            case 0:
                if (GameLogic.Instance.IsPlayerOne)
                {
                    animal = a0;
                }
                else
                {
                    animal = a3;
                }
                
                break;
            case 1:
                if (GameLogic.Instance.IsPlayerOne)
                {
                    animal = a1;
                }
                else
                {
                    animal = a4;
                }
                break;
            case 2:
                if (GameLogic.Instance.IsPlayerOne)
                {
                    animal = a2;
                }
                else
                {
                    animal = a5;
                }
                break;
            case 3:
                if (GameLogic.Instance.IsPlayerOne)
                {
                    animal = a3;
                }
                else
                {
                    animal = a0;
                }
                break;
            case 4:
                if (GameLogic.Instance.IsPlayerOne)
                {
                    animal = a4;
                }
                else
                {
                    animal = a1;
                }
                break;
            case 5:
                if (GameLogic.Instance.IsPlayerOne)
                {
                    animal = a5;
                }
                else
                {
                    animal = a2;
                }
                break;
            default:
                animal = null;
                break;
        }

        return animal;
    }
}
