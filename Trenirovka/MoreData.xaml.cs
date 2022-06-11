using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace Trenirovka
{
    /// <summary>
    /// Логика взаимодействия для MoreData.xaml
    /// </summary>
    public partial class MoreData : Window
    {
        public MoreData(Uprazninie a)
        {
            InitializeComponent();
            this.Title = a.Name;
            opisanie.Text = a.Opisanie;
            howTo.Text = a.HowToDoIt;
            if (String.IsNullOrWhiteSpace(a.Mishchi) == false)
            {
                try
                {
                    img.Source = new BitmapImage(new Uri(a.Mishchi));
                }
                catch
                {
                    MessageBox.Show("Ошибка ссылки на картинку, возможно она не корректная");
                }
            }
   
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

    }
}
