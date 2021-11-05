"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var THREE = require("three");
var OrbitControls_1 = require("three/examples/jsm/controls/OrbitControls");
var TransformControls_1 = require("three/examples/jsm/controls/TransformControls");
var IFCLoader_1 = require("three/examples/jsm/loaders/IFCLoader");
var ifcLoader;
var renderer;
var scene;
var camera;
var orbitControls;
var raycaster;
var sceneMeshes;
var standardMaterial;
var transformControls;
var blazorComponentRef;
var preselectMat = new THREE.MeshLambertMaterial({
    transparent: true,
    opacity: 0.6,
    color: 0xff88ff,
    depthTest: false
});
function SceneInitIFC(blazorComponentReference, ifcURL) {
    blazorComponentRef = blazorComponentReference;
    var divForCanvas = document.getElementById("divForCanvas");
    var width = divForCanvas.clientWidth;
    var height = divForCanvas.clientHeight;
    var canvas = document.createElement('canvas');
    canvas.width = width;
    canvas.height = height;
    divForCanvas.appendChild(canvas);
    sceneMeshes = [];
    scene = new THREE.Scene();
    var backgroundTexture = new THREE.TextureLoader().load('https://pabloassetmanagementclient.azurewebsites.net/images/backGround.jpg');
    scene.background = backgroundTexture;
    camera = new THREE.PerspectiveCamera(75, width / height, 0.1, 1000);
    renderer = new THREE.WebGLRenderer({ canvas: canvas, antialias: true });
    renderer.setSize(width, height);
    orbitControls = new OrbitControls_1.OrbitControls(camera, renderer.domElement);
    //Remap controls because I don't like defaults
    orbitControls.mouseButtons = {
        //LEFT: THREE.MOUSE.LEFT,
        MIDDLE: THREE.MOUSE.RIGHT,
        RIGHT: THREE.MOUSE.LEFT
    };
    transformControls = new TransformControls_1.TransformControls(camera, renderer.domElement);
    transformControls.mode = "translate";
    scene.add(transformControls);
    transformControls.addEventListener('dragging-changed', function (event) {
        orbitControls.enabled = !event.value;
    });
    var mainLight = new THREE.DirectionalLight(0xffffff, 1);
    mainLight.position.set(10, 10, 10);
    scene.add(mainLight);
    var ambientLight = new THREE.AmbientLight(0xffffff, 0.2);
    scene.add(ambientLight);
    ImportIFC(ifcURL);
    animateIFC();
    raycaster = new THREE.Raycaster();
    renderer.domElement.addEventListener('click', onIFCClick, false);
}
function onIFCClick(event) {
    var mouse = {
        x: (event.offsetX / renderer.domElement.clientWidth) * 2 - 1,
        y: -(event.offsetY / renderer.domElement.clientHeight) * 2 + 1
    };
    raycaster.setFromCamera(mouse, camera);
    var intersects = raycaster.intersectObjects(sceneMeshes, true);
    var loader = ifcLoader;
    if (intersects.length > 0) {
        var faceIndex = intersects[0].faceIndex;
        var geo = (intersects[0].object);
        var id = loader.ifcManager.getExpressId((intersects[0].object).geometry, faceIndex);
        loader.ifcManager.createSubset({
            modelID: 0,
            ids: [id],
            material: preselectMat,
            scene: scene,
            removePrevious: true
        });
        var propertySets = ExtractIFCPropertySets(loader.ifcManager, 0, id);
        blazorComponentRef.invokeMethodAsync("UpdateSelection", propertySets);
    }
    else {
        loader.ifcManager.removeSubset(0, scene, preselectMat);
        blazorComponentRef.invokeMethodAsync("CleanSelection");
    }
}
function ImportIFC(ifcURL) {
    ifcLoader = new IFCLoader_1.IFCLoader();
    ifcLoader.load(ifcURL, function (ifcModel) {
        scene.add(ifcModel.mesh),
            sceneMeshes.push(ifcModel.mesh);
        var mesh = ifcModel.mesh;
        //Center camera on imported model
        var geometry = mesh.geometry;
        geometry.computeBoundingSphere();
        var x = geometry.boundingSphere.center.x;
        var y = geometry.boundingSphere.center.y;
        var z = geometry.boundingSphere.center.z;
        camera.position.set(x + 10, y + 10, z + 10);
        orbitControls.target.set(x, y, z);
        orbitControls.update();
    }, function (xhr) {
        blazorComponentRef.invokeMethodAsync("SetLoadProgress", (xhr.loaded / xhr.total * 100));
    }, function (error) { console.log(error); });
}
function DisposeThree() {
    renderer.domElement.remove();
    renderer.dispose();
    var loader = ifcLoader;
    loader.ifcManager.close(0, scene);
}
var animateIFC = function () {
    requestAnimationFrame(animateIFC);
    resizeCanvasToDisplaySize();
    orbitControls.update();
    renderer.render(scene, camera);
};
function ExtractIFCPropertySets(ifcManager, modelId, objectId) {
    var propertySets = [];
    //Get IFC property set
    var IFCSet = ifcManager.getItemProperties(modelId, objectId, true);
    var ifcPropertySet = new PropertySet();
    ifcPropertySet.name = "IFC Properties";
    ifcPropertySet.properties.push(new Property("Global Id", "GUID", IFCSet.GlobalId.value));
    ifcPropertySet.properties.push(new Property("Name", "string", IFCSet.Name.value));
    ifcPropertySet.properties.push(new Property("Object type", "string", IFCSet.ObjectType.value));
    ifcPropertySet.properties.push(new Property("IFC type", "string", IFCSet.PredefinedType.value));
    propertySets.push(ifcPropertySet);
    //Get other property sets
    var Sets = ifcManager.getPropertySets(modelId, objectId, true);
    for (var i = 0; i < Sets.length; i++) {
        var propertySet = new PropertySet();
        var set = Sets[i];
        propertySet.name = set.Name.value;
        if (set.HasProperties != undefined) {
            for (var u = 0; u < set.HasProperties.length; u++) {
                var property = set.HasProperties[u];
                try {
                    propertySet.properties.push(new Property(property.Name.value, property.NominalValue.label, String(property.NominalValue.value)));
                }
                catch (_a) {
                    console.log(property);
                }
            }
            propertySets.push(propertySet);
        }
    }
    return propertySets;
}
function GetObjectByUserDataProperty(name, value) {
    for (var i = 0, l = scene.children.length; i < l; i++) {
        if (scene.children[i].userData[name] === value) {
            return scene.children[i];
        }
    }
    return undefined;
}
function resizeCanvasToDisplaySize() {
    var canvas = renderer.domElement;
    // look up the size the canvas is being displayed
    var width = canvas.clientWidth;
    var height = canvas.clientHeight;
    // adjust displayBuffer size to match
    if (canvas.width !== width || canvas.height !== height) {
        // you must pass false here or three.js sadly fights the browser
        renderer.setSize(width, height, false);
        camera.aspect = width / height;
        camera.updateProjectionMatrix();
        // update any render target sizes here
    }
}
window.SceneInitIFC = SceneInitIFC;
window.DisposeThree = DisposeThree;
//(window as any).SetSelection = SetSelection;
var PropertySet = /** @class */ (function () {
    function PropertySet() {
        this.properties = [];
    }
    return PropertySet;
}());
var Property = /** @class */ (function () {
    function Property(name, type, value) {
        this.name = name;
        this.type = type;
        this.value = value;
    }
    return Property;
}());
//# sourceMappingURL=IFCInterop.js.map