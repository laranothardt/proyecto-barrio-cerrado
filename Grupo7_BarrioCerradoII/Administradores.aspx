<%@ Page Title="Administradores" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Administradores.aspx.cs" Inherits="Grupo7_BarrioCerradoII.Administradores" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div id="pg-admin" class="admin-wrapper">
        <h1 class="admin-titulo">Bienvenido al apartado de Administrador!</h1>
        <div class="admin-accion">

            <asp:Button ID="Bt_Movimientos" runat="server" Text="Registrar Movimientos" OnClick="Bt_Movimientos_Click" CssClass="btn-custom btn-primary-custom" />
            <asp:Button ID="Bt_Reportes" runat="server" Text="Reportes" OnClick="Bt_Reportes_Click" CssClass="btn-custom btn-primary-custom" />
            <asp:Button ID="Bt_Vehiculos" runat="server" Text="Consultar Vehiculos" OnClick="Bt_Vehiculos_Click" CssClass="btn-custom btn-primary-custom" />
            <asp:Button ID="Bt_Restricciones" runat="server" Text="Consultar Restricciones" OnClick="Bt_Restricciones_Click" CssClass="btn-custom btn-primary-custom" />
        </div>
        <div class="card shadow-sm border-0 mt-4">
    <div class="card-header bg-white py-3">
        <h5 class="mb-0 fw-bold text-primary">🤖 Asistente IA Autónomo (Consultas a Base de Datos)</h5>
    </div>
    <div class="card-body">
        <div class="mb-3">
            <label class="form-label">Escribile cualquier consulta sobre el barrio (Ej: "¿Cuántas personas hay adentro?", "¿Qué vehículos ingresaron hoy?"):</label>
            <asp:TextBox ID="txtPreguntaLibre" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Escribí tu pregunta aquí..."></asp:TextBox>
        </div>
        
        <asp:Button ID="btnConsultarIa" runat="server" Text="Analizar con IA y BD" CssClass="btn btn-primary px-4 fw-bold" OnClick="btnConsultarIa_Click" />
        
        <div class="mt-4">
            <label class="form-label fw-bold text-secondary">Respuesta del Asistente:</label>
            <div class="p-3 bg-light border rounded" style="min-height: 80px; white-space: pre-wrap;">
                <asp:Label ID="lblResultadoIa" runat="server" CssClass="text-dark"></asp:Label>
            </div>
        </div>
    </div>
</div>
    </div>



</asp:Content>
