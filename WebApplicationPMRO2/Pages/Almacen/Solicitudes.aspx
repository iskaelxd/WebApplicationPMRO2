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
              <asp:Button ID="btnEntregar" runat="server" CssClass="btn btn-success" Text="Entregado" visible="false" OnClick="btnEntregar_Click" />
             <asp:Button ID="btnEliminar" runat="server" CssClass="btn btn-danger" Text="Eliminar" Visible="false" OnClick="btnEliminar_Click" />
            <asp:Button ID="btnRegresar" runat="server" CssClass="btn btn-outline-secondary" Text="Regresar"  OnClick="btnRegresar_Click" />
            
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



         <div class="modal fade" id="modalEliminar" tabindex="-1" aria-labelledby="modalEliminarLabel" aria-hidden="true">
  <div class="modal-dialog modal-dialog-centered">
    <div class="modal-content rounded-3 shadow">
      <div class="modal-body p-4 text-center">
        <h5 class="mb-0">¿Estás seguro de eliminar la <asp:Literal ID="litNombreRolEliminar" runat="server" />?</h5>
        <p class="mb-0">Una vez eliminado ya no lo podrás recuperar</p>
      </div>
      <div class="modal-footer flex-nowrap p-0">
        <asp:Button ID="confirmDelete" runat="server" Text="Si, Eliminar" class="btn btn-lg btn-link fs-6 text-decoration-none col-6 py-3 m-0 rounded-0 border-end" OnClick="btnSiEliminar_Click"/>
        <button type="button" class="btn btn-lg btn-link fs-6 text-decoration-none col-6 py-3 m-0 rounded-0" data-bs-dismiss="modal">
          No, gracias
        </button>
      </div>
    </div>
  </div>
</div>


      <asp:HiddenField ID="hdnOrderId" runat="server" />


    </asp:View>

          </asp:MultiView>


    <asp:Button ID="btnSignalRListRefresh" runat="server"
    Style="display:none" UseSubmitBehavior="false"
    OnClick="btnSignalRListRefresh_Click" />

<asp:Button ID="btnSignalRDetailRefresh" runat="server"
    Style="display:none" UseSubmitBehavior="false"
    OnClick="btnSignalRDetailRefresh_Click" />


    <script>
        (function () {
            var POLL_MS = 8000; // refresco cada 8s
            var timer = null;
            var pausedReasons = new Set(); // razones activas de pausa (detail, search, postback)

            function byId(id) { return document.getElementById(id); }
            function isVisible(el) { return !!(el && el.offsetParent !== null); }

            // Detectores de estado
            function isListVisible() { return !!byId('<%= tblorder.ClientID %>'); }
    function isDetailVisible() { return !!byId('<%= gvDetalle.ClientID %>'); }
    function hasSearch() {
        var tb = byId('<%= txtBuscarOrden.ClientID %>');
    return tb && tb.value.trim() !== '';
  }
  function modalOpen(){ return !!document.querySelector('.modal.show'); }

  // Solo podemos refrescar cuando estamos en list, sin búsqueda y sin pausas
  function canPoll(){
    return isListVisible() && !isDetailVisible() && !hasSearch() && !modalOpen() && pausedReasons.size === 0;
  }

  function safePostback(btnClientId){
    var el = byId(btnClientId);
    if (!el || !canPoll()) return;
    if (typeof window.__doPostBack === 'function' && el.name) {
      window.__doPostBack(el.name, '');
    } else {
      el.click();
    }
  }

  function tick(){ safePostback('<%= btnSignalRListRefresh.ClientID %>'); }

  function start(){ stop(); timer = setInterval(tick, POLL_MS); }
  function stop(){ if (timer){ clearInterval(timer); timer = null; } }

  function pause(reason){ pausedReasons.add(reason || 'manual'); }
  function resume(reason){
    if (reason) pausedReasons.delete(reason);
    else pausedReasons.clear();
  }

  // PRM: pausar durante postbacks y re-evaluar al terminar
  if (window.Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_beginRequest(function(){ pause('postback'); });
    prm.add_endRequest(function(){
      resume('postback');
      // (Re)aplicar pausas según UI actual
      if (isDetailVisible()) pause('detail'); else resume('detail');
      if (hasSearch())       pause('search'); else resume('search');
      start();
    });
  }

  // Iniciar al cargar
  document.addEventListener('DOMContentLoaded', function(){
    if (isDetailVisible()) pause('detail');
    if (hasSearch())       pause('search');
    start();

    // Si limpian la caja de búsqueda, reanudar automáticamente; si escriben, pausar
    var tb = byId('<%= txtBuscarOrden.ClientID %>');
      if (tb) {
          tb.addEventListener('input', function () {
              if (tb.value.trim() === '') resume('search'); else pause('search');
          });
      }
  });

            // Exporta API para usar desde botones (opcional)
            window.polling = {
                pause: pause,    // polling.pause('motivo')
                resume: resume,  // polling.resume('motivo') o sin args para limpiar todo
                stop: stop,
                start: start
            };
        })();
    </script>



                 </ContentTemplate>

</asp:UpdatePanel>

</asp:Content>