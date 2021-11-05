import * as THREE from 'three'
import { BufferGeometry, Raycaster, Scene, TextureLoader } from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls'
import { TransformControls } from 'three/examples/jsm/controls/TransformControls'
import { IFCLoader } from 'three/examples/jsm/loaders/IFCLoader';


let ifcLoader: IFCLoader;
let renderer: THREE.WebGLRenderer;
let scene: THREE.Scene;
let camera: THREE.PerspectiveCamera;
let orbitControls: OrbitControls;
let raycaster: Raycaster;
let sceneMeshes: THREE.Object3D[]
let standardMaterial: THREE.Material;
let transformControls: TransformControls;
let blazorComponentRef;

const preselectMat = new THREE.MeshLambertMaterial({
	transparent: true,
	opacity: 0.6,
	color: 0xff88ff,
	depthTest: false
})



function SceneInitIFC(blazorComponentReference: any, ifcURL: string) {

	blazorComponentRef = blazorComponentReference;
	let divForCanvas: HTMLDivElement = <HTMLDivElement>document.getElementById("divForCanvas");
	let width: number = divForCanvas.clientWidth;
	let height: number = divForCanvas.clientHeight;
	let canvas: HTMLCanvasElement = document.createElement('canvas');
	canvas.width = width;
	canvas.height = height;
	divForCanvas.appendChild(canvas);

	sceneMeshes = [];

	scene = new THREE.Scene();
	let backgroundTexture = new THREE.TextureLoader().load('https://pabloassetmanagementclient.azurewebsites.net/images/backGround.jpg');
	scene.background = backgroundTexture;

	camera = new THREE.PerspectiveCamera(75, width / height, 0.1, 1000);
	renderer = new THREE.WebGLRenderer({ canvas: canvas, antialias: true })
	renderer.setSize(width, height);

	orbitControls = new OrbitControls(camera, renderer.domElement);

	//Remap controls because I don't like defaults
	orbitControls.mouseButtons = <any>{
		//LEFT: THREE.MOUSE.LEFT,
		MIDDLE: THREE.MOUSE.RIGHT,
		RIGHT: THREE.MOUSE.LEFT
	}
	transformControls = new TransformControls(camera, renderer.domElement);
	transformControls.mode = "translate";
	scene.add(transformControls);
	transformControls.addEventListener('dragging-changed', function (event) {
		orbitControls.enabled = !event.value;
	});

	const mainLight = new THREE.DirectionalLight(0xffffff, 1)
	mainLight.position.set(10, 10, 10)
	scene.add(mainLight)

	let ambientLight = new THREE.AmbientLight(0xffffff, 0.2);
	scene.add(ambientLight);


	ImportIFC(ifcURL);

	animateIFC();

	raycaster = new THREE.Raycaster();
	renderer.domElement.addEventListener('click', onIFCClick, false);

}


function onIFCClick(event: MouseEvent) {

	const mouse = {
		x: (event.offsetX / renderer.domElement.clientWidth) * 2 - 1,
		y: -(event.offsetY / renderer.domElement.clientHeight) * 2 + 1
	}
	raycaster.setFromCamera(mouse, camera)

	const intersects = raycaster.intersectObjects(sceneMeshes, true)
	let loader = <any>ifcLoader;
	if (intersects.length > 0) {
		let faceIndex = intersects[0].faceIndex;
		let geo = <any>(intersects[0].object)
		let id = loader.ifcManager.getExpressId((<THREE.Mesh>(intersects[0].object)).geometry, faceIndex);


		loader.ifcManager.createSubset({
			modelID: 0,
			ids: [id],
			material: preselectMat,
			scene: scene,
			removePrevious: true
		})
		let propertySets = ExtractIFCPropertySets(loader.ifcManager, 0, id)
		blazorComponentRef.invokeMethodAsync("UpdateSelection", propertySets);
	}
	else {
		loader.ifcManager.removeSubset(0, scene, preselectMat);
		blazorComponentRef.invokeMethodAsync("CleanSelection");

	}
}

function ImportIFC(ifcURL : string) {
	ifcLoader = new IFCLoader();
	ifcLoader.load(ifcURL,
		(ifcModel) => {

			scene.add(ifcModel.mesh),
				sceneMeshes.push(ifcModel.mesh)
			let mesh = <THREE.Mesh>ifcModel.mesh

			//Center camera on imported model
			var geometry = mesh.geometry;
			geometry.computeBoundingSphere();
			let x = geometry.boundingSphere.center.x;
			let y = geometry.boundingSphere.center.y;
			let z = geometry.boundingSphere.center.z;
			camera.position.set(x + 10, y + 10, z + 10);
			orbitControls.target.set(x, y, z);
			orbitControls.update();

		},
		(xhr) =>
		{
			blazorComponentRef.invokeMethodAsync("SetLoadProgress", (xhr.loaded / xhr.total * 100));
		},
		(error) => { console.log(error)})
		;
}

function DisposeThree() {

	renderer.domElement.remove();
	renderer.dispose();
	let loader = <any>ifcLoader;
	loader.ifcManager.close(0, scene);
}

const animateIFC = function () {
	requestAnimationFrame(animateIFC);
	resizeCanvasToDisplaySize();
	orbitControls.update();
	renderer.render(scene, camera);
};

function ExtractIFCPropertySets(ifcManager: any, modelId: number, objectId: number)
{

	let propertySets: PropertySet[] = [];
	//Get IFC property set
	var IFCSet = ifcManager.getItemProperties(modelId, objectId, true)
	let ifcPropertySet = new PropertySet();
	ifcPropertySet.name = "IFC Properties";
	ifcPropertySet.properties.push(new Property("Global Id", "GUID", IFCSet.GlobalId.value));
	ifcPropertySet.properties.push(new Property("Name", "string", IFCSet.Name.value));
	ifcPropertySet.properties.push(new Property("Object type", "string", IFCSet.ObjectType.value))
	ifcPropertySet.properties.push(new Property("IFC type", "string", IFCSet.PredefinedType.value))
	propertySets.push(ifcPropertySet);



	//Get other property sets
	var Sets = ifcManager.getPropertySets(modelId, objectId, true);

	for (var i = 0; i < Sets.length; i++) {
		let propertySet = new PropertySet();
		let set = Sets[i];
		propertySet.name = set.Name.value
		if (set.HasProperties != undefined) {
			for (var u = 0; u < set.HasProperties.length; u++) {
				let property = set.HasProperties[u];
				try {
					propertySet.properties.push(new Property(property.Name.value, property.NominalValue.label, String(property.NominalValue.value)))
				}
				catch {
					console.log(property);
				}

			}
			propertySets.push(propertySet);
        }

	}


	return propertySets;
}

function GetObjectByUserDataProperty(name: string, value: any) {

	for (var i = 0, l = scene.children.length; i < l; i++) {

		if (scene.children[i].userData[name] === value) {
			return scene.children[i];
		}
	}
	return undefined;

}

function resizeCanvasToDisplaySize() {
	const canvas = renderer.domElement;
	// look up the size the canvas is being displayed
	const width = canvas.clientWidth;
	const height = canvas.clientHeight;

	// adjust displayBuffer size to match
	if (canvas.width !== width || canvas.height !== height) {
		// you must pass false here or three.js sadly fights the browser
		renderer.setSize(width, height, false);
		camera.aspect = width / height;
		camera.updateProjectionMatrix();

		// update any render target sizes here
	}
}

(window as any).SceneInitIFC = SceneInitIFC;
(window as any).DisposeThree = DisposeThree;
//(window as any).SetSelection = SetSelection;





class PropertySet {
	name: string;
	properties: Property[]
	constructor() {
		this.properties = []
    }

}

class Property {
	name: string;
	type: string;
	value: string;

	constructor(name: string, type: string, value: string) {
		this.name = name;
		this.type = type;
		this.value = value;
	}
}
