<%@ Page Title="Registrarse" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="Grupo7_BarrioCerradoII.Registro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <main id="pg-registro" class="auth-card">
        <div class="login-container">
            <h2 class="login-title">Crear Cuenta</h2>

            <%-- Mensajes de estado --%>
            <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert" Style="margin-bottom: 20px; font-size: 0.95rem; padding: 12px; border-radius: 8px; text-align: left;">
                <asp:Literal ID="litMensaje" runat="server"></asp:Literal>
            </asp:Panel>

            <%--Nombre Completo--%>
            <div class="row g-3">
                <div class="col-md-6 form-group">
                <label class="form-label" for="nombre">Nombre Completo</label>
                <asp:TextBox ID="tx_nombre" runat="server" class="form-control" placeholder="Juan Pérez"></asp:TextBox>
            </div>

            <%--DNI--%>
            <div class="col-md-6 form-group">
                <label class="form-label" for="dni">DNI</label>
                <asp:TextBox ID="tx_dni" runat="server" class="form-control" placeholder="Ej: 30123456"></asp:TextBox>
                <button type="button" id="btnEscanearDni" class="btn btn-outline-secondary btn-sm mt-2">
                    📷 Escanear DNI con cámara
                </button>
            </div>

            <%--Foto Identificatoria--%>
            <div class="col-md-6 form-group">
                <label class="form-label" for="foto">Foto Identificatoria (Escaneo Biométrico)</label>
                <asp:FileUpload ID="file_foto" runat="server" class="form-control" />
            </div>

            <%--Email--%>
            <div class="col-md-6 form-group">
                <label class="form-label" for="email">Correo Electrónico</label>
                <asp:TextBox ID="tx_email" runat="server" class="form-control" placeholder="ejemplo@correo.com"></asp:TextBox>
            </div>

            <%--Contraseña--%>
            <div class="col-md-6 form-group">
                <label class="form-label" for="contraseña">Contraseña</label>
                <asp:TextBox ID="tx_contraseña" runat="server" class="form-control" placeholder="••••••••" TextMode="Password"></asp:TextBox>
            </div>

            <%--Confirmar Contraseña--%>
            <div class="col-md-6 form-group">
                <label class="form-label" for="confirmar_contraseña">Confirmar Contraseña</label>
                <asp:TextBox ID="tx_confirmar_contraseña" runat="server" class="form-control" placeholder="••••••••" TextMode="Password"></asp:TextBox>
            </div>

            <%--Rol--%>
            <div class="col-md-6 form-group">
                <label class="form-label" for="rol">Rol en el Barrio</label>
                <asp:DropDownList ID="ddl_rol" runat="server" class="form-select">
                    <asp:ListItem Value="1" Text="Residente"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Propietario"></asp:ListItem>
                </asp:DropDownList>
            </div>
                </div>

            <%-- Panel de escaneo de DNI por cámara (oculto hasta que se presiona "Escanear DNI") --%>
            <div id="panelScanner" style="display:none; margin-top:15px; margin-bottom:15px; text-align:center;">
                <video id="videoDni" style="width:100%; max-width:400px; border:1px solid #ccc; border-radius:8px;" autoplay muted playsinline=""></video>
                <div id="scanStatus" style="margin-top:8px; font-size:0.9rem;"></div>
                <button type="button" id="btnCancelarScan" class="btn btn-outline-danger btn-sm mt-2">Cancelar</button>
            </div>

            <%--Boton--%>
            <asp:Button ID="bt_registrarse" runat="server" Text="Registrarse" CssClass="btn-submit" OnClick="bt_registrarse_Click" />

            <%-- Iniciar Sesión --%>
            <div class="register-link-container">
                <span>¿Ya tienes una cuenta? </span>
                <asp:LinkButton ID="lnk_login" runat="server" CssClass="btn-register-link" PostBackUrl="~/IniciarSesion.aspx">Inicia Sesión</asp:LinkButton>
            </div>

        </div>
    </main>

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

               // Pedimos mayor resolución a la cámara: el PDF417 es mucho más denso que un QR
               // y con la resolución por defecto del navegador puede no distinguir bien las líneas.
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
                       document.getElementById('<%= tx_dni.ClientID %>').value = datos.dni;
                    document.getElementById('<%= tx_nombre.ClientID %>').value = datos.nombre + ' ' + datos.apellido;
                    document.getElementById('scanStatus').textContent = 'DNI leído correctamente: ' + datos.apellido + ', ' + datos.nombre;
                } else {
                    document.getElementById('scanStatus').textContent = 'No se pudo interpretar el código leído. Cargá los datos manualmente.';
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
