using System;
using System.Collections.Generic;
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
            RpVehiculos.DataSource = BIZ.Data.Vehiculo.ObtenerVehiculo();
            RpVehiculos.DataBind();

        }

        protected void BtRegistrar_Click(object sender, EventArgs e)
        {
            Agregar.Visible = true;
            Buscar.Visible = false;
        }

        protected void txPatente_TextChanged(object sender, EventArgs e)
        {
            RpVehiculos.DataSource = BIZ.Data.Vehiculo.ObtenerVehiculoPatente(txPatente.Text);
            RpVehiculos.DataBind();
        }

        protected void txTitular_TextChanged(object sender, EventArgs e)
        {
            RpVehiculos.DataSource = BIZ.Data.Vehiculo.ObtenerVehiculoTitular(txTitular.Text);
            RpVehiculos.DataBind();
        }

        protected void BtGuardar_Click(object sender, EventArgs e)
        {
            BIZ.Modelo.Vehiculo nuevoVehiculo = new BIZ.Modelo.Vehiculo();

            // 2. Mapeamos los datos (convirtiendo los tipos que correspondan)
            nuevoVehiculo.Patente = IngresoPatente.Text;
            nuevoVehiculo.Seguro = IngresoSeguro.Text;
            nuevoVehiculo.VencimientoSeguro = DateTime.Parse(IngresoVencimiento.Text);
            nuevoVehiculo.NombreTitular = IngresoNombreTitular.Text;
            nuevoVehiculo.ApellidoTitular = IngresoApellidoTitular.Text;

            // 3. Llamamos al método que inserta en la base de datos
            BIZ.Data.Vehiculo.AgregarVehiculo(nuevoVehiculo);
                    
        }
    }
}