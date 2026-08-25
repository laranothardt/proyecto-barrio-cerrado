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
                    cmd.Parameters.AddWithValue("@Estado", string.IsNullOrEmpty(preAcreditacion.Estado) ? "Aceptada" : preAcreditacion.Estado);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        public List<BIZ.Modelo.PreAcreditacion> ObtenerVigentesPorResidente(int idResidente)
        {
            var lista = new List<BIZ.Modelo.PreAcreditacion>();

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                // Agregamos LEFT JOIN CategoriaPersona cat ON p.FK_IDCategoria = cat.IDCategoria
                string query = @"SELECT 
                            p.IDPreacreditacion, p.DNI, p.Nombre, p.Apellido, 
                            p.FK_IDCategoria, p.FK_IDLote, p.FK_IDResidenteAutoriza, 
                            p.Fecha_Desde, p.Fecha_Hasta, p.Motivo, p.Estado,
                            l.LoteNum AS NumeroLote,
                            ISNULL(cat.Descripcion, 'Sin categoría') AS NombreCategoria,
                            ISNULL(res.Nombre + ' ' + res.Apellido, 'Residente') AS NombreResidenteAutoriza
                         FROM Preacreditacion p
                         LEFT JOIN Lote l ON p.FK_IDLote = l.IDLote
                         LEFT JOIN CategoriaPersona cat ON p.FK_IDCategoria = cat.IDCategoria
                         LEFT JOIN Persona res ON p.FK_IDResidenteAutoriza = res.IDPersona
                         WHERE p.FK_IDResidenteAutoriza = @IdResidente
                           AND CAST(p.Fecha_Hasta AS DATE) >= CAST(GETDATE() AS DATE)
                         ORDER BY p.Fecha_Desde DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdResidente", idResidente);
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
                                NombreCategoria = reader["NombreCategoria"].ToString(),
                                IdLoteDestino = Convert.ToInt32(reader["FK_IDLote"]),
                                NumeroLote = reader["NumeroLote"] != DBNull.Value ? reader["NumeroLote"].ToString() : reader["FK_IDLote"].ToString(),
                                IdResidenteAutoriza = Convert.ToInt32(reader["FK_IDResidenteAutoriza"]),
                                NombreResidenteAutoriza = reader["NombreResidenteAutoriza"].ToString(),
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

        public List<BIZ.Modelo.PreAcreditacion> ObtenerTodasVigentes()
        {
            var lista = new List<BIZ.Modelo.PreAcreditacion>();

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = @"SELECT 
                            p.IDPreacreditacion, p.DNI, p.Nombre, p.Apellido, 
                            p.FK_IDCategoria, p.FK_IDLote, p.FK_IDResidenteAutoriza, 
                            p.Fecha_Desde, p.Fecha_Hasta, p.Motivo, p.Estado,
                            l.LoteNum AS NumeroLote,
                            ISNULL(cat.Descripcion, 'Sin categoría') AS NombreCategoria,
                            ISNULL(res.Nombre + ' ' + res.Apellido, 'Residente') AS NombreResidenteAutoriza
                         FROM Preacreditacion p
                         LEFT JOIN Lote l ON p.FK_IDLote = l.IDLote
                         LEFT JOIN CategoriaPersona cat ON p.FK_IDCategoria = cat.IDCategoria
                         LEFT JOIN Persona res ON p.FK_IDResidenteAutoriza = res.IDPersona
                         WHERE CAST(p.Fecha_Hasta AS DATE) >= CAST(GETDATE() AS DATE)
                         ORDER BY p.Fecha_Desde DESC";

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
                                NombreCategoria = reader["NombreCategoria"].ToString(),
                                IdLoteDestino = Convert.ToInt32(reader["FK_IDLote"]),
                                NumeroLote = reader["NumeroLote"] != DBNull.Value ? reader["NumeroLote"].ToString() : reader["FK_IDLote"].ToString(),
                                IdResidenteAutoriza = Convert.ToInt32(reader["FK_IDResidenteAutoriza"]),
                                NombreResidenteAutoriza = reader["NombreResidenteAutoriza"].ToString(),
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