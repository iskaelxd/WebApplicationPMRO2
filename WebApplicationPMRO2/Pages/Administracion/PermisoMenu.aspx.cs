using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplicationPMRO2.Utilities;

namespace WebApplicationPMRO2.Pages.Administracion
{
    public partial class PermisoMenu : System.Web.UI.Page
    {
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [System.Web.Services.WebMethod]
        public static List<string> BuscarUsuarios(string prefixText, int count)
        {
            // Simulación de búsqueda en base de datos
            var usuarios = new List<(int ID, string Nombre)>
            {
                (1, "Juan Pérez"),
                (2, "Ana Gómez"),
                (3, "Carlos Ruiz")
            };

            return usuarios
                .Where(u => u.Nombre.ToLower().Contains(prefixText.ToLower()))
                .Select(u => $"{u.Nombre} | ID:{u.ID}")
                .ToList();
        }


        // es el load page donde se cargan las paginas y los dropdowns
        protected void Page_Load(object sender, EventArgs e)
        {
            

        }





        protected void btnBuscar_Click(object sender, EventArgs e)
        {
           
        }




    } //END
}