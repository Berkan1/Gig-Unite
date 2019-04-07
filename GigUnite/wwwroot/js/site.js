// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {

    if (document.getElementById("age")) {
        calculateAge(document.getElementById("age").innerHTML);
    };

    $('#genres').multiselect({
        columns: 2,
        placeholder: 'Select genres'
    });
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
    document.getElementById("interested").src = "/images/orange_bell.png";
}

function maybeOut() {
    document.getElementById("interested").src = "/images/blank_bell.png";
}

function goingOver() {
    document.getElementById("going").src = "/images/orange_tick.png";
}

function goingOut() {
    document.getElementById("going").src = "/images/blank_tick.png";
}