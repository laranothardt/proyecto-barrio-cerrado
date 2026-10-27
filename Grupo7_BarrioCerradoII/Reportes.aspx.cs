using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Grupo7_BarrioCerradoII
{
    public partial class Reportes : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarReportes();
            }
        }

        private void CargarReportes()
        {
            DataSet ds = BIZ.Data.Movimiento.ObtenerMovimientos();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                RpTablaUsuarios.DataSource = ds.Tables[0];
                RpTablaUsuarios.DataBind();

            }
            else
            {
                RpTablaUsuarios.DataSource = null;
                RpTablaUsuarios.DataBind();
            }
        }
    }
}


