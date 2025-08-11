using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplicationPMRO2.Utilities;
using System.Web.UI.HtmlControls;
using System.Drawing.Printing;
using System.EnterpriseServices;

namespace WebApplicationPMRO2.Pages.Almacen
{
    public partial class Inventario : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                GetTotalPages();
                TotalPages = GetTotalPages();
                LoadDropdownLocacion();
                LoadDropdownCategoria();
                LoadDropdownStock();

                LoadData();
            }
        }

        protected void LoadData()
        {
            string locacion = ddlLocacion.SelectedValue;
            string categoria = ddlCategoria.SelectedValue;
            string stock = ddlStock.SelectedValue;
            string searchText = txtBuscar.Text.Trim();
            try
            {
              


                if(string.IsNullOrEmpty(searchText))
                {
                    searchText = null; // Si no hay texto de búsqueda, se envía como cadena vacía
                }

                // Verifica si se seleccionó una locación, categoría o stock
                if (locacion == "0")
                {
                    locacion = null; // Si no se selecciona, se envía como null
                }

                if (categoria == "0")
                {
                    categoria = null; // Si no se selecciona, se envía como null
                }

                if (stock == "0")
                {
                    stock = null; // Si no se selecciona, se envía como null    
                }


                    DataTable dt = new DataTable();
                    dt.Columns.Add("Imagen", typeof(string));
                    dt.Columns.Add("Np", typeof(string));
                    dt.Columns.Add("Descripcion", typeof(string));
                    dt.Columns.Add("Categoria", typeof(string));
                    dt.Columns.Add("Comprador", typeof(string));
                    dt.Columns.Add("Inventario", typeof(int));
                    dt.Columns.Add("UM", typeof(string));
                    dt.Columns.Add("Locacion", typeof(string));
                    dt.Columns.Add("Min", typeof(int));
                    dt.Columns.Add("Max", typeof(int));
                    dt.Columns.Add("Stock", typeof(string));
                    dt.Columns.Add("ProductId", typeof(int));

                    using (SqlDataReader reader = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_Products]", new[] { "@TransactionCode", "@page", "@pagination", "@CategoryId", "@Location", "@StockLevel", "@SearchText" }, new[] { "P", CurrentPage.ToString(), ddlPagination.Text, categoria,locacion,stock,searchText}))
                    {
                        while (reader.Read())
                        {
                            DataRow row = dt.NewRow();
                            row["Imagen"] = reader["ProductImg"];
                            row["Np"] = reader["PartNumb"];
                            row["Descripcion"] = reader["ProductDescription"];
                            row["Categoria"] = reader["CategoryName"];
                            row["Comprador"] = reader["Buyer"];
                            row["Inventario"] = reader["Inventory"];
                            row["UM"] = reader["UM"];
                            row["Locacion"] = reader["Location"];
                            row["Min"] = reader["MinStock"];
                            row["Max"] = reader["MaxStock"];
                            row["Stock"] = reader["StockLevel"];
                            row["ProductId"] = reader["ProductId"];
                            dt.Rows.Add(row);
                        }
                        tblInventory.DataSource = dt;
                        tblInventory.DataBind(); // ¡No olvides hacer el DataBind!

                    }


                
            }
            catch (Exception ex)
            {

                Funciones.MostrarToast($"Error: {ex.Message} | Parámetros: {searchText}, {categoria}, {locacion}, {stock}", "danger", "top-0 end-0", 5000);

            }
        }



        protected  void tblInventory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = tblInventory.Rows[rowIndex];
                string np = row.Cells[1].Text;
                string descripcion = row.Cells[2].Text;
                string categoria = row.Cells[3].Text;
                string comprador = row.Cells[4].Text;
                int inventario = Convert.ToInt32(row.Cells[5].Text);
                string um = row.Cells[6].Text;
                string locacion = row.Cells[7].Text;
                int min = Convert.ToInt32(row.Cells[8].Text);
                int max = Convert.ToInt32(row.Cells[9].Text);
                string stock = row.Cells[10].Text;
                // Aquí puedes agregar la lógica para editar el inventario
            }else if (e.CommandName == "VerImagen")
                {
                    string url = e.CommandArgument.ToString();
                    hdnImagenUrl.Value = url;

                    // Ejecuta el script para abrir el modal y cambiar la imagen
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModal", $@"
            document.querySelector('#imagenModal img').src = '{url}';
            var modal = new bootstrap.Modal(document.getElementById('imagenModal'));
            modal.show();", true);
                }

        }


        private int CurrentPage
        {
            get => ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1;
            set => ViewState["CurrentPage"] = value;
        }

        private int TotalPages
        {
            get => ViewState["TotalPages"] != null ? (int)ViewState["TotalPages"] : TotalPages;
            set => ViewState["TotalPages"] = value;
        }


        protected int GetTotalPages()
        {
            string locacion = ddlLocacion.SelectedValue;
            string categoria = ddlCategoria.SelectedValue;
            string stock = ddlStock.SelectedValue;
            string searchText = txtBuscar.Text.Trim();
            int Total = 0;
            using (SqlDataReader reader = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_Products]", new[] { "@TransactionCode", "@pagination" }, new[] { "V", ddlPagination.Text }))
            {
                if (reader.Read())
                {
                    Total = Convert.ToInt32(reader["Totalsheets"]);
                }
            }


            return Total;

        }


        protected void btnPage_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            CurrentPage = int.Parse(btn.CommandArgument);
            LoadData();
            UpdatePaginationButtons();
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadData();
                UpdatePaginationButtons();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                LoadData();
                UpdatePaginationButtons();
            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Aquí puedes manejar la lógica de búsqueda
            // Por ejemplo, recargar los datos filtrados por la búsqueda
            LoadData();
        }



        private void LoadDropdownLocacion()
        {
            FuncionesMes.LlenarDropDownList(
                ddlLocacion,
                "[dbo].[SP_IndirectMaterials_Locacion]",
                new[] { "@TransactionCode" },
                new[] { "S" },
                "Todas las locaciones",
                "0",
                "NombreLocation",
                "LocationId"
            );
        }


        private void LoadDropdownCategoria()
        {
            FuncionesMes.LlenarDropDownList(
                ddlCategoria,
                "[dbo].[SP_IndirectMaterials_Category]",
                new[] { "@TransactionCode" },
                new[] { "S" },
                "Todas las categorias",
                "0",
                "CategoryName",
                "CategoryId"
            );
        }

        private void LoadDropdownStock()
        {
            FuncionesMes.LlenarDropDownList(
                ddlStock,
                "[dbo].[SP_IndirectMaterials_Stock]",
                new[] { "@TransactionCode" },
                new[] { "S" },
                "Todos los LevelStock",
                "0",
                "StockLevel",
                "StockId"
            );
        }




        private void UpdatePaginationButtons()
        {
            int startPage = Math.Max(1, CurrentPage - 2);
            int endPage = Math.Min(TotalPages, startPage + 4);

            btnPage1.Text = startPage.ToString();
            btnPage1.CommandArgument = startPage.ToString();
            btnPage1.Visible = startPage <= TotalPages;

            btnPage2.Text = (startPage + 1).ToString();
            btnPage2.CommandArgument = (startPage + 1).ToString();
            btnPage2.Visible = (startPage + 1) <= TotalPages;

            btnPage3.Text = (startPage + 2).ToString();
            btnPage3.CommandArgument = (startPage + 2).ToString();
            btnPage3.Visible = (startPage + 2) <= TotalPages;

            btnPage4.Text = (startPage + 3).ToString();
            btnPage4.CommandArgument = (startPage + 3).ToString();
            btnPage4.Visible = (startPage + 3) <= TotalPages;

            btnPage5.Text = (startPage + 4).ToString();
            btnPage5.CommandArgument = (startPage + 4).ToString();
            btnPage5.Visible = (startPage + 4) <= TotalPages;
        }


        protected void LocationSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void CategorySelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
            // Aquí puedes manejar el cambio de la categoría
            // Por ejemplo, recargar los datos filtrados por la categoría seleccionada
        }


        protected void StockSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
            // Aquí puedes manejar el cambio del stock
            // Por ejemplo, recargar los datos filtrados por el stock seleccionado
        }



        protected void ddlPagination_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Aquí puedes manejar el cambio de la paginación
            // Por ejemplo, recargar los datos con la nueva página seleccionada
            GetTotalPages();
            TotalPages = GetTotalPages();

            if (CurrentPage > TotalPages)
            {
                CurrentPage = TotalPages;
            }

            UpdatePaginationButtons();
            LoadData();
        }

        //mejorar la paginacion y hacer un multiview para que no se recargue la pagina y que haga edicion y eliminacion 






    }//END
}