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
        public int ObtenerOCrearPersonaResidente(string dni, string nombreCompleto)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();

                // 1. Buscar si ya existe en la tabla Persona
                string querySelect = "SELECT IDPersona FROM Persona WHERE DNI = @DNI";
                using (SqlCommand cmdSelect = new SqlCommand(querySelect, conn))
                {
                    cmdSelect.Parameters.AddWithValue("@DNI", dni);
                    object result = cmdSelect.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }

                // 2. Si no existe, separar el nombre y registrarlo en Persona
                string[] partesNombre = nombreCompleto.Split(' ');
                string nombre = partesNombre[0];
                string apellido = partesNombre.Length > 1 ? string.Join(" ", partesNombre, 1, partesNombre.Length - 1) : "Residente";

                string queryInsert = @"INSERT INTO Persona (Nombre, Apellido, DNI, FechaAlta, FK_IDCategoria, FK_Estado) 
                               VALUES (@Nombre, @Apellido, @DNI, GETDATE(), 2, 1);
                               SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmdInsert = new SqlCommand(queryInsert, conn))
                {
                    cmdInsert.Parameters.AddWithValue("@Nombre", nombre);
                    cmdInsert.Parameters.AddWithValue("@Apellido", apellido);
                    cmdInsert.Parameters.AddWithValue("@DNI", dni);

                    object newId = cmdInsert.ExecuteScalar();
                    return Convert.ToInt32(newId);
                }
            }
        }
        public int ObtenerIdPersonaPorDni(string dni)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = "SELECT IDPersona FROM Persona WHERE DNI = @DNI";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DNI", dni);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }
    }
}
