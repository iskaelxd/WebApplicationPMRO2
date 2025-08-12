<%@ Page Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="CreateOrder.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Produccion.CreateOrder" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <style>
        .card-img-top {
    width: 100%;
    height: 200px;
    object-fit: contain;
    background-color: #f8f9fa;
    border-bottom: 1px solid #ddd;
}


        .card {
    box-shadow: 0 4px 8px rgba(0,0,0,0.1);
    transition: transform 0.2s ease-in-out;
    border-radius: 10px;
}

.card:hover {
    transform: scale(1.02);
}


.card-title {
    font-size: 1.2rem;
    color: #0d6efd;
}


    </style>





    <h3>Solicitar Indirecto</h3>

    <hr />

         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
  <ContentTemplate>
         <div class="row">
         <div class="col-md-4">
         <asp:Button ID="btnOrder" runat="server" Text="Crear Orden" CssClass="btn btn-primary" OnClick="btnView_Click" />
          </div>
        </div>
    <asp:MultiView id="mvCreateOrder" runat="server" activeviewindex="0">

        <asp:View ID="vwViewOrders" runat="server">
       

    <div class="row mt-3">
        <div class="col-md-3">
            <label>Seleccione un Status</label>
               <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
               </asp:DropDownList>
        </div>

        <div class="col-md-3">
            <label>Seleccione un Area</label>
            <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-select"   OnSelectedIndexChanged="ddlArea_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
        </div>

        <div class="col-md-3"  ID="linea" runat="server" visible="false">
            <label>Seleccione una linea</label>
            <asp:DropDownList ID="ddlLinea" runat="server" CssClass="form-select">
            </asp:DropDownList>
            </div>

        <div class="col-md-3">
            <label>Seleccione una fecha</label>
            <asp:TextBox runat="server" TextMode="Date" CssClass="form-control"/>
            
        </div>

    </div>

   <div class="bd-example m-0 border-0 mt-4">
  <div class="table-responsive">
    <table class="table table-sm table-bordered table-hover align-middle text-center shadow-sm rounded">
      <thead >
        <tr>
          <th scope="col">OrderId</th>
          <th scope="col">StatusId</th>
          <th scope="col">Área o Línea</th>
          <th scope="col">Fecha</th>
          <th scope="col">Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <th scope="row">1</th>
          <td>Mark</td>
          <td>Otto</td>
          <td>@mdo</td>
          <td>
            <div class="d-flex justify-content-center gap-2">
              <asp:Button ID="btnViewDetails" runat="server" Text="Ver Detalles" CssClass="btn btn-info btn-sm text-white" />
              <asp:Button ID="btnDeleteOrder" runat="server" Text="Eliminar" CssClass="btn btn-danger btn-sm" />
            </div>
          </td>
        </tr>
        <tr>
          <th scope="row">2</th>
          <td>Jacob</td>
          <td>Thornton</td>
          <td>@fat</td>
          <td>
            <div class="d-flex justify-content-center gap-2">
              <asp:Button ID="Button1" runat="server" Text="Ver Detalle" CssClass="btn btn-info btn-sm text-white" />
              <asp:Button ID="Button2" runat="server" Text="Eliminar" CssClass="btn btn-danger btn-sm" />
            </div>
          </td>
        </tr>
        <tr>
          <th scope="row">3</th>
          <td>John</td>
          <td>Doe</td>
          <td>@social</td>
          <td>
            <div class="d-flex justify-content-center gap-2">
              <asp:Button ID="Button3" runat="server" Text="Ver Detalles" CssClass="btn btn-info btn-sm text-white" />
              <asp:Button ID="Button4" runat="server" Text="Eliminar" CssClass="btn btn-danger btn-sm" />
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</div>
     </asp:View>
        <asp:View ID="vmCreateOrder" runat="server">

            <div class="row mt-3">
                <div class="col-md-3">
                    <asp:Label ID="lblOrderId" runat="server" Text="Numero de Parte" CssClass="form-label" />
                    <asp:TextBox ID="txtNumberPart" runat="server" cssClass="form-control" Placeholder="Ingresa un  numero de parte" />
                </div>

                <div class="col-md-3">
                    <asp:Label ID="lblNp" Text="Ingresa  Cantidad" runat="server" CssClass="form-label" />
                    <asp:TextBox ID="txtQnty" runat="server" CssClass="form-control" placeholder="Ingresa la cantidad a solicitar" />
                </div>
                        <div class="col-md-3">
                    <label>Seleccione un Area</label>
                    <asp:DropDownList ID="ddlAreaS" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlDroArea_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </div>

            <div class="col-md-3" ID="lineaS" runat="server" visible="false" >
                <label>Seleccione una linea</label>
                <asp:DropDownList ID="ddlLineaS" runat="server" CssClass="form-select">
                </asp:DropDownList>
                </div>

            </div>

            <div class="row mt-3">

              
                <div class="col-auto">
                <asp:Button ID="btnCatalogo" runat="server" Text="Catálogo" CssClass="btn btn-outline-info" OnClick="OpenModal_Click" />
                </div>

                <div class="col-auto">
                <asp:Button ID="btnAgregarIndirecto" runat="server" Text="Agregar Indirecto" CssClass="btn btn-outline-secondary" OnClick="btnAgregarIndirecto_Click" />
                </div>
                


                <div class="col-auto">
                <asp:Button ID="btnCrearOrden" runat="server" Text="Crear Orden" CssClass="btn btn-outline-primary" OnClick="btnCrearOrden_Click" Visible="false" />
                </div>

                <div class="col-auto">
                <asp:Button ID="btnCancelarOrden" runat="server" Text="Cancelar Orden" CssClass="btn btn-outline-danger" OnClick="btnCancelarOrden_Click" Visible="false" />
                </div>
                
                
            </div>

                        <!-- Modal del catálogo de indirectos -->
            <div class="modal fade" id="catalogModal" tabindex="-1" aria-labelledby="catalogModalLabel" aria-hidden="true">
              <div class="modal-dialog modal-fullscreen">
                <div class="modal-content">
                  <div class="modal-header">
                    <h5 class="modal-title" id="catalogModalLabel">Catálogo de Productos</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                  </div>
                  <div class="modal-body">
                    
                      <div class="row">
                          <div class="col-4">
                              <asp:Label runat="server" Text="Seleccione una categoría:" CssClass="form-label" />
                              <asp:DropDownList ID="ddlCategorias" runat="server" CssClass="form-select">
                              </asp:DropDownList>
                          </div>

                          <div class="col-4">
                              <asp:Label runat="server" Text="Busque un producto por descripcion o numero de parte:" CssClass="form-label" />
                              <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Buscar producto..."/>
                          </div>
                          <div class="col-4">
                              <asp:Button ID="btnBuscarProducto" runat="server" Text="Buscar" CssClass="btn btn-primary mt-4" />
                      </div>
                       </div>

                     
                          <!--cards del modal aqui-->
   <div class="row mt-4">
   
       <asp:Repeater ID="rptCatalogoProductos" runat="server" OnItemDataBound="rptCatalogoProductos_ItemDataBound">
    <ItemTemplate>
        <div class="col-md-4 mb-4">
            <asp:Panel ID="pnlCard" runat="server" CssClass="card h-100">
                <asp:Image ID="imgProducto" runat="server" CssClass="card-img-top" />
                <div class="card-body d-flex flex-column justify-content-between">
                    <div>
                        <h5 class="card-title text-primary fw-bold"><%# Eval("PartNumb") %></h5>
                        <p class="card-text mb-1"><%# Eval("ProductDescription") %></p>
                        <p class="card-text mb-1"><strong>Inventario:</strong> <%# Eval("Inventory") %> <%# Eval("UM") %></p>
                        <p class="card-text mb-1"><strong>Categoría:</strong> <%# Eval("CategoryName") %></p>
                        <p class="card-text mb-1"><strong>Stock Level:</strong> <%# Eval("Stock") %></p>
                    </div>
                    <asp:Button 
                    ID="btnSeleccionar" 
                    runat="server" 
                    Text="Agregar a Orden" 
                    CssClass="btn btn-outline-primary mt-3" 
                    CommandName="AgregarProducto" 
                    CommandArgument='<%# Eval("PartNumb") %>' 
                    OnCommand="btnAgregarBoton_Command" />

                </div>
            </asp:Panel>
        </div>
    </ItemTemplate>
</asp:Repeater>


</div>

                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                  </div>
                </div>
              </div>
            </div>

            <!-- Fin Modal del catálogo de indirectos -->


             <!-- tabla dinamica de pedidos -->

         <div class="bd-example m-0 border-0 mt-4">
          <div class="table-responsive">
            
        <asp:GridView ID="GridViewIndirectos" runat="server" AutoGenerateColumns="False"
            CssClass="table table-hover table-bordered text-center mt-4"
            OnRowCommand="GridViewIndirectos_RowCommand"
            EmptyDataText="No hay Indirectos agregados."
            HeaderStyle-CssClass="table-dark text-center"
            RowStyle-CssClass="align-middle text-center">

            <Columns>
                <asp:BoundField DataField="PartNumber" HeaderText="Número de Parte" />
                <asp:BoundField DataField="ProductDescription" HeaderText="Producto" />
                <asp:TemplateField HeaderText="Cantidad">
                    <ItemTemplate>
                        <asp:TextBox ID="txtCantidad" runat="server"
                            Text='<%# Eval("OrderQnty") %>'
                            AutoPostBack="true"
                            OnTextChanged="txtCantidad_TextChanged"
                            CssClass="form-control form-control-sm text-center" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <div class="d-flex justify-content-center">
                    <!-- Aquí puedes poner botones de acción -->
                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-sm btn-danger " CommandName="Eliminar" CommandArgument='<%# Eval("PartNumber") %>' />
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            </Columns>
        </asp:GridView>

          </div>
</div>


            <!-- fin de tabla dinamica de pedidos -->


        </asp:View>

</asp:MultiView>
             </ContentTemplate>
</asp:UpdatePanel>





</asp:Content>
