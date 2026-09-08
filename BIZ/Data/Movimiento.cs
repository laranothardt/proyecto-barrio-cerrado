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
    public class Movimiento
    {
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Grupo7"].ConnectionString;
        }
        public bool CrearMovimiento(BIZ.Modelo.Movimiento movimiento)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = "INSERT INTO Movimiento (FechaHora, TipoMovimiento, Autorizado, Detalle, FK_IDPersona, FK_IDVehiculo, FK_IDLote, FK_IDPuntoAcceso) VALUES (@FechaHora, @TipoMovimiento, @Autorizado, @Detalle, @FK_IDPersona, @FK_IDVehiculo, @FK_IDLote, @FK_IDPuntoAcceso)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FechaHora", movimiento.FechaHora);
                    cmd.Parameters.AddWithValue("@TipoMovimiento", movimiento.Tipo);
                    cmd.Parameters.AddWithValue("@Autorizado", movimiento.Resultado);
                    cmd.Parameters.AddWithValue("@Detalle", movimiento.Detalle);
                    cmd.Parameters.AddWithValue("@FK_IDPersona", movimiento.IdPersona);
                    cmd.Parameters.AddWithValue("@FK_IDVehiculo", movimiento.IdVehiculo);
                    cmd.Parameters.AddWithValue("@FK_IDLote", movimiento.IdLoteDestino);
                    cmd.Parameters.AddWithValue("@FK_IDPuntoAcceso", movimiento.IdAcceso);

                    try
                    {
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                    catch (SqlException ex)
                    {
                        System.Diagnostics.Trace.TraceError("Error SQL al crear Movimiento: {0}", ex);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError("Error inesperado al crear Movimiento: {0}", ex);
                        return false;
                    }
                }
            }
        }

    }
}
