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
        public int IdPersona { get; set; }
        public string NombreTitular { get; set; }
        public string ApellidoTitular { get; set; }
    }
}
