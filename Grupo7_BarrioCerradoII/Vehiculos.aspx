<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Vehiculos.aspx.cs" Inherits="Grupo7_BarrioCerradoII.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <main id="pg-vehiculos">

        <h1 class="vehiculos-titulo">Registro de Vehículos</h1>


        <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert" Style="margin-bottom: 20px; font-size: 0.95rem; padding: 12px; border-radius: 8px; text-align: left;">
            <asp:Literal ID="litMensaje" runat="server"></asp:Literal>
        </asp:Panel>

        <div class="row g-3 vehiculos-acciones">
            <div class="col-md-6">
                <asp:Button ID="BtBuscar" runat="server" Text="Buscar Vehículo" CssClass="btn-submit" OnClick="BtBuscar_Click" />
            </div>
            <div class="col-md-6">
                <asp:Button ID="BtRegistrar" runat="server" Text="Registrar Nuevo Vehículo" CssClass="btn-submit" OnClick="BtRegistrar_Click" />
            </div>
        </div>


        <asp:Panel ID="Buscar" runat="server" CssClass="vehiculos-panel" Visible="false">

            <div class="row g-3">
                <div class="col-md-6 form-group">
                    <asp:Label ID="lbPatente" runat="server" CssClass="form-label" Text="Patente:"></asp:Label>
                    <asp:TextBox ID="txPatente" runat="server" CssClass="form-control" OnTextChanged="txPatente_TextChanged"></asp:TextBox>
                </div>
                <div class="col-md-6 form-group">
                    <asp:Label ID="lbTitular" runat="server" CssClass="form-label" Text="Titular:"></asp:Label>
                    <asp:TextBox ID="txTitular" runat="server" CssClass="form-control" OnTextChanged="txTitular_TextChanged"></asp:TextBox>
                </div>
            </div>

            <h2 class="vehiculos-subtitulo">Listado de Vehículos</h2>

            <div class="vehiculos-tabla">
                <div class="row vehiculos-tabla-header">
                    <div class="col-md-3">Patente</div>
                    <div class="col-md-3">Titular</div>
                    <div class="col-md-3">Seguro</div>
                    <div class="col-md-3">Fecha de vencimiento</div>
                </div>

                <asp:Repeater ID="RpVehiculos" runat="server">
                    <ItemTemplate>
                        <div class="row vehiculos-tabla-fila">
                            <div class="col-md-3"><%# Eval("patente")%></div>
                            <div class="col-md-3"><%# Eval("titular")%></div>
                            <div class="col-md-3"><%# Eval("seguro")%></div>
                            <div class="col-md-3"><%# Eval("fecha_vencimiento")%></div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

        </asp:Panel>


        <asp:Panel ID="Agregar" runat="server" CssClass="vehiculos-panel" Visible="false">

            <h2 class="vehiculos-subtitulo">Ingrese los datos del vehículo</h2>

            <div class="row g-3">
                <div class="col-md-6 form-group">
                    <label class="form-label" for="IngresoPatente">Patente</label>
                    <asp:TextBox ID="IngresoPatente" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-md-6 form-group">
                    <label class="form-label" for="IngresoTitular">Titular</label>
                    <asp:TextBox ID="IngresoTitular" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-md-6 form-group">
                    <label class="form-label" for="IngresoSeguro">Seguro</label>
                    <asp:TextBox ID="IngresoSeguro" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-md-6 form-group">
                    <label class="form-label" for="IngresoVencimiento">Vencimiento Seguro</label>
                    <asp:TextBox ID="IngresoVencimiento" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <br />
            <asp:Button ID="BtGuardar" runat="server" Text="Guardar Vehículo" CssClass="btn-submit" OnClick="BtGuardar_Click" />

        </asp:Panel>

    </main>
</asp:Content>
