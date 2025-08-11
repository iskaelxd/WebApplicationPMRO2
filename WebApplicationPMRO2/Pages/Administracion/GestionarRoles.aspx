<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionarRoles.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Administracion.GestionarRoles" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    

   <h2>Gestión de Roles</h2>
 <p>En esta sección podrá gestionar los Roles de la aplicacion.</p>
 <hr style="background-color:black" />


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

        <!--agregar rol y filtrar roles por modulos-->
   <div class="row align-items mb-4">
    <div class="col-2">
    <button class="btn btn-success" type="button" data-bs-toggle="collapse" data-bs-target="#collapseExample" aria-expanded="false" aria-controls="collapseExample">
        Agregar Nuevo Rol
    </button>
    </div>
    <div class="col-4">
    <asp:DropDownList ID="ddlModulo" runat="server" CssClass="form-select" OnSelectedIndexChanged="ModuloSeletect_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        </div>
    </div>

    <!-- Contenedor colapsable -->
        <div class="collapse mt-3" id="collapseExample">
        <div class="card card-body">

        <div class="row align-items-end">
    <div class="col-md-5">
        <label for="txtNombreModulo" class="form-label">Selecciona el Módulo:</label>
        <asp:DropDownList ID="ddlModuloForm" runat="server" CssClass="form-select"></asp:DropDownList>
    </div>
    <div class="col-md-5">
        <label for="txtCodigoModulo" class="form-label">Nombre del Rol</label>
        <asp:TextBox ID="txtNombreRol" runat="server" CssClass="form-control" placeholder="Nombre del Rol"></asp:TextBox>
    </div>
    <div class="col-md-2">
        <label class="form-label invisible">.</label>
        <asp:Button ID="btnAgregarModulo" runat="server" Text="Agregar Rol" CssClass="btn btn-primary w-100" OnClick="btnAddRol_Click"/>
    </div>
    </div>

        </div>
        </div>

        <!--Tabla de roles-->


    <asp:GridView ID="tblRol" runat="server" OnRowCommand="tblRol_RowCommand"  CssClass="table table-sm table-bordered mt-5"
    AutoGenerateColumns="False" GridLines="None" EmptyDataText="No hay módulos registrados.">
    <Columns>
        <asp:BoundField DataField="IdRol" HeaderText="IdRol" SortExpression="IdRol" />
        <asp:BoundField DataField="NombreRol" HeaderText="Nombre Rol" SortExpression="NombreRol"/>
        <asp:BoundField DataField="NombreModulo" HeaderText="Nombre Módulo" SortExpression="NombreModulo" />
        <asp:TemplateField HeaderText="Acciones">
            <ItemTemplate>
                <div class="d-flex justify-content-center">
                <!-- Aquí puedes poner botones de acción -->
                <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btn btn-sm btn-warning me-3" CommandName="Editar" CommandArgument='<%# Eval("IdRol") %>' />
                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-sm btn-danger " CommandName="Eliminar" CommandArgument='<%# Eval("IdRol") %>' />
                </div>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

        <!--Modal de editar-->

 <div class="modal fade" id="modalEditar" tabindex="-1" aria-labelledby="modalEditarLabel" aria-hidden="true">
  <div class="modal-dialog modal-dialog-centered">
    <div class="modal-content">
      <div class="modal-header bg-primary text-white">
        <h5 class="modal-title" id="modalEditarLabel">Editar Rol</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
      </div>
      <div class="modal-body">
        <asp:HiddenField ID="hfIdRol" runat="server" />
        <div class="mb-3">
          <label for="txtEditarModulo" class="form-label">Selecciona un Modulo</label>
          <asp:DropDownList ID="EditarModulo" runat="server" CssClass="form-select"></asp:DropDownList>
        </div>
        <div class="mb-3">
          <label for="txtEditarCodigo" class="form-label">Nombre del Rol</label>
          <asp:TextBox ID="txtEditarRol" runat="server" CssClass="form-control" />
        </div>
      </div>
      <div class="modal-footer">
        <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" CssClass="btn btn-success" OnClick="btnActualizar_Click" />
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Salir</button>
      </div>
    </div>
  </div>
</div>

        <!--Modal de Confirmación de Eliminación-->

 <div class="modal fade" id="modalEliminar" tabindex="-1" aria-labelledby="modalEliminarLabel" aria-hidden="true">
  <div class="modal-dialog modal-dialog-centered">
    <div class="modal-content rounded-3 shadow">
      <div class="modal-body p-4 text-center">
          <asp:HiddenField ID="hfIdRolE" runat="server" />
        <h5 class="mb-0">¿Estás seguro de eliminar <asp:Literal ID="litNombreRolEliminar" runat="server" />?</h5>
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

    
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
