<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionarModulos.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Administracion.GestionarModulos" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Gestión de Módulos</h2>
    <p>En esta sección podrá gestionar los módulos de la aplicación.</p>
    <hr style="background-color:black" />

    <!-- Botón para mostrar/ocultar el formulario -->
    <button class="btn btn-success" type="button" data-bs-toggle="collapse" data-bs-target="#collapseExample" aria-expanded="false" aria-controls="collapseExample">
        Agregar Nuevo Módulo
    </button>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <!-- Contenedor colapsable -->
    <div class="collapse mt-3" id="collapseExample">
        <div class="card card-body">
            
                    <div class="row align-items-end">
                        <div class="col-md-5">
                            <label for="txtNombreModulo" class="form-label">Nombre del Módulo:</label>
                            <asp:TextBox ID="txtNombreModulo" runat="server" CssClass="form-control" placeholder="Nombre del Módulo"></asp:TextBox>
                        </div>
                        <div class="col-md-5">
                            <label for="txtCodigoModulo" class="form-label">Código del Módulo:</label>
                            <asp:TextBox ID="txtCodigoModulo" runat="server" CssClass="form-control" placeholder="Código del Módulo"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="form-label invisible">.</label>
                            <asp:Button ID="btnAgregarModulo" runat="server" Text="Agregar Módulo" CssClass="btn btn-primary w-100" OnClick="btn_AddModuloClick" />
                        </div>
                    </div>
             
        </div>
    </div>

    <asp:GridView ID="tblModulo" runat="server" OnRowCommand="tblModulo_RowCommand"  CssClass="table table-sm table-bordered mt-5"
    AutoGenerateColumns="False" GridLines="None" EmptyDataText="No hay módulos registrados.">
    <Columns>
        <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
        <asp:BoundField DataField="Nombre" HeaderText="Nombre Módulo" SortExpression="Nombre"/>
        <asp:BoundField DataField="Codigo" HeaderText="Código" SortExpression="Codigo" />
        <asp:TemplateField HeaderText="Acciones">
            <ItemTemplate>
                <div class="d-flex justify-content-center">
                <!-- Aquí puedes poner botones de acción -->
                <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btn btn-sm btn-warning me-3" CommandName="Editar" CommandArgument='<%# Eval("Id") %>' />
                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-sm btn-danger " CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>' />
                </div>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>


        <!-- Modal de Edición -->
<div class="modal fade" id="modalEditar" tabindex="-1" aria-labelledby="modalEditarLabel" aria-hidden="true">
  <div class="modal-dialog modal-dialog-centered">
    <div class="modal-content">
      <div class="modal-header bg-primary text-white">
        <h5 class="modal-title" id="modalEditarLabel">Editar Módulo</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
      </div>
      <div class="modal-body">
        <asp:HiddenField ID="hfIdModulo" runat="server" />
        <div class="mb-3">
          <label for="txtEditarNombre" class="form-label">Nombre del Módulo</label>
          <asp:TextBox ID="txtEditarNombre" runat="server" CssClass="form-control" />
        </div>
        <div class="mb-3">
          <label for="txtEditarCodigo" class="form-label">Código del Módulo</label>
          <asp:TextBox ID="txtEditarCodigo" runat="server" CssClass="form-control" />
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
          <asp:HiddenField ID="hfIdModuloE" runat="server" />
        <h5 class="mb-0">¿Estás seguro de eliminar <asp:Literal ID="litNombreModuloEliminar" runat="server" />?</h5>
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
