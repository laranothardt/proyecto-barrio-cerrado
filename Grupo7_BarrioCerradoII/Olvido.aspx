<%@ Page Title="Recuperar Contraseña" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Olvido.aspx.cs" Inherits="Grupo7_BarrioCerradoII.Olvido" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <main id="pg-recuperar" class="auth-card">
        <div class="login-container">
            <h2 class="login-title">Recuperar Contraseña</h2>

            <%-- Mensajes de estado --%>
            <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert" style="margin-bottom: 20px; font-size: 0.95rem; padding: 12px; border-radius: 8px; text-align: left;">
                <asp:Literal ID="litMensaje" runat="server"></asp:Literal>
            </asp:Panel>

            <%-- PASO 1: Solicitar Email --%>
            <asp:Panel ID="pnlSolicitud" runat="server">
                <p class="text-muted text-center" style="font-size: 0.9rem; margin-bottom: 25px;">
                    Ingresa tu correo electrónico registrado y te enviaremos un enlace seguro para restablecer tu contraseña.
                </p>

                <div class="form-group mb-4">
                    <label class="form-label" for="<%= tx_email.ClientID %>">Correo Electrónico</label>
                    <asp:TextBox ID="tx_email" runat="server" class="form-control" placeholder="ejemplo@correo.com"></asp:TextBox>
                </div>

                <asp:Button ID="bt_solicitar" runat="server" Text="Enviar Enlace de Recuperación" CssClass="btn-submit" OnClick="bt_solicitar_Click" />
            </asp:Panel>

            <%-- PASO 2: Ingresar Nueva Contraseña (Solo visible con Token válido) --%>
            <asp:Panel ID="pnlRestablecer" runat="server" Visible="false">
                <p class="text-muted text-center" style="font-size: 0.9rem; margin-bottom: 25px;">
                    Ingresa tu nueva contraseña para completar el restablecimiento.
                </p>

                <%--Nueva Contraseña--%>
                <div class="form-group mb-4">
                    <label class="form-label" for="<%= tx_nueva_contraseña.ClientID %>">Nueva Contraseña</label>
                    <asp:TextBox ID="tx_nueva_contraseña" runat="server" class="form-control" placeholder="••••••••" TextMode="Password"></asp:TextBox>
                </div>

                <%--Confirmar Contraseña--%>
                <div class="form-group mb-4">
                    <label class="form-label" for="<%= tx_confirmar_contraseña.ClientID %>">Confirmar Nueva Contraseña</label>
                    <asp:TextBox ID="tx_confirmar_contraseña" runat="server" class="form-control" placeholder="••••••••" TextMode="Password"></asp:TextBox>
                </div>

                <asp:Button ID="bt_restablecer" runat="server" Text="Restablecer Contraseña" CssClass="btn-submit" OnClick="bt_restablecer_Click" />
            </asp:Panel>

            <%-- Volver al Login --%>
            <div class="register-link-container mt-4">
                <span>¿Recordaste tu contraseña? </span>
                <asp:LinkButton ID="lnk_login" runat="server" CssClass="btn-register-link" PostBackUrl="~/IniciarSesion.aspx">Inicia Sesión</asp:LinkButton>
            </div>

        </div>
    </main>
</asp:Content>