using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BIZ.Modelo;

namespace Grupo7_BarrioCerradoII
{
    public partial class Autorizar : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string rol = Session["Rol"]?.ToString();

            if (rol == "Propietario")
            {
                bool tienePermiso = Application["Permiso_Propietario_Autorizar"] != null && (bool)Application["Permiso_Propietario_Autorizar"];
                if (!tienePermiso)
                {
                    Response.Redirect("~/Default.aspx"); // Rebota al inicio
                }
            }

            if (!IsPostBack)
            {
                ddlLote.Items.Add(new ListItem("-- Seleccione un lote --", ""));

                for (int i = 1; i <= 50; i++)
                {
                    ddlLote.Items.Add(new ListItem($"Lote {i}", i.ToString()));
                }

                if (Session["Rol"] == null)
                {
                    Response.Redirect("~/IniciarSesion.aspx");
                    return;
                }

                if (Session["Dni"] != null)
                {
                    txtResidenteAutoriza.Text = Session["Dni"].ToString();
                }

                CargarPreAcreditaciones();
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                // 1. Validar Residente
                int idResidente = ObtenerIdResidente(txtResidenteAutoriza.Text.Trim());

                if (idResidente == 0)
                {
                    MostrarMensaje("El DNI del residente que autoriza no se encuentra registrado en el sistema.", false);
                    return;
                }

                // 2. Validar Lote
                BIZ.Data.Lote dataLote = new BIZ.Data.Lote();
                int idLote = dataLote.ObtenerIdLotePorNumero(ddlLote.SelectedValue);

                if (idLote == 0)
                {
                    MostrarMensaje("El lote ingresado no existe en la base de datos.", false);
                    return;
                }

                // 3. Parsear Fechas
                DateTime fechaDesde = DateTime.Parse(txtFechaDesde.Text);
                DateTime fechaHasta = DateTime.Parse(txtFechaHasta.Text);

                // 4. Validar Restricción de Fin de Semana para Proveedores / Empleados (Categoría 4)
                if (ddlCategoria.SelectedValue == "4")
                {
                    if (fechaDesde.DayOfWeek == DayOfWeek.Saturday || fechaDesde.DayOfWeek == DayOfWeek.Sunday ||
                        fechaHasta.DayOfWeek == DayOfWeek.Saturday || fechaHasta.DayOfWeek == DayOfWeek.Sunday)
                    {
                        MostrarMensaje("Los proveedores y empleados no pueden ingresar los días sábados o domingos.", false);
                        return;
                    }
                }

                // 5. Crear objeto PreAcreditación
                var nuevaPreAcreditacion = new BIZ.Modelo.PreAcreditacion
                {
                    Dni = txtDni.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Nombre = txtNombre.Text.Trim(),
                    IdCategoria = string.IsNullOrEmpty(ddlCategoria.SelectedValue) ? -1 : int.Parse(ddlCategoria.SelectedValue),
                    IdLoteDestino = idLote,
                    IdResidenteAutoriza = idResidente,
                    FechaDesde = fechaDesde,
                    FechaHasta = fechaHasta,
                    Motivo = txtMotivo.Text.Trim(),
                    Estado = "Aceptada"
                };

                // 6. Guardar en Base de Datos
                BIZ.Data.PreAcreditacion data = new BIZ.Data.PreAcreditacion();
                bool creado = data.CrearPreAcreditacion(nuevaPreAcreditacion);

                if (creado)
                {
                    MostrarMensaje("Preacreditación guardada correctamente.", true);
                    LimpiarFormulario();
                    CargarPreAcreditaciones();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error al guardar: " + ex.Message, false);
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            pnlMensaje.Visible = false;
        }

        private void CargarPreAcreditaciones()
        {
            try
            {
                string dniResidente = Session["Dni"] != null ? Session["Dni"].ToString() : txtResidenteAutoriza.Text.Trim();

                BIZ.Data.PreAcreditacion data = new BIZ.Data.PreAcreditacion();

                if (!string.IsNullOrEmpty(dniResidente))
                {
                    // Solo lectura: cargar la lista no debe crear personas
                    BIZ.Data.Persona dataPersona = new BIZ.Data.Persona();
                    int idResidente = dataPersona.ObtenerIdPersonaPorDni(dniResidente);

                    gvPreacreditaciones.DataSource = data.ObtenerVigentesPorResidente(idResidente);
                }
                else
                {
                    gvPreacreditaciones.DataSource = data.ObtenerTodasVigentes();
                }

                gvPreacreditaciones.DataBind();
            }
            catch
            {
            }
        }

        // Devuelve el IdPersona del residente que autoriza, o 0 si no esta registrado.
        // No inventa personas: solo crea/corrige la Persona del usuario logueado con los datos de su sesion.
        private int ObtenerIdResidente(string dni)
        {
            BIZ.Data.Persona dataPersona = new BIZ.Data.Persona();

            bool esUsuarioLogueado = Session["Dni"] != null && Session["Dni"].ToString() == dni;
            string nombreCompleto = Session["NombreCompleto"] as string;

            if (esUsuarioLogueado && !string.IsNullOrWhiteSpace(nombreCompleto))
            {
                string[] partes = nombreCompleto.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (partes.Length > 1)
                {
                    // Aproximacion para cuentas viejas: primera palabra = nombre, resto = apellido
                    string nombre = partes[0];
                    string apellido = string.Join(" ", partes, 1, partes.Length - 1);
                    return dataPersona.ObtenerOCrearPersona(dni, nombre, apellido, 2);
                }
            }

            return dataPersona.ObtenerIdPersonaPorDni(dni);
        }

        private void LimpiarFormulario()
        {
            txtDni.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtNombre.Text = string.Empty;
            ddlCategoria.SelectedIndex = 0;
            ddlLote.SelectedIndex = 0;

            if (Session["Dni"] != null)
                txtResidenteAutoriza.Text = Session["Dni"].ToString();
            else
                txtResidenteAutoriza.Text = string.Empty;

            txtFechaDesde.Text = string.Empty;
            txtFechaHasta.Text = string.Empty;
            txtMotivo.Text = string.Empty;
        }

        private void MostrarMensaje(string texto, bool esExito)
        {
            pnlMensaje.Visible = true;
            litMensaje.Text = texto;
            pnlMensaje.CssClass = esExito ? "alert alert-success" : "alert alert-danger";
        }
    }
}