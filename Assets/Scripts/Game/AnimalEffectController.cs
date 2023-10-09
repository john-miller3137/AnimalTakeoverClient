using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Server.Game;
using SharedLibrary;
using UnityEngine;

namespace Game
{
    public class AnimalEffectController : MonoBehaviour
    {
        private static AnimalEffectController instance;
        public static AnimalEffectController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AnimalEffectController>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("AnimalEffectController");
                        instance = go.AddComponent<AnimalEffectController>();
                    }
                }
                return instance;
            }
        }


        [SerializeField] private GameObject chickenEgg, chickenEggEffect, elephantDirtEffect, dragonFireEffect, clamShell
            ;

        public void DoEffect(int effectId, int x, int y, int crystalKey, int newX, int newY, int addedHealth)
        {
            Debug.Log((PassiveEffectIds)effectId);
            switch (effectId)
            {
                case 0 :
                    break;
                case (int)PassiveEffectIds.ChickenEggEffect:
                    ChickenLayEggEffect(x, y, crystalKey);
                    break;
                case (int)PassiveEffectIds.HedgehogDirtEffect:
                    PlantController.Instance.SpawnDirtTile(x-Constants.hor_offset, y-Constants.vert_offset, true);
                    break;
                case (int) PassiveEffectIds.DeerPlantEffect:
                    StartCoroutine(DeerEatEffect(newX, newY, addedHealth));
                    break;
                case (int) PassiveEffectIds.SealClamShellEffect:
                    StartCoroutine(DoSealEffect(x, y, crystalKey));
                    break;
            }
            
        }
        public void ChickenLayEggEffect(int x, int y, int crystalKey)
        {
            GameObject egg = Instantiate(chickenEgg, new Vector3(x, y, 0), Quaternion.identity);
            GameLogic.Instance.CrystalMap[(byte)(crystalKey)] = egg;
            GameObject effects = Instantiate(chickenEggEffect, new Vector3(x, y, 0), Quaternion.identity);
            Destroy(effects, 2f);
        }

        public IEnumerator DoElephantEffect(int animalId)
        {
            GameObject animal = AttacksController.Instance.GetAnimal(animalId);

            Vector3 animalLoc = animal.transform.position;
            GameObject dirtPArticles = Instantiate(elephantDirtEffect, animalLoc, Quaternion.identity);
            StartCoroutine(AttacksController.Instance.SpawnHealthText(Mathf.RoundToInt(animalLoc.x), Mathf.RoundToInt(animalLoc.y),
                ConstantVars.elephant_dirt_health, Color.green));
            Destroy(dirtPArticles, 1f);
            
            yield return null;
        }

        public IEnumerator DoDragonEffect(int animalId)
        {
            GameObject animal = AttacksController.Instance.GetAnimal(animalId);

            Vector3 animalLoc = animal.transform.position;
            GameObject fireParticles = Instantiate(dragonFireEffect, animalLoc, Quaternion.identity);
            Destroy(fireParticles, 1f);
            yield return null;
        }

        public IEnumerator DoHippoEffect(int animalId)
        {
            GameObject animal = AttacksController.Instance.GetAnimal(animalId);

            Vector3 animalLoc = animal.transform.position;
            GameObject dirtPArticles = Instantiate(elephantDirtEffect, animalLoc, Quaternion.identity);
            
            Destroy(dirtPArticles, 1f);
            
            yield return null;
        }

        private IEnumerator DeerEatEffect(int x, int y, int addedHealth)
        {
            GameObject plant = PlantController.Instance.plantObjects[(x, y)];
            Destroy(plant);
            PlantController.Instance.plantObjects.Remove((x, y));
            Debug.Log("plant destroyed at " + x + " " + y);

            StartCoroutine(AttacksController.Instance.SpawnHealthText(x+Constants.hor_offset, y+Constants.vert_offset,
                addedHealth, addedHealth == 0 ? Color.gray : Color.green));
            yield return null;
        }

        private IEnumerator DoSealEffect(int x, int y, int crystalKey)
        {
            GameObject egg = Instantiate(clamShell, new Vector3(x, y, 0), Quaternion.identity);
            GameLogic.Instance.CrystalMap[(byte)(crystalKey)] = egg;
            GameObject effects = Instantiate(chickenEggEffect, new Vector3(x, y, 0), Quaternion.identity);
            Destroy(effects, 2f);
            yield return null;
        }
    }
}