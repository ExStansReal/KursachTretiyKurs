using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для CheckUser.xaml
    /// </summary>
    public partial class CheckUser : Window
    {
        public CheckUser()
        {
            InitializeComponent();
        }

        IFirebaseConfig fcon = new FirebaseConfig()
        {
            AuthSecret = "Ypj2iHSePko2ni02eUFiYafaGjRUy4wliLAuPy1d",
            BasePath = "https://training-5533b-default-rtdb.europe-west1.firebasedatabase.app/"
        };
        IFirebaseClient client;
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        private void TextBox_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "МЖ".IndexOf(e.Text) < 0;
        }
        public string CurretDir = Directory.GetCurrentDirectory();

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(rost.Text) == false &&
                                String.IsNullOrWhiteSpace(ves.Text) == false &&
                                String.IsNullOrWhiteSpace(age.Text) == false &&
                                String.IsNullOrWhiteSpace(pol.Text) == false && rost.Text != "" && ves.Text != "" && pol.Text != "")
                {
                    if (Convert.ToInt32(rost.Text) <= 272 && Convert.ToInt32(rost.Text) > 0)
                    {
                        if (Convert.ToInt32(ves.Text) <= 300 && Convert.ToInt32(ves.Text) > 0)
                        {
                            if (Convert.ToInt32(age.Text) <= 110 && Convert.ToInt32(age.Text) > 0)
                            {
                                if (pol.Text == "М" || pol.Text == "Ж")
                                {
                                    User user = new User();
                                    
                                    user.Age = Convert.ToInt32(age.Text);
                                    user.Weight = Convert.ToInt32(ves.Text);
                                    user.Rost = Convert.ToInt32(rost.Text);
                                    user.Gender = pol.Text;
                                    double rostMetri = Convert.ToDouble(rost.Text) / 100;
                                    user.Index = Convert.ToInt32(ves.Text) / Math.Pow(rostMetri,2);
                                    if (user.Index > 16 && user.Index <= 30)
                                        user.TipeIndex = 1;
                                    if (user.Index > 30 && user.Index <= 40)
                                        user.TipeIndex = 2;
                                    if (user.Index > 40)
                                        user.TipeIndex = 3;
                                    bool newUser = false;
                                    
                                    while (newUser == false)
                                    {
                                        Random rnd = new Random();
                                        user.ID_User = rnd.Next(0, 100000);
                                        FirebaseResponse res = client.Get($"Users/");
                                        Dictionary<string, User> data = JsonConvert.DeserializeObject<Dictionary<string, User>>(res.Body.ToString());
                                        if (data != null)
                                        {
                                            foreach (var abs in data)
                                            {
                                                
                                                User b = new User()
                                                {
                                                    ID_User = abs.Value.ID_User,
                                                };
                                                if (b.ID_User == user.ID_User)
                                                    newUser = false;
                                                else
                                                    newUser = true;
                                            }
                                        }
                                        else
                                            newUser = true;

                                    }
                                   

                                   
                                    /*16 и менее - выраженный дефицит
                                     * 16-18,5 - недостаточный
                                     * 18,5-25 - норма
                                     * 25-30 - избыточная масса тела
                                     * 30-35 - ожирение 1 степени
                                     * 35-40 - ожирение 2 степени                                    
                                     * 40 и более - ожирение 4 степени
                                     */

                                   
                                    
                                    var setterФ = client.Set("Users/" + user.ID_User, user);
                                    LichnieTrenirovki ad = new LichnieTrenirovki();
                                    ad.Name = "Личная тренировка";
                                    ad.ID_Sozdatel = user.ID_User;
                                    ad.Opisanie = "Ваша личная тренировка созданная автоматически при вводе ваших данных в приложение.";
                                    ad.Rating = 0;

                                    int resultAdding = 0;
                                    if (user.Age > 18 && user.Age < 30)
                                        resultAdding += 1;
                                    if (user.Age >= 30 && user.Age < 50)
                                        resultAdding += 2;
                                    if (user.Age >= 50 && user.Age < 110)
                                        resultAdding += -1;
                                    if (user.Gender == "М")
                                        resultAdding += 1;


                                    ad.ListOfUprazneniy.Add(new Uprazninie("Личная тренировка", true, 1, "Гиперэкстензия", 
                                        "Физическое упражнение для развития выпрямителей спины, сгибателей голени и ягодичных мышц.",
                10 + resultAdding,
                @"https://sport-i-zdorovie.ru/wp-content/uploads/2019/11/giperjekstenzija-myshcy.jpg",
                "Скрестите руки за головой или на груди. Выпрямите корпус в абсолютно прямую линию.Взгляд строго вперед. Центр тяжести на пятках. Опускайте корпус вниз, сгибаясь только в пояснице. Дойдя до угла 90°, верните корпус назад в исходное положение. При движении не допускайте рывков и переразгибаний спины. Задержитесь в верхней точке на 1-2 секунды"));

                                    var setterB = client.Set("Users/" + user.ID_User + "/Trenirovka/Личная тренировка", ad);


                                    using (BinaryWriter writer = new BinaryWriter(File.Open(CurretDir + @"\UserData.dat", FileMode.OpenOrCreate)))
                                    {
                                        writer.Write(user.TipeIndex);
                                        writer.Write(user.Index);
                                        writer.Write(user.Gender);
                                        writer.Write(user.Age);
                                        writer.Write(user.Weight);
                                        writer.Write(user.Rost);
                                        writer.Write(user.ID_User);
                                    }

                                    MainWindow a = new MainWindow();
                                    a.Top = this.Top;
                                    a.Left = this.Left;
                                    this.Hide();
                                    a.Show();

                                    MessageBox.Show("ДОСТУП РАЗРЕШЁН");
                                }
                                else
                                    MessageBox.Show("Вы ввели не верный пол");
                            }
                            else
                                MessageBox.Show("Максимальный возраст - 110 лет, минимальный возраст - 6 лет");
                        }
                        else
                            MessageBox.Show("Максимальный вес - 300 кг, минимальный вес - 45 кг");
                    }
                    else
                        MessageBox.Show("Максимальный рост - 272 см, минимальный рост - 50 см");

                }
                else
                    MessageBox.Show("Введите все данные");
            }
            catch
            {
                MessageBox.Show("Ой-Ой!\n Что-то пошло не так..");
            }
           
        }
       
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            client = new FireSharp.FirebaseClient(fcon);
        }
    }
}
