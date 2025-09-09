<%@ Page Language="C#" MasterPageFile="~/Site.Master" CodeBehind="Movimientos.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Almacen.Movimientos" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Descontar en SAP</h3>
    <hr />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
      <ContentTemplate>

        <div class="row g-3 align-items-end">
          <div class="col-md-5">
            <label class="form-label">Número de parte o descripción</label>
            <asp:TextBox ID="txtPartNumb" runat="server" CssClass="form-control" MaxLength="100"
              placeholder="Ej. 470002 o 'DRILL'..." />
          </div>

          <div class="col-md-2">
            <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary w-100"
              Text="Buscar" OnClick="btnBuscar_Click" />
          </div>
        </div>

        <div class="mt-4">
          <asp:GridView ID="gvPartes" runat="server"
                CssClass="table table-striped table-hover"
                AutoGenerateColumns="False"
                AllowPaging="True" PageSize="15"
                OnPageIndexChanging="gvPartes_PageIndexChanging"
                OnRowCommand="gvPartes_RowCommand">
            <Columns>
              <asp:BoundField DataField="PartNumb" HeaderText="PartNumb" />
              <asp:BoundField DataField="PartDescription" HeaderText="PartDescription" />
              <asp:BoundField DataField="Total" HeaderText="Cantidad" />
              <asp:TemplateField HeaderText="Acción">
                <ItemTemplate>
                  <asp:Button ID="btnPasarSap" runat="server" Text="Pasar a SAP"
                      CssClass="btn btn-warning btn-sm"
                      CommandName="PasarSAP"
                      CommandArgument='<%# Eval("PartNumb") %>'
                      OnClientClick="return confirm('¿Marcar como pasado a SAP este PartNumb?');" />
                </ItemTemplate>
              </asp:TemplateField>
            </Columns>
          </asp:GridView>
        </div>

      </ContentTemplate>
      <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
      </Triggers>
    </asp:UpdatePanel>

    <hr />
</asp:Content>