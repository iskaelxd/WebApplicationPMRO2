<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Login" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <title>Iniciar sesión – MRO</title>

    <!-- Bootstrap 5 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
          rel="stylesheet" />
    
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

    <script src="../Scripts/WebForms/alert.js"></script>


</head>
<body class="m-0">

   

<form id="form1" runat="server" class="d-flex vh-100 vw-100">

    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <!-- Lado izquierdo (branding) -->
    <aside class="w-50 d-flex flex-column justify-content-center align-items-center
                 bg-dark text-white p-4">
        <h1 class="display-1 fw-bold">MRO</h1>
        <p class="mt-4 text-center fs-4" style="max-width:400px;">
            Mantenimiento Reparación y Operaciones
        </p>
    </aside>

    <!-- Lado derecho (login) -->
    <section class="w-50 d-flex flex-column justify-content-center align-items-center
                    bg-light p-4">
        <div class="w-100" style="max-width:400px;">

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <!-- Nº Empleado -->
                    <div class="mb-3">
                        <label for="<%= txtNumeroEmpleado.ClientID %>" class="form-label">
                            Número de Empleado
                        </label>
                        <asp:TextBox ID="txtNumeroEmpleado" runat="server"
                                     CssClass="form-control" placeholder="Número Empleado" />
                        <asp:RequiredFieldValidator ID="rfvNumEmpleado" runat="server"
                                     ControlToValidate="txtNumeroEmpleado"
                                     ErrorMessage="Requerido" CssClass="text-danger"
                                     Display="Dynamic" />
                    </div>

                    <!-- Contraseña -->
                    <div class="mb-3">
                        <label for="<%= txtPassword.ClientID %>" class="form-label">
                            Contraseña
                        </label>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"
                                     CssClass="form-control" placeholder="Contraseña" />
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                                     ControlToValidate="txtPassword"
                                     ErrorMessage="Requerido" CssClass="text-danger"
                                     Display="Dynamic" />
                    </div>

                    <!-- Botón -->
                    <asp:Button ID="btnLogin" runat="server"
                                Text="Iniciar Sesión"
                                CssClass="btn btn-primary w-100"
                                OnClick="btnLogin_Click" />

                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </section>
     
</form>


    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
  

</body>
</html>
