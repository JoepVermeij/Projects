const apiUrl = 'https://localhost:44371/api/';
const messageHeaders = new Headers({ // (1)
    'Content-Type': 'application/json'
})

//algemene methode om eenpost request te doen naar een url/endpont met 'body' als body
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
async function myPostRequestJSON(endPoint, body) {
    const messageHeaders = new Headers({
        'Content-Type': 'application/json'
    })
    return await fetch(apiUrl + endPoint, {
        method: 'POST',
        body: JSON.stringify(body),
        headers: messageHeaders
    })
}

// Probeert je actief in te loggen
function loginRequest(email, wachtwoord) {
    const messageHeaders = new Headers({ // (1)
        'Content-Type': 'application/json'
    })
    let login = {email: email, wachtwoord: wachtwoord};
    fetch(apiUrl + 'login', {
        method: 'POST',
        body: JSON.stringify(login),
        headers: messageHeaders
    })
        .then(validateResponse)
        .then(readResponseAsText)
        .then(loginExecute)
        .catch(logError);
}

// houdt je ingelogd, returned true of false
async function isLoggedin(email, token) {
    let loginToken = {
        email: email,
        token: token
    };
    return myPostRequest("login/check",loginToken)
        .then(loginNavBar)
        .catch(function(error){
            if (error.status==401){
                localStorage.setItem('localemail',null);
                localStorage.setItem('localtoken',null);
                sessionStorage.setItem('sessionemail',null);
                sessionStorage.setItem('sessiontoken',null);
            }
            throw error;
        })
}



// checked of je ingelogd bent
function loginCheck() {
    let email = localStorage.getItem('localemail')??sessionStorage.getItem('sessionemail');
    let token = localStorage.getItem('localtoken')??sessionStorage.getItem('sessiontoken');
    if (email != "null" && token != "null") {
        return isLoggedin(email, token);
    }
    else return false;
}

function validateResponse(response) {
    console.log(response)
    if (!response.ok) {
        openModal();
        throw Error(response.statusText); // (2)
    }
    console.log(response.body)
    return response;
}

function readResponseAsText(response) {
    return response.text(); // (1)
}

function readResponseAsJSON(response) {
    return response.json(); // (1)
}

function readPostResponseAsJson(response) {
    return JSON.parse(response);
}

function logResult(result) {
    console.log(result);
}

function logError(error) {
    console.log('Looks like there was a problem:', error);
}

//functie om de webtoken van ingelogde gebruiker op tevragen
function GetWebToken() {
    if (localStorage.getItem('localemail') != null) {
        let webToken = {
            email: localStorage.getItem('localemail'),
            token: localStorage.getItem('localtoken')
        };
        return webToken;
    } else if (sessionStorage.GetItem('session') != null) {
        let webToken = {
            email: sessionStorage.getItem('localemail'),
            token: sessionStorage.getItem('localtoken')
        };
        return webToken;
    } else {
        console.log("geen webtoken");
        return null;
    }
}

//User is ingelogd, local/session storage wordt geüpdate en gebruiker wordt naar homepage gestuurd.
function loginExecute(result) {
    let resultobj = JSON.parse(result);
    if (document.getElementById("staySignuped").checked === true) {
        localStorage.setItem('localemail', resultobj.email);
        localStorage.setItem('localtoken', resultobj.token);
    } else {
        sessionStorage.setItem('sessionemail', resultobj.email);
        sessionStorage.setItem('sessiontoken', resultobj.token);
    }
    window.location.href = "HomepageForRent.html";
}

//User is ingelogd, de navbar past zich aan
function loginNavBar(result) {
    if (result === true) {
        document.getElementById("inlogbutton").innerHTML = "PROFIEL";
        document.getElementById("inlogbutton").href = "../html/ProfilepageForRent.html";
        document.getElementById("winkelmandhamburgermenu").classList.add("block-element");
        document.getElementById("navwinkelmandcontainer")?.classList.remove("hide-element");
        document.getElementById("navwinkelmandcontainer")?.classList.add("block-element");
    }
    return result;
}

// Poging om een klant aan te maken
function registrationRequest(klant) {
    console.log(klant);
    fetch(apiUrl + 'klanten/add', {
        method: 'POST', // or 'PUT'
        body: JSON.stringify(klant),
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
        }
    }).then(validateResponse)
        .then(readResponseAsText)
        .then(logResult)
        .then(completeRegistration)
        .catch(logError);
}


function completeRegistration(result) {
    //TODO: LOGINREQUEST MOET OP BETERE PLAATS GAAN STAAN!!!!!!
    loginRequest(klant.email, klant.wachtwoord);
    let resultobj = JSON.parse(result);
    console.log(resultobj);

    if (resultobj.succeeded === true) {
        location.reload();
    } else {
        if (resultobj[`error`].includes("UNIQUE KEY")) {
            //window.alert("e-mail adres al in gebruik.")
            pEl.style.display = "flex";
            insertAfter(inputEl, pEl);
        }
    }


}

var pEl = document.createElement("p");
pEl.innerHTML = "e-mail adres al in gebruik.";
pEl.style.color = "red";
pEl.style.margin = "0 auto 0.5rem auto";
var inputEl = document.getElementById("email-sign-up");

function insertAfter(referenceNode, newNode) {
    referenceNode.parentNode.insertBefore(newNode, referenceNode.nextSibling);
}

