using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Core;

namespace Shop.Pages
{
    /// <summary>
    /// Interaction logic for AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        private int countLoginAttempt = 1;
        public AuthorizationPage()
        {
            InitializeComponent();

            tbLogin.Text = Properties.Settings.Default.Login;
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var login = tbLogin.Text;
            var password = pbPassword.Password;

            if (DataAccess.IsLoggingCorrect(login, password) && countLoginAttempt < 3 && Properties.Settings.Default.LastLoginAttempt == DateTime.MinValue)
            {
                if (cbRemember.IsChecked.GetValueOrDefault())
                    Properties.Settings.Default.Login = login;
                else
                    Properties.Settings.Default.Login = null;
                Properties.Settings.Default.LastLoginAttempt = DateTime.MinValue;
                countLoginAttempt = 1;
                Properties.Settings.Default.Save();
                NavigationService.Navigate(new ProductsPage(DataAccess.GetUser(login, password).RoleId));
            }
            else if (countLoginAttempt == 3 || Properties.Settings.Default.LastLoginAttempt != DateTime.MinValue)
            {
                if (Properties.Settings.Default.LastLoginAttempt == DateTime.MinValue)
                {
                    Properties.Settings.Default.LastLoginAttempt = DateTime.Now;
                    MessageBox.Show("3 неверные попытки. Начнем минутную игру", "Ошибка");
                    Properties.Settings.Default.Save();
                }
                else if (DateTime.Now - Properties.Settings.Default.LastLoginAttempt >= TimeSpan.FromSeconds(60))
                {
                    countLoginAttempt = 1;
                    Properties.Settings.Default.LastLoginAttempt = DateTime.MinValue;
                    Properties.Settings.Default.Save();
                }
                else if (DateTime.Now - Properties.Settings.Default.LastLoginAttempt < TimeSpan.FromSeconds(60))
                {
                    MessageBox.Show($"В игру осталось играть {Math.Round(60 - (DateTime.Now - Properties.Settings.Default.LastLoginAttempt).TotalSeconds, 2)} секунд", "Предупреждение");
                }
                return;
            }
            else
            {
                MessageBox.Show($"Неверный логин или пароль\nОсталось {3 - countLoginAttempt} попытки входа", "Ошибка");
                countLoginAttempt++;
            }
        }
    }
}
