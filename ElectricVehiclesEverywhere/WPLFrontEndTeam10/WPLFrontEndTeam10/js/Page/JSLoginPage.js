const loginform = document.getElementById("sign-in-html");
const signupform = document.getElementById("sign-up-html");
let klant;

window.onload = function(){
    loginCheck();
    toonAantalProducten();
}

window.addEventListener("load",
    function (){
        let email = localStorage.getItem('localemail')??sessionStorage.getItem('sessionemail');
        let token = localStorage.getItem('localtoken')??sessionStorage.getItem('sessiontoken');
        if (isLoggedin(email,token)===true){
            window.location.href="ProfilepageForRent.html";
        }
    });
loginform.addEventListener("submit", (event) => {
    event.preventDefault();
    login();
})
function loadPage(){
    window.location.href="HomepageForRent.html";
}
signupform.addEventListener("submit", (event) => {
    event.preventDefault();
    document.body.scrollTop = 100;
    document.documentElement.scrollTop = 100;
    saveRegistrionInfo();
})

signupform[`email-sign-up`].addEventListener("input", () => {
    pEl.style.display = "none";
});

function login() {
    let email = document.getElementById("email-sign-in").value;
    let wachtwoord = document.getElementById("wachtwoord-sign-in").value;
    loginRequest(email, wachtwoord);
}

function saveRegistrionInfo() {
    klant = {
        voornaam: document.getElementById('voornaam').value,
        achternaam: document.getElementById('naam').value,
        email: document.getElementById("email-sign-up").value,
        telefoonnummer: document.getElementById('telefoonnummer').value,
        geboortedatum: document.getElementById('geboortedatum').value,
        rijbewijsB: !!document.getElementById('rijbewijsA').checked,
        rijbewijsA: !!document.getElementById('rijbewijsB').checked,
        adres: document.getElementById('adres').value,
        huisnummer: parseInt(document.getElementById('huisnummer').value) ,
        bus: document.getElementById('bus').value,
        woonplaats: document.getElementById('gemeente').value,
        postcode: parseInt(document.getElementById('postcode').value),
        wachtwoord: document.getElementById('wachtwoord-sign-up').value,
        iban: document.getElementById('iban').value,
        rekeninghouder: document.getElementById('rekeninghouder').value,
        vervaldatum: document.getElementById('vervaldatum').value
    };

    Object.keys(klant).forEach(key => {
        if (klant[key] === "")
            klant[key] = null;
    });

    if (validateEmail(klant.email)){
        registrationRequest(klant);
    }
}

function openModal() {
    createModalOk("Geef een geldige email en/of wachtwoord.");
}

function validateEmail(email){
    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email))
    {
        return (true);
    }
    createModalOk("Je hebt een ongeldig e-mail adres ingevoerd");
    return (false);
}