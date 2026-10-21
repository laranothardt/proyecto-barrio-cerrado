using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIZ.Modelo;

namespace BIZ.Data
{
    public class Persona
    {
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Grupo7"].ConnectionString;
        }
        public int ObtenerPersonaPorDni(string dni, out string nombreCompleto)
        {
            nombreCompleto = string.Empty;
            if (string.IsNullOrWhiteSpace(dni)) return 0;
            dni = dni.Trim();

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string querySelect = "SELECT IDPersona, Nombre, Apellido FROM Persona WHERE DNI = @DNI";
                using (SqlCommand cmdSelect = new SqlCommand(querySelect, conn))
                {
                    cmdSelect.Parameters.AddWithValue("@DNI", dni);
                    conn.Open();
                    using (SqlDataReader reader = cmdSelect.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["IDPersona"]);
                            string nom = reader["Nombre"] == DBNull.Value ? "" : reader["Nombre"].ToString().Trim();
                            string ape = reader["Apellido"] == DBNull.Value ? "" : reader["Apellido"].ToString().Trim();

                            nombreCompleto = $"{nom} {ape}".Trim();
                            return id;
                        }
                    }
                }
            }
            return 0; // No existe en la base de datos
        }

        public int CrearPersona(string dni, string nombre, string apellido)
        {
            if (string.IsNullOrWhiteSpace(dni)) return 0;

            string nombreFinal = string.IsNullOrWhiteSpace(nombre) ? "Sin Nombre" : nombre.Trim();
            string apellidoFinal = string.IsNullOrWhiteSpace(apellido) ? "Sin Apellido" : apellido.Trim();

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string queryInsert = @"INSERT INTO Persona (Nombre, Apellido, DNI, FechaAlta, FK_IDCategoria, FK_Estado)
                                       VALUES (@Nombre, @Apellido, @DNI, GETDATE(), 2, 1);
                                       SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmdInsert = new SqlCommand(queryInsert, conn))
                {
                    cmdInsert.Parameters.AddWithValue("@Nombre", nombreFinal);
                    cmdInsert.Parameters.AddWithValue("@Apellido", apellidoFinal);
                    cmdInsert.Parameters.AddWithValue("@DNI", dni.Trim());

                    conn.Open();
                    object newId = cmdInsert.ExecuteScalar();
                    return Convert.ToInt32(newId);
                }
            }
        }
    }
}