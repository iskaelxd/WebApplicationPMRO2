<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionUsuarios.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Administracion.GestionUsuarios" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    

       <h2>Gestión de Usuarios</h2>
       <hr style="color:black"/>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

 <asp:MultiView ID="mvwContainer" runat="server" ActiveViewIndex="0">

    <asp:View ID="viewMaintenance" runat="server">


    <div class="col-2 mb-4">
    <div class="form-group">
        <asp:Button runat="server" ID="btnAddNew" Text="Ingresar Nuevo Usuario" CssClass="btn btn-success" OnClick="btnAdd_Click" />
     </div>
    </div>

    <asp:GridView ID="tblUser" runat="server" OnRowCommand="tblUser_RowCommand"  CssClass="table table-sm table-bordered mt-5"
    AutoGenerateColumns="False" GridLines="None" EmptyDataText="No hay módulos registrados.">
    <Columns>
        <asp:BoundField DataField="nombreEmpleado" HeaderText="Nombre Empleado" SortExpression="nombreEmpleado"/>
        <asp:BoundField DataField="puesto" HeaderText="Puesto" SortExpression="puesto" />
        <asp:BoundField DataField="globalId" HeaderText="GlobalId" SortExpression="globalId" />
        <asp:BoundField DataField="correo" HeaderText="Correo" SortExpression="correo" />
        <asp:BoundField DataField="Id" HeaderText="Id" Visible="false" />
        <asp:TemplateField HeaderText="Acciones">
            <ItemTemplate>
                <div class="d-flex justify-content-center">
                <!-- Aquí puedes poner botones de acción -->
                <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btn btn-sm btn-warning me-3" CommandName="Editar" CommandArgument='<%# Eval("Id") %>'  />
                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-sm btn-danger" CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>' />
                </div>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>


        <!--Eliminar un usuario-->


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
                            <asp:Button runat="server" ID="btnCancel" Text="Salir de Agregar Usuarios" CssClass="btn btn-secondary" OnClick="btnCancel_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
           <div class="col">
               <div class="row mt-4">
                    <div class="col-4">
                     <asp:Label runat="server"  Text="Nombre:" CssClass="form-label" />
                     <asp:TextBox runat="server" ID="txtnombre" CssClass="form-control" />

                       
                 </div>

                      <div class="col-4">
                    <asp:Label runat="server"  Text="GlobalId:" CssClass="form-label" />
                    <asp:TextBox runat="server" ID="txtglobalId" CssClass="form-control" />
                                                       
                </div>

                    <div class="col-4">
                        <asp:Label runat="server"  Text="Correo:" CssClass="form-label" />
                        <asp:TextBox runat="server" ID="txtcorreo"  CssClass="form-control" />

                                                        


                    </div>
                   </div>

                   <div class="row mt-4">

                       <div class ="col-4">
                           <asp:Label runat="server"  Text="Puesto:" CssClass="form-label" />
                            <asp:TextBox runat="server" ID="txtpuesto" CssClass="form-control" />                                 
                       </div>


                       <div class="col-4">  
                           <asp:Button runat="server" ID="btnAddUser" Text="Agregar Usuario" CssClass="btn btn-primary mt-4" Onclick="btnAddUser_Click"/>                            
                       </div>



                   </div>

                <div class="row mt-5">
                    <div class="col-1">
                       <asp:TextBox runat="server" ID="TextId" CssClass="form-control" enabled ="false" Visible="false"/>   
                    </div>
                </div>
           </div>
    </asp:View>
     </asp:MultiView>
     </ContentTemplate>
 </asp:UpdatePanel>

</asp:Content>
