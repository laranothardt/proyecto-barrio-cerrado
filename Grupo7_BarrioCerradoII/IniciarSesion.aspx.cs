using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using BIZ.Data;
using BIZ.Modelo;

namespace Grupo7_BarrioCerradoII
{
    public partial class Iniciar_Sesion : System.Web.UI.Page
    {
        private const string COOKIE_NAME = "BarrioCerradoUserAuth";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlError.Visible = false;

                // Si ya hay sesión activa
                if (Session["NombreCompleto"] != null)
                {
                    MostrarPantallaBienvenida(Session["NombreCompleto"].ToString());
                    return;
                }

                // Si no hay sesión en Session, pero hay cookie "Recuérdame"
                HttpCookie userCookie = Request.Cookies[COOKIE_NAME];
                if (userCookie != null && !string.IsNullOrEmpty(userCookie["Email"]))
                {
                    string savedEmail = userCookie["Email"];

                    try
                    {
                        BIZ.Data.UsuarioSistema data = new BIZ.Data.UsuarioSistema();
                        BIZ.Modelo.UsuarioSistema usuario = data.ObtenerUsuarioPorEmail(savedEmail);

                        if (usuario != null)
                        {
                            CargarVariablesDeSesion(usuario);
                            CrearTicketAutenticacion(usuario.Username, usuario.FK_Rol, true);
                            MostrarPantallaBienvenida(usuario.NombreCompleto);
                            return;
                        }
                    }
                    catch
                    {
                    }
                }

                pnlFormulario.Visible = true;
                pnlBienvenida.Visible = false;
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
                        // 1. Cargar datos en la Session
                        CargarVariablesDeSesion(usuario);

                        // 2. Crear Ticket de Forms Authentication (Necesario para el Web.config)
                        CrearTicketAutenticacion(usuario.Username, usuario.FK_Rol, chk_recuerdame.Checked);

                        // 3. Manejo de cookie personalizada "Recuérdame"
                        if (chk_recuerdame.Checked)
                        {
                            HttpCookie authCookie = new HttpCookie(COOKIE_NAME);
                            authCookie["Email"] = usuario.Username;
                            authCookie.Expires = DateTime.Now.AddDays(30);
                            Response.Cookies.Add(authCookie);
                        }
                        else
                        {
                            if (Request.Cookies[COOKIE_NAME] != null)
                            {
                                HttpCookie deleteCookie = new HttpCookie(COOKIE_NAME);
                                deleteCookie.Expires = DateTime.Now.AddDays(-1);
                                Response.Cookies.Add(deleteCookie);
                            }
                        }

                        // 4. Redireccionar según el rol (3 = Admin)
                        RedireccionarSegunRol(usuario.FK_Rol);
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

        protected void btnIrPanel_Click(object sender, EventArgs e)
        {
            string rol = Session["Rol"] != null ? Session["Rol"].ToString() : "";
            RedireccionarSegunRol(rol);
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            // Cerrar autenticación de Forms Authentication
            FormsAuthentication.SignOut();

            Session.Clear();
            Session.Abandon();

            if (Request.Cookies[COOKIE_NAME] != null)
            {
                HttpCookie deleteCookie = new HttpCookie(COOKIE_NAME);
                deleteCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(deleteCookie);
            }

            Response.Redirect("~/IniciarSesion.aspx");
        }

        private void CrearTicketAutenticacion(string username, string rol, bool persistente)
        {
            // Genera el ticket de Forms Authentication agregando el ID de rol ("3") en UserData
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,
                username,
                DateTime.Now,
                DateTime.Now.AddMinutes(120),
                persistente,
                rol ?? "3",
                FormsAuthentication.FormsCookiePath
            );

            string ticketEncriptado = FormsAuthentication.Encrypt(ticket);
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticketEncriptado);

            if (persistente)
            {
                authCookie.Expires = ticket.Expiration;
            }

            Response.Cookies.Add(authCookie);
        }

        private void CargarVariablesDeSesion(BIZ.Modelo.UsuarioSistema usuario)
        {
            Session["Usuario"] = usuario.Username;
            Session["NombreCompleto"] = usuario.NombreCompleto;
            Session["Rol"] = usuario.FK_Rol;
            Session["Dni"] = usuario.Dni;
        }

        private void MostrarPantallaBienvenida(string nombreCompleto)
        {
            litSaludo.Text = "¡Hola, " + nombreCompleto + "!";
            pnlFormulario.Visible = false;
            pnlBienvenida.Visible = true;
        }

        private void RedireccionarSegunRol(string rol)
        {
            if (rol == "3" || rol == "Administrador")
            {
                Response.Redirect("~/Administradores.aspx");
            }
            else
            {
                Response.Redirect("~/Autorizar.aspx");
            }
        }
    }
}