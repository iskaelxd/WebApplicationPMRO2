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

  .tbl-order td, .tbl-order th { vertical-align: middle; }
  .tbl-order .cell-id { width: 100px; white-space: nowrap; }
  .tbl-order .cell-area { max-width: 280px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
  .tbl-order .cell-actions { width: 220px; }
  .table-responsive thead.sticky th { position: sticky; top: 0; z-index: 1; background: #f8f9fa; }

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
               <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
               </asp:DropDownList>
        </div>

        <div class="col-md-3">
            <label>Seleccione un Area</label>
            <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-select"   OnSelectedIndexChanged="ddlArea_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
        </div>

        <div class="col-md-3"  ID="linea" runat="server" visible="false">
            <label>Seleccione una linea</label>
            <asp:DropDownList ID="ddlLinea" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlLinea_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
            </div>

    </div>
        <!--tabla de Ordenes creadas-->
          <div class="table-responsive shadow-sm rounded mt-5">
  <asp:GridView ID="tblorder" runat="server"
    CssClass="table table-sm table-striped table-hover align-middle mb-0 tbl-order"
    AutoGenerateColumns="False" GridLines="None" UseAccessibleHeader="true"
    OnRowCommand="tblorder_RowCommand" OnPreRender="tblorder_PreRender"
    EmptyDataText="No hay módulos registrados.">

    <Columns>
      <asp:BoundField DataField="IdOrder" HeaderText="OrderId" SortExpression="IdOrder">
        <ItemStyle CssClass="cell-id" />
      </asp:BoundField>

      <asp:BoundField DataField="AreaOrLine" HeaderText="Area o Linea" SortExpression="Area o Linea">
        <ItemStyle CssClass="cell-area" />
      </asp:BoundField>

      <asp:BoundField DataField="StatusId" HeaderText="Estatus" SortExpression="StatusId" />

      <asp:BoundField DataField="UpdatedBy" HeaderText="Solicitado Por" SortExpression="StatusId" />

      <asp:TemplateField HeaderText="Acciones">
        <ItemStyle CssClass="cell-actions text-center" />
        <ItemTemplate>
          <div class="d-flex justify-content-center gap-2">
            <asp:Button ID="btnEditar" runat="server" Text="Consultar"
              CssClass="btn btn-outline-info btn-sm"
              CommandName="Consultar" CommandArgument='<%# Eval("IdOrder") %>' />
            <asp:Button ID="btnEliminar" runat="server" Text="Eliminar"
              CssClass="btn btn-outline-danger btn-sm"
              CommandName="Eliminar" CommandArgument='<%# Eval("IdOrder") %>' />
          </div>
        </ItemTemplate>
      </asp:TemplateField>
    </Columns>

    <HeaderStyle CssClass="table-light" />
    <EmptyDataTemplate>
      <div class="text-center p-4 text-muted">No hay módulos registrados.</div>
    </EmptyDataTemplate>
  </asp:GridView>
</div>
            <!--fin de Ordenes creadas-->

            <!--Modal Eliminacion-->

 <div class="modal fade" id="modalEliminar" tabindex="-1" aria-labelledby="modalEliminarLabel" aria-hidden="true">
  <div class="modal-dialog modal-dialog-centered">
    <div class="modal-content rounded-3 shadow">
      <div class="modal-body p-4 text-center">
          <asp:HiddenField ID="hfIdorder" runat="server" />
        <h5 class="mb-0">¿Estás seguro de eliminar la <asp:Literal ID="litNombreRolEliminar" runat="server" />?</h5>
        <p class="mb-0">Una vez eliminado ya no lo podrás recuperar</p>
      </div>
      <div class="modal-footer flex-nowrap p-0">
        <asp:Button ID="confirmDelete" runat="server" Text="Si, Eliminar" class="btn btn-lg btn-link fs-6 text-decoration-none col-6 py-3 m-0 rounded-0 border-end" OnClick="btnEliminar_Click"/>
        <button type="button" class="btn btn-lg btn-link fs-6 text-decoration-none col-6 py-3 m-0 rounded-0" data-bs-dismiss="modal">
          No, gracias
        </button>
      </div>
    </div>
  </div>
</div>

            <!-- Fin Modal Eliminacion-->





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
                          <!-- Cabecera de filtros del modal -->
<asp:Panel runat="server" DefaultButton="btnBuscarProducto"> 
  <div class="row">
    <div class="col-4">
      <asp:Label runat="server" Text="Seleccione una categoría:" CssClass="form-label" />
      <asp:DropDownList ID="ddlCategorias" runat="server" CssClass="form-select"
          AutoPostBack="true"
          OnSelectedIndexChanged="ddlCategorias_SelectedIndexChanged">
      </asp:DropDownList>
    </div>

    <div class="col-4">
      <asp:Label runat="server" Text="Busque un producto por descripcion o numero de parte:" CssClass="form-label" />
      <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Buscar producto..." />
    </div>

    <div class="col-4 d-flex align-items-end gap-2">
      <asp:Button ID="btnBuscarProducto" runat="server" Text="Buscar"
          CssClass="btn btn-primary"
          OnClick="btnBuscarProducto_Click" />

      <!-- Opcional: limpiar filtros -->
      <asp:Button ID="btnLimpiarFiltros" runat="server" Text="Limpiar"
          CssClass="btn btn-outline-secondary"
          OnClick="btnLimpiarFiltros_Click" />
    </div>
  </div>
</asp:Panel>

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
                 <div class="modal-footer justify-content-between">
  <asp:Label ID="lblPagerInfo" runat="server" CssClass="small text-muted" />

  <div class="d-flex align-items-center gap-2">
    <!-- Tamaño de página -->
    <span class="small">Por página</span>
    <asp:DropDownList ID="ddlPageSize" runat="server"
        CssClass="form-select form-select-sm w-auto"
        AutoPostBack="true"
        OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
      <asp:ListItem Text="12" Value="12" />
      <asp:ListItem Text="24" Value="24" />
      <asp:ListItem Text="48" Value="48" />
      <asp:ListItem Text="96" Value="96" />
    </asp:DropDownList>

    <!-- Navegación -->
    <asp:LinkButton ID="btnFirst" runat="server"
        CssClass="btn btn-outline-secondary btn-sm"
        CommandName="Page" CommandArgument="First"
        OnCommand="Pager_Command">&laquo;</asp:LinkButton>

    <asp:LinkButton ID="btnPrev" runat="server"
        CssClass="btn btn-outline-secondary btn-sm"
        CommandName="Page" CommandArgument="Prev"
        OnCommand="Pager_Command">&lsaquo;</asp:LinkButton>

    <!-- Números dinámicos -->
    <asp:Repeater ID="rptPages" runat="server" OnItemCommand="rptPages_ItemCommand">
      <ItemTemplate>
        <asp:LinkButton ID="lnkPage" runat="server"
            CommandName="Page"
            CommandArgument='<%# Eval("Number") %>'
            CssClass='<%# (bool)Eval("Active") ? "btn btn-primary btn-sm" : "btn btn-outline-secondary btn-sm" %>'
            Text='<%# Eval("Number") %>' />
      </ItemTemplate>
    </asp:Repeater>

    <asp:LinkButton ID="btnNext" runat="server"
        CssClass="btn btn-outline-secondary btn-sm"
        CommandName="Page" CommandArgument="Next"
        OnCommand="Pager_Command">&rsaquo;</asp:LinkButton>

    <asp:LinkButton ID="btnLast" runat="server"
        CssClass="btn btn-outline-secondary btn-sm"
        CommandName="Page" CommandArgument="Last"
        OnCommand="Pager_Command">&raquo;</asp:LinkButton>

    <!-- Ir a... -->
    <asp:TextBox ID="txtGo" runat="server"
        CssClass="form-control form-control-sm w-auto" placeholder="Ir a..." />
    <asp:LinkButton ID="btnGo" runat="server"
        CssClass="btn btn-outline-secondary btn-sm"
        OnClick="btnGo_Click">Ir</asp:LinkButton>
  </div>
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

      <asp:View ID="vwViewOrderDetails" runat="server">
    <!-- Encabezado de la orden -->
    <div class="d-flex justify-content-between align-items-center mb-4">
        <h1 class="h3 text-primary fw-bold mb-0">
            Detalle de Orden: <asp:Literal ID="OrdeIdH" runat="server" />
        </h1>
        <asp:Button ID="btnBack" runat="server" Text="← Volver" CssClass="btn btn-outline-secondary" OnClick="btnBack_Click" />
    </div>

    <!-- Tabla de detalles de la orden -->
    <div class="card shadow-sm">
        <div class="card-header bg-light">
            <h5 class="mb-0">Numeros de Parte Solicitados</h5>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="tblorderDetails" runat="server"
                    CssClass="table table-hover table-bordered mb-0"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    EmptyDataText="No hay módulos registrados."
                    OnRowDataBound="tblorderDetails_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="PartNumb" HeaderText="Número de Parte" SortExpression="PartNumb" />
                        <asp:BoundField DataField="PartDescription" HeaderText="Descripción" SortExpression="PartDescription" />
                        <asp:BoundField DataField="OrderQnty" HeaderText="Cantidad" SortExpression="Cantidad" />
                        <asp:BoundField DataField="UM" HeaderText="UM" SortExpression="UM" />
                        <asp:BoundField DataField="Marcado" Visible="false" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:View>



</asp:MultiView>
             </ContentTemplate>

</asp:UpdatePanel>


    <script>
        (function () {
            // Limpia backdrops y estado del body
            function cleanupBackdrops() {
                document.body.classList.remove('modal-open');
                document.body.style.removeProperty('padding-right');
                document.querySelectorAll('.modal-backdrop')
                    .forEach(el => el.parentNode && el.parentNode.removeChild(el));
            }

            // Re-vincula eventos del modal (por si el UpdatePanel re-renderizó)
            function wireModal() {
                var el = document.getElementById('catalogModal');
                if (!el || !window.bootstrap) return;
                el.removeEventListener('hidden.bs.modal', cleanupBackdrops);
                el.addEventListener('hidden.bs.modal', cleanupBackdrops);
            }

            // Exponer helpers para usarlos desde server-side
            window.__catalog_cleanup = cleanupBackdrops;
            window.__catalog_wire = wireModal;

            // Al cargar
            document.addEventListener('DOMContentLoaded', function () {
                wireModal();
            });

            // Después de cada postback parcial de ASP.NET AJAX
            if (window.Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_endRequest(function () {
                    // El contenido dentro de UpdatePanel puede haber cambiado
                    wireModal();
                    // Por si quedó una backdrop previa al re-render
                    cleanupBackdrops();
                });
            }
        })();
    </script>



</asp:Content>