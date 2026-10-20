using System;

namespace BIZ.Modelo
{
    public class RestriccionHoraria
    {
        public int IdRestriccion { get; set; }
        public int? IdPersona { get; set; }
        public Persona Persona { get; set; }
        public int? IdCategoria { get; set; }
        public CategoriaPersona Categoria { get; set; }

        // Cambiado a string para coincidir con varchar(20) en la BD
        public string DiaSemana { get; set; }

        public TimeSpan HoraDesde { get; set; }
        public TimeSpan HoraHasta { get; set; }
        public DateTime VigenteDesde { get; set; }
        public DateTime VigenteHasta { get; set; }
    }
}