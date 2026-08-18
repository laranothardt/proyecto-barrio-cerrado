using BIZ.Modelo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ.Data
{
    public class Vehiculo
    {
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Grupo7"].ConnectionString;
        }
        public BIZ.Modelo.Vehiculo ObtenerVehiculoPatente(string patente)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = @"
                    SELECT 
                        v.id_vehiculo, 
                        v.patente, 
                        v.seguro, 
                        v.vencimiento_seguro,
                        p.id_persona,
                        p.nombre,
                        p.apellido
                    FROM Vehiculo v
                    LEFT JOIN Persona_Vehiculo pv ON v.id_vehiculo = pv.id_vehiculo
                    LEFT JOIN Persona p ON pv.id_persona = p.id_persona
                    WHERE v.patente = @Patente";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Patente", patente);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        BIZ.Modelo.Vehiculo vehiculo = null;

                        while (reader.Read())
                        {

                            if (vehiculo == null)
                            {
                                vehiculo = new BIZ.Modelo.Vehiculo
                                {
                                    IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                                    Patente = reader["patente"].ToString(),
                                    Seguro = reader["seguro"].ToString(),
                                    VencimientoSeguro = Convert.ToDateTime(reader["vencimiento_seguro"])
                                };
                            }


                            if (reader["id_persona"] != DBNull.Value)
                            {
                                vehiculo.Titulares.Add(new BIZ.Modelo.Persona
                                {
                                    IdPersona = Convert.ToInt32(reader["id_persona"]),
                                    Nombre = reader["nombre"].ToString(),
                                    Apellido = reader["apellido"].ToString()
                                });
                            }
                        }

                        return vehiculo; // Retorna null si no se encontró la patente en la BD
                    }
                }
            }
        }


        public bool AgregarVehiculo(BIZ.Modelo.Vehiculo vehiculo)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();

                // Iniciamos la transacción para asegurar atomicidad
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Insertar el Vehículo y obtener el ID generado
                        string queryVehiculo = @"
                    INSERT INTO Vehiculo (Patente, Seguro, vencimiento_seguro) 
                    VALUES (@Patente, @Seguro, @VencimientoSeguro);
                    SELECT SCOPE_IDENTITY();"; // Retorna el ID autoincremental generado

                        int idVehiculoGenerado;

                        using (SqlCommand cmdVehiculo = new SqlCommand(queryVehiculo, conn, transaction))
                        {
                            cmdVehiculo.Parameters.AddWithValue("@Patente", vehiculo.Patente);
                            cmdVehiculo.Parameters.AddWithValue("@Seguro", vehiculo.Seguro);
                            cmdVehiculo.Parameters.AddWithValue("@VencimientoSeguro", vehiculo.VencimientoSeguro);

                            // ExecuteScalar ejecuta la consulta y devuelve la primera columna de la primera fila
                            idVehiculoGenerado = Convert.ToInt32(cmdVehiculo.ExecuteScalar());
                        }

                        // 2. Insertar cada relación en Persona_Vehiculo usando la lista
                        string queryRelacion = "INSERT INTO Persona_Vehiculo (id_persona, id_vehiculo) VALUES (@IdPersona, @IdVehiculo)";

                        foreach (var persona in vehiculo.Titulares)
                        {
                            using (SqlCommand cmdRelacion = new SqlCommand(queryRelacion, conn, transaction))
                            {
                                cmdRelacion.Parameters.AddWithValue("@IdPersona", persona.IdPersona);
                                cmdRelacion.Parameters.AddWithValue("@IdVehiculo", idVehiculoGenerado);
                                cmdRelacion.ExecuteNonQuery();
                            }
                        }

                        // Confirmamos los cambios si todo salió bien
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        // Si algo falla en cualquier punto, revertimos todas las inserciones
                        transaction.Rollback();
                        throw; // O manejar la excepción según tu arquitectura
                    }
                }
            }
        }
    }
}

