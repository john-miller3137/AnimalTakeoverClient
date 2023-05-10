
using Riptide;
using SharedLibrary;
using SharedLibrary.Library;
using Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
    public class MessageHandlers : MonoBehaviour
    {
        private static string _key;
        private const int vert_offset = -6;
        private const int hor_offset = -4;

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
            GameLogic.Instance.StartTransition(p1a0id, p1a0x, p1a0y, p1a1id, p1a1x, p1a1y, p1a2id, p1a2x, p1a2y, p2a0id, p2a0x, p2a0y, p2a1id, p2a1x, p2a1y, p2a2id, p2a2x, p2a2y);
        }
        
        [MessageHandler((ushort)MessageResponseCodes.StartTurnSignal)]
        private static void StartGame(Message message)
        {
            Debug.Log("Start turn");
        }

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

        }
        [MessageHandler((ushort)MessageResponseCodes.DrawCard)]
        private static void ReceiveCard(Message message)
        {
            Debug.Log("Card drawn");
            int cardId = message.GetInt();
            CardController.Instance.HandleDrawCard(cardId);
        }

        [MessageHandler((ushort)MessageResponseCodes.CardPlaySuccess)]
        private static void PlayCardSuccess(Message message)
        {
            int cardId = message.GetInt();
            int animalId = message.GetInt();
            byte cardCode = message.GetByte();
            int v1 = message.GetInt();
            int v2 = message.GetInt();
            if (cardCode == (byte)CardCodes.Move)
            {
                MoveController.Instance.MoveAnimal(animalId, v1 + hor_offset, v2 + vert_offset);
            } else if (cardCode == (byte)CardCodes.Attack)
            {
                AttacksController.Instance.DoAttack(cardId,v1, v2);
            }
        }
        [MessageHandler((ushort)MessageResponseCodes.CardPlayDeny)]
        private static void PlayCardDeny(Message message)
        {
            int cardId = message.GetInt();
            int animalId = message.GetInt();
            
        }

        
    }
}