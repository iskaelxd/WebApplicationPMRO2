<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Inventario.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Almacen.Inventario" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

       <h3>Inventario</h3>
        
 
    <hr />
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>


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
    AutoGenerateColumns="False" GridLines="None" EmptyDataText="No hay módulos registrados.">
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
                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-sm btn-danger " CommandName="Eliminar" CommandArgument='<%# Eval("ProductId") %>' />
                </div>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>




        <div class="bd-example m-0 border-0"> 
        <nav aria-label="Standard pagination example">
          <ul class="pagination">
            <asp:LinkButton ID="btnPrev" runat="server" CssClass="page-link" OnClick="btnPrev_Click">«</asp:LinkButton>
            <asp:LinkButton ID="btnPage1" runat="server" CssClass="page-link" OnClick="btnPage_Click" CommandArgument="1">1</asp:LinkButton>
            <asp:LinkButton ID="btnPage2" runat="server" CssClass="page-link" OnClick="btnPage_Click" CommandArgument="2">2</asp:LinkButton>
            <asp:LinkButton ID="btnPage3" runat="server" CssClass="page-link" OnClick="btnPage_Click" CommandArgument="3">3</asp:LinkButton>
            <asp:LinkButton ID="btnPage4" runat="server" CssClass="page-link" OnClick="btnPage_Click" CommandArgument="4">4</asp:LinkButton>
            <asp:LinkButton ID="btnPage5" runat="server" CssClass="page-link" OnClick="btnPage_Click" CommandArgument="5">5</asp:LinkButton>
            <asp:LinkButton ID="btnNext" runat="server" CssClass="page-link" OnClick="btnNext_Click">»</asp:LinkButton>

          </ul>
        </nav>
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
        <img id="imgVista" src="hdnImagenUrl" alt="Vista previa" class="img-fluid" />

      </div>
    </div>
  </div>
</div>


    <script type="text/javascript">
    function mostrarModalImagen(url) {
        const img = document.querySelector('#imagenModal img');
        img.src = url;
        const modal = new bootstrap.Modal(document.getElementById('imagenModal'));
        modal.show();
    }
    </script>

      </ContentTemplate>
  </asp:UpdatePanel>

</asp:Content>
