using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplicationPMRO2.Utilities;
using static System.Net.Mime.MediaTypeNames;

namespace WebApplicationPMRO2.Pages.Administracion
{
    public partial class GestionUsuarios : System.Web.UI.Page
    {

  
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
              //  LoadData();
                //LoadDropdownModuloAdd();
                //ddlRol.Items.Add(new ListItem("Seleccione un Modulo antes del Rol", "0"));
            }
        }



        protected void LoadData()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("nombreEmpleado", typeof(string));
            dt.Columns.Add("puesto", typeof(string));
            dt.Columns.Add("globalId", typeof(string));
            dt.Columns.Add("correo", typeof(string));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("RolId", typeof(int));
            dt.Columns.Add("ModuloId", typeof(int));

            using (SqlDataReader reader = Funciones.ExecuteReader(
                "[Administracion].[SP_Gestion_Usuarios]",
                new[] { "@TransactionCode" },
                new[] { "S"}))
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["nombreEmpleado"] = reader["nombreEmpleado"];
                    row["puesto"] = reader["puesto"];
                    row["globalId"] = reader["globalId"];
                    row["correo"] = reader["correo"];
                    row["Nombre"] = reader["Nombre"];
                    row["Id"] = reader["Id"];
                    row["RolId"] = reader["RolId"];
                    row["ModuloId"] = reader["ModuloId"];
                    dt.Rows.Add(row);
                }
            }
            tblUser.DataSource = dt;
            tblUser.DataBind();

        }



        protected void btnAdd_Click(object sender, EventArgs e)
        {
            mvwContainer.SetActiveView(viewRecord);

        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CleanElements();
            mvwContainer.SetActiveView(viewMaintenance);
        }


        protected void Modulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = ddlModulo.SelectedValue;
            if (value != "0")
            {
                LoadDropdownRol(value);
            }
            else
            {
                ddlRol.Items.Clear();
                ddlRol.Items.Add(new ListItem("Seleccione un Rol", "0"));
               // LoadData();
            }
        }

        //realizar un insert a la base de datos 
        protected void btnAddUser_Click(object sender, EventArgs e)
        {

            try
            {



                if (string.IsNullOrEmpty(txtnombre.Text) || string.IsNullOrEmpty(txtcorreo.Text) ||
                    string.IsNullOrEmpty(txtglobalId.Text) || string.IsNullOrEmpty(txtpuesto.Text) || ddlRol.SelectedValue == "0")
                {
                    Funciones.MostrarToast("debe de completar todos los campos", "danger", "top-0 end-0", 3000);

                }


                if (btnAddUser.Text == "Actualizar" && !string.IsNullOrEmpty(TextId.Text))
                {

                    using (SqlDataReader reader = Funciones.ExecuteReader(
                    "[Administracion].[SP_Gestion_Usuarios]",
                    new[] { "@TransactionCode", "@nombreEmpleado", "@correo", "@puesto", "@globalId", "@RolId","@UserId" },
                    new[] { "U", txtnombre.Text, txtcorreo.Text, txtpuesto.Text, txtglobalId.Text, ddlRol.SelectedValue,TextId.Text}))
                    {
                        if (reader.Read())
                        {

                            if (reader["Resultado"].ToString() == "1")
                            {
                                Funciones.MostrarToast("Usuario Actulizado Correctamente", "success", "top-0 end-0", 3000);
                                LoadData();

                               
                            }


                        }
                    }


                }
                else if(btnAddUser.Text == "Agregar Usuario")
                {
                    using (SqlDataReader reader = Funciones.ExecuteReader(
                    "[Administracion].[SP_Gestion_Usuarios]",
                    new[] { "@TransactionCode", "@nombreEmpleado", "@correo", "@puesto", "@globalId", "@RolId" },
                    new[] { "I", txtnombre.Text, txtcorreo.Text, txtpuesto.Text, txtglobalId.Text, ddlRol.SelectedValue }))
                    {
                        if (reader.Read())
                        {

                            if (reader["Resultado"].ToString() == "1")
                            {
                                Funciones.MostrarToast("Usuario agregado correctamente", "success", "top-0 end-0", 3000);
                                LoadData();
                                CleanElements();
                                txtpuesto.Text = string.Empty;
                                txtcorreo.Text = string.Empty;
                                txtglobalId.Text = string.Empty;
                                txtnombre.Text = string.Empty;
                            }


                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Funciones.MostrarToast("Error al agregar el usuario: " + ex.Message, "danger", "top-0 end-0", 3000);
            }
            

            }//end





        protected void CleanElements()
        {
            ddlModulo.SelectedIndex = 0;
            ddlRol.Items.Clear();
            ddlRol.Items.Add(new ListItem("Seleccione un Modulo antes del Rol", "0"));
            txtnombre.Text = string.Empty;
            txtcorreo.Text = string.Empty;
            txtglobalId.Text = string.Empty;
            txtpuesto.Text = string.Empty;
            TextId.Text = string.Empty;
            btnAddUser.Text = "Agregar Usuario";

        }

  
        protected void tblUser_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                // mvwContainer.SetActiveView(viewRecord);
                int id = Convert.ToInt32(e.CommandArgument);
                DataTable dt = (DataTable)tblUser.DataSource;
                if (dt == null)
                {
                    LoadData(); // Asegúrate de tener datos cargados
                    dt = (DataTable)tblUser.DataSource;
                }

                DataRow[] rows = dt.Select("Id = " + id);


                if (rows.Length > 0)
                {
                    // Cargar los datos del usuario seleccionado en los controles de edición
                    DataRow row = rows[0];
                    txtnombre.Text = row["nombreEmpleado"].ToString();
                    txtcorreo.Text = row["correo"].ToString();
                    txtglobalId.Text = row["globalId"].ToString();
                    txtpuesto.Text = row["puesto"].ToString();
                    ddlModulo.SelectedValue = row["ModuloId"].ToString();
                    LoadDropdownRol(ddlModulo.SelectedValue);
                    ddlRol.SelectedValue = row["RolId"].ToString();
                    TextId.Text = row["Id"].ToString();
                    btnAddUser.Text = "Actualizar";

                    mvwContainer.SetActiveView(viewRecord);

                }


           }else if(e.CommandName == "Eliminar") 
            {

                int id = Convert.ToInt32(e.CommandArgument);
                DataTable dt = (DataTable)tblUser.DataSource;
                if (dt == null)
                {
                    LoadData(); // Asegúrate de tener datos cargados
                    dt = (DataTable)tblUser.DataSource;
                }

                DataRow[] rows = dt.Select("Id = " + id);
                if (rows.Length > 0)
                {
                    hfIdMenuE.Value = rows[0]["Id"].ToString();
                    litNombreMenuEliminar.Text = "<strong>" + rows[0]["nombreEmpleado"].ToString() + "?" + "</strong>";

                    ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", "$('#modalEliminar').modal('show');", true);
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
                    if (reader.Read())
                    {
                        if (reader["Resultado"].ToString() == "1")
                        {
                            Funciones.MostrarToast("Usuario eliminado correctamente", "success", "top-0 end-0", 3000);
                            LoadData();


                            ScriptManager.RegisterStartupScript(this, GetType(), "ocultarModal", @"
                            $('#modalEliminar').modal('hide');
                            $('body').removeClass('modal-open');
                            $('.modal-backdrop').remove();", true);
                        }
                        else
                        {
                            Funciones.MostrarToast("Error al eliminar el usuario", "danger", "top-0 end-0", 3000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast("Error al eliminar el usuario: " + ex.Message, "danger", "top-0 end-0", 3000);
            }
        }


        protected void LoadDropdownModuloAdd()
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

        protected void LoadDropdownRol(string value)
        {
            Funciones.LlenarDropDownList(
         ddlRol,
             "[Administracion].[SP_ModulosRoles]",
             new[] { "@TransactionCode", "@ModuloId" },
             new[] { "SR", value },
             "Seleccione un Rol",
             "0",
             "NombreRol",
             "Id"
         );
        }

    }//END
}