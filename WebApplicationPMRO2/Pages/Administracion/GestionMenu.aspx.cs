using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
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
               
                //LoadDropdown();
                //LoadDropdownAdd();
                //LoadData();
            }
        }

        protected void LoadData()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("MenuId", typeof(int));
                dt.Columns.Add("ModuloNombre", typeof(string));
                dt.Columns.Add("Titulo", typeof(string));
                dt.Columns.Add("Url", typeof(string));
                dt.Columns.Add("Icono", typeof(string));
                dt.Columns.Add("Orden", typeof(int));
                dt.Columns.Add("ModuloId", typeof(int));
                string moduleId = ddlModulo.SelectedValue;
                if (moduleId == "0")
                {
                    moduleId = null; // Si no se selecciona un módulo, no filtrar por módulo
                }

                using (SqlDataReader reader = Funciones.ExecuteReader("[Administracion].[SP_GestionMenu]", new[] { "@TransactionCode", "@ModuloId" }, new[] {"S",moduleId})) 
                {
                    while (reader.Read())
                    {
                        DataRow row = dt.NewRow();
                        row["MenuId"] = reader["MenuId"];
                        row["ModuloNombre"] = reader["ModuloNombre"];
                        row["Titulo"] = reader["Titulo"];
                        row["Url"] = reader["Url"];
                        row["Icono"] = reader["Icono"];
                        row["Orden"] = reader["Orden"];
                        row["ModuloId"] = reader["ModuloId"];
                        dt.Rows.Add(row);
                    }
                    tblMenu.DataSource = dt;
                    tblMenu.DataBind(); // ¡No olvides hacer el DataBind!

                }


            }
            catch (Exception ex)
            {

                Funciones.MostrarToast($"Error al cargar los datos: {ex.Message}", "danger", "top-0 end-0", 3000);
            }
        }

        protected void LoadDropdown()
        {
            Funciones.LlenarDropDownList(
           ddlModulo,
               "[Administracion].[SP_ModulosRoles]",
               new[] { "@TransactionCode" },
               new[] { "SM" },
               "Seleccione un Módulo",
               "0",
               "Nombre",
               "Id"
           );
        }

        protected void LoadDropdownAdd()
        {
            Funciones.LlenarDropDownList(
           ddlMenu,
               "[Administracion].[SP_ModulosRoles]",
               new[] { "@TransactionCode" },
               new[] { "SM" },
               "Seleccione un Módulo",
               "0",
               "Nombre",
               "Id"
           );
        }

        protected void MenuSelected_SelectedIndexChanged (object sender, EventArgs e)
        {
            LoadData();
            // Aquí puedes manejar el evento de cambio de selección del menú si es necesario
            // Por ejemplo, podrías cargar datos específicos del menú seleccionado
        }

        protected void tblMenu_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                //Funciones.MostrarToast("Editar no implementado aún.", "info", "top-0 end-0", 3000);

                int id = Convert.ToInt32(e.CommandArgument);
                DataTable dt = (DataTable)tblMenu.DataSource;
                if (dt == null)
                {
                    LoadData(); // Asegúrate de tener datos cargados
                    dt = (DataTable)tblMenu.DataSource;
                }

                DataRow[] rows = dt.Select("MenuId = " + id);
                
                if (rows.Length > 0)
                {
                    DataRow row = rows[0];
                    // Asigna los valores a los controles de edición
                    txtTitulo.Text = row["Titulo"].ToString();
                    txtUrl.Text = row["Url"].ToString();
                    txtIcono.Text = row["Icono"].ToString();
                    txtOrden.Text = row["Orden"].ToString();
                    ddlMenu.SelectedValue = row["ModuloId"].ToString();
                    txtMenuId.Text = row["MenuId"].ToString(); // Asigna el ID del menú al campo oculto
                    mvwContainer.SetActiveView(viewRecord);
                    btnGuardar.Text = "Actualizar"; // Cambia el texto del botón a "Actualizar"
                }



            }
            else if (e.CommandName == "Eliminar")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                DataTable dt = (DataTable)tblMenu.DataSource;
                if (dt == null)
                {
                    LoadData(); // Asegúrate de tener datos cargados
                    dt = (DataTable)tblMenu.DataSource;
                }

                DataRow[] rows = dt.Select("MenuId = " + id);
                if (rows.Length > 0)
                {
                    hfIdMenuE.Value = rows[0]["MenuId"].ToString();
                    litNombreMenuEliminar.Text = "<strong>" + rows[0]["Titulo"].ToString() + "?" + "</strong>";

                    ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", "$('#modalEliminar').modal('show');", true);
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        { 
           if(string.IsNullOrEmpty(txtTitulo.Text) || string.IsNullOrEmpty(txtUrl.Text) || string.IsNullOrEmpty(txtIcono.Text) ||string.IsNullOrEmpty(txtOrden.Text) ||ddlMenu.SelectedValue == "0")
            {
                Funciones.MostrarToast("Por favor, complete todos los campos obligatorios.", "warning", "top-0 end-0", 3000);
                return;
            }

            if (btnGuardar.Text == "Actualizar" && !string.IsNullOrEmpty(txtMenuId.Text))
            {
                UpdateMenu();
                return;
            }
            
            try
            {
              using (SqlDataReader reader = Funciones.ExecuteReader("[Administracion].[SP_GestionMenu]", new[] {"@ModuloId","@Titulo","@Url","@Icono","@Orden","@TransactionCode" }, new[] {ddlMenu.SelectedValue,txtTitulo.Text,txtUrl.Text, txtIcono.Text,txtOrden.Text,"I"}  ))
                {
                    if (reader.Read() && reader["Resultado"].ToString() == "1") {

                        Funciones.MostrarToast("Menu Agregado correctamente.", "success", "top-0 end-0", 3000);
                        ddlMenu.SelectedValue = "0"; // Resetea el dropdown
                        txtTitulo.Text = string.Empty; // Resetea el campo de título
                        txtUrl.Text = string.Empty; // Resetea el campo de URL
                        txtIcono.Text = string.Empty; // Resetea el campo de icono
                        txtOrden.Text = string.Empty; // Resetea el campo de orden

                        LoadData();
                    }
                    else
                    {
                        Funciones.MostrarToast("Error al agregar el menú. Por favor, inténtelo de nuevo.", "danger", "top-0 end-0", 3000);
                    }
                }
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast($"Error al guardar el registro: {ex.Message}", "danger", "top-0 end-0", 3000);
            }
        }

        protected void UpdateMenu()
        {
            try
            {
                using (SqlDataReader reader = Funciones.ExecuteReader("[Administracion].[SP_GestionMenu]", new[] { "@MenuId", "@ModuloId", "@Titulo", "@Url", "@Icono", "@Orden", "@TransactionCode" }, new[] { txtMenuId.Text, ddlMenu.SelectedValue, txtTitulo.Text, txtUrl.Text, txtIcono.Text, txtOrden.Text, "U" }))
                {
                    if (reader.Read() && reader["Resultado"].ToString() == "1")
                    {
                        Funciones.MostrarToast("Menú actualizado correctamente.", "success", "top-0 end-0", 3000);
                        LoadData();
                    }
                    else
                    {
                        Funciones.MostrarToast("Error al actualizar el menú. Por favor, inténtelo de nuevo.", "danger", "top-0 end-0", 3000);
                    }
                }
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast($"Error al actualizar el registro: {ex.Message}", "danger", "top-0 end-0", 3000);
            }
        }
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
          
            mvwContainer.SetActiveView(viewRecord);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtTitulo.Text = string.Empty;
            txtUrl.Text = string.Empty;
            txtIcono.Text = string.Empty;
            txtOrden.Text = string.Empty;
            ddlMenu.SelectedValue = "0"; // Resetea el dropdown
            txtMenuId.Text = string.Empty; // Limpia el campo oculto del ID del menú
            btnGuardar.Text = "Guardar"; // Cambia el texto del botón a "Guardar"
            mvwContainer.SetActiveView(viewMaintenance);

        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                using(SqlDataReader reader = Funciones.ExecuteReader("[Administracion].[SP_GestionMenu]", new[] { "@MenuId", "@TransactionCode" }, new[] { hfIdMenuE.Value, "D" }))
                {
                    if (reader.Read() && reader["Resultado"].ToString() == "1")
                    {
                        Funciones.MostrarToast("Menú eliminado correctamente.", "success", "top-0 end-0", 3000);
                        LoadData();
                        ScriptManager.RegisterStartupScript(this, GetType(), "ocultarModal", @"
                            $('#modalEliminar').modal('hide');
                            $('body').removeClass('modal-open');
                            $('.modal-backdrop').remove();", true);
                    }
                    else
                    {
                        Funciones.MostrarToast("Error al eliminar el menú. Por favor, inténtelo de nuevo.", "danger", "top-0 end-0", 3000);
                    }
                }
            }
            catch (Exception ex) 
            {
                Funciones.MostrarToast($"Error al eliminar el registro: {ex.Message}", "danger", "top-0 end-0", 3000);
            }
        }


        //aqui puedo manejar el menu
        protected void MenuSeletect_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}