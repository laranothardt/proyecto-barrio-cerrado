using System;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using BIZ.Data;
using BIZ.Modelo;

namespace Grupo7_BarrioCerradoII
{
    public partial class Olvido : System.Web.UI.Page
    {
        private const string SMTP_HOST = "smtp.gmail.com";
        private const int SMTP_PORT = 587;
        private string SMTP_USER => System.Configuration.ConfigurationManager.AppSettings["SmtpUser"] ?? "laranothardt@gmail.com";
        private string SMTP_PASS => System.Configuration.ConfigurationManager.AppSettings["SmtpPass"] ?? string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlMensaje.Visible = false;

                string token = Request.QueryString["token"];

                if (!string.IsNullOrEmpty(token))
                {
                    if (ValidarToken(token))
                    {
                        pnlSolicitud.Visible = false;
                        pnlRestablecer.Visible = true;
                    }
                    else
                    {
                        MostrarMensaje("El enlace de recuperación es inválido o ha expirado. Por favor solicita uno nuevo.", false);
                        pnlSolicitud.Visible = true;
                        pnlRestablecer.Visible = false;
                    }
                }
                else
                {
                    pnlSolicitud.Visible = true;
                    pnlRestablecer.Visible = false;
                }
            }
        }

        protected void bt_solicitar_Click(object sender, EventArgs e)
        {
            string email = tx_email.Text.Trim();

            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                MostrarMensaje("Ingrese un correo electrónico válido.", false);
                return;
            }

            try
            {
                BIZ.Data.UsuarioSistema data = new BIZ.Data.UsuarioSistema();
                var usuario = data.ObtenerUsuarioPorEmail(email);

                if (usuario == null)
                {
                    MostrarMensaje("El correo electrónico no está registrado.", false);
                    return;
                }

                string token = Guid.NewGuid().ToString();
                Session["ResetToken_" + token] = email;
                Session["ResetToken_Expira_" + token] = DateTime.Now.AddMinutes(30);

                string resetLink = Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/Olvido.aspx?token=" + token);

                EnviarEmailRecuperacion(email, resetLink);

                MostrarMensaje("Se ha enviado un enlace de recuperación a tu correo electrónico. Revisa tu bandeja de entrada o SPAM.", true);
                pnlSolicitud.Visible = false;
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al enviar el correo electrónico: " + ex.Message, false);
            }
        }

        protected void bt_restablecer_Click(object sender, EventArgs e)
        {
            string token = Request.QueryString["token"];
            string nuevaContraseña = tx_nueva_contraseña.Text.Trim();
            string confirmarContraseña = tx_confirmar_contraseña.Text.Trim();

            if (string.IsNullOrEmpty(nuevaContraseña) || string.IsNullOrEmpty(confirmarContraseña))
            {
                MostrarMensaje("Todos los campos son obligatorios.", false);
                return;
            }

            if (nuevaContraseña != confirmarContraseña)
            {
                MostrarMensaje("Las contraseñas no coinciden.", false);
                return;
            }

            if (!ValidarToken(token))
            {
                MostrarMensaje("El enlace ha expirado o es inválido.", false);
                return;
            }

            try
            {
                string email = Session["ResetToken_" + token].ToString();
                BIZ.Data.UsuarioSistema data = new BIZ.Data.UsuarioSistema();

                string passwordHash = BIZ.Data.UsuarioSistema.HashPassword(nuevaContraseña);
                bool actualizado = data.ActualizarPassword(email, passwordHash);

                if (actualizado)
                {
                    Session.Remove("ResetToken_" + token);
                    Session.Remove("ResetToken_Expira_" + token);

                    MostrarMensaje("¡Contraseña restablecida con éxito! Ya puedes iniciar sesión con tu nueva clave.", true);
                    pnlRestablecer.Visible = false;
                }
                else
                {
                    MostrarMensaje("No se pudo actualizar la contraseña. Inténtalo de nuevo.", false);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error al restablecer la contraseña: " + ex.Message, false);
            }
        }

        private bool ValidarToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return false;

            if (Session["ResetToken_" + token] != null && Session["ResetToken_Expira_" + token] != null)
            {
                DateTime expira = (DateTime)Session["ResetToken_Expira_" + token];
                if (DateTime.Now <= expira)
                {
                    return true;
                }
            }
            return false;
        }

        private void EnviarEmailRecuperacion(string emailDestino, string enlace)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(SMTP_USER, "Barrio Cerrado - Soporte");
            mail.To.Add(emailDestino);
            mail.Subject = "Restablece tu contraseña - Barrio Cerrado";
            mail.Body = $@"
                <h2>Solicitud de restablecimiento de contraseña</h2>
                <p>Has solicitado restablecer tu contraseña para acceder al sistema.</p>
                <p>Haz clic en el siguiente enlace para continuar:</p>
                <p><a href='{enlace}' style='background-color:#00a8cc; color:white; padding:10px 15px; text-decoration:none; border-radius:5px;'>Restablecer Contraseña</a></p>
                <br/>
                <p><small>Este enlace expira en 30 minutos. Si no solicitaste este cambio, puedes ignorar este correo.</small></p>";
            mail.IsBodyHtml = true;

            using (SmtpClient smtp = new SmtpClient(SMTP_HOST, SMTP_PORT))
            {
                smtp.Credentials = new NetworkCredential(SMTP_USER, SMTP_PASS);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
        }

        private void MostrarMensaje(string texto, bool esExito)
        {
            pnlMensaje.Visible = true;
            litMensaje.Text = texto;
            pnlMensaje.CssClass = esExito ? "alert alert-success" : "alert alert-danger";
        }

        
    }
}