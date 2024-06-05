using Microsoft.EntityFrameworkCore;
using RailStream_Server.Models;
using RailStream_Server_Backend.Managers;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace RailStream_Server
{
    public partial class RoutesEditAndAddWindow : Window
    {
        private Server RailStreamServer { get; }
        private DatabaseManager dataBase;
        private int? _routeId; // ID Маршрута для редактирования

        public RoutesEditAndAddWindow(Server server, int? routeId = null)
        {
            InitializeComponent();
            RailStreamServer = server;
            dataBase = new DatabaseManager();
            _routeId = routeId;
            LoadStatuses();
            
            if (_routeId.HasValue) {
                LoadRouteData();
            }
        }

        // Отображение данных существующего маршрута 
        private void LoadRouteData()
        {
            // Загрузка маршрута с его статусом
            var route = dataBase.Routes.Include(r => r.RouteStatus).FirstOrDefault(r => r.RouteId == _routeId);

            if (route != null)
            {
                DprtPlace.Text = route.DeparturePlace;
                Dstn.Text = route.Destination;
                ComboStatus.SelectedItem = route.RouteStatus;
                DprtDate.SelectedDate = route.DepartureDate.ToDateTime(TimeOnly.MinValue);
                DprtTime.Text = route.DepartureTime.ToString(@"hh\:mm\:ss");
                ArrDate.SelectedDate = route.ArrivalDate.ToDateTime(TimeOnly.MinValue);
                ArrTime.Text = route.ArrivalTime.ToString(@"hh\:mm\:ss");
                Distan.Text = route.Distance.ToString();
                WaysTime.Text = route.TimeWays;
            }
        }

        // Загрузка данных в ComboBox
        private void LoadStatuses() 
        {
            ComboStatus.ItemsSource = dataBase.RouteStatus.ToList();
        }

        // Возвращение назад
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var routesEditingWindow = new RoutesEditingWindow(RailStreamServer);
            routesEditingWindow.Show();
            this.Close();
        }

        // Проверка строки на правильный формат
        private bool IsValidTimeFormat(string timeString)
        {
            // Регулярное выражение для проверки времени (hh:mm:ss)
            string timeFormatPattern = @"^(?<hours>\d{2}):(?<minutes>\d{2}):(?<seconds>\d{2})$";
            var match = Regex.Match(timeString, timeFormatPattern);

            if (!match.Success)
            {
                return false;
            }

            // Извлечение часов, минут, секунд
            int hours = int.Parse(match.Groups["hours"].Value);
            int minutes = int.Parse(match.Groups["minutes"].Value);
            int seconds = int.Parse(match.Groups["seconds"].Value);

            // Проверка значений на выход за пределы
            if (hours < 0 || hours >= 24 || minutes < 0 || minutes >= 60 || seconds < 0 || seconds >= 60)
            {
                return false;
            }

            return true;
        }

        public bool IsValidWaysTime(string text)
        {
            // Регулярное выражение для проверки времени (5 ч 10 мин)
            string pattern = @"^(\d+) ч (\d{2}) мин$";
            var match = Regex.Match(text, pattern);

            if (!match.Success)
            {
                return false;
            }

            // Извлечение часов, минут
            int hours = int.Parse(match.Groups[1].Value);
            int minutes = int.Parse(match.Groups[2].Value);

            // Проверка значений на выход за пределы
            if (hours < 0 || hours >= 300 || minutes <= 0 || minutes > 59)
            {
                return false;
            }

            return true;
        }

        // Сохранение результатов
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            // Проверки на пустой выбор
            if (DprtDate.SelectedDate == null) {
                MessageBox.Show("Укажите дату отправления!");
                return;
            }
            if (ArrDate.SelectedDate == null) {
                MessageBox.Show("Укажите дату прибытия!");
                return;
            }
            if (DprtTime == null) {
                MessageBox.Show("Укажите время отправления!");
                return;
            }
            if (ArrTime == null) {
                MessageBox.Show("Укажите время прибытия!");
                return;
            }
            if (WaysTime == null) {
                MessageBox.Show("Укажите время в пути!");
                return;
            }

            // Определение диапазона дат
            DateOnly minDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly maxDate = new DateOnly(2050, 12, 31);

            // Переменные с текущим выбором даты
            DateTime dprtDateTime = DprtDate.SelectedDate.Value;
            DateTime arrDateTime = ArrDate.SelectedDate.Value;

            // Определение дистанции
            decimal distance;

            // Проверка мест отправления и прибытия
            if (string.IsNullOrWhiteSpace(DprtPlace.Text) || DprtPlace.Text.Length > 20) {
                errors.AppendLine("Укажите место отправления в пределах до 20 символов!");
            }
            if (string.IsNullOrWhiteSpace(Dstn.Text) || Dstn.Text.Length > 20) {
                errors.AppendLine("Укажите место прибытия в пределах до 20 символов!");
            }

            // Проверка выбора в ComboBox
            if (ComboStatus.SelectedItem == null) {
                errors.AppendLine("Укажите статус маршрута!");
            }

            // Проверка дат отправления и прибытия
            if (DateOnly.FromDateTime(dprtDateTime) < minDate || DateOnly.FromDateTime(dprtDateTime) > maxDate) {
                errors.AppendLine($"Укажите дату отправления в диапазоне от {minDate} до {maxDate}!");
            }
            if (DateOnly.FromDateTime(arrDateTime) < minDate || DateOnly.FromDateTime(arrDateTime) > maxDate || DateOnly.FromDateTime(arrDateTime) < DateOnly.FromDateTime(dprtDateTime)) {
                errors.AppendLine($"Укажите дату прибытия в диапазоне от {minDate} до {maxDate}!");
            }

            // Проверка времени отправления и прибытия
            if (!IsValidTimeFormat(DprtTime.Text)) {
                errors.AppendLine($"Укажите время отправления в правильном формате (ЧЧ:ММ:СС)");
            }
            if (!IsValidTimeFormat(ArrTime.Text)) {
                errors.AppendLine($"Укажите время прибытия в правильном формате (ЧЧ:ММ:СС)");
            }

            // Проверка дистанции
            if (!decimal.TryParse(Distan.Text, out distance)) {
                errors.AppendLine($"Неверный формат расстояния. Пожалуйста, введите число!");
            }
            if (distance < 0 || distance > 5000) {
                errors.AppendLine($"Расстояние должно быть в пределах от 0 до 5000!");
            }

            // Проверка времени в пути (на правильный формат)
            if (!IsValidWaysTime(WaysTime.Text)) {
                errors.AppendLine($"Укажите время в пути в правильном формате (ЧЧЧ 'ч' ММ 'мин')");
            }

            // Вывод списка ошибок на экран
            if (errors.Length > 0) {
                MessageBox.Show(errors.ToString());
                return;
            }

            // Добавление измененных данных
            if (_routeId.HasValue)
            {
                var route = dataBase.Routes.FirstOrDefault(r => r.RouteId == _routeId);
                if (route != null)
                {
                    // Преобразование данных
                    DateOnly saveDprtDate = DateOnly.FromDateTime(DprtDate.SelectedDate.Value);
                    DateOnly saveArrDate = DateOnly.FromDateTime(ArrDate.SelectedDate.Value);
                    TimeOnly.TryParse(DprtTime.Text, out var saveDprtTime);
                    TimeOnly.TryParse(ArrTime.Text, out var saveArrTime);
                    string saveDistance = Distan.Text;
                    decimal saveDistan = Convert.ToDecimal(saveDistance);

                    route.RouteName = DprtPlace.Text + " - " + Dstn.Text;
                    route.RouteStatusId = (ComboStatus.SelectedItem as RouteStatus).RouteStatusId;
                    route.DeparturePlace = DprtPlace.Text;
                    route.Destination = Dstn.Text;
                    route.DepartureDate = saveDprtDate;
                    route.DepartureTime = saveDprtTime;
                    route.ArrivalDate = saveArrDate;
                    route.ArrivalTime = saveArrTime;
                    route.Distance = saveDistan;
                    route.TimeWays = WaysTime.Text;
                }
            }
            // Иначе добавление новых данных
            else
            {
                // Преобразование данных
                DateOnly newDprtDate = DateOnly.FromDateTime(DprtDate.SelectedDate.Value);
                DateOnly newArrDate = DateOnly.FromDateTime(ArrDate.SelectedDate.Value);
                TimeOnly.TryParse(DprtTime.Text, out var newDprtTime);
                TimeOnly.TryParse(ArrTime.Text, out var newArrTime);
                string newDistance = Distan.Text;
                decimal newDistan = Convert.ToDecimal(newDistance);

                Route newRoute = new Route
                {
                    RouteName = DprtPlace.Text + " - " + Dstn.Text,
                    RouteStatusId = (ComboStatus.SelectedItem as RouteStatus).RouteStatusId,
                    Destination = Dstn.Text,
                    DeparturePlace = DprtPlace.Text,
                    DepartureDate = newDprtDate,
                    DepartureTime = newDprtTime,
                    ArrivalDate = newArrDate,
                    ArrivalTime = newArrTime,
                    Distance = newDistan,
                    TimeWays = WaysTime.Text
                };
                // Добавление данных в БД
                dataBase.Routes.Add(newRoute);
            }
            // Сохранение измененных данных
            dataBase.SaveChanges();
            MessageBox.Show("Изменения сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            var routesEditingWindow = new RoutesEditingWindow(RailStreamServer);
            routesEditingWindow.Show();
            this.Close();
        }
    }
}