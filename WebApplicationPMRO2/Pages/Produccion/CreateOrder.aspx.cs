using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplicationPMRO2.Pages.Almacen;
using WebApplicationPMRO2.Utilities;

namespace WebApplicationPMRO2.Pages.Produccion
{
    public partial class CreateOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                LoadDropdownArea();
                LoadDropdownStatus();
                LoadDropdownLinea();
                LoadDropdownAreaS();
                LoadDropdownLineaS();
                LoadDropdownCategorias();
                
            }
        }


        //cargar catalogo de productos

        private void LoadProdcutsCatalog()
        {
            var productos = new List<dynamic>();

            using (SqlDataReader reader = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_Products]",
                new[] { "@TransactionCode", "@pagination", "@page" },
                new[] { "P","100","1" }))
            {
                while (reader.Read())
                {
                    productos.Add(new
                    {
                        PartNumb = reader["PartNumb"].ToString(),
                        ProductDescription = reader["ProductDescription"].ToString(),
                        Inventory = reader["Inventory"].ToString(),
                        CategoryName = reader["CategoryName"].ToString(),
                        ProductImg = reader["ProductImg"].ToString(),
                        UM = reader["UM"].ToString(),
                        StockLevel = reader["StockLevel"].ToString(),
                        Stock = reader["Stock"].ToString(),
                    });
                }
            }

            rptCatalogoProductos.DataSource = productos;
            rptCatalogoProductos.DataBind();
        }

        protected void rptCatalogoProductos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                dynamic data = e.Item.DataItem;
                string stockLevel = data.StockLevel.ToString();
                int inventory = int.TryParse(data.Inventory.ToString(), out int inv) ? inv : 0;

                Panel pnlCard = (Panel)e.Item.FindControl("pnlCard");
                Image imgProducto = (Image)e.Item.FindControl("imgProducto");
                Button btnSeleccionar = (Button)e.Item.FindControl("btnSeleccionar");

                // Asignar imagen
                imgProducto.ImageUrl = data.ProductImg;
                imgProducto.AlternateText = data.ProductDescription;

                // Desactivar botón si no hay inventario
                if (inventory <= 0)
                {
                    btnSeleccionar.Enabled = false;
                    btnSeleccionar.CssClass += " disabled";
                    btnSeleccionar.Text = "Sin Inventario";
                }

                // Asignar clase de fondo según el nivel de stock
                switch (stockLevel)
                {
                    case "1":
                        pnlCard.CssClass += " bg-success bg-opacity-25";
                        break;
                    case "2":
                        pnlCard.CssClass += " bg-warning bg-opacity-25";
                        break;
                    case "3":
                        pnlCard.CssClass += " bg-danger bg-opacity-25";
                        break;
                    default:
                        pnlCard.CssClass += " bg-light";
                        break;
                }
            }
        }


        protected void OpenModal_Click(object sender, EventArgs e)
        {
            // Aquí puedes agregar la lógica para abrir el modal
            // Por ejemplo, puedes usar un script para mostrar el modal
            LoadProdcutsCatalog();
            ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", "$('#catalogModal').modal('show');", true);
        }

        //


        //dropdown
        protected void btnView_Click(object sender, EventArgs e)
        {
            string boton = btnOrder.Text;

            if (boton == "Crear Orden")
            {

                mvCreateOrder.SetActiveView(vmCreateOrder);
                btnOrder.Text = "Ver Ordenes";

            }else if (boton == "Ver Ordenes")
            {
                mvCreateOrder.SetActiveView(vwViewOrders);
                btnOrder.Text = "Crear Orden";


            }
        }

        //deropdowns 

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



        private void LoadDropdownAreaS()
        {
            FuncionesMes.LlenarDropDownList(
                ddlAreaS,
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

        private void LoadDropdownLineaS()
        {
            FuncionesMes.LlenarDropDownList(
                ddlLineaS,
                "[dbo].[SP_IndirectMaterials_Line]",
                new[] { "@TransactionCode" },
                new[] { "S" },
                "Todos las lineas",
                "0",
                "LineShortName",
                "LineId"
            );
        }


        private void LoadDropdownCategorias()
        {
            FuncionesMes.LlenarDropDownList(
                ddlCategorias,
                "[dbo].[SP_IndirectMaterials_Category]",
                new[] { "@TransactionCode" },
                new[] { "S" },
                "Todos las categorias",
                "0",
                "CategoryName",
                "CategoryId"
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
            }
        }

        protected void ddlDroArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAreaS.SelectedValue == "13")
            {
                lineaS.Visible = true;
            }
            else
            {
                lineaS.Visible = false;
            }
        }

        //agregar indirecto
        protected void btnAgregarIndirecto_Click(object sender, EventArgs e)
        {

            try { 
            //validacion de si esta vacio o no es un numero valido
            if (string.IsNullOrEmpty(txtNumberPart.Text) || string.IsNullOrEmpty(txtQnty.Text) ||
                ddlAreaS.SelectedValue == "0")
            {

                Funciones.MostrarToast("Debe poder agregar una orden o un numero vacio.", "warning", "top-0 end-0", 3000);
                return;
            }

            if (!int.TryParse(txtQnty.Text, out int quantity) || quantity <= 0)
            {
                Funciones.MostrarToast("La cantidad debe ser un número válido mayor que cero.", "warning", "top-0 end-0", 3000);
                return;
            }

            if (!int.TryParse(txtNumberPart.Text, out int np) || np <= 0)
            {
                Funciones.MostrarToast("El número de parte debe ser un número válido mayor que cero.", "warning", "top-0 end-0", 3000);
                return;
            }

            //validar que exista suficiente inventario y que el numero de parte exista

            using (SqlDataReader reader = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_Products]", new[] { "PartNumb", "@TransactionCode" }, new[] { txtNumberPart.Text, "S" }))
            {
                if (reader.Read())
                {
                    int inventario = int.Parse(reader["Inventory"].ToString());
                    if (quantity > inventario)
                    {
                        Funciones.MostrarToast("No hay suficiente inventario para la cantidad solicitada.", "warning", "top-0 end-0", 3000);
                        return;
                    }
                    else
                    {
                        // Si pasa validación, crear el objeto
                        Indirecto nuevoIndirecto = new Indirecto
                        {
                            PartNumber = txtNumberPart.Text.Trim(),
                            ProductDescription = reader["ProductDescription"].ToString(),
                            OrderQnty = txtQnty.Text.Trim()
                        };

                        // Obtener la lista actual de la sesión
                        List<Indirecto> listaIndirectos = Session["ListaIndirectos"] as List<Indirecto> ?? new List<Indirecto>();

                        // Agregar el nuevo
                        listaIndirectos.Add(nuevoIndirecto);

                        // Guardar de nuevo en sesión
                        Session["ListaIndirectos"] = listaIndirectos;

                        // Mostrar en el GridView
                        GridViewIndirectos.DataSource = listaIndirectos;
                        GridViewIndirectos.DataBind();

                            btnCrearOrden.Visible = true;
                            btnCancelarOrden.Visible = true;

                        }
                }
                else
                {
                    Funciones.MostrarToast("El número de parte no existe.", "warning", "top-0 end-0", 3000);
                    return;
                }

            }


        }
            catch (Exception ex)
            {
                Funciones.MostrarToast("Error al agregar el indirecto: " + ex.Message, "danger", "top-0 end-0", 3000);
            }

      }


        protected void GridViewIndirectos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }



        //crear orden
        protected void btnCrearOrden_Click(object sender, EventArgs e)
        {

        
                
        }

        //cancelar orden
        protected void btnCancelarOrden_Click(object sender, EventArgs e)
        {

            Session["ListaIndirectos"] = null; // Limpiar la lista de indirectos en la sesión
            btnCrearOrden.Visible = false; // Ocultar el botón de crear orden
            btnCancelarOrden.Visible = false; // Ocultar el botón de cancelar orden
            GridViewIndirectos.DataSource = null; // Limpiar el GridView
            GridViewIndirectos.DataBind(); // Actualizar el GridView para que no muestre datos
            txtNumberPart.Text = string.Empty; // Limpiar el campo de número de parte
            txtQnty.Text = string.Empty; // Limpiar el campo de cantidad
            ddlAreaS.SelectedIndex = 0; // Reiniciar el dropdown de área
            ddlLineaS.SelectedIndex = 0; // Reiniciar el dropdown de línea

        }



        protected void btnAgregarBoton_Command(object sender, CommandEventArgs e)
        {
            string partNumber = e.CommandArgument.ToString();

            txtNumberPart.Text = partNumber;

            // Cierra el modal con JavaScript
            ScriptManager.RegisterStartupScript(this, GetType(), "ocultarModal", @"
        $('#catalogModal').modal('hide');
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();", true);
        }


    }//END





}