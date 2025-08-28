using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplicationPMRO2.Utilities;

namespace WebApplicationPMRO2.Pages.Administracion
{
    public partial class GestionMenu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mvwContainer.SetActiveView(viewMaintenance);
                LoadData(null);
            }
        }

        // ====== Listado y búsqueda ======

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            LoadData(txtBuscarTitulo.Text?.Trim());
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBuscarTitulo.Text = string.Empty;
            LoadData(null);
        }

        private void LoadData(string filtroTitulo)
        {
            var dt = new DataTable();
            dt.Columns.Add("MenuId", typeof(int));
            dt.Columns.Add("Titulo", typeof(string));
            dt.Columns.Add("Url", typeof(string));
            dt.Columns.Add("Icono", typeof(string));
            dt.Columns.Add("Orden", typeof(int));

            if (!string.IsNullOrWhiteSpace(filtroTitulo))
            {
                using (SqlDataReader reader = Funciones.ExecuteReader(
                    "[Administracion].[SP_GestionMenu]",
                    new[] { "@TransactionCode", "@Titulo" },
                    new[] { "S", filtroTitulo }))
                {
                    while (reader.Read())
                    {
                        var row = dt.NewRow();
                        row["MenuId"] = Convert.ToInt32(reader["MenuId"]);
                        row["Titulo"] = reader["Titulo"].ToString();
                        row["Url"] = reader["Url"].ToString();
                        row["Icono"] = reader["Icono"].ToString();
                        row["Orden"] = Convert.ToInt32(reader["Orden"]);
                        dt.Rows.Add(row);
                    }
                }
            }
            else
            {
                using (SqlDataReader reader = Funciones.ExecuteReader(
                    "[Administracion].[SP_GestionMenu]",
                    new[] { "@TransactionCode" },
                    new[] { "S" }))
                {
                    while (reader.Read())
                    {
                        var row = dt.NewRow();
                        row["MenuId"] = Convert.ToInt32(reader["MenuId"]);
                        row["Titulo"] = reader["Titulo"].ToString();
                        row["Url"] = reader["Url"].ToString();
                        row["Icono"] = reader["Icono"].ToString();
                        row["Orden"] = Convert.ToInt32(reader["Orden"]);
                        dt.Rows.Add(row);
                    }
                }
            }

            tblMenu.DataSource = dt;
            tblMenu.DataBind();
        }

        // ====== Alta / Edición ======

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            CleanForm();
            btnGuardar.Text = "Guardar";
            mvwContainer.SetActiveView(viewRecord);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CleanForm();
            mvwContainer.SetActiveView(viewMaintenance);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            var titulo = (txtTitulo.Text ?? "").Trim();
            var url = (txtUrl.Text ?? "").Trim();
            var icono = (txtIcono.Text ?? "").Trim();
            var ordenStr = (txtOrden.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(titulo) ||
                string.IsNullOrWhiteSpace(url) ||
                string.IsNullOrWhiteSpace(icono) ||
                string.IsNullOrWhiteSpace(ordenStr))
            {
                Funciones.MostrarToast("Por favor, completa todos los campos.", "danger", "top-0 end-0", 3000);
                return;
            }

            if (!int.TryParse(ordenStr, out int orden) || orden < 0)
            {
                Funciones.MostrarToast("El orden debe ser un entero válido (>= 0).", "danger", "top-0 end-0", 3000);
                return;
            }

            try
            {
                if (btnGuardar.Text == "Actualizar" && !string.IsNullOrEmpty(txtMenuId.Text))
                {
                    using (SqlDataReader reader = Funciones.ExecuteReader(
                        "[Administracion].[SP_GestionMenu]",
                        new[] { "@TransactionCode", "@MenuId", "@Titulo", "@Url", "@Icono", "@Orden" },
                        new[] { "U", txtMenuId.Text, titulo, url, icono, orden.ToString() }))
                    {
                        if (reader.Read())
                        {
                            var resultado = reader["Resultado"]?.ToString();
                            var message = SafeRead(reader, "Message");

                            if (resultado == "1")
                            {
                                Funciones.MostrarToast("Menú actualizado correctamente.", "success", "top-0 end-0", 3000);
                                CleanForm();
                                mvwContainer.SetActiveView(viewMaintenance);
                                LoadData(txtBuscarTitulo.Text?.Trim());
                            }
                            else
                            {
                                Funciones.MostrarToast(!string.IsNullOrWhiteSpace(message) ? message : "No se pudo actualizar.", "danger", "top-0 end-0", 4000);
                            }
                        }
                    }
                }
                else
                {
                    using (SqlDataReader reader = Funciones.ExecuteReader(
                        "[Administracion].[SP_GestionMenu]",
                        new[] { "@TransactionCode", "@Titulo", "@Url", "@Icono", "@Orden" },
                        new[] { "I", titulo, url, icono, orden.ToString() }))
                    {
                        if (reader.Read())
                        {
                            var resultado = reader["Resultado"]?.ToString();
                            var message = SafeRead(reader, "Message");

                            if (resultado == "1")
                            {
                                Funciones.MostrarToast("Menú agregado correctamente.", "success", "top-0 end-0", 3000);
                                CleanForm();
                                mvwContainer.SetActiveView(viewMaintenance);
                                LoadData(txtBuscarTitulo.Text?.Trim());
                            }
                            else
                            {
                                Funciones.MostrarToast(!string.IsNullOrWhiteSpace(message) ? message : "No se pudo agregar.", "danger", "top-0 end-0", 4000);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast("Error al guardar: " + ex.Message, "danger", "top-0 end-0", 4000);
            }
        }

        // ====== Acciones en la tabla ======

        protected void tblMenu_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var row = GetMenuById(id);
                if (row != null)
                {
                    txtMenuId.Text = row["MenuId"].ToString();
                    txtTitulo.Text = row["Titulo"].ToString();
                    txtUrl.Text = row["Url"].ToString();
                    txtIcono.Text = row["Icono"].ToString();
                    txtOrden.Text = row["Orden"].ToString();

                    btnGuardar.Text = "Actualizar";
                    mvwContainer.SetActiveView(viewRecord);
                }
                else
                {
                    Funciones.MostrarToast("No se encontró el registro.", "warning", "top-0 end-0", 3000);
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var row = GetMenuById(id);
                if (row != null)
                {
                    hfIdMenuE.Value = row["MenuId"].ToString();
                    litNombreMenuEliminar.Text = "<strong>" + Server.HtmlEncode(row["Titulo"].ToString()) + "</strong>";
                    ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", "$('#modalEliminar').modal('show');", true);
                }
                else
                {
                    Funciones.MostrarToast("No se encontró el registro.", "warning", "top-0 end-0", 3000);
                }
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlDataReader reader = Funciones.ExecuteReader(
                    "[Administracion].[SP_GestionMenu]",
                    new[] { "@TransactionCode", "@MenuId" },
                    new[] { "D", hfIdMenuE.Value }))
                {
                    if (reader.Read() && reader["Resultado"]?.ToString() == "1")
                    {
                        Funciones.MostrarToast("Menú eliminado correctamente.", "success", "top-0 end-0", 3000);
                        LoadData(txtBuscarTitulo.Text?.Trim());

                        ScriptManager.RegisterStartupScript(this, GetType(), "ocultarModal", @"
                            $('#modalEliminar').modal('hide');
                            $('body').removeClass('modal-open');
                            $('.modal-backdrop').remove();", true);
                    }
                    else
                    {
                        Funciones.MostrarToast("Error al eliminar el menú.", "danger", "top-0 end-0", 3000);
                    }
                }
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast("Error al eliminar: " + ex.Message, "danger", "top-0 end-0", 3000);
            }
        }

        // ====== Utilidades ======

        private void CleanForm()
        {
            txtMenuId.Text = string.Empty;
            txtTitulo.Text = string.Empty;
            txtUrl.Text = string.Empty;
            txtIcono.Text = string.Empty;
            txtOrden.Text = string.Empty;
            btnGuardar.Text = "Guardar";
        }

        private DataRow GetMenuById(int id)
        {
            var dt = new DataTable();
            dt.Columns.Add("MenuId", typeof(int));
            dt.Columns.Add("Titulo", typeof(string));
            dt.Columns.Add("Url", typeof(string));
            dt.Columns.Add("Icono", typeof(string));
            dt.Columns.Add("Orden", typeof(int));

            using (SqlDataReader reader = Funciones.ExecuteReader(
                "[Administracion].[SP_GestionMenu]",
                new[] { "@TransactionCode", "@MenuId" },
                new[] { "S", id.ToString() }))
            {
                while (reader.Read())
                {
                    var row = dt.NewRow();
                    row["MenuId"] = Convert.ToInt32(reader["MenuId"]);
                    row["Titulo"] = reader["Titulo"].ToString();
                    row["Url"] = reader["Url"].ToString();
                    row["Icono"] = reader["Icono"].ToString();
                    row["Orden"] = Convert.ToInt32(reader["Orden"]);
                    dt.Rows.Add(row);
                }
            }

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        private static string SafeRead(SqlDataReader reader, string colName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
                if (reader.GetName(i).Equals(colName, StringComparison.OrdinalIgnoreCase))
                    return reader.IsDBNull(i) ? null : reader.GetValue(i)?.ToString();
            return null;
        }
    }
}