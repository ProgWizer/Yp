using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Yp2.Pages
{
    public partial class BookPage : Page
    {
        private int BookId;
        private Books book;

        public BookPage(int bookId)
        {
            InitializeComponent();

            BookId = bookId;

            LoadBook();
            LoadReviews();
            CheckRole();
        }

        private void LoadBook()
        {
            book = Core.DB.Books.FirstOrDefault(b => b.Id == BookId);

            if (book == null)
            {
                MessageBox.Show("Книга не найдена");
                return;
            }
            if (book.IsFrozen == true)
            {
                MessageBox.Show("Книга заморожена");

                SendReviewBtn.Visibility = Visibility.Collapsed;
            }

            TitleText.Text = book.Title;

            DescriptionText.Text = book.Description;

            if (book.Authors != null)
            {
                AuthorText.Text = "Автор: " + book.Authors.Users.Username;
            }

            GenresText.Text = "Жанры: " + string.Join(", ", book.Genres.Select(g => g.Name));


            double avg = 0;

            if (book.Reviews.Any())
            {
                avg = (double)book.Reviews.Average(r => r.Rating);
            }

            RatingText.Text = avg.ToString("0.0");

        }

        private void LoadReviews()
        {
            ReviewsList.ItemsSource = Core.DB.Reviews.Where(r => r.BookId == BookId).ToList();
        }

        private void CheckRole()
        {
            //if (Core.CurrentUser.Role == "Admin")
            {
                FreezeBookBtn.Visibility = Visibility.Visible;
            }
        }

        private void SendReview_Click(object sender, RoutedEventArgs e)
        {
            if (RatingBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите оценку");
                return;
            }

            int rating = int.Parse(
                ((ComboBoxItem)RatingBox.SelectedItem).Content.ToString());

            Reviews review = new Reviews()
            {
                UserId = Core.CurrentUser.Id,
                BookId = BookId,
                Rating = rating,
                Comment = ReviewText.Text
            };

            Core.DB.Reviews.Add(review);
            Core.DB.SaveChanges();

            MessageBox.Show("Отзыв добавлен");

            LoadReviews();
            LoadBook();
        }

        private void AddToList_Click(object sender, RoutedEventArgs e)
        {
            var firstList = Core.DB.BookLists.FirstOrDefault();

            UserBookLists newEntry = new UserBookLists
            {
                UserId = Core.CurrentUser.Id,
                BookId = BookId,
                ListId = firstList.Id
            };

            Core.DB.UserBookLists.Add(newEntry);
            Core.DB.SaveChanges();


        }

        private void ReportBook_Click(object sender, RoutedEventArgs e)
        {
            Reports report = new Reports()
            {
                //ReporterId = Core.CurrentUser.Id,
                TargetId = BookId,
                //TargetType = "Book",
                Reason = "Жалоба на книгу"
            };

            Core.DB.Reports.Add(report);
            Core.DB.SaveChanges();

            MessageBox.Show("Жалоба отправлена");
        }

        private void ReportReview_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            int reviewId = (int)btn.Tag;

            Reports report = new Reports()
            {
                //ReporterId = Core.CurrentUser.Id,
                TargetId = reviewId,
                //TargetType = "Review",
                Reason = "Жалоба на отзыв"
            };

            Core.DB.Reports.Add(report);
            Core.DB.SaveChanges();

            MessageBox.Show("Жалоба отправлена");
        }

        private void ReportAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (book.AuthorId == null)
            {
                MessageBox.Show("Автор не найден");
                return;
            }

            Reports report = new Reports()
            {
                TargetId = (int)book.AuthorId,
                Reason = "Жалоба на автора"
            };

            Core.DB.Reports.Add(report);
            Core.DB.SaveChanges();

            MessageBox.Show("Жалоба отправлена");
        }

        private void FreezeBook_Click(object sender, RoutedEventArgs e)
        {
            book.IsFrozen = true;

            Core.DB.SaveChanges();

            MessageBox.Show("Книга заморожена");
        }

        private void FreezeReview_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            int reviewId = (int)btn.Tag;

            Reviews review = Core.DB.Reviews.FirstOrDefault(r => r.Id == reviewId);

            if (review != null)
            {
                review.IsFrozen = true;

                Core.DB.SaveChanges();

                MessageBox.Show("Отзыв заморожен");

                LoadReviews();
            }
        }


    }
}