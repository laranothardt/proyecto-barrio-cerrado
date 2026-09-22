<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Movimientos.aspx.cs" Inherits="Grupo7_BarrioCerradoII.Movimientos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div id="pg-movimientos" class="container my-5">
        <h2 class="mb-4 fw-bold movimiento-titulo-registro">Registrar Movimientos</h2>
        <p class="text-muted">Registre y guarde los movimientos de la persona.</p>

        <div class="card shadow-sm border-0">
            <div class="card-body p-4">
                <div class="row g-4">

                    <%-- Panel DNI  --%>

                    <div class="col-md-4">
                        <label class="form-label">DNI del Usuario</label>
                        <div class="input-group">
                            <asp:TextBox ID="Tx_Dni" runat="server" CssClass="form-control" placeholder="40123456"></asp:TextBox>
                            <asp:Button ID="Bt_BuscarDNI" runat="server" Text="Buscar" CssClass="btn-dni-movimientos" OnClick="Bt_BuscarDNI_Click" />
                        </div>
                        <asp:Label ID="LbNombre" runat="server" CssClass="text-success small mt-1"></asp:Label>
                        <button type="button" id="btnEscanearDni" class="btn btn-outline-secondary btn-sm mt-2">
                            📷 Escanear DNI con cámara
                        </button>
                    </div>

                    <%-- Tipo de Movimiento --%>
                    <div class="col-md-4">
                        <label class="form-label">Tipo de Movimiento</label>
                        <asp:DropDownList ID="DDLTipoMovimiento" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Ingreso" Value="Ingreso" />
                            <asp:ListItem Text="Egreso" Value="Egreso" />
                        </asp:DropDownList>
                    </div>

                    <%-- Panel Punto de Acceso --%>
                    <div class="col-md-4">
                        <label class="form-label">Punto de Acceso</label>
                        <asp:DropDownList ID="DDLPuntoAcceso" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Guardia Principal" Value="1" />
                            <asp:ListItem Text="Acceso Proveedores" Value="2" />
                        </asp:DropDownList>
                    </div>

                    <%-- Lote Destino --%>
                    <div class="col-md-4">
                        <label class="form-label">Lote Destino (Opcional)</label>
                        <asp:TextBox ID="TxLote" runat="server" CssClass="form-control" placeholder="Lote 180"></asp:TextBox>
                    </div>

                    <%-- Patente --%>
                    <div class="col-md-4">
                        <label class="form-label">Patente (Opcional)</label>
                        <asp:TextBox ID="txtPatente" runat="server" CssClass="form-control" placeholder="AA691BD"></asp:TextBox>
                    </div>

                    <%-- Detalles u Observaciones --%>
                    <div class="col-md-4">
                        <label class="form-label">Detalles/Observaciones</label>
                        <asp:TextBox ID="txtDetalle" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Ingresa con material de construcción..."></asp:TextBox>
                    </div>

                    <%-- Estado de Autorización --%>
                    <div class="col-12 d-flex align-items-center mt-2">
                        <asp:CheckBox ID="chkAutorizado" runat="server" Checked="true" Text="Acceso Autorizado" CssClass="custom-checkbox fw-bold text-dark ms-2" />
                    </div>
                </div>

                <%-- Panel de escaneo de DNI por cámara (oculto hasta que se presiona "Escanear DNI") --%>
                <div id="panelScanner" style="display: none; margin-top: 15px; margin-bottom: 15px; text-align: center;">
                    <video id="videoDni" style="width: 100%; max-width: 400px; border: 1px solid #ccc; border-radius: 8px;" autoplay muted playsinline></video>
                    <div id="scanStatus" style="margin-top: 8px; font-size: 0.9rem;"></div>
                    <button type="button" id="btnCancelarScan" class="btn btn-outline-danger btn-sm mt-2">Cancelar</button>
                </div>
            </div>

            <div class="action-footer-right">
                <asp:Label ID="LbMensaje" runat="server" CssClass="me-3 fw-bold"></asp:Label>
                <asp:Button ID="Bt_RegistrarMovimiento" runat="server" Text="Guardar Movimiento" CssClass="btn btn-custom btn-primary-custom btn-lg px-5" OnClick="Bt_RegistrarMovimiento_Click" />
            </div>
        </div>
    </div>

    <%-- Librería para leer códigos de barras PDF417 (formato del DNI argentino) desde la cámara --%>
    <script src="https://cdn.jsdelivr.net/npm/@zxing/library@0.20.0/umd/index.min.js"></script>
    <script type="text/javascript">
        (function () {
            var codeReader = null;

            // El código de barras del DNI argentino trae los datos separados por "@":
            // NroTramite@Apellido@Nombre@Sexo@DNI@Ejemplar@FechaNacimiento@FechaEmision@CUIL
            function parseCodigoDni(texto) {
                var campos = texto.split('@');
                if (campos.length < 8) {
                    return null;
                }
                return {
                    nroTramite: campos[0],
                    apellido: campos[1],
                    nombre: campos[2],
                    sexo: campos[3],
                    dni: campos[4],
                    ejemplar: campos[5],
                    fechaNacimiento: campos[6],
                    fechaEmision: campos[7],
                    cuil: campos.length > 8 ? campos[8] : null
                };
            }

            function detenerScanner() {
                if (codeReader) {
                    codeReader.reset();
                }
                document.getElementById('panelScanner').style.display = 'none';
            }

            function iniciarScanner() {
                if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
                    alert('Este navegador no soporta acceso a la cámara. Probá con Chrome o Edge actualizados.');
                    return;
                }

                document.getElementById('panelScanner').style.display = 'block';
                document.getElementById('scanStatus').textContent = 'Apuntá la cámara al código de barras del DNI...';

                codeReader = new ZXing.BrowserPDF417Reader();

                var constraints = {
                    video: {
                        facingMode: 'environment',
                        width: { ideal: 1920 },
                        height: { ideal: 1080 }
                    }
                };

                codeReader.decodeOnceFromConstraints(constraints, 'videoDni').then(function (resultado) {
                    var datos = parseCodigoDni(resultado.getText());

                    if (datos) {
                        document.getElementById('<%= Tx_Dni.ClientID %>').value = datos.dni;
                        document.getElementById('scanStatus').textContent = 'DNI leído correctamente: ' + datos.dni + '. Presioná "Buscar" para completar el nombre.';
                    } else {
                        document.getElementById('scanStatus').textContent = 'No se pudo interpretar el código leído. Cargá el DNI manualmente.';
                    }

                    detenerScanner();
                }).catch(function (err) {
                    console.error(err);
                    var msg = 'No se pudo acceder a la cámara.';
                    if (err && err.name === 'NotAllowedError') {
                        msg = 'Permiso de cámara denegado. Habilitalo en el navegador e intentá de nuevo.';
                    } else if (err && err.name === 'NotFoundError') {
                        msg = 'No se encontró ninguna cámara en este dispositivo.';
                    } else if (err && err.name === 'OverconstrainedError') {
                        msg = 'La cámara no soporta la resolución pedida. Probá sin restricción de resolución.';
                    }
                    document.getElementById('scanStatus').textContent = msg;
                    detenerScanner();
                });
            }

            document.addEventListener('DOMContentLoaded', function () {
                document.getElementById('btnEscanearDni').addEventListener('click', iniciarScanner);
                document.getElementById('btnCancelarScan').addEventListener('click', detenerScanner);
            });
        })();
    </script>

</asp:Content>
