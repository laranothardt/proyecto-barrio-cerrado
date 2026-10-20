using System;
using System.Web.UI;

namespace Grupo7_BarrioCerradoII
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Session["Rol"] != null)
            {
                string rol = Session["Rol"].ToString(); // "1" = Residente, "2" = Propietario, "3" = Admin

                if (rol == "2" || rol == "Propietario")
                {
                    // Aplicar visibilidad segun variables guardadas
                    linkAdministradores.Visible = Application["Permiso_Propietario_Administradores"] != null && (bool)Application["Permiso_Propietario_Administradores"];
                    linkAutorizar.Visible = Application["Permiso_Propietario_Autorizar"] == null || (bool)Application["Permiso_Propietario_Autorizar"];
                    //linkIngresoEgreso.Visible = Application["Permiso_Propietario_IngresoEgreso"] != null && (bool)Application["Permiso_Propietario_IngresoEgreso"];
                    //linkMovimientos.Visible = Application["Permiso_Propietario_Movimientos"] != null && (bool)Application["Permiso_Propietario_Movimientos"];
                    //linkRestricciones.Visible = false;
                }
                else if (rol == "1" || rol == "Residente")
                {
                    linkAdministradores.Visible = Application["Permiso_Residente_Administradores"] != null && (bool)Application["Permiso_Residente_Administradores"];
                    linkAutorizar.Visible = Application["Permiso_Residente_Autorizar"] == null || (bool)Application["Permiso_Residente_Autorizar"];
                    //linkIngresoEgreso.Visible = Application["Permiso_Residente_IngresoEgreso"] != null && (bool)Application["Permiso_Residente_IngresoEgreso"];
                    //linkMovimientos.Visible = Application["Permiso_Residente_Movimientos"] != null && (bool)Application["Permiso_Residente_Movimientos"];
                    //linkRestricciones.Visible = false;
                }
            }
        }
    }
}