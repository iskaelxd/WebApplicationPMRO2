
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplicationPMRO2.Utilities;
using WebApplicationPMRO2.Pages.Produccion;

namespace WebApplicationPMRO2.Pages.Almacen
{
    public partial class Solicitudes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropdownArea();
                LoadDropdownStatus();
                LoadDropdownLinea();
                LoadData();
            }
        }


        private void LoadDropdownArea()
        {
            FuncionesMes.LlenarDropDownList(
                ddlArea,
                "[dbo].[SP_IndirectMaterials_Area]",
                new[] { "@TransactionCode" },
                new[] { "S" },
                "Todas las Areas",
                "0",
                "AreaName",
                "AreaId"
            );
        }


        private void LoadDropdownStatus()
        {
            FuncionesMes.LlenarDropDownList(
                ddlStatus,
                "[dbo].[SP_IndirectMaterials_Status]",
                new[] { "@TransactionCode" },
                new[] { "S" },
                "Todos los Status",
                "0",
                "StatusCode",
                "StatusId"
            );
        }

        private void LoadDropdownLinea()
        {
            FuncionesMes.LlenarDropDownList(
                ddlLinea,
                "[dbo].[SP_IndirectMaterials_Line]",
                new[] { "@TransactionCode" },
                new[] { "S" },
                "Todos las lineas",
                "0",
                "LineShortName",
                "LineId"
            );
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlArea.SelectedValue == "13")
            {
                linea.Visible = true;
            }
            else
            {
                linea.Visible = false;
                ddlLinea.SelectedIndex = 0; // Reiniciar el dropdown de línea si no es necesario
            }
            LoadData(); // Recargar la tabla de órdenes al cambiar el área
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            // Aquí puedes implementar la lógica para buscar solicitudes basadas en los filtros seleccionados
            // Por ejemplo, podrías llamar a un método que recupere y muestre las solicitudes filtradas
            LoadData();
        }

        protected void LoadData()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("IdOrder", typeof(int));
                dt.Columns.Add("AreaOrLine", typeof(string));
                dt.Columns.Add("StatusId", typeof(string));
                dt.Columns.Add("UpdatedOn", typeof(string));
                dt.Columns.Add("UpdatedBy", typeof(string));

                string areaId = ddlArea.SelectedValue == "0" ? null : ddlArea.SelectedValue;
                string lineId = ddlLinea.SelectedValue == "0" ? null : ddlLinea.SelectedValue;
                string statusId = ddlStatus.SelectedValue == "0" ? null : ddlStatus.SelectedValue;

                // --- NUEVO: interpretar el texto de búsqueda ---
                string raw = txtBuscarOrden.Text.Trim();


                // Armamos los parámetros (mantén el orden de nombres/valores)
                var paramNames = new List<string> { "@TransactionCode", "@AreaId", "@LineId", "@StatusId", "@Search" };
                var paramValues = new List<string> { "S", areaId, lineId, statusId, raw };

                using (SqlDataReader reader = FuncionesMes.ExecuteReader(
                    "[dbo].[SP_IndirectMaterials_OrderHeader]",
                    paramNames.ToArray(),
                    paramValues.ToArray()))
                {
                    if (reader == null)
                    {
                        Funciones.MostrarToast("Error al obtener la información: ", "danger", "top-0 end-0", 3000);
                        return;
                    }

                    while (reader.Read())
                    {
                        string AreaOrLine;
                        if (reader["AreaId"].ToString() == "13")
                            AreaOrLine = reader["LineShortName"].ToString();
                        else
                            AreaOrLine = reader["AreaName"].ToString();

                        dt.Rows.Add(
                            reader["OrderHeaderId"],
                            AreaOrLine,
                            reader["StatusCode"],
                            reader["UpdatedOn"],
                            reader["UpdatedBy"]
                        );
                    }

                    tblorder.DataSource = dt;
                    tblorder.DataBind();
                }
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast($"Error al cargar los datos: {ex.Message}", "danger", "top-0 end-0", 3000);
            }
        }



        protected void tblorder_PreRender(object sender, EventArgs e)
        {
            if (tblorder.HeaderRow != null)
            {
                tblorder.HeaderRow.TableSection = TableRowSection.TableHeader;
                // aplica la clase 'sticky' al thead
                tblorder.HeaderRow.CssClass = (tblorder.HeaderRow.CssClass + " sticky").Trim();
            }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlLinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData(); // Recargar la tabla de órdenes al cambiar la línea
        }

        private void MostrarListado() => mvMain.ActiveViewIndex = 0;
        private void MostrarDetalle() => mvMain.ActiveViewIndex = 1;

        protected void tblorder_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Consultar")
            {
                //int orderId = Convert.ToInt32(e.CommandArgument);

                int orderId = Convert.ToInt32(e.CommandArgument); // Asegúrate de que la celda 0 tenga el ID

                Funciones.MostrarToast($"Cargando detalles de la orden {orderId}...", "info", "top-0 end-0", 2000);


                //ver si el estatus de la orden si es CREADA





                // Limpiar el campo oculto para el ID de la orden
                hdnOrderId.Value = string.Empty; // Limpiar antes de asignar un nuevo valor
                hdnOrderId.Value = orderId.ToString();

                CargarEncabezado(orderId);
                CargarDetalle(orderId);

                MostrarDetalle();
            }
        }


        private void CargarEncabezado(int orderId)
        {
            try
            {
                string status = null;

                using (SqlDataReader reader = FuncionesMes.ExecuteReader(
                    "[dbo].[SP_IndirectMaterials_OrderHeader]",
                    new[] { "@TransactionCode", "@OrderHeaderId" },
                    new[] { "S", orderId.ToString() }))
                {
                    if (reader != null && reader.Read())
                    {

                       lblOrderId.Text = reader["OrderHeaderId"].ToString();

                        status = reader["StatusId"].ToString();

                        string areaOrLine = reader["AreaId"].ToString() == "13"
                            ? reader["LineShortName"].ToString()
                            : reader["AreaName"].ToString();

                        lblAreaLinea.Text = areaOrLine;
                        lblSolicitadoPor.Text = reader["UpdatedBy"].ToString();
                    }
                    else
                    {
                        Funciones.MostrarToast("No se encontró la orden.", "warning", "top-0 end-0", 3000);
                        MostrarListado();
                    }
                }

                if(status == "1")
                {
                    using (SqlDataReader reader = FuncionesMes.ExecuteReader(
                   "[dbo].[SP_IndirectMaterials_OrderHeader]",
                   new[] { "@TransactionCode", "@OrderHeaderId", "@StatusId" },
                   new[] { "U", orderId.ToString(),"3" })) { }
                }else if(status == "5")
                {
                    btnListo.Visible = false;
                    btnSinInv.Visible = false;
                    gvDetalle.Enabled = false; // Deshabilitar la grilla para evitar cambios
                }


            }
            catch (Exception ex)
            {
                Funciones.MostrarToast($"Error al cargar encabezado: {ex.Message}", "danger", "top-0 end-0", 3000);
                MostrarListado();
            }
        }


        private void CargarDetalle(int orderId)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("OrderDetailId", typeof(long));   // <-- NUEVO (DataKey)
                dt.Columns.Add("PartNumb", typeof(string));
                dt.Columns.Add("PartDescription", typeof(string));
                dt.Columns.Add("OrderQnty", typeof(int));
                dt.Columns.Add("Disponible", typeof(int));
                dt.Columns.Add("NombreLocation", typeof(string));
                dt.Columns.Add("Estado", typeof(string));
                dt.Columns.Add("CssEstado", typeof(string));
                dt.Columns.Add("FaltanTexto", typeof(string));
                dt.Columns.Add("Marcado", typeof(bool));         // <-- NUEVO
                dt.Columns.Add("Habilitado", typeof(bool));      // <-- NUEVO (para Enable del checkbox)
                dt.Columns.Add("RowCss", typeof(string));        // <-- NUEVO (pintar fila)

                using (SqlDataReader reader = FuncionesMes.ExecuteReader(
                    "[dbo].[SP_IndirectMaterials_OrderDetail]",
                    new[] { "@TransactionCode", "@OrderHeaderId" },
                    new[] { "S", orderId.ToString() }))
                {
                    if (reader == null)
                    {
                        Funciones.MostrarToast("Error al obtener detalles.", "danger", "top-0 end-0", 3000);
                        return;
                    }

                    while (reader.Read())
                    {
                        int qty = Convert.ToInt32(reader["OrderQnty"]);
                        int disp = 0; int.TryParse((reader["Inventory"] ?? "0").ToString(), out disp);

                        bool ok = qty <= disp;
                        string estado = ok ? "OK" : "Insuficiente";
                        string cssEstado = ok ? "badge bg-success" : "badge bg-danger";
                        string faltanTxt = ok ? "" : $"Faltan {qty - disp}";

                        bool marcado = false;
                        if (reader["Marcado"] != DBNull.Value)
                            marcado = Convert.ToBoolean(reader["Marcado"]);

                        // Pintado de fila:
                        // - Si ya está marcado -> verde
                        // - Si está insuficiente -> rojo
                        string rowCss = marcado ? "table-success" : (ok ? "" : "table-danger");

                        dt.Rows.Add(
                            Convert.ToInt64(reader["OrderDetailId"]),
                            reader["PartNumb"].ToString(),
                            reader["PartDescription"].ToString(),
                            qty,
                            disp,
                            reader["NombreLocation"].ToString(),
                            estado,
                            cssEstado,
                            faltanTxt,
                            marcado,
                            ok,        // Habilitado solo si hay inventario suficiente
                            rowCss
                        );
                    }

                    gvDetalle.DataSource = dt;
                    gvDetalle.DataBind();
                }
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast($"Error al cargar detalles: {ex.Message}", "danger", "top-0 end-0", 3000);
            }
        }


        protected void gvDetalle_PreRender(object sender, EventArgs e)
        {
            if (gvDetalle.HeaderRow != null)
            {
                gvDetalle.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvDetalle.HeaderRow.CssClass = (gvDetalle.HeaderRow.CssClass + " sticky").Trim();
            }
        }




        protected void gvDetalle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var rowCss = DataBinder.Eval(e.Row.DataItem, "RowCss")?.ToString();
                if (!string.IsNullOrEmpty(rowCss))
                    e.Row.CssClass = (e.Row.CssClass + " " + rowCss).Trim();
            }
        }


        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            // Si quieres refrescar el listado para reflejar cambios:
            LoadData();
            MostrarListado();
            btnListo.Visible = true; // Asegúrate de mostrar el botón de listo al regresar
            btnSinInv.Visible = true; // Asegúrate de mostrar el botón de sin inventario al regresar
            gvDetalle.Enabled = true; // Deshabilitar la grilla para evitar cambios
        }

        protected void btnListo_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: sustituye "X_READY" por el StatusId/StatusCode real
                // Ejemplo usando StatusId:
                // FuncionesMes.ExecuteNonQuery("[dbo].[SP_IndirectMaterials_OrderHeader]",
                //     new[] { "@TransactionCode", "@OrderHeaderId", "@StatusId", "@UpdatedOn", "@UpdatedBy" },
                //     new[] { "U", hdnOrderId.Value, "X_READY", DateTime.Now.ToString("s"), User.Identity.Name });

                Funciones.MostrarToast("Orden marcada como 'Listo para recoger'.", "success", "top-0 end-0", 2500);
                // Si quieres recargar detalle después del cambio:
               // CargarEncabezado(int.Parse(hdnOrderId.Value));
                CargarDetalle(int.Parse(hdnOrderId.Value));
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast($"Error: {ex.Message}", "danger", "top-0 end-0", 3000);
            }
        }

        protected void btnSinInv_Click(object sender, EventArgs e)
        {
            try
            {
           // TODO: sustituye "X_NO_STOCK" por el StatusId / StatusCode real
                 FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_OrderHeader]",
                     new[] { "@TransactionCode", "@OrderHeaderId", "@StatusId"},
                     new[] { "U", hdnOrderId.Value, "5" });

                Funciones.MostrarToast("Orden marcada como 'Sin inventario'.", "warning", "top-0 end-0", 2500);

                btnSinInv.Visible = false; // Ocultar botón después de marcar
                btnListo.Visible = false; // Ocultar botón de listo también 
                                          // CargarEncabezado(int.Parse(hdnOrderId.Value));

                gvDetalle.Enabled = false; // Deshabilitar la grilla para evitar cambios
                CargarDetalle(int.Parse(hdnOrderId.Value));
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast($"Error: {ex.Message}", "danger", "top-0 end-0", 3000);
            }
        }

        protected void btnGuardarMarcados_Click(object sender, EventArgs e)
        {
            try
            {
                string usuario = (HttpContext.Current?.User?.Identity?.IsAuthenticated ?? false)
                    ? HttpContext.Current.User.Identity.Name
                    : "Sistema";

                int actualizados = 0;

                foreach (GridViewRow row in gvDetalle.Rows)
                {
                    if (row.RowType != DataControlRowType.DataRow) continue;

                    long orderDetailId = Convert.ToInt64(gvDetalle.DataKeys[row.RowIndex].Value);
                    var chk = row.FindControl("chkEntregar") as CheckBox;
                    if (chk == null) continue;

                    // No intentes guardar si no está habilitado (insuficiente inventario)
                    if (!chk.Enabled) continue;

                    // Marcado = 1 si checked, 0 si no
                    string marcado = chk.Checked ? "1" : "0";

                    // Llama a tu SP de detalle en modo Update
                    // Ajusta si tu helper requiere tipos distintos; aquí asumo ExecuteNonQuery con pares nombre/valor:
                    FuncionesMes.ExecuteReader(
                        "[dbo].[SP_IndirectMaterials_OrderDetail]",
                        new[] { "@TransactionCode", "@OrderDetailId", "@Marcado", "@UpdatedOn", "@UpdatedBy" },
                        new[] { "U", orderDetailId.ToString(), marcado, DateTime.Now.ToString("s"), usuario }
                    );

                    actualizados++;
                }

                Funciones.MostrarToast($"Seleccion guardada. Renglones procesados: {actualizados}.", "success", "top-0 end-0", 3000);

                // Recargar para refrescar colores y estados
                CargarDetalle(int.Parse(hdnOrderId.Value));
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast($"Error al guardar selección: {ex.Message}", "danger", "top-0 end-0", 3000);
            }
        }


    }//end
}