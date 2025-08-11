using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplicationPMRO2.Utilities;
using WebApplicationPMRO2.Models;
using System.Data.SqlClient;

namespace WebApplicationPMRO2
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {


                if (Funciones.SesionIniciada() == false)
                {
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    return;
                }
                LoadMenu();
            }
            else
            {
                LoadMenu();
            }


        }


        protected void LoadMenu()
        {
            List<MenuItemBD> menuItems = ObtenerMenuBD(Session["Rol"].ToString());


            foreach (var item in menuItems)
            {
                HyperLink link = new HyperLink
                {
                    CssClass = "nav-link px-3 d-flex gap-2 align-items-center",
                    NavigateUrl = item.Url,
                    Text = $"<i class='{item.Icono}'></i> <span>{item.Titulo}</span>",
                    ID = "menu_" + item.MenuId
                };
                phMenuItems.Controls.Add(link);
            }


        }


        private List<MenuItemBD> ObtenerMenuBD( string rol)
        {
            List<MenuItemBD> items = new List<MenuItemBD>();

            using (SqlDataReader reader = Funciones.ExecuteReader("[Administracion].[SP_MenuPermisos]", new[] { "@UsuarioId", "@TransactionCode" }, new[] {rol,"S"} ))

            {
                while (reader.Read()) 
                {
                    items.Add(new MenuItemBD
                    {
                        MenuId = reader.GetInt32(reader.GetOrdinal("MenuId")),
                        Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                        Url = reader.GetString(reader.GetOrdinal("Url")),
                        Icono = reader.GetString(reader.GetOrdinal("Icono")),
                        Orden = reader.GetInt32(reader.GetOrdinal("Orden"))
                    });
                }
            }

            return items;
        }



    }//END

}