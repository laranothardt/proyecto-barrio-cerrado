<%@ Page Title="Autorizaciones y preautorizaciones" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Autorizar.aspx.cs" Inherits="Grupo7_BarrioCerradoII.Autorizar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <h1>Preacreditacion de visitas</h1>
    <p class="text-muted">Carga los datos de la persona que va a ingresar al barrio para dejarla autorizada de antemano.</p>

    <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert" role="alert">
        <asp:Literal ID="litMensaje" runat="server" />
    </asp:Panel>

    <div class="card mb-4">
        <div class="card-body">
            <asp:ValidationSummary ID="valSummary" runat="server" CssClass="text-danger" DisplayMode="BulletList" HeaderText="Revisa los siguientes datos:" />

            <div class="row g-3">
                <div class="col-md-3">
                    <label for="<%= txtDni.ClientID %>" class="form-label">DNI</label>
                    <asp:TextBox ID="txtDni" runat="server" CssClass="form-control" MaxLength="8" placeholder="Ej: 30123456" />
                    <asp:RequiredFieldValidator ID="rfvDni" runat="server" ControlToValidate="txtDni" ErrorMessage="El DNI es obligatorio." CssClass="text-danger" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revDni" runat="server" ControlToValidate="txtDni" ValidationExpression="\d{7,8}" ErrorMessage="El DNI debe tener 7 u 8 digitos." CssClass="text-danger" Display="Dynamic" />
                </div>
                <div class="col-md-4">
                    <label for="<%= txtApellido.ClientID %>" class="form-label">Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" MaxLength="100" />
                    <asp:RequiredFieldValidator ID="rfvApellido" runat="server" ControlToValidate="txtApellido" ErrorMessage="El apellido es obligatorio." CssClass="text-danger" Display="Dynamic" />
                </div>
                <div class="col-md-5">
                    <label for="<%= txtNombre.ClientID %>" class="form-label">Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="100" />
                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio." CssClass="text-danger" Display="Dynamic" />
                </div>

                <div class="col-md-4">
                    <label for="<%= ddlCategoria.ClientID %>" class="form-label">Categoria <span class="text-muted">(opcional)</span></label>
                    <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                        <asp:ListItem Text="Sin categoria especifica" Value="" />
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label for="<%= ddlLote.ClientID %>" class="form-label">Lote destino</label>
                    <asp:DropDownList ID="ddlLote" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                        <asp:ListItem Text="Selecciona un lote..." Value="" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvLote" runat="server" ControlToValidate="ddlLote" InitialValue="" ErrorMessage="Elegi el lote destino." CssClass="text-danger" Display="Dynamic" />
                </div>
                <div class="col-md-5">
                    <label for="<%= txtResidenteAutoriza.ClientID %>" class="form-label">DNI del residente que autoriza</label>
                    <asp:TextBox ID="txtResidenteAutoriza" runat="server" CssClass="form-control" MaxLength="8" placeholder="DNI del residente" />
                    <asp:RequiredFieldValidator ID="rfvResidente" runat="server" ControlToValidate="txtResidenteAutoriza" ErrorMessage="Indica el DNI de quien autoriza el ingreso." CssClass="text-danger" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revResidente" runat="server" ControlToValidate="txtResidenteAutoriza" ValidationExpression="\d{7,8}" ErrorMessage="El DNI del residente debe tener 7 u 8 digitos." CssClass="text-danger" Display="Dynamic" />
                </div>

                <div class="col-md-3">
                    <label for="<%= txtFechaDesde.ClientID %>" class="form-label">Valido desde</label>
                    <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control" TextMode="Date" />
                    <asp:RequiredFieldValidator ID="rfvDesde" runat="server" ControlToValidate="txtFechaDesde" ErrorMessage="Indica la fecha desde." CssClass="text-danger" Display="Dynamic" />
                </div>
                <div class="col-md-3">
                    <label for="<%= txtFechaHasta.ClientID %>" class="form-label">Valido hasta</label>
                    <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control" TextMode="Date" />
                    <asp:RequiredFieldValidator ID="rfvHasta" runat="server" ControlToValidate="txtFechaHasta" ErrorMessage="Indica la fecha hasta." CssClass="text-danger" Display="Dynamic" />
                </div>
                <div class="col-md-6">
                    <label for="<%= txtMotivo.ClientID %>" class="form-label">Motivo de la visita</label>
                    <asp:TextBox ID="txtMotivo" runat="server" CssClass="form-control" MaxLength="250" />
                    <asp:RequiredFieldValidator ID="rfvMotivo" runat="server" ControlToValidate="txtMotivo" ErrorMessage="Indica el motivo de la visita." CssClass="text-danger" Display="Dynamic" />
                </div>
            </div>

            <div class="mt-4">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar preacreditacion" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary" CausesValidation="false" OnClick="btnLimpiar_Click" />
            </div>
        </div>
    </div>

    <h2>Preacreditaciones cargadas</h2>
    <div class="table-responsive">
        <asp:GridView ID="gvPreacreditaciones" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="false" GridLines="None" EmptyDataText="Todavia no hay preacreditaciones cargadas.">
            <Columns>
                <asp:BoundField DataField="Dni" HeaderText="DNI" />
                <asp:TemplateField HeaderText="Apellido y nombre">
                    <ItemTemplate><%# Eval("Apellido") %>, <%# Eval("Nombre") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Categoria">
                    <ItemTemplate></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Lote">
                    <ItemTemplate></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Autoriza">
                    <ItemTemplate></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="FechaDesde" HeaderText="Desde" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="FechaHasta" HeaderText="Hasta" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="Motivo" HeaderText="Motivo" />
                <asp:BoundField DataField="Estado" HeaderText="Estado" />
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>