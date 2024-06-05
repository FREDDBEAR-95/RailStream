using Microsoft.EntityFrameworkCore;
using RailStream_Server.Models;
using RailStream_Server_Backend.Managers;
using System.Text;
using System.Windows;

namespace RailStream_Server
{
    public partial class WagonsEditAndAddWindow : Window
    {
        private Server RailStreamServer { get; }
        private DatabaseManager dataBase;   // Экземпляр базы данных
        private string? _wagonId;           // ID Вагона для редактирования

        public WagonsEditAndAddWindow(Server server, string? wagonId = null)
        {
            // Инициализация компонентов из Xaml
            InitializeComponent();
            RailStreamServer = server;
            dataBase = new DatabaseManager();
            _wagonId = wagonId;
            LoadWagonCombo();

            if (_wagonId != null)
            {
                LoadWagonData();
            }
        }

        // Отображение данных существующего вагона
        public void LoadWagonData()
        {
            // Загрузка поезда с его типом и статусом
            var wagon = dataBase.Wagons
                        .Include(r => r.WagonType)
                        .Include(r => r.Train)
                        .FirstOrDefault(r => r.WagonNumber == _wagonId);

            if (wagon != null)
            {
                WagonNum.Text = wagon.WagonNumber;
                WagonNum.IsReadOnly = true; // Отключаем возможность редактировать TextBox
                ComboTrainNums.SelectedItem = wagon.Train;
                ComboWagonTypes.SelectedItem = wagon.WagonType;
                WagonSeatNums.Text = wagon.SeatsNumber.ToString();
            }
        }

        // Загрузка данных в ComboBox
        public void LoadWagonCombo()
        {
            ComboTrainNums.ItemsSource = dataBase.Trains.ToList();
            ComboWagonTypes.ItemsSource = dataBase.WagonType.ToList();
        }

        // Возвращение назад
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var wagonsEditingWindow = new WagonsEditingWindow(RailStreamServer);
            wagonsEditingWindow.Show();
            this.Close();
        }

        // Проверка номера вагона
        private bool IsValidWagonNum(string text)
        {
            var existWagon = dataBase.Wagons.FirstOrDefault(r => r.WagonNumber == text);

            if (existWagon != null && existWagon.WagonNumber != _wagonId.ToString())
            {
                return false;
            }
            if (text.Length > 4)
            {
                return false;
            }
            int result;
            if (!int.TryParse(text, out result))
            {
                return false;
            }

            return true;
        }

        // Проверка количества мест
        private bool IsValidSeatsNum(string text)
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

        // Кнопка для сохранения результатов
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Проверка строк
            StringBuilder errors = new StringBuilder();

            // Проверки на пустой выбор
            if (string.IsNullOrEmpty(WagonNum.Text))
            {
                MessageBox.Show("Укажите номер вагона!");
                return;
            }
            if (string.IsNullOrEmpty(WagonSeatNums.Text))
            {
                MessageBox.Show("Укажите количество мест в вагоне!");
                return;
            }

            // Проверка на корректность номера вагона
            if (!IsValidWagonNum(WagonNum.Text))
            {
                errors.AppendLine("Укажите корректный, неповторяющийся номер вагона, с длиной до 4 символов!");
            }

            // Проверка выбора ComboBox
            if (ComboTrainNums.SelectedItem == null)
            {
                errors.AppendLine("Выберите номер поезда!");
            }
            if (ComboWagonTypes.SelectedItem == null)
            {
                errors.AppendLine("Выберите тип вагона!");
            }

            // Проверка количества мест
            if (!IsValidSeatsNum(WagonSeatNums.Text))
            {
                errors.AppendLine($"Введите корректное количество мест!");
            }

            // Вывод списка ошибок на экран
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }


            // Добавление измененных данных
            if (_wagonId != null)
            {
                var wagon = dataBase.Wagons.FirstOrDefault(r => r.WagonNumber == _wagonId.ToString());

                if (wagon != null)
                {
                    // Преобразование данных
                    string saveSeatsNum = WagonSeatNums.Text;
                    int saveSeatsNumber = Convert.ToInt32(saveSeatsNum);

                    wagon.WagonNumber = WagonNum.Text;
                    wagon.TrainId = (ComboTrainNums.SelectedItem as Train).TrainId;
                    wagon.WagonTypeId = (ComboWagonTypes.SelectedItem as WagonType).WagonTypeId;
                    wagon.SeatsNumber = saveSeatsNumber;
                }
            }
            // Иначе добавление новых данных
            else
            {
                // Преобразование данных
                string newSeatsNum = WagonSeatNums.Text;
                int newSeatsNumber = Convert.ToInt32(newSeatsNum);

                Wagon newWagon = new Wagon
                {
                    WagonNumber = WagonNum.Text,
                    TrainId = (ComboTrainNums.SelectedItem as Train).TrainId,
                    WagonTypeId = (ComboWagonTypes.SelectedItem as WagonType).WagonTypeId,
                    SeatsNumber = newSeatsNumber
                };
                // Добавление данных в БД
                dataBase.Wagons.Add(newWagon);
            }
            // Сохранение измененных данных
            dataBase.SaveChanges();
            MessageBox.Show("Изменения сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            var wagonsEditingWindow = new WagonsEditingWindow(RailStreamServer);
            wagonsEditingWindow.Show();
            this.Close();
        }
    }
}
