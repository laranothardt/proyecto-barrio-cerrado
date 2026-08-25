using System;
using System.Web.UI;
using BIZ.Modelo;

namespace Grupo7_BarrioCerradoII
{
    public partial class Autorizar : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Rol"] == null)
                {
                    Response.Redirect("~/IniciarSesion.aspx");
                    return;
                }

                // Si en Session guardas el DNI del usuario logueado, lo precargamos
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
                BIZ.Data.Persona dataPersona = new BIZ.Data.Persona();
                int idResidente = dataPersona.ObtenerOCrearPersonaResidente(txtResidenteAutoriza.Text.Trim(), "Residente " + txtResidenteAutoriza.Text.Trim());

                if (idResidente == 0)
                {
                    MostrarMensaje("El DNI del residente que autoriza no se encuentra registrado en el sistema.", false);
                    return;
                }

                BIZ.Data.Lote dataLote = new BIZ.Data.Lote();
                int idLote = dataLote.ObtenerIdLotePorNumero(txtLote.Text.Trim());

                if (idLote == 0)
                {
                    MostrarMensaje("El lote ingresado no existe en la base de datos.", false);
                    return;
                }

                var nuevaPreAcreditacion = new BIZ.Modelo.PreAcreditacion
                {
                    Dni = txtDni.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Nombre = txtNombre.Text.Trim(),
                    IdCategoria = string.IsNullOrEmpty(ddlCategoria.SelectedValue) ? -1 : int.Parse(ddlCategoria.SelectedValue),
                    IdLoteDestino = idLote,
                    IdResidenteAutoriza = idResidente,
                    FechaDesde = DateTime.Parse(txtFechaDesde.Text),
                    FechaHasta = DateTime.Parse(txtFechaHasta.Text),
                    Motivo = txtMotivo.Text.Trim(),
                    Estado = "Aceptada"
                };

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
                // 1. Intentamos obtener el DNI de la Session o del TextBox
                string dniResidente = Session["Dni"] != null ? Session["Dni"].ToString() : txtResidenteAutoriza.Text.Trim();

                BIZ.Data.PreAcreditacion data = new BIZ.Data.PreAcreditacion();

                if (!string.IsNullOrEmpty(dniResidente))
                {
                    BIZ.Data.Persona dataPersona = new BIZ.Data.Persona();
                    int idResidente = dataPersona.ObtenerOCrearPersonaResidente(dniResidente, "Residente " + dniResidente);

                    // Carga solo las del residente logueado
                    gvPreacreditaciones.DataSource = data.ObtenerVigentesPorResidente(idResidente);
                }
                else
                {
                    // Si no hay DNI en sesión aún, obtiene todas las vigentes para no dejar la grilla vacía
                    gvPreacreditaciones.DataSource = data.ObtenerTodasVigentes();
                }

                gvPreacreditaciones.DataBind();
            }
            catch
            {
            }
        }

        private void LimpiarFormulario()
        {
            txtDni.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtNombre.Text = string.Empty;
            ddlCategoria.SelectedIndex = 0;
            txtLote.Text = string.Empty;

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