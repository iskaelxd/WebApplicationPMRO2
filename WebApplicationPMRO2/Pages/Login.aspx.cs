using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplicationPMRO2.Utilities; 

namespace WebApplicationPMRO2.Pages
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

            try
            {
                string numero = txtNumeroEmpleado.Text.Trim();
                string password = txtPassword.Text;

                if(string.IsNullOrEmpty(numero) || string.IsNullOrEmpty(password))
                {
                    // Aquí puedes manejar el caso de error, como mostrar un mensaje de error
                    Funciones.MostrarToast("Por favor, ingrese número de empleado y contraseña.", "danger", "bottom-0 end-0", 3000);
                    return;
                }

                using (SqlDataReader reader = Funciones.ExecuteReader("[Administracion].[SP_Gestion_Usuarios]", new[] { "@numeroEmpleado", "@Contrasena", "@TransactionCode" }, new[] { numero,password, "S" }))
                {
                    if (reader.Read())
                    {
                        if (reader["NumeroEmpleado"].ToString() == numero && reader["Contrasena"].ToString() == password)
                        {
                            
                            Session["Username"] = reader["nombreEmpleado"].ToString() ?? string.Empty;
                            Session["EmployeeNumber"] = reader["numeroEmpleado"].ToString();
                            Session["Rol"] = reader["RolId"].ToString() ?? string.Empty;

                            System.Web.Security.FormsAuthentication.RedirectFromLoginPage(Session["EmployeeNumber"].ToString(), false);
                        }
                        else
                        {
                            // Aquí puedes manejar el caso de error, como mostrar un mensaje de error
                            //JS.Show("Credenciales inválidas", "danger", "bottom-0 end-0");

                            Funciones.MostrarToast("Usuario Invalido", "danger", "bottom-0 end-0", 3000);

                           // Console.WriteLine("Credenciales inválidas");
                        }

                    }
                    else
                    {
                        //JS.Show("Credenciales inválidas", "danger", "bottom-0 end-0");
                        Funciones.MostrarToast("Usuario Invalido", "danger", "bottom-0 end-0", 3000);
                        //Console.WriteLine("Credenciales inválidas");
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones, como mostrar un mensaje de error
                Funciones.MostrarToast($"Error al validar usuario: {ex.Message}", "danger", "bottom-0 end-0", 5000);
                //Console.WriteLine($"Error al validar usuario: {ex.Message}");
                //JS.Show($"Error al validar usuario: {ex.Message}", "danger", "bottom-0 end-0");
            }


        }
    }
}