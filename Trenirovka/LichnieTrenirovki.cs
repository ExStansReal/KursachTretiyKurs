using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trenirovka
{
    public class LichnieTrenirovki
    {
        public string Name { get; set; }
        public int ID_Sozdatel { get; set; }
        public string Opisanie { get; set; }
        public double Rating { get; set; }
        public List<int> CountOfVotes = new List<int>();
        public List<Uprazninie> ListOfUprazneniy = new List<Uprazninie>();
    }
}
