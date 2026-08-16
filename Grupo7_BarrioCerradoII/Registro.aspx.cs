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
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlMensaje.Visible = false;
            }
        }

        protected void bt_registrarse_Click(object sender, EventArgs e)
        {
            string nombre = tx_nombre.Text.Trim();
            string dni = tx_dni.Text.Trim();
            string email = tx_email.Text.Trim();
            string password = tx_contraseña.Text.Trim();
            string confirmPassword = tx_confirmar_contraseña.Text.Trim();
            string rol = ddl_rol.SelectedValue;

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(dni) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MostrarMensaje("Todos los campos son obligatorios.", false);
                return;
            }

            if (password != confirmPassword)
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

            byte[] fotoBytes = null;
            if (file_foto.HasFile)
            {
                string ext = System.IO.Path.GetExtension(file_foto.FileName).ToLower();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".gif")
                {
                    MostrarMensaje("La foto debe ser un archivo de imagen (.jpg, .jpeg, .png, o .gif).", false);
                    return;
                }

                fotoBytes = file_foto.FileBytes;
            }
            else
            {
                MostrarMensaje("La foto identificatoria para escaneo biométrico es obligatoria.", false);
                return;
            }

            try
            {
                BIZ.Data.UsuarioSistema data = new BIZ.Data.UsuarioSistema();

                // Verificar si ya existe el usuario
                if (data.ObtenerUsuarioPorEmail(email) != null)
                {
                    MostrarMensaje("El correo electrónico ya está registrado.", false);
                    return;
                }

                // Crear el nuevo usuario
                var nuevoUsuario = new BIZ.Modelo.UsuarioSistema
                {
                    Username = email,
                    PasswordHash = BIZ.Data.UsuarioSistema.HashPassword(password),
                    NombreCompleto = nombre,
                    Rol = rol,
                    Dni = dni,
                    Foto = fotoBytes
                };

                bool creado = data.CrearUsuario(nuevoUsuario);

                if (creado)
                {
                    MostrarMensaje("Cuenta creada exitosamente. Ya puedes iniciar sesión.", true);
                    LimpiarFormulario();
                }
                else
                {
                    MostrarMensaje("No se pudo crear la cuenta. Inténtelo de nuevo.", false);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error al registrar la cuenta: " + ex.Message, false);
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
            tx_nombre.Text = string.Empty;
            tx_dni.Text = string.Empty;
            tx_email.Text = string.Empty;
            tx_contraseña.Text = string.Empty;
            tx_confirmar_contraseña.Text = string.Empty;
            ddl_rol.SelectedIndex = 0;
        }
    }
}