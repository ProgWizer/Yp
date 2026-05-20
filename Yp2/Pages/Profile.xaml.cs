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
using System.Data.Entity;

namespace Yp2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        public Profile()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            var user = Core.DB.Users.Include(u => u.Roles).FirstOrDefault(u => u.Id == Core.CurrentUser.Id);


            if (user == null)
            {
                return;
            }

            UsernameTxt.Text = user.Username;
            EmailTxt.Text = user.Email;

            var rolesList = user.Roles.Select(r => r.Name).ToList();
            RolesTxt.Text = string.Join(", ", rolesList);

            var freeze = Core.DB.Freezes.FirstOrDefault(f => f.TargetId == user.Id && f.TargetType == "User" && f.IsActive == true);
            if (freeze != null)
            {
                FreezeAlert.Visibility = Visibility.Visible;
                FreezeReasonText.Text = $"Причина: {freeze.Reason}";
            }

            if (!rolesList.Contains("Author"))
            {
                BecomeAuthorBtn.Visibility = Visibility.Visible;
            }

            LoadReviews(user.Id);
        }


        private void LoadReviews(int userId)
        {
            var reviews = Core.DB.Reviews.Where(r => r.UserId == userId).
                Select(r => new
                {
                    BookTitle = r.Books.Title,
                    RatingDisplay = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                })
                .ToList();

            ReviewsControl.ItemsSource = reviews;
        }

        private void BecomeAuthor_Click(object sender, RoutedEventArgs e)
        {
            var request = new AuthorRequests
            {
                UserId = Core.CurrentUser.Id,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            Core.DB.AuthorRequests.Add(request);
            Core.DB.SaveChanges();

            MessageBox.Show("Заявка на роль автора успешно отправлена!");
            BecomeAuthorBtn.IsEnabled = false;
        }

        private void UnfreezeRequest_Click(object sender, RoutedEventArgs e)
        {
            var freeze = Core.DB.Freezes.FirstOrDefault(f => f.TargetId == Core.CurrentUser.Id && f.IsActive == true);

            if (freeze != null)
            {
                var unfreezeReq = new UnfreezeRequests
                {
                    UserId = Core.CurrentUser.Id,
                    FreezeId = freeze.Id,
                    Reason = "Прошу разморозить, так как я осознал ошибку...",
                    Status = "Pending",
                    CreatedAt = DateTime.Now
                };

                Core.DB.UnfreezeRequests.Add(unfreezeReq);
                Core.DB.SaveChanges();
                MessageBox.Show("Запрос на разморозку отправлен модераторам.");
            }
        }

    }
}
