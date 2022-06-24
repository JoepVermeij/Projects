/*moet localhost:44371/api/ zijn */
const apiUrl = 'https://localhost:44371/api/';

/*
window.onload = function (){
    fetchJSON();
    postRequest();

    //Ben Sleurs Tests
    insertKlant();
    klantTest();
}

 */

/*
Fetch JSON
(1) The global fetch() method starts the process of fetching a resource from the network, returning a promise which
    is fulfilled once the response is available.
    docs: https://developer.mozilla.org/en-US/docs/Web/API/fetch
(2) Indien we meerdere asynchrone operaties na elkaar willen uitvoeren gaan we promises moeten chainen.
    Dit doen we door de .then()'s achter elkaar te plaatsen (chainen).
(3) Eens de promise die we terugkregen van de fetch() methode resolved (async operatie is voltooid) gaan we het resultaat
    daarvan doorgeven naar de validateResponse() functie.
(4) Indien de response 'ok' was geven we het resultaat van de fetch() functie door naar de readResponseAsJSON() functie.
    Indien de response niet 'ok' was zouden we in de catch() terechtkomen omdat we een error throwen in de validateResponse() functie.
(5) De JSON data is in de vorige functie omgezet naar een Javascript object. Nu kan je er dus mee gaan doen wat je wilt.
    In het voorbeeld hier wordt het object simpelweg in de console getoond.
(6) Indien in één van de vorige promise chains een error wordt gegooid (thrown) komt die error terecht in de catch block.
*/
function GetKlanten(){
    fetch(apiUrl+`klanten`).then(validateResponse) // (3)
        .then(readResponseAsJSON) // (4)
        .then(logResult) // (5)
        .catch(logError);
}
async function GetKlanten2(){
    let response = await fetch(apiUrl+'klanten');
    let json = response.json();
    return json;
}
function fetchJSON() {
    fetch(apiUrl + 'students') // (1) (2)
        .then(validateResponse) // (3)
        .then(readResponseAsJSON) // (4)
        .then(logResult) // (5)
        .catch(logError); // (6)
}

// Helper functions
/*
(1) Controleer of de response positief is. docs: https://developer.mozilla.org/en-US/docs/Web/API/Response/ok
(2) Als de response niet ok is gooien we een error message.
    docs: https://developer.mozilla.org/en-US/docs/Web/API/Response/statusText
*/
function validateResponse(response) {
    console.log(response);
    if (!response.ok) { // (1)
        throw Error(response.statusText); // (2)
    }
    return response;
}

/*
(1) The json() method of the Response interface takes a Response stream and reads it to completion.
    ! It returns a promise ! which resolves with the result of parsing the body text as JSON.
    Note that despite the method being named json(), the result is not JSON but is instead the result of taking JSON
    as input and parsing it to produce a JavaScript object.
    docs: https://developer.mozilla.org/en-US/docs/Web/API/Response/json
*/
function readResponseAsJSON(response) {
    return response.json(); // (1)
}

function logResult(result) {
    console.log(result);
}

function logError(error) {
    console.log('Looks like there was a problem:', error);
}

/* POST JSON
NOTE: Never send unencrypted user credentials in production!
Extra info: https://developer.mozilla.org/en-US/docs/Web/API/fetch
(1) Request header info: https://developer.mozilla.org/en-US/docs/Glossary/Request_header
*/

async function myPostRequest(endPoint, body) {
    const messageHeaders = new Headers({
        'Content-Type': 'application/json'
    })
    const response = await fetch(apiUrl + endPoint, {
        method: 'POST',
        body: JSON.stringify(body),
        headers: messageHeaders
    })
    if (!response.ok) {
        throw response;
    }
    const content = await response.json();

    return content;
}

function getBestellingenTest(){
    let temp = localStorage.getItem("localemail");
    let temptoken = localStorage.getItem("localtoken");
    let webtoken = {
        email: temp,
        token: temptoken
    }
    console.log(webtoken);
    webtoken = JSON.stringify(webtoken);
    console.log(webtoken);
    const messageHeaders = new Headers({
        'Content-Type': 'application/json'
    })
    return fetch(apiUrl + `Bestellingen/GetBestellingenFromWebToken`, {
        method: 'POST',
        body: webtoken,
        headers: messageHeaders
    })
        .then(validateResponse)
        .then(readResponseAsJSON)
        .catch(logError);


}

function insertKlant(){
    const messageHeaders = new Headers({ // (1)
        'Content-Type': 'application/json'})
    let date= new Date();
    console.log(date);
    let newKlant ={achternaam:'Test2', username:'Test.Test', wachtwoord: 'wachtwoord', postcode: 3910,email: 'naam@mail.com',
        adres: 'straatnaam', huisnummer:10, bus:'0', telefoonnummer:'0477/77/77/77',geboortedatum:date}
    fetch(apiUrl+'klanten/add',{
        method:'POST',
        body: JSON.stringify(newKlant),
        headers: messageHeaders
    })
        .then(validateResponse)
        .then(readResponseAsText)
        .then(logResult)
        .catch(logError);
}
async function deleteKlant(){
    const messageHeaders = new Headers({ // (1)
        'Content-Type': 'application/json'})
    let date= new Date();
    console.log(date);
    let response = await GetKlanten2();
    let klantID = response[0].KlantID;
    let deleteKlant ={KlantId: klantID}
    fetch(apiUrl+'klanten/delete',{
        method:'POST',
        body: JSON.stringify(deleteKlant),
        headers: messageHeaders
    })
        .then(validateResponse)
        .then(readResponseAsText)
        .then(logResult)
        .catch(logError);
}
function postRequest() {
    const messageHeaders = new Headers({ // (1)
        'Content-Type': 'application/json'
    })
    let newPerson = {studentId: 2, lastName: 'Beuls', firstName: 'Jasper'}
    fetch(apiUrl + 'students', {
        method: 'POST',
        body: JSON.stringify(newPerson),
        headers: messageHeaders
    })
        .then(validateResponse)
        .then(readResponseAsText)
        .then(logResult)
        .catch(logError);
}

/*
(1) The text() method of the Response interface takes a Response stream and reads it to completion.
! It returns a promise ! that resolves with a String.
 docs: https://developer.mozilla.org/en-US/docs/Web/API/Response/text
 */
function Login(){
    const messageHeaders = new Headers({ // (1)
        'Content-Type': 'application/json'
    })
    let login ={email: 'naam.com', wachtwoord: 'wachtwoord'}
    fetch(apiUrl + 'login', {
        method: 'POST',
        body: JSON.stringify(login),
        headers: messageHeaders
    })
        .then(validateResponse)
        .then(readResponseAsText)
        .then(logResult)
        .catch(logError);
}
function readResponseAsText(response) {
    return response.text(); // (1)
}

