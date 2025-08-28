<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionUsuarios.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Administracion.GestionUsuarios" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Gestión de Usuarios</h2>
    <hr style="color:black" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <!-- Barra de búsqueda y acciones -->
            <div class="row g-2 align-items-end">
                <div class="col-md-4">
                    <label for="<%= txtBuscarNombre.ClientID %>" class="form-label">Buscar por nombre</label>
                    <asp:TextBox ID="txtBuscarNombre" runat="server" CssClass="form-control" MaxLength="200" placeholder="Escribe el nombre a buscar..." />
                </div>
                <div class="col-md-2">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary w-100" OnClick="btnBuscar_Click" />
                </div>
                <div class="col-md-2">
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary w-100" OnClick="btnLimpiar_Click" />
                </div>
                <div class="col-md-4 text-end">
                    <asp:Button runat="server" ID="btnAddNew" Text="Ingresar Nuevo Usuario" CssClass="btn btn-success" OnClick="btnAdd_Click" />
                </div>
            </div>

            <asp:MultiView ID="mvwContainer" runat="server" ActiveViewIndex="0">

                <!-- Listado -->
                <asp:View ID="viewMaintenance" runat="server">

                    <asp:GridView ID="tblUser" runat="server"
                        CssClass="table table-sm table-bordered mt-4"
                        AutoGenerateColumns="False" GridLines="None"
                        DataKeyNames="Id"
                        EmptyDataText="No hay usuarios registrados."
                        OnRowCommand="tblUser_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="nombreEmpleado" HeaderText="Nombre" SortExpression="nombreEmpleado" />
                            <asp:BoundField DataField="puesto" HeaderText="Puesto" SortExpression="puesto" />
                            <asp:BoundField DataField="globalId" HeaderText="GlobalId" SortExpression="globalId" />
                            <asp:BoundField DataField="correo" HeaderText="Correo" SortExpression="correo" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <div class="d-flex justify-content-center">
                                        <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btn btn-sm btn-warning me-2"
                                            CommandName="Editar" CommandArgument='<%# Eval("Id") %>' />
                                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-sm btn-danger"
                                            CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>' />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <!-- Modal eliminar -->
                    <div class="modal fade" id="modalEliminar" tabindex="-1" aria-labelledby="modalEliminarLabel" aria-hidden="true">
                        <div class="modal-dialog modal-dialog-centered">
                            <div class="modal-content rounded-3 shadow">
                                <div class="modal-body p-4 text-center">
                                    <asp:HiddenField ID="hfIdMenuE" runat="server" />
                                    <h5 class="mb-2">¿Estás seguro de eliminar
                                        <asp:Literal ID="litNombreMenuEliminar" runat="server" />?</h5>
                                    <p class="mb-0">Una vez eliminado ya no lo podrás recuperar.</p>
                                </div>
                                <div class="modal-footer flex-nowrap p-0">
                                    <asp:Button ID="confirmDelete" runat="server" Text="Sí, eliminar"
                                        CssClass="btn btn-lg btn-link fs-6 text-decoration-none col-6 py-3 m-0 rounded-0 border-end"
                                        OnClick="btnEliminar_Click" />
                                    <button type="button" class="btn btn-lg btn-link fs-6 text-decoration-none col-6 py-3 m-0 rounded-0" data-bs-dismiss="modal">
                                        No, gracias
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>

                </asp:View>

                <!-- Alta/Edición -->
                <asp:View runat="server" ID="viewRecord">

                    <div class="row">
                        <div class="col">
                            <div class="row">
                                <div class="col-12 col-md-4">
                                    <asp:Button runat="server" ID="btnCancel" Text="Salir" CssClass="btn btn-secondary mt-2" OnClick="btnCancel_Click" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row mt-3 gy-3">
                        <div class="col-md-6">
                            <asp:Label runat="server" AssociatedControlID="txtnombre" Text="Nombre:" CssClass="form-label" />
                            <asp:TextBox runat="server" ID="txtnombre" CssClass="form-control" MaxLength="200" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtnombre" ErrorMessage="El nombre es obligatorio."
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="vgUsuario" />
                        </div>

                        <div class="col-md-6">
                            <asp:Label runat="server" AssociatedControlID="txtglobalId" Text="GlobalId:" CssClass="form-label" />
                            <asp:TextBox runat="server" ID="txtglobalId" CssClass="form-control" MaxLength="100" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtglobalId" ErrorMessage="El GlobalId es obligatorio."
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="vgUsuario" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtglobalId"
                                ValidationExpression="^[A-Za-z0-9._\-]+$"
                                ErrorMessage="GlobalId solo admite letras, números, punto, guion y guion bajo."
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="vgUsuario" />
                        </div>

                        <div class="col-md-6">
                            <asp:Label runat="server" AssociatedControlID="txtcorreo" Text="Correo:" CssClass="form-label" />
                            <asp:TextBox runat="server" ID="txtcorreo" CssClass="form-control" MaxLength="150" TextMode="Email" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtcorreo" ErrorMessage="El correo es obligatorio."
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="vgUsuario" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtcorreo"
                                ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                                ErrorMessage="Formato de correo inválido."
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="vgUsuario" />
                        </div>

                        <div class="col-md-6">
                            <asp:Label runat="server" AssociatedControlID="txtpuesto" Text="Puesto:" CssClass="form-label" />
                            <asp:TextBox runat="server" ID="txtpuesto" CssClass="form-control" MaxLength="200" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtpuesto" ErrorMessage="El puesto es obligatorio."
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="vgUsuario" />
                        </div>

                        <div class="col-12">
                            <asp:Button runat="server" ID="btnAddUser" Text="Agregar Usuario" CssClass="btn btn-primary"
                                OnClick="btnAddUser_Click" ValidationGroup="vgUsuario" />
                        </div>

                        <div class="col-1">
                            <!-- Id para edición -->
                            <asp:TextBox runat="server" ID="TextId" CssClass="form-control" Enabled="false" Visible="false" />
                        </div>
                    </div>

                </asp:View>

            </asp:MultiView>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>