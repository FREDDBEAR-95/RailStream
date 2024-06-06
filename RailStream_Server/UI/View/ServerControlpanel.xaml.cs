using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Net.Sockets;
using RailStream_Server.Services;

namespace RailStream_Server.UI.View
{
    /// <summary>
    /// Логика взаимодействия для ServerControlPanel.xaml
    /// </summary>
    public partial class ServerControlPanel : Window
    {
        Server RailStreamServer;

        public ServerControlPanel()
        {
            RailStreamServer = new Server(8080);
            InitializeComponent();
            StatusServer.Content = RailStreamServer.ServerStatus;
            RailStreamServer.serviceManager.AddService(new UserManagerService());
            RailStreamServer.serviceManager.StartServices();
        }

        public ServerControlPanel(Server server)
        {
            RailStreamServer = server;
            InitializeComponent();
            StatusServer.Content = RailStreamServer.ServerStatus;
            RailStreamServer.serviceManager.AddService(new UserManagerService());
            RailStreamServer.serviceManager.StartServices();
        }

        private void BtnRunServer_Click(object sender, RoutedEventArgs e)
        {
            RailStreamServer.Open();
            StatusServer.Content = RailStreamServer.ServerStatus;
        }

        private void BtnStopServer_Click(object sender, RoutedEventArgs e)
        {
            RailStreamServer.Close();
            StatusServer.Content = RailStreamServer.ServerStatus;
        }

        private void BtnEditDataFromDB_Click(object sender, RoutedEventArgs e)
        {
            var routesMove = new RoutesEditingWindow(RailStreamServer);
            routesMove.Show();
            this.Close();
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
