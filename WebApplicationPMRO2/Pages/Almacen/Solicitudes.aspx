<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Solicitudes.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Almacen.Solicitudes" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    

       <h3>Solicitudes</h3>
        
 
    <hr />

           <asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
      <asp:MultiView ID="mvMain" runat="server" ActiveViewIndex="0">


          <asp:View ID="vwListado" runat="server">

    <div class="row mt-3">

      <div class="col-md-5">
  <asp:Label ID="lblMensaje" runat="server" CssClass="form-label">Buscar Orden</asp:Label>
  
  <div class="d-flex mb-3">
    <asp:TextBox ID="txtBuscarOrden" runat="server" CssClass="form-control me-2" placeholder="Ingresa el OrderId o Nombre del Empleado"></asp:TextBox>
    <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary" Text="Buscar" OnClick="btnBuscar_Click" />
  </div>
</div>

    </div>


    <div class="row mt-3">
        
        <div class="col-md-4">
            <label class="form-label">Seleccione un Estatus</label>
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
        </div>
        <div class="col-md-4">
            <label class="form-label">Seleccione un Area</label>
            <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged"  AutoPostBack="true">
                </asp:DropDownList>
        </div>

        <div class="col-md-4" ID="linea" runat="server" visible="false">
             <label class="form-label">Seleccione una Linea</label>
            <asp:DropDownList ID="ddlLinea" runat="server" CssClass="form-select"  OnSelectedIndexChanged="ddlLinea_SelectedIndexChanged" AutoPostBack="true" >
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

         <asp:BoundField DataField="UpdatedOn" HeaderText="Fecha de Solicitud" SortExpression="UpdatedOn" />

      <asp:BoundField DataField="UpdatedBy" HeaderText="Solicitado Por" SortExpression="StatusId" />

      <asp:TemplateField HeaderText="Acciones">
        <ItemStyle CssClass="cell-actions text-center" />
        <ItemTemplate>
          <div class="d-flex justify-content-center gap-2">
            <asp:Button ID="btnEditar" runat="server" Text="Consultar"
              CssClass="btn btn-outline-info btn-sm"
              CommandName="Consultar" CommandArgument='<%# Eval("IdOrder") %>' />

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
            </asp:View>
    <asp:View ID="vwDetalle" runat="server">

        <!-- Encabezado -->
      <div class="card shadow-sm mb-3 mt-4">
        <div class="card-body d-flex flex-wrap align-items-center justify-content-between gap-3">
          <div>
            <h5 class="mb-1">Orden <asp:Label ID="lblOrderId" runat="server" CssClass="fw-bold"></asp:Label></h5>
            <div class="text-muted">
              <span class="me-3">Área/Líneas: <asp:Label ID="lblAreaLinea" runat="server"></asp:Label></span>
              <span>Solicitado por: <asp:Label ID="lblSolicitadoPor" runat="server"></asp:Label></span>
            </div>
          </div>
          <div class="d-flex gap-2">
            <asp:Button ID="btnListo" runat="server" CssClass="btn btn-success" Text="Listo para recoger" OnClick="btnListo_Click" />
            <asp:Button ID="btnSinInv" runat="server" CssClass="btn btn-warning" Text="Sin inventario" OnClick="btnSinInv_Click" />
            <asp:Button ID="btnRegresar" runat="server" CssClass="btn btn-outline-secondary" Text="Regresar" OnClick="btnRegresar_Click" />
          </div>
        </div>
      </div>

      <!-- Detalle de renglones -->
      <asp:GridView ID="gvDetalle" runat="server"
  CssClass="table table-sm table-striped table-hover align-middle mb-0"
  AutoGenerateColumns="False" GridLines="None" UseAccessibleHeader="true"
  OnPreRender="gvDetalle_PreRender" OnRowDataBound="gvDetalle_RowDataBound"
  DataKeyNames="OrderDetailId"
  EmptyDataText="No hay artículos en esta orden.">
  <Columns>
    <asp:BoundField DataField="PartNumb" HeaderText="PartNumb" />
    <asp:BoundField DataField="PartDescription" HeaderText="Descripción" />
    <asp:BoundField DataField="OrderQnty" HeaderText="Cantidad" />
    <asp:BoundField DataField="Disponible" HeaderText="Disponible" />
    <asp:BoundField DataField="NombreLocation" HeaderText="Locación" />

    <asp:TemplateField HeaderText="Estado">
      <ItemTemplate>
        <span class='<%# Eval("CssEstado") %>'><%# Eval("Estado") %></span>
        <asp:Label ID="lblFaltan" runat="server" Text='<%# Eval("FaltanTexto") %>' CssClass="ms-1 text-muted" />
      </ItemTemplate>
    </asp:TemplateField>


    <asp:TemplateField HeaderText="Entregar">
      <ItemTemplate>
        <asp:CheckBox ID="chkEntregar" runat="server"
          Checked='<%# Convert.ToBoolean(Eval("Marcado")) %>'
          Enabled='<%# Convert.ToBoolean(Eval("Habilitado")) %>' />
      </ItemTemplate>
      <ItemStyle CssClass="text-center" />
      <HeaderStyle CssClass="text-center" />
    </asp:TemplateField>
  </Columns>
  <HeaderStyle CssClass="table-light" />
  <EmptyDataTemplate>
    <div class="text-center p-4 text-muted">No hay artículos en esta orden.</div>
  </EmptyDataTemplate>
</asp:GridView>


<div class="d-flex justify-content-end mt-3">
  <asp:Button ID="btnGuardarMarcados" runat="server"
    CssClass="btn btn-primary" Text="Guardar selección"
    OnClick="btnGuardarMarcados_Click" />
</div>


      <asp:HiddenField ID="hdnOrderId" runat="server" />


    </asp:View>

          </asp:MultiView>

                 </ContentTemplate>

</asp:UpdatePanel>

</asp:Content>
