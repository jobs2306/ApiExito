using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormExito.Model
{
    public class Cliente
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string? nit { get; set; }
        public int? cc { get; set; }
        public string? celular { get; set; }
    }
}
