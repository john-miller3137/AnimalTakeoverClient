
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Game;
using Riptide;
using Scripts.GameStructure;
using SharedLibrary;
using SharedLibrary.Library;
using SharedLibrary.ReturnCodes;
using Unity;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
        private static void TokenReceived(Message message)
        {
            InputLogic.Instance.ShowOptions();
        }
        
        public static void CancelSearch()
        {
            Message msg = Message.Create(MessageSendMode.Reliable, (ushort)MessageResponseCodes.CancelSearch);
            msg.AddString(Key).AddString(InputLogic.Token);
            try { NetworkManager.Singleton.MainClient.Send(msg); } catch
            {
                Debug.Log("not connected");
            }
        }
        public static void StartMatchmakingDefault()
        {
            Message msg = Message.Create(MessageSendMode.Reliable, (ushort)MessageResponseCodes.QueueMatchmaking);
            msg.AddString(Key).AddString(InputLogic.Token);
            try { NetworkManager.Singleton.MainClient.Send(msg); } catch
            {
                Debug.Log("not connected");
            }
        }
        public static void StartMatchmakingTwos()
        {
            Message msg = Message.Create(MessageSendMode.Reliable, (ushort)MessageResponseCodes.QueueMatchmakingTwos);
            msg.AddString(Key).AddString(InputLogic.Token);
            try { NetworkManager.Singleton.MainClient.Send(msg); } catch
            {
                Debug.Log("not connected");
            }
        }
        public static void StartMatchmakingTwosAI()
        {
            Message msg = Message.Create(MessageSendMode.Reliable, (ushort)MessageResponseCodes.QueueMatchmakingTwosAI);
            msg.AddString(Key).AddString(InputLogic.Token);
            try { NetworkManager.Singleton.MainClient.Send(msg); } catch
            {
                Debug.Log("not connected");
            }
        }
        
        [MessageHandler((ushort) MessageResponseCodes.BoardReset)]
        private static void HandleBoardReset(Message message)
        {
            (byte,byte)[,] gameBoard = new (byte,byte)[Constants.max_x, Constants.max_y];
            for (int i = 0; i < Constants.max_x; i++)
            {
                for (int j = 0; j < Constants.max_y; j++)
                {
                    byte crystalKey = message.GetByte();
                    byte crystalId = message.GetByte();
                    if (GameLogic.Instance.IsPlayerOne)
                    {
                        gameBoard[i, j] = (crystalKey, crystalId);
                    }
                    else
                    {
                        int x = flipX(i);
                        int y = flipY(j);
                        gameBoard[x, y] = (crystalKey, crystalId);
                    }

                }
            }

            GameLogic.Instance.DoBoardReset(gameBoard);
        }
        [MessageHandler((ushort)MessageResponseCodes.GameFound)]
        private static void GameFound(Message message)
        {
            int rId = message.GetInt();
            string player1Name = message.GetString();
            string player2Name = message.GetString();
            _roomId = rId;
            GameController.Instance.player1Name = player1Name;
            GameController.Instance.player2Name = player2Name;
            
        }
        [MessageHandler((ushort)MessageResponseCodes.StartTranisitionSignal)]
        private static void StartTransition(Message message)
        {
            (byte,byte)[,] gameBoard = new (byte,byte)[Constants.max_x, Constants.max_y];
            GameMode gameMode = (GameMode)message.GetInt();
            ushort[] teamOneIDs;
            int number_of_animals;
            if (gameMode == GameMode.TWOS || gameMode == GameMode.TWOS_AI)
            {
                teamOneIDs = new ushort[2];
                ushort client1Id = message.GetUShort();
                ushort client2Id = message.GetUShort();
                teamOneIDs[0] = client1Id;
                teamOneIDs[1] = client2Id;
                number_of_animals = 8;
            }
            else
            {
                teamOneIDs = new ushort[1];
                ushort client1Id = message.GetUShort();
                teamOneIDs[0] = client1Id;
                number_of_animals = 6;
            }

            AnimalBoardEntry[] animalResponse = new AnimalBoardEntry[number_of_animals];
            for (int i = 0; i < number_of_animals; i++)
            {
                byte aId = message.GetByte();
                byte aX = message.GetByte();
                byte aY = message.GetByte();
                AnimalBoardEntry abe = new AnimalBoardEntry();
                abe.Id = aId;
                abe.X = aX;
                abe.Y = aY;
                animalResponse[i] = abe;
            }


            // Debug.Log(p1a1id + " " + p1a1x + " " + p1a1y);
            if (teamOneIDs.Contains(NetworkManager.Singleton.MainClient.Id))
            {
                //Debug.Log("this is player 1");
                GameLogic.Instance.IsPlayerOne = true;
            }
            else
            {
                GameLogic.Instance.IsPlayerOne = false;
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

            CheckForCrystalReturn[] crystalPickupResponse = new CheckForCrystalReturn[number_of_animals];
            for (int i = 0; i < number_of_animals; i++)
            {
                byte crystalId = message.GetByte();
                byte crystalKey = message.GetByte();
                bool spawnDirt = message.GetBool();
                byte itemId = message.GetByte();
                CheckForCrystalReturn ccr = new CheckForCrystalReturn();
                ccr.CrystalId = crystalId;
                ccr.CrystalKey = crystalKey;
                ccr.SpawnDirt = spawnDirt;
                ccr.ItemId = itemId;
                crystalPickupResponse[i] = ccr;
            }

            int playerNum = message.GetInt();

            Debug.Log("player numb : " + playerNum);
            GameLogic.Instance.StartTransition(animalResponse,gameBoard,
                crystalPickupResponse, gameMode, number_of_animals, playerNum);
            
            
        }
        
        [MessageHandler((ushort)MessageResponseCodes.StartTurnSignal)]
        private static void StartGame(Message message)
        {
            //Debug.Log("Start turn");
        }
        
        [MessageHandler((ushort)MessageResponseCodes.SendHealthUpdate)]
        private static void UpdateAnimalHealth(Message message)
        {
            int animalId = message.GetInt();
            float ratio = message.GetFloat();
            if (GameHelperMethods.GetPlayerFromAnimal(animalId, GameLogic.Instance.numberOfAnimals,
                    GameLogic.Instance.numberOfPlayers) == GameLogic.Instance.playerNum) 
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
            int addedXp = message.GetInt();
            float xpRatio = message.GetFloat();
            bool addedMove = message.GetBool();
            int tX = message.GetInt();
            int tY = message.GetInt();
            int playerNum = message.GetInt();
            Debug.Log("tx: " + tX + " tY: " + tY);
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
            routine.addedXp = addedXp;
            routine.xpRatio = xpRatio;
            routine.addedMove = addedMove;
            routine.tX = tX;
            routine.tY = tY;
            routine.playerNum = playerNum;
            GameEventRoutineManager.Instance.AddRoutine(routine);
            //Debug.Log("animalId: " + animalId + " selectedAnimal" + GameLogic.Instance.selectedAnimal);
            if (GameLogic.Instance.IsPlayerOne)
            {
                
                if (GameLogic.Instance.selectedAnimal == animalId)
                {
                    GameLogic.Instance.CallDeselectAnimal();
                }
            }
            else
            {
                if (GameLogic.Instance.selectedAnimal == animalId-GameLogic.Instance.numberOfAnimals/2)
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
            int addedXp = message.GetInt();
            float xpRatio = message.GetFloat();
            bool isDead1 = message.GetBool();
            bool isDead2 = message.GetBool();
            float healthRatio1 = message.GetFloat();
            float healthRatio2 = message.GetFloat();
            int addedHealth = message.GetInt();
            int tX = message.GetInt();
            int tY = message.GetInt();
            int playerNum = message.GetInt();
            byte attackModifierCount = message.GetByte();
            AttackModifiers[] attackModifiers = new AttackModifiers[attackModifierCount];
            for (int i = 0; i < attackModifierCount; i++)
            {
                byte attackModifier = message.GetByte();
                attackModifiers[i] = (AttackModifiers)attackModifier;
            }
            
            //Debug.Log(animalId + " " + targetId + " " + spiritType);
            GameEventRoutine routine = new GameEventRoutine();
            routine.animalId = animalId;
            routine.targetId = targetId;
            routine.spiritType = (SpiritType) spiritType;
            routine.damage = damage;
            routine.isKOed = isDead;
            routine.spawnNewCrystal = spawnNewCrystal;
            routine.newX = newX;
            routine.newY = newY;
            routine.crystalKey = crystalKey;
            routine.gameEvent = GameEvent.Attack;
            routine.healthRatio = healthRatio;
            routine.attackModifier = attackModifiers;
            routine.execute = AttacksController.Instance.SpiritAttackRoutine;
            routine.addedXp = addedXp;
            routine.xpRatio = xpRatio;
            routine.isDead1 = isDead1;
            routine.isDead2 = isDead2;
            routine.healthRatio1 = healthRatio1;
            routine.healthRatio2 = healthRatio2;
            routine.addedHealth = addedHealth;
            routine.tX = tX;
            routine.tY = tY;
            routine.playerNum = playerNum;
            GameEventRoutineManager.Instance.AddRoutine(routine);
            
            /*
            AttacksController.Instance.DoSpiritAttack(animalId, targetId, (SpiritType) spiritType, damage, isDead,
                spawnNewCrystal, newX, newY, crystalKey);
                */
        }
        
        [MessageHandler((ushort)MessageResponseCodes.EmoteSuccess)]
        private static void HandleEmoteSuccess(Message message)
        {
            int playerSender = message.GetInt();
            int emoteCode = message.GetInt();
            EmoteController.Instance.ShowEmoteVoid((EmoteCodes)emoteCode, playerSender);
        }

        [MessageHandler((ushort)MessageResponseCodes.AnimalMoveFailure)]
        private static void HandleNoMove(Message message)
        {
            AttacksController.Instance._cameraShake.Shake(.8f);
        }
        [MessageHandler((ushort)MessageResponseCodes.EndTurnSignal)]
        private static void HandleNewTurn(Message message)
        {
            int playerTurn = message.GetInt();
            int plantResponseCount = message.GetInt();
            int effectsResponseCount = message.GetInt();
            int animalEffectModifiers = message.GetInt();
            for (int i = 0; i < plantResponseCount; i++)
            {
                byte item1 = message.GetByte();
                byte item2 = message.GetByte();
                byte item3 = message.GetByte();
                byte item4 = message.GetByte();
                byte item5 = message.GetByte();
                bool item6 = message.GetBool();
                //Debug.Log($"x:{item1} y: {item2} plantId={item3} animal={item4} statuscode{item5}");
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

            for (int i = 0; i < animalEffectModifiers; i++)
            {
                int animalIdModCode = message.GetInt();
                int animalIdModNum = message.GetInt();
                GameEventRoutine gameEventRoutine = new GameEventRoutine();
                gameEventRoutine.statusEffectId = (byte)animalIdModCode;
                gameEventRoutine.animalId = i;
                gameEventRoutine.addedHealth = animalIdModNum;
                gameEventRoutine.gameEvent = GameEvent.EndTurnEffects;
                gameEventRoutine.execute = AnimalEffectController.Instance.DoEndTurnModifier;
                GameEventRoutineManager.Instance.AddRoutine(gameEventRoutine);
            }
            
            
            //Debug.Log($"playerTurn: {playerTurn}");
            GameLogic.Instance.UpdateTurn(playerTurn);
        }
        [MessageHandler((ushort)MessageResponseCodes.EndGameSignal)]
        private static void HandleEndGame(Message message)
        {
            EndGameValue egv = (EndGameValue)message.GetInt();
            byte a0 = message.GetByte();
            int d0 = message.GetInt();
            String s0 = message.GetString();
            byte a1 = message.GetByte();
            int d1 = message.GetInt();
            String s1 = message.GetString();
            byte a2 = message.GetByte();
            int d2 = message.GetInt();
            String s2 = message.GetString();
            EndGameController.Instance.ShowEndGame(egv, a0, a1, a2, d0, d1, d2, s0, s1, s2);
            //Debug.Log($"winner: Player {winner}");
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
        
        [MessageHandler((ushort)MessageResponseCodes.CrateOpenResponse)]
        private static void HandleCrateOpen(Message message)
        {
            int animalId = message.GetInt();
            TransitionController.Instance.DoCrateOpenVoid(animalId);
        }
        
        [MessageHandler((ushort)MessageResponseCodes.CrateOpenFailure)]
        private static void HandleCrateFailure(Message message)
        {
            Debug.Log("failed");
            GameLogic.Instance.EndGame();
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
            int totalAnimals = GameLogic.Instance.numberOfAnimals;
            int halfAnimals = totalAnimals / 2;
            if (animalId < halfAnimals)
            {
                return animalId + halfAnimals;
            }
            else if (animalId < totalAnimals)
            {
                return animalId - halfAnimals;
            }
            else
            {
                return -1; 
            }
        }
        
        

        
    }
}