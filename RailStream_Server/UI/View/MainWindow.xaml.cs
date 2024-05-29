using RailStream_Server.Models;
using RailStream_Server.Models.Other;
using RailStream_Server.Services;
using RailStream_Server_Backend.Managers;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace RailStream_Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            UserManagerService userManagerService = new UserManagerService();

            Dictionary<string, string> content = new Dictionary<string, string>();

            content["Login"] = "Test@mail.ru";
            content["Password"] = "12345";
            content["Address"] = "127.0.0.1";
            content["DeviceName"] = "Windows";

            ClientRequest clientRequest = new ClientRequest
            (
                null,
                JsonSerializer.Serialize(content)
            );

            var message = userManagerService.LoginUser(clientRequest);
            MessageBox.Show(message.Content);
        }
    }
}