using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Grupo7_BarrioCerradoII
{
    public partial class Movimientos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ddlLote.Items.Add(new ListItem("-- Seleccione un lote --", ""));

            for (int i = 1; i <= 50; i++)
            {
                ddlLote.Items.Add(new ListItem($"Lote {i}", i.ToString()));
            }
        }


        protected void Bt_BuscarDNI_Click(object sender, EventArgs e)
        {

        }

        protected void Bt_RegistrarMovimiento_Click(object sender, EventArgs e)
        {

        }
    }
}