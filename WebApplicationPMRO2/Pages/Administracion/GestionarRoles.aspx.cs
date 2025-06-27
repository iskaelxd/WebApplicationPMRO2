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
    public partial class GestionarRoles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropdownModulo();
                LoadDropdownModuloForm();
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
      


    }
}