using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using BIZ.Modelo;

namespace BIZ.Data
{
    public class UsuarioSistema
    {
        private static string GetConnectionString()
        {
            var connSetting = System.Configuration.ConfigurationManager.ConnectionStrings["MyDB"];
            if (connSetting != null && !string.IsNullOrEmpty(connSetting.ConnectionString))
            {
                return connSetting.ConnectionString;
            }
            return @"Data Source=localhost\SQLEXPRESS;Initial Catalog=BarrioCerrado;Integrated Security=True";
        }

        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return string.Empty;
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public BIZ.Modelo.UsuarioSistema ObtenerUsuarioPorEmail(string email)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = "SELECT IdUsuario, Username, PasswordHash, NombreCompleto, Rol, Dni, Foto FROM UsuarioSistema WHERE Username = @Username";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", email);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new BIZ.Modelo.UsuarioSistema
                            {
                                IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                                Username = reader["Username"].ToString(),
                                PasswordHash = reader["PasswordHash"].ToString(),
                                NombreCompleto = reader["NombreCompleto"].ToString(),
                                Rol = reader["Rol"].ToString(),
                                Dni = reader["Dni"] != DBNull.Value ? reader["Dni"].ToString() : null,
                                Foto = reader["Foto"] != DBNull.Value ? (byte[])reader["Foto"] : null
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool CrearUsuario(BIZ.Modelo.UsuarioSistema usuario)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = "INSERT INTO UsuarioSistema (Username, PasswordHash, NombreCompleto, Rol, Dni, Foto) VALUES (@Username, @PasswordHash, @NombreCompleto, @Rol, @Dni, @Foto)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", usuario.Username);
                    cmd.Parameters.AddWithValue("@PasswordHash", usuario.PasswordHash);
                    cmd.Parameters.AddWithValue("@NombreCompleto", usuario.NombreCompleto);
                    cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
                    cmd.Parameters.AddWithValue("@Dni", (object)usuario.Dni ?? DBNull.Value);

                    SqlParameter paramFoto = new SqlParameter("@Foto", SqlDbType.VarBinary);
                    paramFoto.Value = (object)usuario.Foto ?? DBNull.Value;
                    cmd.Parameters.Add(paramFoto);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        public bool ActualizarPassword(string email, string passwordHash)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = "UPDATE UsuarioSistema SET PasswordHash = @PasswordHash WHERE Username = @Username";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", email);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }
    }
}
