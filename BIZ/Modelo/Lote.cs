using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ.Modelo
{
    public class Lote
    {
        public int IdLote { get; set; }
        public string NumeroLote { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string DireccionInterna { get; set; }
        public string Observaciones { get; set; }
    }
}
