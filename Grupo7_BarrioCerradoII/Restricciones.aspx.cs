using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Grupo7_BarrioCerradoII
{
    public partial class Restricciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Validación de seguridad (Administrador = 3)
                string rol = Session["Rol"]?.ToString();
                if (rol != "3" && rol != "Administrador")
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }

                // Cargar datos actuales en pantalla
                CargarConfiguracionActual();
            }
        }

        private void CargarConfiguracionActual()
        {
            try
            {
                // Cargar Proveedores
                if (Application["Prov_Desde"] != null)
                {
                    txtProveedorDesde.Text = ((TimeSpan)Application["Prov_Desde"]).ToString(@"hh\:mm");
                    txtProveedorHasta.Text = ((TimeSpan)Application["Prov_Hasta"]).ToString(@"hh\:mm");
                    chkProveedorHabilitado.Checked = (bool)Application["Prov_Habilitado"];
                    MarcarDiasSeleccionados(cblDiasProveedor, (List<string>)Application["Prov_Dias"]);
                }

                // Cargar Visitas
                if (Application["Visita_Desde"] != null)
                {
                    txtVisitaDesde.Text = ((TimeSpan)Application["Visita_Desde"]).ToString(@"hh\:mm");
                    txtVisitaHasta.Text = ((TimeSpan)Application["Visita_Hasta"]).ToString(@"hh\:mm");
                    chkVisitaHabilitado.Checked = (bool)Application["Visita_Habilitado"];
                    MarcarDiasSeleccionados(cblDiasVisita, (List<string>)Application["Visita_Dias"]);
                }

                // Cargar Permisos de Páginas
                if (Application["Permiso_Propietario_Autorizar"] != null)
                {
                    chkAdmProp.Checked = (bool)Application["Permiso_Propietario_Administradores"];
                    chkAutProp.Checked = (bool)Application["Permiso_Propietario_Autorizar"];
                    chkIngProp.Checked = (bool)Application["Permiso_Propietario_IngresoEgreso"];
                    chkMovProp.Checked = (bool)Application["Permiso_Propietario_Movimientos"];

                    chkAdmRes.Checked = (bool)Application["Permiso_Residente_Administradores"];
                    chkAutRes.Checked = (bool)Application["Permiso_Residente_Autorizar"];
                    chkIngRes.Checked = (bool)Application["Permiso_Residente_IngresoEgreso"];
                    chkMovRes.Checked = (bool)Application["Permiso_Residente_Movimientos"];
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar la configuración: " + ex.Message, false);
            }
        }

        protected void btnGuardarConfiguracion_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. CAPTURAR DATOS DEL FORMULARIO
                TimeSpan provDesde = TimeSpan.Parse(txtProveedorDesde.Text);
                TimeSpan provHasta = TimeSpan.Parse(txtProveedorHasta.Text);
                bool provHab = chkProveedorHabilitado.Checked;
                List<string> provDias = ObtenerDiasSeleccionados(cblDiasProveedor);

                TimeSpan visitaDesde = TimeSpan.Parse(txtVisitaDesde.Text);
                TimeSpan visitaHasta = TimeSpan.Parse(txtVisitaHasta.Text);
                bool visitaHab = chkVisitaHabilitado.Checked;
                List<string> visitaDias = ObtenerDiasSeleccionados(cblDiasVisita);


                // 2. GUARDAR EN LA BASE DE DATOS (Capa Data)
                BIZ.Data.RestriccionHoraria dataRestriccion = new BIZ.Data.RestriccionHoraria();

                // IDs de las categorías según BD
                // IDs exactos según tu base de datos
                int idCategoriaVisita = 3;    // Corresponde a 'Visita'[cite: 11]
                int idCategoriaProveedor = 5; // Corresponde a 'Proveedor' (también podés usar 4 si preferís 'Proveedor / Empleado')[cite: 11]

                // A) Sobrescribir Proveedores (ID = 5)
                dataRestriccion.EliminarRestriccionesPorCategoria(idCategoriaProveedor);
                if (provHab)
                {
                    foreach (string dia in provDias)
                    {
                        dataRestriccion.Crear(new BIZ.Modelo.RestriccionHoraria
                        {
                            IdCategoria = idCategoriaProveedor,
                            DiaSemana = dia, // <-- PASAMOS EL TEXTO DIRECTAMENTE
                            HoraDesde = provDesde,
                            HoraHasta = provHasta,
                            VigenteDesde = DateTime.Now.Date,
                            VigenteHasta = new DateTime(2099, 12, 31)
                        });
                    }
                }

                // B) Sobrescribir Visitas (ID = 4)
                dataRestriccion.EliminarRestriccionesPorCategoria(idCategoriaVisita);
                if (visitaHab)
                {
                    foreach (string dia in visitaDias)
                    {
                        dataRestriccion.Crear(new BIZ.Modelo.RestriccionHoraria
                        {
                            IdCategoria = idCategoriaVisita,
                            DiaSemana = dia, // <-- PASAMOS EL TEXTO DIRECTAMENTE
                            HoraDesde = visitaDesde,
                            HoraHasta = visitaHasta,
                            VigenteDesde = DateTime.Now.Date,
                            VigenteHasta = new DateTime(2099, 12, 31)
                        });
                    }
                }

                // 3. ACTUALIZAR CACHÉ (Variables Application) PARA QUE EL SISTEMA REACCIONE AL INSTANTE
                Application["Prov_Desde"] = provDesde;
                Application["Prov_Hasta"] = provHasta;
                Application["Prov_Habilitado"] = provHab;
                Application["Prov_Dias"] = provDias;

                Application["Visita_Desde"] = visitaDesde;
                Application["Visita_Hasta"] = visitaHasta;
                Application["Visita_Habilitado"] = visitaHab;
                Application["Visita_Dias"] = visitaDias;

                // Permisos Propietario (Rol ID = 2)
                Application["Permiso_Propietario_Administradores"] = chkAdmProp.Checked;
                Application["Permiso_Propietario_Autorizar"] = chkAutProp.Checked;
                Application["Permiso_Propietario_IngresoEgreso"] = chkIngProp.Checked;
                Application["Permiso_Propietario_Movimientos"] = chkMovProp.Checked;

                // Permisos Residente (Rol ID = 1)
                Application["Permiso_Residente_Administradores"] = chkAdmRes.Checked;
                Application["Permiso_Residente_Autorizar"] = chkAutRes.Checked;
                Application["Permiso_Residente_IngresoEgreso"] = chkIngRes.Checked;
                Application["Permiso_Residente_Movimientos"] = chkMovRes.Checked;

                MostrarMensaje("Configuración guardada en Base de Datos y aplicada con éxito.", true);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar: " + ex.Message, false);
            }
        }

        // --- MÉTODOS AUXILIARES ---

        private List<string> ObtenerDiasSeleccionados(CheckBoxList cbl)
        {
            List<string> dias = new List<string>();
            foreach (ListItem item in cbl.Items)
            {
                if (item.Selected) dias.Add(item.Value);
            }
            return dias;
        }

        private void MarcarDiasSeleccionados(CheckBoxList cbl, List<string> diasSeleccionados)
        {
            if (diasSeleccionados == null) return;
            foreach (ListItem item in cbl.Items)
            {
                item.Selected = diasSeleccionados.Contains(item.Value);
            }
        }

        // Mapea el string del checkbox al entero del Día de la semana (0=Domingo, 1=Lunes...)
        private int TraducirDiaAInt(string diaIngles)
        {
            switch (diaIngles)
            {
                case "Sunday": return 0;
                case "Monday": return 1;
                case "Tuesday": return 2;
                case "Wednesday": return 3;
                case "Thursday": return 4;
                case "Friday": return 5;
                case "Saturday": return 6;
                default: return 1;
            }
        }

        private void MostrarMensaje(string texto, bool exito)
        {
            lblMensaje.CssClass = "fw-bold mb-3 d-block " + (exito ? "text-success" : "text-danger");
            lblMensaje.Text = texto;
        }
    }
}