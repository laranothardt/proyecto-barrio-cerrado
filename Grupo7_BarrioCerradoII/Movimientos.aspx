<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Movimientos.aspx.cs" Inherits="Grupo7_BarrioCerradoII.Movimientos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div id="pg-movimientos" class="container my-5">
        <h2 class="mb-4 fw-bold text-center movimiento-titulo-registro">Registrar Movimientos</h2>

        <div class="card shadow-sm border-0">
            <div class="card-body p-4">
                <div class="row g-4">

                    <%-- Panel DNI  --%>

                    <div class="col-md-4">
                        <label class="form-label fw-bold text-secondary">DNI del Usuario</label>
                        <div class="input-group">
                            <asp:TextBox ID="Tx_Dni" runat="server" CssClass="form-control" placeholder="40123456"></asp:TextBox>
                            <asp:Button ID="Bt_BuscarDNI" runat="server" Text="Buscar" CssClass="btn btn-custom btn-primary-custom" OnClick="Bt_BuscarDNI_Click" />
                        </div>
                        <asp:Label ID="LbNombre" runat="server" CssClass="text-success small mt-1"></asp:Label>
                    </div>

                    <%-- Tipo de Movimiento --%>
                    <div class="col-md-4">
                        <label class="form-label fw-bold text-secondary">Tipo de Movimiento</label>
                        <asp:DropDownList ID="DDLTipoMovimiento" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Ingreso" Value="Ingreso" />
                            <asp:ListItem Text="Egreso" Value="Egreso" />
                        </asp:DropDownList>
                    </div>

                    <%-- Panel Punto de Acceso --%>
                    <div class="col-md-4">
                        <label class="form-label fw-bold text-secondary">Punto de Acceso</label>
                        <asp:DropDownList ID="DDLPuntoAcceso" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Guardia Principal" Value="1" />
                            <asp:ListItem Text="Acceso Proveedores" Value="2" />
                        </asp:DropDownList>
                    </div>

                    <%-- Lote Destino --%>
                    <div class="col-md-4">
                        <label class="form-label fw-bold text-secondary">Lote Destino (Opcional)</label>
                        <asp:TextBox ID="TxLote" runat="server" CssClass="form-control" placeholder="Lote 180"></asp:TextBox>
                    </div>

                    <%-- Patente --%>
                    <div class="col-md-4">
                        <label class="form-label fw-bold text-secondary">Patente (Opcional)</label>
                        <asp:TextBox ID="txtPatente" runat="server" CssClass="form-control" placeholder="AA691BD"></asp:TextBox>
                    </div>

                    <%-- Detalles u Observaciones --%>
                    <div class="col-md-4">
                        <label class="form-label fw-bold text-secondary">Detalles/Observaciones</label>
                        <asp:TextBox ID="txtDetalle" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Ingresa con material de construcción..."></asp:TextBox>
                    </div>

                    <%-- Estado de Autorización --%>
                    <div class="col-12 d-flex align-items-center mt-2">
                        <asp:CheckBox ID="chkAutorizado" runat="server" Checked="true" Text="Acceso Autorizado" CssClass="custom-checkbox fw-bold text-dark ms-2" />
                    </div>
                </div> 
            </div> 
            
            <div class="action-footer-right">
                <asp:Label ID="LbMensaje" runat="server" CssClass="me-3 fw-bold"></asp:Label>
                <asp:Button ID="Bt_RegistrarMovimiento" runat="server" Text="Guardar Movimiento" CssClass="btn btn-custom btn-primary-custom btn-lg px-5" OnClick="Bt_RegistrarMovimiento_Click" />
            </div>
        </div>
    </div>

</asp:Content>
