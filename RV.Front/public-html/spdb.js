var map, path;
var markers = [];

// Inicjalizacja mapy.
function initMap() {
    map = new google.maps.Map(document.getElementById('map'), {
        center: {lat: 52.216404, lng: 21.016272},
        zoom: 8,
        mapTypeControl: false,
        streetViewControl: false
    });

    // Ograniczenie wyszukiwań do województwa mazowieckiego.
    var searchBounds = {bounds: new google.maps.LatLngBounds(
                                    new google.maps.LatLng(51.027513, 19.164994),
                                    new google.maps.LatLng(53.494106, 23.059860)),
                        strictBounds: true};
    var places = [
        new google.maps.places.Autocomplete(document.getElementById('startingPoint'), searchBounds),
        new google.maps.places.Autocomplete(document.getElementById('destinationPoint'), searchBounds)
    ];

    places.forEach(function(element) {
        google.maps.event.addListener(element, 'place_changed', function () {
            putMarkers(places);
        });
    });
}

// Nakładanie markerów na mapę.
function putMarkers(places) {
    clearMap();

    var bounds = new google.maps.LatLngBounds();
    let start = places[0].getPlace();
    let destination = places[1].getPlace();
    var icon = {
        url: start.icon,
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(26, 50),
        scaledSize: new google.maps.Size(50, 50),
        labelOrigin: new google.maps.Point(26, 70)
    };

    // Tworzenie markerów.
    markers.push(new google.maps.Marker({
        map: map,
        icon: icon,
        title: start.name,
        position: start.geometry.location,
        label: {text: "Punkt startowy", fontSize: '15px'}
    }));

    if(destination) {
        markers.push(new google.maps.Marker({
            map: map,
            icon: icon,
            title: destination.name,
            position: destination.geometry.location,
            label: {text: "Punkt docelowy", fontSize: '15px'}
        }));
    }

    markers.forEach(function(marker) {
        bounds.extend(marker.position);
    });
    map.fitBounds(bounds);
    if (!destination) map.setZoom(8);
    }

// Funkcja odblokowująca pole z minimalną długością.
function enableMinPath() {
    document.getElementById('minLength').disabled = false;
    document.getElementById('minLength').required = true;
}

// Funkcja zablokowująca pole z minimalną długością.
function disableMinPath() {
    document.getElementById('minLength').disabled = true;
    document.getElementById('minLength').required = false;
}

// Funkcja czyszcząca mapę z dotychczasowych ścieżek i markerów.
function clearMap() {
    markers.forEach(function(marker) {
        marker.setMap(null);
    });
    markers = [];

    if(path) {
        path.setMap(null);
    }
}

// Zatwierdzanie formularza.
function submitForm() {
    if(markers.length != 2) {
        alert("Wybierz dwa miejsca!")
        return;
    }
    var data = {};
    var url;
    data.sourcePoint = {};
    data.targetPoint = {};
    data.sourcePoint.latitude = markers[0].position.lat();
    data.sourcePoint.longitude = markers[0].position.lng();
    data.targetPoint.latitude = markers[1].position.lat();
    data.targetPoint.longitude = markers[1].position.lng();

    if(document.getElementById('modeShortest').checked) url = '/api/point/shortest-path-astar';
    else {
        url = '/api/point/shortest-path-with-view-roads';
        data.minimalLengthOfViewRoads = parseInt(document.getElementById('minLength').value);
    }

    var coordinates = [];
    var request = new XMLHttpRequest();
    request.responseType = 'json';

    //
    request.onload = function() {
        if (request.status == 200 && typeof request.response == typeof []) {
            request.response.forEach(function (element) {
                coordinates.push({
                    'lat': element['latitude'],
                    'lng': element['longitude']
                });
            });
        }

        path = new google.maps.Polyline({
            path: coordinates,
            geodesic: true,
            strokeColor: '#228B22',
            strokeOpacity: 1.0,
            strokeWeight: 3
        });
        path.setMap(map);
    };
    request.open("POST", "http://localhost:5000" + url, true);
    request.setRequestHeader('Content-Type', 'application/json');
    request.send(JSON.stringify(data));
}
