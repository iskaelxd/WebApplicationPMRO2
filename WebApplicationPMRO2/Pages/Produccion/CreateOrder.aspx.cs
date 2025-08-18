using System;
using System.Collections.Generic;
using System.Data;
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
        private const string UPLOAD_DIR = "~/Uploads/Productos";
        private const string DEFAULT_IMG = "~/Uploads/Productos/imagenDefault.png";
        protected void Page_Load(object sender, EventArgs e)
        {



            if (!IsPostBack)
            {
                EnsureUploadFolder();

                LoadDropdownArea();
                LoadDropdownStatus();
                LoadDropdownLinea();
                LoadDropdownAreaS();
                LoadDropdownLineaS();
                LoadDropdownCategorias();
                //BORRAR LA LISTA DE INDIRECTOS EN LA SESION
                Session["ListaIndirectos"] = null; // Limpiar la lista de indirectos en la sesión
                LoadData();

            }





        }




        protected void LoadData()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("IdOrder", typeof(int));
                dt.Columns.Add("AreaOrLine", typeof(string));
                dt.Columns.Add("StatusId", typeof(string));
                dt.Columns.Add("UpdatedBy", typeof(string));

                string areaId = ddlArea.SelectedValue == "0" ? null : ddlArea.SelectedValue;
                string lineId = ddlLinea.SelectedValue == "0" ? null : ddlLinea.SelectedValue;
                string statusId = ddlStatus.SelectedValue == "0" ? null : ddlStatus.SelectedValue;


                using (SqlDataReader reader = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_OrderHeader]", new[] { "@TransactionCode", "@globalId", "@AreaId", "@LineId", "@StatusId" }, new[] { "S", Session["globalId"].ToString(), areaId, lineId, statusId }))
                {
                    if (reader == null)
                    {
                        Funciones.MostrarToast("Error al obtener la información: ", "danger", "top-0 end-0", 3000);
                        return;
                    }
                    while (reader.Read())
                    {

                        string AreaOrLine = string.Empty; // Inicializar la variable
                        if (reader["AreaId"].ToString() == "13")
                        {
                            AreaOrLine = reader["LineShortName"].ToString();
                        }
                        else
                        {
                            AreaOrLine = reader["AreaName"].ToString();
                        }

                        dt.Rows.Add(
                            reader["OrderHeaderId"],
                            AreaOrLine,
                            reader["StatusCode"],
                            reader["UpdatedBy"]
                        );
                    }
                    tblorder.DataSource = dt;
                    tblorder.DataBind(); // ¡No olvides hacer el DataBind!
                }
            }
            catch (SqlException sqlEx)
            {
                Funciones.MostrarToast($"Error de base de datos: {sqlEx.Message}", "danger", "top-0 end-0", 3000);
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast($"Error al cargar los datos: {ex.Message}", "danger", "top-0 end-0", 3000);

            }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void tblorder_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Consultar")
            {
                string orderId = e.CommandArgument.ToString();
                btnOrder.Visible = false;
                OrdeIdH.Text = "Orden #" + orderId;
                mvCreateOrder.SetActiveView(vwViewOrderDetails);

                LoadDataDetails(orderId); // Cargar los detalles de la orden seleccionada


            }
            else if (e.CommandName == "Eliminar")
            {
                string orderId = e.CommandArgument.ToString();

                hfIdorder.Value = orderId;
                litNombreRolEliminar.Text = "Orden #" + orderId;
                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", "$('#modalEliminar').modal('show');", true);
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


        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            string orderId = hfIdorder.Value;
            // Aquí puedes agregar la lógica para eliminar la orden de la base de datos
            FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_OrderHeader]", new[] { "@TransactionCode", "@OrderHeaderId" }, new[] { "D", orderId });
            Funciones.MostrarToast("Orden eliminada exitosamente.", "success", "top-0 end-0", 3000);
            LoadData(); // Recargar la lista de órdenes después de eliminar
            ScriptManager.RegisterStartupScript(this, GetType(), "ocultarModal", @"
                            $('#modalEliminar').modal('hide');
                            $('body').removeClass('modal-open');
                            $('.modal-backdrop').remove();", true);
        }
        protected void LoadDataDetails(string orderId)
        {

            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PartNumb", typeof(string));
                dt.Columns.Add("PartDescription", typeof(string));
                dt.Columns.Add("OrderQnty", typeof(int));
                dt.Columns.Add("UM", typeof(string));
                using (SqlDataReader reader = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_OrderDetail]", new[] { "@TransactionCode", "@OrderHeaderId" }, new[] { "S", orderId }))
                {
                    if (reader == null)
                    {
                        Funciones.MostrarToast("Error al obtener la información: ", "danger", "top-0 end-0", 3000);
                        return;
                    }
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["PartNumb"],
                            reader["PartDescription"],
                            reader["OrderQnty"],
                            reader["UM"]
                        );
                    }
                    tblorderDetails.DataSource = dt;
                    tblorderDetails.DataBind(); // ¡No olvides hacer el DataBind!
                }
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast($"Error al cargar los datos: {ex.Message}", "danger", "top-0 end-0", 3000);
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            mvCreateOrder.SetActiveView(vwViewOrders);
            btnOrder.Visible = true;
            OrdeIdH.Text = string.Empty; // Limpiar el encabezado de la orden
        }


        // Abrir modal
        protected void OpenModal_Click(object sender, EventArgs e)
        {
            // Valores iniciales
            CatalogPage = 1;
            if (ddlPageSize.Items.Count > 0)
                CatalogPageSize = int.Parse(ddlPageSize.SelectedValue);
            else
                CatalogPageSize = 12;

            CatalogSearch = txtBuscar.Text?.Trim() ?? string.Empty;

            LoadProdcutsCatalog();
        }





        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            CatalogPageSize = int.Parse(ddlPageSize.SelectedValue);
            CatalogPage = 1;
            LoadProdcutsCatalog();
        }



        //dropdown
        protected void btnView_Click(object sender, EventArgs e)
        {
            string boton = btnOrder.Text;

            if (boton == "Crear Orden")
            {

                mvCreateOrder.SetActiveView(vmCreateOrder);
                btnOrder.Text = "Ver Ordenes";

            }
            else if (boton == "Ver Ordenes")
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
                ddlLinea.SelectedIndex = 0; // Reiniciar el dropdown de línea si no es necesario
            }
            LoadData(); // Recargar la tabla de órdenes al cambiar el área
        }


        protected void ddlLinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData(); // Recargar la tabla de órdenes al cambiar la línea
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

            try
            {
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

                if (ddlAreaS.SelectedValue == "13" && ddlLineaS.SelectedValue == "0")
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

                string lineas = ddlLineaS.SelectedValue.ToString();

                if (lineas == "0")
                {
                    lineas = null;
                }

                //Insetar orden en la base de datos
                using (SqlDataReader reader = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_OrderHeader]", new[] { "@TransactionCode", "@AreaId", "@LineId", "@StatusId", "@UpdatedBy", "@Correo", "@globalId" },
                    new[] { "I", ddlAreaS.SelectedValue.ToString(), lineas, "1", Session["Username"], Session["Correo"], Session["globalId"] }))
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

                Funciones.MostrarToast("Orden creada exitosamente." + Id.ToString(), "success", "top-0 end-0", 3000);

                // Limpiar sesión y controles
                Session["ListaIndirectos"] = null;
                GridViewIndirectos.DataSource = null;
                GridViewIndirectos.DataBind();

                btnCrearOrden.Visible = false;
                btnCancelarOrden.Visible = false;
                ddlAreaS.Enabled = true;
                ddlLineaS.Enabled = true;
                lineaS.Visible = false; // Ocultar el dropdown de línea si no es necesario
                LoadData(); // Recargar la tabla de órdenes para mostrar la nueva orden creada


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

        //helpers


        // --- Estado del catálogo (ViewState) ---
        private int CatalogPage
        {
            get => (int)(ViewState["CatalogPage"] ?? 1);
            set => ViewState["CatalogPage"] = value < 1 ? 1 : value;
        }

        private int CatalogPageSize
        {
            get => (int)(ViewState["CatalogPageSize"] ?? 12);
            set => ViewState["CatalogPageSize"] = value < 1 ? 12 : value;
        }

        private int CatalogTotalPages
        {
            get => (int)(ViewState["CatalogTotalPages"] ?? 1);
            set => ViewState["CatalogTotalPages"] = value < 1 ? 1 : value;
        }

        private string CatalogSearch
        {
            get => (string)(ViewState["CatalogSearch"] ?? string.Empty);
            set => ViewState["CatalogSearch"] = value ?? string.Empty;
        }

        // --- Mostrar SIEMPRE el modal tras un postback del catálogo ---

        private void ShowCatalogModal()
        {
            string js = @"
    (function(){
      if (window.__catalog_cleanup) { __catalog_cleanup(); }
      var el = document.getElementById('catalogModal');
      if (el && window.bootstrap){
        var m = bootstrap.Modal.getOrCreateInstance(el);
        m.show();
      }
      if (window.__catalog_wire) { __catalog_wire(); }
    })();";
            ScriptManager.RegisterStartupScript(this, GetType(), "showCatalogModal", js, true);
        }


        //loadcatalog

        private void LoadProdcutsCatalog()
        {
            // 1) Total de páginas (SP 'V')
            var pnames = new List<string> { "@TransactionCode", "@pagination" };
            var pvals = new List<string> { "V", CatalogPageSize.ToString() };

            // Filtros opcionales:
            if (!string.IsNullOrWhiteSpace(CatalogSearch))
            {
                pnames.Add("@SearchText"); pvals.Add(CatalogSearch.Trim());
            }
            if (int.TryParse(ddlCategorias.SelectedValue, out int catId) && catId > 0)
            {
                pnames.Add("@CategoryId"); pvals.Add(catId.ToString());
            }

            using (var r = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_Products]",
                pnames.ToArray(), pvals.ToArray()))
            {
                if (r.Read())
                    CatalogTotalPages = Convert.ToInt32(r["Totalsheets"]);
                else
                    CatalogTotalPages = 1;
            }

            if (CatalogPage > CatalogTotalPages) CatalogPage = CatalogTotalPages;

            // 2) Traer página actual (SP 'P')
            var pnames2 = new List<string> { "@TransactionCode", "@pagination", "@page" };
            var pvals2 = new List<string> { "P", CatalogPageSize.ToString(), CatalogPage.ToString() };

            if (!string.IsNullOrWhiteSpace(CatalogSearch))
            {
                pnames2.Add("@SearchText"); pvals2.Add(CatalogSearch.Trim());
            }
            if (int.TryParse(ddlCategorias.SelectedValue, out int catId2) && catId2 > 0)
            {
                pnames2.Add("@CategoryId"); pvals2.Add(catId2.ToString());
            }

            var productos = new List<dynamic>();
            using (SqlDataReader reader = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_Products]",
                pnames2.ToArray(), pvals2.ToArray()))
            {
                while (reader.Read())
                {
                    productos.Add(new
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
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

            // 3) Info del pager y numeritos
            BuildPager();

            lblPagerInfo.Text = $"Página {CatalogPage} de {Math.Max(CatalogTotalPages, 1)} · {CatalogPageSize} por página";

            // 4) Mantener modal abierto
            ShowCatalogModal();
        }

        private void BuildPager()
        {
            // Ventana de hasta 7 páginas (actual ±3)
            int total = Math.Max(CatalogTotalPages, 1);
            int start = Math.Max(1, CatalogPage - 3);
            int end = Math.Min(total, start + 6);
            if (end - start < 6) start = Math.Max(1, end - 6);

            var pages = new List<dynamic>();
            for (int i = start; i <= end; i++)
                pages.Add(new { Number = i, Active = (i == CatalogPage) });

            rptPages.DataSource = pages;
            rptPages.DataBind();

            // Habilitar/deshabilitar extremos
            btnFirst.Enabled = btnPrev.Enabled = CatalogPage > 1;
            btnNext.Enabled = btnLast.Enabled = CatalogPage < total;
        }

        protected void Pager_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandArgument.ToString())
            {
                case "First": CatalogPage = 1; break;
                case "Prev": CatalogPage = Math.Max(1, CatalogPage - 1); break;
                case "Next": CatalogPage = Math.Min(CatalogTotalPages, CatalogPage + 1); break;
                case "Last": CatalogPage = CatalogTotalPages; break;
            }
            LoadProdcutsCatalog();
        }

        protected void rptPages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Page" && int.TryParse(e.CommandArgument.ToString(), out int page))
            {
                CatalogPage = Math.Max(1, Math.Min(CatalogTotalPages, page));
                LoadProdcutsCatalog();
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtGo.Text, out int page))
            {
                CatalogPage = Math.Max(1, Math.Min(CatalogTotalPages, page));
                LoadProdcutsCatalog();
            }
            else
            {
                Funciones.MostrarToast("Número de página inválido.", "warning", "top-0 end-0", 3000);
                ShowCatalogModal();
            }
        }

        protected void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            CatalogSearch = txtBuscar.Text?.Trim() ?? string.Empty;
            CatalogPage = 1;
            LoadProdcutsCatalog();
        }

        protected void ddlCategorias_SelectedIndexChanged(object sender, EventArgs e)
        {
            CatalogPage = 1;
            LoadProdcutsCatalog();
        }

        protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = string.Empty;

            // Deja "Todas las categorías" (Value = "0")
            if (ddlCategorias.Items.Count > 0)
                ddlCategorias.SelectedValue = "0";

            CatalogSearch = string.Empty;
            CatalogPage = 1;
            LoadProdcutsCatalog();
        }


        private void EnsureUploadFolder()
        {
            try
            {
                var phys = Server.MapPath(UPLOAD_DIR);
                if (!System.IO.Directory.Exists(phys))
                    System.IO.Directory.CreateDirectory(phys);
            }
            catch { /* no bloquear la página */ }
        }

        private bool FileExists(string virtualPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(virtualPath)) return false;

                // Si es URL absoluta (http/https) no mapeamos a disco
                if (virtualPath.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                    virtualPath.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                    virtualPath.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                    return true;

                return System.IO.File.Exists(Server.MapPath(virtualPath));
            }
            catch { return false; }
        }

        private string SanitizeForFileName(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            foreach (var c in System.IO.Path.GetInvalidFileNameChars())
                input = input.Replace(c, '_');
            return input.Trim();
        }

        // Devuelve la mejor URL de imagen para mostrar en el catálogo
        private string ResolveCatalogImage(string dbUrl, string partNumb, int? productId)
        {
            // 1) Si la BD trae algo y existe, úsalo
            if (!string.IsNullOrWhiteSpace(dbUrl) && FileExists(dbUrl))
                return dbUrl;

            // 2) Intenta por convención con productId + NP
            string safeNp = SanitizeForFileName(partNumb);
            if (productId.HasValue && productId.Value > 0)
            {
                foreach (var ext in new[] { ".jpg", ".jpeg", ".png" })
                {
                    var candidate = $"{UPLOAD_DIR}/{productId.Value}_{safeNp}{ext}";
                    if (FileExists(candidate)) return candidate;
                }
            }

            // 3) Intenta sólo por NP (por si guardaste así)
            foreach (var ext in new[] { ".jpg", ".jpeg", ".png" })
            {
                var candidate = $"{UPLOAD_DIR}/{safeNp}{ext}";
                if (FileExists(candidate)) return candidate;
            }

            // 4) Fallback global
            return DEFAULT_IMG;
        }



        protected void rptCatalogoProductos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                dynamic data = e.Item.DataItem;

                string stockLevel = Convert.ToString(data.StockLevel);
                int inventory = int.TryParse(Convert.ToString(data.Inventory), out int inv) ? inv : 0;

                Panel pnlCard = (Panel)e.Item.FindControl("pnlCard");
                Image imgProducto = (Image)e.Item.FindControl("imgProducto");
                Button btnSeleccionar = (Button)e.Item.FindControl("btnSeleccionar");

                // *** RESOLVER IMAGEN ***
                int? pid = null;
                try { pid = (int)data.ProductId; } catch { /* si no viene, queda null */ }

                string finalUrl = ResolveCatalogImage(
                    Convert.ToString(data.ProductImg),
                    Convert.ToString(data.PartNumb),
                    pid
                );

                imgProducto.ImageUrl = ResolveUrl(finalUrl);
                imgProducto.AlternateText = Convert.ToString(data.ProductDescription);

                // Botón según inventario
                if (inventory <= 0)
                {
                    btnSeleccionar.Enabled = false;
                    btnSeleccionar.CssClass += " disabled";
                    btnSeleccionar.Text = "Sin Inventario";
                }

                // Fondo por nivel de stock
                switch (stockLevel)
                {
                    case "1": pnlCard.CssClass += " bg-success bg-opacity-25"; break;
                    case "2": pnlCard.CssClass += " bg-warning bg-opacity-25"; break;
                    case "3": pnlCard.CssClass += " bg-danger bg-opacity-25"; break;
                    default: pnlCard.CssClass += " bg-light"; break;
                }
            }
        }

    }//END


}