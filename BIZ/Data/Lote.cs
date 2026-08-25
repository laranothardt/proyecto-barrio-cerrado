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
    public class Lote
    {
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Grupo7"].ConnectionString;
        }
        public int ObtenerIdLotePorNumero(string loteNum)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string loteFormateado = loteNum.PadLeft(2, '0');

                string query = "SELECT IDLote FROM Lote WHERE LoteNum = @LoteNum OR LoteNum = @LoteOriginal";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@LoteNum", loteFormateado);
                    cmd.Parameters.AddWithValue("@LoteOriginal", loteNum);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }
    }
}
    