using Microsoft.EntityFrameworkCore;
using RailStream_Server.Models;
using RailStream_Server.UI.View;
using RailStream_Server_Backend.Managers;
using System.Windows;

namespace RailStream_Server
{
    public partial class TrainsEditingWindow : Window
    {
        private Server RailStreamServer { get; }

        public TrainsEditingWindow(Server server)
        {
            // Инициализация компонентов из Xaml
            InitializeComponent();
            RailStreamServer = server;
        }

        // Редактирование записи
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Получение информации с кнопки
            var train = ((FrameworkElement)sender).DataContext as Train;

            if (train != null)
            {
                var trainEdit = new TrainsEditAndAddWindow(RailStreamServer, train.TrainId);
                trainEdit.Show();
                this.Close();
            }
        }

        // Добавление записи
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var trainAdd = new TrainsEditAndAddWindow(RailStreamServer);
            trainAdd.Show();
            this.Close();
        }

        // Удаление записи
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Выбор поезда и присваивание объекта в переменную
            if (GridTrains.SelectedItem is Train trainToDelete)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить поезд с номером '{trainToDelete.TrainId}' марки '{trainToDelete.TrainBrand}'?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                // Удаление поезда
                if (result == MessageBoxResult.Yes)
                {
                    using (var db = new DatabaseManager())
                    {
                        db.Trains.Remove(trainToDelete);
                        db.SaveChanges();
                    }
                    MessageBox.Show("Поезд успешно удален.");
                    LoadTrains();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите поезд для удаления.");
            }
        }

        // Обновление данных при изменении страницы
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                LoadTrains();
            }
        }

        // Метод для обновления данных
        private void LoadTrains()
        {
            using (var db = new DatabaseManager())
            {
                // Обновляем данные из базы и устанавливаем их в DataGrid
                var trains = db.Trains
                            .Include(r => r.TrainType)
                            .Include(r => r.TrainStatus)
                            .ToList();
                GridTrains.ItemsSource = trains;
            }
        }

        /*
         * Кнопки для перемещения
         */

        // Страница маршрутов
        private void RoutesBtn_Click(object sender, RoutedEventArgs e)
        {
            var routesMove = new RoutesEditingWindow(RailStreamServer);
            routesMove.Show();
            this.Close();
        }

        // Страница пользователей
        private void UsersBtn_Click(object sender, RoutedEventArgs e)
        {
            var usersMove = new UsersEditingWindow(RailStreamServer);
            usersMove.Show();
            this.Close();
        }

        // Страница билетов
        private void TicketsBtn_Click(object sender, RoutedEventArgs e)
        {
            var ticketsMove = new TicketsEditingWindow(RailStreamServer);
            ticketsMove.Show();
            this.Close();
        }

        // Страница вагонов
        private void WagonsBtn_Click(object sender, RoutedEventArgs e)
        {
            var wagonsMove = new WagonsEditingWindow(RailStreamServer);
            wagonsMove.Show();
            this.Close();
        }

        private void ServerControlPanelBtn_Click(object sender, RoutedEventArgs e)
        {
            var serverControlPanelMove = new ServerControlPanel(RailStreamServer);
            serverControlPanelMove.Show();
            this.Close();
        }
    }
}
