using System.Collections;
using Game;
using Network;
using Scripts.GameStructure;
using SharedLibrary;
using SharedLibrary.Library;
using SharedLibrary.Objects;
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
        int addedXp = eventParams.addedXp;
        float xpRatio = eventParams.xpRatio;
        bool addedMove = eventParams.addedMove;
        int tX = eventParams.tX;
        int tY = eventParams.tY;
        int playerNum = eventParams.playerNum;
        if (GameLogic.Instance.IsPlayerOne)
        {
            if (playerNum == GameLogic.Instance.playerNum)
            {
                InventoryController.Instance.AddItemToInventoryVoid((int)itemId, x+Constants.hor_offset, y+Constants.vert_offset);
            }
            yield return MoveAnimal(animalId, x+Constants.hor_offset, y + Constants.vert_offset);
            CrystalController.Instance.PickupCrystal(animalId, crystalId, crystalKey, x, y, addedHealth, spawnDirt, tX, tY);
            AnimalEffectController.Instance.DoEffect(effectId, oldX + Constants.hor_offset, 
                oldY + Constants.vert_offset, oldCrystalKey,
                x, y, addedHealth);
            StartCoroutine(AttacksController.Instance.updateXp(x + Constants.hor_offset, y + Constants.vert_offset, addedXp, Color.blue, animalId, xpRatio));
            if (addedMove)
            {
                StartCoroutine(AttacksController.Instance.SpawnAddedMove(x + Constants.hor_offset,
                    y + Constants.vert_offset, AnimalEffectController.Instance.PickRandomEggColor()));
            }
        }
        else
        {
            if (playerNum == GameLogic.Instance.playerNum)
            {
                InventoryController.Instance.AddItemToInventoryVoid((int)itemId, GameLogic.flipX(x)+Constants.hor_offset, GameLogic.flipY(y)+Constants.vert_offset);
            }
            yield return MoveAnimal(animalId, GameLogic.flipX(x)+Constants.hor_offset, GameLogic.flipY(y)+Constants.vert_offset);
            CrystalController.Instance.PickupCrystal(MessageHandlers.SwitchAnimal(animalId), crystalId, crystalKey,
                GameLogic.flipX(x), GameLogic.flipY(y), addedHealth, spawnDirt, GameLogic.flipX(tX), GameLogic.flipY(tY));
            AnimalEffectController.Instance.DoEffect(effectId, GameLogic.flipX(oldX) + Constants.hor_offset, 
                GameLogic.flipY(oldY) + Constants.vert_offset, oldCrystalKey, GameLogic.flipX(x), GameLogic.flipY(y), addedHealth);
            StartCoroutine(AttacksController.Instance.updateXp(GameLogic.flipX(x) + Constants.hor_offset, GameLogic.flipY(y) + Constants.vert_offset, 
                addedXp, Color.blue, animalId, xpRatio));
            if (addedMove)
            {
                StartCoroutine(AttacksController.Instance.SpawnAddedMove(GameLogic.flipX(x) + Constants.hor_offset,
                    GameLogic.flipY(y) + Constants.vert_offset, AnimalEffectController.Instance.PickRandomEggColor()));
            }
        }
        
        yield return null;
    }
    public IEnumerator MoveAnimal(int animalId, int xMove, int yMove)
    {
        GameObject animal = GetAnimal(animalId);
        animal.transform.position = new Vector3(xMove, yMove, 0);
        GameController.Instance.PlayMoveSound();
        CrystalController.Instance.UpdateLayerAnimal(animal);
        CheckForModifiers();
        //Debug.Log("animal "+ animalId + " moved!");
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
            foreach (GameObject dirtTile in PlantController.Instance.dirtTiles.Values)
            {
                Vector3 dirtTilePos = dirtTile.transform.position;
                bool isAtAnimalLocation = false;

                // Check myAnimals
                foreach (var animal in GameLogic.Instance.myAnimals)
                {
                    Vector3 animalPos = animal.transform.position;
                    if (Mathf.RoundToInt(dirtTilePos.x) == Mathf.RoundToInt(animalPos.x) &&
                        Mathf.RoundToInt(dirtTilePos.y) == Mathf.RoundToInt(animalPos.y))
                    {
                        isAtAnimalLocation = true;
                        break;
                    }
                }

                // Check enemyAnimals if not found in myAnimals
                if (!isAtAnimalLocation)
                {
                    foreach (var animal in GameLogic.Instance.enemyAnimals)
                    {
                        Vector3 animalPos = animal.transform.position;
                        if (Mathf.RoundToInt(dirtTilePos.x) == Mathf.RoundToInt(animalPos.x) &&
                            Mathf.RoundToInt(dirtTilePos.y) == Mathf.RoundToInt(animalPos.y))
                        {
                            isAtAnimalLocation = true;
                            break;
                        }
                    }
                }

                // Enable or disable the BoxCollider2D based on whether the dirtTile is at an animal location
                dirtTile.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = !isAtAnimalLocation;
            }
        }
    }

    private GameObject GetAnimal(int animalId)
    {
        GameObject animal;
        int numAnimals = GameLogic.Instance.numberOfAnimals;

        if (animalId == 5 && InputLogic.Instance.isTutorial)
        {
            animal = GameLogic.Instance.myAnimal2;
        }
        else if (GameLogic.Instance.IsPlayerOne)
        {
            animal = animalId < numAnimals / 2 ? GameLogic.Instance.myAnimals[animalId] : GameLogic.Instance.enemyAnimals[animalId - numAnimals / 2];
        }
        else
        {
            animal = animalId < numAnimals / 2 ? GameLogic.Instance.enemyAnimals[animalId] : GameLogic.Instance.myAnimals[animalId - numAnimals / 2];
        }

        return animal;
    }
}
