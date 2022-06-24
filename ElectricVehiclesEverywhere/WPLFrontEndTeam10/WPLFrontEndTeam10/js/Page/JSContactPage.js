window.onload = function(){
    loginCheck();
    toonAantalProducten();
    loadDeferredIframe();
}

function loadDeferredIframe() {
    // this function will load the Google map into the iframe
    let iframe = document.getElementById('gmap_canvas');
    iframe.src = "https://maps.google.com/maps?q=corda%20campus&t=&z=9&ie=UTF8&iwloc=&output=embed";
}
