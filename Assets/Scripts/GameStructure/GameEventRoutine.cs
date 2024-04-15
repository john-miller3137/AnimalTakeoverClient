using System;
using System.Collections;
using System.Linq.Expressions;
using SharedLibrary;
using UnityEditor;
using UnityEngine.Rendering;

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
        public int addedXp;
        public float xpRatio;
        public bool addedMove;
        public bool isDead1;
        public bool isDead2;
        public float healthRatio1;
        public float healthRatio2;
        public int damage;
        public int tX;
        public int tY;
        public int playerNum;
        
        public Func<GameEventParams, IEnumerator> execute;

        public IEnumerator Execute()
        {
            GameEventParams eventParams = new GameEventParams(animalId, targetId, itemId, gameEvent, x, y, plantEffectId,
                crystalKey, crystalId, addedHealth, effectId, oldCrystalKey, oldX, oldY, spiritType, newX, newY, isKOed, 
                spawnNewCrystal, spawnDirt, statusEffectId, healthRatio, attackModifier, addedXp, xpRatio, addedMove,
                isDead1, isDead2, healthRatio1, healthRatio2, damage, tX, tY, playerNum);
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
        public int addedXp;
        public float xpRatio;
        public bool addedMove;
        public bool isDead1;
        public bool isDead2;
        public float healthRatio1;
        public float healthRatio2;
        public int damage;
        public int tX;
        public int tY;
        public int playerNum;

        public GameEventParams(int animalId, int targetId, ItemId itemId, GameEvent gameEvent, int x, int y,
            PlantEffectId plantEffectId,
            byte crystalKey, byte crystalId, int addedHealth, int effectId, int oldCrystalKey, int oldX, int oldY,
            SpiritType spiritType,
            byte newX, byte newY, bool isKOed, bool spawnNewCrystal, bool spawnDirt, byte statusEffectId,
            float healthRatio,
            AttackModifiers[] attackModifier, int addedXp, float xpRatio, bool addedMove, bool isDead1, bool isDead2,
            float healthRatio1,
            float healthRatio2, int damage, int tX, int tY, int playerNum)

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
            this.addedXp = addedXp;
            this.xpRatio = xpRatio;
            this.addedMove = addedMove;
            this.isDead1 = isDead1;
            this.isDead2 = isDead2;
            this.healthRatio1 = healthRatio1;
            this.healthRatio2 = healthRatio2;
            this.damage = damage;
            this.tX = tX;
            this.tY = tY;
            this.playerNum = playerNum;

        }
    }
}