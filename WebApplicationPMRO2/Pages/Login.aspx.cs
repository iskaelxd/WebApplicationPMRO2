using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplicationPMRO2.Pages
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string numero = txtNumeroEmpleado.Text.Trim();
            string password = txtPassword.Text;

            // TODO: sustituye por tu propio repositorio/servicio de usuarios
            

        }
    }
}