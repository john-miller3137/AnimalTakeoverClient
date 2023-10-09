using Network;
using Riptide;
using SharedLibrary;
using UnityEngine;

namespace Game
{
    public class GameMessages
    {
        public static void SendAttackRequest(int animalId, int targetId, int spiritType, int spiritCount)
        {
            Message msg = Message.Create(MessageSendMode.Reliable, (ushort)MessageResponseCodes.AnimalAttackRequest);
            msg.AddString(MessageHandlers.Key);
            msg.AddInt(MessageHandlers.RoomId);
            msg.AddInt(animalId);
            msg.AddInt(targetId);
            msg.AddInt(spiritType);
            msg.AddInt(spiritCount);
            NetworkManager.Singleton.MainClient.Send(msg);
        }

        public static void SendPlantRequest(int itemId, int newX, int newY)
        {
            Message message =
                Message.Create(MessageSendMode.Reliable, (ushort)MessageResponseCodes.PlantSeedRequest);
            message.AddString(MessageHandlers.Key);
            message.AddInt(MessageHandlers.RoomId);
            message.AddInt(itemId).AddInt(newX).AddInt(newY);
            NetworkManager.Singleton.MainClient.Send(message);
        }
        
    }
}