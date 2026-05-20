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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            RefreshData();
        }

        private void RefreshData()
        {
            ReportsGrid.ItemsSource = Core.DB.Reports.Where(r => r.Status == "Pending").ToList();
            AuthorRequestsGrid.ItemsSource = Core.DB.AuthorRequests.Where(a => a.Status == "Pending").ToList();
            UnfreezeRequestsGrid.ItemsSource = Core.DB.UnfreezeRequests.Where(u => u.Status == "Pending").ToList();
            UsersGrid.ItemsSource = Core.DB.Users.ToList();
        }

        private void ApproveAuthor_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            int userId = (int)btn.Tag;

            if (!Core.DB.Authors.Any(a => a.UserId == userId))
            {
                Core.DB.Authors.Add(new Authors { UserId = userId, Bio = "Новый автор" });
            }

            var req = Core.DB.AuthorRequests.FirstOrDefault(r => r.UserId == userId && r.Status == "Pending");
            if (req != null) req.Status = "Approved";

            Core.DB.SaveChanges();
            RefreshData();
        }

        private void ApproveUnfreeze_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            int freezeId = (int)btn.Tag;

            var freeze = Core.DB.Freezes.Find(freezeId);
            if (freeze != null)
            {
                freeze.IsActive = false;

                if (freeze.TargetType == "Book")
                {
                    var book = Core.DB.Books.Find(freeze.TargetId);
                    if (book != null) book.IsFrozen = false;
                }
            }

            var req = Core.DB.UnfreezeRequests.FirstOrDefault(r => r.FreezeId == freezeId);
            if (req != null) req.Status = "Completed";

            Core.DB.SaveChanges();
            RefreshData();
        }

        private void ChangePass_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as Button).DataContext as Users;
            user.Password = "123";
            Core.DB.SaveChanges();
            MessageBox.Show($"Пароль для {user.Username} изменен на стандартный.");
        }

        private void FreezeUser_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as Button).DataContext as Users;

            Freezes newFreeze = new Freezes
            {
                TargetId = user.Id,
                TargetType = "User",
                Reason = "Нарушение правил сообщества",
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            Core.DB.Freezes.Add(newFreeze);
            Core.DB.SaveChanges();
            MessageBox.Show("Пользователь заморожен.");
        }

        private void AcceptReport_Click(object sender, RoutedEventArgs e)
        {
            var report = (sender as Button).DataContext as Reports;
            report.Status = "Accepted";
            Core.DB.SaveChanges();
            RefreshData();
        }

        private void RejectReport_Click(object sender, RoutedEventArgs e)
        {
            var report = (sender as Button).DataContext as Reports;
            report.Status = "Rejected";
            Core.DB.SaveChanges();
            RefreshData();
        }
    }
}
