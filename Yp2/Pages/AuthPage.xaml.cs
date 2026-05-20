using Microsoft.Win32;
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

namespace Yp2.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void B_Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Назад больше нельзя");
            }
        }

        private void reg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegPage());

        }

        private void enter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(login.Text) || string.IsNullOrWhiteSpace(passw.Text))
            {
                MessageBox.Show("Не все поля заполнены");
                return;
            }

            var user = Core.DB.Users.FirstOrDefault(u => u.Login == login.Text && u.Password == passw.Text);

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }

            Core.CurrentUser = user;

            MessageBox.Show("Успешно");
            NavigationService.Navigate(new MainPage());
        }
    }
}
