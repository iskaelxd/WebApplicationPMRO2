using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplicationPMRO2.Utilities; // <- donde está FuncionesMes

namespace WebApplicationPMRO2.Pages.Administracion
{
    public partial class PermisoMenu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) lblMsg.Text = string.Empty;
        }

        // ================= Eventos UI =================

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            BuscarUsuarios(txtBuscarUsuario.Text.Trim());
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBuscarUsuario.Text = string.Empty;
            gvUsuarios.DataSource = null; gvUsuarios.DataBind();
            pnlUsuario.Visible = false; gvMenus.Visible = false;
            hfUserId.Value = string.Empty; lblUsuario.Text = string.Empty;
            lblMsg.Text = "Formulario limpio.";
        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectUser")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int userId = Convert.ToInt32(gvUsuarios.DataKeys[rowIndex].Value);

                string nombre = gvUsuarios.Rows[rowIndex].Cells[0].Text;
                string correo = gvUsuarios.Rows[rowIndex].Cells[1].Text;

                hfUserId.Value = userId.ToString();
                lblUsuario.Text = $"{nombre} ({correo})";
                pnlUsuario.Visible = true;

                CargarMenus(userId);
            }
        }

        protected void gvMenus_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ToggleInsertDelete")
            {
                if (string.IsNullOrEmpty(hfUserId.Value))
                {
                    lblMsg.Text = "Seleccione primero un usuario.";
                    return;
                }

                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int userId = int.Parse(hfUserId.Value);

                int menuId = Convert.ToInt32(gvMenus.DataKeys[rowIndex].Values["MenuId"]);
                bool tienePermiso = Convert.ToBoolean(gvMenus.DataKeys[rowIndex].Values["TienePermiso"]);

                try
                {
                    if (tienePermiso)
                        QuitarPermiso(userId, menuId); // DELETE
                    else
                        AsignarPermiso(userId, menuId); // INSERT

                    CargarMenus(userId); // refresca
                    lblMsg.Text = tienePermiso ? "Permiso eliminado." : "Permiso asignado.";
                }
                catch (Exception ex)
                {
                    lblMsg.Text = "Error: " + ex.Message;
                }
            }
        }




        protected void gvMenus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool tiene = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "TienePermiso"));

                var btn = (LinkButton)e.Row.FindControl("btnAccion");
                var lbl = (Label)e.Row.FindControl("lblEstado");

                if (btn != null)
                {
                    btn.Text = tiene ? "Eliminar" : "Asignar";
                    btn.CssClass = tiene ? "btn btn-sm btn-danger" : "btn btn-sm btn-success";
                }
                if (lbl != null)
                {
                    lbl.Text = tiene ? "Asignado" : "Sin asignar";
                    lbl.CssClass = tiene ? "badge bg-success" : "badge bg-secondary";
                }
            }
        }


        // ================= Datos (SP con ExecuteReader) =================

        private void BuscarUsuarios(string term)
        {
            try
            {

                using (SqlDataReader reader = Funciones.ExecuteReader(
                    "Administracion.SP_MenuPermisoRol",
                    new[] { "@TransactionCode", "@Search" },
                    new object[] { "Q", (object)term ?? DBNull.Value }
                ))
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    gvUsuarios.DataSource = dt;
                    gvUsuarios.DataBind();

                    pnlUsuario.Visible = false;
                    gvMenus.Visible = false;
                    lblMsg.Text = dt.Rows.Count == 0 ? "Sin usuarios que coincidan." : string.Empty;
                }
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast("Error al buscar usuarios: " + ex.Message + ex.StackTrace, "danger", "top-0 end-0", 3000);
            }
        }

        private void CargarMenus(int userId)
        {
            using (SqlDataReader reader = Funciones.ExecuteReader(
                "Administracion.SP_MenuPermisoRol",
                new[] { "@TransactionCode", "@UserId" },
                new object[] { "S", userId }
            ))
            {
                var dt = new DataTable();
                dt.Load(reader);
                gvMenus.DataSource = dt;
                gvMenus.DataBind();
                gvMenus.Visible = true;
            }
        }

        private void AsignarPermiso(int userId, int menuId)
        {
            // INSERT (PuedeVer = 1)
            using (SqlDataReader _ = Funciones.ExecuteReader(
                "Administracion.SP_MenuPermisoRol",
                new[] { "@TransactionCode", "@UserId", "@MenuId" },
                new object[] { "I", userId, menuId }
            ))
            {
                // opcionalmente podrías leer el RowsInserted si te interesa
                // var dt = new DataTable(); dt.Load(_);
            }
        }

        private void QuitarPermiso(int userId, int menuId)
        {
            // DELETE
            using (SqlDataReader _ = Funciones.ExecuteReader(
                "Administracion.SP_MenuPermisoRol",
                new[] { "@TransactionCode", "@UserId", "@MenuId" },
                new object[] { "D", userId, menuId }
            ))
            {
                // opcionalmente leer RowsDeleted
                // var dt = new DataTable(); dt.Load(_);
            }
        }
    }
}