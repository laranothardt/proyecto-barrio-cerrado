<%@ Page Title="Administradores" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Administradores.aspx.cs" Inherits="Grupo7_BarrioCerradoII.Administradores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Bienvenido al apartado de Administrador!</h1>

    <asp:Button ID="Bt_Movimientos" runat="server" Text="Registrar Movimientos" OnClick="Bt_Movimientos_Click" CssClass="btn-custom btn-primary-custom" />
    <asp:Button ID="Bt_Reportes" runat="server" Text="Reportes" OnClick="Bt_Reportes_Click" CssClass="btn-custom btn-primary-custom" />

</asp:Content>
