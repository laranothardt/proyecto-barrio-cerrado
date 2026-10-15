using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace Grupo7_BarrioCerradoII
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Código que se ejecuta al iniciar la aplicación
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
            {
                try
                {
                    // Desencriptar la cookie de autenticación de Forms
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

                    if (ticket != null && !ticket.Expired)
                    {
                        // Extraer el rol que guardamos en UserData ("3")
                        string[] roles = ticket.UserData.Split(',');

                        // Asignar la identidad y el rol al contexto HTTP actual
                        HttpContext.Current.User = new GenericPrincipal(new FormsIdentity(ticket), roles);
                    }
                }
                catch
                {
                    // Manejo de cookie corrupta o inválida
                }
            }
        }
    }
}