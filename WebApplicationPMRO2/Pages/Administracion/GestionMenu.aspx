<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionMenu.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Administracion.GestionMenu" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    

       <h2>Gestión de Menu</h2>
       <hr style="color:black"/>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

        <asp:MultiView ID="mvwContainer" runat="server" ActiveViewIndex="0">

            <asp:View ID="viewMaintenance" runat="server">
               

                <div class="row align-items mb-4">
                           <div class="col-2">
                               <div class="form-group">
                                   <asp:Button runat="server" ID="btnAddNew" Text="Nueva Opcion Menu" CssClass="btn btn-success" OnClick="btnAddNew_Click" />
                            </div>
                           </div>
                    <div class="col-4">
                       <asp:DropDownList ID="ddlModulo" runat="server" CssClass="form-select" OnSelectedIndexChanged="MenuSelected_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>
                    </div>
                  

        <asp:GridView ID="tblMenu" runat="server" OnRowCommand="tblMenu_RowCommand"  CssClass="table table-sm table-bordered mt-5"
        AutoGenerateColumns="False" GridLines="None" EmptyDataText="No hay módulos registrados.">
        <Columns>
            <asp:BoundField DataField="MenuId" HeaderText="MenuId" SortExpression="MenuId" />
            <asp:BoundField DataField="ModuloNombre" HeaderText="Nombre Modulo" SortExpression="ModuloNombre"/>
            <asp:BoundField DataField="Titulo" HeaderText="Titulo" SortExpression="Titulo" />
            <asp:BoundField DataField="Url" HeaderText="Url" SortExpression="Url" />
            <asp:BoundField DataField="Icono" HeaderText="Icono" SortExpression="Icono" />
            <asp:BoundField DataField="Orden" HeaderText="Orden" SortExpression="Orden" />
            
   
            <asp:BoundField DataField="ModuloId" HeaderText="ModuloId" Visible="false" />

            <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <div class="d-flex justify-content-center">
                    <!-- Aquí puedes poner botones de acción -->
                    <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btn btn-sm btn-warning me-3" CommandName="Editar" CommandArgument='<%# Eval("MenuId") %>' />
                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-sm btn-danger " CommandName="Eliminar" CommandArgument='<%# Eval("MenuId") %>' />
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

                        <!--Modal de Confirmación de Eliminación-->

         <div class="modal fade" id="modalEliminar" tabindex="-1" aria-labelledby="modalEliminarLabel" aria-hidden="true">
          <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content rounded-3 shadow">
              <div class="modal-body p-4 text-center">
                  <asp:HiddenField ID="hfIdMenuE" runat="server" />
                <h5 class="mb-0">¿Estás seguro de eliminar <asp:Literal ID="litNombreMenuEliminar" runat="server" />?</h5>
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


                
            </asp:View>



            <asp:View runat="server" ID="viewRecord">
                  <div class="row">
                    <div class="col">
                        <div class="row">
                            <div class="col-2">
                                <div class="form-group">
                                    <asp:Button runat="server" ID="btnCancel" Text="Salir de Agregar Menu" CssClass="btn btn-secondary" OnClick="btnCancel_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                    <div class="row mt-4">
        <div class="col">
            <div class="row mb-3">
                <div class="col-4">
                    <asp:Label runat="server" Text="Seleccione Módulo:" CssClass="form-label" />
                    <asp:DropDownList ID="ddlMenu" runat="server" CssClass="form-select" AutoPostBack="true"></asp:DropDownList>
                </div>
                <div class="col-4">
                    <asp:Label runat="server" AssociatedControlID="txtTitulo" Text="Título:" CssClass="form-label" />
                    <asp:TextBox runat="server" ID="txtTitulo" CssClass="form-control" />
                </div>
                <div class="col-2">
                    <asp:TextBox runat="server" ID="txtMenuId" CssClass="form-control" ReadOnly="true" visible="false"/>
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-4">
                    <asp:Label runat="server"  Text="URL:" CssClass="form-label" />
                    <asp:TextBox runat="server" ID="txtUrl" CssClass="form-control" placeholder="/Pages/NombreCarpeta/NombreVista.aspx" />
                </div>
                <div class="col-4">
                    <asp:Label runat="server"  Text="Ícono:" CssClass="form-label" />
                    <asp:TextBox runat="server" ID="txtIcono" CssClass="form-control" />
                </div>
                <div class="col-2">
                    <asp:Label runat="server"  Text="Orden:" CssClass="form-label" />
                    <asp:TextBox runat="server" ID="txtOrden" CssClass="form-control" />
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-2 mt-2">
                    <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="btn btn-primary" Onclick="btnGuardar_Click" />
                </div>
            </div>
        </div>
    </div>


                
            </asp:View>

        </asp:MultiView>
       
  


    
    </ContentTemplate>
    </asp:UpdatePanel>


    

</asp:Content>
