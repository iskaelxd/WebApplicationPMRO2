using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplicationPMRO2.Utilities;

namespace WebApplicationPMRO2.Pages.Almacen
{
    public partial class Seguimiento_Solicitud : System.Web.UI.Page
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
            
            DataTable dt = new DataTable();
            dt.Columns.Add("OrderHeaderId", typeof(string));
            dt.Columns.Add("Area", typeof(string));
            dt.Columns.Add("Linea", typeof(string));
            dt.Columns.Add("status", typeof(string));
            dt.Columns.Add("PartNumb", typeof(string));
            dt.Columns.Add("OrderQnty", typeof(string));
            dt.Columns.Add("Solicitado", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Marcado", typeof(int));
            dt.Columns.Add("SAPM", typeof(int));

            using (SqlDataReader reader = Funciones.ExecuteReader("[dbo].[SP_IndirectMaterials_OrderDetail]", new[] { "@TransactionCode" }, new[] {"Q"}))
            {
                while (reader.Read()) 
                { 
                    var row = dt.NewRow();
                    row["OrderHeaderId"] = reader["OrderHeaderId"].ToString();
                    row["Area"] = reader["AreaName"].ToString();
                    row["Linea"] = reader["LineShortName"].ToString() ?? null;
                    row["status"] = reader["StatusCode"].ToString() ?? null;
                    row["PartNumb"] = reader["PartNumb"].ToString();
                    row["OrderQnty"] = reader["OrderQnty"].ToString();
                    row["Solicitado"] = reader["SolicitadoPor"].ToString();
                    row["Date"] = reader["UpdatedOn"].ToString();
                    row["Marcado"] = Convert.ToInt32(reader["Marcado"]);
                    row["SAPM"] = Convert.ToInt32(reader["SAPM"]);
                    dt.Rows.Add(row);
                }
                dt.Rows.Add();
                tblSolicitudes.DataSource = dt;
                tblSolicitudes.DataBind();

            }

        }


        protected void tblSolicitudes_RowDataBound(object sender, GridViewRowEventArgs e) 
        { 
            
        }



    }
}