using Microsoft.EntityFrameworkCore;
using RailStream_Server.Models;
using RailStream_Server_Backend.Managers;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace RailStream_Server
{
    public partial class UsersEditAndAddWindow : Window
    {
        private Server RailStreamServer { get; }
        private DatabaseManager dataBase;   // Экземпляр базы данных
        private int? _userId;               // ID Пользователя для редактирования

        public UsersEditAndAddWindow(Server server, int? userId = null)
        {
            // Инициализация компонентов из Xaml
            InitializeComponent();
            dataBase = new DatabaseManager();
            _userId = userId;
            LoadRoles();

            if (_userId.HasValue)
            {
                LoadUserData();
            }
        }

        // Отображение данных существующего пользователя
        private void LoadUserData()
        {
            // Загрузка пользователя с его ролью
            var user = dataBase.Users.Include(r => r.Role).FirstOrDefault(r => r.UserId == _userId);

            if (user != null)
            {
                ComboRole.SelectedItem = user.Role;
                UserSurname.Text = user.Surname;
                UserName.Text = user.Name;
                UserPatronymic.Text = user.Patronymic;
                UserBirth.SelectedDate = user.BirthDate.ToDateTime(TimeOnly.MinValue);
                UserPassId.Text = user.PassportId;
                UserEmail.Text = user.Email;
                UserPhoneNum.Text = user.PhoneNumber.ToString();
                UserPassword.Text = user.Password;
                UserImgUrl.Text = user.ImageUrl;
                ComboIsBanned.SelectedItem = ComboIsBanned.Items.Cast<ComboBoxItem>()
                                            .FirstOrDefault(item => (string)item.Tag == user.IsBanned.ToString().ToLower());
            }
        }

        // Загрузка данных в ComboBox
        private void LoadRoles()
        {
            ComboRole.ItemsSource = dataBase.Roles.ToList();
        }

        // Возвращение назад
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var usersEditingWindow = new UsersEditingWindow(RailStreamServer);
            usersEditingWindow.Show();
            this.Close();
        }
        
        // Проверка номера паспорта
        public bool IsValidPassport(string text)
        {
            var existPassport = dataBase.Users.FirstOrDefault(r => r.PassportId == text);
            if (existPassport != null && existPassport.UserId != _userId) {
                return false;
            }
            if (text.Length > 12 || text.Length < 5) {
                return false;
            }
            return true;
        }

        // Проверка Email-адреса
        public bool IsValidEmail(string email)
        {
            try {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException) {
                return false;
            }
        }

        // Проверка URL-ссылки
        public bool IsValidUrl(string url)
        {
            Uri uriResult;
            return Uri.TryCreate(url, UriKind.Absolute, out uriResult) 
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        // Проверка номера телефона
        private bool IsValidPhoneNumber(string text)
        {
            long result;
            if(!long.TryParse(text, out result))
            {
                return false;
            }
            if (Math.Abs(result).ToString().Length > 11)
            {
                return false;
            }
            return true;
        }

        // Сохранение результатов
        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Проверка строк
            StringBuilder errors = new StringBuilder();

            // Проверки на пустой выбор
            if (UserBirth.SelectedDate == null) {
                MessageBox.Show("Укажите дату рождения!");
                return;
            }
            if (string.IsNullOrEmpty(UserPassId.Text)) {
                MessageBox.Show("Укажите номер паспорта!");
                return;
            }
            if (string.IsNullOrEmpty(UserEmail.Text)) {
                MessageBox.Show("Укажите электронную почту!");
                return;
            }
            if (string.IsNullOrEmpty(UserPhoneNum.Text)) {
                MessageBox.Show("Укажите номер телефона!");
                return;
            }
            if (string.IsNullOrEmpty(UserImgUrl.Text)) {
                MessageBox.Show("Укажите ссылку на фото!");
                return;
            }

            // Определение диапазона дат
            DateOnly maxDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly minDate = new DateOnly(1900, 12, 31);
            DateTime userDateTime = UserBirth.SelectedDate.Value;

            // Проверка на пустой выбор ComboBox
            if (ComboRole.SelectedItem == null) {
                errors.AppendLine("Укажите роль пользователя!");
            }

            // Проверка фамилии, имени, отчества пользователя
            if (string.IsNullOrEmpty(UserSurname.Text) || UserSurname.Text.Length > 40) {
                errors.AppendLine("Укажите фамилию в пределах до 40 символов!");
            }
            if (string.IsNullOrEmpty(UserName.Text) || UserName.Text.Length > 40) {
                errors.AppendLine("Укажите имя в пределах до 40 символов!");
            }
            if (string.IsNullOrEmpty(UserPatronymic.Text) || UserPatronymic.Text.Length > 40) {
                errors.AppendLine("Укажите отчество в пределах до 40 символов!");
            }

            // Проверка даты рождения (на выход из диапазона)
            if (DateOnly.FromDateTime(userDateTime) <= minDate || DateOnly.FromDateTime(userDateTime) >= maxDate) {
                errors.AppendLine($"Укажите дату рождения в диапазоне от {minDate} до {maxDate}!");
            }

            // Проверка номера паспорта
            if (!IsValidPassport(UserPassId.Text)) {
                errors.AppendLine("Укажите корректный, неповторяющийся номер паспорта, с длиной до 12 символов!");
            }
            // Проверка Email-адреса
            if (!IsValidEmail(UserEmail.Text)) {
                errors.AppendLine($"Введите правильный адрес электронный почты");
            }
            // Проверка номера телефона
            if (!IsValidPhoneNumber(UserPhoneNum.Text)) {
                errors.AppendLine($"Введите корректный номер телефона до 11 символов!");
            }
            // Проверка пароля
            if (string.IsNullOrEmpty(UserPassword.Text) || UserPassword.Text.Length > 40) {
                errors.AppendLine($"Введите пароль в пределах до 40 символов!");
            }
            // Проверка Url-ссылки
            if (!IsValidUrl(UserImgUrl.Text)) {
                errors.AppendLine($"Введите действительный URL-ссылку HTTP или HTTPS!");
            }

            // Проверка на пустой выбор ComboBox (isBanned)
            if (ComboIsBanned.SelectedItem == null) {
                errors.AppendLine("Укажите статус блокировки пользователя!");
            }

            // Вывод списка ошибок на экран
            if (errors.Length > 0) {
                MessageBox.Show(errors.ToString());
                return;
            }

            // Добавление измененных данных
            if (_userId.HasValue)
            {
                var user = dataBase.Users.FirstOrDefault(r => r.UserId == _userId);
                if (user != null)
                {
                    // Преобразование данных
                    DateOnly saveUserBirth = DateOnly.FromDateTime(UserBirth.SelectedDate.Value);
                    string savePhoneNumber = UserPhoneNum.Text;
                    long savePhoneNum = Convert.ToInt64(savePhoneNumber);
                    ComboBoxItem selectedBannedItem = ComboIsBanned.SelectedItem as ComboBoxItem;
                    bool saveUserBanned = Convert.ToBoolean(selectedBannedItem.Tag);

                    user.RoleId = (ComboRole.SelectedItem as Role).RoleId;
                    user.Surname = UserSurname.Text;
                    user.Name = UserName.Text;
                    user.Patronymic = UserPatronymic.Text;
                    user.BirthDate = saveUserBirth;
                    user.PassportId = UserPassId.Text;
                    user.Email = UserEmail.Text;
                    user.PhoneNumber = savePhoneNum;
                    user.Password = UserPassword.Text;
                    user.ImageUrl = UserImgUrl.Text;
                    user.IsBanned = saveUserBanned;
                }
            }
            // Иначе добавление новых данных
            else
            {
                // Преобразование данных
                DateOnly newUserBirth = DateOnly.FromDateTime(UserBirth.SelectedDate.Value);
                string newPhoneNumber = UserPhoneNum.Text;
                long newPhoneNum = Convert.ToInt64(newPhoneNumber);
                ComboBoxItem selectedBannedItem = ComboIsBanned.SelectedItem as ComboBoxItem;
                bool newUserBanned = Convert.ToBoolean(selectedBannedItem.Tag);

                User newUser = new User
                {
                    RoleId = (ComboRole.SelectedItem as Role).RoleId,
                    Surname = UserSurname.Text,
                    Name = UserName.Text,
                    Patronymic = UserPatronymic.Text,
                    BirthDate = newUserBirth,
                    PassportId = UserPassId.Text,
                    Email = UserEmail.Text,
                    PhoneNumber = newPhoneNum,
                    Password = UserPassword.Text,
                    ImageUrl = UserImgUrl.Text,
                    IsBanned = newUserBanned
                };
                // Добавление данных в БД
                dataBase.Users.Add(newUser);
            }
            // Сохранение измененных данных
            dataBase.SaveChanges();
            MessageBox.Show("Изменения сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            var usersEditingWindow = new UsersEditingWindow(RailStreamServer);
            usersEditingWindow.Show();
            this.Close();
        }
    }
}
