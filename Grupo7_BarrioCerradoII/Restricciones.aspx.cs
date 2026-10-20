using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Grupo7_BarrioCerradoII
{
    public partial class Restricciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Solo Administradores pueden ver esta página (Validación de seguridad)
                string rol = Session["Rol"]?.ToString();
                if (rol != "3" && rol != "Administrador")
                {
                    Response.Redirect("~/Default.aspx");
                }
            }
        }

        protected void btnGuardarConfiguracion_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. GUARDAR HORARIOS
                Application["Prov_Desde"] = TimeSpan.Parse(txtProveedorDesde.Text);
                Application["Prov_Hasta"] = TimeSpan.Parse(txtProveedorHasta.Text);
                Application["Prov_Habilitado"] = chkProveedorHabilitado.Checked;
                Application["Prov_Dias"] = ObtenerDiasSeleccionados(cblDiasProveedor);

                Application["Visita_Desde"] = TimeSpan.Parse(txtVisitaDesde.Text);
                Application["Visita_Hasta"] = TimeSpan.Parse(txtVisitaHasta.Text);
                Application["Visita_Habilitado"] = chkVisitaHabilitado.Checked;
                Application["Visita_Dias"] = ObtenerDiasSeleccionados(cblDiasVisita);

                // 2. GUARDAR PERMISOS DE PÁGINAS POR ROL (Propietario = ej. "2", Residente = ej. "1")
                // Permisos Propietario
                Application["Permiso_Propietario_Administradores"] = chkAdmProp.Checked;
                Application["Permiso_Propietario_Autorizar"] = chkAutProp.Checked;
                Application["Permiso_Propietario_IngresoEgreso"] = chkIngProp.Checked;
                Application["Permiso_Propietario_Movimientos"] = chkMovProp.Checked;

                // Permisos Residente
                Application["Permiso_Residente_Administradores"] = chkAdmRes.Checked;
                Application["Permiso_Residente_Autorizar"] = chkAutRes.Checked;
                Application["Permiso_Residente_IngresoEgreso"] = chkIngRes.Checked;
                Application["Permiso_Residente_Movimientos"] = chkMovRes.Checked;

                MostrarMensaje("Configuración actualizada correctamente y aplicada en tiempo real.", true);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar: " + ex.Message, false);
            }
        }

        private List<string> ObtenerDiasSeleccionados(CheckBoxList cbl)
        {
            List<string> dias = new List<string>();
            foreach (ListItem item in cbl.Items)
            {
                if (item.Selected) dias.Add(item.Value); // Ej: "Monday", "Tuesday"
            }
            return dias;
        }

        private void MostrarMensaje(string texto, bool exito)
        {
            lblMensaje.CssClass = "fw-bold mb-3 d-block " + (exito ? "text-success" : "text-danger");
            lblMensaje.Text = texto;
        }
    }
}