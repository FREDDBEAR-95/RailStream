using Microsoft.EntityFrameworkCore;
using RailStream_Server.Models;
using RailStream_Server_Backend.Managers;
using System.Text;
using System.Windows;

namespace RailStream_Server
{
    public partial class TicketsEditAndAddWindow : Window
    {
        private Server RailStreamServer { get; }
        private DatabaseManager dataBase;   // Экземпляр базы данных
        private int? _ticketId;             // ID Билета для редактирования

        public TicketsEditAndAddWindow(Server server, int? ticketId = null)
        {
            // Инициализация компонентов из Xaml
            InitializeComponent();
            RailStreamServer = server;
            dataBase = new DatabaseManager();
            _ticketId = ticketId;
            LoadTicketCombo();

            if (_ticketId != null)
            {
                LoadTicketData();
            }
        }

        // Отображение данных существующего билета
        private void LoadTicketData()
        {
            // Загрузка билета с его пользователем, маршрутом и поездом
            var ticket = dataBase.Tickets
                        .Include(r => r.User)
                        .Include(r => r.Route)
                        .Include(r => r.WagonNumber) // Добавлена строка
                        .FirstOrDefault(r => r.TicketId == _ticketId);

            if (ticket != null)
            {
                ComboUserPass.SelectedItem = ticket.User;
                ComboRouteId.SelectedItem = ticket.Route;
                TicketPlaceNum.Text = ticket.PlaceNumber.ToString();
            }
        }

        // Загрузка данных в ComboBox
        private void LoadTicketCombo()
        {
            ComboUserPass.ItemsSource = dataBase.Users.ToList();
            ComboRouteId.ItemsSource = dataBase.Routes.ToList();
            ComboWagonNumber.ItemsSource = dataBase.Wagons.ToList();
        }

        // Возвращение назад
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var ticketsEditingWindow = new TicketsEditingWindow(RailStreamServer);
            ticketsEditingWindow.Show();
            this.Close();
        }

        // Проверка места на пределы и преобразование (от 1 до 64)
        private bool IsValidPlaceNum(string text)
        {
            int result;
            if (!int.TryParse(text, out result))
            {
                return false;
            }

            if (result > 64 || result < 1)
            {
                return false;
            }

            return true;
        }

        // Проверка занято ли место или нет
        private bool IsPlaceOrder(string text)
        {
            int plcNum = Convert.ToInt32(text);
            var isTrainId = ComboRouteId.SelectedItem as Route;
            var isWagonId = ComboWagonNumber.SelectedItem as Wagon;

            if (isTrainId == null || isWagonId == null)
            {
                return false; //  Не выбрано ни одного маршрута или вагона
            }

            var orderTicket = dataBase.Tickets
                                .FirstOrDefault(t => t.PlaceNumber == plcNum &&
                                                t.RouteId == isTrainId.RouteId &&
                                                t.WagonNumber == isWagonId.WagonNumber);

            if (orderTicket != null && orderTicket.TicketId != _ticketId)
            {
                return false;
            }

            return true;
        }

        // Проверка в ремонте ли поезд при добавлении
        private bool IsValidTrain(Route route)
        {
            if (route == null)
            {
                return false;
            }

            var trainStatus = dataBase.RouteStatus.FirstOrDefault(rt => rt.RouteStatusId == route.RouteStatusId);
            if (trainStatus == null || trainStatus.Status == "В ремонте")
            {
                return false;
            }

            return true;
        }

        // Проверка выходит ли место за пределы общего кол-ва мест в вагоне
        private bool IsValidWagonSeat(string text)
        {
            int plcNum = Convert.ToInt32(text);
            var isWagonId = ComboWagonNumber.SelectedItem as Wagon;

            if (isWagonId != null)
            {
                var wagon = dataBase.Wagons
                            .FirstOrDefault(w => w.WagonNumber == isWagonId.WagonNumber);

                if (wagon != null)
                {
                    if (plcNum > wagon.SeatsNumber)
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        // Сохранение результатов
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Проверка строк
            StringBuilder errors = new StringBuilder();

            // Проверки на пустой выбор
            if (TicketPlaceNum.Text == null)
            {
                MessageBox.Show("Укажите место!");
                return;
            }
            if (ComboRouteId.SelectedItem == null)
            {
                MessageBox.Show("Укажите поезд!");
                return;
            }

            // Проверка выбора в ComboBox
            if (ComboUserPass.SelectedItem == null)
            {
                errors.AppendLine("Укажите пользователя!");
            }
            if (ComboWagonNumber.SelectedItem == null)
            {
                errors.AppendLine("Укажите номер вагона!");
            }

            // Проверка статуса поезда (в ремонте ли он или активен)
            if (!IsValidTrain(ComboRouteId.SelectedItem as Route))
            {
                errors.AppendLine("Поезд недоступен, так как он в ремонте. Выберите другой!");
            }

            // Проверка места на пределы и преобразование (от 1 до 64)
            if (!IsValidPlaceNum(TicketPlaceNum.Text))
            {
                errors.AppendLine("Укажите место в диапазоне от 1 до 64!");
            }

            // Проверка занято ли место или нет
            if (!IsPlaceOrder(TicketPlaceNum.Text))
            {
                errors.AppendLine("Укажите не занятое место!");
            }

            // Проверка выходит ли место за пределы общего кол-ва мест в вагоне
            if (!IsValidWagonSeat(TicketPlaceNum.Text))
            {
                errors.AppendLine("Указанное место превышает общее количество мест в вагоне!");
            }

            // Вывод списка ошибок на экран
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            // Добавление измененных данных
            if (_ticketId != null)
            {
                var ticket = dataBase.Tickets.FirstOrDefault(r => r.TicketId == _ticketId);
                if (ticket != null)
                {
                    // Преобразование данных
                    string savePlaceNum = TicketPlaceNum.Text;
                    int savePlaceNumber = Convert.ToInt32(savePlaceNum);

                    ticket.UserId = (ComboUserPass.SelectedItem as User).UserId;
                    ticket.RouteId = (ComboRouteId.SelectedItem as Route).RouteId;
                    ticket.WagonNumber = (ComboWagonNumber.SelectedItem as Wagon).WagonNumber;
                    ticket.PlaceNumber = savePlaceNumber;
                }
            }
            // Иначе добавление новых данных
            else
            {
                // Преобразование данных
                string newPlaceNum = TicketPlaceNum.Text;
                int newPlaceNumber = Convert.ToInt32(newPlaceNum);

                Ticket newTicket = new Ticket
                {
                    UserId = (ComboUserPass.SelectedItem as User).UserId,
                    RouteId = (ComboRouteId.SelectedItem as Route).RouteId,
                    WagonNumber = (ComboWagonNumber.SelectedItem as Wagon).WagonNumber,
                    PlaceNumber = newPlaceNumber
                };
                // Добавление данных в БД
                dataBase.Tickets.Add(newTicket);
            }
            // Сохранение измененных данных
            dataBase.SaveChanges();
            MessageBox.Show("Изменения сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            var ticketsEditingWindow = new TicketsEditingWindow(RailStreamServer);
            ticketsEditingWindow.Show();
            this.Close();
        }
    }
}