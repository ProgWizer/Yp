using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Логика взаимодействия для EditBookPage.xaml
    /// </summary>
    public partial class EditBookPage : Page
    {
        private Books currentBook;

        public EditBookPage(int? bookId)
        {
            InitializeComponent(); if (bookId == null) 
            {
                currentBook = new Books();
                PageHeader.Text = "Новая книга";
            }
            else 
            {
                currentBook = Core.DB.Books.Find(bookId);
                TitleBox.Text = currentBook.Title;
                DescBox.Text = currentBook.Description;
                UrlBox.Text = currentBook.CoverUrl;
                PageHeader.Text = "Редактирование: " + currentBook.Title;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleBox.Text)) { 
                MessageBox.Show("Введите название"); 
                return; 
            }

            currentBook.Title = TitleBox.Text;
            currentBook.Description = DescBox.Text;
            currentBook.CoverUrl = UrlBox.Text;

                // новая книга
            if (currentBook.Id == 0) 
            {
                var author = Core.DB.Authors.FirstOrDefault(a => a.UserId == Core.CurrentUser.Id);
                currentBook.AuthorId = author.Id;
                currentBook.CreatedAt = DateTime.Now;
                currentBook.IsFrozen = false;
                Core.DB.Books.Add(currentBook);
            }

            Core.DB.SaveChanges();
            MessageBox.Show("Данные сохранены!");
            NavigationService.GoBack();
        }


    }
}
