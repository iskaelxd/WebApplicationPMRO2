using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplicationPMRO2.Pages.Almacen
{
    public partial class Recepciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnMultiview_Click(object sender, EventArgs e)
        {
            if (btnMultiview.Text == "Actualizar Inventario")
            {
                btnMultiview.Text = "Agregar Numero Parte";
                MultiViewNumerosParte.SetActiveView(ViewActulizarNumerosParte);
            }
            else
            {
                btnMultiview.Text = "Actualizar Inventario";
                MultiViewNumerosParte.SetActiveView(ViewNumerosParte);
            }

               // MultiViewNumerosParte.SetActiveView(ViewActulizarNumerosParte);
        }

        protected void btnGuardarNumeroParte_Click(object sender, EventArgs e)
        {

        }

        //Dropdowns 

        protected void LoadBuyers()
        {
            //Llenar dropdown con Buyers


        }




    }//END
}