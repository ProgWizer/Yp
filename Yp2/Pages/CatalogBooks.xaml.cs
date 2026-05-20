using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Yp2.Pages
{
    public partial class CatalogBooks : Page
    {
        public CatalogBooks()
        {
            InitializeComponent();

            LoadObj();
        }

        private void LoadObj()
        {
            BooksList.ItemsSource = Core.DB.Books.ToList();

            
            cmbGenre.Items.Add("Все жанры");

            foreach (var genre in Core.DB.Genres.ToList())
            {
                cmbGenre.Items.Add(genre.Name);
            }

            cmbGenre.SelectedIndex = 0;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            FilterBooks();
        }

        private void cmbGenre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterBooks();
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterBooks();
        }

        private void FilterBooks()
        {
            var books = Core.DB.Books.ToList();

            if (!string.IsNullOrWhiteSpace(Search.Text))
            {
                books = books.Where(b => b.Title.ToLower().Contains(Search.Text.ToLower())).ToList();
            }

            if (cmbGenre.SelectedItem != null && cmbGenre.SelectedItem.ToString() != "Все жанры")
            {
                string selectedGenre = cmbGenre.SelectedItem.ToString();

                books = books.Where(b => b.Genres.Any(g => g.Name == selectedGenre)).ToList();
            }

            if (cmbSort.SelectedItem != null)
            {
                string sort = ((ComboBoxItem)cmbSort.SelectedItem) .Content.ToString();

                switch (sort)
                {
                    case "По имени":
                        books = books.OrderBy(b => b.Title).ToList();
                        break;
                    case "По рейтингу":
                        books = books.OrderByDescending(b =>b.Reviews.Any()? b.Reviews.Average(r => r.Rating): 0).ToList();
                        break;
                }
            }

            BooksList.ItemsSource = books;
        }

        private void OpenBook_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            int bookId = (int)button.Tag;

            NavigationService.Navigate(new BookPage(bookId));
        }
    }
}