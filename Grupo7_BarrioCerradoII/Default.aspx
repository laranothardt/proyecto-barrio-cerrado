<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Grupo7_BarrioCerradoII._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section class="hero-banner">
            <%--Titulo--%> 
            <h1 class="hero-title">Bienvenido a Barrio Cerrado El Burro</h1>
            <%--Descripcion--%>
            <p class="hero-subtitle">
                Gestión ágil y segura para nuestros residentes y visitas. Accede a tu cuenta o autoriza nuevos ingresos al barrio de forma rápida.
            </p>

            <%--Botones de acción--%>
            <div class="action-buttons">
                <asp:Button ID="btnIniciarSesion" runat="server" Text="Iniciar Sesión" CssClass="btn-custom btn-primary-custom" OnClick="btnIniciarSesion_Click" />
                <asp:Button ID="btnAutorizar" runat="server" Text="Autorizar Ingreso" CssClass="btn-custom btn-outline-custom" OnClick="btnAutorizar_Click" />
            </div>

        </section>
    </main>

</asp:Content>
