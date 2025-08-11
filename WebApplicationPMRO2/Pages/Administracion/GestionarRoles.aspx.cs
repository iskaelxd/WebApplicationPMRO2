using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplicationPMRO2.Utilities;

namespace WebApplicationPMRO2.Pages.Administracion
{
    public partial class GestionarRoles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               // LoadDropdownModulo();
                //LoadDropdownModuloForm();
               // LoadData();
            }
        }

        private void LoadDropdownModulo()
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

        private void LoadDropdownModuloForm()
        {
            Funciones.LlenarDropDownList(
                ddlModuloForm,
                "[Administracion].[SP_ModulosRoles]",
                new[] { "@TransactionCode" },
                new[] { "SM" },
                "Seleccione un Módulo",
                "0",
                "Nombre",
                "Id"
            );
        }

        protected void LoadDropdowndModalEdit()
        {
           Funciones.LlenarDropDownList(
            EditarModulo,
                "[Administracion].[SP_ModulosRoles]",
                new[] { "@TransactionCode" },
                new[] { "SM" },
                "Seleccione un Módulo",
                "0",
                "Nombre",
                "Id"
            );
        }



        protected void LoadData()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("IdRol", typeof(int));
                dt.Columns.Add("NombreRol", typeof(string));
                dt.Columns.Add("NombreModulo", typeof(string));

                string moduleId = ddlModulo.SelectedValue;
                if (moduleId == "0")
                {
                    moduleId = null; // Si no se selecciona un módulo, no filtrar por módulo
                }

                using (SqlDataReader reader = Funciones.ExecuteReader("[Administracion].[SP_ModulosRoles]", new[] { "@TransactionCode", "@ModuloId" }, new[] { "SR", moduleId }))
                {
                    if (reader == null)
                    {
                        Funciones.MostrarToast("Error al obtener la información: ", "danger", "top-0 end-0", 3000);
                        return;
                    }
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["Id"],
                            reader["NombreRol"],
                            reader["NombreModulo"]
                        );
                    }
                    tblRol.DataSource = dt;
                    tblRol.DataBind(); // ¡No olvides hacer el DataBind!
                }
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast($"Error al cargar los datos: {ex.Message}", "danger", "top-0 end-0", 3000);

            }
        }

        //ACCIONES DE LA TABLA
        protected void tblRol_RowCommand (object sender, GridViewCommandEventArgs e)
        {
            if(e.CommandName == "Editar")
            {
               
                int id = Convert.ToInt32(e.CommandArgument);
                DataTable dt = (DataTable)tblRol.DataSource;
                if (dt == null)
                {
                    LoadData(); // Asegúrate de tener datos cargados
                    dt = (DataTable)tblRol.DataSource;
                }

                DataRow[] rows = dt.Select("IdRol = " + id);
                LoadDropdowndModalEdit();

                if (rows.Length > 0)
                {
                    EditarModulo.SelectedValue = "0"; // Resetear el dropdown antes de asignar el valor
                    hfIdRol.Value = rows[0]["IdRol"].ToString();
                    txtEditarRol.Text = rows[0]["NombreRol"].ToString();

                    // Mostrar el modal con JavaScript
                    ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", "$('#modalEditar').modal('show');", true);
                }
            }else if(e.CommandName =="Eliminar")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                DataTable dt = (DataTable)tblRol.DataSource;
                if (dt == null)
                {
                    LoadData(); // Asegúrate de tener datos cargados
                    dt = (DataTable)tblRol.DataSource;
                }

                DataRow[] rows = dt.Select("IdRol = " + id);
                if (rows.Length > 0)
                {
                    hfIdRolE.Value = rows[0]["IdRol"].ToString();
                    litNombreRolEliminar.Text = "<strong>" + rows[0]["NombreRol"].ToString() + "?" + "</strong>";

                    ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", "$('#modalEliminar').modal('show');", true);
                }
            }
            
        }


        protected void btnAddRol_Click(object sender, EventArgs e)
        {
            try
            {
              
                string moduleId = ddlModuloForm.SelectedValue;
                string rolname = txtNombreRol.Text.Trim();
                // Aquí puedes agregar la lógica para guardar el nuevo módulo
                // Por ejemplo, llamar a un procedimiento almacenado o insertar en la base de datos
              //  Funciones.MostrarToast($"Módulo '{moduleName}' agregado correctamente.", "success", "top-0 end-0", 3000);
              if( moduleId == "0" || string.IsNullOrEmpty(rolname))
                {
                    Funciones.MostrarToast("Debe seleccionar un módulo y proporcionar un nombre de rol.", "warning", "top-0 end-0", 3000);
                    return;
                }

                using (SqlDataReader reader = Funciones.ExecuteReader("[Administracion].[SP_ModulosRoles]", new[] { "@RolNombre", "@ModuloId", "@TransactionCode" }, new[] {rolname,moduleId,"IR"}  )) 
                {
                    if (reader.Read() && reader["Resultado"].ToString() == "1") {

                        Funciones.MostrarToast("Exitoso", "success", "top-0 end-0", 4000);
                        ddlModuloForm.SelectedValue = "0"; // Resetear el dropdown después de agregar
                        txtNombreRol.Text = string.Empty; // Limpiar el campo de texto
                        LoadData(); // Recargar los datos de la tabla

                    }
                    else
                    {
                        Funciones.MostrarToast("Error al agregar el rol", "danger", "top-0 end-0", 3000);
                    }
                }

                  
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast($"Error al agregar el módulo: {ex.Message}", "danger", "top-0 end-0", 3000);
            }
        }

        protected void ModuloSeletect_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        //BOTONES DE LA VENTANA MODAL

        protected void btnActualizar_Click(object sender, EventArgs e) 
        {
            string moduleId = EditarModulo.SelectedValue;
            string rolname = txtEditarRol.Text.Trim();
            if (moduleId == "0" || string.IsNullOrEmpty(rolname))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ocultarModal", @"
                    $('#modalEditar').modal('hide');
                    $('body').removeClass('modal-open');
                    $('.modal-backdrop').remove();", true);
                Funciones.MostrarToast("Debe seleccionar un módulo y proporcionar un nombre de rol.", "warning", "top-0 end-0", 3000);
                return;
            }
            try
            {
                using (SqlDataReader reader = Funciones.ExecuteReader("[Administracion].[SP_ModulosRoles]", new[] { "@RolNombre", "@ModuloId", "@RolId", "@TransactionCode" }, new[] { rolname, moduleId, hfIdRol.Value, "UR" }))
                {
                    if (reader.Read() && reader["Resultado"].ToString() == "1")
                    {
                        Funciones.MostrarToast("Rol actualizado correctamente.", "success", "top-0 end-0", 3000);
                        txtEditarRol.Text = string.Empty; // Limpiar el campo de texto
                        LoadData(); // Recargar los datos de la tabla
                        ScriptManager.RegisterStartupScript(this, GetType(), "ocultarModal", @"
                            $('#modalEditar').modal('hide');
                            $('body').removeClass('modal-open');
                            $('.modal-backdrop').remove();", true);
                    }
                    else
                    {
                        Funciones.MostrarToast("Error al actualizar el rol.", "danger", "top-0 end-0", 3000);
                    }
                }
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast($"Error al actualizar el rol: {ex.Message}", "danger", "top-0 end-0", 3000);
            }

        }

        protected void btnEliminar_Click(object sender, EventArgs e) 
        {
            // Aquí puedes agregar la lógica para eliminar el rol
            try
            {
                using (SqlDataReader reader = Funciones.ExecuteReader("[Administracion].[SP_ModulosRoles]", new[] { "@RolId", "@TransactionCode" }, new[] { hfIdRolE.Value, "DR" }))
                {
                    if (reader.Read() && reader["Resultado"].ToString() == "1")
                    {
                        Funciones.MostrarToast("Rol eliminado correctamente.", "success", "top-0 end-0", 3000);
                        LoadData(); // Recargar los datos de la tabla
                        ScriptManager.RegisterStartupScript(this, GetType(), "ocultarModal", @"
                            $('#modalEliminar').modal('hide');
                            $('body').removeClass('modal-open');
                            $('.modal-backdrop').remove();", true);
                    }
                    else
                    {
                        Funciones.MostrarToast("Error al eliminar el rol.", "danger", "top-0 end-0", 3000);
                    }
                }
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast($"Error al eliminar el rol: {ex.Message}", "danger", "top-0 end-0", 3000);
            }
        }

    }//END
    }