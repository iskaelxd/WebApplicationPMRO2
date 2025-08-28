using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplicationPMRO2.Utilities;

namespace WebApplicationPMRO2.Pages.Almacen
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }



        protected void LoadData()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("CategoryId", typeof(int));
                dt.Columns.Add("CategoryName", typeof(string));
                dt.Columns.Add("Inventario", typeof(int));
                dt.Columns.Add("MinStock", typeof(int));
                dt.Columns.Add("MaxStock", typeof(int));



                using (SqlDataReader reader = FuncionesMes.ExecuteReader("[dbo].[SP_Indirect_Dashboard]", new[] { "@TransactionCode" }, new[] { "SC" }))
                {
                    if (reader == null)
                    {
                        Funciones.MostrarToast("Error al obtener la información: ", "danger", "top-0 end-0", 3000);
                        return;
                    }
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["CategoryId"],
                            reader["CategoryName"],
                            reader["Inventario"],
                            reader["MinStock"],
                            reader["MaxStock"]
                        );
                    }
                    tblCategorias.DataSource = dt;
                    tblCategorias.DataBind(); // ¡No olvides hacer el DataBind!
                }
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast($"Error al cargar los datos: {ex.Message}", "danger", "top-0 end-0", 3000);

            }
        }


        protected void tblCategorias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int inventario = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Inventario"));
                int minStock = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "MinStock"));

                if (inventario == 0)
                {
                    e.Row.CssClass += " fila-roja";
                }
                else if (inventario < minStock)
                {
                    e.Row.CssClass += " fila-amarilla";
                }
                else
                {
                    e.Row.CssClass += " fila-verde";
                }
            }
        }


    }
}