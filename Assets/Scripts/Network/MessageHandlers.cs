using Riptide;
using SharedLibrary;

namespace Network
{
    public class MessageHandlers
    {
        private static string _key;

        public static string Key
        {
            get { return _key; }
            set { _key = value; }
        }
        [MessageHandler((ushort) MessageResponseCodes.KeyResponse)]
        private static void HandleKeyResponse(Message message)
        {
            Key = message.GetString();
        }
        [MessageHandler((ushort) MessageResponseCodes.TokenReceived)]
        private static void StartMatchmaking(Message message)
        {
            Message msg = Message.Create(MessageSendMode.Reliable, (ushort)MessageResponseCodes.QueueMatchmaking);
            msg.AddString(Key).AddString(InputLogic.Token);
            NetworkManager.MainClient.Send(msg);
        }
        [MessageHandler((ushort)MessageResponseCodes.StartTurnSignal)]
        private static void StartGame(Message message)
        {
            InputLogic.LoadGameScene1();
        }
    }
}