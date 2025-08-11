<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PermisoMenu.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Administracion.PermisoMenu" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    

       <h2>Permisos Menu</h2>
    <hr style="color:black"/>

      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <asp:MultiView ID="mvwContainer" runat="server" ActiveViewIndex="0">
                <asp:View ID="viewMaintenance" runat="server">
                    <div class="row align-items mb-4">

                    <div class="col-2">
                        <div class="form-group">
                           <asp:Button runat="server" ID="btnAddNew" Text="Agregar Permiso a Rol" CssClass="btn btn-success" OnClick="btnAddNew_Click" />
                    </div>
                   </div>
                    <div class="col-3">
                       <asp:DropDownList ID="ddlModulo" runat="server" CssClass="form-select" OnSelectedIndexChanged ="Modulo_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                    </div>

                        <div class="col-3">
                           <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-select" OnSelectedIndexChanged="Rol_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                        </div>

                        <div class="col-3">
                       <asp:DropDownList ID="ddlMenu" runat="server" CssClass="form-select" OnSelectedIndexChanged="Menu_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>

                    </div>

                    <asp:GridView ID="tblPM" runat="server" OnRowCommand="tblPM_RowCommand"  CssClass="table table-sm table-bordered mt-5"
                    AutoGenerateColumns="False" GridLines="None" EmptyDataText="No hay módulos registrados.">
                    <Columns>
                        <asp:BoundField DataField="NombreRol" HeaderText="Nombre Rol" SortExpression="NombreRol"/>
                        <asp:BoundField DataField="NombreMenu" HeaderText="Nombre Menu" SortExpression="NombreMenu" />
                        <asp:BoundField DataField="PuedeVer" HeaderText="Puede Ver" SortExpression="PuedeVer" />
                        <asp:BoundField DataField="PuedeCrear" HeaderText="Puede Crear" SortExpression="PuedeCrear" />
                        <asp:BoundField DataField="PuedeEditar" HeaderText="Puede Editar" SortExpression="PuedeEditar" />
                        <asp:BoundField DataField="PuedeEliminar" HeaderText="Puede Eliminar" SortExpression="PuedeEliminar" />
                        <asp:BoundField DataField="MenuId" HeaderText="MenuId" Visible="false" />
                        <asp:BoundField DataField="RolId" HeaderText="RolId" Visible="false" />
                        <asp:BoundField DataField="ModuloId" HeaderText="ModuloId" Visible="false" />
                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <div class="d-flex justify-content-center">
                                <!-- Aquí puedes poner botones de acción -->
                                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-sm btn-danger " CommandName="Eliminar" CommandArgument='<%# Eval("ID") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                    <!--Modal de eliminacion-->

                       <div class="modal fade" id="modalEliminar" tabindex="-1" aria-labelledby="modalEliminarLabel" aria-hidden="true">
                      <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content rounded-3 shadow">
                          <div class="modal-body p-4 text-center">
                              <asp:HiddenField ID="hfIdModuloE" runat="server" />
                            <h5 class="mb-0">¿Estás seguro de eliminar la relacion entre <asp:Literal ID="litNombreModuloEliminar" runat="server" />?</h5>
       
                          </div>
                          <div class="modal-footer flex-nowrap p-0">
                            <asp:Button ID="confirmDelete" runat="server" Text="Si, Eliminar" class="btn btn-lg btn-link fs-6 text-decoration-none col-6 py-3 m-0 rounded-0 border-end" OnClick="btnEliminarModal_Click"/>
                            <button type="button" class="btn btn-lg btn-link fs-6 text-decoration-none col-6 py-3 m-0 rounded-0" data-bs-dismiss="modal">
                              No, gracias
                            </button>
                          </div>
                        </div>
                      </div>
                    </div>

                    <!--Fin del Modal de eliminacion-->


                 </asp:View>

                <asp:View runat="server" ID="viewRecord">

                          <div class="row align-items mb-4">

                      <div class="col-2">
                          <div class="form-group">
                             <asp:Button runat="server" ID="btnCancel" Text="Salir de Agregar Permiso" CssClass="btn btn-secondary" OnClick="btnCancel_Click" />
                      </div>
                     </div>
                      <div class="col-3">
                         <asp:DropDownList ID="DroModulo" runat="server" CssClass="form-select" OnSelectedIndexChanged="DroModulo_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                      </div>
                        <div class="col-3">
                       <asp:DropDownList ID="DropRol" runat="server" CssClass="form-select" OnSelectedIndexChanged="DropRol_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>
                       <div class="col-3">
                       <asp:DropDownList ID="DropMenu" runat="server" CssClass="form-select" OnSelectedIndexChanged="DropMenu_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>
                      </div>

                    <h2 class="mt-5 mb-5">Permisos del Rol <asp:Literal ID="Literal1" runat="server"></asp:Literal></h2>

                    <div class="mb-3 form-check">
                    <input type="checkbox" class="form-check-input" runat="server" ID="chkVer">
                    <label class="form-check-label" for="exampleCheck1">Puede Ver</label>
                  </div>

                   
                    <div class="mb-3 form-check">
                    <input type="checkbox" class="form-check-input" runat="server" ID="chkCrear">
                    <label class="form-check-label" for="exampleCheck1">Puede Crear</label>
                  </div>

                <div class="mb-3 form-check">
                  <input type="checkbox" class="form-check-input" runat="server" ID="chkEditar">
                  <label class="form-check-label" for="exampleCheck1">Puede Editar</label>
                </div>

                  <div class="mb-3 form-check">
                  <input type="checkbox" class="form-check-input" runat="server" ID="chkEliminar">
                  <label class="form-check-label" for="exampleCheck1">Puede Eliminar</label>
                </div>

                <p><asp:Literal ID="Literal2" runat="server"></asp:Literal></p>

                <asp:Button Text="Guardar" ID="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" />



                </asp:View>

                
           
            </asp:MultiView>

        </ContentTemplate>
      </asp:UpdatePanel>
</asp:Content>
