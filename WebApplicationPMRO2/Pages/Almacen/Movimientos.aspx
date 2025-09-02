<%@ Page Language="C#" MasterPageFile="~/Site.Master" CodeBehind="Movimientos.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Almacen.Movimientos" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    

       <h3>Descontar en SAP</h3>
        
 
    <hr />

    
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

    <div class="row">
        <div class="col-4">
            <label>Número de parte:</label>
            <asp:TextBox ID="txtPartNumb" runat="server" CssClass="form-control" MaxLength="30" placeholder="Número de parte o descripcion..." />
        </div>
        
        <div class="col-4">
            <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary mt-4" Text="Buscar" />
        </div>

    </div>

    <div class="bd-example-snippet bd-code-snippet mt-4"> 
        <div class="bd-example m-0 border-0"> 
          <table class="table table-striped">
            <thead>
            <tr>
              <th scope="col">PartNumb</th>
              <th scope="col">PartDescription</th>
              <th scope="col">Total</th>
              <th scope="col">Accion</th>
            </tr>
            </thead>
            <tbody>
            <tr>
              <th scope="row">470002</th>
              <td>DRILL CARBIDE 4.8M</td>
              <td>10</td>
              <td>
                  <button id="btnAccion" class="btn btn-warning" >Pasar a SAP</button>
              </td>
            </tr>
            <tr>
              <th scope="row">469793</th>
              <td>INSERT R266RG-16UNO1A180M 1125</td>
              <td>4</td>
              <td>
                <button id="btnAcciwon" class="btn btn-warning" >Pasar a SAP</button>
            </td>
            </tr>
            <tr>
              <th scope="row">5010620</th>
              <td>LIQ NEVER SEEZ WHITE FOOD GRADE</td>
              <td>10</td>
              <td>
                <button id="btnAccwion" class="btn btn-warning" >Pasar a SAP</button>
            </td>
            </tr>
            </tbody>
          </table> 
             </div>  
    </div>



               </ContentTemplate>
</asp:UpdatePanel>


</asp:Content>
