<%@ Page Title="Autorizaciones y preautorizaciones" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Autorizar.aspx.cs" Inherits="Grupo7_BarrioCerradoII.Autorizar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="pg-autorizar" class="container py-3">
        
        <div class="mb-4">
            <h1 class="fw-bold">Preacreditación de visitas</h1>
            <p class="text-muted">Carga los datos de la persona que va a ingresar al barrio para dejarla autorizada de antemano.</p>
        </div>

        <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert mb-4" role="alert">
            <asp:Literal ID="litMensaje" runat="server" />
        </asp:Panel>

        <div class="card shadow-sm mb-5">
            <div class="card-body p-4">
                <asp:ValidationSummary ID="valSummary" runat="server" CssClass="alert alert-danger mb-4" DisplayMode="BulletList" HeaderText="Revisa los siguientes datos:" />

                <!-- SECCIÓN 1: Datos del Visitante -->
                <h5 class="text-primary fw-semibold mb-3">1. Datos del visitante</h5>
                <div class="row g-3 mb-4">
                    <div class="col-md-4">
                        <asp:Label runat="server" AssociatedControlID="txtDni" CssClass="form-label fw-bold">DNI</asp:Label>
                        <asp:TextBox ID="txtDni" runat="server" CssClass="form-control" MaxLength="8" placeholder="Ej: 30123456" />
                        <asp:RequiredFieldValidator ID="rfvDni" runat="server" ControlToValidate="txtDni" ErrorMessage="El DNI es obligatorio." CssClass="text-danger small" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revDni" runat="server" ControlToValidate="txtDni" ValidationExpression="^\d{7,8}$" ErrorMessage="El DNI debe tener 7 u 8 dígitos." CssClass="text-danger small" Display="Dynamic" />
                    </div>
                    <div class="col-md-4">
                        <asp:Label runat="server" AssociatedControlID="txtApellido" CssClass="form-label fw-bold">Apellido</asp:Label>
                        <asp:TextBox ID="txtApellido" runat="server" placeholder="Ej: Gómez" CssClass="form-control" MaxLength="100" />
                        <asp:RequiredFieldValidator ID="rfvApellido" runat="server" ControlToValidate="txtApellido" ErrorMessage="El apellido es obligatorio." CssClass="text-danger small" Display="Dynamic" />
                    </div>
                    <div class="col-md-4">
                        <asp:Label runat="server" AssociatedControlID="txtNombre" CssClass="form-label fw-bold">Nombre</asp:Label>
                        <asp:TextBox ID="txtNombre" runat="server" placeholder="Ej: Juan" CssClass="form-control" MaxLength="100" />
                        <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio." CssClass="text-danger small" Display="Dynamic" />
                    </div>
                </div>

                <hr class="my-4 text-muted" />

                <!-- SECCIÓN 2: Destino y Autorización -->
                <h5 class="text-primary fw-semibold mb-3">2. Destino y Autorización</h5>
                <div class="row g-3 mb-4">
                    <div class="col-md-4">
                        <asp:Label runat="server" AssociatedControlID="ddlCategoria" CssClass="form-label fw-bold">Categoría <span class="text-muted fw-normal">(opcional)</span></asp:Label>
                        <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Sin categoría específica" Value="" />
                            <asp:ListItem Text="Propietario / Inquilino" Value="2" />
                            <asp:ListItem Text="Visita" Value="3" />
                            <asp:ListItem Text="Proveedor / Empleado" Value="4" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4">
                        <asp:Label runat="server" AssociatedControlID="txtLote" CssClass="form-label fw-bold">Lote destino</asp:Label>
                        <asp:TextBox ID="txtLote" runat="server" placeholder="Ej: 09" CssClass="form-control" MaxLength="10" />
                        <asp:RequiredFieldValidator ID="rfvLote" runat="server" ControlToValidate="txtLote" ErrorMessage="Elegí el lote destino." CssClass="text-danger small" Display="Dynamic" />
                    </div>
                    <div class="col-md-4">
                        <asp:Label runat="server" AssociatedControlID="txtResidenteAutoriza" CssClass="form-label fw-bold">DNI del residente que autoriza</asp:Label>
                        <asp:TextBox ID="txtResidenteAutoriza" runat="server" CssClass="form-control" MaxLength="8" placeholder="DNI del residente" />
                        <asp:RequiredFieldValidator ID="rfvResidente" runat="server" ControlToValidate="txtResidenteAutoriza" ErrorMessage="Indica el DNI de quien autoriza." CssClass="text-danger small" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revResidente" runat="server" ControlToValidate="txtResidenteAutoriza" ValidationExpression="^\d{7,8}$" ErrorMessage="El DNI del residente debe tener 7 u 8 dígitos." CssClass="text-danger small" Display="Dynamic" />
                    </div>
                </div>

                <hr class="my-4 text-muted" />

                <!-- SECCIÓN 3: Vigencia y Motivo -->
                <h5 class="text-primary fw-semibold mb-3">3. Fechas y Motivo</h5>
                <div class="row g-3 mb-4">
                    <div class="col-md-4">
                        <asp:Label runat="server" AssociatedControlID="txtFechaDesde" CssClass="form-label fw-bold">Válido desde</asp:Label>
                        <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control" TextMode="Date" />
                        <asp:RequiredFieldValidator ID="rfvDesde" runat="server" ControlToValidate="txtFechaDesde" ErrorMessage="Indica la fecha desde." CssClass="text-danger small" Display="Dynamic" />
                    </div>
                    <div class="col-md-4">
                        <asp:Label runat="server" AssociatedControlID="txtFechaHasta" CssClass="form-label fw-bold">Válido hasta</asp:Label>
                        <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control" TextMode="Date" />
                        <asp:RequiredFieldValidator ID="rfvHasta" runat="server" ControlToValidate="txtFechaHasta" ErrorMessage="Indica la fecha hasta." CssClass="text-danger small" Display="Dynamic" />
                    </div>
                    <div class="col-md-4">
                        <asp:Label runat="server" AssociatedControlID="txtMotivo" CssClass="form-label fw-bold">Motivo de la visita</asp:Label>
                        <asp:TextBox ID="txtMotivo" runat="server" placeholder="Ej: Podas, Delivery, Reparación..." CssClass="form-control" MaxLength="250" />
                        <asp:RequiredFieldValidator ID="rfvMotivo" runat="server" ControlToValidate="txtMotivo" ErrorMessage="Indica el motivo de la visita." CssClass="text-danger small" Display="Dynamic" />
                    </div>
                </div>

                <!-- Botones de Acción -->
                <div class="d-flex justify-content-end gap-2 pt-3 border-top">
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary px-4" CausesValidation="false" OnClick="btnLimpiar_Click" />
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar preacreditación" CssClass="btn btn-primary px-4" OnClick="btnGuardar_Click" />
                </div>
            </div>
        </div>

        <!-- Tabla de Registros -->
        <h2 class="h4 fw-bold mb-3">Preacreditaciones cargadas</h2>
        <div class="table-responsive card shadow-sm">
            <asp:GridView ID="gvPreacreditaciones" runat="server" CssClass="table table-hover align-middle mb-0" AutoGenerateColumns="false" GridLines="None" EmptyDataText="Todavía no hay preacreditaciones cargadas.">
                <HeaderStyle CssClass="table-light" />
                <Columns>
                    <asp:BoundField DataField="Dni" HeaderText="DNI" />
                    <asp:TemplateField HeaderText="Apellido y nombre">
                        <ItemTemplate><%# Eval("Apellido") %>, <%# Eval("Nombre") %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="NombreCategoria" HeaderText="Categoría" />
                    <asp:BoundField DataField="NumeroLote" HeaderText="Lote" />
                    <asp:BoundField DataField="NombreResidenteAutoriza" HeaderText="Autoriza" />
                    <asp:BoundField DataField="FechaDesde" HeaderText="Desde" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="FechaHasta" HeaderText="Hasta" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Motivo" HeaderText="Motivo" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>