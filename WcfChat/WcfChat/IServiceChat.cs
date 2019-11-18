using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace WcfChat
{
    // Интерфейс для сервиса
    [ServiceContract(CallbackContract = typeof(IServerChatCallback))]
    public interface IServiceChat
    {
        // Метод подключения
        [FaultContract(typeof(Exception))]
        [OperationContract]
        int Connect(ServerUser user);

        // Метод отключения
        [OperationContract]
        void Disconnect(int id);

        // Метод отправки сообщения
        [OperationContract(IsOneWay = true)]
        void SendMsg(string msg, int id, int room);

        // Метод смены комнаты
        [FaultContract(typeof(Exception))]
        [OperationContract]
        void ChangeRoom(int id, int room);

        // Метод смены цвета имени
        [FaultContract(typeof(Exception))]
        [OperationContract]
        void ChangeColor(int id, int color);

        [OperationContract]
        string[] GetAllMessages();
    }

    // Интерфейс для отправки данных клиенту
    public interface IServerChatCallback
    {
        [OperationContract(IsOneWay = true)]
        void MsgCallBack(string msg, int color);
    }
}
