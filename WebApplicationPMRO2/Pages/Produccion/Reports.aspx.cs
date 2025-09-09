using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplicationPMRO2.Utilities;  // Asegúrate que esta ruta es correcta para FuncionesMes / Funciones

namespace WebApplicationPMRO2.Pages.Produccion
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BuscarYBind();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarYBind();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtPartDesc.Text = string.Empty;
            gvResultados.PageIndex = 0;
            BuscarYBind();
        }

        protected void gvResultados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvResultados.PageIndex = e.NewPageIndex;
            BuscarYBind();
        }

        private void BuscarYBind()
        {
            try
            {
                var globalId = Session["globalId"] != null ? Session["globalId"].ToString() : null;
                if (string.IsNullOrWhiteSpace(globalId))
                {
                    Funciones.MostrarToast("No hay GlobalId en la sesión. Inicia sesión nuevamente.", "warning", "top-0 end-0", 3000);
                    gvResultados.DataSource = null;
                    gvResultados.DataBind();
                    lblResumen.Text = "";
                    return;
                }

                string filtro = string.IsNullOrWhiteSpace(txtPartDesc.Text) ? null : txtPartDesc.Text.Trim();

                // Parametrización para el SP en modo Reporte ('R')
                string[] paramNames = new[] { "@TransactionCode", "@UpdatedBy", "@PartDescription" };
                string[] paramValues = new[] { "R", globalId, filtro }; // filtro puede ir null

                var resultados = new List<ReporteRow>();

                using (SqlDataReader reader = Funciones.ExecuteReader(
                    "[dbo].[SP_IndirectMaterials_OrderDetail]",
                    paramNames,
                    paramValues))
                {
                    if (reader == null)
                    {
                        Funciones.MostrarToast("Error al obtener la información.", "danger", "top-0 end-0", 3000);
                        return;
                    }

                    while (reader.Read())
                    {
                        resultados.Add(new ReporteRow
                        {
                            PartNumb = reader["PartNumb"] as string,
                            PartDescription = reader["PartDescription"] as string,
                            TotalOrderQnty = SafeToInt(reader["TotalOrderQnty"])
                        });
                    }
                }

                gvResultados.DataSource = resultados;
                gvResultados.DataBind();

                lblResumen.Text = resultados.Count > 0
                    ? $"Mostrando {resultados.Count} resultado(s) {(string.IsNullOrWhiteSpace(filtro) ? "" : $"para \"{Server.HtmlEncode(filtro)}\"")} filtrado por tu GlobalId."
                    : "Sin resultados.";

            }
            catch (Exception ex)
            {
                Funciones.MostrarToast("Ocurrió un error al consultar el reporte: " + ex.Message, "danger", "top-0 end-0", 4000);
            }
        }

        private static int SafeToInt(object value)
        {
            if (value == null || value == DBNull.Value) return 0;
            int n;
            return int.TryParse(value.ToString(), out n) ? n : 0;
        }

        private class ReporteRow
        {
            public string PartNumb { get; set; }
            public string PartDescription { get; set; }
            public int TotalOrderQnty { get; set; }
        }
    }
}