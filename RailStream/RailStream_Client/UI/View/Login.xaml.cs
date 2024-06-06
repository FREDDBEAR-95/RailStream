using RailStream_Server.Models.Other;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RailStream_Client.UI.View
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginBox.Text))
            {
                MessageBox.Show("Введите логин", "Вход", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Введите пароль", "Вход", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ServerResponce responce = SendForm().Result;

            Dictionary<string, object> res = JsonSerializer.Deserialize<Dictionary<string, object>>(responce.Content);
            //MessageBox.Show($"Логин:{LoginBox.Text} Пароль:{PasswordBox.Password}", "Вход", MessageBoxButton.OK);
            MessageBox.Show($"{string.Join(Environment.NewLine, res)}", "Вход", MessageBoxButton.OK);
        }

        private void SingUpBtn_Click(object sender, RoutedEventArgs e)
        {
            var SingUpFormMove = new Registration();
            SingUpFormMove.Show();
            this.Close();
        }

        public async Task<ServerResponce> SendForm()
        {
            try
            {

                Dictionary<string, string> Headers = new Dictionary<string, string>() 
                {
                    {"ServiceName", "UserManagerService"},
                    {"Command", "LogIn"}

                };

                try
                {
                    // Создайте TcpClient.
                    using (TcpClient client = new TcpClient())
                    {
                        // Установите соединение с сервером.
                        client.Connect("127.0.0.1", 8080);

                        Dictionary<string, string> Content = new Dictionary<string, string>()
                        {
                            {"Login", LoginBox.Text},
                            {"Password", PasswordBox.Password },
                            {"Address", Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString()},
                            {"DeviceName", Dns.GetHostName()}
                        };

                        MessageBox.Show(string.Join(Environment.NewLine, Content));

                        ClientRequest clientRequest = new ClientRequest(JsonSerializer.Serialize(Headers), JsonSerializer.Serialize(Content));

                        // Получите поток для чтения/записи.
                        using (NetworkStream stream = client.GetStream())
                        {
                            // Сериализуйте и отправьте объект ClientRequest.
                            string req = JsonSerializer.Serialize(clientRequest);
                            byte[] data = Encoding.UTF8.GetBytes(req);
                            stream.Write(data, 0, data.Length);

                            // Получите ответ от сервера.
                            StringBuilder responseBuilder = new StringBuilder();
                            byte[] buffer = new byte[1024];
                            int bytesRead = 0;

                            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                responseBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                            }

                            string response = responseBuilder.ToString(); 
                            return JsonSerializer.Deserialize<ServerResponce>(response) ?? new ServerResponce(false, "Ошибка");
                        }
                    }
                }

                catch (Exception ex)
                {
                    return new ServerResponce(false, "Ошибка");
                }
            }

            catch (Exception ex)
            {
                return new ServerResponce(false, "Ошибка");
            }
        }
    }
}
