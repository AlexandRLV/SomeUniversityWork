using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using WcfChat.Models;

namespace WcfChat
{
    // Реализация интерфейса сервиса
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceChat : IServiceChat
    {
        public DbChat db = new DbChat();

        // Список пользователей
        public List<ServerUser> users = new List<ServerUser>();

        // Реализация метода смены цвета
        public void ChangeColor(int id, int color)
        {
            // Если полученный цвет не поддерживается, отправляем ошибку
            if (color > 9)
                throw new Exception("Некорректный номер цвета");

            // Ищем пользователя, если такой есть, меняем цвет
            var user = users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                user.Color = (NameColor)color;
            }
        }

        // Реализация метода смены комнаты
        public void ChangeRoom(int id, int room)
        {
            // Если такой комнаты нет, отправляем ошибку
            if (room > 3)
                throw new Exception("Некорректный номер комнаты");

            // Ищем пользователя, если такой есть, меняем ему комнату
            var user = users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                user.Room = room;
            }
        }

        // Реализация метода подключения
        public int Connect(ServerUser user)
        {
            // Если имя пустое или нет такой комнаты, отправляем ошибку
            if (String.IsNullOrWhiteSpace(user.Name))
                throw new Exception("Некорректное имя пользователя");
            if (user.Room > 3)
                throw new Exception("Некорректный номер комнаты");

            // Присваиваем ид новому клиенту, отправляем сообщение о его приходе и сохраняем его в списке
            user.Id = users.Count + 1;
            user.operationContext = OperationContext.Current;
            SendMsg($": {user.Name} подключился, комната {user.Room}.", 0, 0);
            users.Add(user);
            return user.Id;
        }

        // Реализация метода отключения
        public void Disconnect(int id)
        {
            // Ищем клиента, если находим, удаляем из списка и отправляем сообщение об отключении
            var user = users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                users.Remove(user);
                SendMsg($": {user.Name} отключился.", 0, 0);
            }
        }

        // Возвращаем список сохранённых в базе сообщений в нужном формате
        public string[] GetAllMessages()
        {
            List<string> messages = new List<string>();
            foreach (var m in db.Messages)
            {
                messages.Add($"From data ({m.DateTime.ToShortDateString()}): {m.Text}");
            }
            return messages.ToArray();
        }

        // Реализация метода отправки сообщения
        public void SendMsg(string msg, int id, int room)
        {
            var user = users.FirstOrDefault(x => x.Id == id);
            // Формируем строку сообщения, добавляем в неё дату, имя пользователя, комнату и сам текст
            string message = DateTime.Now.ToShortTimeString();
            int color = 0;
            if (user != null)
            {
                message += $": {user.Name}";
                if (room != 0)
                    message += $" (комната {room})";
                else
                    message += " (всем)";
                message += ": ";
                color = (int)user.Color;
            }
            message += msg;
            // Проходим по тем клиентам в списке, кто в этой же комнате (по всем - если сообщение для всех)
            foreach (var item in users)
            {
                if (room == 0 || room == item.Room)
                {
                    // Отправляем сообщение пользователю, указав, какой цвет имени у отправителя
                    item.operationContext.GetCallbackChannel<IServerChatCallback>().MsgCallBack(message, color);
                }
            }

            // Добавляем сообщение в базу данных
            db.Messages.Add(new Message { DateTime = DateTime.Now, Text = message });
            db.SaveChanges();
        }
    }
}
