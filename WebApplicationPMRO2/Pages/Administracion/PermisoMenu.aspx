<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PermisoMenu.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Administracion.PermisoMenu" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    

       <h2>Permisos Menu</h2>
    <hr style="color:black"/>

      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

        <asp:HiddenField ID="hfIdUsuario" runat="server" />

        <div class="row">
            <div class="col-8">
        <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Buscar usuario"></asp:TextBox>
            </div>

            <div class="col-4">
                <asp:Button  ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
            </div>

        </div>
        

<ajaxToolkit:AutoCompleteExtender
    ID="ae_txtBuscar" 
    runat="server" 
    TargetControlID="txtBuscar"
    ServiceMethod="BuscarUsuarios" 
    MinimumPrefixLength="2" 
    CompletionInterval="100" 
    EnableCaching="true" 
    CompletionSetCount="10"
    FirstRowSelected="true" />




        </ContentTemplate>
      </asp:UpdatePanel>
</asp:Content>
