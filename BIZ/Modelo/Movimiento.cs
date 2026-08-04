using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ.Modelo
{
    public class Movimiento
    {
        public int IdMovimiento { get; set; }
        public int? IdPersona { get; set; }
        public Persona Persona { get; set; }
        public int? IdVehiculo { get; set; }
        public Vehiculo Vehiculo { get; set; }
        public int? IdLoteDestino { get; set; }
        public Lote LoteDestino { get; set; }
        public int IdAcceso { get; set; }
        public PuntoAcceso PuntoAcceso { get; set; }
        public int? IdPreacreditacion { get; set; }
        public PreAcreditacion Preacreditacion { get; set; }
        public string Tipo { get; set; }
        public DateTime FechaHora { get; set; }
        public string MetodoValidacion { get; set; }
        public string Resultado { get; set; }
        public string MotivoDenegacion { get; set; }
        public string FotoCapturada { get; set; }
        public int? IdOperador { get; set; }
        public UsuarioSistema Operador { get; set; }
    }
}
