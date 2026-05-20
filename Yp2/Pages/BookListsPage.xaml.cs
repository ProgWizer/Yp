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
    /// Логика взаимодействия для BookListsPage.xaml
    /// </summary>
    public partial class BookListsPage : Page
    {
        public BookListsPage()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            cmbLists.ItemsSource = Core.DB.BookLists.ToList();
            cmbLists.DisplayMemberPath = "Name";
            
            cmbLists.SelectedIndex = 0;
            cmbGenre.SelectedIndex = 0;
            cmbSort.SelectedIndex = 0;

            cmbGenre.Items.Add("Все жанры");

            foreach (var genre in Core.DB.Genres.ToList())
            {
                cmbGenre.Items.Add(genre.Name);
            }
            LoadBooks();
        }

        private void LoadBooks()
        {
            if (cmbLists.SelectedItem == null)
            {
                return;
            }

            BookLists selectedList = cmbLists.SelectedItem as BookLists;

            var books = Core.DB.UserBookLists.Where(u => u.UserId == Core.CurrentUser.Id && u.ListId == selectedList.Id).Select(u => u.Books).ToList();

            BooksList.ItemsSource = books;
        }

        private void FilterBooks()
        {
            if (cmbLists.SelectedItem == null) { 
                return;
            }

            BookLists selectedList = cmbLists.SelectedItem as BookLists;

            var books = Core.DB.UserBookLists.Where(u => u.UserId == Core.CurrentUser.Id && u.ListId == selectedList.Id).Select(u => u.Books).ToList();

            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                string search = SearchBox.Text.ToLower();

                books = books.Where(b => b.Title.ToLower().Contains(search) || b.Authors.Users.Username.ToLower().Contains(search)).ToList();
            }

            if (cmbGenre.SelectedItem != null && cmbGenre.SelectedItem.ToString() != "Все жанры")
            {
                string genre = cmbGenre.SelectedItem.ToString();

                books = books.Where(b =>b.Genres.Any(g => g.Name == genre)).ToList();
            }

            if (cmbSort.SelectedItem != null)
            {
                string sort = ((ComboBoxItem)cmbSort.SelectedItem).Content.ToString();

                switch (sort)
                {
                    case "По имени":

                        books = books.OrderBy(b => b.Title).ToList();
                        break;
                    case "По рейтингу":

                        books = books.OrderByDescending(b => b.Reviews.Any() ? b.Reviews.Average(r => r.Rating) : 0).ToList();

                        break;
                }
            }

            BooksList.ItemsSource = books;
        }

        private void cmbLists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterBooks();
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

        private void OpenBook_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            int bookId = (int)btn.Tag;

            NavigationService.Navigate(new BookPage(bookId));
        }

        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            int bookId = (int)btn.Tag;

            var item = Core.DB.UserBookLists.FirstOrDefault(u => u.UserId == Core.CurrentUser.Id && u.BookId == bookId);

            if (item != null)
            {
                Core.DB.UserBookLists.Remove(item);

                Core.DB.SaveChanges();

                MessageBox.Show("Книга удалена");

                FilterBooks();
            }
        }

        private void MoveBook_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;

            cmb.ItemsSource = Core.DB.BookLists.ToList();

            cmb.DisplayMemberPath = "Name";

            cmb.SelectedValuePath = "Id";

            int bookId = (int)cmb.Tag;

            var current = Core.DB.UserBookLists.FirstOrDefault(u => u.UserId == Core.CurrentUser.Id && u.BookId == bookId);

            if (current != null)
            {
                cmb.SelectedValue = current.ListId;
            }
        }

        private void MoveBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;


            BookLists selectedList = cmb.SelectedItem as BookLists;

            int bookId = (int)cmb.Tag;

            var item = Core.DB.UserBookLists.FirstOrDefault(u => u.UserId == Core.CurrentUser.Id && u.BookId == bookId);

            if (item != null)
            {
                if (item.ListId != selectedList.Id)
                {
                    item.ListId = selectedList.Id;

                    Core.DB.SaveChanges();

                    FilterBooks();
                }
            }
        }
    }
}
