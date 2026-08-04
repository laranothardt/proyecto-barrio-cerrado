using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ.Modelo
{
    public class Residencia
    {
        public int IdResidencia { get; set; }
        public int IdPersona { get; set; }
        public Persona Persona { get; set; }
        public int IdLote { get; set; }
        public Lote Lote { get; set; }
        public string TipoVinculo { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
    }
}
