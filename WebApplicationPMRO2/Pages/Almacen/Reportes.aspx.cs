using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplicationPMRO2.Pages.Almacen
{
    public partial class Reportes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                LoadDropdownArea();
                LoadDropdownStatus();
                LoadDropdownLinea();
                LoadDropdownSupervisor();
            }
        }






        //cargar dropdowns

        private void LoadDropdownArea()
        {
            FuncionesMes.LlenarDropDownList(
                ddlArea,
                "[dbo].[SP_IndirectMaterials_Area]",
                new[] { "@TransactionCode" },
                new[] { "S" },
                "Todas las Areas",
                "0",
                "AreaName",
                "AreaId"
            );
        }


        private void LoadDropdownStatus()
        {
            FuncionesMes.LlenarDropDownList(
                ddlStatus,
                "[dbo].[SP_IndirectMaterials_Status]",
                new[] { "@TransactionCode" },
                new[] { "S" },
                "Todos los Status",
                "0",
                "StatusCode",
                "StatusId"
            );
        }

        private void LoadDropdownLinea()
        {
            FuncionesMes.LlenarDropDownList(
                ddlLinea,
                "[dbo].[SP_IndirectMaterials_Line]",
                new[] { "@TransactionCode" },
                new[] { "S" },
                "Todos las lineas",
                "0",
                "LineShortName",
                "LineId"
            );
        }

        private void LoadDropdownSupervisor()
        {
            FuncionesMes.LlenarDropDownList(
                ddlSupervisor,
                "[dbo].[SP_IndirectMaterials_User]",
                new[] { "@TransactionCode", "@isAdmin" },
                new[] { "S","1" },
                "Todos los Supervisores",
                "0",
                "UserName",
                "MROUserId"
            );
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlArea.SelectedValue == "13")
            {
                linea.Visible = true;
            }
            else
            {
                linea.Visible = false;
            }
        }


        protected void ddlreport_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlreport.SelectedValue)
            {
                case "empleados":
                    mvReportes.SetActiveView(vwEmpleados);
                    break;
                case "Np":
                    mvReportes.SetActiveView(vwProductos);
                    break;
                case "Area":
                    mvReportes.SetActiveView(vwVentas);
                    break;
                default:
                    mvReportes.SetActiveView(vwEmpleados);
                    break;
            }
        }



    }
}