using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BIZ.Modelo;

namespace BIZ.Data
{
    public class PreAcreditacion
    {
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Grupo7"].ConnectionString;
        }

        public bool CrearPreAcreditacion(BIZ.Modelo.PreAcreditacion preAcreditacion)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = @"INSERT INTO Preacreditacion 
                                (DNI, Nombre, Apellido, FK_IDCategoria, FK_IDLote, FK_IDResidenteAutoriza, Fecha_Desde, Fecha_Hasta, Motivo, Estado) 
                                VALUES 
                                (@DNI, @Nombre, @Apellido, @FK_IDCategoria, @FK_IDLote, @FK_IDResidenteAutoriza, @Fecha_Desde, @Fecha_Hasta, @Motivo, @Estado)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DNI", preAcreditacion.Dni);
                    cmd.Parameters.AddWithValue("@Nombre", preAcreditacion.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", preAcreditacion.Apellido);

                    if (preAcreditacion.IdCategoria > 0)
                        cmd.Parameters.AddWithValue("@FK_IDCategoria", preAcreditacion.IdCategoria);
                    else
                        cmd.Parameters.AddWithValue("@FK_IDCategoria", DBNull.Value);

                    cmd.Parameters.AddWithValue("@FK_IDLote", preAcreditacion.IdLoteDestino);
                    cmd.Parameters.AddWithValue("@FK_IDResidenteAutoriza", preAcreditacion.IdResidenteAutoriza);
                    cmd.Parameters.AddWithValue("@Fecha_Desde", preAcreditacion.FechaDesde);
                    cmd.Parameters.AddWithValue("@Fecha_Hasta", preAcreditacion.FechaHasta);
                    cmd.Parameters.AddWithValue("@Motivo", preAcreditacion.Motivo);
                    cmd.Parameters.AddWithValue("@Estado", string.IsNullOrEmpty(preAcreditacion.Estado) ? "Pendiente" : preAcreditacion.Estado);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        public List<BIZ.Modelo.PreAcreditacion> ObtenerPreAcreditaciones()
        {
            var lista = new List<BIZ.Modelo.PreAcreditacion>();

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = @"SELECT IDPreacreditacion, DNI, Nombre, Apellido, FK_IDCategoria, FK_IDLote, 
                                        FK_IDResidenteAutoriza, Fecha_Desde, Fecha_Hasta, Motivo, Estado 
                                 FROM Preacreditacion 
                                 ORDER BY Fecha_Desde DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new BIZ.Modelo.PreAcreditacion
                            {
                                IdPreacreditacion = Convert.ToInt32(reader["IDPreacreditacion"]),
                                Dni = reader["DNI"].ToString(),
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                IdCategoria = reader["FK_IDCategoria"] != DBNull.Value ? Convert.ToInt32(reader["FK_IDCategoria"]) : 0,
                                IdLoteDestino = Convert.ToInt32(reader["FK_IDLote"]),
                                IdResidenteAutoriza = Convert.ToInt32(reader["FK_IDResidenteAutoriza"]),
                                FechaDesde = Convert.ToDateTime(reader["Fecha_Desde"]),
                                FechaHasta = Convert.ToDateTime(reader["Fecha_Hasta"]),
                                Motivo = reader["Motivo"].ToString(),
                                Estado = reader["Estado"].ToString()
                            };
                            lista.Add(item);
                        }
                    }
                }
            }
            return lista;
        }
    }
}