using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;


namespace WebApplicationPMRO2.Utilities
{
    public class Funciones
    {
        /// <summary>
        public static string ConnectionString { get; set; } = ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;
        public static SqlDataReader ExecuteReader(
        string storedProc,
        string[] paramNames,
        object[] paramValues)
            {
                if (paramNames.Length != paramValues.Length)
                    throw new ArgumentException(
                        "paramNames y paramValues deben tener la misma longitud.");

                var conn = new SqlConnection(ConnectionString);
                var cmd = conn.CreateCommand();
                cmd.CommandText = storedProc;
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < paramNames.Length; i++)
                {
                    cmd.Parameters.AddWithValue(paramNames[i], paramValues[i] ?? DBNull.Value);
                }

                conn.Open();
                // Cuando cierres el reader, la conexión se cerrará automáticamente.
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }


        public static bool SesionIniciada()
        {
            try
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return false;
                }
                if (HttpContext.Current.Session["EmployeeNumber"] == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }



        
            public static void MostrarToast(string mensaje, string nivel = "success", string posicion = "top-0 end-0", int delay = 4000)
            {
                string script = $"mroShowToast('{mensaje}', '{nivel}', '{posicion}', {delay});";

                Page page = HttpContext.Current.CurrentHandler as Page;
                if (page != null && ScriptManager.GetCurrent(page) != null)
                {
                    ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), script, true);
                }
            }
        




    }

}