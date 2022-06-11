using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Trenirovka
{
    public class Uprazninie
    {
     

        public Uprazninie(string _NameTreni, bool _Verhnuya,int _number,string _name, string _opisan, int _Kollich,string _path, string do_it)
        {
            NameTrenirovki = _NameTreni;
            Verhnaya = _Verhnuya;
            Number = _number;
            Name = _name;
            Opisanie = _opisan;
            KollischestvoPovtorov = _Kollich;
            Mishchi = _path;
            HowToDoIt = do_it;
        }
        public string NameTrenirovki { get; set; }
        public bool Verhnaya { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Opisanie { get; set; }
        public int KollischestvoPovtorov { get; set; }
        public string Mishchi { get; set; }
        public string HowToDoIt { get; set; }
    }
}
