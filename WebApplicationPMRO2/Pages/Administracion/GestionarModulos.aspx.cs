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
    public partial class GestionarModulos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
               // LoadData();
            }
        }


        protected void LoadData() 
        { 
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("Codigo", typeof(string));


            using (SqlDataReader reader = Funciones.ExecuteReader("[Administracion].[SP_ModulosRoles]", new[] { "@TransactionCode" }, new[] { "SM" }))
            {
                if (reader == null)
                {
                    Funciones.MostrarToast("Error al obtener la informacion: ", "danger", "top-0 end-0", 3000);
                    return;
                }

                while (reader.Read())
                {
                    dt.Rows.Add(
                        reader["Id"],
                        reader["Nombre"],
                        reader["Codigo"]

                    );
                }

                tblModulo.DataSource = dt;
                tblModulo.DataBind(); // ¡No olvides hacer el DataBind!
            }

        }



        protected void btn_AddModuloClick(object sender, EventArgs e)
        {
            try
            {
                string nombreModulo = txtNombreModulo.Text.Trim();

                string codigoModulo = txtCodigoModulo.Text.Trim();

                if (string.IsNullOrEmpty(nombreModulo) || string.IsNullOrEmpty(codigoModulo))
                {
                    Funciones.MostrarToast("Por favor, complete todos los campos.", "warning", "top-0 end-0", 3000);
                    return;
                }

                using (SqlDataReader reader = Funciones.ExecuteReader("[Administracion].[SP_ModulosRoles]", new[] { "@ModuloNombre", "@Codigo", "@TransactionCode" }, new[] {nombreModulo,codigoModulo,"IM"}))
                {
                    if (reader.Read())
                    {
                        string resultado = reader["Resultado"].ToString();
                        if (resultado == "1")
                        {
                            Funciones.MostrarToast("Módulo agregado correctamente.", "success", "top-0 end-0", 3000);
                            txtNombreModulo.Text = string.Empty;
                            txtCodigoModulo.Text = string.Empty;
                            
                            LoadData(); // Recargar los datos de la tabla
                        }
                        else
                        {
                            Funciones.MostrarToast("Error al agregar el módulo: " + resultado, "danger", "top-0 end-0", 3000);
                        }
                    }
                    else
                    {
                        Funciones.MostrarToast("No se pudo obtener una respuesta del servidor.", "danger", "top-0 end-0", 3000);
                    }
                }



            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Funciones.MostrarToast("error al agregar modulo" + ex.Message, "danger","top-0 end-0",3000);
            }
        }

        protected void tblModulo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                DataTable dt = (DataTable)tblModulo.DataSource;
                if (dt == null)
                {
                    LoadData(); // Asegúrate de tener datos cargados
                    dt = (DataTable)tblModulo.DataSource;
                }

                DataRow[] rows = dt.Select("Id = " + id);
                if (rows.Length > 0)
                {
                    hfIdModulo.Value = rows[0]["Id"].ToString();
                    txtEditarNombre.Text = rows[0]["Nombre"].ToString();
                    txtEditarCodigo.Text = rows[0]["Codigo"].ToString();

                    // Mostrar el modal con JavaScript
                    ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", "$('#modalEditar').modal('show');", true);
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                DataTable dt = (DataTable)tblModulo.DataSource;
                if (dt == null)
                {
                    LoadData(); // Asegúrate de tener datos cargados
                    dt = (DataTable)tblModulo.DataSource;
                }

                DataRow[] rows = dt.Select("Id = " + id);
                if (rows.Length > 0)
                {
                    hfIdModuloE.Value = rows[0]["Id"].ToString();
                    litNombreModuloEliminar.Text = "<strong>" + rows[0]["Nombre"].ToString() + "?" + "</strong>";

                    ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", "$('#modalEliminar').modal('show');", true);
                }
            }


        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            int id = int.Parse(hfIdModulo.Value);
            string nuevoNombre = txtEditarNombre.Text.Trim();
            string nuevoCodigo = txtEditarCodigo.Text.Trim();

            if (string.IsNullOrEmpty(nuevoNombre) || string.IsNullOrEmpty(nuevoCodigo))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ocultarModal", @"
                    $('#modalEditar').modal('hide');
                    $('body').removeClass('modal-open');
                    $('.modal-backdrop').remove();", true);
                // Mostrar mensaje de error
                Funciones.MostrarToast("Por favor, complete todos los campos.", "warning", "top-0 end-0", 5000);
                return;
            }


            using (SqlDataReader reader = Funciones.ExecuteReader("[Administracion].[SP_ModulosRoles]",
                new[] { "@ModuloId", "@ModuloNombre", "@Codigo", "@TransactionCode" },
                new[] { id.ToString(), nuevoNombre, nuevoCodigo, "UM" }))
            {
                if (reader.Read() && reader["Resultado"].ToString() == "1")
                {

                    // Ocultar el modal
                    //  ScriptManager.RegisterStartupScript(this, GetType(), "ocultarModal", "$('#modalEditar').modal('hide');", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "ocultarModal", @"
                    $('#modalEditar').modal('hide');
                    $('body').removeClass('modal-open');
                    $('.modal-backdrop').remove();", true);

                    // Actualización exitosa
                    Funciones.MostrarToast("Módulo actualizado correctamente.", "success", "top-0 end-0", 3000);
                    LoadData();

                }
                else
                {
                    Funciones.MostrarToast("Error al actualizar el módulo.", "danger", "top-0 end-0", 3000);
                }
            }
        }


        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int id = int.Parse(hfIdModuloE.Value);
            using (SqlDataReader reader = Funciones.ExecuteReader("[Administracion].[SP_ModulosRoles]",
                new[] { "@ModuloId", "@TransactionCode" },
                new[] { id.ToString(), "DM" }))
            {
                if (reader.Read() && reader["Resultado"].ToString() == "1")
                {
                  
                    // Eliminación exitosa
                    Funciones.MostrarToast("Módulo eliminado correctamente.", "success", "top-0 end-0", 3000);
                    LoadData();

                    // Ocultar el modal
                    ScriptManager.RegisterStartupScript(this, GetType(), "ocultarModal", @"
                    $('#modalEliminar').modal('hide');
                    $('body').removeClass('modal-open');
                    $('.modal-backdrop').remove();", true);
                }
                else
                {
                    Funciones.MostrarToast("Error al eliminar el módulo.", "danger", "top-0 end-0", 3000);
                }
            }
        }


    } //END
}