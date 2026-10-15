using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Grupo7_BarrioCerradoII
{
    public partial class Restricciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Cargar valores desde la Base de Datos o Variables de Sesión al iniciar
                CargarConfiguracionesExistentes();
            }
        }

        private void CargarConfiguracionesExistentes()
        {
            // Ejemplo: Acá se cargarían los horarios guardados en la BBDD.
            // txtProveedorDesde.Text = "07:00";
            // txtProveedorHasta.Text = "18:00";
        }

        protected void btnGuardarConfiguracion_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Capturar Horarios
                TimeSpan horaProveedorDesde = TimeSpan.Parse(txtProveedorDesde.Text);
                TimeSpan horaProveedorHasta = TimeSpan.Parse(txtProveedorHasta.Text);

                TimeSpan horaVisitaDesde = TimeSpan.Parse(txtVisitaDesde.Text);
                TimeSpan horaVisitaHasta = TimeSpan.Parse(txtVisitaHasta.Text);

                // 2. Capturar Permisos de Páginas por Rol (Guardia / Residente)
                bool guardiaAutorizar = chkAutGuardia.Checked;
                bool guardiaIngresoEgreso = chkIngEgGuardia.Checked;
                bool guardiaMovimientos = chkMovGuardia.Checked;
                bool guardiaVehiculos = chkVehGuardia.Checked;

                bool residenteAutorizar = chkAutResidente.Checked;
                bool residenteVehiculos = chkVehResidente.Checked;

                // 3. Persistir en Base de Datos / Capa BIZ
                // new BIZ.Data.Configuracion().GuardarRestricciones(...);

                MostrarMensaje("La configuración de páginas y horarios se guardó correctamente.", true);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar la configuración: " + ex.Message, false);
            }
        }
    }
}