using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using WebApplicationPMRO2.Utilities;

namespace WebApplicationPMRO2.Pages.Almacen
{
    public partial class Movimientos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindGrid();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            gvPartes.PageIndex = 0;
            BindGrid();
        }

        protected void gvPartes_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvPartes.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        private void BindGrid()
        {
            string search = txtPartNumb.Text?.Trim();
            object updatedBy = (Session["globalId"] != null) ? (object)Session["globalId"].ToString() : DBNull.Value;

            // DataTable de resultado (PartNumb, PartDescription, Total)
            var dt = new DataTable();
            dt.Columns.Add("PartNumb", typeof(string));
            dt.Columns.Add("PartDescription", typeof(string));
            dt.Columns.Add("Total", typeof(int));

            // Llamada al SP en modo R (reporte agrupado)
            string[] names = { "@TransactionCode", "@PartNumb", "@PartDescription", "@UpdatedBy" };
            object[] vals = {
                "T",
                string.IsNullOrWhiteSpace(search) ? (object)DBNull.Value : search,
                string.IsNullOrWhiteSpace(search) ? (object)DBNull.Value : search,
                updatedBy
            };

            using (SqlDataReader reader = Funciones.ExecuteReader("[dbo].[SP_IndirectMaterials_OrderDetail]", names, vals))
            {
                if (reader != null)
                {
                    int ordPart = -1, ordDesc = -1, ordTot = -1;
                    try
                    {
                        ordPart = reader.GetOrdinal("PartNumb");
                        ordDesc = reader.GetOrdinal("PartDescription");
                        ordTot = reader.GetOrdinal("Total");
                    }
                    catch
                    {
                        // Si los nombres no coinciden, leemos por índice defensivamente
                    }

                    while (reader.Read())
                    {
                        string part = ordPart >= 0 ? reader.GetString(ordPart) : reader["PartNumb"].ToString();
                        string desc = ordDesc >= 0 ? reader.GetString(ordDesc) : reader["PartDescription"].ToString();
                        int total = ordTot >= 0 ? Convert.ToInt32(reader.GetValue(ordTot)) : Convert.ToInt32(reader["Total"]);

                        var row = dt.NewRow();
                        row["PartNumb"] = part;
                        row["PartDescription"] = desc;
                        row["Total"] = total;
                        dt.Rows.Add(row);
                    }
                }
            }

            gvPartes.DataSource = dt;
            gvPartes.DataBind();

            ShowToast($"Resultados: {dt.Rows.Count}", "info");
        }

        protected void gvPartes_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "PasarSAP")
            {
                string partNumb = e.CommandArgument?.ToString();
                if (string.IsNullOrWhiteSpace(partNumb))
                {
                    Funciones.MostrarToast("Número de parte inválido.", "danger");
                    return;
                }

                int afectados = PasarASAP(partNumb);
                if (afectados > 0)
                {
                    ShowToast($"Se marcaron {afectados} registros de [{partNumb}] como SAPM=1.", "success");
                    BindGrid();
                }
                else
                {
                    ShowToast($"No se encontraron pendientes para [{partNumb}].", "warning");
                }
            }
        }

        private int PasarASAP(string partNumb)
        {
            // Llama al SP en modo P y lee la columna 'Afectados' del resultset
            string[] names = { "@TransactionCode", "@PartNumb", "@UpdatedBy", "@UpdatedOn" };
            object[] vals = {
                "P",
                partNumb,
                Session["globalId"] != null ? (object)Session["globalId"].ToString() : DBNull.Value,
                DateTime.Now
            };

            int afectados = 0;
            using (SqlDataReader reader = Funciones.ExecuteReader("[dbo].[SP_IndirectMaterials_OrderDetail]", names, vals))
            {
                // El SP ejecuta SP_SelectReturnValue y además hace: SELECT @ReturnValue AS Afectados
                if (reader != null)
                {
                    // Avanza hasta el último resultset con filas; algunos SP_SelectReturnValue devuelven varios sets.
                    do
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                try
                                {
                                    int ord = -1;
                                    try { ord = reader.GetOrdinal("Afectados"); } catch { ord = 0; }
                                    if (!reader.IsDBNull(ord))
                                        afectados = Convert.ToInt32(reader.GetValue(ord));
                                }
                                catch
                                {
                                    // Ignorar errores de forma; dejamos afectados=0 si no se pudo leer
                                }
                            }
                        }
                    } while (reader.NextResult());
                }
            }

            return afectados;
        }

        private void ShowToast(string message, string type = "info")
        {
            // type: success | info | warning | danger
            ScriptManager.RegisterStartupScript(this, GetType(), Guid.NewGuid().ToString(),
                $"if(window.mroShowToast) mroShowToast({EscapeJs(message)}, '{type}', 'top-0 end-0', 3000);", true);
        }

        private string EscapeJs(string s)
        {
            if (s == null) return "''";
            return "'" + s.Replace(@"\", @"\\").Replace("'", @"\'").Replace(Environment.NewLine, " ") + "'";
        }
    }
}