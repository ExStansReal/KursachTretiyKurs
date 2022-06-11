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
using System.Windows.Shapes;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using Newtonsoft.Json;
using System.Diagnostics;


namespace Trenirovka
{
    /// <summary>
    /// Логика взаимодействия для Adding_Upraznenie.xaml
    /// </summary>
    public partial class Adding_Upraznenie : Window
    {
        User Polzovatel = null;
        string Key = "";
        public Adding_Upraznenie(User a, string _key)
        {
            InitializeComponent();
            Polzovatel = a;
            Key = _key;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainWindow a = new MainWindow();
            a.Top = this.Top;
            a.Left = this.Left;
            this.Hide();
            a.Show();
        }
        private bool IsExists()
        {
            FirebaseResponse res = client.Get($"Users/{Polzovatel.ID_User}/Trenirovka/");
            Dictionary<string, LichnieTrenirovki> data = JsonConvert.DeserializeObject<Dictionary<string, LichnieTrenirovki>>(res.Body.ToString());
            if (data != null)
            {
                foreach (var a in data)
                {
                    if (a.Key.ToString() == Key)
                    {
                        LichnieTrenirovki abs = new LichnieTrenirovki()
                        {
                            ListOfUprazneniy = a.Value.ListOfUprazneniy
                        };
                        foreach (Uprazninie b in abs.ListOfUprazneniy)
                        {
                            if (b.Name == name.Text)
                                return true;
                        }
                        break;
                    }
                }
            }
            return false;
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(name.Text) == false)
            {
                if (IsExists() == false)
                {
                    if (String.IsNullOrWhiteSpace(opisanie.Text) == false)
                    {
                        if (String.IsNullOrWhiteSpace(kol.Text) == false)
                        {
                            if (Convert.ToInt32(kol.Text) > 0)
                            {
                                if (String.IsNullOrWhiteSpace(how.Text) == false)
                                {

                                    FirebaseResponse res = client.Get($"Users/{Polzovatel.ID_User}/Trenirovka/");
                                    Dictionary<string, LichnieTrenirovki> data = JsonConvert.DeserializeObject<Dictionary<string, LichnieTrenirovki>>(res.Body.ToString());
                                    if (data != null)
                                    {
                                        foreach (var a in data)
                                        {
                                            if (a.Key.ToString() == Key)
                                            {
                                                LichnieTrenirovki abs = new LichnieTrenirovki()
                                                {
                                                    Name = a.Value.Name,
                                                    Opisanie = a.Value.Opisanie,
                                                    Rating = a.Value.Rating,
                                                    ID_Sozdatel = a.Value.ID_Sozdatel,
                                                    CountOfVotes = a.Value.CountOfVotes,                                                    
                                                    ListOfUprazneniy = a.Value.ListOfUprazneniy
                                                };
                                                abs.ListOfUprazneniy.Add(new Uprazninie(Key, true, 1, name.Text, opisanie.Text,
                                                    Convert.ToInt32(kol.Text), ssilka.Text, how.Text));
                                                var setter = client.Set($"Users/{Polzovatel.ID_User}/Trenirovka/{Key}", abs);
                                                MessageBox.Show("Данне добавлены!");
                                                MainWindow aMain = new MainWindow();
                                                aMain.Top = this.Top;
                                                aMain.Left = this.Left;
                                                this.Hide();
                                                aMain.Show();
                                            }
                                        }
                                    }
                                }
                                else
                                    MessageBox.Show("Введите описание того, как выполнять данное упражнения");
                            }
                            else
                                MessageBox.Show("Нужно делать хотя-бы 1 повторение для упражнения");
                        }
                        else
                            MessageBox.Show("Введите колличество повторений для упражнения");
                    }
                    else
                        MessageBox.Show("Введите описание упражнения");
                }
                else
                    MessageBox.Show("Такое упражнение уже есть");
            }
            else
                MessageBox.Show("Введите название упражнения");
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }
    }
}
