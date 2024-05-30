﻿using Microsoft.EntityFrameworkCore;
using RailStream_Server.Models;
using RailStream_Server_Backend.Managers;
using System.Windows;

namespace RailStream_Server
{
    public partial class TicketsEditingWindow : Window
    {
        public TicketsEditingWindow()
        {
            // Инициализация компонентов из Xaml
            InitializeComponent();
        }

        // Редактирование записи
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Получение информации с кнопки
            var ticket = ((FrameworkElement)sender).DataContext as Ticket;

            if (ticket != null) {
                var ticketEdit = new TicketsEditAndAddWindow(ticket.TicketId);
                ticketEdit.Show();
                this.Close();
            }
        }

        // Кнопка добавления записей
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var ticketsAdd = new TicketsEditAndAddWindow();
            ticketsAdd.Show();
            this.Close();
        }

        // Удаление записи
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Выбор билета и присваивание объекта в переменную
            if (GridTickets.SelectedItem is Ticket ticketToDelete)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить билет с номером '{ticketToDelete.TicketId}'?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                // Удаление билета
                if (result == MessageBoxResult.Yes) {
                    using (var db = new DatabaseManager()) {
                        db.Tickets.Remove(ticketToDelete);
                        db.SaveChanges();
                    }
                    MessageBox.Show("Билет успешно удален.");
                    LoadTickets();
                }
            }
            else {
                MessageBox.Show("Пожалуйста, выберите билет для удаления.");
            }
        }

        // Обновление данных при изменении страницы
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible) {
                LoadTickets();
            }
        }

        // Метод для обновления данных
        private void LoadTickets()
        {
            using (var db = new DatabaseManager())
            {
                // Обновляем данные из базы и устанавливаем их в DataGrid
                var tickets = db.Tickets
                    .Include(r => r.User)
                    .Include(r => r.Train)
                    .Include(r => r.Wagon)
                    .ToList();
                GridTickets.ItemsSource = tickets;
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
