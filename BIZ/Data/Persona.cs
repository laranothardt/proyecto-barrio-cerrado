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
        /// <summary>
        /// Devuelve el IdPersona del DNI; si no existe, lo crea con nombre y apellido reales.
        /// Si existe pero quedo con el nombre de relleno "Residente", lo corrige con los datos recibidos.
        /// </summary>
        public int ObtenerOCrearPersona(string dni, string nombre, string apellido, int idCategoria = 2)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new ArgumentException("El DNI es obligatorio.", nameof(dni));
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es obligatorio.", nameof(nombre));
            if (string.IsNullOrWhiteSpace(apellido))
                throw new ArgumentException("El apellido es obligatorio.", nameof(apellido));

            dni = dni.Trim();
            nombre = nombre.Trim();
            apellido = apellido.Trim();

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();

                // 1. Buscar si ya existe en la tabla Persona
                int idExistente = 0;
                string nombreActual = null;

                string querySelect = "SELECT IDPersona, Nombre FROM Persona WHERE DNI = @DNI";
                using (SqlCommand cmdSelect = new SqlCommand(querySelect, conn))
                {
                    cmdSelect.Parameters.AddWithValue("@DNI", dni);
                    using (SqlDataReader reader = cmdSelect.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idExistente = Convert.ToInt32(reader["IDPersona"]);
                            nombreActual = reader["Nombre"] == DBNull.Value ? null : reader["Nombre"].ToString();
                        }
                    }
                }

                if (idExistente != 0)
                {
                    // Filas viejas creadas con el relleno "Residente": se completan con los datos reales
                    if (string.Equals(nombreActual, "Residente", StringComparison.OrdinalIgnoreCase))
                    {
                        string queryUpdate = "UPDATE Persona SET Nombre = @Nombre, Apellido = @Apellido WHERE IDPersona = @IdPersona";
                        using (SqlCommand cmdUpdate = new SqlCommand(queryUpdate, conn))
                        {
                            cmdUpdate.Parameters.AddWithValue("@Nombre", nombre);
                            cmdUpdate.Parameters.AddWithValue("@Apellido", apellido);
                            cmdUpdate.Parameters.AddWithValue("@IdPersona", idExistente);
                            cmdUpdate.ExecuteNonQuery();
                        }
                    }
                    return idExistente;
                }

                // 2. Si no existe, registrarlo en Persona con los datos recibidos
                string queryInsert = @"INSERT INTO Persona (Nombre, Apellido, DNI, FechaAlta, FK_IDCategoria, FK_Estado)
                               VALUES (@Nombre, @Apellido, @DNI, GETDATE(), @IdCategoria, 1);
                               SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmdInsert = new SqlCommand(queryInsert, conn))
                {
                    cmdInsert.Parameters.AddWithValue("@Nombre", nombre);
                    cmdInsert.Parameters.AddWithValue("@Apellido", apellido);
                    cmdInsert.Parameters.AddWithValue("@DNI", dni);
                    cmdInsert.Parameters.AddWithValue("@IdCategoria", idCategoria);

                    object newId = cmdInsert.ExecuteScalar();
                    return Convert.ToInt32(newId);
                }
            }
        }        public int ObtenerIdPersonaPorDni(string dni)
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
        public string ObtenerNombrePorDni(string dni)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = "SELECT Nombre + ' ' + Apellido FROM Persona WHERE DNI = @DNI";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DNI", dni);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null && result != DBNull.Value ? result.ToString() : null;
                }
            }
        }
    }
}
