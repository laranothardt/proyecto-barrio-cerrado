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
            }
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

            string nombre = new BIZ.Data.Persona().ObtenerNombrePorDni(dni);

            if (nombre == null)
            {
                LbNombre.CssClass = "text-danger small mt-1";
                LbNombre.Text = "No se encontró una persona con ese DNI.";
                return;
            }

            int idPersona = new BIZ.Data.Persona().ObtenerIdPersonaPorDni(dni);
            string proximoTipo = new BIZ.Data.Movimiento().ObtenerProximoTipoMovimiento(idPersona);

            DDLTipoMovimiento.SelectedValue = proximoTipo;

            LbNombre.CssClass = "text-success small mt-1";
            LbNombre.Text = nombre + " — se registrará un " + proximoTipo;
        }

        protected void Bt_RegistrarMovimiento_Click(object sender, EventArgs e)
        {
            // 1) Persona (obligatoria)
            int idPersona = new BIZ.Data.Persona().ObtenerIdPersonaPorDni(Tx_Dni.Text.Trim());
            if (idPersona == 0)
            {
                MostrarMensaje("Ingresá un DNI válido y registrado.", false);
                return;
            }

            // 2) Validar Restricciones Horarias y de Días
            string puntoAcceso = DDLPuntoAcceso.SelectedValue; // "1" = Guardia Principal, "2" = Acceso Proveedores
            string detalleTexto = txtDetalle.Text.Trim().ToLower();

            TimeSpan horaActual = DateTime.Now.TimeOfDay;
            string diaActual = DateTime.Now.DayOfWeek.ToString(); // Ej: "Monday", "Tuesday", etc.

            // Evaluar si es Proveedor
            bool esProveedor = puntoAcceso == "2" || detalleTexto.Contains("proveedor") || detalleTexto.Contains("obra");

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
                // Para visitas en general
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

            // 3) Vehículo (opcional)
            int? idVehiculo = null;
            string patente = txtPatente.Text.Trim();
            if (!string.IsNullOrEmpty(patente))
            {
                int idV = BIZ.Data.Vehiculo.ObtenerIdVehiculoPorPatente(patente);
                if (idV == 0)
                {
                    MostrarMensaje("La patente " + patente + " no está registrada.", false);
                    return;
                }
                idVehiculo = idV;
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

            // 5) Detalles y Coordenadas opcionales del mapa
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

            // 6) Modelo de Movimiento
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

            // 7) Guardar en BD
            string error;
            bool ok = new BIZ.Data.Movimiento().CrearMovimiento(mov, out error);

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
            ddlLote.SelectedIndex = 0;
            txtPatente.Text = "";
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