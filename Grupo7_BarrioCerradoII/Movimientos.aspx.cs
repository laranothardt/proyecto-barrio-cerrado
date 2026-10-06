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
        protected void Page_Load(object sender, EventArgs e) { }

        protected void Bt_BuscarDNI_Click(object sender, EventArgs e)
        {
            string nombre = new BIZ.Data.Persona().ObtenerNombrePorDni(Tx_Dni.Text.Trim());

            if (nombre == null)
            {
                LbNombre.CssClass = "text-danger small mt-1";
                LbNombre.Text = "No se encontró una persona con ese DNI.";
                return;
            }

            LbNombre.CssClass = "text-success small mt-1";
            LbNombre.Text = nombre;
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
            if (patente != "")
            {
                int idV = BIZ.Data.Vehiculo.ObtenerIdVehiculoPorPatente(patente);
                if (idV == 0)
                {
                    MostrarMensaje("La patente " + patente + " no está registrada.", false);
                    return;
                }
                idVehiculo = idV;
            }

            // 3) Lote (opcional): "Lote 180" -> "180"
            int? idLote = null;
            if (TxLote.Text.Trim() != "")
            {
                string loteNum = new string(TxLote.Text.Where(char.IsDigit).ToArray());
                if (loteNum == "")
                {
                    MostrarMensaje("Ingresá el número de lote (ej: 180).", false);
                    return;
                }

                int idL = new BIZ.Data.Lote().ObtenerIdLotePorNumero(loteNum);
                if (idL == 0)
                {
                    MostrarMensaje("El lote " + loteNum + " no existe.", false);
                    return;
                }
                idLote = idL;
            }

            // 4) Armar el modelo
            string detalle = txtDetalle.Text.Trim();
            if (detalle.Length > 255) detalle = detalle.Substring(0, 255);

            BIZ.Modelo.Movimiento mov = new BIZ.Modelo.Movimiento
            {
                FechaHora = DateTime.Now,
                Tipo = DDLTipoMovimiento.SelectedValue,
                Autorizado = chkAutorizado.Checked,
                Detalle = detalle == "" ? null : detalle,
                IdPersona = idPersona,
                IdVehiculo = idVehiculo,
                IdLoteDestino = idLote,
                IdAcceso = int.Parse(DDLPuntoAcceso.SelectedValue)
            };

            // 5) Guardar
            string error;
            bool ok = new BIZ.Data.Movimiento().CrearMovimiento(mov, out error);

            if (ok)
            {
                MostrarMensaje("Movimiento registrado correctamente.", true);
                Tx_Dni.Text = "";
                TxLote.Text = "";
                txtPatente.Text = "";
                txtDetalle.Text = "";
                LbNombre.Text = "";
            }
            else
            {
                MostrarMensaje(error ?? "No se pudo guardar el movimiento.", false);
            }
        }

        private void MostrarMensaje(string texto, bool exito)
        {
            LbMensaje.CssClass = "me-3 fw-bold " + (exito ? "text-success" : "text-danger");
            LbMensaje.Text = texto;
        }
    }
}