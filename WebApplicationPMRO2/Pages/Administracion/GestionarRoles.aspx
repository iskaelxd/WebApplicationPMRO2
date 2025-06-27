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
    <asp:DropDownList ID="ddlModulo" runat="server" CssClass="form-select"></asp:DropDownList>
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

        <%--Tabla de roles--%>

        <table class="table table-sm table-bordered mt-5">
            <thead>
            <tr>
              <th scope="col">IdRol</th>
              <th scope="col">Modulo</th>
              <th scope="col">Rol</th>
              <th scope="col">Acciones</th>
            </tr>
            </thead>
            <tbody>
            <tr>
              <th scope="row">1</th>
              <td>Almacenista</td>
              <td>almacen</td>
              <td>ACCIONES</td>
            </tr>
            <tr>
              <th scope="row">2</th>
              <td>Comprador</td>
              <td>Compras</td>
              <td>ACCIONES</td>
            </tr>
            <tr>
              <th scope="row">3</th>
              <td>Administrador</td>
              <td>Admonistrador</td>
              <td>ACCIONES</td>
            </tr>
            </tbody>
          </table>
    
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
