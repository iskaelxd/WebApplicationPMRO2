<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Almacen.Reportes" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
   <ContentTemplate>
       <div class="row">
    <div class="col-12 d-flex justify-content-between align-items-center">
        <h3>Reportes</h3>
        <asp:DropDownList ID="ddlreport" runat="server" CssClass="form-select w-auto" AutoPostBack="true" OnSelectedIndexChanged="ddlreport_SelectedIndexChanged">
            <asp:ListItem Text="Reporte de Empleados" Value="empleados" />
            <asp:ListItem Text="Reporte de Numeros de Parte" Value="Np" />
            <asp:ListItem Text="Reporte por Area" Value="Area" />
        </asp:DropDownList>
    </div>
</div>

        <hr />

       <!--Multivista-->
       
<asp:MultiView ID="mvReportes" runat="server" ActiveViewIndex="0">


     <!--inicio de vwEmpleados-->
    <asp:View ID="vwEmpleados" runat="server">

    <div class="row">
            <div class="col-md-4">
            <asp:Label Text="Ingrese Numero empleado" CssClass="form-label" runat="server"  />
            <asp:TextBox ID="txtNp" runat="server" CssClass="form-control"  placeholder="Ingrese Numero de empleado"></asp:TextBox>
            </div>
            <div class="col-md-3 d-flex align-items-end">
            <asp:Button  ID="btnBuscar" runat="server" CssClass ="btn btn-primary" Text="Buscar"/>
            </div>
        
            <div class="col-md-5 d-flex align-items-end justify-content-end">
                    <asp:Button ID="btnVerde" runat="server" CssClass="btn btn-success" Text="Descargar Excel" />
                </div>

    </div>
    <div class="row mt-3">
        <div class="col-md-3">
        <asp:Label Text="Seleccione Status:" runat="server" ></asp:Label>
        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select" AutoPostBack="true">
        </asp:DropDownList>
       </div>


         <div class="col-md-3">
         <asp:Label Text="Seleccione Area:" runat="server" ></asp:Label>
         <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged" AutoPostBack="true">
         </asp:DropDownList>
        </div>

         <div class="col-md-3" ID="linea" runat="server" visible="false">
             <asp:Label Text="Seleccione Linea:" runat="server" ></asp:Label>
             <asp:DropDownList ID="ddlLinea" runat="server" CssClass="form-select" AutoPostBack="true">
             </asp:DropDownList>
            </div>

         <div class="col-md-3">
         <asp:Label Text="Seleccione un Supervisor:" runat="server" ></asp:Label>
         <asp:DropDownList ID="ddlSupervisor" runat="server" CssClass="form-select" AutoPostBack="true">
         </asp:DropDownList>
        </div>

    </div>

    <div class="row mt-3">
        <div class="col-md-3">
            <asp:Label Text="Aqui se veran la cantidad de registros:" runat="server"></asp:Label>
            <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" placeholder="Visualizar Datos"/>

        </div>
        <div class="col-md-3">
            <asp:Label Text="Seleccione una fecha:" runat="server"></asp:Label>
            <asp:TextBox ID="txtFechaHora" runat="server" CssClass="form-control" TextMode="Date" />

        </div>
        

        <div class="col-md-auto ms-auto d-flex align-items-end">
            <asp:DropDownList ID="ddlCantidad" runat="server" CssClass="form-select">
                <asp:ListItem Text="25" Value="25" Selected="True" />
                <asp:ListItem Text="50" Value="50" />
                <asp:ListItem Text="100" Value="100" />
            </asp:DropDownList>
        </div>

    </div>

    <div class="bd-example m-0 border-0 mt-5"> 
          <table class="table table-sm table-bordered">
            <thead>
            <tr>
              <th scope="col">#</th>
              <th scope="col">First</th>
              <th scope="col">Last</th>
              <th scope="col">Handle</th>
            </tr>
            </thead>
            <tbody>
            <tr>
              <th scope="row">1</th>
              <td>Mark</td>
              <td>Otto</td>
              <td>@mdo</td>
            </tr>
            <tr>
              <th scope="row">2</th>
              <td>Jacob</td>
              <td>Thornton</td>
              <td>@fat</td>
            </tr>
            <tr>
              <th scope="row">3</th>
              <td>John</td>
              <td>Doe</td>
              <td>@social</td>
            </tr>
            </tbody>
          </table> 

    </div>


        </asp:View>

    <!--fin de vwEmpleados-->
    

     <!--inicio de vwProducto-->

    <asp:View ID="vwProductos" runat="server">

        
            <div class="row">
            <div class="col-md-4">
            <asp:Label Text="Ingrese Numero de Parte" CssClass="form-label" runat="server"  />
            <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control"  placeholder="Ingrese Numero de Parte"></asp:TextBox>
            </div>
            <div class="col-md-3 d-flex align-items-end">
            <asp:Button  ID="btnSearch" runat="server" CssClass ="btn btn-primary" Text="Buscar"/>
            </div>
        
            <div class="col-md-5 d-flex align-items-end justify-content-end">
                    <asp:Button ID="btnDWExcel" runat="server" CssClass="btn btn-success" Text="Descargar Excel" />
                </div>

    </div>

    <div class="row mt-3">
        <div class="col-md-3">
            <asp:Label Text="Aqui se veran la cantidad de registros:" runat="server"></asp:Label>
            <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" placeholder="Visualizar Datos"/>

        </div>
        <div class="col-md-3">
            <asp:Label Text="Seleccione una fecha:" runat="server"></asp:Label>
            <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control" TextMode="Date" />

        </div>
        

        <div class="col-md-auto ms-auto d-flex align-items-end">
            <asp:DropDownList ID="DropDownList5" runat="server" CssClass="form-select">
                <asp:ListItem Text="25" Value="25" Selected="True" />
                <asp:ListItem Text="50" Value="50" />
                <asp:ListItem Text="100" Value="100" />
            </asp:DropDownList>
        </div>

    </div>

    <div class="bd-example m-0 border-0 mt-5"> 
          <table class="table table-sm table-bordered">
            <thead>
            <tr>
              <th scope="col">#</th>
              <th scope="col">First</th>
              <th scope="col">Last</th>
              <th scope="col">Handle</th>
            </tr>
            </thead>
            <tbody>
            <tr>
              <th scope="row">1</th>
              <td>Mark</td>
              <td>Otto</td>
              <td>@mdo</td>
            </tr>
            <tr>
              <th scope="row">2</th>
              <td>Jacob</td>
              <td>Thornton</td>
              <td>@fat</td>
            </tr>
            <tr>
              <th scope="row">3</th>
              <td>John</td>
              <td>Doe</td>
              <td>@social</td>
            </tr>
            </tbody>
          </table> 

    </div>


    </asp:View>

    <!--fin de vwProducto-->

    <!--inicio de vwVentas-->
    <asp:View ID="vwVentas" runat="server">


    </asp:View>
    <!--fin de vwVentas-->


</asp:MultiView>



           </ContentTemplate>
</asp:UpdatePanel>

</asp:Content>
