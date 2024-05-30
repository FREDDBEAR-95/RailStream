﻿using Microsoft.EntityFrameworkCore;
using RailStream_Server.Models;
using RailStream_Server_Backend.Managers;
using System.Windows;

namespace RailStream_Server
{
    public partial class UsersEditingWindow : Window
    {
        public UsersEditingWindow()
        {
            // Инициализация компонентов из Xaml
            InitializeComponent();
        }

        // Редактирование записи
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Получение информации с кнопки
            var user = ((FrameworkElement)sender).DataContext as User;

            if (user != null) {
                var userEdit = new UsersEditAndAddWindow(user.UserId);
                userEdit.Show();
                this.Close();
            }
        }

        // Добавление записи
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var userAdd = new UsersEditAndAddWindow();
            userAdd.Show();
            this.Close();
        }

        // Удаление записи
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Выбор пользователя и присваивание объекта в переменную
            if (GridUsers.SelectedItem is User userToDelete) {

                var result = MessageBox.Show($"Вы уверены, что хотите удалить пользователя '{userToDelete.Name} {userToDelete.Surname}'?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                // Удаление пользователя
                if (result == MessageBoxResult.Yes) {
                    using (var db = new DatabaseManager()) {
                        db.Users.Remove(userToDelete);
                        db.SaveChanges();
                    }
                    MessageBox.Show("Пользователь успешно удален.");
                    LoadUsers();
                }
            }
            else {
                MessageBox.Show("Пожалуйста, выберите пользователя для удаления.");
            }
        }

        // Обновление данных при изменении страницы
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible) {
                LoadUsers();
            }
        }

        // Метод для обновления данных
        private void LoadUsers()
        {
            using (var db = new DatabaseManager()) {
                // Обновляем данные из базы и устанавливаем их в DataGrid
                var users = db.Users.Include(r => r.Role).ToList();
                GridUsers.ItemsSource = users;
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
