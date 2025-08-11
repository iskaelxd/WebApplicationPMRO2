using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplicationPMRO2
{
    public class FuncionesMes
    {
        public static string ConnectionString { get; set; } = ConfigurationManager.ConnectionStrings["MESConn"].ConnectionString;
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





        public static void LlenarDropDownList(
DropDownList ddl,
string storedProcedure,
string[] parametros,
string[] valores,
string textoDefault,
string valorDefault,
string campoTexto,
string campoValor)
        {
            DataTable dt = new DataTable();

            // Agregar columnas dinámicamente si no existen
            dt.Columns.Add(campoValor);
            dt.Columns.Add(campoTexto);

            // Fila por defecto
            DataRow defaultRow = dt.NewRow();
            defaultRow[campoValor] = valorDefault;
            defaultRow[campoTexto] = textoDefault;
            dt.Rows.Add(defaultRow);

            using (SqlDataReader reader = ExecuteReader(storedProcedure, parametros, valores))
            {
                if (reader == null)
                {
                    ddl.DataSource = dt;
                    ddl.DataTextField = campoTexto;
                    ddl.DataValueField = campoValor;
                    ddl.DataBind();
                    return;
                }

                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row[campoValor] = reader[campoValor].ToString();
                    row[campoTexto] = reader[campoTexto].ToString();
                    dt.Rows.Add(row);
                }
            }

            ddl.DataSource = dt;
            ddl.DataTextField = campoTexto;
            ddl.DataValueField = campoValor;
            ddl.DataBind();
        }






    }//END
}