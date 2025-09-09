<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Seguimiento_Solicitud.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Almacen.Seguimiento_Solicitud" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Seguimiento Solicitudes</h2>

    <hr />


        <div class="row"> 

            <div class="col-4">
            <label>Bucar por Numero Parte</label>
            <input type="text" id="txtPartNumb" runat="server" class="form-control" placeholder="Numero de Parte" />
            </div>

            <div class="col-4">
                <asp:Button  ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary mt-4" />
            </div>

        </div>
    
    <div class="row mt-4">
           <asp:GridView ID="tblSolicitudes" runat="server"
           CssClass="table table-hover table-bordered mb-0"
           AutoGenerateColumns="False"
           GridLines="None"
           EmptyDataText="No hay Solicitudes."
           OnRowDataBound="tblSolicitudes_RowDataBound">
           <Columns>
               <asp:BoundField DataField="OrderHeaderId" HeaderText="OrderHeaderId" SortExpression="OrderHeaderId" />
               <asp:BoundField DataField="Area" HeaderText="Area" SortExpression="Area" />
               <asp:BoundField DataField="Linea" HeaderText="Linea" SortExpression="Linea" />
               <asp:BoundField DataField="status" HeaderText="EStatus" SortExpression="Status" />
               <asp:BoundField DataField="PartNumb" HeaderText="PartNumb" SortExpression="PartNumb" />
               <asp:BoundField DataField="OrderQnty" HeaderText="OrderQnty" SortExpression="OrderQnty" />
               <asp:BoundField DataField="Solicitado" HeaderText="Solicitado Por" SortExpression="Solicitado Por" />
               <asp:BoundField DataField="Date" HeaderText="Fecha" SortExpression="Fecha" />
               <asp:BoundField DataField="Marcado" HeaderText="Marcado" SortExpression="Marcado" />
                <asp:BoundField DataField="SAPM" HeaderText="SAPM" SortExpression="SAPM" />
           </Columns>
       </asp:GridView>
        </div>



</asp:Content>
    