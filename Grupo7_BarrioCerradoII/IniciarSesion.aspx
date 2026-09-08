<%@ Page Title="Iniciar Sesión" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IniciarSesion.aspx.cs" Inherits="Grupo7_BarrioCerradoII.Iniciar_Sesion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <main id="pg-login" class="auth-card">
        <div class="login-container">

            <%-- Panel de Bienvenida (si ya está sesión iniciada) --%>
            <asp:Panel ID="pnlBienvenida" runat="server" Visible="false" CssClass="text-center py-3">
                <h2 class="login-title mb-3">
                    <asp:Literal ID="litSaludo" runat="server"></asp:Literal>
                </h2>
                <p class="text-muted mb-4">Ya has iniciado sesión en el sistema.</p>
                <div class="d-grid gap-2">
                    <asp:Button ID="btnIrPanel" runat="server" Text="Ir al Panel Principal" CssClass="btn-submit mb-2" OnClick="btnIrPanel_Click" />
                    <asp:Button ID="btnCerrarSesion" runat="server" Text="Iniciar sesión con otra cuenta" CssClass="btn btn-outline-secondary w-100" OnClick="btnCerrarSesion_Click" CausesValidation="false" />
                </div>
            </asp:Panel>

            <%-- Panel Formulario de Login --%>
            <asp:Panel ID="pnlFormulario" runat="server">
                <h2 class="login-title">Iniciar Sesión</h2>

                <%-- Panel de Error --%>
                <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger" style="margin-bottom: 20px; font-size: 0.95rem; padding: 12px; border-radius: 8px; text-align: left;">
                    <asp:Literal ID="litError" runat="server"></asp:Literal>
                </asp:Panel>

                <%-- Email --%>
                <div class="form-group mb-4">
                    <label class="form-label" for="<%= tx_email.ClientID %>">Correo Electrónico</label>
                    <asp:TextBox ID="tx_email" runat="server" class="form-control" placeholder="ejemplo@correo.com"></asp:TextBox>
                </div>

                <%-- Contraseña --%>
                <div class="form-group mb-4">
                    <label class="form-label" for="<%= tx_contraseña.ClientID %>">Contraseña</label>
                    <asp:TextBox ID="tx_contraseña" runat="server" class="form-control" placeholder="••••••••" TextMode="Password"></asp:TextBox>
                </div>

                <%-- Opciones --%>
                <div class="row align-items-center mb-4">
                    <div class="col text-start">
                        <div class="form-check custom-checkbox">
                            <asp:CheckBox ID="chk_recuerdame" runat="server" />
                            <label class="form-check-label" for="<%= chk_recuerdame.ClientID %>">Recuérdame</label>
                        </div>
                    </div>
                    <div class="col text-end">
                        <asp:LinkButton ID="lnk_olvido" runat="server" CssClass="btn-register-link" PostBackUrl="~/Olvido.aspx">¿Olvidaste tu contraseña?</asp:LinkButton>
                    </div>
                </div>

                <%-- Botón --%>
                <asp:Button ID="bt_iniciar_sesion" runat="server" Text="Iniciar Sesión" CssClass="btn-submit" OnClick="bt_iniciar_sesion_Click" />

                <%-- Registrarse --%>
                <div class="register-link-container">
                    <span>¿No tienes cuenta? </span>
                    <asp:LinkButton ID="lnk_registro" runat="server" CssClass="btn-register-link" PostBackUrl="~/Registro.aspx">Regístrate</asp:LinkButton>
                </div>
            </asp:Panel>

        </div>
    </main>
</asp:Content>