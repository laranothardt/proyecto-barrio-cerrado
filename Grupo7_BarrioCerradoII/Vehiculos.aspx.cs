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


        }

        protected void BtRegistrar_Click(object sender, EventArgs e)
        {
            Agregar.Visible = true;
            Buscar.Visible = false;
        }

        protected void txPatente_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txTitular_TextChanged(object sender, EventArgs e)
        {

        }

        protected void BtGuardar_Click(object sender, EventArgs e)
        {

        }
    }
}