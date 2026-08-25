using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Grupo7_BarrioCerradoII
{
    public partial class Administradores : System.Web.UI.Page
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

                if (rol != "3" && rol != "Administrador")
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }
            }
        }

        protected void Bt_Reportes_Click(object sender, EventArgs e)
        {
            Response.Redirect("Reportes.aspx");
        }

        protected void Bt_Movimientos_Click(object sender, EventArgs e)
        {
            Response.Redirect("Movimientos.aspx");
        }
    }
}