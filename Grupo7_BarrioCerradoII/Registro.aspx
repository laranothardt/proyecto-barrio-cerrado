<%@ Page Title="Registrarse" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="Grupo7_BarrioCerradoII.Registro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <main id="pg-registro" class="auth-card">
        <div class="login-container">
            <h2 class="login-title">Crear Cuenta</h2>

            <%-- Mensajes de estado --%>
            <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert" Style="margin-bottom: 20px; font-size: 0.95rem; padding: 12px; border-radius: 8px; text-align: left;">
                <asp:Literal ID="litMensaje" runat="server"></asp:Literal>
            </asp:Panel>

            <%--Nombre Completo--%>
            <div class="row g-3">
                <div class="col-md-6 form-group">
                <label class="form-label" for="nombre">Nombre Completo</label>
                <asp:TextBox ID="tx_nombre" runat="server" class="form-control" placeholder="Juan Pérez"></asp:TextBox>
            </div>

            <%--DNI--%>
            <div class="col-md-6 form-group">
                <label class="form-label" for="dni">DNI</label>
                <asp:TextBox ID="tx_dni" runat="server" class="form-control" placeholder="Ej: 30123456"></asp:TextBox>
            </div>

            <%--Foto Identificatoria--%>
            <div class="col-md-6 form-group">
                <label class="form-label" for="foto">Foto Identificatoria (Escaneo Biométrico)</label>
                <asp:FileUpload ID="file_foto" runat="server" class="form-control" />
            </div>

            <%--Email--%>
            <div class="col-md-6 form-group">
                <label class="form-label" for="email">Correo Electrónico</label>
                <asp:TextBox ID="tx_email" runat="server" class="form-control" placeholder="ejemplo@correo.com"></asp:TextBox>
            </div>

            <%--Contraseña--%>
            <div class="col-md-6 form-group">
                <label class="form-label" for="contraseña">Contraseña</label>
                <asp:TextBox ID="tx_contraseña" runat="server" class="form-control" placeholder="••••••••" TextMode="Password"></asp:TextBox>
            </div>

            <%--Confirmar Contraseña--%>
            <div class="col-md-6 form-group">
                <label class="form-label" for="confirmar_contraseña">Confirmar Contraseña</label>
                <asp:TextBox ID="tx_confirmar_contraseña" runat="server" class="form-control" placeholder="••••••••" TextMode="Password"></asp:TextBox>
            </div>

            <%--Rol--%>
            <div class="col-md-6 form-group">
                <label class="form-label" for="rol">Rol en el Barrio</label>
                <asp:DropDownList ID="ddl_rol" runat="server" class="form-select">
                    <asp:ListItem Value="1" Text="Residente"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Propietario"></asp:ListItem>
                </asp:DropDownList>
            </div>
                </div>

            <%--Boton--%>
            <asp:Button ID="bt_registrarse" runat="server" Text="Registrarse" CssClass="btn-submit" OnClick="bt_registrarse_Click" />

            <%-- Iniciar Sesión --%>
            <div class="register-link-container">
                <span>¿Ya tienes una cuenta? </span>
                <asp:LinkButton ID="lnk_login" runat="server" CssClass="btn-register-link" PostBackUrl="~/IniciarSesion.aspx">Inicia Sesión</asp:LinkButton>
            </div>

        </div>
    </main>
</asp:Content>
