using System;
using System.Collections;
using System.Linq.Expressions;
using SharedLibrary;
using UnityEditor;

namespace Scripts.GameStructure
{
    public class GameEventRoutine
    {
        public int animalId;
        public int targetId;
        public ItemId itemId;
        public GameEvent gameEvent;
        public int x;
        public int y;
        public PlantEffectId plantEffectId;
        public byte crystalKey;
        public byte crystalId;
        public int addedHealth;
        public int effectId;
        public int oldCrystalKey;
        public int oldX;
        public int oldY;
        public SpiritType spiritType;
        public byte newX;
        public byte newY;
        public bool isKOed;
        public bool spawnNewCrystal;
        public bool spawnDirt;
        public byte statusEffectId;
        public float healthRatio;
        public AttackModifiers[] attackModifier;
        
        public Func<GameEventParams, IEnumerator> execute;

        public IEnumerator Execute()
        {
            GameEventParams eventParams = new GameEventParams(animalId, targetId, itemId, gameEvent, x, y, plantEffectId,
                crystalKey, crystalId, addedHealth, effectId, oldCrystalKey, oldX, oldY, spiritType, newX, newY, isKOed, 
                spawnNewCrystal, spawnDirt, statusEffectId, healthRatio, attackModifier);
            yield return execute?.Invoke(eventParams);
        }
    }

    public class GameEventParams
    {
        public int animalId;
        public int targetId;
        public ItemId itemId;
        public GameEvent gameEvent;
        public int x;
        public int y;
        public PlantEffectId plantEffectId;
        public byte crystalKey;
        public byte crystalId;
        public int addedHealth;
        public int effectId;
        public int oldCrystalKey;
        public int oldX;
        public int oldY;
        public SpiritType spiritType;
        public byte newX;
        public byte newY;
        public bool isKOed;
        public bool spawnNewCrystal;
        public bool spawnDirt;
        public byte statusEffectId;
        public float healthRatio;
        public AttackModifiers[] attackModifier;

        public GameEventParams(int animalId, int targetId, ItemId itemId, GameEvent gameEvent, int x, int y, PlantEffectId plantEffectId,
            byte crystalKey, byte crystalId, int addedHealth, int effectId, int oldCrystalKey, int oldX, int oldY, SpiritType spiritType,
            byte newX, byte newY, bool isKOed, bool spawnNewCrystal, bool spawnDirt, byte statusEffectId, float healthRatio,
            AttackModifiers[] attackModifier)
        {
            this.animalId = animalId;
            this.targetId = targetId;
            this.itemId = itemId;
            this.gameEvent = gameEvent;
            this.x = x;
            this.y = y;
            this.plantEffectId = plantEffectId;
            this.crystalKey = crystalKey;
            this.crystalId = crystalId;
            this.addedHealth = addedHealth;
            this.effectId = effectId;
            this.oldCrystalKey = oldCrystalKey;
            this.oldX = oldX;
            this.oldY = oldY;
            this.spiritType = spiritType;
            this.newX = newX;
            this.newY = newY;
            this.isKOed = isKOed;
            this.spawnNewCrystal = spawnNewCrystal;
            this.spawnDirt = spawnDirt;
            this.statusEffectId = statusEffectId;
            this.healthRatio = healthRatio;
            this.attackModifier = attackModifier;
        }
    }
}