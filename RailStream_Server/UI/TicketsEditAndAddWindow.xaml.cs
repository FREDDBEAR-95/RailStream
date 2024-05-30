using Microsoft.EntityFrameworkCore;
using RailStream_Server.Models;
using RailStream_Server_Backend.Managers;
using System.Text;
using System.Windows;

namespace RailStream_Server
{
    public partial class TicketsEditAndAddWindow : Window
    {
        private DatabaseManager dataBase;   // Экземпляр базы данных
        private int? _ticketId;             // ID Билета для редактирования

        public TicketsEditAndAddWindow(int? ticketId = null)
        {
            // Инициализация компонентов из Xaml
            InitializeComponent();
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
                        .Include(r => r.Train)
                        .Include(r => r.Wagon)
                        .FirstOrDefault(r => r.TicketId == _ticketId);

            if (ticket != null)
            {
                ComboUserPass.SelectedItem = ticket.User;
                ComboTrainId.SelectedItem = ticket.Train;
                ComboWagonNumber.SelectedItem = ticket.Wagon;
                TicketPlaceNum.Text = ticket.PlaceNumber.ToString();
            }
        }

        // Загрузка данных в ComboBox
        private void LoadTicketCombo()
        {
            ComboUserPass.ItemsSource = dataBase.Users.ToList();
            ComboTrainId.ItemsSource = dataBase.Trains.ToList();
            ComboWagonNumber.ItemsSource = dataBase.Wagons.ToList();
        }

        // Возвращение назад
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var ticketsEditingWindow = new TicketsEditingWindow();
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

            if (Math.Abs(result).ToString().Length > 2)
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
            var isTrainId = ComboTrainId.SelectedItem as Train;
            var isWagonId = ComboWagonNumber.SelectedItem as Wagon;

            var orderTicket = dataBase.Tickets
                                .FirstOrDefault(t => t.PlaceNumber == plcNum &&
                                                t.TrainId == isTrainId.TrainId &&
                                                t.WagonId == isWagonId.Id);

            if (orderTicket != null && orderTicket.TicketId != _ticketId)
            {
                return false;
            }

            return true;
        }

        // Проверка в ремонте ли поезд при добавлении
        private bool IsValidTrain(Train train)
        {
            if (train == null)
            {
                return false;
            }

            var trainStatus = dataBase.TrainStatus.FirstOrDefault(ts => ts.TrainStatusId == train.TrainStatusId);
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

            if (isWagonId != null) {
                var wagon = dataBase.Wagons
                            .FirstOrDefault(w => w.Id == isWagonId.Id);

                if (wagon != null) {
                    if (plcNum > wagon.SeatsNumber) {
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
            if (ComboTrainId.SelectedItem == null)
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
            if (!IsValidTrain(ComboTrainId.SelectedItem as Train))
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
                    ticket.TrainId = (ComboTrainId.SelectedItem as Train).TrainId;
                    ticket.WagonId = (ComboWagonNumber.SelectedItem as Wagon).Id;
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
                    TrainId = (ComboTrainId.SelectedItem as Train).TrainId,
                    WagonId = (ComboWagonNumber.SelectedItem as Wagon).Id,
                    PlaceNumber = newPlaceNumber
                };
                // Добавление данных в БД
                dataBase.Tickets.Add(newTicket);
            }
            // Сохранение измененных данных
            dataBase.SaveChanges();
            MessageBox.Show("Изменения сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            var ticketsEditingWindow = new TicketsEditingWindow();
            ticketsEditingWindow.Show();
            this.Close();
        }
    }
}
