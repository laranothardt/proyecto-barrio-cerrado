using System;
using System.Globalization;
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

                string rol = Session["Rol"].ToString();

                bool esResidente = rol == "1" || rol == "Residente";
                bool esPropietario = rol == "2" || rol == "Propietario";

                if (!esResidente && !esPropietario)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

        }
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            pnlMensaje.Visible = false;
        }
        private void LimpiarFormulario()
        {
            txtDni.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtNombre.Text = string.Empty;
            ddlCategoria.SelectedIndex = 0;
            ddlLote.SelectedIndex = 0;
            txtResidenteAutoriza.Text = string.Empty;
            txtFechaDesde.Text = string.Empty;
            txtFechaHasta.Text = string.Empty;
            txtMotivo.Text = string.Empty;
        }
    }
}
