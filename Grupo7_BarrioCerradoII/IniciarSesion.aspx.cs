using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BIZ.Data;
using BIZ.Modelo;

namespace Grupo7_BarrioCerradoII
{
    public partial class Iniciar_Sesion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlError.Visible = false;
            }
        }

        protected void bt_iniciar_sesion_Click(object sender, EventArgs e)
        {
            string email = tx_email.Text.Trim();
            string password = tx_contraseña.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                pnlError.Visible = true;
                litError.Text = "Por favor ingrese su correo electrónico y contraseña.";
                return;
            }

            try
            {
                BIZ.Data.UsuarioSistema data = new BIZ.Data.UsuarioSistema();
                BIZ.Modelo.UsuarioSistema usuario = data.ObtenerUsuarioPorEmail(email);

                if (usuario != null)
                {
                    string passwordHash = BIZ.Data.UsuarioSistema.HashPassword(password);
                    if (usuario.PasswordHash == passwordHash)
                    {
                        // Iniciar sesión
                        Session["Usuario"] = usuario.Username;
                        Session["NombreCompleto"] = usuario.NombreCompleto;
                        Session["Rol"] = usuario.FK_Rol;
                        Session["Dni"] = usuario.Dni;

                        // Redireccionar
                        if (usuario.FK_Rol == "3" || usuario.FK_Rol == "Administrador")
                        {
                            Response.Redirect("~/Administradores.aspx");
                        }
                        else
                        {
                            Response.Redirect("~/Autorizar.aspx");
                        }
                    }
                    else
                    {
                        pnlError.Visible = true;
                        litError.Text = "Contraseña incorrecta.";
                    }
                }
                else
                {
                    pnlError.Visible = true;
                    litError.Text = "El correo electrónico no está registrado.";
                }
            }
            catch (Exception ex)
            {
                pnlError.Visible = true;
                litError.Text = "Ocurrió un error al intentar iniciar sesión: " + ex.Message;
            }
        }
    }
}