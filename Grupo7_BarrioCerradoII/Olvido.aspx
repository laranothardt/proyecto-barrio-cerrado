<%@ Page Title="Recuperar Contraseña" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Olvido.aspx.cs" Inherits="Grupo7_BarrioCerradoII.Olvido" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <main id="pg-recuperar" class="auth-card">
        <div class="login-container">
            <h2 class="login-title">Recuperar Contraseña</h2>
            <p class="text-muted text-center" style="font-size: 0.9rem; margin-bottom: 25px;">
                Ingresa tu correo electrónico registrado y tu nueva contraseña para restablecerla.
            </p>

            <%-- Mensajes de estado --%>
            <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert" style="margin-bottom: 20px; font-size: 0.95rem; padding: 12px; border-radius: 8px; text-align: left;">
                <asp:Literal ID="litMensaje" runat="server"></asp:Literal>
            </asp:Panel>

            <%--Email--%>
            <div class="form-group">
                <label class="form-label" for="email">Correo Electrónico</label>
                <asp:TextBox ID="tx_email" runat="server" class="form-control" placeholder="ejemplo@correo.com"></asp:TextBox>
            </div>

            <%--Nueva Contraseña--%>
            <div class="form-group">
                <label class="form-label" for="nueva_contraseña">Nueva Contraseña</label>
                <asp:TextBox ID="tx_nueva_contraseña" runat="server" class="form-control" placeholder="••••••••" TextMode="Password"></asp:TextBox>
            </div>

            <%--Confirmar Contraseña--%>
            <div class="form-group">
                <label class="form-label" for="confirmar_contraseña">Confirmar Nueva Contraseña</label>
                <asp:TextBox ID="tx_confirmar_contraseña" runat="server" class="form-control" placeholder="••••••••" TextMode="Password"></asp:TextBox>
            </div>

            <%--Boton--%>
            <asp:Button ID="bt_restablecer" runat="server" Text="Restablecer Contraseña" CssClass="btn-submit" OnClick="bt_restablecer_Click" />

            <%-- Volver al Login --%>
            <div class="register-link-container">
                <span>¿Recordaste tu contraseña? </span>
                <asp:LinkButton ID="lnk_login" runat="server" CssClass="btn-register-link" PostBackUrl="~/IniciarSesion.aspx">Inicia Sesión</asp:LinkButton>
            </div>

        </div>
    </main>
</asp:Content>
