// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {

    if (document.getElementById("notification")) {
        if (document.getElementById("notification").innerHTML == "You are interested in this gig") {
            document.getElementById("interested").src = "/images/orange_bell.png";
        }
        if (document.getElementById("notification").innerHTML == "You are going to this gig") {
            document.getElementById("going").src = "/images/orange_tick.png";
        }
    };

    if (document.getElementById("age")) {
        calculateAge(document.getElementById("age").innerHTML);
    };

    $('#genres').multiselect({
        columns: 2,
        placeholder: 'Select genres'
    });

    if (document.getElementById("search")) {
        $("#search").keyup(function () {
            $(".filtered").each(function () {
                if (($(this).text().toUpperCase().includes($("#search").val().toUpperCase()))) {
                    $(this).show();
                }
                else {
                    $(this).hide();
                }
            });
        });
    }
});

function calculateAge(dob) {
    var dobValues = dob.split("/");

    var d = new Date();
    var dobYear = parseInt(dobValues[2]);
    var dobMonth = parseInt(dobValues[1]);
    var dobDay = parseInt(dobValues[0]);
    var currentYear = d.getFullYear();
    var currentMonth = d.getMonth() + 1;
    var currentDay = d.getDate();

    var age = currentYear - dobYear;
    if (currentMonth < dobMonth) {
        age = age - 1;
    }
    if (currentMonth == dobMonth && currentDay < dobDay) {
        age = age - 1;
    }

    document.getElementById("age").innerHTML = "Age: " + age;
}

function maybeOver() {
    if (document.getElementById("interested").src.includes("orange_bell.png")) {
        document.getElementById("interested").src = "/images/blank_bell.png";
    }
    else {
        document.getElementById("interested").src = "/images/orange_bell.png";
    }
}

function maybeOut() {
    if (document.getElementById("interested").src.includes("orange_bell.png")) {
        document.getElementById("interested").src = "/images/blank_bell.png";
    }
    else {
        document.getElementById("interested").src = "/images/orange_bell.png";
    }
}

function goingOver() {
    if (document.getElementById("going").src.includes("orange_tick.png")) {
        document.getElementById("going").src = "/images/blank_tick.png";
    }
    else {
        document.getElementById("going").src = "/images/orange_tick.png";
    }
}

function goingOut() {
    if (document.getElementById("going").src.includes("orange_tick.png")) {
        document.getElementById("going").src = "/images/blank_tick.png";
    }
    else {
        document.getElementById("going").src = "/images/orange_tick.png";
    }
}

function gigOver(x) {
    x.style.backgroundColor = "#007BFF";
} 

function gigOut(x) {
    x.style.backgroundColor = "#48D7F5";
}

function showIt(x) {
    if (x.innerHTML == 'Show') {
        document.getElementById("hidden").style.visibility = 'visible';
        x.innerHTML = 'Hide';
    }
    else {
        document.getElementById("hidden").style.visibility = 'hidden';
        x.innerHTML = 'Show';
    }
}

var map;
var service;
var infowindow;

function showMap() {
    var london = new google.maps.LatLng(51.5074, 0.1278);

    infowindow = new google.maps.InfoWindow();

    map = new google.maps.Map(
        document.getElementById('map'), { center: london, zoom: 15 });

    var geocoder = new google.maps.Geocoder();

    geocodeAddress(geocoder, map);
}

function geocodeAddress(geocoder, resultsMap) {
    var address = document.getElementById("location").innerHTML;
    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status === 'OK') {
            resultsMap.setCenter(results[0].geometry.location);
            resultsMap.setZoom(16);
            var marker = new google.maps.Marker({
                map: resultsMap,
                position: results[0].geometry.location
            });

            var request = {
                location: results[0].geometry.location,
                radius: '400',
                type: ['cafe']
            };
            
            service = new google.maps.places.PlacesService(map);
            service.nearbySearch(request, callback);
        }
    });
}

function callback(results, status) {
    if (status == google.maps.places.PlacesServiceStatus.OK) {
        for (var i = 0; i < results.length; i++) {
            var place = results[i];
            createMarker(results[i]);
        }
    }
}

function createMarker(place) {
    var marker = new google.maps.Marker({
        map: map,
        position: place.geometry.location,
        icon: 'http://maps.google.com/mapfiles/ms/icons/green-dot.png'
    });

    google.maps.event.addListener(marker, 'click', function () {
        infowindow.setContent(place.name);
        infowindow.open(map, this);
    })
}

function showMap2() {
    var map2 = new google.maps.Map(document.getElementById('map'), {
        zoom: 8,
        center: { lat: 51.5074, lng: 0.1278 }
    });
    var geocoder = new google.maps.Geocoder();

    geocodeAddress2(geocoder, map2);
}

function geocodeAddress2(geocoder, resultsMap) {
    var address = document.getElementById('Venue').value;
    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status === 'OK') {
            resultsMap.setCenter(results[0].geometry.location);
            resultsMap.setZoom(12);
            var marker = new google.maps.Marker({
                map: resultsMap,
                position: results[0].geometry.location
            });
        }
    });
}