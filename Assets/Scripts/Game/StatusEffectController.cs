using System.Collections;
using Scripts.GameStructure;
using SharedLibrary;
using UnityEngine;

namespace Game
{
    public class StatusEffectController : MonoBehaviour
    {
        private static StatusEffectController instance;
        public static StatusEffectController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<StatusEffectController>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("StatusEffectController");
                        instance = go.AddComponent<StatusEffectController>();
                    }
                }
                return instance;
            }
        }
        public IEnumerator DoStatusEffects(int animalId, byte statusEffectId, int damage, bool isKOed, float healthRatio)
        {
            GameObject animal = AttacksController.Instance.GetAnimal(animalId);
            Vector3 animalPos = animal.transform.position;
            switch (statusEffectId)
            {
                case (byte)StatusEffect.POISON:
                    StartCoroutine(AttacksController.Instance.SpawnHealthText((int) animalPos.x, 
                        (int)animalPos.y, damage, Color.red));
                    StartCoroutine(PlantController.Instance.DoPoisonEffect(animal));
                    break;
                case (byte) StatusEffect.FROZEN:
                    StartCoroutine(PlantController.Instance.DoFrozenEffect(animal));
                    break;
                case (byte) StatusEffect.THAW:
                    StartCoroutine(PlantController.Instance.DoThawEffect(animal));
                    break;
                default:
                    break;
                
            }

            if (GameLogic.Instance.IsPlayerOne && animalId < 3)
            {
                HealthController.Instance.UpdateAnimalHealth(animalId, healthRatio);
            } else if (!GameLogic.Instance.IsPlayerOne && animalId >= 3 && animalId < 6)
            {
                HealthController.Instance.UpdateAnimalHealth(animalId, healthRatio);
            }
            

            if (isKOed)
            {
                StartCoroutine(HealthController.Instance.OnDeadAnimal(animal, animalId));
            }
            

            yield return null;
        }
        public IEnumerator StatusEffectRoutine(GameEventParams gameEventParams)
        {
            int animalId = gameEventParams.animalId;
            byte statusEffectId = gameEventParams.statusEffectId;
            int damage = gameEventParams.addedHealth;
            bool isKOed = gameEventParams.isKOed;
            float healthRatio = gameEventParams.healthRatio;
            yield return DoStatusEffects(animalId, statusEffectId, damage, isKOed, healthRatio);
        }
    }
}