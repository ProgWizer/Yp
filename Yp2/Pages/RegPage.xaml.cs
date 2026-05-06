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
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
        }

        private void BBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
            {

                NavigationService.GoBack();
            }
            else
            {

                MessageBox.Show("Назад больше нельзя");
            }
        }
        public enum role
        {
            User,
            Author,
            Admin

        }

        private void reg_Click(object sender, RoutedEventArgs e)
        {

            if (Core.DB.Users.Any(u => u.Login == login.Text))
            {
                MessageBox.Show("Такой логин уже существует");
                return;
            }

            Users user = new Users
            {
                Username = Nameu.Text,
                Login = login.Text,
                Password = passw.Text,
            };

            Core.DB.Users.Add(user);
            Core.DB.SaveChanges();

            MessageBox.Show("Регистрация успешна");
            NavigationService.Navigate(new AuthPage());
        }

        private void enter_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage());
        }
    }
}
