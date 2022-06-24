//region Variabelen
// *********************** DECLAREREN VARIABELEN ***************** //
let userInfo;
let klantenArray;
let adminKlant = {
    klantId: null,
    rekeningHouder: null,
    vervalDatum: null,
    voornaam: null,
    achternaam: null,
    telefoonnummer: null,
    geboortedatum: null,
    rijbewijsB: null,
    rijbewijsA: null,
    adres: null,
    huisnummer: null,
    bus: null,
    salt: null,
    postcode: null,
    woonPlaats: null,
    wachtwoord: null,
    iban: null,
    email: null,
    vervalDatum: null
};
// opslaan van alle textboxen in een array, gebruikt voor te hiden en laden
const profielTextBoxes = document.querySelectorAll(".profiel-text-box");
// opslaan van alle inputboxen in een array, gebruikt voor te hiden en laden
const profielInputBoxes = document.querySelectorAll(".profiel-input-box");
//endregion
//region PAGINA LADEN
// *********************** PAGINA LADEN ************************** //
window.onload = async function(){
    let result = await getprofielinfo();
    adminChecker(result);
    toonAantalProducten();
}

async function  getprofielinfo(){
    let email = localStorage.getItem('localemail')??sessionStorage.getItem('sessionemail');
    let token = localStorage.getItem('localtoken')??sessionStorage.getItem('sessiontoken');

    if (await isLoggedin(email,token)===true){
        return getprofielinfofetch(email,token);

    }
    else{
        window.location.href="../html/LoginpageForRent.html";
    }

}
function getprofielinfofetch(email,token) {
    const messageHeaders = new Headers({ // (1)
        'Content-Type': 'application/json'
    })
    let loginToken = {
        email: email,
        token: token
    };
    return fetch(apiUrl + 'klanten/getprofielinfo', {
        method: 'POST',
        body: JSON.stringify(loginToken),
        headers: messageHeaders
    })
        .then(response => response.text())
        .then(data =>
        {
            userInfo = JSON.parse(data);
            GegevensOpladen(userInfo);
            return userInfo;
        })
        .catch(logError)
}
//endregion
//region GEGEVENS LADEN
// *********************** GEGEVENS LADEN ************************ //
function GegevensOpladen(klant){
    // opladen in de text boxen
    document.getElementById("profiel-iban").innerHTML = klant.iban;
    document.getElementById("profiel-bankvervaldatum").innerHTML = klant.vervalDatum === null||""? "": klant.vervalDatum.slice(0,7);
    document.getElementById("profiel-bankrekeninghouder").innerHTML = klant.rekeningHouder;
    document.getElementById("profiel-voornaam").innerHTML = klant.voornaam;
    document.getElementById("profiel-klantnummer").innerHTML = klant.klantId;
    document.getElementById("profiel-achternaam").innerHTML = klant.achternaam;
    document.getElementById("profiel-profielemail").innerHTML = klant.email;
    document.getElementById("profiel-telefoon").innerHTML = klant.telefoonnummer;
    if(klant.rijbewijsB === true && klant.rijbewijsA === true){
        document.getElementById("profiel-rijbewijs").innerHTML = "A en B";
    }
    else if (klant.rijbewijsB === true){
        document.getElementById("profiel-rijbewijs").innerHTML = "B";
    }
    else if (klant.rijbewijsA === true){
        document.getElementById("profiel-rijbewijs").innerHTML = "A";
    }
    else{
        document.getElementById("profiel-rijbewijs").innerHTML = "geen";
    }
    document.getElementById("profiel-geboortedatum").innerHTML =klant.geboortedatum ===null||"" ? "": klant.geboortedatum.slice(0,10);
    document.getElementById("profiel-woonplaats").innerHTML = klant.woonPlaats;
    document.getElementById("profiel-adres").innerHTML = klant.adres;
    document.getElementById("profiel-huisnummer").innerHTML = klant.huisnummer;
    document.getElementById("profiel-bus").innerHTML = klant.bus;
    document.getElementById("profiel-postcode").innerHTML = klant.postcode;
    document.getElementById("profiel-username").innerHTML = klant.email;
    document.getElementById("profiel-wachtwoord").innerHTML = "********";

    // opladen in de input boxen
    document.getElementById("profiel-iban-input").value = klant.iban;
    document.getElementById("profiel-bankvervaldatum-input").value = klant.geboortedatum ===null||"" ? "": klant.vervalDatum.slice(0,10);
    document.getElementById("profiel-bankrekeninghouder-input").value = klant.rekeningHouder;
    document.getElementById("profiel-voornaam-input").value = klant.voornaam;
    document.getElementById("profiel-achternaam-input").value = klant.achternaam;
    document.getElementById("profiel-telefoon-input").value = klant.telefoonnummer;
    if(klant.rijbewijsB === true){
        document.getElementById("profiel-rijbewijsB-input").checked = true;
    }
    if(klant.rijbewijsA === true){
        document.getElementById("profiel-rijbewijsA-input").checked = true;
    }
    document.getElementById("profiel-geboortedatum-input").value =klant.geboortedatum ===null||"" ? "": klant.geboortedatum.slice(0,10);
    document.getElementById("profiel-woonplaats-input").value = klant.woonPlaats;
    document.getElementById("profiel-adres-input").value = klant.adres;
    document.getElementById("profiel-huisnummer-input").value = klant.huisnummer;
    document.getElementById("profiel-bus-input").value = (klant.bus === null) ? "" : klant.bus;
    document.getElementById("profiel-postcode-input").value = klant.postcode;
    document.getElementById("profiel-wachtwoord-input").innerHTML = "********";
}
//endregion
//region KLANT OBJECT UPDATEN
// *********************** KLANT OBJECT UPDATEN ****************** //
function KlantUpdaten(klant){
    console.log(klant);
    klant.voornaam= document.getElementById("profiel-voornaam-input").value;
    klant.achternaam= document.getElementById("profiel-achternaam-input").value;
    klant.telefoonnummer= document.getElementById("profiel-telefoon-input").value;
    klant.geboortedatum= document.getElementById("profiel-geboortedatum-input").value === "" ? null : `${document.getElementById("profiel-geboortedatum-input").value}T00:00:00`;
    klant.rijbewijsB= document.getElementById("profiel-rijbewijsB-input").checked;
    klant.rijbewijsA= document.getElementById("profiel-rijbewijsA-input").checked;
    klant.adres= document.getElementById("profiel-adres-input").value;
    klant.huisnummer= parseInt(document.getElementById("profiel-huisnummer-input").value);
    klant.bus= document.getElementById("profiel-bus-input").value;
    klant.salt= null;
    klant.postcode= parseInt(document.getElementById("profiel-postcode-input").value);
    klant.woonPlaats= document.getElementById("profiel-woonplaats-input").value;
    klant.wachtwoord= null;
    klant.iban= document.getElementById("profiel-iban-input").value;
    klant.rekeningHouder= document.getElementById("profiel-bankrekeninghouder-input").value;
    klant.vervalDatum= document.getElementById("profiel-bankvervaldatum-input").value === ""? null : `${document.getElementById("profiel-bankvervaldatum-input").value}T00:00:00`;
    Object.keys(klant).forEach(key => {
        if (klant[key] === "")
            klant[key] = null;
    });
    console.log(JSON.stringify(klant));
    return fetch(apiUrl+'klanten/updateprofile',{
        method:'POST',
        body: JSON.stringify(klant),
        headers: messageHeaders
    })
        .then(response => console.log(response.text()))
        .catch(logError);

}
//endregion
//region PROFIEL INFO WIJZIGEN
// *********************** PROFIEL INFO WIJZIGEN ***************** //
// het laden van alle inputboxen en hide alle textboxen
// maakt het mogelijk om je gegevens aan te passen
function GegevensWijzigen(){
    for (let i = 0; i < profielInputBoxes.length; i++) {
        profielInputBoxes[i].style.display = "block";
    }
    for (let i = 0; i < profielTextBoxes.length; i++) {
        profielTextBoxes[i].style.display = "none";
    }
    // terugopladen klantnummer want die werdt op display none gezet in de for loop
    document.getElementById("profiel-klantnummer").style.display = "block";
    document.getElementById("profiel-username").style.display = "block";
    document.getElementById("profiel-profielemail").style.display = "block";
    // width op de hele lengte zetten van klantnummer
    document.getElementById("profiel-klantnummer").style.width = "calc(100% - 140px)";
    document.getElementById("profiel-username").style.width = "calc(100% - 140px)";
    document.getElementById("profiel-profielemail").style.width = "calc(100% - 140px)";
    // wijziging knop hiden en opslaan knop laten zien
    document.getElementById("persoonlijkegegevens-wijzigen").style.display = "none";
    document.getElementById("persoonlijkegegevens-opslaan").style.display = "block";
    // rijbewijs boxen op flex zetten van none
    document.getElementById("profiel-input-box-rijbewijs").style.display = "flex";
    if (adminKlant.klantId != null){
        GegevensOpladen(adminKlant);
        document.getElementById("profiel-wachtwoord-input").style.display ="none";
        document.getElementById("profiel-wachtwoord").style.display ="none";

    }
    else{
        GegevensOpladen(userInfo);
    }
}
//endregion
// region PROFIEL INFO OPSLAAN
// *********************** PROFIEL INFO OPSLAAN ****************** //
// het hiden van alle inputboxen en laden alle textboxen
// keert je terug naar de uneditable profielpagina en slaat de aanpassingen op
async function GegevensOpslaan(){

    for (let i = 0; i < profielInputBoxes.length; i++) {
        profielInputBoxes[i].style.display = "none";
    }

    for (let i = 0; i < profielTextBoxes.length; i++) {
        profielTextBoxes[i].style.display = "block";
    }
    document.getElementById("persoonlijkegegevens-wijzigen").style.display = "block";
    document.getElementById("persoonlijkegegevens-opslaan").style.display = "none";
    document.getElementById("profiel-input-box-rijbewijs").style.display = "none";

    if (adminKlant.klantId != null){
        KlantUpdaten(adminKlant);
        getprofielinfo(adminKlant).then(aanpassenKlanten);
    }
    else{
        let promises = [];
        promises.push(KlantUpdaten(userInfo));
        // checked of het wachtwoord veranderd is
        // als het niet veranderd is worden de gegevens meteen gestuur naar de backend anders via bevestigingsmail
        if (document.getElementById("profiel-wachtwoord-input").value !== "********"){
            promises.push(sendWachtWoordMail());
        }
        await Promise.all(promises);
        getprofielinfo();
    }
}
// endregion
//region UITLOGGEN
// *********************** UITLOGGEN ***************************** //
function uitloggen(){
    localStorage.removeItem('localemail');
    localStorage.removeItem('localtoken');
    sessionStorage.removeItem('sessionemail');
    sessionStorage.removeItem('sessiontoken');
}
//endregion
//region ADMIN RECHTEN
// *********************** ADMIN RECHTEN ************************* //
/// admin check
function adminChecker(klant){
    if (klant.isAdmin){
        let aanpassenklantknop = document.createElement("button");
        aanpassenklantknop.id = "persoonlijkegegevens-aanpassenklanten";
        aanpassenklantknop.className = "verwijderen";
        aanpassenklantknop.innerHTML = "AANPASSEN KLANTEN";
        aanpassenklantknop.onclick = function(){aanpassenKlanten(); zoekenLaden();};
        document.getElementById("containerlinks").appendChild(aanpassenklantknop);
    }
}
function aanpassenKlanten(){
    let localEmail = localStorage.getItem('localemail');
    let localtoken = localStorage.getItem('localtoken');
    let sessionEmail = sessionStorage.getItem('sessionemail');
    let sessiontoken = sessionStorage.getItem('sessiontoken');
    if (localEmail != null && localtoken != null)
    {
        return adminCheck(localEmail, localtoken);
    }
    else if(sessionEmail != null && sessiontoken != null)
    {
        return adminCheck(sessionEmail, sessiontoken)
    }
    function adminCheck(email, token){
        const adminToken ={
            email,
            token
        }
        fetch(apiUrl+'admin/check',{
            method:'POST',
            body: JSON.stringify(adminToken),
            headers: messageHeaders
        })
            .then(response => response.json())
            .then(data => {klantenArray = data; console.log(klantenArray)})
            .catch(error => console.log(error));
    }
}
function zoekenLaden(){
    let searchbar = document.createElement("input");
    let searchbutton = document.createElement("button");
    searchbar.id = "persoonlijkegegevens-zoekklant";
    searchbar.type = "text";
    searchbar.className = "input-container-links profiel-input-box:focus-visible";

    searchbutton.id = "persoonlijkegegevens-zoeken";
    searchbutton.className = "verwijderen";
    searchbutton.innerText = "ZOEKEN";
    searchbutton.style.padding = "1rem 5.5rem";
    document.getElementById("containerlinks").append(searchbutton);
    document.getElementById("containerlinks").append(searchbar);
    document.getElementById("persoonlijkegegevens-zoeken").addEventListener("click",klantzoeken);
    document.getElementById("persoonlijkegegevens-aanpassenklanten").style.display = "none";
}
function klantzoeken(){
    console.log(klantenArray)
    let zoekCriteria = document.getElementById("persoonlijkegegevens-zoekklant").value;
    if(!klantenArray.some(nieuweKlant => nieuweKlant.KlantID.toString() === zoekCriteria)){
        createModalOk("Gelieve een geldig klantnummer in te geven.");
    }
    let nieuweKlant = klantenArray.filter(obj => {
        return obj.KlantID.toString() === zoekCriteria
    });
    console.log(nieuweKlant[0].KlantID);
    adminKlant.klantId = nieuweKlant[0].KlantID;
    adminKlant.rekeningHouder= nieuweKlant[0].rekeningHouder;
    adminKlant.vervalDatum= nieuweKlant[0].vervalDatum;
    adminKlant.voornaam= nieuweKlant[0].voornaam;
    adminKlant.achternaam= nieuweKlant[0].achternaam;
    adminKlant.telefoonnummer= nieuweKlant[0].telefoonnummer;
    adminKlant.geboortedatum= nieuweKlant[0].geboortedatum;
    adminKlant.rijbewijsB= nieuweKlant[0].rijbewijsB;
    adminKlant.rijbewijsA= nieuweKlant[0].rijbewijsA;
    adminKlant.adres= nieuweKlant[0].adres;
    adminKlant.huisnummer= nieuweKlant[0].huisnummer;
    adminKlant.bus= nieuweKlant[0].bus;
    adminKlant.salt= null;
    adminKlant.postcode= nieuweKlant[0].postcode;
    adminKlant.woonPlaats= nieuweKlant[0].woonplaats;
    adminKlant.wachtwoord= null;
    adminKlant.iban= nieuweKlant[0].iban;
    adminKlant.rekeningHouder= nieuweKlant[0].rekeninghouder;
    adminKlant.vervalDatum= nieuweKlant[0].vervaldatum;
    adminKlant.email= nieuweKlant[0].email;
    adminKlant.isAdmin= nieuweKlant[0].isAdmin;
    GegevensOpladen(adminKlant);
}

//endregion
//region ACCOUNT VERWIJDEREN
// *********************** ACCOUNT VERWIJDEREN ******************* //
async function ProfielVerwijderen() {
    let webToken = GetWebToken();
    let response = await myPostRequestJSON("mail/deleteprofiel", webToken);
    if (response.ok) {
        createModalOk(await response.json());
    } else createModalOk(await response.json());

}
function openVerwijderModal() {
    createModalYesNo("Wilt u uw account verwijderen?", ProfielVerwijderen);
}

function openVerwijderBevestigingsModal() {
    createModalOk("Bevestiginsmail is verzonden!");
}

//endregion
//region WACHTWOORD VERANDEREN
// *********************** WACHTWOORD VERANDEREN ******************* //
function sendWachtWoordMail(){
    let webToken = GetWebToken();
    let wachtwoord = document.getElementById("profiel-wachtwoord-input").value;
    if (webToken!=null&&wachtwoord!=null){
        let passwordRequest = {
            email: webToken.email,
            wachtwoord: wachtwoord
        };

        return fetch(apiUrl+'Mail/updatewachtwoord',{
            method:'POST',
            body: JSON.stringify(passwordRequest),
            headers: messageHeaders
        })
            .then(response => response.text())
            .then(openWachtwoordModal)
            .catch(logError);
    }
}
function openWachtwoordModal() {
    createModalOk("Er is een bevestigingsmail verstuurd")
}
//endregion
