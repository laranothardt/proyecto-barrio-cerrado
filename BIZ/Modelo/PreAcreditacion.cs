using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ.Modelo
{
    public class PreAcreditacion
    {
        public int IdPreacreditacion { get; set; }
        public string Dni { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public int IdCategoria { get; set; }
        public CategoriaPersona Categoria { get; set; }
        public int IdLoteDestino { get; set; }
        public Lote LoteDestino { get; set; }
        public int IdResidenteAutoriza { get; set; }
        public Persona ResidenteAutoriza { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; }
    }
}
