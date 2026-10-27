using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Grupo7_BarrioCerradoII
{
    public partial class Movimientos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PoblarLotes();
                PoblarPatentes(0);
            }
        }

        // Valor centinela del combo: no es un id_vehiculo, despliega el panel de alta
        private const string ValorNuevoVehiculo = "NUEVO";

        /// <summary>
        /// Carga el combo de patentes con los vehiculos de esa persona.
        /// Con idPersona en 0 deja solo "sin vehiculo" y la opcion de cargar uno nuevo.
        /// </summary>
        private void PoblarPatentes(int idPersona)
        {
            DDLPatente.Items.Clear();
            DDLPatente.Items.Add(new ListItem("-- Sin vehículo --", ""));

            if (idPersona != 0)
            {
                System.Data.DataSet ds = BIZ.Data.Vehiculo.ObtenerVehiculosPorPersona(idPersona);

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (System.Data.DataRow fila in ds.Tables[0].Rows)
                    {
                        DDLPatente.Items.Add(new ListItem(
                            fila["Patente"].ToString(),
                            fila["id_vehiculo"].ToString()));
                    }
                }
            }

            DDLPatente.Items.Add(new ListItem("➕ Otro vehículo...", ValorNuevoVehiculo));

            // Al recargar el combo la seleccion vuelve al principio: el panel sobra
            pnlNuevoVehiculo.Visible = false;
        }

        protected void DDLPatente_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlNuevoVehiculo.Visible = DDLPatente.SelectedValue == ValorNuevoVehiculo;
        }

        private void PoblarLotes()
        {
            ddlLote.Items.Clear();
            ddlLote.Items.Add(new ListItem("-- Seleccione un lote --", ""));

            for (int i = 1; i <= 50; i++)
            {
                ddlLote.Items.Add(new ListItem($"Lote {i}", i.ToString()));
            }
        }

        protected void Tx_Dni_TextChanged(object sender, EventArgs e)
        {
            string dni = Tx_Dni.Text.Trim();
            if (string.IsNullOrEmpty(dni)) return;

            // 1. Consultar únicamente si existe en la base de datos (sin insertar)
            BIZ.Data.Persona dataPersona = new BIZ.Data.Persona();
            int idPersona = dataPersona.ObtenerPersonaPorDni(dni, out string nombrePersona);

            // Si el DNI no corresponde a nadie, idPersona es 0 y el combo queda vacio
            PoblarPatentes(idPersona);

            if (idPersona != 0)
            {
                // SI EXISTE: Ocultar panel de datos de persona nueva
                pnlDatosNuevaPersona.Visible = false;

                // Obtener proximo movimiento y lote previo
                new BIZ.Data.Movimiento().ObtenerUltimoMovimiento(idPersona, out string proximoTipo, out string idLotePrevio);

                DDLTipoMovimiento.SelectedValue = proximoTipo;

                if (proximoTipo == "Egreso" && !string.IsNullOrEmpty(idLotePrevio))
                {
                    // Convertimos el IDLote recuperado de la BD al NumeroLote real
                    int idL = int.Parse(idLotePrevio);
                    string numeroLoteReal = new BIZ.Data.Lote().ObtenerNumeroLotePorId(idL);

                    if (string.IsNullOrEmpty(numeroLoteReal))
                    {
                        numeroLoteReal = idLotePrevio; // Fallback de seguridad
                    }

                    ListItem item = ddlLote.Items.FindByValue(numeroLoteReal);
                    if (item != null)
                    {
                        ddlLote.SelectedValue = numeroLoteReal;
                        ScriptManager.RegisterStartupScript(this, GetType(), "ActualizarMapa", "if(typeof actualizarMapaPorLote === 'function') { actualizarMapaPorLote(); }", true);
                    }
                }

                LbNombre.CssClass = "text-success small mt-1";
                LbNombre.Text = $"{nombrePersona} — Se registrará: {proximoTipo}";
            }
            else
            {
                BIZ.Data.PreAcreditacion dataPre = new BIZ.Data.PreAcreditacion();
                var preacreditaciones = dataPre.ObtenerVigentePorDni(dni);

                if (preacreditaciones != null)
                {
                    // A) Crear automáticamente el registro en la tabla Persona con los datos de la preacreditación
                    idPersona = dataPersona.CrearPersona(dni, preacreditaciones.Nombre, preacreditaciones.Apellido);

                    // B) Ocultar el panel de pedir nombre/apellido
                    pnlDatosNuevaPersona.Visible = false;

                    // C) Autocompletar con Ingreso y seleccionar Lote destino preacreditado
                    DDLTipoMovimiento.SelectedValue = "Ingreso";
                    if (!string.IsNullOrEmpty(preacreditaciones.NumeroLote))
                    {
                        ListItem item = ddlLote.Items.FindByValue(preacreditaciones.NumeroLote);
                        if (item != null)
                        {
                            ddlLote.SelectedValue = preacreditaciones.NumeroLote;
                            ScriptManager.RegisterStartupScript(this, GetType(), "ActualizarMapa", "if(typeof actualizarMapaPorLote === 'function') { actualizarMapaPorLote(); }", true);
                        }
                    }

                    // D) Mostrar mensaje de autorización
                    LbNombre.CssClass = "text-success small mt-1";
                    LbNombre.Text = $"✅ {preacreditaciones.Nombre} {preacreditaciones.Apellido} — Autorizado por Preacreditación (Ingreso)";
                }
                else
                {
                    // NO EXISTE EN PERSONA NI EN PREACREDITACIÓN: Mostrar panel para cargar datos manualmente
                    pnlDatosNuevaPersona.Visible = true;
                    DDLTipoMovimiento.SelectedValue = "Ingreso";

                    LbNombre.CssClass = "text-warning small mt-1";
                    LbNombre.Text = "Persona no registrada. Ingrese Nombre y Apellido para continuar.";
                }
            }
        }

        protected void Bt_RegistrarMovimiento_Click(object sender, EventArgs e)
        {
            string dni = Tx_Dni.Text.Trim();
            if (string.IsNullOrEmpty(dni))
            {
                MostrarMensaje("Por favor, ingresá un DNI.", false);
                return;
            }

            BIZ.Data.Persona dataPersona = new BIZ.Data.Persona();
            int idPersona = dataPersona.ObtenerPersonaPorDni(dni, out _);

            // 1) Si no existe en la base de datos, lo creamos ahora exigiendo Nombre y Apellido
            if (idPersona == 0)
            {
                string nombreInput = txtNombrePersona.Text.Trim();
                string apellidoInput = txtApellidoPersona.Text.Trim();

                if (string.IsNullOrEmpty(nombreInput) || string.IsNullOrEmpty(apellidoInput))
                {
                    MostrarMensaje("El Nombre y Apellido son obligatorios para registrar a una persona nueva.", false);
                    return;
                }

                idPersona = dataPersona.CrearPersona(dni, nombreInput, apellidoInput);
                if (idPersona == 0)
                {
                    MostrarMensaje("No se pudo dar de alta a la nueva persona.", false);
                    return;
                }
            }

            // 2) Validaciones Horarias y Días
            string puntoAcceso = DDLPuntoAcceso.SelectedValue;
            string detalleTexto = txtDetalle.Text.Trim().ToLower();
            string categoriaSeleccionada = ddlCategoria.SelectedValue;

            TimeSpan horaActual = DateTime.Now.TimeOfDay;
            string diaActual = DateTime.Now.DayOfWeek.ToString();

            bool esProveedor = categoriaSeleccionada == "4" || puntoAcceso == "2" || detalleTexto.Contains("proveedor") || detalleTexto.Contains("obra");

            if (esProveedor)
            {
                bool habilitado = Application["Prov_Habilitado"] == null || (bool)Application["Prov_Habilitado"];
                TimeSpan desde = Application["Prov_Desde"] != null ? (TimeSpan)Application["Prov_Desde"] : new TimeSpan(7, 0, 0);
                TimeSpan hasta = Application["Prov_Hasta"] != null ? (TimeSpan)Application["Prov_Hasta"] : new TimeSpan(18, 0, 0);
                List<string> diasPermitidos = Application["Prov_Dias"] as List<string>;

                if (!habilitado)
                {
                    MostrarMensaje("El ingreso de proveedores se encuentra deshabilitado.", false);
                    return;
                }

                if (diasPermitidos != null && !diasPermitidos.Contains(diaActual))
                {
                    MostrarMensaje("Los proveedores no tienen permitido ingresar los días " + TraducirDia(diaActual) + ".", false);
                    return;
                }

                if (horaActual < desde || horaActual > hasta)
                {
                    MostrarMensaje($"Ingreso rechazado: el horario permitido de proveedores es de {desde:hh\\:mm} a {hasta:hh\\:mm} hs.", false);
                    return;
                }
            }
            else
            {
                bool habilitado = Application["Visita_Habilitado"] == null || (bool)Application["Visita_Habilitado"];
                TimeSpan desde = Application["Visita_Desde"] != null ? (TimeSpan)Application["Visita_Desde"] : new TimeSpan(8, 0, 0);
                TimeSpan hasta = Application["Visita_Hasta"] != null ? (TimeSpan)Application["Visita_Hasta"] : new TimeSpan(22, 0, 0);
                List<string> diasPermitidos = Application["Visita_Dias"] as List<string>;

                if (!habilitado)
                {
                    MostrarMensaje("El ingreso de visitas está deshabilitado.", false);
                    return;
                }

                if (diasPermitidos != null && !diasPermitidos.Contains(diaActual))
                {
                    MostrarMensaje("Las visitas no tienen permitido ingresar los días " + TraducirDia(diaActual) + ".", false);
                    return;
                }

                if (horaActual < desde || horaActual > hasta)
                {
                    MostrarMensaje($"Ingreso rechazado: el horario de visitas es de {desde:hh\\:mm} a {hasta:hh\\:mm} hs.", false);
                    return;
                }
            }

            // 3) Vehículo (opcional): el combo ya trae el id_vehiculo como value
            int? idVehiculo = null;
            if (DDLPatente.SelectedValue == ValorNuevoVehiculo)
            {
                // Alta rapida de un vehiculo que todavia no esta a nombre de esta persona
                string patenteNueva = txtPatente.Text.Trim();
                string seguroNuevo = txtSeguro.Text.Trim();

                if (string.IsNullOrEmpty(patenteNueva) || string.IsNullOrEmpty(seguroNuevo) || string.IsNullOrEmpty(txtVencimiento.Text))
                {
                    MostrarMensaje("Para cargar un vehículo nuevo completá patente, seguro y vencimiento.", false);
                    return;
                }

                if (!DateTime.TryParse(txtVencimiento.Text, out DateTime vencimiento))
                {
                    MostrarMensaje("La fecha de vencimiento del seguro no es válida.", false);
                    return;
                }

                BIZ.Modelo.Vehiculo nuevoVehiculo = new BIZ.Modelo.Vehiculo
                {
                    Patente = patenteNueva,
                    Seguro = seguroNuevo,
                    VencimientoSeguro = vencimiento,
                    IdPersona = idPersona
                };

                try
                {
                    // Si la patente ya existe a nombre de otro, se reutiliza y se agrega el vinculo
                    BIZ.Data.Vehiculo.AgregarVehiculo(nuevoVehiculo);
                }
                catch (Exception ex)
                {
                    MostrarMensaje("No se pudo registrar el vehículo: " + ex.Message, false);
                    return;
                }

                idVehiculo = BIZ.Data.Vehiculo.ObtenerIdVehiculoPorPatente(patenteNueva);
            }
            else if (!string.IsNullOrEmpty(DDLPatente.SelectedValue))
            {
                idVehiculo = int.Parse(DDLPatente.SelectedValue);
            }

            // 4) Lote Destino (obligatorio)
            int? idLote = null;
            if (!string.IsNullOrEmpty(ddlLote.SelectedValue))
            {
                string loteNum = ddlLote.SelectedValue;
                int idL = new BIZ.Data.Lote().ObtenerIdLotePorNumero(loteNum);
                if (idL == 0)
                {
                    MostrarMensaje("El lote " + loteNum + " no existe en el sistema.", false);
                    return;
                }
                idLote = idL;
            }
            else
            {
                MostrarMensaje("Seleccioná un lote destino.", false);
                return;
            }

            // 5) Ubicación y detalles
            string detalle = txtDetalle.Text.Trim();
            string latitud = hfLatitud.Value;
            string longitud = hfLongitud.Value;

            if (!string.IsNullOrEmpty(latitud) && !string.IsNullOrEmpty(longitud))
            {
                string infoUbicacion = $" [Ubicación: {latitud}, {longitud}]";
                if (detalle.Length + infoUbicacion.Length <= 255)
                {
                    detalle += infoUbicacion;
                }
            }

            if (detalle.Length > 255) detalle = detalle.Substring(0, 255);

            // 6) Crear modelo de movimiento
            BIZ.Modelo.Movimiento mov = new BIZ.Modelo.Movimiento
            {
                FechaHora = DateTime.Now,
                Tipo = DDLTipoMovimiento.SelectedValue,
                Autorizado = chkAutorizado.Checked,
                Detalle = string.IsNullOrEmpty(detalle) ? null : detalle,
                IdPersona = idPersona,
                IdVehiculo = idVehiculo,
                IdLoteDestino = idLote,
                IdAcceso = int.Parse(DDLPuntoAcceso.SelectedValue)
            };

            // 7) Guardar movimiento
            bool ok = new BIZ.Data.Movimiento().CrearMovimiento(mov, out string error);

            if (ok)
            {
                MostrarMensaje("Movimiento registrado correctamente.", true);
                LimpiarFormulario();
            }
            else
            {
                MostrarMensaje(error ?? "No se pudo guardar el movimiento.", false);
            }
        }

        private string TraducirDia(string diaIngles)
        {
            switch (diaIngles)
            {
                case "Monday": return "Lunes";
                case "Tuesday": return "Martes";
                case "Wednesday": return "Miércoles";
                case "Thursday": return "Jueves";
                case "Friday": return "Viernes";
                case "Saturday": return "Sábado";
                case "Sunday": return "Domingo";
                default: return diaIngles;
            }
        }

        private void LimpiarFormulario()
        {
            Tx_Dni.Text = "";
            txtNombrePersona.Text = "";
            txtApellidoPersona.Text = "";
            ddlCategoria.SelectedIndex = 0;
            ddlLote.SelectedIndex = 0;
            PoblarPatentes(0);
            txtPatente.Text = "";
            txtSeguro.Text = "";
            txtVencimiento.Text = "";
            txtDetalle.Text = "";
            LbNombre.Text = "";
            hfLatitud.Value = "";
            hfLongitud.Value = "";
        }

        private void MostrarMensaje(string texto, bool exito)
        {
            LbMensaje.CssClass = "me-3 fw-bold " + (exito ? "text-success" : "text-danger");
            LbMensaje.Text = texto;
        }
    }
}