function InitializeMap(assets, avgLatitude, avgLongitud, zoomLevel) {


    let map = L.map('mapDiv').setView([avgLatitude, avgLongitud], zoomLevel);
    let markers = new L.FeatureGroup();
    map.addLayer(markers);

    //Set map tiles source
    var CartoDB_Voyager = L.tileLayer('https://{s}.basemaps.cartocdn.com/rastertiles/voyager/{z}/{x}/{y}{r}.png', {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors &copy; <a href="https://carto.com/attributions">CARTO</a>',
        subdomains: 'abcd',
        maxZoom: 19
    }).addTo(map);


    //Add new markers
    for (let asset of assets) {
        let marker = L.marker([asset.latitude, asset.longitude]);
        let link = nameField = `<a href="/assets/${asset.id.toString()}">${asset.name}</a>`;
        marker.bindPopup(link);
        markers.addLayer(marker)
    }

    return markers;

}

function UpdateMap(markers, assets) {
    markers.clearLayers();

    for (let asset of assets) {
        let marker = L.marker([asset.latitude, asset.longitude]);
        let link = nameField = `<a href="/assets/${asset.id.toString()}">${asset.name}</a>`;
        marker.bindPopup(link);
        markers.addLayer(marker)
    }
}