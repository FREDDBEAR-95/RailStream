using RailStream_Server.Models;
using RailStream_Server.UI;
using RailStream_Server_Backend.Managers;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            SubmitDataChooseButton.Click += SubmitButtonClick;
        }

        private void SubmitButtonClick(object sender, RoutedEventArgs e)
        {
            string fromStation = FromTextBox.Text;
            string toStation = ToTextBox.Text;
            DateTime? startDate = DateChooseBox.SelectedDate;
            List<Route> routes;

            using (DatabaseManager databaseManager = new DatabaseManager())
            {
                routes = databaseManager.Routes.ToList<Route>();
            }
            
            if (fromStation != "")
                routes = routes.Where(t => t.DeparturePlace.ToLower().Contains(fromStation.ToLower())).ToList();
            if (toStation != "")
                routes = routes.Where(t => t.Destination.ToLower().Contains(toStation.ToLower())).ToList();
            if (startDate != null)
                routes = routes.Where(t => t.DepartureDate ==  startDate).ToList();

            if (RouteListBox.Items.Count > 0) RouteListBox.Items.Clear();
            foreach (Route route in routes)
            {
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(150) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(150) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(150) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(150) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(150) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(150) });

                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(20) });

                TextBlock textBlockDepaturePlace = new TextBlock(){ Text = route.DeparturePlace, FontSize = 10};
                TextBlock textBlockDestination = new TextBlock() { Text = route.Destination };
                TextBlock textBlockDepatureDate = new TextBlock() { Text = route.DepartureDate.ToShortDateString() };
                TextBlock textBlockDepatureTime = new TextBlock() { Text = route.DepartureTime.ToShortTimeString() };
                TextBlock textBlockArrivalTime = new TextBlock() { Text = route.ArrivalTime.ToShortTimeString() };

                Button chooseButton = new Button() { Content = "Выбрать", Tag = route.RouteId };
                chooseButton.Click += ClickRouteChoose;

                Grid.SetRow(textBlockDepaturePlace, 0);
                Grid.SetColumn(textBlockDepaturePlace, 0);
                Grid.SetRow(textBlockDestination, 0);
                Grid.SetColumn(textBlockDestination, 1);
                Grid.SetRow(textBlockDepatureDate, 0);
                Grid.SetColumn(textBlockDepatureDate, 2);
                Grid.SetRow(textBlockDepatureTime, 0);
                Grid.SetColumn(textBlockDepatureTime, 3);
                Grid.SetRow(textBlockArrivalTime, 0);
                Grid.SetColumn(textBlockArrivalTime, 4);
                Grid.SetRow(chooseButton, 0);
                Grid.SetColumn(chooseButton, 5);

                grid.Children.Add(textBlockDepaturePlace);
                grid.Children.Add(textBlockDestination);
                grid.Children.Add(textBlockDepatureDate);
                grid.Children.Add(textBlockDepatureTime);
                grid.Children.Add(textBlockArrivalTime);
                grid.Children.Add(chooseButton);

                RouteListBox.Items.Add(grid);
            }
        }

        private void ClickRouteChoose(object sender, RoutedEventArgs e)
        {
            var routeId = (int)(((Button)sender).Tag);
            ChooseWagonAndSeat chooseWagonAndSeatWindow = new ChooseWagonAndSeat(routeId);
            chooseWagonAndSeatWindow.Show();
        }
    }
}