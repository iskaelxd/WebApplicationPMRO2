<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Produccion.Reports" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h3>Reportes</h3>
    <hr />

    <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <!-- Filtros -->
            <div class="row g-2 align-items-end">
                <div class="col-md-6">
                    <label for="txtPartDesc" class="form-label">PartDescription (búsqueda aproximada)</label>
                    <asp:TextBox ID="txtPartDesc" runat="server" CssClass="form-control" MaxLength="200" placeholder="Ej. filtro por texto dentro de la descripción..." />
                </div>

                <div class="col-md-6 d-flex gap-2">
                    <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary" Text="Buscar" OnClick="btnBuscar_Click" />
                    <asp:Button ID="btnLimpiar" runat="server" CssClass="btn btn-outline-secondary" Text="Limpiar" OnClick="btnLimpiar_Click" CausesValidation="false" UseSubmitBehavior="false" />
                </div>
            </div>

            <div class="mt-3">
                <asp:Label ID="lblResumen" runat="server" CssClass="text-muted"></asp:Label>
            </div>

            <!-- Resultados -->
            <div class="table-responsive mt-2">
                <asp:GridView ID="gvResultados" runat="server" AutoGenerateColumns="False" CssClass="table table-sm table-striped table-hover align-middle"
                    EmptyDataText="Sin resultados para los criterios de búsqueda."
                    AllowPaging="true" PageSize="20" OnPageIndexChanging="gvResultados_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="PartNumb" HeaderText="PartNumb" />
                        <asp:BoundField DataField="PartDescription" HeaderText="PartDescription" />
                        <asp:BoundField DataField="TotalOrderQnty" HeaderText="Total Qty" DataFormatString="{0:N0}" HtmlEncode="false" />
                    </Columns>
                    <PagerStyle CssClass="pagination" />
                    <HeaderStyle CssClass="table-dark" />
                </asp:GridView>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnLimpiar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript">
        // Enter en el textbox dispara Buscar
        document.addEventListener('DOMContentLoaded', function () {
            var tb = document.getElementById('<%= txtPartDesc.ClientID %>');
            if (tb) {
                tb.addEventListener('keydown', function (e) {
                    if (e.key === 'Enter') {
                        e.preventDefault();
                        document.getElementById('<%= btnBuscar.ClientID %>').click();
                    }
                });
            }
        });
    </script>

</asp:Content>