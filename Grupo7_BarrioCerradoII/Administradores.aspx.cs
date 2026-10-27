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

                // 1. Si es Administrador (Rol 3 o texto), pasa siempre
                if (rol == "3" || rol == "Administrador")
                {
                    return;
                }

                // 2. Si es Propietario (Rol 2), validamos su permiso dinámico
                if (rol == "2" || rol == "Propietario")
                {
                    // Si es null (porque se reinició el servidor), asumimos true (habilitado) hasta que se desmarque explícitamente
                    bool tienePermiso = Application["Permiso_Propietario_Administradores"] == null || (bool)Application["Permiso_Propietario_Administradores"];
                    if (!tienePermiso)
                    {
                        Response.Redirect("~/Default.aspx");
                    }
                    return;
                }

                // 3. Si es Residente (Rol 1), validamos su permiso dinámico
                if (rol == "1" || rol == "Residente")
                {
                    bool tienePermiso = Application["Permiso_Residente_Administradores"] == null || (bool)Application["Permiso_Residente_Administradores"];
                    if (!tienePermiso)
                    {
                        Response.Redirect("~/Default.aspx");
                    }
                    return;
                }

                // Si no coincide con ningún rol reconocido, rebota al inicio
                Response.Redirect("~/Default.aspx");
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

        protected async void btnConsultarIa_Click(object sender, EventArgs e)
        {
            string pregunta = txtPreguntaLibre.Text.Trim();
            if (string.IsNullOrEmpty(pregunta)) return;

            lblResultadoIa.Text = "Analizando la base de datos y redactando respuesta...";

            try
            {
                string sqlQuery = await GenerarSqlDesdePreguntaAsync(pregunta);

                sqlQuery = LimpiarSentenciaSql(sqlQuery);

                if (string.IsNullOrEmpty(sqlQuery) || !sqlQuery.ToUpper().StartsWith("SELECT"))
                {
                    lblResultadoIa.Text = "La IA no pudo interpretar la consulta para generar un comando SQL válido. Probá reformulando la pregunta.";
                    return;
                }

                DataTable resultadosDt = EjecutarConsultaSegura(sqlQuery);
                string datosJson = Newtonsoft.Json.JsonConvert.SerializeObject(resultadosDt);

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
                               "1. Persona (IDPersona, Nombre, Apellido, DNI, FechaAlta, HabilitadoReconocimiento, FK_IDCategoria, FK_Estado, Password)[cite: 14]\n" +
                               "2. Categoria Persona (IDCategoria, Descripcion) -> Descripciones: Residente, Propietario / Inquilino, Visita, Proveedor / Empleado, Proveedor[cite: 6]\n" +
                               "3. Movimiento (IDMovimiento, FechaHora, TipoMovimiento ('Ingreso' o 'Egreso'), Autorizado, Detalle, FK_IDPersona, FK_IDVehiculo, FK_IDLote, FK_IDPuntoAcceso)[cite: 16]\n" +
                               "4. Lote (IDLote, LoteNum, DireccionInterna, Latitud, Longitud, Observaciones)[cite: 15]\n" +
                               "5. Vehiculo (id_vehiculo, Patente, Seguro, vencimiento_seguro)\n" +
                               "6. PreAcreditacion (IDPreacreditacion, DNI, Nombre, Apellido, FK_IDCategoria, FK_IDLote, FK_IDResidenteAutoriza, Fecha_Desde, Fecha_Hasta, Motivo, Estado)[cite: 15]";

            string prompt = "Sos un experto programador de SQL Server.\n" +
                            esquemaBD + "\n\n" +
                            "Basate estrictamente en este esquema para responder la pregunta del usuario generando ÚNICAMENTE una consulta SQL de tipo SELECT. " +
                            "No agregues explicaciones, ni saludos, ni bloques markdown de texto adicional, solo el código SQL plano y directo.\n\n" +
                            "Pregunta del usuario: " + preguntaUsuario;

            return await ConsultarOllamaApiAsync(prompt);
        }

        private async Task<string> InterpretarDatosParaUsuarioAsync(string preguntaUsuario, string datosJson)
        {
            string prompt = "Sos el asistente virtual del barrio cerrado 'El Burro'.\n" +
                            "Un administrador te hizo la siguiente consulta: '" + preguntaUsuario + "'.\n\n" +
                            "Para responderle, el sistema ejecutó una consulta en la base de datos y obtuvo estos resultados en formato JSON:\n" +
                            datosJson + "\n\n" +
                            "REGLA ESTRICTA: Respondé única y exclusivamente con el dato solicitado de forma directa, breve y al pie. " +
                            "No saludes, no uses frases de cortesía como 'Espero que esta información...' ni te despidas con cierres formales. " +
                            "Solo la respuesta concreta y nada más.";

            return await ConsultarOllamaApiAsync(prompt);
        }

        private async Task<string> ConsultarOllamaApiAsync(string prompt)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(2);
                string url = "http://localhost:11434/api/generate";

                var payload = new
                {
                    model = "llama3",
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