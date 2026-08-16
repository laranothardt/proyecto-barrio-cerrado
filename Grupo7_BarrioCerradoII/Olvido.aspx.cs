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
    public partial class Olvido : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlMensaje.Visible = false;
            }
        }

        protected void bt_restablecer_Click(object sender, EventArgs e)
        {
            string email = tx_email.Text.Trim();
            string nuevaContraseña = tx_nueva_contraseña.Text.Trim();
            string confirmarContraseña = tx_confirmar_contraseña.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(nuevaContraseña) || string.IsNullOrEmpty(confirmarContraseña))
            {
                MostrarMensaje("Todos los campos son obligatorios.", false);
                return;
            }

            if (nuevaContraseña != confirmarContraseña)
            {
                MostrarMensaje("Las contraseñas no coinciden.", false);
                return;
            }

            // Validar formato básico de email
            if (!email.Contains("@") || !email.Contains("."))
            {
                MostrarMensaje("Ingrese un correo electrónico válido.", false);
                return;
            }

            try
            {
                BIZ.Data.UsuarioSistema data = new BIZ.Data.UsuarioSistema();

                // Verificar si existe el usuario
                var usuario = data.ObtenerUsuarioPorEmail(email);
                if (usuario == null)
                {
                    MostrarMensaje("El correo electrónico no está registrado.", false);
                    return;
                }

                // Generar hash y actualizar contraseña
                string passwordHash = BIZ.Data.UsuarioSistema.HashPassword(nuevaContraseña);
                bool actualizado = data.ActualizarPassword(email, passwordHash);

                if (actualizado)
                {
                    MostrarMensaje("Contraseña restablecida exitosamente. Ya puedes iniciar sesión con tu nueva contraseña.", true);
                    LimpiarFormulario();
                }
                else
                {
                    MostrarMensaje("No se pudo actualizar la contraseña. Inténtelo de nuevo.", false);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error al restablecer la contraseña: " + ex.Message, false);
            }
        }

        private void MostrarMensaje(string texto, bool esExito)
        {
            pnlMensaje.Visible = true;
            litMensaje.Text = texto;
            if (esExito)
            {
                pnlMensaje.CssClass = "alert alert-success";
            }
            else
            {
                pnlMensaje.CssClass = "alert alert-danger";
            }
        }

        private void LimpiarFormulario()
        {
            tx_email.Text = string.Empty;
            tx_nueva_contraseña.Text = string.Empty;
            tx_confirmar_contraseña.Text = string.Empty;
        }
    }
}