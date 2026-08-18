<%@ Page Title="Administradores" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Administradores.aspx.cs" Inherits="Grupo7_BarrioCerradoII.Administradores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Bienvenido al apartado de Administrador!</h1>


    <asp:Button ID="Bt_Registrar_Ingreso_Egreso" runat="server" Text="Registro Egresos/Egresos" CssClass="btn-primary-custom" OnClick="Bt_Registrar_Ingreso_Egreso_Click"/>
    <asp:Button ID="Bt_Reportes" runat="server" Text="Reportes" OnClick="Bt_Reportes_Click" />

<%--    <div class="row fw-bold border-bottom pb-2">
        <div class="col-md-2">Movimiento</div>
        <div class="col-md-1">Fecha</div>
        <div class="col-md-1">Hora</div>
        <div class="col-md-1">Lote</div>
        <div class="col-md-1">DNI</div>
        <div class="col-md-2">Nombre y Apellido</div>
        <div class="col-md-2">Tipo</div>
        <div class="col-md-2">Patente</div>
    </div>

    <asp:Repeater ID="RpTablaUsuarios" runat="server">
        <ItemTemplate>
            <div class="row border-bottom py-1">
                <div class="col-md-2">
                    <asp:Label ID="lblTipoMovimiento" runat="server" Text='<%#Eval("TipoMovimiento") %>'></asp:Label>
                </div>
                <div class="col-md-1">
                    <asp:Label ID="lblFecha" runat="server" Text='<%#Eval("Fecha", "{0:dd/MM/yyyy}") %>'></asp:Label>
                </div>
                <div class="col-md-1">
                    <asp:Label ID="lblHora" runat="server" Text='<%#Eval("Hora", "{0:HH:mm}") %>'></asp:Label>
                </div>
                <div class="col-md-1">
                    <asp:Label ID="lblLote" runat="server" Text='<%#Eval("Lote") %>'></asp:Label>
                </div>
                <div class="col-md-1">
                    <asp:Label ID="lblDNI" runat="server" Text='<%#Eval("DNI") %>'></asp:Label>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblNombre" runat="server" Text='<%#Eval("NombreCompleto") %>'></asp:Label>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblTipoIngreso" runat="server" Text='<%#Eval("TipoIngreso") %>'></asp:Label>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblPatente" runat="server" Text='<%#Eval("Patente") %>'></asp:Label>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>--%>
</asp:Content>
