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
                    v.Patente, 
                    v.Seguro, 
                    v.vencimiento_seguro,
                    ISNULL(p.Nombre, 'Sin Titular') AS Nombre,
                    ISNULL(p.Apellido, '') AS Apellido
                FROM dbo.Vehiculo v
                LEFT JOIN dbo.Persona_Vehiculo pv ON v.id_vehiculo = pv.id_vehiculo
                LEFT JOIN dbo.Persona p ON pv.IDPersona = p.IDPersona";

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
            return ds;
        }

        public static DataSet ObtenerVehiculoPatente(string patente)
        {
            DataSet ds = new DataSet();
            string cn = ConfigurationManager.ConnectionStrings["Grupo7"].ConnectionString;

            string query = @"
                SELECT 
                    v.id_vehiculo, 
                    v.Patente, 
                    v.Seguro, 
                    v.vencimiento_seguro,
                    ISNULL(p.Nombre, 'Sin Titular') AS Nombre,
                    ISNULL(p.Apellido, '') AS Apellido
                FROM dbo.Vehiculo v
                LEFT JOIN dbo.Persona_Vehiculo pv ON v.id_vehiculo = pv.id_vehiculo
                LEFT JOIN dbo.Persona p ON pv.IDPersona = p.IDPersona
                WHERE v.Patente LIKE @Patente + '%'";

            using (SqlConnection CN = new SqlConnection(cn))
            {
                using (SqlCommand cmd = new SqlCommand(query, CN))
                {
                    cmd.Parameters.AddWithValue("@Patente", patente.Trim());
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                }
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
                    v.Patente, 
                    v.Seguro, 
                    v.vencimiento_seguro,
                    ISNULL(p.Nombre, 'Sin Titular') AS Nombre,
                    ISNULL(p.Apellido, '') AS Apellido
                FROM dbo.Vehiculo v
                LEFT JOIN dbo.Persona_Vehiculo pv ON v.id_vehiculo = pv.id_vehiculo
                LEFT JOIN dbo.Persona p ON pv.IDPersona = p.IDPersona
                WHERE p.Nombre LIKE '%' + @Titular + '%' OR p.Apellido LIKE '%' + @Titular + '%'";

            using (SqlConnection CN = new SqlConnection(cn))
            {
                using (SqlCommand cmd = new SqlCommand(query, CN))
                {
                    cmd.Parameters.AddWithValue("@Titular", titular.Trim());
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                }
            }
            return ds;
        }


        public static void AgregarVehiculo(BIZ.Modelo.Vehiculo vehiculo)
        {
            string CN = ConfigurationManager.ConnectionStrings["Grupo7"].ConnectionString;

            string queryVehiculo = "INSERT INTO Vehiculo (patente, seguro, vencimiento_seguro) " +
                                   "VALUES (@patente, @seguro, @vencimiento); " +
                                   "SELECT SCOPE_IDENTITY();";

            string queryRelacion = "INSERT INTO Persona_Vehiculo (id_persona, id_vehiculo) " +
                                   "VALUES (@id_persona, @id_vehiculo)";

            using (SqlConnection con = new SqlConnection(CN))
            {
                con.Open();

                int idVehiculoGenerado;

                using (SqlCommand cmdVehiculo = new SqlCommand(queryVehiculo, con))
                {
                    cmdVehiculo.Parameters.AddWithValue("@patente", vehiculo.Patente);
                    cmdVehiculo.Parameters.AddWithValue("@seguro", vehiculo.Seguro);
                    cmdVehiculo.Parameters.AddWithValue("@vencimiento", vehiculo.VencimientoSeguro);

                    idVehiculoGenerado = Convert.ToInt32(cmdVehiculo.ExecuteScalar());
                }

                using (SqlCommand cmdRelacion = new SqlCommand(queryRelacion, con))
                {
                    cmdRelacion.Parameters.AddWithValue("@id_persona", vehiculo.IdPersona);
                    cmdRelacion.Parameters.AddWithValue("@id_vehiculo", idVehiculoGenerado);

                    cmdRelacion.ExecuteNonQuery();
                }
            }
        }
        public static int ObtenerIdPersona(string nombre, string apellido)
        {
            int idPersona = 0;
            string cn = ConfigurationManager.ConnectionStrings["Grupo7"].ConnectionString;
            string query = "SELECT IDPersona FROM dbo.Persona WHERE Nombre = @Nombre AND Apellido = @Apellido";

            using (SqlConnection con = new SqlConnection(cn))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Apellido", apellido);
                    con.Open();

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        idPersona = Convert.ToInt32(result);
                    }
                }
            }
            return idPersona;
        }
    }
}