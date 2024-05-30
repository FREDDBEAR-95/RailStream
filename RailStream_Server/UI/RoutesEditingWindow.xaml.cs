using Microsoft.EntityFrameworkCore;
using RailStream_Server.Models;
using RailStream_Server_Backend.Managers;
using System.Windows;

namespace RailStream_Server
{
    public partial class RoutesEditingWindow : Window
    {
        public RoutesEditingWindow() 
        {
            // Инициализация компонентов из Xaml
            InitializeComponent();
        }

        // Редактирование записи
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Получение информации с кнопки
            var route = ((FrameworkElement)sender).DataContext as Route;

            if (route != null) {
                var routeEdit = new RoutesEditAndAddWindow(route.RouteId);
                routeEdit.Show();
                this.Close();
            }
        }

        // Добавление записи
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var routeAdd = new RoutesEditAndAddWindow();
            routeAdd.Show();
            this.Close();
        }

        // Удаление записи
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Выбор маршрута и присваивание объекта в переменную
            if (GridRoutes.SelectedItem is Route routeToDelete) {

                var result = MessageBox.Show($"Вы уверены, что хотите удалить маршрут '{routeToDelete.RouteName}'?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                // Удаление маршрута
                if (result == MessageBoxResult.Yes) {
                    using (var db = new DatabaseManager()) {
                        db.Routes.Remove(routeToDelete);
                        db.SaveChanges();
                    }
                    MessageBox.Show("Маршрут успешно удален.");
                    LoadRoutes();
                }
            }
            else {
                MessageBox.Show("Пожалуйста, выберите маршрут для удаления!");
            }
        }

        // Обновление данных при изменении страницы
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible) {
                LoadRoutes();
            }
        }

        // Метод для обновления данных
        private void LoadRoutes()
        {
            using (var db = new DatabaseManager()) {
                // Обновляем данные из базы и устанавливаем их в DataGrid
                var routes = db.Routes.Include(r => r.RouteStatus).ToList();
                GridRoutes.ItemsSource = routes;
            }
        }

        /*
         * Кнопки для перемещения
         */

        // Страница маршрутов
        private void RoutesBtn_Click(object sender, RoutedEventArgs e)
        {
            var routesMove = new RoutesEditingWindow();
            routesMove.Show();
            this.Close();
        }

        // Страница пользователей
        private void UsersBtn_Click(object sender, RoutedEventArgs e)
        {
            var usersMove = new UsersEditingWindow();
            usersMove.Show();
            this.Close();
        }

        // Страница билетов
        private void TicketsBtn_Click(object sender, RoutedEventArgs e)
        {
            var ticketsMove = new TicketsEditingWindow();
            ticketsMove.Show();
            this.Close();
        }

        // Страница поездов
        private void TrainsBtn_Click(object sender, RoutedEventArgs e)
        {
            var trainsMove = new TrainsEditingWindow();
            trainsMove.Show();
            this.Close();
        }

        // Страница вагонов
        private void WagonsBtn_Click(object sender, RoutedEventArgs e)
        {
            var wagonsMove = new WagonsEditingWindow();
            wagonsMove.Show();
            this.Close();
        }
    }
}