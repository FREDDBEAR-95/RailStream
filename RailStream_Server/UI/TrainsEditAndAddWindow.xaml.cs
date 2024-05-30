using Microsoft.EntityFrameworkCore;
using RailStream_Server.Models;
using RailStream_Server_Backend.Managers;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace RailStream_Server
{
    public partial class TrainsEditAndAddWindow : Window
    {
        private DatabaseManager dataBase;   // Экземпляр базы данных
        private int? _trainId;              // ID Поезда для редактирования

        public TrainsEditAndAddWindow(int? trainId = null)
        {
            // Инициализация компонентов из Xaml
            InitializeComponent();
            dataBase = new DatabaseManager();
            _trainId = trainId;
            LoadTrainCombo();
            LoadLocations();

            if (_trainId.HasValue)
            {
                LoadTrainData();
            }
        }

        // Отображение данных существующего поезда
        private void LoadTrainData()
        {
            // Загрузка поезда с его типом и статусом
            var train = dataBase.Trains
                        .Include(r => r.TrainType)
                        .Include(r => r.TrainStatus)
                        .Include(r => r.Route)
                        .FirstOrDefault(r => r.TrainId == _trainId);

            if (train != null)
            {
                ComboTrainTypes.SelectedItem = train.TrainType;
                ComboTrainStatuses.SelectedItem = train.TrainStatus;
                ComboRouteName.SelectedItem = train.Route;
                TrainBrnd.Text = train.TrainBrand;
                TrainRlsDate.SelectedDate = train.ReleaseDate.ToDateTime(TimeOnly.MinValue);
                ComboLocation.SelectedItem = train.Location;
            }
        }

        // Загрузка локаций из JSON-файла
        private void LoadLocations()
        {
            try {
                string jsonString = File.ReadAllText("city_list.json");
                List<string> cities = JsonSerializer.Deserialize<List<string>>(jsonString);
                ComboLocation.ItemsSource = cities;
            }
            catch (Exception ex) {
                MessageBox.Show("Не удалось загрузить список локаций");
            }
        }

        // Загрузка данных в ComboBox
        private void LoadTrainCombo()
        {
            ComboTrainTypes.ItemsSource = dataBase.TrainType.ToList();
            ComboTrainStatuses.ItemsSource = dataBase.TrainStatus.ToList();
            ComboRouteName.ItemsSource = dataBase.Routes.ToList();

        }

        // Возвращение назад
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var trainsEditingWindow = new TrainsEditingWindow();
            trainsEditingWindow.Show();
            this.Close();
        }

        // Сохранение результатов
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Проверка строк
            StringBuilder errors = new StringBuilder();

            // Проверка на пустой выбор
            if (TrainRlsDate.SelectedDate == null) {
                MessageBox.Show("Укажите дату выпуска поезда!");
                return;
            }

            // Определение диапазона дат
            DateOnly maxDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly minDate = new DateOnly(1950, 12, 31);

            // Переменная с текущим выбором даты
            DateTime userDateTime = TrainRlsDate.SelectedDate.Value;

            // Проверка выбора ComboBox
            if (ComboTrainTypes.SelectedItem == null) {
                errors.AppendLine("Выберите тип поезда!");
            }
            if (ComboTrainStatuses.SelectedItem == null) {
                errors.AppendLine("Выберите статус поезда!");
            }
            if (ComboRouteName.SelectedItem == null)
            {
                errors.AppendLine("Выберите маршрут!");
            }
            if (ComboLocation.SelectedItem == null) {
                errors.AppendLine("Выберите местоположение поезда!");
            }

            // Проверка марки поезда
            if (string.IsNullOrEmpty(TrainBrnd.Text)) {
                errors.AppendLine("Укажите марку поезда!");
            }
            
            // Проверка даты выпуска (на выход из диапазона)
            if (DateOnly.FromDateTime(userDateTime) <= minDate || DateOnly.FromDateTime(userDateTime) > maxDate) {
                errors.AppendLine($"Укажите дату выпуска в диапазоне от {minDate} до {maxDate}!");
            }

            // Вывод списка ошибок на экран
            if (errors.Length > 0) {
                MessageBox.Show(errors.ToString());
                return;
            }

            // Добавление измененных данных
            if (_trainId.HasValue)
            {
                var train = dataBase.Trains.FirstOrDefault(r => r.TrainId == _trainId);
                if (train != null)
                {
                    // Преобразование данных
                    DateOnly saveRlsDate = DateOnly.FromDateTime(TrainRlsDate.SelectedDate.Value);

                    train.TrainTypeId = (ComboTrainTypes.SelectedItem as TrainType).TrainTypeId;
                    train.TrainStatusId = (ComboTrainStatuses.SelectedItem as TrainStatus).TrainStatusId;
                    train.RouteId = (ComboRouteName.SelectedItem as Route).RouteId;
                    train.TrainBrand = TrainBrnd.Text;
                    train.ReleaseDate = saveRlsDate;
                    train.Location = ComboLocation.SelectedItem as string;
                }
            }
            // Иначе добавление новых данных
            else
            {
                // Преобразование данных
                DateOnly newRlsDate = DateOnly.FromDateTime(TrainRlsDate.SelectedDate.Value);

                Train newTrain = new Train
                {
                    TrainTypeId = (ComboTrainTypes.SelectedItem as TrainType).TrainTypeId,
                    TrainStatusId = (ComboTrainStatuses.SelectedItem as TrainStatus).TrainStatusId,
                    RouteId = (ComboRouteName.SelectedItem as Route).RouteId,
                    TrainBrand = TrainBrnd.Text,
                    ReleaseDate = newRlsDate,
                    Location = ComboLocation.SelectedItem as string
                };
                // Добавление данных в БД
                dataBase.Trains.Add(newTrain);
            }
            // Сохранение измененных данных
            dataBase.SaveChanges();
            MessageBox.Show("Изменения сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            var trainsEditingWindow = new TrainsEditingWindow();
            trainsEditingWindow.Show();
            this.Close();
        }
    }
}