<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Recepciones.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Almacen.Recepciones" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<ul class="nav nav-tabs mb-4" id="myTab" role="tablist">
  <li class="nav-item" role="presentation">
    <button class="nav-link active" id="home-tab" data-bs-toggle="tab" data-bs-target="#home-tab-pane" type="button" role="tab" aria-controls="home-tab-pane" aria-selected="true">Registrar Numero de Parte</button>
  </li>
  <li class="nav-item" role="presentation">
    <button class="nav-link" id="profile-tab" data-bs-toggle="tab" data-bs-target="#profile-tab-pane" type="button" role="tab" aria-controls="profile-tab-pane" aria-selected="false">Actulizar Numero de Parte</button>
  </li>
  <li class="nav-item" role="presentation">
    <button class="nav-link" id="contact-tab" data-bs-toggle="tab" data-bs-target="#contact-tab-pane" type="button" role="tab" aria-controls="contact-tab-pane" aria-selected="false"> Comprador </button>
  </li>
  <li class="nav-item" role="presentation">
    <button class="nav-link" id="disabled-tab" data-bs-toggle="tab" data-bs-target="#disabled-tab-pane" type="button" role="tab" aria-controls="disabled-tab-pane" aria-selected="false" > Locaciones </button>
  </li>
</ul>
<div class="tab-content" id="myTabContent">
  <div class="tab-pane fade show active" id="home-tab-pane" role="tabpanel" aria-labelledby="home-tab" tabindex="0">

              <div class="row align-items-center mb-5">
            <div class="col-6">
                <h2>Números de Parte</h2>
            </div>
            <div class="col-6 text-end">
                <asp:Button ID="btnMultiview" runat="server" Text="Actualizar Inventario" CssClass="btn btn-success" OnClick="btnMultiview_Click" />
            </div>

        </div>

<asp:MultiView ID="MultiViewNumerosParte" runat="server" ActiveViewIndex="0">
    <asp:View ID="ViewNumerosParte" runat="server" EnableViewState="true">

        <div class="row">
            <div class="col-4">

                <label>Numero de Parte</label>
                <asp:TextBox ID="txtNumeroParte" runat="server" CssClass="form-control" placeholder="Numero de Parte"></asp:TextBox>

            </div>
            <div class="col-8">
                <label>Descripcion del Numero de Parte</label>
                <asp:TextBox ID="txtDescripcionNumeroParte" runat="server" CssClass="form-control" placeholder="Descripcion de Numero de Parte"></asp:TextBox>
            </div>
        </div>

        <div class="row">
            <div class="col-2">
                <label>MinStock</label>
                <asp:TextBox ID="txtMinStock" runat="server" CssClass="form-control" placeholder="Min"></asp:TextBox>
            </div>
            <div class="col-2">
                <label>MaxStock</label>
                <asp:TextBox ID="txtMaxStock" runat="server" CssClass="form-control" placeholder="Max" />
            </div>

            <div class="col-4">
                <label>Selecciona una Categoria</label>
                <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select">

                </asp:DropDownList>
            </div>

            <div class="col-4">

                <label>Inventory</label>
                <asp:TextBox ID="txtInventory" runat="server" CssClass="form-control" placeholder="Cantidad en el Inventario"></asp:TextBox>

            </div>

        </div>

        <div class="row">

            <div class="col-4">
                <label>Location</label>
                <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-select" >

                </asp:DropDownList>
            </div>


            <div class="col-4">
                <label>Comprador</label>
                <asp:DropDownList ID="ddlBuyer" runat="server" CssClass="form-select" >

                </asp:DropDownList>
            </div>

            <div class="col-4">
                <label>UM</label>
                <asp:TextBox ID="txtUM" runat="server" CssClass="form-control" placeholder="Unidad de Medida" />
            </div>

        </div>

        <div class="row mt-4">
            <div class="col-12 text-end">
                <asp:Button ID="btnGuardarNumeroParte" runat="server" Text="Guardar Número de Parte" CssClass="btn btn-primary" OnClick="btnGuardarNumeroParte_Click" />
            </div>
            </div>

    </asp:View> 

    <asp:View ID="ViewActulizarNumerosParte" runat="server">

        <div class="row align-items-end">
    <div class="col-4">
        <label>Numero de Parte</label>
        <asp:TextBox ID="txtNumeroParteActualizar" runat="server" CssClass="form-control" placeholder="Numero de Parte"></asp:TextBox>
    </div>

    <div class="col-4">
        <label>Cantidad</label>
        <asp:TextBox ID="txtCantidadActualizar" runat="server" CssClass="form-control" placeholder="La cantidad ingresada se sumará al inventario actual"></asp:TextBox>
    </div>

    <div class="col-4 d-flex justify-content">
        <asp:Button Text="Actualizar" CssClass="btn btn-primary" runat="server" />
    </div>
</div>


    </asp:View> 


</asp:MultiView>



  </div>
  <div class="tab-pane fade" id="profile-tab-pane" role="tabpanel" aria-labelledby="profile-tab" tabindex="0">
      <h2>Profile</h2>
  </div>
  <div class="tab-pane fade" id="contact-tab-pane" role="tabpanel" aria-labelledby="contact-tab" tabindex="0">

      <h2>Comprador</h2>
  </div>
  <div class="tab-pane fade" id="disabled-tab-pane" role="tabpanel" aria-labelledby="disabled-tab" tabindex="0">
      <h2>Locacion</h2>
  </div>
</div>


</asp:Content>

