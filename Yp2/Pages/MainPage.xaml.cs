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
        /// Логика взаимодействия для MainPage.xaml
        /// </summary>
        public partial class MainPage : Page
        {
            public MainPage()
            {
                InitializeComponent();
                PageFrame.Navigate(new CatalogBooks());
                //Admin.Visibility = Visibility.Visible;

                if (Core.CurrentUser != null && Core.CurrentUser.Username == "admin")
                {
                    Admin.Visibility = Visibility.Visible;
                }
                Load();
            }

            private void Load()
            {
                
                Admin.Visibility = Visibility.Visible;
                //Author.Visibility = Visibility.Collapsed;
                //Frozen.Visibility = Visibility.Collapsed;

            if (Core.CurrentUser == null) return;


                

                bool isAuthor = Core.DB.Authors.Any(a => a.UserId == Core.CurrentUser.Id);

                if (isAuthor)
                {
                    Author.Visibility = Visibility.Visible;
                }
            }

            private void Catalog_Click(object sender, RoutedEventArgs e)
            {
                PageFrame.Navigate(new CatalogBooks());
            }

            private void Books_Click(object sender, RoutedEventArgs e)
            {
                PageFrame.Navigate(new BookListsPage());
            }

            private void Profile_Click(object sender, RoutedEventArgs e)
            {
                PageFrame.Navigate(new Profile());
            }

            private void Admin_Click(object sender, RoutedEventArgs e)
            {
                PageFrame.Navigate(new AdminPage());
            }

            private void Author_Click(object sender, RoutedEventArgs e)
            {


                PageFrame.Navigate(new AuthorPage());
            }

            private void Frozen_Click(object sender, RoutedEventArgs e)
            {
                //PageFrame.Navigate(new FrozenPage());
            }



        }
    }
