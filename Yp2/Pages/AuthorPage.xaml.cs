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
    /// Логика взаимодействия для AuthorPage.xaml
    /// </summary>
    public partial class AuthorPage : Page
    {
        private int currentAuthorId;

        public AuthorPage()
        {
            InitializeComponent();
            LoadAuthorData();
        }

        private void LoadAuthorData()
        {
            var author = Core.DB.Authors.FirstOrDefault(a => a.UserId == Core.CurrentUser.Id);
            if (author == null)
            {
                return;
            }
            currentAuthorId = author.Id;

            ActiveBooksGrid.ItemsSource = Core.DB.Books.Where(b => b.AuthorId == currentAuthorId && (b.IsFrozen == false || b.IsFrozen == null)).ToList();

            FrozenBooksGrid.ItemsSource = Core.DB.Books.Where(b => b.AuthorId == currentAuthorId && b.IsFrozen == true).ToList();
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditBookPage(null)); 
        }

        private void EditBook_Click(object sender, RoutedEventArgs e)
        {
            int bookId = (int)((Button)sender).Tag;
            NavigationService.Navigate(new EditBookPage(bookId));
        }

        private void AppealFreeze_Click(object sender, RoutedEventArgs e)
        {
            int bookId = (int)((Button)sender).Tag;


            var freeze = Core.DB.Freezes.FirstOrDefault(f => f.TargetId == bookId && f.TargetType == "Book" && f.IsActive == true);

            if (freeze != null)
            {
                var request = new UnfreezeRequests
                {
                    UserId = Core.CurrentUser.Id,
                    FreezeId = freeze.Id,
                    Reason = "Прошу разморозить книгу, все нарушения исправлены.",
                    Status = "Pending",
                    CreatedAt = DateTime.Now
                };
                Core.DB.UnfreezeRequests.Add(request);
                Core.DB.SaveChanges();
                MessageBox.Show("Заявка на разморозку отправлена.");
            }
        }
    }
}
