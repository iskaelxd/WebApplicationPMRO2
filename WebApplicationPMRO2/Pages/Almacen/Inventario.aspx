<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Inventario.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Almacen.Inventario" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

       <h3>Inventario</h3>
        
 
    <hr />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
  <ContentTemplate>

    <asp:MultiView ID="mvInventario" runat="server" ActiveViewIndex="0">

        <asp:View ID="vwListado" runat="server">
  



        <asp:HiddenField ID="hdnImagenUrl" runat="server" />


    <div class="row">
        <div class="col-4">
            <label for="txtBuscar" class="form-label">Buscar por Np o Descripcion</label>
            <asp:TextBox  for="txtBuscar" ID="txtBuscar"  runat="server"  CssClass="form-control" placeholder="Ingrese su NP o descripcion" />
        </div>

        <div class="col-4 d-flex align-items-end">
            <asp:Button ID="btnBuscar" Text="Buscar" runat="server" CssClass="btn btn-primary" OnClick ="btnSearch_Click"/>
            
        </div>
    </div>

    <div class="row mt-4">

        <div class="col-3">
            <asp:DropDownList ID="ddlStock" runat="server" CssClass="form-select" OnSelectedIndexChanged="StockSelected_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </div>
           
                <div class="col-4">
    <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select" OnSelectedIndexChanged="CategorySelected_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        </div>

 <div class="col-4">
    <asp:DropDownList ID="ddlLocacion" runat="server" CssClass="form-select" OnSelectedIndexChanged="LocationSelected_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    </div>
         <div class="col-1">
    <asp:DropDownList ID="ddlPagination" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPagination_SelectedIndexChanged" CssClass="form-select">
        <asp:ListItem Text="25" Value="25" />
        <asp:ListItem Text="50" Value="50" />
        <asp:ListItem Text="100" Value="100" />
    </asp:DropDownList>

    </div>

    </div>

        <asp:GridView ID="tblInventory" runat="server" OnRowCommand="tblInventory_RowCommand"  CssClass="table table-sm table-bordered mt-4"
    AutoGenerateColumns="False" GridLines="None" EmptyDataText="No hay módulos registrados."  OnRowDataBound="tblInventory_RowDataBound">
    <Columns>
        <asp:TemplateField HeaderText="Imagen">
    <ItemTemplate>
        <asp:LinkButton ID="lnkImagen" runat="server"
    OnClientClick='<%# "mostrarModalImagen(\"" + Eval("Imagen") + "\"); return false;" %>'>
    <asp:Image ID="imgProducto" runat="server" ImageUrl='<%# Eval("Imagen") %>' Width="60px" Height="60px" />
</asp:LinkButton>


    </ItemTemplate>
</asp:TemplateField>


        <asp:BoundField DataField="Np" HeaderText="Np" SortExpression="Np"/>
        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion" />
        <asp:BoundField DataField="Categoria" HeaderText="Categoria" SortExpression="Categoria" />
        <asp:BoundField DataField="Comprador" HeaderText="Comprador" SortExpression="Comprador" />
        <asp:BoundField DataField="Inventario" HeaderText="Inventario" SortExpression="Inventario" />
        <asp:BoundField DataField="UM" HeaderText="UM" SortExpression="UM" />
        <asp:BoundField DataField="Locacion" HeaderText="Locacion" SortExpression="Locacion" />
        <asp:BoundField DataField="Min" HeaderText="Min" SortExpression="Min" />
        <asp:BoundField DataField="Max" HeaderText="Max" SortExpression="Max" />
        <asp:BoundField DataField="Stock" HeaderText="Stock" SortExpression="Stock" />

       <asp:BoundField DataField="ProductId" HeaderText="ProductId" Visible="false" />

        <asp:TemplateField HeaderText="Acciones">
            <ItemTemplate>
                <div class="d-flex justify-content-center">
                <!-- Aquí puedes poner botones de acción -->
                <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btn btn-sm btn-warning me-3" CommandName="Editar" CommandArgument='<%# Eval("ProductId") %>' />
                
                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar"
    CssClass="btn btn-sm btn-danger"
    OnClientClick='<%# "return openDeleteFromRow(this, " + Eval("ProductId") + ");" %>' />


                </div>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>




       <div class="bd-example m-0 border-0 mt-2">
  <div class="d-flex justify-content-between align-items-center flex-wrap gap-2">
    <!-- Info -->
    <asp:Label ID="lblPagerInfo" runat="server" CssClass="small text-muted" />

    <!-- Controles -->
    <div class="d-flex align-items-center gap-2">
      <span class="small">Por página</span>

      <asp:LinkButton ID="lnkFirst" runat="server"
          CssClass="btn btn-outline-secondary btn-sm"
          CommandName="Page" CommandArgument="First"
          OnCommand="Pager_Command">&laquo;</asp:LinkButton>

      <asp:LinkButton ID="lnkPrev" runat="server"
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

      <asp:LinkButton ID="lnkNext" runat="server"
          CssClass="btn btn-outline-secondary btn-sm"
          CommandName="Page" CommandArgument="Next"
          OnCommand="Pager_Command">&rsaquo;</asp:LinkButton>

      <asp:LinkButton ID="lnkLast" runat="server"
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


        <!-- Modal -->
<div class="modal fade" id="imagenModal" tabindex="-1" aria-labelledby="imagenModalLabel" aria-hidden="true">
  <div class="modal-dialog modal-dialog-centered">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="imagenModalLabel">Vista de Imagen</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
      </div>
      <div class="modal-body text-center">
        <img id="imgVista" src="" alt="Vista previa" class="img-fluid" />

      </div>
    </div>
  </div>
</div>

            <asp:HiddenField ID="hfDeleteId" runat="server" />
<asp:HiddenField ID="hfDeleteNp" runat="server" />

<div class="modal fade" id="confirmDeleteModal" tabindex="-1" aria-labelledby="confirmDeleteLabel" aria-hidden="true">
  <div class="modal-dialog modal-dialog-centered">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="confirmDeleteLabel">Confirmar eliminación</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
      </div>
      <div class="modal-body">
        ¿Estás seguro de eliminar el número de parte <strong id="lblNpDelete"></strong>?
        <div class="text-muted small mt-2">Esta acción marcará el artículo como eliminado.</div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
        <asp:Button ID="btnConfirmDelete" runat="server" Text="Sí, eliminar" CssClass="btn btn-danger"
            OnClick="btnConfirmDelete_Click" />
      </div>
    </div>
  </div>
</div>



    <script type="text/javascript">

        // Abre el modal leyendo el NP desde el span oculto de la fila
        function openDeleteFromRow(btn, id) {
            const row = btn.closest('tr');
            const npTag = row ? row.querySelector('[data-np]') : null;
            const np = npTag ? npTag.getAttribute('data-np') : '';
            return openDelete(id, np);
        }

        // Rellena campos y muestra el modal
        function openDelete(id, np) {
            document.getElementById('<%= hfDeleteId.ClientID %>').value = id;
      document.getElementById('<%= hfDeleteNp.ClientID %>').value = np;
            document.getElementById('lblNpDelete').textContent = np || '';
            const modal = new bootstrap.Modal(document.getElementById('confirmDeleteModal'));
            modal.show();
            return false; // evita postback del botón original
        }


        function mostrarModalImagen(url) {
            const img = document.querySelector('#imagenModal img');
            img.src = url;
            const modal = new bootstrap.Modal(document.getElementById('imagenModal'));
            modal.show();
        }
    </script>

 
             </asp:View>

        <asp:View ID="vwEditar" runat="server">
                <div class="card mt-3">
      <div class="card-header">Editar artículo</div>
      <div class="card-body">
        <asp:HiddenField ID="hfProductId" runat="server" />
        <asp:HiddenField ID="hfPartNumb" runat="server" />

        <div class="row g-3">
          <div class="col-md-4 text-center">
            <asp:Image ID="imgPreview" runat="server" Width="220" Height="220" CssClass="img-thumbnail mb-2" />
            <div class="form-text">Vista previa</div>
            <asp:FileUpload ID="fuImagen" runat="server" CssClass="form-control mt-2" />
            <div class="form-text">Formatos: .jpg, .png, .jpeg (máx. 4 MB)</div>
          </div>

          <div class="col-md-8">
            <div class="mb-3">
              <label class="form-label">NP (solo lectura)</label>
              <asp:TextBox ID="txtNpEdit" runat="server" CssClass="form-control" ReadOnly="true" />
            </div>

            <div class="mb-3">
              <label class="form-label">Descripción</label>
              <asp:TextBox ID="txtDescripcionEdit" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
            </div>

            <div class="row">
              <div class="col-md-6 mb-3">
                <label class="form-label">Categoría</label>
                <asp:DropDownList ID="ddlCategoriaEdit" runat="server" CssClass="form-select" />
              </div>
              <div class="col-md-6 mb-3">
                <label class="form-label">Locación</label>
                <asp:DropDownList ID="ddlLocacionEdit" runat="server" CssClass="form-select" />
              </div>
            </div>

            <div class="d-flex gap-2">
              <asp:Button ID="btnUpdate" runat="server" Text="Actualizar" CssClass="btn btn-success" OnClick="btnUpdate_Click" />
              <asp:Button ID="btnCancelEdit" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelEdit_Click" CausesValidation="false" />
            </div>
          </div>
        </div>

      </div>
    </div>
  </asp:View>

    </asp:MultiView>
         </ContentTemplate>
           <Triggers>
          <asp:PostBackTrigger ControlID="btnUpdate" />
        </Triggers>
 </asp:UpdatePanel>
     

</asp:Content>