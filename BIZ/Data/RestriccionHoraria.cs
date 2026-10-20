using System;
using System.Data.SqlClient;
using System.Configuration;

namespace BIZ.Data
{
    public class RestriccionHoraria
    {
        private string connectionString = ConfigurationManager.ConnectionStrings[1].ConnectionString;

        public bool EliminarRestriccionesPorCategoria(int idCategoria)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Nombre de columna actualizado a FK_ID_Categoria
                    string query = "DELETE FROM RestriccionHoraria WHERE FK_ID_Categoria = @IdCategoria";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@IdCategoria", idCategoria);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Crear(BIZ.Modelo.RestriccionHoraria restriccion)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Nombres de columnas actualizados según tu tabla
                    string query = @"INSERT INTO RestriccionHoraria 
                                     (FK_ID_Categoria, Dia_Semana, Hora_Desde, Hora_Hasta, Vigente_Desde, Vigente_Hasta) 
                                     VALUES (@IdCategoria, @DiaSemana, @HoraDesde, @HoraHasta, @VigenteDesde, @VigenteHasta)";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@IdCategoria", restriccion.IdCategoria.HasValue ? (object)restriccion.IdCategoria.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@DiaSemana", restriccion.DiaSemana);
                    cmd.Parameters.AddWithValue("@HoraDesde", restriccion.HoraDesde);
                    cmd.Parameters.AddWithValue("@HoraHasta", restriccion.HoraHasta);
                    cmd.Parameters.AddWithValue("@VigenteDesde", restriccion.VigenteDesde);
                    cmd.Parameters.AddWithValue("@VigenteHasta", restriccion.VigenteHasta);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en BD: " + ex.Message);
            }
        }
    }
}