using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplicationPMRO2.Utilities;

namespace WebApplicationPMRO2.Pages.Administracion
{
    public partial class GestionUsuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Vista por defecto: listado
                mvwContainer.SetActiveView(viewMaintenance);
                LoadData(null);
            }
        }

        // ========= Listado y Búsqueda =========

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            LoadData(txtBuscarNombre.Text?.Trim());
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBuscarNombre.Text = string.Empty;
            LoadData(null);
        }

        private void LoadData(string filtroNombre)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("nombreEmpleado", typeof(string));
            dt.Columns.Add("puesto", typeof(string));
            dt.Columns.Add("globalId", typeof(string));
            dt.Columns.Add("correo", typeof(string));

            // Llamada al SP con o sin filtro
            if (!string.IsNullOrWhiteSpace(filtroNombre))
            {
                using (SqlDataReader reader = Funciones.ExecuteReader(
                    "[Administracion].[SP_Gestion_Usuarios]",
                    new[] { "@TransactionCode", "@nombreEmpleado" },
                    new[] { "S", filtroNombre }))
                {
                    while (reader.Read())
                    {
                        var row = dt.NewRow();
                        row["Id"] = Convert.ToInt32(reader["Id"]);
                        row["nombreEmpleado"] = reader["nombreEmpleado"].ToString();
                        row["puesto"] = reader["puesto"].ToString();
                        row["globalId"] = reader["globalId"].ToString();
                        row["correo"] = reader["correo"].ToString();
                        dt.Rows.Add(row);
                    }
                }
            }
            else
            {
                using (SqlDataReader reader = Funciones.ExecuteReader(
                    "[Administracion].[SP_Gestion_Usuarios]",
                    new[] { "@TransactionCode" },
                    new[] { "S" }))
                {
                    while (reader.Read())
                    {
                        var row = dt.NewRow();
                        row["Id"] = Convert.ToInt32(reader["Id"]);
                        row["nombreEmpleado"] = reader["nombreEmpleado"].ToString();
                        row["puesto"] = reader["puesto"].ToString();
                        row["globalId"] = reader["globalId"].ToString();
                        row["correo"] = reader["correo"].ToString();
                        dt.Rows.Add(row);
                    }
                }
            }

            tblUser.DataSource = dt;
            tblUser.DataBind();
        }

        // ========= Alta / Edición =========

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            CleanElements();
            btnAddUser.Text = "Agregar Usuario";
            mvwContainer.SetActiveView(viewRecord);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CleanElements();
            mvwContainer.SetActiveView(viewMaintenance);
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            // Validación del lado servidor (además de los Validators en front)
            var nombre = (txtnombre.Text ?? "").Trim();
            var correo = (txtcorreo.Text ?? "").Trim();
            var globalId = (txtglobalId.Text ?? "").Trim();
            var puesto = (txtpuesto.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(globalId) ||
                string.IsNullOrWhiteSpace(puesto))
            {
                Funciones.MostrarToast("Debes completar todos los campos.", "danger", "top-0 end-0", 3000);
                return;
            }

            if (!IsValidEmail(correo))
            {
                Funciones.MostrarToast("El correo no tiene un formato válido.", "danger", "top-0 end-0", 3000);
                return;
            }

            if (!Regex.IsMatch(globalId, @"^[A-Za-z0-9._\-]+$"))
            {
                Funciones.MostrarToast("GlobalId solo admite letras, números, punto, guion y guion bajo.", "danger", "top-0 end-0", 3000);
                return;
            }

            try
            {
                if (btnAddUser.Text == "Actualizar" && !string.IsNullOrEmpty(TextId.Text))
                {
                    using (SqlDataReader reader = Funciones.ExecuteReader(
                        "[Administracion].[SP_Gestion_Usuarios]",
                        new[] { "@TransactionCode", "@UserId", "@nombreEmpleado", "@correo", "@puesto", "@globalId" },
                        new[] { "U", TextId.Text, nombre, correo, puesto, globalId }))
                    {
                        if (reader.Read())
                        {
                            var resultado = reader["Resultado"]?.ToString();
                            var message = SafeRead(reader, "Message");

                            if (resultado == "1")
                            {
                                Funciones.MostrarToast("Usuario actualizado correctamente.", "success", "top-0 end-0", 3000);
                                CleanElements();
                                mvwContainer.SetActiveView(viewMaintenance);
                                LoadData(txtBuscarNombre.Text?.Trim());
                            }
                            else
                            {
                                Funciones.MostrarToast(!string.IsNullOrWhiteSpace(message) ? message : "No se pudo actualizar el usuario.", "danger", "top-0 end-0", 4000);
                            }
                        }
                    }
                }
                else // Insert
                {
                    using (SqlDataReader reader = Funciones.ExecuteReader(
                        "[Administracion].[SP_Gestion_Usuarios]",
                        new[] { "@TransactionCode", "@nombreEmpleado", "@correo", "@puesto", "@globalId" },
                        new[] { "I", nombre, correo, puesto, globalId }))
                    {
                        if (reader.Read())
                        {
                            var resultado = reader["Resultado"]?.ToString();
                            var message = SafeRead(reader, "Message");

                            if (resultado == "1")
                            {
                                Funciones.MostrarToast("Usuario agregado correctamente.", "success", "top-0 end-0", 3000);
                                CleanElements();
                                mvwContainer.SetActiveView(viewMaintenance);
                                LoadData(txtBuscarNombre.Text?.Trim());
                            }
                            else
                            {
                                Funciones.MostrarToast(!string.IsNullOrWhiteSpace(message) ? message : "No se pudo agregar el usuario.", "danger", "top-0 end-0", 4000);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast("Error al guardar el usuario: " + ex.Message, "danger", "top-0 end-0", 4000);
            }
        }

        // ========= Acciones de la tabla =========

        protected void tblUser_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var row = GetUsuarioById(id);
                if (row != null)
                {
                    txtnombre.Text = row["nombreEmpleado"].ToString();
                    txtcorreo.Text = row["correo"].ToString();
                    txtglobalId.Text = row["globalId"].ToString();
                    txtpuesto.Text = row["puesto"].ToString();
                    TextId.Text = row["Id"].ToString();

                    btnAddUser.Text = "Actualizar";
                    mvwContainer.SetActiveView(viewRecord);
                }
                else
                {
                    Funciones.MostrarToast("No se encontró el usuario seleccionado.", "warning", "top-0 end-0", 3000);
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var row = GetUsuarioById(id);
                if (row != null)
                {
                    hfIdMenuE.Value = row["Id"].ToString();
                    litNombreMenuEliminar.Text = "<strong>" + Server.HtmlEncode(row["nombreEmpleado"].ToString()) + "</strong>";
                    ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", "$('#modalEliminar').modal('show');", true);
                }
                else
                {
                    Funciones.MostrarToast("No se encontró el usuario seleccionado.", "warning", "top-0 end-0", 3000);
                }
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(hfIdMenuE.Value);
                using (SqlDataReader reader = Funciones.ExecuteReader(
                    "[Administracion].[SP_Gestion_Usuarios]",
                    new[] { "@TransactionCode", "@UserId" },
                    new[] { "D", id.ToString() }))
                {
                    if (reader.Read() && reader["Resultado"]?.ToString() == "1")
                    {
                        Funciones.MostrarToast("Usuario eliminado correctamente.", "success", "top-0 end-0", 3000);
                        LoadData(txtBuscarNombre.Text?.Trim());
                        // Cerrar modal
                        ScriptManager.RegisterStartupScript(this, GetType(), "ocultarModal", @"
                            $('#modalEliminar').modal('hide');
                            $('body').removeClass('modal-open');
                            $('.modal-backdrop').remove();", true);
                    }
                    else
                    {
                        Funciones.MostrarToast("Error al eliminar el usuario.", "danger", "top-0 end-0", 3000);
                    }
                }
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast("Error al eliminar el usuario: " + ex.Message, "danger", "top-0 end-0", 3000);
            }
        }

        // ========= Utilidades =========

        private void CleanElements()
        {
            txtnombre.Text = string.Empty;
            txtcorreo.Text = string.Empty;
            txtglobalId.Text = string.Empty;
            txtpuesto.Text = string.Empty;
            TextId.Text = string.Empty;
            btnAddUser.Text = "Agregar Usuario";
        }

        private DataRow GetUsuarioById(int id)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("nombreEmpleado", typeof(string));
            dt.Columns.Add("puesto", typeof(string));
            dt.Columns.Add("globalId", typeof(string));
            dt.Columns.Add("correo", typeof(string));

            using (SqlDataReader reader = Funciones.ExecuteReader(
                "[Administracion].[SP_Gestion_Usuarios]",
                new[] { "@TransactionCode", "@UserId" },
                new[] { "S", id.ToString() }))
            {
                while (reader.Read())
                {
                    var row = dt.NewRow();
                    row["Id"] = Convert.ToInt32(reader["Id"]);
                    row["nombreEmpleado"] = reader["nombreEmpleado"].ToString();
                    row["puesto"] = reader["puesto"].ToString();
                    row["globalId"] = reader["globalId"].ToString();
                    row["correo"] = reader["correo"].ToString();
                    dt.Rows.Add(row);
                }
            }

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            // Regex simple y robusta para email
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
        }

        private static string SafeRead(SqlDataReader reader, string colName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(colName, StringComparison.OrdinalIgnoreCase))
                {
                    return reader.IsDBNull(i) ? null : reader.GetValue(i)?.ToString();
                }
            }
            return null;
        }
    }
}