<%@ Page Title="Restricciones" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" 
    CodeBehind="Restricciones.aspx.cs" 
    Inherits="Grupo7_BarrioCerradoII.Restricciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-5">
        <h2 class="mb-2 fw-bold">Panel de Administración</h2>
        <p class="text-muted mb-4">Configuración de horarios de acceso y visibilidad de páginas del sistema.</p>

        <asp:Label ID="lblMensaje" runat="server" CssClass="fw-bold mb-3 d-block"></asp:Label>

        <!-- 1. FRANJAS HORARIAS DE INGRESO -->
        <div class="card shadow-sm border-0 mb-4">
            <div class="card-header bg-white py-3">
                <h5 class="mb-0 fw-bold text-primary">Restricciones Horarias de Ingreso</h5>
            </div>
            <div class="card-body">
                <p class="small text-muted mb-3">Defina en qué horario está permitido el ingreso según el tipo de persona/rol.</p>
                
                <div class="table-responsive">
                    <table class="table align-middle border">
                        <thead class="table-light">
                            <tr>
                                <th>Rol / Tipo de Persona</th>
                                <th>Permitir Ingreso</th>
                                <th>Hora Desde</th>
                                <th>Hora Hasta</th>
                                <th>Días Permitidos</th>
                            </tr>
                        </thead>
                        <tbody>
                            <!-- Residentes -->
                            <tr>
                                <td class="fw-bold">Residentes</td>
                                <td>
                                    <asp:CheckBox ID="chkResidenteHabilitado" runat="server" Checked="true" Text=" Habilitado" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtResidenteDesde" runat="server" TextMode="Time" CssClass="form-control form-control-sm" Text="00:00" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtResidenteHasta" runat="server" TextMode="Time" CssClass="form-control form-control-sm" Text="23:59" />
                                </td>
                                <td><span class="badge bg-success">Todos los días (24/7)</span></td>
                            </tr>

                            <!-- Visitas -->
                            <tr>
                                <td class="fw-bold">Visitas</td>
                                <td>
                                    <asp:CheckBox ID="chkVisitaHabilitado" runat="server" Checked="true" Text=" Habilitado" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVisitaDesde" runat="server" TextMode="Time" CssClass="form-control form-control-sm" Text="08:00" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVisitaHasta" runat="server" TextMode="Time" CssClass="form-control form-control-sm" Text="22:00" />
                                </td>
                                <td><span class="badge bg-secondary">Lunes a Domingo</span></td>
                            </tr>

                            <!-- Proveedores -->
                            <tr>
                                <td class="fw-bold">Proveedores / Obras</td>
                                <td>
                                    <asp:CheckBox ID="chkProveedorHabilitado" runat="server" Checked="true" Text=" Habilitado" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtProveedorDesde" runat="server" TextMode="Time" CssClass="form-control form-control-sm" Text="07:00" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtProveedorHasta" runat="server" TextMode="Time" CssClass="form-control form-control-sm" Text="18:00" />
                                </td>
                                <td><span class="badge bg-warning text-dark">Solo Lunes a Viernes</span></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

        <!-- 2. VISIBILIDAD DE PÁGINAS Y MÓDULOS -->
        <div class="card shadow-sm border-0 mb-4">
            <div class="card-header bg-white py-3">
                <h5 class="mb-0 fw-bold text-primary">Visibilidad y Accesibilidad de Páginas Web</h5>
            </div>
            <div class="card-body">
                <p class="small text-muted mb-3">Marque qué secciones del sitio web estarán visibles y accesibles para cada rol de usuario.</p>

                <div class="table-responsive">
                    <table class="table table-hover border align-middle">
                        <thead class="table-light">
                            <tr>
                                <th>Página / Módulo Web</th>
                                <th class="text-center">Administrador</th>
                                <th class="text-center">Guardia / Seguridad</th>
                                <th class="text-center">Propietario / Residente</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td><code>/Administradores.aspx</code> (Gestión de Administradores)</td>
                                <td class="text-center"><asp:CheckBox ID="chkAdmAdmin" runat="server" Checked="true" Enabled="false" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkAdmGuardia" runat="server" Checked="false" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkAdmResidente" runat="server" Checked="false" /></td>
                            </tr>
                            <tr>
                                <td><code>/Autorizar.aspx</code> (Autorización de Visitas y Entradas)</td>
                                <td class="text-center"><asp:CheckBox ID="chkAutAdmin" runat="server" Checked="true" Enabled="false" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkAutGuardia" runat="server" Checked="true" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkAutResidente" runat="server" Checked="true" /></td>
                            </tr>
                            <tr>
                                <td><code>/IngresoEgreso.aspx</code> (Registro Rápido de Ingreso y Egreso)</td>
                                <td class="text-center"><asp:CheckBox ID="chkIngEgAdmin" runat="server" Checked="true" Enabled="false" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkIngEgGuardia" runat="server" Checked="true" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkIngEgResidente" runat="server" Checked="false" /></td>
                            </tr>
                            <tr>
                                <td><code>/Movimientos.aspx</code> (Historial y Registro de Movimientos)</td>
                                <td class="text-center"><asp:CheckBox ID="chkMovAdmin" runat="server" Checked="true" Enabled="false" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkMovGuardia" runat="server" Checked="true" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkMovResidente" runat="server" Checked="false" /></td>
                            </tr>
                            <tr>
                                <td><code>/Reportes.aspx</code> (Estadísticas e Informes)</td>
                                <td class="text-center"><asp:CheckBox ID="chkRepAdmin" runat="server" Checked="true" Enabled="false" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkRepGuardia" runat="server" Checked="false" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkRepResidente" runat="server" Checked="false" /></td>
                            </tr>
                            <tr>
                                <td><code>/Restricciones.aspx</code> (Configuración de Permisos y Horarios)</td>
                                <td class="text-center"><asp:CheckBox ID="chkResAdmin" runat="server" Checked="true" Enabled="false" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkResGuardia" runat="server" Checked="false" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkResResidente" runat="server" Checked="false" /></td>
                            </tr>
                            <tr>
                                <td><code>/Vehiculos.aspx</code> (Padrón de Vehículos)</td>
                                <td class="text-center"><asp:CheckBox ID="chkVehAdmin" runat="server" Checked="true" Enabled="false" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkVehGuardia" runat="server" Checked="true" /></td>
                                <td class="text-center"><asp:CheckBox ID="chkVehResidente" runat="server" Checked="true" /></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

        <!-- BOTÓN DE GUARDADO -->
        <div class="d-flex justify-content-end">
            <asp:Button ID="btnGuardarConfiguracion" runat="server" Text="Guardar" 
                CssClass="btn btn-primary btn-lg px-4 fw-bold" OnClick="btnGuardarConfiguracion_Click" />
        </div>
    </div>
</asp:Content>
