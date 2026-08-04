using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ.Modelo
{
    public class Persona
    {
        public int IdPersona { get; set; }
        public string Dni { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public bool HabilitadoReconocimientoFacial { get; set; }
        public byte[] Foto { get; set; }
        public DateTime FechaAlta { get; set; }
        public string Estado { get; set; }
        public int IdCategoria { get; set; }
        public CategoriaPersona Categoria { get; set; }
    }
}
