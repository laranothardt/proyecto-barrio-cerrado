using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Grupo7_BarrioCerradoII
{
    public partial class Administradores : System.Web.UI.Page
    {
        // Cadena de conexión obtenida de tu configuración
        private string connString = ConfigurationManager.ConnectionStrings[1].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Rol"] == null)
                {
                    Response.Redirect("~/IniciarSesion.aspx");
                    return;
                }

                string rol = Session["Rol"].ToString();

                if (rol != "3" && rol != "Administrador")
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }
            }
        }

        protected void Bt_Reportes_Click(object sender, EventArgs e)
        {
            Response.Redirect("Reportes.aspx");
        }

        protected void Bt_Movimientos_Click(object sender, EventArgs e)
        {
            Response.Redirect("Movimientos.aspx");
        }

        protected void Bt_Vehiculos_Click(object sender, EventArgs e)
        {
            Response.Redirect("Vehiculos.aspx");
        }

        protected void Bt_Restricciones_Click(object sender, EventArgs e)
        {
            Response.Redirect("Restricciones.aspx");
        }

        // ==========================================
        // INTEGRACIÓN DE ASISTENTE IA (OLLAMA + BD)
        // ==========================================

        protected async void btnConsultarIa_Click(object sender, EventArgs e)
        {
            string pregunta = txtPreguntaLibre.Text.Trim();
            if (string.IsNullOrEmpty(pregunta)) return;

            lblResultadoIa.Text = "Analizando la base de datos y redactando respuesta...";

            try
            {
                // PASO 1: Pedirle a Ollama que convierta la pregunta en una sentencia SQL basada en tu esquema
                string sqlQuery = await GenerarSqlDesdePreguntaAsync(pregunta);

                // Limpieza de seguridad por si la IA devuelve bloques markdown tipo ```sql ... ```
                sqlQuery = LimpiarSentenciaSql(sqlQuery);

                if (string.IsNullOrEmpty(sqlQuery) || !sqlQuery.ToUpper().StartsWith("SELECT"))
                {
                    lblResultadoIa.Text = "La IA no pudo interpretar la consulta para generar un comando SQL válido. Probá reformulando la pregunta.";
                    return;
                }

                // PASO 2: Ejecutar la consulta SQL de manera segura en la Base de Datos
                DataTable resultadosDt = EjecutarConsultaSegura(sqlQuery);
                string datosJson = Newtonsoft.Json.JsonConvert.SerializeObject(resultadosDt);

                // PASO 3: Pedirle a Ollama que explique los resultados al usuario en lenguaje natural
                string respuestaFinal = await InterpretarDatosParaUsuarioAsync(pregunta, datosJson);

                lblResultadoIa.Text = respuestaFinal;
            }
            catch (Exception ex)
            {
                lblResultadoIa.Text = "Error al procesar la consulta inteligente: " + ex.Message;
            }
        }

        private async Task<string> GenerarSqlDesdePreguntaAsync(string preguntaUsuario)
        {
            string esquemaBD = "Tablas de la base de datos SQL Server:\n" +
                               "1. Persona (IDPersona, Nombre, Apellido, DNI, FK_IDCategoria)\n" +
                               "2. CategoriaPersona (IDCategoria, Descripcion) -> Descripciones: Residente, Propietario / Inquilino, Visita, Proveedor\n" +
                               "3. Movimiento (IDMovimiento, FechaHora, Tipo ('Ingreso' o 'Egreso'), IdPersona, IdVehiculo, IdLoteDestino, IdAcceso, Autorizado)\n" +
                               "4. Lote (IDLote, Numero)\n" +
                               "5. Vehiculo (IDVehiculo, Patente)";

            string prompt = "Sos un experto programador de SQL Server.\n" +
                            esquemaBD + "\n\n" +
                            "Basate estrictamente en este esquema para responder la pregunta del usuario generando ÚNICAMENTE una consulta SQL de tipo SELECT. " +
                            "No agregues explicaciones, ni saludos, ni bloques markdown de texto adicional, solo el código SQL plano y directo.\n\n" +
                            "Pregunta del usuario: " + preguntaUsuario;

            return await ConsultarOllamaApiAsync(prompt);
        }

        private async Task<string> InterpretarDatosParaUsuarioAsync(string preguntaUsuario, string datosJson)
        {
            string prompt = "Sos el asistente virtual inteligente del barrio cerrado 'El Burro'.\n" +
                            "Un administrador te hizo la siguiente consulta: '" + preguntaUsuario + "'.\n\n" +
                            "Para responderle, el sistema ejecutó una consulta en la base de datos y obtuvo estos resultados en formato JSON:\n" +
                            datosJson + "\n\n" +
                            "Redactá una respuesta amable, profesional, clara y directa en castellano basada puramente en estos datos. Si el resultado está vacío, aclarale amablemente que no se encontraron registros.";

            return await ConsultarOllamaApiAsync(prompt);
        }

        private async Task<string> ConsultarOllamaApiAsync(string prompt)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                string url = "http://localhost:11434/api/generate";

                var payload = new
                {
                    model = "llama3", // O el modelo que tengas instalado (ej: phi3, mistral)
                    prompt = prompt,
                    stream = false
                };

                string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var resultadoObj = Newtonsoft.Json.JsonConvert.DeserializeObject<OllamaResponse>(jsonResponse);
                    return resultadoObj?.response?.Trim() ?? "";
                }
                throw new Exception("No se pudo establecer comunicación con el servidor local de Ollama.");
            }
        }

        private DataTable EjecutarConsultaSegura(string query)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connString))
            {
                string queryUpper = query.Trim().ToUpper();
                if (!queryUpper.StartsWith("SELECT"))
                {
                    throw new Exception("Por motivos de seguridad, solo se permiten consultas de lectura.");
                }

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        con.Open();
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        private string LimpiarSentenciaSql(string sql)
        {
            sql = sql.Replace("```sql", "").Replace("```", "").Trim();
            int indexSelect = sql.ToUpper().IndexOf("SELECT");
            if (indexSelect > 0)
            {
                sql = sql.Substring(indexSelect);
            }
            return sql;
        }

        private class OllamaResponse
        {
            public string response { get; set; }
        }
    }
}