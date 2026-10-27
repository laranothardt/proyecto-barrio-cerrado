<%@ Page Title="Restricciones" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Restricciones.aspx.cs" Inherits="Grupo7_BarrioCerradoII.Restricciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="pg-restricciones" class="container my-5">
        <h2 class="mb-2 fw-bold">Panel de Administración</h2>
        <p class="text-muted mb-4">Configuración de horarios de acceso y visibilidad de páginas del sistema.</p>

        <asp:Label ID="lblMensaje" runat="server" CssClass="fw-bold mb-3 d-block"></asp:Label>

        <!-- 1. FRANJAS HORARIAS Y DÍAS DE INGRESO -->
        <div class="card shadow-sm border-0 mb-4">
            <div class="card-header bg-white py-3">
                <h5 class="mb-0 fw-bold card-titulo">Restricciones Horarias de Ingreso</h5>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <table class="table align-middle border">
                        <thead class="table-light">
                            <tr>
                                <th>Tipo de Persona</th>
                                <th>Permitido</th>
                                <th>Hora Desde</th>
                                <th>Hora Hasta</th>
                                <th>Días Permitidos</th>
                            </tr>
                        </thead>
                        <tbody>
                            <!-- Residentes -->
                            <tr>
                                <td class="fw-bold">Residentes</td>
                                <td><asp:CheckBox ID="chkResidenteHabilitado" runat="server" Checked="true" /></td>
                                <td><asp:TextBox ID="txtResidenteDesde" runat="server" TextMode="Time" CssClass="form-control form-control-sm" Text="00:00" /></td>
                                <td><asp:TextBox ID="txtResidenteHasta" runat="server" TextMode="Time" CssClass="form-control form-control-sm" Text="23:59" /></td>
                                <td>
                                    <asp:CheckBoxList ID="cblDiasResidente" runat="server" RepeatDirection="Horizontal" CssClass="d-flex gap-3 small">
                                        <asp:ListItem Text="L" Value="Monday" Selected="True" />
                                        <asp:ListItem Text="M" Value="Tuesday" Selected="True" />
                                        <asp:ListItem Text="X" Value="Wednesday" Selected="True" />
                                        <asp:ListItem Text="J" Value="Thursday" Selected="True" />
                                        <asp:ListItem Text="V" Value="Friday" Selected="True" />
                                        <asp:ListItem Text="S" Value="Saturday" Selected="True" />
                                        <asp:ListItem Text="D" Value="Sunday" Selected="True" />
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <!-- Visitas -->
                            <tr>
                                <td class="fw-bold">Visitas</td>
                                <td><asp:CheckBox ID="chkVisitaHabilitado" runat="server" Checked="true" /></td>
                                <td><asp:TextBox ID="txtVisitaDesde" runat="server" TextMode="Time" CssClass="form-control form-control-sm" Text="08:00" /></td>
                                <td><asp:TextBox ID="txtVisitaHasta" runat="server" TextMode="Time" CssClass="form-control form-control-sm" Text="22:00" /></td>
                                <td>
                                    <asp:CheckBoxList ID="cblDiasVisita" runat="server" RepeatDirection="Horizontal" CssClass="d-flex gap-3 small">
                                        <asp:ListItem Text="L" Value="Monday" Selected="True" />
                                        <asp:ListItem Text="M" Value="Tuesday" Selected="True" />
                                        <asp:ListItem Text="X" Value="Wednesday" Selected="True" />
                                        <asp:ListItem Text="J" Value="Thursday" Selected="True" />
                                        <asp:ListItem Text="V" Value="Friday" Selected="True" />
                                        <asp:ListItem Text="S" Value="Saturday" Selected="True" />
                                        <asp:ListItem Text="D" Value="Sunday" Selected="True" />
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <!-- Proveedores -->
                            <tr>
                                <td class="fw-bold">Proveedores</td>
                                <td><asp:CheckBox ID="chkProveedorHabilitado" runat="server" Checked="true" /></td>
                                <td><asp:TextBox ID="txtProveedorDesde" runat="server" TextMode="Time" CssClass="form-control form-control-sm" Text="07:00" /></td>
                                <td><asp:TextBox ID="txtProveedorHasta" runat="server" TextMode="Time" CssClass="form-control form-control-sm" Text="18:00" /></td>
                                <td>
                                    <asp:CheckBoxList ID="cblDiasProveedor" runat="server" RepeatDirection="Horizontal" CssClass="d-flex gap-3 small">
                                        <asp:ListItem Text="L" Value="Monday" Selected="True" />
                                        <asp:ListItem Text="M" Value="Tuesday" Selected="True" />
                                        <asp:ListItem Text="X" Value="Wednesday" Selected="True" />
                                        <asp:ListItem Text="J" Value="Thursday" Selected="True" />
                                        <asp:ListItem Text="V" Value="Friday" Selected="True" />
                                        <asp:ListItem Text="S" Value="Saturday" />
                                        <asp:ListItem Text="D" Value="Sunday" />
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

        <!-- 2. VISIBILIDAD Y ACCESIBILIDAD -->
        <div class="card shadow-sm border-0 mb-4">
            <div class="card-header bg-white py-3">
                <h5 class="mb-0 fw-bold card-titulo">Visibilidad y Accesibilidad de Páginas Web</h5>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <table class="table table-hover border align-middle">
                        <thead class="table-light">
                            <tr>
                                <th>Página / Módulo Web</th>
                                <th class="text-center">Administrador</th>
                                <th class="text-center">Propietario</th>
                                <th class="text-center">Residente</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td><code>/Administradores.aspx</code></td>
                                <td class="text-center"><asp:CheckBox ID="chkAdmAdmin" runat="server" Checked="true" Enabled="false" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkAdmProp" runat="server" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkAdmRes" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><code>/Autorizar.aspx</code></td>
                                <td class="text-center"><asp:CheckBox ID="chkAutAdmin" runat="server" Checked="true" Enabled="false" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkAutProp" runat="server" Checked="true" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkAutRes" runat="server" Checked="true" /></td>
                            </tr>
                            <tr>
                                <td><code>/IngresoEgreso.aspx</code></td>
                                <td class="text-center"><asp:CheckBox ID="chkIngAdmin" runat="server" Checked="true" Enabled="false" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkIngProp" runat="server" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkIngRes" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><code>/Movimientos.aspx</code></td>
                                <td class="text-center"><asp:CheckBox ID="chkMovAdmin" runat="server" Checked="true" Enabled="false" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkMovProp" runat="server" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkMovRes" runat="server" /></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

        <div class="d-flex justify-content-end">
            <asp:Button ID="btnGuardarConfiguracion" runat="server" Text="Guardar Configuración" CssClass="btn-custom btn-primary-custom" OnClick="btnGuardarConfiguracion_Click" />
        </div>
    </div>
</asp:Content>