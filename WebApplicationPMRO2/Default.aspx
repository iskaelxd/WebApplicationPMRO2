<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplicationPMRO2._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
 
    <style>
    .main-content {
        padding: 0px;
        position: relative;
    }

    #threejs-container {
        width: 100%;
        height: 100vh;
        margin: 0;
        overflow: hidden;
    }

    #mro-text {
        position: absolute;
        bottom: 20px;
        width: 100%;
        text-align: center;
        font-size: 64px;
        color: #888888;
        font-weight: bold;
        font-family: Arial, sans-serif;
        pointer-events: none; /* para que no interfiera con el mouse */
    }
</style>

    <div id="threejs-container"  style="margin: 0px; "></div>
    <div id="mro-text">MRO</div>
            
                      <script>
                          if (typeof THREE === 'undefined') {
                              console.error('Three.js no está cargado.');
                          } else {
                              initThreeJS();
                          }

                          function initThreeJS() {
                              console.log('Inicializando Three.js...');
                              const scene = new THREE.Scene();
                              const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);

                              const renderer = new THREE.WebGLRenderer();
                              renderer.setSize(window.innerWidth, window.innerHeight);
                              renderer.setClearColor(0xffffff);
                              document.getElementById('threejs-container').appendChild(renderer.domElement);

                              const controls = new THREE.OrbitControls(camera, renderer.domElement);
                              controls.enableDamping = true;
                              controls.dampingFactor = 0.05;

                              const directionalLight = new THREE.DirectionalLight(0xffffff, 1);
                              directionalLight.position.set(5, 10, 7.5);
                              scene.add(directionalLight);

                              const ambientLight = new THREE.AmbientLight(0x404040, 2);
                              scene.add(ambientLight);

                              const loader = new THREE.GLTFLoader();
                              loader.load('Modelos3D/Camioneta1/scene.gltf', function (gltf) {
                                  scene.add(gltf.scene);
                                  animate();
                              }, undefined, function (error) {
                                  console.error(error);
                              });

                              camera.position.set(0, 2, 5);

                              function animate() {
                                  requestAnimationFrame(animate);
                                  controls.update();
                                  renderer.render(scene, camera);
                              }
                          }
                      </script>

    
</asp:Content>
