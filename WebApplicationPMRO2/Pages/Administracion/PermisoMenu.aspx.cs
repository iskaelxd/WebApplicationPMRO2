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
    public partial class PermisoMenu : System.Web.UI.Page
    {


        // es el load page donde se cargan las paginas y los dropdowns
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                //LoadDropdownModulo();
                //LoadDropdownModuloAdd();
                //ddlRol.Items.Add(new ListItem("Seleccione un Rol", "0"));
                //ddlMenu.Items.Add(new ListItem("Seleccione un Menú", "0"));
                //DropRol.Items.Add(new ListItem("Seleccione un Rol", "0"));
                //DropMenu.Items.Add(new ListItem("Seleccione un Menú", "0"));
                //LoadData();
            }

        }


        //aqui se cargan los permisos que tiene cada rol sobre un apartado del menu
        protected void LoadData()
        {


            string rolId = ddlRol.SelectedValue;

            string menuId = ddlMenu.SelectedValue;

            if(rolId == "0" && menuId == "0") 
            {
                rolId = null; menuId = null;
            }
            else if(menuId == "0")
            {
                menuId = null;
            }else if(rolId == "0")
            {
                rolId = null;
            }

            DataTable dt = new DataTable();

            dt.Columns.Add("NombreRol", typeof(string));
            dt.Columns.Add("NombreMenu", typeof(string));
            dt.Columns.Add("PuedeVer", typeof(bool));
            dt.Columns.Add("PuedeCrear", typeof(bool));
            dt.Columns.Add("PuedeEditar", typeof(bool));
            dt.Columns.Add("PuedeEliminar", typeof(bool));
            dt.Columns.Add("MenuId", typeof(int));
            dt.Columns.Add("RolId", typeof(int));
            dt.Columns.Add("ModuloId", typeof(int));
            dt.Columns.Add("ID", typeof(int));

            using (SqlDataReader reader = Funciones.ExecuteReader(
                "[Administracion].[PermisosMenu]",
                new[] { "@TransactionCode","@MenuId","@RolId" },
                new[] { "S",menuId,rolId }))
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["NombreRol"] = reader["RolNombre"];
                    row["NombreMenu"] = reader["MenuNombre"];
                    row["PuedeVer"] = reader["PuedeVer"];
                    row["PuedeCrear"] = reader["PuedeCrear"];
                    row["PuedeEditar"] = reader["PuedeEditar"];
                    row["PuedeEliminar"] = reader["PuedeEliminar"];
                    row["MenuId"] = reader["MenuId"];
                    row["RolId"] = reader["RolId"];
                    row["ModuloId"] = reader["ModuloId"];
                    row["ID"] = reader["ID"];
                    dt.Rows.Add(row);
                }
            }
            tblPM.DataSource = dt;
            tblPM.DataBind();
        }


        protected void tblPM_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                DataTable dt = (DataTable)tblPM.DataSource;
                if (dt == null)
                {
                    LoadData(); // Asegúrate de tener datos cargados
                    dt = (DataTable)tblPM.DataSource;
                }

                DataRow[] rows = dt.Select("ID = " + id);
                if (rows.Length > 0)
                {
                    hfIdModuloE.Value = rows[0]["ID"].ToString();
                    litNombreModuloEliminar.Text = "<strong>" + " " + rows[0]["NombreMenu"].ToString() + "</strong>" + " y el rol " + "<strong>" + rows[0]["NombreRol"].ToString() + "<strong>";

                    ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", "$('#modalEliminar').modal('show');", true);
                }
            }
        }


        protected void btnEliminarModal_Click(object sender, EventArgs e)
        {
            int id = int.Parse(hfIdModuloE.Value);
            try
            {
                using (SqlDataReader reader = Funciones.ExecuteReader(
                    "[Administracion].[PermisosMenu]",
                    new[] { "@TransactionCode", "@Id" },
                    new[] { "D", id.ToString() }))
                {
                    if (reader.Read())
                    {
                        Funciones.MostrarToast("Permiso eliminado correctamente.", "success", "top-0 end-0", 3000);
                        LoadData();
                        // Ocultar el modal
                        ScriptManager.RegisterStartupScript(this, GetType(), "ocultarModal", @"
                            $('#modalEliminar').modal('hide');
                            $('body').removeClass('modal-open');
                            $('.modal-backdrop').remove();", true);
                        //CleanElements();
                    }
                }
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast("Error al eliminar el permiso: " + ex.Message, "danger", "top-0 end-0", 3000);
            }
        }

        protected void LoadDropdownModulo()
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

        protected void LoadDropdownModuloAdd()
        {
            Funciones.LlenarDropDownList(
           DroModulo,
               "[Administracion].[SP_ModulosRoles]",
               new[] { "@TransactionCode" },
               new[] { "SM" },
               "Seleccione un Módulo",
               "0",
               "Nombre",
               "Id"
           );
        }

        protected void DroModulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DroModulo.SelectedValue != "0")
            {
                LoadDropdownRolAdd();
                LoadDropdownMenuAdd();
                Literal1.Text = string.Empty; // Limpiar el literal si hay un módulo seleccionado
                Literal2.Text = string.Empty; // Limpiar el literal de permisos
                ClearCheckBoxes();
                btnSave.Text = "Guardar"; // Resetear el botón a "Guardar"
            }
            else
            {
                DropRol.Items.Clear();
                DropRol.Items.Add(new ListItem("Seleccione un Rol", "0"));
                DropMenu.Items.Clear();
                DropMenu.Items.Add(new ListItem("Seleccione un Menú", "0"));
                Literal1.Text = string.Empty; // Limpiar el literal si no hay módulo seleccionado
                Literal2.Text = string.Empty; // Limpiar el literal de permisos
                ClearCheckBoxes();
                btnSave.Text = "Guardar"; // Resetear el botón a "Guardar"
            }
        }

        protected void LoadDropdownRolAdd()
        {
            Funciones.LlenarDropDownList(
            DropRol,
             "[Administracion].[SP_ModulosRoles]",
             new[] { "@TransactionCode", "@ModuloId" },
             new[] { "SR", DroModulo.SelectedValue },
             "Seleccione un Rol",
             "0",
             "NombreRol",
             "Id"
         );
        }



        protected void LoadDropdownMenuAdd()
        {
            Funciones.LlenarDropDownList(
            DropMenu,
                "[Administracion].[SP_ModulosRoles]",
                new[] { "@TransactionCode", "@ModuloId" },
                new[] { "SN", DroModulo.SelectedValue },
                "Seleccione un Menú",
                "0",
                "Titulo",
                "MenuId"
            );
        }


        protected  void Modulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = ddlModulo.SelectedValue;
            if (value != "0")
            {
                LoadDropdownRol(value);
                LoadDropdownMenu(value);
            }
            else
            {
                ddlRol.Items.Clear();
                ddlRol.Items.Add(new ListItem("Seleccione un Rol", "0"));
                ddlMenu.Items.Clear();
                ddlMenu.Items.Add(new ListItem("Seleccione un Menú", "0"));
                LoadData();
            }




        }

        protected void Rol_SelectedIndexChanged (object sender, EventArgs e)
        {
            LoadData();
        }

        protected void Menu_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }


        protected void LoadDropdownRol(string value)
        {
            Funciones.LlenarDropDownList(
         ddlRol,
             "[Administracion].[SP_ModulosRoles]",
             new[] { "@TransactionCode", "@ModuloId" },
             new[] { "SR",value},
             "Seleccione un Rol",
             "0",
             "NombreRol",
             "Id"
         );
        }

        protected  void LoadDropdownMenu(string value)
        {
            Funciones.LlenarDropDownList(
           ddlMenu,
               "[Administracion].[SP_ModulosRoles]",
               new[] { "@TransactionCode", "@ModuloId" },
               new[] { "SN", value},
               "Seleccione un Menú",
               "0",
               "Titulo",
               "MenuId"
           );
        }
        protected void btnAddNew_Click(object sender, EventArgs e)
        {

            mvwContainer.SetActiveView(viewRecord);
        }


        protected void CleanElements()
        {
            DroModulo.SelectedValue = "0"; // Limpiar el dropdown de módulos
            DropRol.Items.Clear();
            DropRol.Items.Add(new ListItem("Seleccione un Rol", "0"));
            DropMenu.Items.Clear();
            DropMenu.Items.Add(new ListItem("Seleccione un Menú", "0"));
            Literal1.Text = string.Empty; // Limpiar el literal si no hay módulo seleccionado
            Literal2.Text = string.Empty; // Limpiar el literal de permisos
            btnSave.Text = "Guardar"; // Resetear el botón a "Guardar"
            ClearCheckBoxes();
            mvwContainer.SetActiveView(viewMaintenance);
        }

        
        protected void btnCancel_Click(object sender, EventArgs e)
        {
           CleanElements();
        }



        //valida que el rol tenga permisos sobre el menu seleccionado
        protected void DropRol_SelectedIndexChanged(Object sender, EventArgs e) 
        {
            ClearCheckBoxes();
            Literal2.Text = string.Empty; // Limpiar el literal de permisos
            Literal1.Text = string.Empty; // Limpiar el literal de rol y menú

            if (DropRol.SelectedValue != "0" && DropMenu.SelectedValue != "0")
            {
                try
                {
                    Literal1.Text = DropRol.SelectedItem.Text + " Sobre el Menu " + DropMenu.SelectedItem.Text;

                    using (SqlDataReader reader = Funciones.ExecuteReader(
                       "[Administracion].[PermisosMenu]",
                       new[] { "@TransactionCode", "@MenuId", "@RolId" },
                       new[] { "S", DropMenu.SelectedValue, DropRol.SelectedValue }))
                    {
                        if (reader.Read())
                        {
                            chkVer.Checked = Convert.ToBoolean(reader["PuedeVer"]);
                            chkCrear.Checked = Convert.ToBoolean(reader["PuedeCrear"]);
                            chkEditar.Checked = Convert.ToBoolean(reader["PuedeEditar"]);
                            chkEliminar.Checked = Convert.ToBoolean(reader["PuedeEliminar"]);
                            if (chkVer.Checked == true || chkCrear.Checked == true || chkEditar.Checked == true || chkEliminar.Checked == true)
                            {
                                btnSave.Text = "Actualizar";
                            }
                        }
                        else
                        {
                            Literal2.Text = "No se encontraron permisos para el rol y menú seleccionados.";
                            btnSave.Text = "Guardar"; // Resetear el botón a "Guardar" si no hay permisos
                        }
                    }
                }
                catch (Exception ex)
                {
                    Funciones.MostrarToast("Error al obtener la informacion: " + ex.Message, "danger", "top-0 end-0", 3000);
                }

              //  Funciones.MostrarToast("Error al obtener la informacion: ", "danger", "top-0 end-0", 3000);
            }

        }

        private void ClearCheckBoxes()
        {
            chkVer.Checked = false;
            chkCrear.Checked = false;
            chkEditar.Checked = false;
            chkEliminar.Checked = false;
        }


        //Dropdown del menu valida que los boxes esten limpios y que el literal de permisos este vacio
        protected void DropMenu_SelectedIndexChanged(Object sender, EventArgs e)
        {
            ClearCheckBoxes();
            Literal2.Text = string.Empty; // Limpiar el literal de permisos
            Literal1.Text = string.Empty; // Limpiar el literal de permisos

            if (DropRol.SelectedValue != "0" && DropMenu.SelectedValue != "0")
            {
                try
                {
                    Literal1.Text = DropRol.SelectedItem.Text + " Sobre el Menu " + DropMenu.SelectedItem.Text;

                    using (SqlDataReader reader = Funciones.ExecuteReader(
                        "[Administracion].[PermisosMenu]",
                        new[] { "@TransactionCode", "@MenuId", "@RolId" },
                        new[] { "S", DropMenu.SelectedValue, DropRol.SelectedValue }))
                    {
                        if (reader.Read())
                        {
                            chkVer.Checked = Convert.ToBoolean(reader["PuedeVer"]);
                            chkCrear.Checked = Convert.ToBoolean(reader["PuedeCrear"]);
                           chkEditar.Checked = Convert.ToBoolean(reader["PuedeEditar"]);
                           chkEliminar.Checked = Convert.ToBoolean(reader["PuedeEliminar"]);

                            if (chkVer.Checked == true || chkCrear.Checked == true || chkEditar.Checked == true || chkEliminar.Checked == true)
                            {
                                btnSave.Text = "Actualizar";
                            }
                        }
                        else
                        {
                            Literal2.Text = "No se encontraron permisos para el rol y menú seleccionados.";
                            btnSave.Text = "Guardar";
                        }
                    }

                }
                catch (Exception ex)
                {
                    Funciones.MostrarToast("Error al obtener la informacion: " + ex.Message, "danger", "top-0 end-0", 3000);
                }

          
            }

        }



        //boton de guardar permisos y actulizar permisos

        protected void btnSave_Click(object sender, EventArgs e)
        {
            

            if(DropRol.SelectedValue == "0" || DropMenu.SelectedValue == "0" || DroModulo.SelectedValue == "0")
            {
                Funciones.MostrarToast("Debe seleccionar un rol y un menú.", "warning", "top-0 end-0", 3000);
                return;
            }


            //opcion inteligente de actualizar permisos

            if(btnSave.Text == "Actualizar") 
            {
                using (SqlDataReader reader = Funciones.ExecuteReader(
                    "[Administracion].[PermisosMenu]",
                    new[] { "@TransactionCode", "@RolId", "@MenuId", "@PuedeVer", "@PuedeCrear", "@PuedeEditar", "@PuedeEliminar" },
                    new[] { "U", DropRol.SelectedValue, DropMenu.SelectedValue, chkVer.Checked.ToString(), chkCrear.Checked.ToString(), chkEditar.Checked.ToString(), chkEliminar.Checked.ToString() }))
                {
                    if (reader.Read())
                    {
                        Funciones.MostrarToast("Permisos actualizados correctamente.", "success", "top-0 end-0", 3000);
                        LoadData();
                        CleanElements();
                    }
                }
            }


            ///corregir error del boton de actulizar y del de guardar, que no se actualiza el texto del boton

            // opcion de guardar permisos nuevos
            else if (btnSave.Text == "Guardar")
            {
                
                using(SqlDataReader reader = Funciones.ExecuteReader(
                    "[Administracion].[PermisosMenu]",
                    new[] { "@TransactionCode", "@RolId", "@MenuId", "@PuedeVer", "@PuedeCrear", "@PuedeEditar", "@PuedeEliminar" },
                    new[] { "I", DropRol.SelectedValue, DropMenu.SelectedValue, chkVer.Checked.ToString(), chkCrear.Checked.ToString(), chkEditar.Checked.ToString(), chkEliminar.Checked.ToString() }))
                {
                    if (reader.Read())
                    {
                        Funciones.MostrarToast("Permisos guardados correctamente.", "success", "top-0 end-0", 3000);
                        LoadData();
                        CleanElements();
                    }
                }
            }
            //  Funciones.MostrarToast("Guardando permisos...", "info", "top-0 end-0", 3000);
        }
    } //END
}