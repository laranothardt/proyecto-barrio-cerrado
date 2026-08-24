<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Movimientos.aspx.cs" Inherits="Grupo7_BarrioCerradoII.Movimientos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container my-5">
        <h2 class="mb-4 text-primary">Registrar Ingreso / Egreso</h2>
        
        <div class="card shadow-sm">
            <div class="card-body">
                <div class="row g-3">
                    
                    <%-- DNI Usuario --%>
                    <div class="col-md-4">
                        <label class="form-label fw-bold">DNI del Usuario</label>
                        <div class="input-group">
                            <asp:TextBox ID="Tx_Dni" runat="server" CssClass="form-control" placeholder="40123456"></asp:TextBox>
                            <asp:Button ID="Bt_BuscarDNI" runat="server" Text="Buscar" CssClass="btn btn-secondary" OnClick="Bt_BuscarDNI_Click"/>
                        </div>
                        <asp:Label ID="Lb_NombrePersona" runat="server" CssClass="text-success small mt-1"></asp:Label>
                    </div>

                    <%-- Tipo de Movimiento --%>
                    <div class="col-md-4">
                        <label class="form-label fw-bold">Tipo de Movimiento</label>
                        <asp:DropDownList ID="DDLTipoMovimiento" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Ingreso" Value="Ingreso" />
                            <asp:ListItem Text="Egreso" Value="Egreso" />
                        </asp:DropDownList>
                    </div>

                    <%-- Punto de Acceso --%>
                    <div class="col-md-4">
                        <label class="form-label fw-bold">Punto de Acceso</label>
                        <asp:DropDownList ID="DDLPuntoAcceso" runat="server" CssClass="form-select">
                            <%-- Esto luego se carga desde la BDD--%>
                            <asp:ListItem Text="Guardia Principal" Value="1" />
                            <asp:ListItem Text="Acceso Proveedores" Value="2" />
                        </asp:DropDownList>
                    </div>

                    <%-- Lote Destino --%>
                    <div class="col-md-4">
                        <label class="form-label fw-bold">Lote Destino (Opcional)</label>
                        <asp:DropDownList ID="DDLLoteDestino" runat="server" CssClass="form-select">                          
                            <asp:ListItem Text="(Cargar Lotes)" Value="1" />
                        </asp:DropDownList>
                    </div>

                    <%-- Vehículo (Patente) --%>
                    <div class="col-md-4">
                        <label class="form-label fw-bold">Patente (Opcional)</label>
                        <asp:TextBox ID="txtPatente" runat="server" CssClass="form-control" placeholder="Ej: AB123CD"></asp:TextBox>
                    </div>

                    <%-- Estado de Autorización --%>
                    <div class="col-md-4 d-flex align-items-end">
                        <div class="form-check form-switch mb-2">
                            <asp:CheckBox ID="chkAutorizado" runat="server" CssClass="form-check-input" Checked="true" />
                            <label class="form-check-label fw-bold">Acceso Autorizado</label>
                        </div>
                    </div>

                    <%-- Detalles / Observaciones --%>
                    <div class="col-12">
                        <label class="form-label fw-bold">Detalles/Observaciones</label>
                        <asp:TextBox ID="txtDetalle" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Ej: Ingresa con material de construcción..."></asp:TextBox>
                    </div>

                </div>

                <div class="mt-4 text-end">
                    <asp:Label ID="LbMensaje" runat="server" CssClass="me-3 fw-bold"></asp:Label>
                    <asp:Button ID="Bt_RegistrarMovimiento" runat="server" Text="Guardar Movimiento" CssClass="btn btn-primary btn-lg" OnClick="Bt_RegistrarMovimiento_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
