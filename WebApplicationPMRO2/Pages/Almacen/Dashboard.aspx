<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="WebApplicationPMRO2.Pages.Almacen.Dashboard" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


        <style>
        .chart-container {
            position: relative;
            width: 100%;
            height: 250px;
            margin: auto;
        }


            .card-body {
                display: flex;
                flex-direction: column;
                align-items: center;
                justify-content: center;

            }

            .bar-chart-container {
            position: relative;
            width: 100%;
            height: 500px; 
            margin: auto;
            }

            .bar-chart-container canvas {
                width: 100% !important;
                height: 100% !important;
            }

            
    .tabla-categorias {
        width: 100%;
        border-collapse: collapse;
    }

    .tabla-categorias th,
    .tabla-categorias td {
        padding: 10px;
        border: 1px solid #ddd;
        text-align: left;
    }

    .tabla-categorias tr:hover {
        background-color: #f5f5f5;
    }

  
    .fila-roja {
        background-color: #f44336 !important; 
        color: white;
    }

    .fila-amarilla {
        background-color: #ffeb3b !important; 
        color: black;
    }

    .fila-verde {
        background-color: #c8e6c9 !important; 
        color: black;
    }
</style>




     <h3 class="text-center">Dashboard</h3>
     <hr />

    <div class="container mt-4">

        <!-- Gráfico de Barras -->
        <div class="row mb-5">
            <div class="col-md-12">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="bar-chart-container">
                            <canvas id="barChart"></canvas>
                        </div>
                        <p class="mt-2 text-center"><strong>Estatus de las ordenes</strong></p>
                    </div>
                </div>
            </div>
        </div>

            <asp:GridView ID="tblCategorias" runat="server" ClientIDMode="Static" CssClass="tabla-categorias"
    AutoGenerateColumns="False" GridLines="None" EmptyDataText="No hay módulos registrados." OnRowDataBound="tblCategorias_RowDataBound">
    <Columns>
        <asp:BoundField DataField="CategoryId" HeaderText="CategoryId" SortExpression="CategoryId" />
        <asp:BoundField DataField="CategoryName" HeaderText="Category Nombre" SortExpression="CategoryName"/>
        <asp:BoundField DataField="Inventario" HeaderText="Inventario" SortExpression="Inventario" />
        <asp:BoundField DataField="MinStock" HeaderText="MinStock" Visible="false" />
        <asp:BoundField DataField="MaxStock" HeaderText="MaxStock" Visible="false" />
    </Columns>
</asp:GridView>



        <!-- Gráficos de Pastel -->
        <div class="row text-center mt-4">
            <div class="col-md-4">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="chart-container">
                            <canvas id="chart1"></canvas>
                        </div>
                        <p class="mt-3"><strong>Inventario</strong><br />Productos disponibles y agotados.</p>
                    </div>
                </div>
            </div>

            <div class="col-md-4">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="chart-container">
                            <canvas id="chart2"></canvas>
                        </div>
                        <p class="mt-3"><strong> Ordenes </strong><br />Ordenes listas y pendientes.</p>
                    </div>
                </div>
            </div>

            <div class="col-md-4">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="chart-container">
                            <canvas id="chart3"></canvas>
                        </div>
                        <p class="mt-3"><strong>Stock</strong><br />Nivel de Stock del Inventario.</p>
                    </div>
                </div>
            </div>

        </div>
    </div>

    <!-- Chart.js CDN -->

  
    
    <script>
        // Gráfico de Barras
        new Chart(document.getElementById('barChart'), {
            type: 'bar',
            data: {
                labels: ['Creada', 'Aprobada', 'Visto', 'Entregada', 'Sin Inventario','Listas'],
                datasets: [{
                    label: 'Solicitud Indirectos',
                    data: [12, 19, 3, 5, 2,5],
                    backgroundColor: '#007bff'
                }]
            },
            options: {
                responsive: true,
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });

        // Configuración base para gráficos de pastel
        const pieConfig = (labels, data, colors) => ({
            type: 'pie',
            data: {
                labels: labels,
                datasets: [{
                    data: data,
                    backgroundColor: colors
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                aspectRatio: 1
            }
        });

        new Chart(document.getElementById('chart1'), pieConfig(
            ['Disponible', 'Agotado'],
            [70, 30],
            ['#28a745', '#dc3545']
        ));

        new Chart(document.getElementById('chart2'), pieConfig(
            ['Listos', 'Pendientes'],
            [40, 50],
            ['#ffc107', '#17a2b8', '#6c757d']
        ));

        new Chart(document.getElementById('chart3'), pieConfig(
            ['Ok', 'Low','Empty'],
            [30, 40,30],
            ['#6610f2', '#20c997', '#6c757d']
        ));
    </script>
    

</asp:Content>
