using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplicationPMRO2.Utilities;

using System.Net;
using System.Text;
using Newtonsoft.Json;

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
                string user = txtNumeroEmpleado.Text.Trim();
                string password = txtPassword.Text;

                if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
                {
                    Funciones.MostrarToast("Por favor, ingrese número de empleado y contraseña.", "danger", "bottom-0 end-0", 3000);
                    return;
                }

                var loginData = new
                {
                    User = user,
                    Password = password
                };

                string json = JsonConvert.SerializeObject(loginData);

                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    string response = client.UploadString("http://C1234S004/MESAPI/Api/Security/Logon", "POST", json);

                    dynamic result = JsonConvert.DeserializeObject(response);

                    if (result.Result.IsValidUser == true)
                    {

                        using (SqlDataReader reader = Funciones.ExecuteReader("[Administracion].[SP_Gestion_Usuarios]", new[] { "@globalId", "@TransactionCode" }, new[] { result.Result.GlobalId.ToString(), "S" }))
                        {
                            if (reader.Read())
                            {
                                if (reader["globalId"].ToString() == result.Result.GlobalId.ToString())
                                {

                                    Session["globalId"] = result.Result.GlobalId.ToString();
                                    Session["Rol"] = reader["Id"].ToString() ?? string.Empty;
                                    Session["Username"] = result.Result.DisplayName.ToString() ?? string.Empty;
                                    Session["Correo"] = result.Result.EmailAddress.ToString() ?? string.Empty;

                                    System.Web.Security.FormsAuthentication.RedirectFromLoginPage(Session["globalId"].ToString(), false);
                                }
                                else
                                {
                                    // Aquí puedes manejar el caso de error, como mostrar un mensaje de error
                                    //JS.Show("Credenciales inválidas", "danger", "bottom-0 end-0");

                                    Funciones.MostrarToast("No tienes acceso al sistema", "danger", "bottom-0 end-0", 3000);

                                    // Console.WriteLine("Credenciales inválidas");
                                }

                            }
                            else
                            {
                                Funciones.MostrarToast("No tienes acceso al sistema", "danger", "bottom-0 end-0", 3000);
                            }
                        }


                            //Session["Username"] = result.Result.DisplayName.ToString();
                            //Session["EmployeeNumber"] = result.Result.GlobalId.ToString();
                            //Session["Email"] = result.Result.EmailAddress.ToString();

                            // FormsAuthentication.RedirectFromLoginPage(Session["EmployeeNumber"].ToString(), false);
                        }
                    else
                    {
                        Funciones.MostrarToast("Usuario inválido", "danger", "bottom-0 end-0", 3000);
                    }
                }
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast($"Error al validar usuario: {ex.Message}", "danger", "bottom-0 end-0", 5000);
            }

            //try
            //{
            //    string numero = txtNumeroEmpleado.Text.Trim();
            //    string password = txtPassword.Text;

            //    if(string.IsNullOrEmpty(numero) || string.IsNullOrEmpty(password))
            //    {
            //        // Aquí puedes manejar el caso de error, como mostrar un mensaje de error
            //        Funciones.MostrarToast("Por favor, ingrese número de empleado y contraseña.", "danger", "bottom-0 end-0", 3000);
            //        return;
            //    }


                //        else
                //        {
                //            //JS.Show("Credenciales inválidas", "danger", "bottom-0 end-0");
                //            Funciones.MostrarToast("Usuario Invalido", "danger", "bottom-0 end-0", 3000);
                //            //Console.WriteLine("Credenciales inválidas");
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    // Manejo de excepciones, como mostrar un mensaje de error
                //    Funciones.MostrarToast($"Error al validar usuario: {ex.Message}", "danger", "bottom-0 end-0", 5000);
                //    //Console.WriteLine($"Error al validar usuario: {ex.Message}");
                //    //JS.Show($"Error al validar usuario: {ex.Message}", "danger", "bottom-0 end-0");
                //}


            }
    }
}