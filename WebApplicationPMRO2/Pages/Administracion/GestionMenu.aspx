<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionMenu.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Administracion.GestionMenu" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Gestión de Menú</h2>
    <hr style="color:black" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <!-- Barra de búsqueda y acciones -->
            <div class="row g-2 align-items-end">
                <div class="col-md-4">
                    <label for="<%= txtBuscarTitulo.ClientID %>" class="form-label">Buscar por título</label>
                    <asp:TextBox ID="txtBuscarTitulo" runat="server" CssClass="form-control" MaxLength="100" placeholder="Escribe el título a buscar..." />
                </div>
                <div class="col-md-2">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary w-100" OnClick="btnBuscar_Click" />
                </div>
                <div class="col-md-2">
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary w-100" OnClick="btnLimpiar_Click" />
                </div>
                <div class="col-md-4 text-end">
                    <asp:Button runat="server" ID="btnAddNew" Text="Nueva opción de menú" CssClass="btn btn-success" OnClick="btnAddNew_Click" />
                </div>
            </div>

            <asp:MultiView ID="mvwContainer" runat="server" ActiveViewIndex="0">

                <!-- Listado -->
                <asp:View ID="viewMaintenance" runat="server">
                    <asp:GridView ID="tblMenu" runat="server"
                        CssClass="table table-sm table-bordered mt-4"
                        AutoGenerateColumns="False" GridLines="None"
                        DataKeyNames="MenuId"
                        EmptyDataText="No hay menús registrados."
                        OnRowCommand="tblMenu_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="MenuId" HeaderText="ID" />
                            <asp:BoundField DataField="Titulo" HeaderText="Título" />
                            <asp:BoundField DataField="Url" HeaderText="URL" />
                            <asp:BoundField DataField="Icono" HeaderText="Ícono" />
                            <asp:BoundField DataField="Orden" HeaderText="Orden" />

                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <div class="d-flex justify-content-center">
                                        <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btn btn-sm btn-warning me-2"
                                            CommandName="Editar" CommandArgument='<%# Eval("MenuId") %>' />
                                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-sm btn-danger"
                                            CommandName="Eliminar" CommandArgument='<%# Eval("MenuId") %>' />
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
                            <asp:Button runat="server" ID="btnCancel" Text="Salir" CssClass="btn btn-secondary mt-2" OnClick="btnCancel_Click" />
                        </div>
                    </div>

                    <div class="row mt-3 gy-3">
                        <div class="col-md-4">
                            <asp:Label runat="server" AssociatedControlID="txtTitulo" Text="Título:" CssClass="form-label" />
                            <asp:TextBox runat="server" ID="txtTitulo" CssClass="form-control" MaxLength="100" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTitulo" ErrorMessage="El título es obligatorio."
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="vgMenu" />
                        </div>

                        <div class="col-md-4">
                            <asp:Label runat="server" AssociatedControlID="txtUrl" Text="URL:" CssClass="form-label" />
                            <asp:TextBox runat="server" ID="txtUrl" CssClass="form-control" MaxLength="255" placeholder="/Pages/Carpeta/Vista.aspx" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUrl" ErrorMessage="La URL es obligatoria."
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="vgMenu" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtUrl"
                                ValidationExpression="^/[-A-Za-z0-9_/\.]+$"
                                ErrorMessage="La URL debe ser relativa y comenzar con '/'."
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="vgMenu" />
                        </div>

                        <div class="col-md-2">
                            <asp:Label runat="server" AssociatedControlID="txtIcono" Text="Ícono:" CssClass="form-label" />
                            <asp:TextBox runat="server" ID="txtIcono" CssClass="form-control" MaxLength="100" placeholder="fa fa-home" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtIcono" ErrorMessage="El ícono es obligatorio."
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="vgMenu" />
                        </div>

                        <div class="col-md-2">
                            <asp:Label runat="server" AssociatedControlID="txtOrden" Text="Orden:" CssClass="form-label" />
                            <asp:TextBox runat="server" ID="txtOrden" CssClass="form-control" MaxLength="6" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrden" ErrorMessage="El orden es obligatorio."
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="vgMenu" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtOrden"
                                ValidationExpression="^\d+$"
                                ErrorMessage="El orden debe ser un número entero."
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="vgMenu" />
                        </div>

                        <div class="col-12">
                            <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="btn btn-primary"
                                OnClick="btnGuardar_Click" ValidationGroup="vgMenu" />
                        </div>

                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtMenuId" CssClass="form-control" ReadOnly="true" Visible="false" />
                        </div>
                    </div>
                </asp:View>

            </asp:MultiView>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>