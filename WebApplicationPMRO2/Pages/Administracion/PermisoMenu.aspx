<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PermisoMenu.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Administracion.PermisoMenu" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Permisos Menú</h2>
    <hr />

    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <!-- Buscador -->
            <div class="row g-2 align-items-end">
                <div class="col-md-6">
                    <label class="form-label">Buscar usuario (nombre o correo)</label>
                    <asp:TextBox ID="txtBuscarUsuario" runat="server" CssClass="form-control"
                        placeholder="Ej. Juan Pérez o juan@empresa.com" />
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar"
                        CssClass="btn btn-primary w-100" OnClick="btnBuscar_Click" />
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar"
                        CssClass="btn btn-outline-secondary w-100" OnClick="btnLimpiar_Click" />
                </div>
            </div>

            <!-- Resultados de usuarios -->
       
            <asp:GridView ID="gvUsuarios" runat="server" CssClass="table table-sm table-hover mt-3"
            AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="gvUsuarios_RowCommand"
            EmptyDataText="Sin resultados.">
            <Columns>
                <asp:BoundField DataField="nombreEmpleado" HeaderText="Nombre" />
                <asp:BoundField DataField="correo" HeaderText="Correo" />
                <asp:BoundField DataField="puesto" HeaderText="Puesto" />
                <asp:TemplateField HeaderText="Acción">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkElegir" runat="server"
                            CommandName="SelectUser"
                            CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            CssClass="btn btn-sm btn-info">
                            Elegir
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

            <!-- Usuario seleccionado -->
            <asp:Panel ID="pnlUsuario" runat="server" Visible="false" CssClass="mt-2">
                <div class="alert alert-info mb-2">
                    <strong>Usuario:</strong> <asp:Label ID="lblUsuario" runat="server" />
                </div>
                <asp:HiddenField ID="hfUserId" runat="server" />
            </asp:Panel>

            <!-- Menús -->
            <asp:GridView ID="gvMenus" runat="server" CssClass="table table-bordered table-striped"
    AutoGenerateColumns="False" DataKeyNames="MenuId,TienePermiso"
    OnRowCommand="gvMenus_RowCommand" OnRowDataBound="gvMenus_RowDataBound"
    EmptyDataText="Seleccione un usuario para mostrar sus permisos."
    Visible="false">
    <Columns>
        <asp:BoundField DataField="MenuId" HeaderText="ID" />
        <asp:BoundField DataField="Titulo" HeaderText="Menú" />

        <asp:TemplateField HeaderText="Estado">
            <ItemTemplate>

                <asp:Label ID="lblEstado" runat="server" CssClass="badge"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Acción">
            <ItemTemplate>
                <asp:LinkButton ID="btnAccion" runat="server"
                    CommandName="ToggleInsertDelete"
                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                    CssClass="btn btn-sm">
                    Acción
                </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

            <asp:Label ID="lblMsg" runat="server" CssClass="text-muted"></asp:Label>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnLimpiar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>