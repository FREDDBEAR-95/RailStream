using RailStream_Server_Backend.Managers;
using System;
using System.Collections.Generic;
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
using RailStream_Server.Models;
using RailStream_Server.Models.Other;
using System.Text.Json;
using RailStream_Server.Services;

namespace RailStream_Server.UI
{
    /// <summary>
    /// Логика взаимодействия для ChooseWagonAndSeat.xaml
    /// </summary>
    public partial class ChooseWagonAndSeat : Window
    {
        private int UserId { get; set; }
        private int ChoosedRouteId { get; set; }
        private Wagon? ChoosedWagon { get; set; } = null;
        private int? ChoosedPlace { get; set; } = null;
        private TicketManagerService ticketManagerService { get; set; } = new TicketManagerService();
        public ChooseWagonAndSeat(int routeId, int userId)
        {
            InitializeComponent();
            ChoosedRouteId = routeId;
            UserId = userId;

            using (DatabaseManager dbManager = new DatabaseManager())
            {
                Route? route = dbManager.Routes.Find(ChoosedRouteId);
                if (route == null) 
                {
                    MessageBox.Show("Выбранный маршрут не обнаружен!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
                List<Ticket> tickets = dbManager.Tickets.ToList();
                List<WagonType> wagonTypes = dbManager.WagonType.ToList();

                foreach(Wagon wagon in dbManager.Wagons.Where(w => w.TrainId == route.TrainId))
                {
                    TextBlock textBlock = new TextBlock() { Tag = wagon, Width = WagonList.Width };
                    textBlock.MouseDown += WagonMouseDown;

                    int freeSeatsNumber = (int)wagon.SeatsNumber - tickets.Where(t => t.WagonNumber == wagon.WagonNumber).Count();
                    textBlock.Text = $"{wagon.WagonNumber} {wagonTypes.Where(w => w.WagonTypeId == wagon.WagonTypeId).First().Type} Всего:{wagon.SeatsNumber} Свободно:{freeSeatsNumber}";
                    
                    WagonList.Items.Add(textBlock);
                }
            }
        }

        private void WagonMouseDown(object sender, MouseEventArgs e)
        {
            ChoosedWagon = (Wagon)(((TextBlock)sender).Tag);
            WagonTextBlock.Text = ((TextBlock)sender).Text;
            ChoosedPlace = null;
            SeatTextBlock.Text = "Место не выбрано";

            if (SeatList.Items.Count > 0) SeatList.Items.Clear();
            using (DatabaseManager dbManager = new DatabaseManager())
            {
                
                for (int i = 1; i <= ChoosedWagon.SeatsNumber; i++)
                {
                    bool isSold = dbManager.Tickets.Where(t => t.WagonNumber == ChoosedWagon.WagonNumber && t.PlaceNumber == i).Any();

                    if (isSold) continue;

                    TextBlock textBlock = new TextBlock() { Tag = i, Width = SeatList.Width };
                    textBlock.MouseDown += SeatMouseDown;

                    textBlock.Text = $"Номер: {i}";

                    SeatList.Items.Add(textBlock);
                }
            }
        }

        private void SeatMouseDown(object sender, MouseEventArgs e)
        {
            ChoosedPlace = (int)(((TextBlock)sender).Tag);
            SeatTextBlock.Text = ((TextBlock)sender).Text;
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            if (ChoosedWagon == null)
            {
                MessageBox.Show("Не выбран вагон!", "Ошибка покупки!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (ChoosedPlace == null)
            {
                MessageBox.Show("Не выбрано место!", "Ошибка покупки!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Ticket ticket = new Ticket()
            {
                UserId = UserId,
                RouteId = ChoosedRouteId,
                WagonNumber = ChoosedWagon.WagonNumber,
                PlaceNumber = (int)ChoosedPlace
            };

            ClientRequest clientRequest = new ClientRequest(new Dictionary<string, string>(), JsonSerializer.Serialize(ticket));

            ServerResponse serverResponse = ticketManagerService.RegisterTicket(clientRequest);

            if (serverResponse.Status)
            {
                MessageBox.Show("Вы успешно купили билет!", "Покупка совершена.", MessageBoxButton.OK);
                this.Close();
            }
            else
                MessageBox.Show(JsonSerializer.Deserialize<Dictionary<string, string>>(serverResponse.Content)["Message"], "Покупка не совершена.", MessageBoxButton.OK,MessageBoxImage.Error);
        }
    }
}
