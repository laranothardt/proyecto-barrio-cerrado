using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Grupo7_BarrioCerradoII
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            Response.Redirect("Iniciar Sesion.aspx");
        }

        protected void btnAutorizar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Autorizar.aspx");
        }
    }
}