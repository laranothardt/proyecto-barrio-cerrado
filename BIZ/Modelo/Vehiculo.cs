using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ.Modelo
{
    public class Vehiculo
    {
        public int IdVehiculo { get; set; }
        public string Patente { get; set; }
        public string Seguro { get; set; }
        public DateTime VencimientoSeguro { get; set; }
        public List<Persona> Titulares { get; set; } = new List<Persona>();
    }
}
