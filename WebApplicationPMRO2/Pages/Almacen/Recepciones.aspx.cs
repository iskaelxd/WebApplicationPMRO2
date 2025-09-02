using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplicationPMRO2.Utilities;

namespace WebApplicationPMRO2.Pages.Almacen
{
    public partial class Recepciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                LoadDropdownLocacion();
                LoadBuyers();
                LoadDropdownCategoria();
                LoadBuyer();
                LoadDropdownLocaciones();
            }
            else 
            { 
                UpdatePanel1.Update();
            }
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
            try
            {
                if (txtNumeroParte.Text == "" || txtDescripcionNumeroParte.Text == "" || ddlCategoria.SelectedValue == "0" || ddlBuyer.SelectedValue == "0" || ddlLocation.SelectedValue == "0"
                || txtMaxStock.Text == "" || txtMinStock.Text == "" || txtInventory.Text == ""  || txtUM.Text == "")
                {

                    Funciones.MostrarToast("Ingrese todos los datos", "danger", "top-0 end-0", 3000);

                    return;
                }else if (Convert.ToInt32(txtMinStock.Text) > Convert.ToInt32(txtMaxStock.Text))
                {
                    Funciones.MostrarToast("El stock minimo no puede ser mayor al maximo", "danger", "top-0 end-0", 3000);
                    return;

                }else if (Convert.ToInt32(txtInventory.Text) <= 0)
                {
                    Funciones.MostrarToast("El inventario no puede ser menor a 0", "danger", "top-0 end-0", 3000);
                    return;
                } //valida que sean numeros validos

                else if(!int.TryParse(txtMinStock.Text, out _) || !int.TryParse(txtMaxStock.Text, out _) || !int.TryParse(txtInventory.Text, out _) || !int.TryParse(txtNumeroParte.Text, out _))
                {
                    Funciones.MostrarToast("El stock minimo, maximo e inventario deben ser numeros validos", "danger", "top-0 end-0", 3000);
                    return;
                }


                else
                {

                    using (SqlDataReader reader = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_Products]",
                    new[] { "@ProductDescription", "@PartNumb", "@MinStock", "@MaxStock", "@Inventory", "@Location", "@Price", "@CategoryId", "@UM", "@Buyer", "UpdatedBy", "@TransactionCode" },
                    new[] { txtDescripcionNumeroParte.Text, txtNumeroParte.Text, txtMinStock.Text, txtMaxStock.Text, txtInventory.Text, ddlLocation.SelectedValue, "0.0", ddlCategoria.SelectedValue, txtUM.Text, ddlBuyer.SelectedValue, Session["globalId"].ToString(),"I" }))
                    {
                        //ReturnValue

                        if (reader.Read() && reader["ReturnValue"].ToString() == "-1") 
                        {
                            Funciones.MostrarToast("Datos agregados exitosamente", "success", "top-0 end-0", 3000);

                            txtNumeroParte.Text = string.Empty;
                            txtInventory.Text = string.Empty;
                            txtDescripcionNumeroParte.Text = string.Empty;
                            txtMaxStock.Text = string.Empty;
                            txtMinStock.Text = string.Empty;
                            txtUM.Text = string.Empty;
                            ddlBuyer.SelectedValue = "0";
                            ddlCategoria.SelectedValue = "0";
                            ddlLocation.SelectedValue = "0";
                        }
                        else if(reader["ReturnValue"].ToString() == "-800")
                        {
                            Funciones.MostrarToast(reader["Message"].ToString(), "danger", "top-0 end-0", 3000);

                        }

                      


                    }


                }
            }
            catch (SqlException sqlEx)
            {
          
                    Funciones.MostrarToast("Error al guardar los datos: " + sqlEx.Message, "danger", "top-0 end-0", 3000);
                
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast("Error al guardar los datos" + ex.Message + ex.StackTrace, "danger", "top-0 end-0", 3000);
               // Debug.WriteLine(ex.Message);
            }

            }




        protected void btnUpdateInventory_Click(object sender, EventArgs e)
        {
            //txtNumeroParteActualizar  txtCantidadActualizar

            try
            {

                string partNumber = string.Empty;

                if (txtNumeroParteActualizar.Text == "" || txtCantidadActualizar.Text == "")
                {
                    Funciones.MostrarToast("Ingrese todos los datos", "danger", "top-0 end-0", 3000);
                    return;
                }
                else if (!int.TryParse(txtCantidadActualizar.Text, out _) || !int.TryParse(txtNumeroParteActualizar.Text, out _))
                {
                    Funciones.MostrarToast("La cantidad debe ser un numero valido", "danger", "top-0 end-0", 3000);
                    return;
                }
                else
                {
                    using (SqlDataReader reader = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_Products]",
                    new[] { "@PartNumb", "@TransactionCode" },
                    new[] { txtNumeroParteActualizar.Text, "S" }))
                    {
                        if (reader.Read())
                        {
                            partNumber = reader["Inventory"].ToString();
                        }
                        else { 
                            Funciones.MostrarToast("El numero de parte no existe", "danger", "top-0 end-0", 3000);
                            return;
                        }


                    }


                    int currentInventory = Convert.ToInt32(partNumber) + Convert.ToInt32(txtCantidadActualizar.Text);

                    //Actualizar inventario



                    using (SqlDataReader reader = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_Products]",
                   new[] { "@PartNumb","@UpdatedBy" , "@Inventory", "@TransactionCode" },
                   new[] { txtNumeroParteActualizar.Text, Session["globalId"],currentInventory.ToString() ,"U" }))
                    {
                       if(reader.Read() && reader["ReturnValue"].ToString() == "-1")
                        {

                            Funciones.MostrarToast("Inventario actualizado exitosamente", "success", "top-0 end-0", 3000);
                            txtNumeroParteActualizar.Text = string.Empty;
                            txtCantidadActualizar.Text = string.Empty;
                        }

                    }



                }
            }
            catch (SqlException sqlEx)
            {
                Funciones.MostrarToast("Error al actualizar el inventario: " + sqlEx.Message, "danger", "top-0 end-0", 3000);
            }
            catch (Exception ex) 
            { 
                Funciones.MostrarToast("Error al actualizar el inventario: " + ex.Message, "danger", "top-0 end-0", 3000);
            }
            }


        //Dropdowns 

        protected void LoadBuyers()
        {
            FuncionesMes.LlenarDropDownList(
                ddlBuyer,
                "[dbo].[SP_IndirectMaterials_Buyer]",
                new[] { "@TransactionCode" },
                new[] { "S" },
                "Todos los Compradores",
                "0",
                "NombreBuyer",
                "BuyerId"
            );


        }



        protected void LoadBuyer()
        {
            FuncionesMes.LlenarDropDownList(
                ddlBuyers,
                "[dbo].[SP_IndirectMaterials_Buyer]",
                new[] { "@TransactionCode" },
                new[] { "S" },
                "Todos los Compradores",
                "0",
                "NombreBuyer",
                "BuyerId"
            );


        }

        private void LoadDropdownLocacion()
        {
            FuncionesMes.LlenarDropDownList(
                ddlLocation,
                "[dbo].[SP_IndirectMaterials_Locacion]",
                new[] { "@TransactionCode" },
                new[] { "S" },
                "Todas las locaciones",
                "0",
                "NombreLocation",
                "LocationId"
            );
        }


        private void LoadDropdownLocaciones()
        {
            FuncionesMes.LlenarDropDownList(
                ddlLocations,
                "[dbo].[SP_IndirectMaterials_Locacion]",
                new[] { "@TransactionCode" },
                new[] { "S" },
                "Todas las locaciones",
                "0",
                "NombreLocation",
                "LocationId"
            );
        }



        private void LoadDropdownCategoria()
        {
            FuncionesMes.LlenarDropDownList(
                ddlCategoria,
                "[dbo].[SP_IndirectMaterials_Category]",
                new[] { "@TransactionCode" },
                new[] { "S" },
                "Todas las categorias",
                "0",
                "CategoryName",
                "CategoryId"
            );
        }


        protected void ddlComprador_SelectedIndexChanged( object sender, EventArgs e) 
        {

          

            if (ddlBuyers.SelectedValue != "0")
            {
                txtBuyer.Text = ddlBuyers.SelectedItem.Text;
                ///txtBuyer.Enabled = false;
            }
            else
            {
                txtBuyer.Text = string.Empty;
                //txtBuyer.Enabled = true;
            }

        }

        protected void btnGuardarBuyer_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBuyer.Text == "")
                {
                    Funciones.MostrarToast("Ingrese el nombre del comprador", "danger", "top-0 end-0", 3000);
                    return;
                }else if(ddlBuyers.SelectedValue != "0")
                {
                    Funciones.MostrarToast("Para agregar un comprador no debe seleccionar ninguno", "danger", "top-0 end-0", 3000);
                    return;
                }
                else
                {
                    using (SqlDataReader reader = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_Buyer]",
                    new[] { "@NombreBuyer", "@TransactionCode" },
                    new[] { txtBuyer.Text, "I" }))
                    {
                        //ReturnValue
                        if (reader.Read() && reader["ReturnValue"].ToString() == "-1")
                        {
                            Funciones.MostrarToast("Datos agregados exitosamente", "success", "top-0 end-0", 3000);
                            txtBuyer.Text = string.Empty;
                            LoadBuyers();
                            LoadBuyer();
                            ddlBuyers.SelectedValue = "0";
                        }
                        else if (reader["ReturnValue"].ToString() == "-800")
                        {
                            Funciones.MostrarToast(reader["Message"].ToString(), "danger", "top-0 end-0", 3000);
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                Funciones.MostrarToast("Error al guardar los datos: " + ex.Message, "danger", "top-0 end-0", 3000);
            }
        }



        protected void btnUpdateBuyer_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBuyer.Text == "")
                {
                    Funciones.MostrarToast("Ingrese el nombre del comprador", "danger", "top-0 end-0", 3000);
                    return;
                }
                else if (ddlBuyers.SelectedValue == "0")
                {
                    Funciones.MostrarToast("Para actualizar un comprador debe seleccionar uno", "danger", "top-0 end-0", 3000);
                    return;
                }
                else
                {
                    using (SqlDataReader reader = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_Buyer]",
                    new[] { "@BuyerId", "@NombreBuyer", "@TransactionCode" },
                    new[] { ddlBuyers.SelectedValue, txtBuyer.Text, "U" }))
                    {
                        //ReturnValue
                        if (reader.Read() && reader["ReturnValue"].ToString() == "-1")
                        {
                            Funciones.MostrarToast("Datos actualizados exitosamente", "success", "top-0 end-0", 3000);
                            txtBuyer.Text = string.Empty;
                            LoadBuyers();
                            LoadBuyer();
                            ddlBuyers.SelectedValue = "0";
                        }
                        else if (reader["ReturnValue"].ToString() == "-800")
                        {
                            Funciones.MostrarToast(reader["Message"].ToString(), "danger", "top-0 end-0", 3000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast("Error al actualizar los datos: " + ex.Message, "danger", "top-0 end-0", 3000);
            }
        }

        protected void btnEliminarBuyer_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtBuyer.Text == "")
                {
                    Funciones.MostrarToast("Ingrese el nombre del comprador", "danger", "top-0 end-0", 3000);
                    return;
                }
                else if (ddlBuyers.SelectedValue == "0")
                {
                    Funciones.MostrarToast("Para actualizar un comprador debe seleccionar uno", "danger", "top-0 end-0", 3000);
                    return;
                }
                else
                {
                    using (SqlDataReader reader = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_Buyer]",
                    new[] { "@BuyerId", "@TransactionCode" },
                    new[] { ddlBuyers.SelectedValue, "D" }))
                    {
                        //ReturnValue
                        if (reader.Read() && reader["ReturnValue"].ToString() == "-1")
                        {
                            Funciones.MostrarToast("Datos actualizados exitosamente", "success", "top-0 end-0", 3000);
                            txtBuyer.Text = string.Empty;

                            LoadBuyers();
                            LoadBuyer();
                            ddlBuyers.SelectedValue = "0";
                        }
                        else if (reader["ReturnValue"].ToString() == "-800")
                        {
                            Funciones.MostrarToast(reader["Message"].ToString(), "danger", "top-0 end-0", 3000);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Funciones.MostrarToast("Error al eliminar los datos: " + ex.Message, "danger", "top-0 end-0", 3000);
            }
        }


        protected void btnSaveLocation_Click(object sender, EventArgs e)
        {

            try
            {
                if (txtLocation.Text == "")
                {
                    Funciones.MostrarToast("Ingrese el nombre de la locacion", "danger", "top-0 end-0", 3000);
                    return;
                }
                else if (ddlLocations.SelectedValue != "0")
                {
                    Funciones.MostrarToast("Para agregar una locacion no debe seleccionar ninguna", "danger", "top-0 end-0", 3000);
                    return;
                }
                else
                {
                    using (SqlDataReader reader = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_Locacion]",
                    new[] { "@NombreLocation", "@TransactionCode" },
                    new[] { txtLocation.Text, "I" }))
                    {
                        //ReturnValue
                        if (reader.Read() && reader["ReturnValue"].ToString() == "-1")
                        {
                            Funciones.MostrarToast("Datos agregados exitosamente", "success", "top-0 end-0", 3000);
                            txtLocation.Text = string.Empty;
                            LoadDropdownLocacion();
                            LoadDropdownLocaciones();
                            ddlLocations.SelectedValue = "0";
                        }
                        else if (reader["ReturnValue"].ToString() == "-800")
                        {
                            Funciones.MostrarToast(reader["Message"].ToString(), "danger", "top-0 end-0", 3000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Funciones.MostrarToast("Error al guardar los datos: " + ex.Message, "danger", "top-0 end-0", 3000);
            }
        }



        protected void ddlLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLocations.SelectedValue != "0")
            {
                txtLocation.Text = ddlLocations.SelectedItem.Text;
                ///txtBuyer.Enabled = false;
            }
            else
            {
                txtLocation.Text = string.Empty;
                //txtBuyer.Enabled = true;
            }
        }   


        protected void btnEditLocation_Click(object sender, EventArgs e) 
        {
            try
            {

                if (txtLocation.Text == "")
                {
                    Funciones.MostrarToast("Ingrese el nombre de la locacion", "danger", "top-0 end-0", 3000);
                    return;
                }
                else if (ddlLocations.SelectedValue == "0")
                {
                    Funciones.MostrarToast("Para actualizar una locacion debe seleccionar una", "danger", "top-0 end-0", 3000);
                    return;
                }
                else
                {
                    using (SqlDataReader reader = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_Locacion]",
                    new[] { "@LocationId", "@NombreLocation", "@TransactionCode" },
                    new[] { ddlLocations.SelectedValue, txtLocation.Text, "U" }))
                    {
                        //ReturnValue
                        if (reader.Read() && reader["ReturnValue"].ToString() == "-1")
                        {
                            Funciones.MostrarToast("Datos actualizados exitosamente", "success", "top-0 end-0", 3000);
                            txtLocation.Text = string.Empty;
                            LoadDropdownLocacion();
                            LoadDropdownLocaciones();
                            ddlLocations.SelectedValue = "0";
                        }
                        else if (reader["ReturnValue"].ToString() == "-800")
                        {
                            Funciones.MostrarToast(reader["Message"].ToString(), "danger", "top-0 end-0", 3000);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Funciones.MostrarToast("Error al actualizar los datos: " + ex.Message, "danger", "top-0 end-0", 3000);
            }
            

        }


        protected void btnEliminarLocation_Click(object sender, EventArgs e)
        {

            try
            {
                if (txtLocation.Text == "")
                {
                    Funciones.MostrarToast("Ingrese el nombre de la locacion", "danger", "top-0 end-0", 3000);
                    return;
                }
                else if (ddlLocations.SelectedValue == "0")
                {
                    Funciones.MostrarToast("Para actualizar una locacion debe seleccionar una", "danger", "top-0 end-0", 3000);
                    return;
                }
                else
                {
                    using (SqlDataReader reader = FuncionesMes.ExecuteReader("[dbo].[SP_IndirectMaterials_Locacion]",
                    new[] { "@LocationId", "@TransactionCode" },
                    new[] { ddlLocations.SelectedValue, "D" }))
                    {
                        //ReturnValue
                        if (reader.Read() && reader["ReturnValue"].ToString() == "-1")
                        {
                            Funciones.MostrarToast("Datos actualizados exitosamente", "success", "top-0 end-0", 3000);
                            txtLocation.Text = string.Empty;
                            LoadDropdownLocacion();
                            LoadDropdownLocaciones();
                            ddlLocations.SelectedValue = "0";
                        }
                        else if (reader["ReturnValue"].ToString() == "-800")
                        {
                            Funciones.MostrarToast(reader["Message"].ToString(), "danger", "top-0 end-0", 3000);
                        }
                    }
                }
            }
            catch (Exception ex) {
            
                Funciones.MostrarToast("Error al eliminar los datos: " + ex.Message, "danger", "top-0 end-0", 3000);
            }
        }



            }//END
}