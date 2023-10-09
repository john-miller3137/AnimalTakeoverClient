using System.Collections;
using Game;
using Network;
using Scripts.GameStructure;
using SharedLibrary;
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


    public IEnumerator MoveRoutine(GameEventParams eventParams)
    {
        int animalId = eventParams.animalId;
        int x = eventParams.x;
        int y = eventParams.y;
        byte crystalKey = eventParams.crystalKey;
        byte crystalId = eventParams.crystalId;
        int addedHealth = eventParams.addedHealth;
        int effectId = eventParams.effectId;
        int oldX = eventParams.oldX;
        int oldY = eventParams.oldY;
        int oldCrystalKey = eventParams.oldCrystalKey;
        bool spawnDirt = eventParams.spawnDirt;
        ItemId itemId = eventParams.itemId;
        if (GameLogic.Instance.IsPlayerOne)
        {
            if (animalId < 3)
            {
                InventoryController.Instance.AddItemToInventoryVoid((int)itemId, x+Constants.hor_offset, y+Constants.vert_offset);
            }
            yield return MoveAnimal(animalId, x+Constants.hor_offset, y + Constants.vert_offset);
            CrystalController.Instance.PickupCrystal(animalId, crystalId, crystalKey, x, y, addedHealth, spawnDirt);
            AnimalEffectController.Instance.DoEffect(effectId, oldX + Constants.hor_offset, oldY + Constants.vert_offset, oldCrystalKey,
                x, y, addedHealth);
        }
        else
        {
            if (animalId >= 3 && animalId <= 5)
            {
                InventoryController.Instance.AddItemToInventoryVoid((int)itemId, GameLogic.flipX(x)+Constants.hor_offset, GameLogic.flipY(y)+Constants.vert_offset);
            }
            yield return MoveAnimal(animalId, GameLogic.flipX(x)+Constants.hor_offset, GameLogic.flipY(y)+Constants.vert_offset);
            CrystalController.Instance.PickupCrystal(MessageHandlers.SwitchAnimal(animalId), crystalId, crystalKey,
                GameLogic.flipX(x), GameLogic.flipY(y), addedHealth, spawnDirt);
            AnimalEffectController.Instance.DoEffect(effectId, GameLogic.flipX(oldX) + Constants.hor_offset, 
                GameLogic.flipY(oldY) + Constants.vert_offset, oldCrystalKey, GameLogic.flipX(x), GameLogic.flipY(y), addedHealth);
        }
        
        yield return null;
    }
    public IEnumerator MoveAnimal(int animalId, int xMove, int yMove)
    {
        GameObject animal = GetAnimal(animalId);
        animal.transform.position = new Vector3(xMove, yMove, 0);
        GameController.Instance.PlayMoveSound();
        CheckForModifiers();
        Debug.Log("animal "+ animalId + " moved!");
        yield return null;
    }

    public void CheckForModifiers()
    {
        UpdateDirtTileColliders();
        
    }

    public void UpdateDirtTileColliders()
    {
        lock (PlantController.Instance.dirtLock)
        {
            for (int i = 0; i < PlantController.Instance.dirtTiles.Count; i++)
            {
                GameObject dirtTile = PlantController.Instance.dirtTiles[i];
                Vector3 dirtTilePos = dirtTile.transform.position;
                Vector3 animal0Pos = GameLogic.Instance.myAnimal0.transform.position;
                Vector3 animal1Pos = GameLogic.Instance.myAnimal1.transform.position;
                Vector3 animal2Pos = GameLogic.Instance.myAnimal2.transform.position;
                Vector3 animal3Pos = GameLogic.Instance.enemyAnimal0.transform.position;
                Vector3 animal4Pos = GameLogic.Instance.enemyAnimal1.transform.position;
                Vector3 animal5Pos = GameLogic.Instance.enemyAnimal2.transform.position;
                
                if(((Mathf.RoundToInt(dirtTilePos.x) != Mathf.RoundToInt(animal0Pos.x)) || 
                    (Mathf.RoundToInt(dirtTilePos.y) != Mathf.RoundToInt(animal0Pos.y))) &&
                    ((Mathf.RoundToInt(dirtTilePos.x) != Mathf.RoundToInt(animal1Pos.x)) ||
                      (Mathf.RoundToInt(dirtTilePos.y) != Mathf.RoundToInt(animal1Pos.y))) && 
                    ((Mathf.RoundToInt(dirtTilePos.x) != Mathf.RoundToInt(animal2Pos.x)) ||
                     (Mathf.RoundToInt(dirtTilePos.y) != Mathf.RoundToInt(animal2Pos.y))) && 
                    ((Mathf.RoundToInt(dirtTilePos.x) != Mathf.RoundToInt(animal3Pos.x)) ||
                     (Mathf.RoundToInt(dirtTilePos.y) != Mathf.RoundToInt(animal3Pos.y))) && 
                    ((Mathf.RoundToInt(dirtTilePos.x) != Mathf.RoundToInt(animal4Pos.x)) || 
                     (Mathf.RoundToInt(dirtTilePos.y) != Mathf.RoundToInt(animal4Pos.y))) &&
                    ((Mathf.RoundToInt(dirtTilePos.x) != Mathf.RoundToInt(animal5Pos.x)) ||
                     (Mathf.RoundToInt(dirtTilePos.y) != Mathf.RoundToInt(animal5Pos.y))))
                {
                    dirtTile.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
                }
                else
                {
                    dirtTile.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
        
    }

    private GameObject GetAnimal(int animalId)
    {
        GameObject animal;
        switch (animalId)
        {
            case 0:
                animal = GameLogic.Instance.IsPlayerOne ? a0 : a3;
                
                break;
            case 1:
                animal = GameLogic.Instance.IsPlayerOne ? a1 : a4;
                break;
            case 2:
                animal = GameLogic.Instance.IsPlayerOne ? a2 : a5;
                break;
            case 3:
                animal = GameLogic.Instance.IsPlayerOne ? a3 : a0;
                break;
            case 4:
                animal = GameLogic.Instance.IsPlayerOne ? a4 : a1;
                break;
            case 5:
                animal = GameLogic.Instance.IsPlayerOne ? a5 : a2;
                break;
            default:
                animal = null;
                break;
        }

        return animal;
    }
}
