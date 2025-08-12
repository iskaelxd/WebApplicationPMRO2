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
                //BORRAR LA LISTA DE INDIRECTOS EN LA SESION
                Session["ListaIndirectos"] = null; // Limpiar la lista de indirectos en la sesión

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

            if(ddlAreaS.SelectedValue == "13" && ddlLineaS.SelectedValue == "0")
                {
                    Funciones.MostrarToast("Debe seleccionar una linea", "warning", "top-0 end-0", 3000);
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
                            OrderQnty = txtQnty.Text.Trim(),
                            UM = reader["UM"].ToString(),
                            PrLocation = reader["Location"].ToString(),
                            Price = reader["Price"].ToString()
                        };

                        // Obtener la lista actual de la sesión
                        List<Indirecto> listaIndirectos = Session["ListaIndirectos"] as List<Indirecto> ?? new List<Indirecto>();


                            // Validar si el número de parte ya existe en la lista
                         bool yaExiste = listaIndirectos.Any(i => i.PartNumber == txtNumberPart.Text.Trim());
                         if (yaExiste)
                         {
                               Funciones.MostrarToast("Este número de parte ya está en la lista.", "warning", "top-0 end-0", 3000);
                               return;
                         }


                        // Agregar el nuevo
                        listaIndirectos.Add(nuevoIndirecto);

                        // Guardar de nuevo en sesión
                        Session["ListaIndirectos"] = listaIndirectos;

                        // Mostrar en el GridView
                        GridViewIndirectos.DataSource = listaIndirectos;
                        GridViewIndirectos.DataBind();

                            btnCrearOrden.Visible = true;
                            btnCancelarOrden.Visible = true;
                            ddlAreaS.Enabled = false; // Deshabilitar el dropdown de área
                            ddlLineaS.Enabled = false; // Deshabilitar el dropdown de línea

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


            if (e.CommandName == "Eliminar")
            {
                string partNumber = e.CommandArgument.ToString();

                // Obtener la lista actual de la sesión
                List<Indirecto> listaIndirectos = Session["ListaIndirectos"] as List<Indirecto>;

                if (listaIndirectos != null)
                {
                    // Buscar y eliminar el elemento con el número de parte correspondiente
                    Indirecto itemAEliminar = listaIndirectos.FirstOrDefault(i => i.PartNumber == partNumber);
                    if (itemAEliminar != null)
                    {
                        listaIndirectos.Remove(itemAEliminar);

                        // Actualizar la sesión y el GridView
                        Session["ListaIndirectos"] = listaIndirectos;
                        GridViewIndirectos.DataSource = listaIndirectos;
                        GridViewIndirectos.DataBind();

                        // Si la lista queda vacía, ocultar botones y habilitar dropdowns
                        if (listaIndirectos.Count == 0)
                        {
                            btnCrearOrden.Visible = false;
                            btnCancelarOrden.Visible = false;
                            
                            ddlAreaS.Enabled = true;
                            ddlAreaS.SelectedIndex = 0; // Reiniciar el dropdown de área

                            ddlLineaS.Enabled = true;
                            ddlLineaS.SelectedIndex = 0; // Reiniciar el dropdown de línea
                            lineaS.Visible = false;
                        }
                    }
                }
            }


        }

        protected void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            TextBox txtCantidad = (TextBox)sender;
            GridViewRow row = (GridViewRow)txtCantidad.NamingContainer;

            // Obtener el número de parte desde la fila
            string partNumber = ((Button)row.FindControl("btnEliminar")).CommandArgument;

            // Validar que la cantidad sea un número válido
            if (!int.TryParse(txtCantidad.Text, out int nuevaCantidad) || nuevaCantidad <= 0)
            {
                Funciones.MostrarToast("La cantidad debe ser un número válido mayor que cero.", "warning", "top-0 end-0", 3000);
                RevertirCantidad(row, partNumber);
                return;
            }

            // Validar inventario desde la base de datos
            using (SqlDataReader reader = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_Products]", new[] { "PartNumb", "@TransactionCode" }, new[] { partNumber, "S" }))
            {
                if (reader.Read())
                {
                    int inventario = int.Parse(reader["Inventory"].ToString());
                    if (nuevaCantidad > inventario)
                    {
                        Funciones.MostrarToast("No hay suficiente inventario para la cantidad solicitada.", "warning", "top-0 end-0", 3000);
                        RevertirCantidad(row, partNumber);
                        return;
                    }

                    // Actualizar la cantidad en la lista de sesión
                    List<Indirecto> listaIndirectos = Session["ListaIndirectos"] as List<Indirecto>;
                    var item = listaIndirectos?.FirstOrDefault(i => i.PartNumber == partNumber);
                    if (item != null)
                    {
                        item.OrderQnty = nuevaCantidad.ToString();
                        Session["ListaIndirectos"] = listaIndirectos;
                        GridViewIndirectos.DataSource = listaIndirectos;
                        GridViewIndirectos.DataBind();
                    }
                }
            }
        }


        private void RevertirCantidad(GridViewRow row, string partNumber)
        {
            List<Indirecto> listaIndirectos = Session["ListaIndirectos"] as List<Indirecto>;
            var item = listaIndirectos?.FirstOrDefault(i => i.PartNumber == partNumber);
            if (item != null)
            {
                TextBox txtCantidad = (TextBox)row.FindControl("txtCantidad");
                txtCantidad.Text = item.OrderQnty;
            }
        }


        //crear orden
        protected void btnCrearOrden_Click(object sender, EventArgs e)
        {

            try
            {

                string Id = null;

                //Insetar orden en la base de datos
                using (SqlDataReader reader = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_OrderHeader]", new[] { "@TransactionCode", "@AreaId", "@LineId", "@StatusId", "@UpdatedBy", "@Correo", "@globalId" },
                    new[] { "I", ddlAreaS.SelectedIndex.ToString(), ddlLineaS.SelectedIndex.ToString(), "1", Session["Username"], Session["Correo"], Session["globalId"] }))
                {

                    if (reader.Read())
                    {
                        Id = reader["OrderHeaderId"].ToString();
                    }

                }


                if (string.IsNullOrEmpty(Id))
                {
                    Funciones.MostrarToast("Error al crear la orden. No se obtuvo un ID válido.", "danger", "top-0 end-0", 3000);
                    return;
                }


                //HACER EL INSERT DE LOS INDIRECTOS EN LA BASE DE DATOS OrderHeaderId, PartNumb,OrderQnty,UpdatedBy, PrLocation, Price, PartDescription,UM

                // Obtener lista de indirectos desde sesión
                List<Indirecto> listaIndirectos = Session["ListaIndirectos"] as List<Indirecto>;

                if (listaIndirectos == null || listaIndirectos.Count == 0)
                {
                    Funciones.MostrarToast("No hay materiales indirectos para insertar.", "warning", "top-0 end-0", 3000);
                    return;
                }

                // Insertar cada indirecto en la base de datos
                foreach (var item in listaIndirectos)
                {
                    FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_OrderDetail]",
                        new[] { "@TransactionCode", "@OrderHeaderId", "@PartNumb", "@OrderQnty", "@UpdatedBy", "@PrLocation", "@Price", "@PartDescription", "@UM" },
                        new[] { "I", Id, item.PartNumber, item.OrderQnty, Session["Username"].ToString(), item.PrLocation, item.Price, item.ProductDescription, item.UM });
                }

                Funciones.MostrarToast("Orden creada exitosamente.", "success", "top-0 end-0", 3000);

                // Limpiar sesión y controles
                Session["ListaIndirectos"] = null;
                GridViewIndirectos.DataSource = null;
                GridViewIndirectos.DataBind();

                btnCrearOrden.Visible = false;
                btnCancelarOrden.Visible = false;
                ddlAreaS.Enabled = true;
                ddlLineaS.Enabled = true;
                lineaS.Visible = false; // Ocultar el dropdown de línea si no es necesario


            }
            catch (Exception ex)
            {
                Funciones.MostrarToast("Error al crear la orden: " + ex.Message, "danger", "top-0 end-0", 3000);
                return;
            }

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
            
            ddlLineaS.Enabled = true; // Habilitar el dropdown de línea
            lineaS.Visible = false;
            ddlAreaS.Enabled = true; // Habilitar el dropdown de área

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