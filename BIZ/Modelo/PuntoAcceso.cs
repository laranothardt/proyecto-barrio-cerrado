using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ.Modelo
{
    public class PuntoAcceso
    {
        public int IdAcceso { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public bool TieneCamaraFacial { get; set; }
    }
}
