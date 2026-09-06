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
        public static DataSet ObtenerVehiculo()
        {
            DataSet ds = new DataSet();
            string cn = ConfigurationManager.ConnectionStrings["Grupo7"].ConnectionString;
            string query = @"
                SELECT 
                    v.id_vehiculo, 
                    v.patente, 
                    v.seguro, 
                    v.vencimiento_seguro,
                    p.nombre,
                    p.apellido
                FROM Vehiculo v
                LEFT JOIN Persona_Vehiculo pv ON v.id_vehiculo = pv.id_vehiculo
                LEFT JOIN Persona p ON pv.id_persona = p.id_persona";
            try
            {
                using (SqlConnection CN = new SqlConnection(cn))
                {
                    using (SqlCommand cmd = new SqlCommand(query, CN))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(ds);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ds = null;
                Console.WriteLine("Error al obtener el vehículo: {0}", ex.Message);
            }
            return ds;
        }
        public static DataSet ObtenerVehiculoPatente(string patente)
        {
            DataSet ds = new DataSet();
            string cn = ConfigurationManager.ConnectionStrings["Grupo7"].ConnectionString;
            string query = @"
                SELECT 
                    v.id_vehiculo, 
                    v.patente, 
                    v.seguro, 
                    v.vencimiento_seguro,
                    p.nombre,
                    p.apellido
                FROM Vehiculo v
                LEFT JOIN Persona_Vehiculo pv ON v.id_vehiculo = pv.id_vehiculo
                LEFT JOIN Persona p ON pv.id_persona = p.id_persona
                WHERE v.patente = @Patente";
            try
            {
                using (SqlConnection CN = new SqlConnection(cn))
                {
                    using (SqlCommand cmd = new SqlCommand(query, CN))
                    {
                        cmd.Parameters.AddWithValue("@Patente", patente);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(ds);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ds = null;
                Console.WriteLine("Error al obtener el vehículo: {0}", ex.Message);
            }
            return ds;
        }
        public static DataSet ObtenerVehiculoTitular(string titular)
        {
            DataSet ds = new DataSet();
            string cn = ConfigurationManager.ConnectionStrings["Grupo7"].ConnectionString;
            string query = @"
                SELECT 
                    v.id_vehiculo, 
                    v.patente, 
                    v.seguro, 
                    v.vencimiento_seguro,
                    p.nombre,
                    p.apellido
                FROM Vehiculo v
                LEFT JOIN Persona_Vehiculo pv ON v.id_vehiculo = pv.id_vehiculo
                LEFT JOIN Persona p ON pv.id_persona = p.id_persona
                WHERE p.nombre = @NombreTitular AND p.apellido = @ApellidoTitular";
            try
            {
                using (SqlConnection CN = new SqlConnection(cn))
                {
                    using (SqlCommand cmd = new SqlCommand(query, CN))
                    {
                        cmd.Parameters.AddWithValue("@NombreTitular", titular.Split(' ')[0]);
                        cmd.Parameters.AddWithValue("@ApellidoTitular", titular.Split(' ')[1]);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(ds);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ds = null;
                Console.WriteLine("Error al obtener el vehículo: {0}", ex.Message);
            }
            return ds;
        }


        public static void AgregarVehiculo(BIZ.Modelo.Vehiculo vehiculo)
        {
            string CN = ConfigurationManager.ConnectionStrings["Grupo7"].ConnectionString;

            // 1. Inserción del vehículo
            string queryVehiculo = "INSERT INTO Vehiculo (patente, seguro, vencimiento_seguro) " +
                                   "VALUES (@patente, @seguro, @vencimiento); " +
                                   "SELECT SCOPE_IDENTITY();";

            // 2. Inserción de la relación en Persona_Vehiculo
            string queryRelacion = "INSERT INTO Persona_Vehiculo (id_persona, id_vehiculo) " +
                                   "VALUES (@id_persona, @id_vehiculo)";

            using (SqlConnection con = new SqlConnection(CN))
            {
                // Abrimos la conexión una sola vez al principio
                con.Open();

                int idVehiculoGenerado;

                // Paso 1: Guardar el vehículo y obtener su ID generado
                using (SqlCommand cmdVehiculo = new SqlCommand(queryVehiculo, con))
                {
                    cmdVehiculo.Parameters.AddWithValue("@patente", vehiculo.Patente);
                    cmdVehiculo.Parameters.AddWithValue("@seguro", vehiculo.Seguro);
                    cmdVehiculo.Parameters.AddWithValue("@vencimiento", vehiculo.VencimientoSeguro);

                    // ExecuteScalar ejecuta la consulta y devuelve el ID generado por SCOPE_IDENTITY()
                    idVehiculoGenerado = Convert.ToInt32(cmdVehiculo.ExecuteScalar());
                }

                // Paso 2: Guardar la relación en Persona_Vehiculo
                using (SqlCommand cmdRelacion = new SqlCommand(queryRelacion, con))
                {
                    cmdRelacion.Parameters.AddWithValue("@id_persona", vehiculo.IdPersona);
                    cmdRelacion.Parameters.AddWithValue("@id_vehiculo", idVehiculoGenerado);

                    cmdRelacion.ExecuteNonQuery();
                }
            }
        }
    }
}