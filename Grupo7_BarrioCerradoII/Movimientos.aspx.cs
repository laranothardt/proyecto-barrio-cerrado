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

            // 2) Vehículo (opcional)
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

            // 3) Lote Destino (obligatorio desde el dropdown)
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

            // 4) Detalles y Coordenadas opcionales del mapa
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

            // 5) Modelo de Movimiento
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

            // 6) Guardar en BD
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