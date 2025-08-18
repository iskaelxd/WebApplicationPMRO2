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
using System.IO;

namespace WebApplicationPMRO2.Pages.Almacen
{
    public partial class Inventario : System.Web.UI.Page
    {
        private const string UPLOAD_DIR = "~/Uploads/Productos";
        private const string DEFAULT_IMG = "~/Uploads/Productos/imagenDefault.png";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                EnsureUploadFolder();

                LoadDropdownLocacion();
                LoadDropdownCategoria();
                LoadDropdownStock();

                // también carga ddls de edición
                LoadDropdownCategoriaEdit();
                LoadDropdownLocacionEdit();


                TotalPages = GetTotalPages();  // con filtros actuales
                CurrentPage = 1;

                LoadData();
            }
        }


        protected void btnPage_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            CurrentPage = int.Parse(btn.CommandArgument);
            LoadData();

        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadData();

            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                LoadData();

            }
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










        //mejorar la paginacion y hacer un multiview para que no se recargue la pagina y que haga edicion y eliminacion 


        private int CurrentPage
        {
            get => (int)(ViewState["CurrentPage"] ?? 1);
            set => ViewState["CurrentPage"] = value < 1 ? 1 : value;
        }

        private int TotalPages
        {
            get => (int)(ViewState["TotalPages"] ?? 1);
            set => ViewState["TotalPages"] = value < 1 ? 1 : value;
        }

        private int PageSize
        {
            get => int.TryParse(ddlPagination.SelectedValue, out var n) ? n : 25;
        }


        protected int GetTotalPages()
        {
            var pnames = new List<string> { "@TransactionCode", "@pagination" };
            var pvals = new List<string> { "V", PageSize.ToString() };

            string searchText = string.IsNullOrWhiteSpace(txtBuscar.Text) ? null : txtBuscar.Text.Trim();

            if (int.TryParse(ddlCategoria.SelectedValue, out var cat) && cat > 0)
            { pnames.Add("@CategoryId"); pvals.Add(cat.ToString()); }

            if (int.TryParse(ddlLocacion.SelectedValue, out var loc) && loc > 0)
            { pnames.Add("@Location"); pvals.Add(loc.ToString()); }

            if (int.TryParse(ddlStock.SelectedValue, out var stk) && stk > 0)
            { pnames.Add("@StockLevel"); pvals.Add(stk.ToString()); }

            if (!string.IsNullOrEmpty(searchText))
            { pnames.Add("@SearchText"); pvals.Add(searchText); }

            int total = 1;
            using (var r = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_Products]",
                pnames.ToArray(), pvals.ToArray()))
            {
                if (r.Read())
                    total = Convert.ToInt32(r["Totalsheets"]);
            }
            return Math.Max(total, 1);
        }


        protected void LoadData()
        {
            // Asegura que TotalPages esté alineado con filtros actuales
            TotalPages = GetTotalPages();
            if (CurrentPage > TotalPages) CurrentPage = TotalPages;

            string searchText = string.IsNullOrWhiteSpace(txtBuscar.Text) ? null : txtBuscar.Text.Trim();

            var pnames = new List<string> { "@TransactionCode", "@page", "@pagination" };
            var pvals = new List<string> { "P", CurrentPage.ToString(), PageSize.ToString() };

            if (int.TryParse(ddlCategoria.SelectedValue, out var cat) && cat > 0)
            { pnames.Add("@CategoryId"); pvals.Add(cat.ToString()); }

            if (int.TryParse(ddlLocacion.SelectedValue, out var loc) && loc > 0)
            { pnames.Add("@Location"); pvals.Add(loc.ToString()); }

            if (int.TryParse(ddlStock.SelectedValue, out var stk) && stk > 0)
            { pnames.Add("@StockLevel"); pvals.Add(stk.ToString()); }

            if (!string.IsNullOrEmpty(searchText))
            { pnames.Add("@SearchText"); pvals.Add(searchText); }

            var dt = new System.Data.DataTable();
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

            using (SqlDataReader reader = FuncionesMes.ExecuteReader(
                "[dbo].[SP_IndirectMaterials_Products]", pnames.ToArray(), pvals.ToArray()))
            {
                while (reader.Read())
                {
                    var row = dt.NewRow();
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
                    row["Stock"] = reader["Stock"]; // <- usa alias descriptivo del SP
                    row["ProductId"] = reader["ProductId"];
                    dt.Rows.Add(row);
                }
            }

            tblInventory.DataSource = dt;
            tblInventory.DataBind();

            BuildPager(); // números dinámicos + habilitar extremos
            lblPagerInfo.Text = $"Página {CurrentPage} de {TotalPages} · {PageSize} por página";
        }


        private void BuildPager()
        {
            int total = Math.Max(TotalPages, 1);
            int start = Math.Max(1, CurrentPage - 3);
            int end = Math.Min(total, start + 6);
            if (end - start < 6) start = Math.Max(1, end - 6);

            var pages = new List<dynamic>();
            for (int i = start; i <= end; i++)
                pages.Add(new { Number = i, Active = (i == CurrentPage) });

            rptPages.DataSource = pages;
            rptPages.DataBind();

            lnkFirst.Enabled = lnkPrev.Enabled = CurrentPage > 1;
            lnkNext.Enabled = lnkLast.Enabled = CurrentPage < total;
        }


        protected void Pager_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandArgument.ToString())
            {
                case "First": CurrentPage = 1; break;
                case "Prev": CurrentPage = Math.Max(1, CurrentPage - 1); break;
                case "Next": CurrentPage = Math.Min(TotalPages, CurrentPage + 1); break;
                case "Last": CurrentPage = TotalPages; break;
            }
            LoadData();
        }

        protected void rptPages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Page" && int.TryParse(e.CommandArgument.ToString(), out int page))
            {
                CurrentPage = Math.Max(1, Math.Min(TotalPages, page));
                LoadData();
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtGo.Text, out int page))
            {
                CurrentPage = Math.Max(1, Math.Min(TotalPages, page));
                LoadData();
            }
            else
            {
                Funciones.MostrarToast("Número de página inválido.", "warning", "top-0 end-0", 3000);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            LoadData();
        }

        protected void LocationSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = 1;
            LoadData();
        }

        protected void CategorySelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = 1;
            LoadData();
        }

        protected void StockSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = 1;
            LoadData();
        }

        protected void ddlPagination_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = 1;           // cambia el tamaño: vuelve al inicio
            TotalPages = GetTotalPages();
            LoadData();
        }


        // ====== Dropdowns para vista de edición ======
        private void LoadDropdownCategoriaEdit()
        {
            FuncionesMes.LlenarDropDownList(
                ddlCategoriaEdit,
                "[dbo].[SP_IndirectMaterials_Category]",
                new[] { "@TransactionCode" },
                new[] { "S" },
                "-- Selecciona categoría --",
                "0",
                "CategoryName",
                "CategoryId"
            );
        }

        private void LoadDropdownLocacionEdit()
        {
            FuncionesMes.LlenarDropDownList(
                ddlLocacionEdit,
                "[dbo].[SP_IndirectMaterials_Locacion]",
                new[] { "@TransactionCode" },
                new[] { "S" },
                "-- Selecciona locación --",
                "0",
                "NombreLocation",
                "LocationId"
            );
        }

        private void EnsureUploadFolder()
        {
            var phys = Server.MapPath(UPLOAD_DIR);
            if (!Directory.Exists(phys)) Directory.CreateDirectory(phys);
        }

        // ====== GRID: Fallback de imagen + click modal corregido ======
        protected void tblInventory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            var img = (Image)e.Row.FindControl("imgProducto");
            var lnk = (LinkButton)e.Row.FindControl("lnkImagen");
            if (img != null)
            {
                var url = string.IsNullOrWhiteSpace(img.ImageUrl) ? DEFAULT_IMG : img.ImageUrl;
                if (!FileExists(url)) url = DEFAULT_IMG;
                img.ImageUrl = ResolveUrl(url);

                if (lnk != null)
                {
                    // Garantiza que el modal abra la imagen efectiva
                    lnk.OnClientClick = $"mostrarModalImagen('{ResolveUrl(url)}'); return false;";
                }
            }
        }

        private bool FileExists(string virtualPath)
        {
            try { return File.Exists(Server.MapPath(virtualPath)); }
            catch { return false; }
        }

        // ====== GRID: Comandos ======
        protected void tblInventory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int productId = Convert.ToInt32(e.CommandArgument);
                CargarEdicion(productId);
            }
            else if (e.CommandName == "VerImagen")
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
        // ====== Cargar datos a la vista de edición ======
        private void CargarEdicion(int productId)
        {
            // Lee un producto por Id
            using (var r = FuncionesMes.ExecuteReader(
                "[dbo].[SP_IndirectMaterials_Products]",
                new[] { "@TransactionCode", "@ProductId" },
                new[] { "S", productId.ToString() }))
            {
                if (r.Read())
                {
                    var partNumb = Convert.ToString(r["PartNumb"]);
                    var descripcion = Convert.ToString(r["ProductDescription"]);
                    var categoriaId = Convert.ToInt32(r["CategoryId"]);
                    var locacionId = Convert.ToInt32(r["Location"]);
                    var imgDb = Convert.ToString(r["ProductImg"]);

                    hfProductId.Value = productId.ToString();
                    hfPartNumb.Value = partNumb;
                    txtNpEdit.Text = partNumb;
                    txtDescripcionEdit.Text = descripcion;

                    // Seleccionar ddl (si no existe valor, queda en 0)
                    if (ddlCategoriaEdit.Items.FindByValue(categoriaId.ToString()) != null)
                        ddlCategoriaEdit.SelectedValue = categoriaId.ToString();

                    if (ddlLocacionEdit.Items.FindByValue(locacionId.ToString()) != null)
                        ddlLocacionEdit.SelectedValue = locacionId.ToString();

                    // Imagen preview con fallback
                    var url = string.IsNullOrWhiteSpace(imgDb) ? DEFAULT_IMG : imgDb;
                    if (!FileExists(url)) url = DEFAULT_IMG;
                    imgPreview.ImageUrl = ResolveUrl(url);

                    mvInventario.ActiveViewIndex = 1; // --> Ir a vista de edición
                }
                else
                {
                    Funciones.MostrarToast("No se encontró el artículo a editar.", "warning", "top-0 end-0", 3000);
                }
            }
        }

        // ====== Guardar cambios ======
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(hfPartNumb.Value))
            {
                Funciones.MostrarToast("NP inválido.", "warning", "top-0 end-0", 3000);
                return;
            }

            int categoryId = int.TryParse(ddlCategoriaEdit.SelectedValue, out var c) ? c : 0;
            int locationId = int.TryParse(ddlLocacionEdit.SelectedValue, out var l) ? l : 0;
            string descripcion = string.IsNullOrWhiteSpace(txtDescripcionEdit.Text) ? null : txtDescripcionEdit.Text.Trim();

            // Si subió imagen, guárdala y usa la ruta relativa
            string newImageVirtualPath = null;
            if (fuImagen.HasFile)
            {
                if (!ValidImageExtension(fuImagen.FileName))
                {
                    Funciones.MostrarToast("Formato de imagen no permitido. Usa .jpg, .jpeg o .png.", "danger", "top-0 end-0", 4000);
                    return;
                }
                if (fuImagen.PostedFile.ContentLength > 4 * 1024 * 1024)
                {
                    Funciones.MostrarToast("La imagen supera 4 MB.", "danger", "top-0 end-0", 4000);
                    return;
                }

                int productId = int.TryParse(hfProductId.Value, out var pid) ? pid : 0;
                newImageVirtualPath = GuardarImagenEnProyecto(productId, hfPartNumb.Value, fuImagen);
            }

            // Construye parámetros para U (solo lo editable + imagen si aplica)
            var pnames = new List<string> { "@TransactionCode", "@PartNumb" };
            var pvals = new List<string> { "U", hfPartNumb.Value };

            pnames.Add("@ProductDescription"); pvals.Add(descripcion);
            pnames.Add("@CategoryId"); pvals.Add(categoryId > 0 ? categoryId.ToString() : null);
            pnames.Add("@Location"); pvals.Add(locationId > 0 ? locationId.ToString() : null);

            if (!string.IsNullOrEmpty(newImageVirtualPath))
            {
                pnames.Add("@ProductImg"); pvals.Add(newImageVirtualPath);
            }

            // Ejecuta SP de actualización
            try
            {
                using (var r = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_Products]",
                    pnames.ToArray(), pvals.ToArray()))
                {
                    // Si usas SP_SelectReturnValue, puedes leer resultado si lo necesitas
                }

                Funciones.MostrarToast("Artículo actualizado correctamente.", "success", "top-0 end-0", 3000);
                mvInventario.ActiveViewIndex = 0;  // regreso a listado
                LoadData();                        // recargar grilla
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast("Error al actualizar: " + ex.Message, "danger", "top-0 end-0", 4000);
            }
        }

        private bool ValidImageExtension(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return ext == ".jpg" || ext == ".jpeg" || ext == ".png";
        }

        private string GuardarImagenEnProyecto(int productId, string partNumb, FileUpload fu)
        {
            EnsureUploadFolder();

            string ext = Path.GetExtension(fu.FileName).ToLowerInvariant();
            string safeNp = SanitizeForFileName(partNumb);
            string fileName = $"{productId}_{safeNp}{ext}";
            string virtualPath = $"{UPLOAD_DIR}/{fileName}";
            string physicalPath = Server.MapPath(virtualPath);

            fu.SaveAs(physicalPath);
            return virtualPath; // Guarda ruta relativa en DB
        }

        private string SanitizeForFileName(string input)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                input = input.Replace(c, '_');
            }
            return input.Trim();
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            mvInventario.ActiveViewIndex = 0;
        }

        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(hfDeleteId.Value, out var productId) || productId <= 0)
            {
                Funciones.MostrarToast("ID inválido.", "warning", "top-0 end-0", 3000);
                return;
            }

            try
            {
                // Soft delete vía SP: TransactionCode = 'D'
                using (var r = FuncionesMes.ExecuteReader(
                    "[dbo].[SP_IndirectMaterials_Products]",
                    new[] { "@TransactionCode", "@ProductId" },
                    new[] { "D", productId.ToString() }))
                {
                    // Si tu SP devuelve algo por SP_SelectReturnValue puedes leerlo aquí si deseas
                }

                // Cerrar el modal en el cliente (si está abierto)
                ScriptManager.RegisterStartupScript(this, GetType(), "hideDelModal",
                    "try{ bootstrap.Modal.getInstance(document.getElementById('confirmDeleteModal'))?.hide(); }catch(e){}", true);


                ScriptManager.RegisterStartupScript(this, GetType(), "forceBackdropCleanup",
           "document.body.classList.remove('modal-open');" +
           "document.body.style.removeProperty('padding-right');" +
           "document.querySelectorAll('.modal-backdrop').forEach(b => b.remove());", true);

                Funciones.MostrarToast($"Se eliminó el número de parte {hfDeleteNp.Value}.", "success", "top-0 end-0", 3000);




                // Refrescar grilla manteniendo filtros/paginación actuales si quieres;
                // aquí lo simple es recargar los datos
                LoadData();
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast("Error al eliminar: " + ex.Message, "danger", "top-0 end-0", 4000);
            }
        }


    }//END
}
