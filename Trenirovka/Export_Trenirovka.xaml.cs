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
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;

namespace Trenirovka
{
    /// <summary>
    /// Логика взаимодействия для Export_Trenirovka.xaml
    /// </summary>
    public partial class Export_Trenirovka : Window
    {
        User Polzovatel = null;
        LichnieTrenirovki Trenirovka = null;
        public Export_Trenirovka(User user, LichnieTrenirovki trenirovki)
        {
            InitializeComponent();
            Polzovatel = user;
            Trenirovka = trenirovki;
            name.Text = Trenirovka.Name;
            opisanie.Text = Trenirovka.Opisanie;
        }

        IFirebaseConfig fcon = new FirebaseConfig()
        {
            AuthSecret = "Ypj2iHSePko2ni02eUFiYafaGjRUy4wliLAuPy1d",
            BasePath = "https://training-5533b-default-rtdb.europe-west1.firebasedatabase.app/"
        };
        IFirebaseClient client;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(fcon);
            }
            catch
            {
                MessageBox.Show("Проверьте подключение к интернету");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch
            {

            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainWindow a = new MainWindow();
            a.Top = this.Top;
            a.Left = this.Left;
            this.Hide();
            a.Show();
        }

        private bool CheckSameTrenirovka()
        {
            FirebaseResponse res = client.Get($"ExportedTrenirovku/");
            Dictionary<string, LichnieTrenirovki> data = JsonConvert.DeserializeObject<Dictionary<string, LichnieTrenirovki>>(res.Body.ToString());
            if (data != null)
            {
                foreach (var a in data)
                {
                    if (a.Value.Name == name.Text)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(name.Text) == false)
            {
                if (String.IsNullOrWhiteSpace(opisanie.Text) == false)
                {
                    if (CheckSameTrenirovka() == false)
                    {
                        LichnieTrenirovki a = new LichnieTrenirovki();
                        a.Name = name.Text;
                        a.Opisanie = opisanie.Text;
                        a.ID_Sozdatel = Polzovatel.ID_User;
                        a.Rating = 0;
                        a.ListOfUprazneniy = Trenirovka.ListOfUprazneniy;
                        var setter = client.Set($"ExportedTrenirovku/{a.Name}", a);
                        MessageBox.Show("Данне добавлены!");
                        MainWindow aMain = new MainWindow();
                        aMain.Top = this.Top;
                        aMain.Left = this.Left;
                        this.Hide();
                        aMain.Show();
                    }
                    else
                        MessageBox.Show("Введите другое название тренировки, видимо такая тренировка уже есть");
                }
                else
                    MessageBox.Show("Введите описание для тренировки");
            }
            else
                MessageBox.Show("Введите название для тренировки");
        }
    }
}
