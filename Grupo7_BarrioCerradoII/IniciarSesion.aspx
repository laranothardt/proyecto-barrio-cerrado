<%@ Page Title="Iniciar Sesion" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IniciarSesion.aspx.cs" Inherits="Grupo7_BarrioCerradoII.Iniciar_Sesion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <main id="pg-login" class="auth-card">
    <div class="login-container" href="Site.css">
        <h2 class="login-title">Iniciar Sesión</h2>

        <%-- Panel de Error --%>
        <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger" style="margin-bottom: 20px; font-size: 0.95rem; padding: 12px; border-radius: 8px; text-align: left;">
            <asp:Literal ID="litError" runat="server"></asp:Literal>
        </asp:Panel>

        <%--Email--%>
        <div class="form-group mb-4">
            <label class="form-label" for="email">Correo Electrónico</label>
            <asp:TextBox ID="tx_email" runat="server" class="form-control" placeholder="ejemplo@correo.com"></asp:TextBox>
        </div>

        <%--Contraseña--%>
        <div class="form-group mb-4">
            <label class="form-label" for="contraseña">Contraseña</label>
            <asp:TextBox ID="tx_contraseña" runat="server" class="form-control" placeholder="••••••••" TextMode="Password"></asp:TextBox>
        </div>

        <%--Opciones--%>
        <div class="row align-items-center mb-4">
            <div class="col text-start">
                <div class="form-check custom-checkbox">
                    <asp:CheckBox ID="chk_recuerdame" runat="server" value="" />
                    <label class="form-check-label" for="recuerdame">Recuérdame</label>
                </div>
            </div>
            <div class="col text-end">
                <asp:LinkButton ID="lnk_olvido" runat="server" CssClass="btn-register-link" PostBackUrl="~/Olvido.aspx">¿Olvidaste tu contraseña?</asp:LinkButton>
            </div>
        </div>

        <%--Boton--%>
        <asp:Button ID="bt_iniciar_sesion" runat="server" Text="Iniciar Sesión" CssClass="btn-submit" OnClick="bt_iniciar_sesion_Click" />

        <%-- Registrarse --%>
        <div class="register-link-container">
            <span>¿No tienes cuenta? </span>
            <asp:LinkButton ID="lnk_registro" runat="server" CssClass="btn-register-link" PostBackUrl="~/Registro.aspx">Regístrate</asp:LinkButton>
        </div>

    </div>
</main>
</asp:Content>
