<%@ Page Title="Movimientos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Movimientos.aspx.cs" Inherits="Grupo7_BarrioCerradoII.Movimientos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%-- CSS de Leaflet --%>
    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.9.4/dist/leaflet.css" />

    <div id="pg-movimientos" class="container my-5">
        <h2 class="mb-4 fw-bold movimiento-titulo-registro">Registrar Movimientos</h2>
        <p class="text-muted">Registre y guarde los movimientos de la persona.</p>

        <div class="card shadow-sm border-0">
            <div class="card-body p-4">
                <div class="row g-4">

                    <%-- Panel DNI --%>
                    <div class="col-md-4">
                        <label class="form-label">DNI del Usuario</label>
                        <div class="input-group">
                            <asp:TextBox ID="Tx_Dni" runat="server" CssClass="form-control" placeholder="Ej: 40123456" OnTextChanged="Tx_Dni_TextChanged" AutoPostBack="true"></asp:TextBox>
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
                        <asp:Label runat="server" AssociatedControlID="ddlLote" CssClass="form-label fw-bold">Lote destino</asp:Label>
                        <asp:DropDownList ID="ddlLote" runat="server" CssClass="form-select">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvLote" runat="server"
                            ControlToValidate="ddlLote"
                            InitialValue=""
                            ErrorMessage="Elegí el lote destino."
                            CssClass="text-danger small"
                            Display="Dynamic" />
                    </div>

                    <%-- Patente --%>
                    <div class="col-md-4">
                        <label class="form-label">Patente (Opcional)</label>
                        <asp:TextBox ID="txtPatente" runat="server" CssClass="form-control" placeholder="Ej: AA691BD"></asp:TextBox>
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

                    <%-- HiddenFields para Coordenadas --%>
                    <asp:HiddenField ID="hfLatitud" runat="server" />
                    <asp:HiddenField ID="hfLongitud" runat="server" />

                    <%-- Mapa de Ubicación del Lote --%>
                    <div class="col-12 mt-3">
                        <div class="d-flex align-items-center justify-content-between mb-2">
                            <label class="form-label fw-bold mb-0">Ubicación del Lote en Mapa</label>
                            <span id="lblCoordenadas" class="text-muted small">Seleccioná un lote para ver su ubicación.</span>
                        </div>
                        <div id="mapaLote" style="height: 320px; width: 100%; border-radius: 8px; border: 1px solid #ccc;"></div>
                    </div>

                </div>

                <%-- Panel de escaneo de DNI por cámara --%>
                <div id="panelScanner" style="display: none; margin-top: 15px; margin-bottom: 15px; text-align: center;">
                    <video id="videoDni" style="width: 100%; max-width: 400px; border: 1px solid #ccc; border-radius: 8px;" autoplay="autoplay" muted="muted" playsinline="playsinline"></video>
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

    <%-- Librerías JS: ZXing (Lectura de DNI) y Leaflet (Mapa) --%>
    <script src="https://cdn.jsdelivr.net/npm/@zxing/library@0.20.0/umd/index.min.js"></script>
    <script src="https://unpkg.com/leaflet@1.9.4/dist/leaflet.js"></script>

    <script type="text/javascript">
        // @ts-nocheck

        // 1. ESCÁNER DE DNI (ZXing - PDF417)
        (function () {
            var codeReader = null;

            function parseCodigoDni(texto) {
                var campos = texto.split('@');
                if (campos.length < 8) return null;
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
                if (codeReader) codeReader.reset();
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
                var constraints = { video: { facingMode: 'environment', width: { ideal: 1920 }, height: { ideal: 1080 } } };

                codeReader.decodeOnceFromConstraints(constraints, 'videoDni').then(function (resultado) {
                    var datos = parseCodigoDni(resultado.getText());
                    if (datos) {
                        document.getElementById('<%= Tx_Dni.ClientID %>').value = datos.dni;
                        document.getElementById('scanStatus').textContent = 'DNI leído correctamente: ' + datos.dni + '.';
                    } else {
                        document.getElementById('scanStatus').textContent = 'No se pudo interpretar el código leído. Cargá el DNI manualmente.';
                    }
                    detenerScanner();
                }).catch(function (err) {
                    console.error(err);
                    document.getElementById('scanStatus').textContent = 'Error al acceder a la cámara.';
                    detenerScanner();
                });
            }

            document.addEventListener('DOMContentLoaded', function () {
                var btnScan = document.getElementById('btnEscanearDni');
                var btnCancel = document.getElementById('btnCancelarScan');
                if (btnScan) btnScan.addEventListener('click', iniciarScanner);
                if (btnCancel) btnCancel.addEventListener('click', detenerScanner);
            });
        })();

        // 2. MAPA LEAFLET Y COORDENADAS POR LOTE
        var mapa = null;
        var marcador = null;
        var ubicacionesLotes = {};

        var latBase = -34.4620;
        var lngBase = -58.8520;

        function generarUbicaciones() {
            var id = 1;
            for (var fila = 0; fila < 10; fila++) {
                for (var col = 0; col < 5; col++) {
                    if (id > 50) break;
                    var lat = latBase - (fila * 0.0006);
                    var lng = lngBase + (col * 0.0008);
                    ubicacionesLotes[id] = { lat: lat, lng: lng };
                    id++;
                }
            }
        }

        document.addEventListener('DOMContentLoaded', function () {
            generarUbicaciones();

            // Inicialización de mapa
            mapa = L.map('mapaLote').setView([latBase, lngBase], 16);

            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                maxZoom: 19,
                attribution: '© OpenStreetMap'
            }).addTo(mapa);

            var ddl = document.getElementById('<%= ddlLote.ClientID %>');
            if (ddl) {
                ddl.addEventListener('change', actualizarMapaPorLote);

                if (ddl.value) {
                    actualizarMapaPorLote();
                }
            }
        });

function actualizarMapaPorLote() {
    var ddl = document.getElementById('<%= ddlLote.ClientID %>');
            var loteSeleccionado = ddl ? ddl.value : '';

            if (loteSeleccionado && ubicacionesLotes[loteSeleccionado]) {
                var coords = ubicacionesLotes[loteSeleccionado];
                mostrarLoteEnMapa(loteSeleccionado, coords.lat, coords.lng);
            } else {
                limpiarMapa();
            }
        }

        function mostrarLoteEnMapa(numeroLote, lat, lng) {
            if (marcador) mapa.removeLayer(marcador);

            marcador = L.marker([lat, lng]).addTo(mapa)
                .bindPopup("<b>Lote " + numeroLote + "</b><br>Lat: " + lat.toFixed(5) + "<br>Lng: " + lng.toFixed(5))
                .openPopup();

            mapa.flyTo([lat, lng], 17);

            document.getElementById('<%= hfLatitud.ClientID %>').value = lat;
            document.getElementById('<%= hfLongitud.ClientID %>').value = lng;
            document.getElementById('lblCoordenadas').textContent = 'Lote ' + numeroLote + ' — Lat: ' + lat.toFixed(5) + ', Lng: ' + lng.toFixed(5);
        }

        function limpiarMapa() {
            if (marcador) {
                mapa.removeLayer(marcador);
                marcador = null;
            }
            mapa.flyTo([latBase, lngBase], 16);
            document.getElementById('<%= hfLatitud.ClientID %>').value = '';
            document.getElementById('<%= hfLongitud.ClientID %>').value = '';
            document.getElementById('lblCoordenadas').textContent = 'Seleccioná un lote para ver su ubicación.';
        }
    </script>
</asp:Content>