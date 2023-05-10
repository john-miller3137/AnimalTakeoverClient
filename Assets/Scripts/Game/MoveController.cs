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
                animal = a0;
                break;
            case 1:
                animal = a1;
                break;
            case 2:
                animal = a2;
                break;
            case 3:
                animal = a3;
                break;
            case 4:
                animal = a4;
                break;
            case 5:
                animal = a5;
                break;
            default:
                animal = null;
                break;
        }

        return animal;
    }
}
