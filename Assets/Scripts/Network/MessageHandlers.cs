
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Game;
using Riptide;
using Scripts.GameStructure;
using SharedLibrary;
using SharedLibrary.Library;
using Unity;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
    public class MessageHandlers : MonoBehaviour
    {
        private static string _key;
        private const int vert_offset = -6;
        private const int hor_offset = -4;
        private const int max_x = 8;
        private const int max_y = 16;

        public static string Key
        {
            get { return _key; }
            set { _key = value; }
        }
        private static int _roomId;

        public static int RoomId
        {
            get { return _roomId; }
            set { _roomId = value; }
        }
        [MessageHandler((ushort)MessageResponseCodes.KeyResponse)]
        private static void HandleKeyResponse(Message message)
        {
            Debug.Log("key repsonse");
            Key = message.GetString();
        }
        [MessageHandler((ushort)MessageResponseCodes.TokenReceived)]
        private static void StartMatchmaking(Message message)
        {
            Message msg = Message.Create(MessageSendMode.Reliable, (ushort)MessageResponseCodes.QueueMatchmaking);
            msg.AddString(Key).AddString(InputLogic.Token);
            try { NetworkManager.Singleton.MainClient.Send(msg); } catch
            {
                Debug.Log("not connected");
            }
        }
        [MessageHandler((ushort)MessageResponseCodes.GameFound)]
        private static void GameFound(Message message)
        {
            int rId = message.GetInt();
            _roomId = rId;
        }
        [MessageHandler((ushort)MessageResponseCodes.StartTranisitionSignal)]
        private static void StartTransition(Message message)
        {
            (byte,byte)[,] gameBoard = new (byte,byte)[Constants.max_x, Constants.max_y];
            ushort client1Id = message.GetUShort();
            byte p1a0id = message.GetByte();
            byte p1a0x = message.GetByte();
            byte p1a0y = message.GetByte();
            byte p1a1id = message.GetByte();
            byte p1a1x = message.GetByte();
            byte p1a1y = message.GetByte();
            byte p1a2id = message.GetByte();
            byte p1a2x = message.GetByte();
            byte p1a2y = message.GetByte();
            byte p2a0id = message.GetByte();
            byte p2a0x = message.GetByte();
            byte p2a0y = message.GetByte();
            byte p2a1id = message.GetByte();
            byte p2a1x = message.GetByte();
            byte p2a1y = message.GetByte();
            byte p2a2id = message.GetByte();
            byte p2a2x = message.GetByte();
            byte p2a2y = message.GetByte();
            
            
            Debug.Log(p1a1id + " " + p1a1x + " " + p1a1y);
            if (NetworkManager.Singleton.MainClient.Id == client1Id)
            {
                Debug.Log("this is player 1");
                GameLogic.Instance.IsPlayerOne = true;
            }
            for (int i = 0; i < Constants.max_x; i++)
            {
                for (int j = 0; j < Constants.max_y; j++)
                {
                    byte crystalKey = message.GetByte();
                    byte crystalId = message.GetByte(); 
                    if (GameLogic.Instance.IsPlayerOne)
                    {
                        gameBoard[i, j] = (crystalKey,crystalId);
                    }
                    else
                    {
                        int x = flipX(i);
                        int y = flipY(j);
                        gameBoard[x, y] = (crystalKey,crystalId);
                    }
                    
                }
            }

            byte c0Id = message.GetByte();
            byte c0Key = message.GetByte();
            byte c1Id = message.GetByte();
            byte c1Key = message.GetByte();
            byte c2Id = message.GetByte();
            byte c2Key = message.GetByte();
            byte c3Id = message.GetByte();
            byte c3Key = message.GetByte();
            byte c4Id = message.GetByte();
            byte c4Key = message.GetByte();
            byte c5Id = message.GetByte();
            byte c5Key = message.GetByte();
            bool c0Dirt = message.GetBool();
            bool c1Dirt = message.GetBool();
            bool c2Dirt = message.GetBool();
            bool c3Dirt = message.GetBool();
            bool c4Dirt = message.GetBool();
            bool c5Dirt = message.GetBool();
            byte item0 = message.GetByte();
            byte item1 = message.GetByte();
            byte item2 = message.GetByte();
            byte item3 = message.GetByte();
            byte item4 = message.GetByte();
            byte item5 = message.GetByte();
           


            GameLogic.Instance.StartTransition(p1a0id, p1a0x, p1a0y, p1a1id, p1a1x, p1a1y, p1a2id, p1a2x, p1a2y, p2a0id, 
                p2a0x, p2a0y, p2a1id, p2a1x, p2a1y, p2a2id, p2a2x, p2a2y,gameBoard,
                c0Id, c0Key, c1Id, c1Key, c2Id, c2Key, c3Id, c3Key, c4Id, 
                c4Key, c5Id, c5Key, c0Dirt, c1Dirt, c2Dirt, c3Dirt, c4Dirt, c5Dirt, item0, item1, item2, 
                item3, item4,item5);
            
            
        }
        
        [MessageHandler((ushort)MessageResponseCodes.StartTurnSignal)]
        private static void StartGame(Message message)
        {
            Debug.Log("Start turn");
        }
        /*
        [MessageHandler((ushort)MessageResponseCodes.DrawHand)]
        private static void RecieveHand(Message message)
        {
            Debug.Log("Hand received");
            int card0id = message.GetInt();
            int card1id = message.GetInt();
            int card2id = message.GetInt();
            int card3id = message.GetInt();
            Debug.Log(card0id + " " + card1id + " " + card2id + " " + card3id);
            GameLogic.Instance.gc0 = CardInfo.idToCardMap[card0id];
            GameLogic.Instance.gc1 = CardInfo.idToCardMap[card1id];
            GameLogic.Instance.gc2 = CardInfo.idToCardMap[card2id];
            GameLogic.Instance.gc3 = CardInfo.idToCardMap[card3id];
            CardController.Instance.UpdateCard(GameLogic.Instance.gc0, GameLogic.Instance.card0);
            CardController.Instance.UpdateCard(GameLogic.Instance.gc1, GameLogic.Instance.card1);
            CardController.Instance.UpdateCard(GameLogic.Instance.gc2, GameLogic.Instance.card2);
            CardController.Instance.UpdateCard(GameLogic.Instance.gc3, GameLogic.Instance.card3);
            int[] handIdentifier = CardController.Instance.gameCards;
            for(int i =0; i < handIdentifier.Length; i++)
            {
                CardController.Instance.gameCards[i] = i;
            }

        }
        [MessageHandler((ushort)MessageResponseCodes.DrawCard)]
        private static void ReceiveCard(Message message)
        {
            
            int cardId = message.GetInt();
            byte handId = message.GetByte();
            Debug.Log("Card drawn" + cardId + " " + handId);
            CardController.Instance.DoDrawCard(cardId, handId);
        }

        [MessageHandler((ushort)MessageResponseCodes.CardPlaySuccess)]
        private static void PlayCardSuccess(Message message)
        {
            
            bool isPlayerOne = GameLogic.Instance.IsPlayerOne;
            int cardId = message.GetInt();
            int animalId = message.GetInt();
            byte cardCode = message.GetByte();
            int v1 = message.GetInt();
            int v2 = message.GetInt();
            byte handId = message.GetByte();
            byte playerId = message.GetByte();
            if (cardCode == (byte)CardCodes.Move)
            {
                if (isPlayerOne)
                {
                    Debug.Log(animalId);
                    MoveController.Instance.MoveAnimal(animalId, v1 + hor_offset, v2 + vert_offset);
                }
                else
                {
                    Debug.Log(animalId);
                    MoveController.Instance.MoveAnimal(animalId, flipX(v1) + hor_offset, flipY(v2) + vert_offset);
                   
                }
                
            } else if (cardCode == (byte)CardCodes.Attack)
            {
                if (isPlayerOne)
                {
                    
                    AttacksController.Instance.DoAttack(cardId,v1, v2);
                }
                else
                {
                    AttacksController.Instance.DoAttack(cardId,v1, v2);
                }
                
            }
            Debug.Log("PlayerId" + playerId);
            if (isPlayerOne && playerId == 1)
            {
                Debug.Log("Player 1 discarding " +cardId );
                CardController.Instance.CallDiscardCard(handId);
            } else if (!isPlayerOne && playerId == 2)
            {
                Debug.Log("Player 2 discarding " + cardId);
                CardController.Instance.CallDiscardCard(handId);
            }


        }
        [MessageHandler((ushort)MessageResponseCodes.CardPlayDeny)]
        private static void PlayCardDeny(Message message)
        {
            int cardId = message.GetInt();
            int animalId = message.GetInt();
            
        }*/
        [MessageHandler((ushort)MessageResponseCodes.SendHealthUpdate)]
        private static void UpdateAnimalHealth(Message message)
        {
            int animalId = message.GetInt();
            float ratio = message.GetFloat();
            if (GameLogic.Instance.IsPlayerOne && animalId >=0 && animalId <3)
            {
                HealthController.Instance.UpdateAnimalHealth(animalId, ratio);
            } else if (!GameLogic.Instance.IsPlayerOne && animalId >= 3 && animalId < 6)
            {
                HealthController.Instance.UpdateAnimalHealth(animalId, ratio);
            } 
        }
        [MessageHandler((ushort)MessageResponseCodes.AnimalDeath)]
        private static void HandleAnimalDeath(Message message)
        {
            int animalId = message.GetInt();
           
        }

        [MessageHandler((ushort)MessageResponseCodes.SendGameBoard)]
        private static void HandleGameBoardMessage(Message message)
        {
            for (int i = 0; i < Constants.max_x; i++)
            {
                for (int j = 0; j < Constants.max_y; j++)
                {
                    byte messageByte = message.GetByte();
                }
            }
        }

        [MessageHandler((ushort)MessageResponseCodes.AnimalMoveSignal)]
        private static void HandleAnimalMove(Message message)
        {
            int animalId = message.GetInt();
            int x = message.GetInt();
            int y = message.GetInt();
            int effectId = message.GetInt();
            int oldX = message.GetInt();
            int oldY = message.GetInt();
            byte crystalId = message.GetByte();
            byte crystalKey = message.GetByte();
            byte oldCrystalKey = message.GetByte();
            int addedHealth = message.GetInt();
            bool spawnDirt = message.GetBool();
            byte itemId = message.GetByte();
            
            GameEventRoutine routine = new GameEventRoutine();
            routine.animalId = animalId;
            routine.targetId = -1;
            routine.gameEvent = GameEvent.Move;
            routine.x = x;
            routine.y = y;
            routine.crystalKey = crystalKey;
            routine.crystalId = crystalId;
            routine.addedHealth = addedHealth;
            routine.effectId = effectId;
            routine.oldCrystalKey = oldCrystalKey;
            routine.spawnDirt = spawnDirt;
            routine.oldX = oldX;
            routine.oldY = oldY;
            routine.execute = MoveController.Instance.MoveRoutine;
            routine.gameEvent = GameEvent.Move;
            routine.itemId = (ItemId)itemId;
            GameEventRoutineManager.Instance.AddRoutine(routine);
            Debug.Log("animalId: " + animalId + " selectedAnimal" + GameLogic.Instance.selectedAnimal);
            if (GameLogic.Instance.IsPlayerOne)
            {
                
                if (GameLogic.Instance.selectedAnimal == animalId)
                {
                    GameLogic.Instance.CallDeselectAnimal();
                }
            }
            else
            {
                if (GameLogic.Instance.selectedAnimal == animalId-3)
                {
                    GameLogic.Instance.CallDeselectAnimal();
                }
            }
            
            
            
            /*
             if (GameLogic.Instance.IsPlayerOne)
            {
                MoveController.Instance.MoveAnimal(animalId, x+hor_offset, y + vert_offset);
                CrystalController.Instance.PickupCrystal(animalId, crystalId, crystalKey, x, y, addedHealth);
                AnimalEffectController.Instance.DoEffect(effectId, oldX + hor_offset, oldY + vert_offset, oldCrystalKey);
            }
            else
            {
                MoveController.Instance.MoveAnimal(animalId, flipX(x)+hor_offset, flipY(y)+vert_offset);
                
                CrystalController.Instance.PickupCrystal(SwitchAnimal(animalId), crystalId, crystalKey,
                    flipX(x), flipY(y), addedHealth);
                AnimalEffectController.Instance.DoEffect(effectId, flipX(oldX) + hor_offset, flipY(oldY) + vert_offset, oldCrystalKey);
            }*/
            
        }
/*
        [MessageHandler((ushort)MessageResponseCodes.PickupCrystal)]
        private static void HandlePickupCrystal(Message message)
        {
            int animalId = message.GetInt();
            byte crystalId = message.GetByte();
            byte crystalKey = message.GetByte();
            bool fullCrystal = message.GetBool();
            int x = message.GetInt();
            int y = message.GetInt();
            if (GameLogic.Instance.IsPlayerOne)
            {
                Debug.Log("crystal picked up");
                
            }
            else
            {
                Debug.Log("crystal picked up");
                CrystalController.Instance.PickupCrystal(SwitchAnimal(animalId), crystalId, crystalKey,
                    flipX(x), flipY(y));
            }
        }*/

        [MessageHandler((ushort)MessageResponseCodes.AnimalAttackSuccess)]
        private static void HandleAttackAction(Message message)
        {
            int animalId = message.GetInt();
            int targetId = message.GetInt();
            int spiritType = message.GetInt();
            int damage = message.GetInt();
            bool isDead = message.GetBool();
            bool spawnNewCrystal = message.GetBool();
            byte newX = message.GetByte();
            byte newY = message.GetByte();
            byte crystalKey = message.GetByte();
            float healthRatio = message.GetFloat();
            byte attackModifierCount = message.GetByte();
            AttackModifiers[] attackModifiers = new AttackModifiers[attackModifierCount];
            for (int i = 0; i < attackModifierCount; i++)
            {
                byte attackModifier = message.GetByte();
                attackModifiers[i] = (AttackModifiers)attackModifier;
            }
            
            Debug.Log(animalId + " " + targetId + " " + spiritType);
            GameEventRoutine routine = new GameEventRoutine();
            routine.animalId = animalId;
            routine.targetId = targetId;
            routine.spiritType = (SpiritType) spiritType;
            routine.addedHealth = damage;
            routine.isKOed = isDead;
            routine.spawnNewCrystal = spawnNewCrystal;
            routine.newX = newX;
            routine.newY = newY;
            routine.crystalKey = crystalKey;
            routine.gameEvent = GameEvent.Attack;
            routine.healthRatio = healthRatio;
            routine.attackModifier = attackModifiers;
            routine.execute = AttacksController.Instance.SpiritAttackRoutine;
            GameEventRoutineManager.Instance.AddRoutine(routine);
            /*
            AttacksController.Instance.DoSpiritAttack(animalId, targetId, (SpiritType) spiritType, damage, isDead,
                spawnNewCrystal, newX, newY, crystalKey);
                */
        }
        [MessageHandler((ushort)MessageResponseCodes.EndTurnSignal)]
        private static void HandleNewTurn(Message message)
        {
            int playerTurn = message.GetInt();
            int plantResponseCount = message.GetInt();
            int effectsResponseCount = message.GetInt();
            int locationEffectsCount = message.GetInt();
            for (int i = 0; i < plantResponseCount; i++)
            {
                byte item1 = message.GetByte();
                byte item2 = message.GetByte();
                byte item3 = message.GetByte();
                byte item4 = message.GetByte();
                byte item5 = message.GetByte();
                bool item6 = message.GetBool();
                Debug.Log($"x:{item1} y: {item2} plantId={item3} animal={item4} statuscode{item5}");
                PlantController.Instance.DoPlantGrowthEffectVoid(item1, item2, item3, item4, item5, item6);
            }

            for (int i = 0; i < effectsResponseCount; i++)
            {
                byte animalId = message.GetByte();
                byte statusId = message.GetByte();
                int damage = message.GetInt();
                bool isKOed = message.GetBool();
                float healthRatio = message.GetFloat();
                GameEventRoutine gameEventParams = new GameEventRoutine();
                gameEventParams.statusEffectId = statusId;
                gameEventParams.animalId = animalId;
                gameEventParams.targetId = -1;
                gameEventParams.gameEvent = GameEvent.EndTurnEffects;
                gameEventParams.addedHealth = damage;
                gameEventParams.isKOed = isKOed;
                gameEventParams.healthRatio = healthRatio;
                gameEventParams.execute = StatusEffectController.Instance.StatusEffectRoutine;
                GameEventRoutineManager.Instance.AddRoutine(gameEventParams);
            }
            
            Debug.Log($"playerTurn: {playerTurn}");
            GameLogic.Instance.UpdateTurn(playerTurn);
        }
        [MessageHandler((ushort)MessageResponseCodes.EndGameSignal)]
        private static void HandleEndGame(Message message)
        {
            int winner = message.GetInt();
            Debug.Log($"winner: Player {winner}");
            GameLogic.Instance.EndGame();
        }

        [MessageHandler((ushort)MessageResponseCodes.PlantSeedSuccess)]
        private static void HandlePlantSuccess(Message message)
        {
            int plantId = message.GetInt();
            int x = message.GetInt();
            int y = message.GetInt();
            int seedId = message.GetInt();
            byte playerNum = message.GetByte();
            
            PlantController.Instance.DoPlant(seedId, plantId, x, y, playerNum);
        }
        private static int flipX(int x)
        {
            return max_x - x;
        }
        private static int flipY(int y)
        {
            return max_y - y;
        }

        public static int SwitchAnimal(int animalId)
        {
            switch (animalId)
            {
                case 0:
                    return 3;
                case 1:
                    return 4;
                case 2:
                    return 5;
                case 3:
                    return 0;
                case 4 :
                    return 1;
                case 5:
                    return 2;
                default:
                    return -1;
            }
        }
        
        

        
    }
}