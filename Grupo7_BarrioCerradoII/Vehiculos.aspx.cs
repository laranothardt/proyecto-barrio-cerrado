using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Grupo7_BarrioCerradoII
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Buscar.Visible = false;
                Agregar.Visible = false;
            }
        }

        protected void BtBuscar_Click(object sender, EventArgs e)
        {
            Buscar.Visible = true;
            Agregar.Visible = false;
            DataSet ds = BIZ.Data.Vehiculo.ObtenerVehiculo();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                RpVehiculos.DataSource = ds.Tables[0];
                RpVehiculos.DataBind();
                pnlMensaje.Visible = false;
            }
            else
            {
                RpVehiculos.DataSource = null;
                RpVehiculos.DataBind();

                pnlMensaje.Visible = true;
                litMensaje.Text = "No se encontraron vehículos registrados en la base de datos.";
            }
        }

        protected void BtRegistrar_Click(object sender, EventArgs e)
        {
            Agregar.Visible = true;
            Buscar.Visible = false;
        }

        protected void txPatente_TextChanged(object sender, EventArgs e)
        {
            var ds = BIZ.Data.Vehiculo.ObtenerVehiculoPatente(txPatente.Text);
            if (ds != null && ds.Tables.Count > 0)
            {
                RpVehiculos.DataSource = ds.Tables[0];
                RpVehiculos.DataBind();
            }
        }

        protected void txTitular_TextChanged(object sender, EventArgs e)
        {
            var ds = BIZ.Data.Vehiculo.ObtenerVehiculoTitular(txTitular.Text);
            if (ds != null && ds.Tables.Count > 0)
            {
                RpVehiculos.DataSource = ds.Tables[0];
                RpVehiculos.DataBind();
            }
        }

        protected void BtGuardar_Click(object sender, EventArgs e)
        {
            string nombre = IngresoNombreTitular.Text.Trim();
            string apellido = IngresoApellidoTitular.Text.Trim();

            int idPersona = BIZ.Data.Vehiculo.ObtenerIdPersona(nombre, apellido);

            if (idPersona == 0)
            {
                pnlMensaje.Visible = true;
                litMensaje.Text = "La persona ingresada no existe en el sistema. Registre la persona primero.";
                return;
            }

            BIZ.Modelo.Vehiculo nuevoVehiculo = new BIZ.Modelo.Vehiculo();
            nuevoVehiculo.Patente = IngresoPatente.Text.Trim();
            nuevoVehiculo.Seguro = IngresoSeguro.Text.Trim();
            nuevoVehiculo.VencimientoSeguro = DateTime.Parse(IngresoVencimiento.Text);
            nuevoVehiculo.IdPersona = idPersona;

            BIZ.Data.Vehiculo.AgregarVehiculo(nuevoVehiculo);

            pnlMensaje.Visible = true;
            litMensaje.Text = "Vehículo registrado correctamente.";
        }
    }
}