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
    /// Логика взаимодействия для MoveBook.xaml
    /// </summary>
    public partial class MoveBook : Page
    {
        private int BookId;

        public MoveBook(int bookId)
        {
            InitializeComponent();

            BookId = bookId;

            cmbLists.ItemsSource =
                Core.DB.BookLists.ToList();

            cmbLists.DisplayMemberPath = "Name";
        }

        private void Move_Click(object sender, RoutedEventArgs e)
        {
            BookLists list =
                cmbLists.SelectedItem as BookLists;

            if (list == null)
            {
                MessageBox.Show("Выберите список");
                return;
            }


        }
    }
}