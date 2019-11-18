using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ChatClient.ServiceChat;
using ChatClient.Models;

namespace ChatClient
{
    // Класс клиента
    public partial class MainWindow : Window, IServiceChatCallback
    {
        // Флаг состояния, подключен или нет
        private bool IsConnected = false;

        // Ссылка на сервис
        private ServiceChatClient client;

        // ИД пользователя
        private int id;

        // Список доступных цветов
        private List<Brush> brushes = new List<Brush>
        {
            Brushes.Black,
            Brushes.Green,
            Brushes.Blue,
            Brushes.Red,
            Brushes.Yellow,
            Brushes.Gray,
            Brushes.Purple,
            Brushes.Pink,
            Brushes.Orange,
            Brushes.Brown
        };

        public MainWindow()
        {
            InitializeComponent();

            // Добавляем поддерживаемые цвета и комнаты в списки выбора
            cmbColor.Items.Add(new ComboBoxItem { Content = "Black", Background = Brushes.Black });
            cmbColor.Items.Add(new ComboBoxItem { Content = "Green", Background = Brushes.Green });
            cmbColor.Items.Add(new ComboBoxItem { Content = "Blue", Background = Brushes.Blue });
            cmbColor.Items.Add(new ComboBoxItem { Content = "Red", Background = Brushes.Red });
            cmbColor.Items.Add(new ComboBoxItem { Content = "Yellow", Background = Brushes.Yellow });
            cmbColor.Items.Add(new ComboBoxItem { Content = "Gray", Background = Brushes.Gray });
            cmbColor.Items.Add(new ComboBoxItem { Content = "Purple", Background = Brushes.Purple });
            cmbColor.Items.Add(new ComboBoxItem { Content = "Pink", Background = Brushes.Pink });
            cmbColor.Items.Add(new ComboBoxItem { Content = "Orange", Background = Brushes.Orange });
            cmbColor.Items.Add(new ComboBoxItem { Content = "Brown", Background = Brushes.Brown });
            cmbColor.SelectedIndex = 0;
            cmbGroup.Items.Add(new ComboBoxItem { IsEnabled = false });
            cmbGroup.Items.Add("Комната 1");
            cmbGroup.Items.Add("Комната 2");
            cmbGroup.Items.Add("Комната 3");
            cmbGroup.SelectedIndex = 1;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // При запуске создаём подключение к сервису
            client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
        }

        // Метод подключения
        public void Connect()
        {
            // Если ещё не подключены, создаём пользователя, заносим в него данные и отправляем его сервису. Получаем обратно назначенный ИД
            if (!IsConnected)
            {
                ServerUser user = new ServerUser
                {
                    Name = tbUserName.Text,
                    Room = cmbGroup.SelectedIndex,
                    Color = (NameColor)cmbColor.SelectedIndex
                };
                id = client.Connect(user);
                tbUserName.IsEnabled = false;
                btnConnect.Content = "Отключиться";
                IsConnected = true;
            }
        }

        // Метод отключения
        public void Disconnect()
        {
            // Если подлючены, отправляем сервису свой ид в методе отключения
            if (IsConnected)
            {
                client.Disconnect(id);
                tbUserName.IsEnabled = true;
                btnConnect.Content = "Подключиться";
                IsConnected = false;
            }
        }

        // Метод получения сообщений с сервиса
        public void MsgCallBack(string msg, int color)
        {
            // Добавляем сообщение в список и прокручиваем окно до него
            lbChat.Items.Add(new ListBoxItem { Content = msg, Foreground = brushes[color] });
            lbChat.ScrollIntoView(lbChat.Items[lbChat.Items.Count - 1]);
        }

        // Нажатие на кнопку подключения
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsConnected)
            {
                Disconnect();
            }
            else
            {
                Connect();
            }
        }

        // Отключаемся при закрытии окна
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Disconnect();
        }

        // Метод отправки сообщения всем
        private void btnToAll_Click(object sender, RoutedEventArgs e)
        {
            // Если строка не пустая, отправляем сообщение сервису, указав свой ИД
            if (String.IsNullOrWhiteSpace(tbMessage.Text))
                return;
            if (IsConnected)
            {
                client.SendMsg(tbMessage.Text, id, 0);
                tbMessage.Text = "";
            }
        }

        // Метод отправки сообщения в комнату
        private void btnToGroup_Click(object sender, RoutedEventArgs e)
        {
            // Если строка не пустая, отправляем сообщение сервису, указав свой ИД и комнату
            if (String.IsNullOrWhiteSpace(tbMessage.Text))
                return;
            if (IsConnected)
            {
                client.SendMsg(tbMessage.Text, id, cmbGroup.SelectedIndex);
                tbMessage.Text = "";
            }
        }

        // Смена цвета - отправляем свой ИД и новый цвет сервису
        private void btnColor_Click(object sender, RoutedEventArgs e)
        {
            client.ChangeColor(id, cmbColor.SelectedIndex);
        }

        // Смена комнаты - отправляем свой ИД и новую комнату сервису
        private void btnRoom_Click(object sender, RoutedEventArgs e)
        {
            client.ChangeRoom(id, cmbGroup.SelectedIndex);
        }

        // Загрузка сохранённых сообщений
        private void btnDb_Click(object sender, RoutedEventArgs e)
        {
            string[] messages = client.GetAllMessages();
            lbChat.Items.Clear();
            foreach (var m in messages)
            {
                lbChat.Items.Add(new ListBoxItem { Content = m });
            }
            lbChat.ScrollIntoView(lbChat.Items[lbChat.Items.Count - 1]);
        }
    }
}
