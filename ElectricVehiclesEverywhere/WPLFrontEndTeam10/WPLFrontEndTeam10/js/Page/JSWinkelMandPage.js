let winkelmandje = document.getElementById("winkelMandje");
let productenArray = [];
let occupiedDatesWinkelmand =[];
let winkelmandProducts =[];
let totaalBedrag = 0;
let winkelmandOccupiedDates = [];
let winkelmandByNaam = [];
let email = localStorage.getItem('localemail');
let winkelmandLijst = JSON.parse(localStorage.getItem('winkelmandLijst'));


window.onload = async function(){
    await loginCheck();

    getLocalCart();
    winkelmandByNaam = [...new Map(winkelmandProducts.map((item) => [item["naam"], item])).values()];

    winkelmandOccupiedDates = await fillWinkelmandOccupiedDates();
    for (const elem of winkelmandOccupiedDates) {
        //map gebruiken voor mooiere code
        for (let i=0;i<elem.occupiedDates.length;i++){
            let string = elem.occupiedDates[i];
            let year = string.substr(0,4);
            let month = string.substr(5,2);
            let day =string.substr(8,2);
            elem.occupiedDates[i] = new Date(year,month-1,day).toDateString();
        }
    }
    for (let i=0;i<winkelmandProducts.length;i++) {
        createWinkelmandProductCard(winkelmandProducts[i],i);
    }
    toonAantalProducten();
}

//winkelmandProducts wordt de volledige winkelmand voor de gebruiker
function getLocalCart(){
    let newWinkelmandProducts = winkelmandLijst[`null`];
    if (newWinkelmandProducts == null)newWinkelmandProducts=[];

    let emailWinkelmandProducts = winkelmandLijst[`${email}`];
    if (emailWinkelmandProducts == null)emailWinkelmandProducts=[];

    let mergedWinkelmandProducts = [...newWinkelmandProducts, ...emailWinkelmandProducts];
    winkelmandProducts = [...new Set(mergedWinkelmandProducts.map(a => JSON.stringify(a)))].map(a => JSON.parse(a));

    if (email !== "null") delete  winkelmandLijst[`null`];

    setLocalCart();
}


function setLocalCart(){
    winkelmandLijst[`${email}`] = winkelmandProducts;
    if (winkelmandProducts.length===0) delete winkelmandLijst[`${email}`];
    localStorage.setItem('winkelmandLijst',JSON.stringify(winkelmandLijst));
}

//returns array met alle voertuigen in winkelmand en hun corresponderende bezette datums
async function fillWinkelmandOccupiedDates(){
    const promises = winkelmandByNaam
        .map(vehicle => getOccupiedDates(vehicle.naam).then(dates => ({
            naam: vehicle.naam,
            occupiedDates: dates
        })));

    const results = await Promise.all(promises);
    return results
}

function createWinkelmandProductCard(product,index){
    let productencardContainer = document.createElement("div");
    productencardContainer.classList.add('productcardcontainer') ;

    let productenCard = document.createElement("div");
    productenCard.classList.add("productcards") ;

    let productImageDiv = document.createElement("div");
    productImageDiv.classList.add("productimage") ;
    let img = document.createElement("img");
    img.src = product.imageUrl;
    productImageDiv.appendChild(img);

    let bestelInfo = document.createElement("div");
    bestelInfo.classList.add("bestellinginfo") ;

    let productnaam = document.createElement("div");
    productnaam.classList.add("productnaam") ;

    let p1 = document.createElement("p");
    p1.innerText=product.naam;
    p1.onclick = function () {
        location.href = `IndividueleProductPage.html?vehicle=${product.naam}`;
    };
    productnaam.appendChild(p1);


    let infoContainer = document.createElement("div");
    infoContainer.classList.add("infocontainer") ;

    let productDatum = document.createElement("div");
    productDatum.classList.add("productdatum") ;
    productDatum.classList.add("datum");


    let startDatum = document.createElement("div");
    startDatum.classList.add("startdatum") ;

    let startLabel = document.createElement("label");
    startLabel.htmlFor="start"+index;
    startLabel.innerText=`Startdatum:`;

    let startButton = document.createElement("button");
    startButton.value = `${index}`;
    startButton.classList.add("start");
    startButton.id="start"+index;
    startButton.name="trip-start";
    startButton.innerText=product.StartDatum;
    let occDates = winkelmandOccupiedDates.filter(obj => {
        return obj.naam === product.naam;
    });
    startButton.onclick = function (){
        ShowCalender(this,occDates[0].occupiedDates);
    }
    startDatum.appendChild(startLabel);
    startDatum.appendChild(startButton);

    let eindDatum = document.createElement("div");
    eindDatum.classList.add("einddatum") ;

    let eindLabel = document.createElement("label");
    eindLabel.htmlFor="eind"+index;
    eindLabel.innerText=`Einddatum:`;

    let eindButton = document.createElement("button");
    startButton.value=`${index}`;
    eindButton.classList.add("eind");
    eindButton.id="eind"+index;
    eindButton.name="trip-start";
    eindButton.innerText=product.EindDatum;

    eindButton.onclick = function (){
        ShowCalender(this,occDates[0].occupiedDates);
    }
    eindDatum.appendChild(eindLabel);
    eindDatum.appendChild(eindButton);

    productDatum.appendChild(startDatum);
    productDatum.appendChild(eindDatum);

    let prijs = document.createElement("div");
    prijs.classList.add("prijs") ;

    let p2 = document.createElement("p");
    p2.innerText = 'Prijs: '

    let p3 = document.createElement("p");
    p3.innerText = `${calculateProductPrice(product.prijs,product.StartDatum,product.EindDatum)} €`;
    totaalBedrag += calculateProductPrice(product.prijs,product.StartDatum,product.EindDatum);
    prijs.appendChild(p2);
    prijs.appendChild(p3);

    infoContainer.appendChild(productDatum);
    infoContainer.appendChild(prijs);

    bestelInfo.appendChild(productnaam);
    bestelInfo.appendChild(infoContainer);



    let productVerwijderen = document.createElement("div");
    productVerwijderen.classList.add('productverwijderen');

    let verwijderenButton = document.createElement("button");
    verwijderenButton.innerText="Verwijderen"
    verwijderenButton.onclick=function (){
        createModalYesNo("Wilt u de bestelling verwijderen?", function () {
            verwijder(index);
        })
    }
    productVerwijderen.appendChild(verwijderenButton);

    productenCard.appendChild(productImageDiv);
    productenCard.appendChild(createLine());
    productenCard.appendChild(bestelInfo);
    productenCard.appendChild(createLine());
    productenCard.appendChild(productVerwijderen);


    productencardContainer.appendChild(productenCard);
    document.getElementById("winkelMandje").appendChild(productencardContainer);



    document.getElementById("totaalRekeningBedrag").innerText = totaalBedrag + ' €';
}

//Berekent de totaalprijs van een winkelmand product op basis van ingegeven datums en prijs/dag
function berekenPrijs(elem){
    const index = elem.id.charAt(elem.id.length - 1);
    let date1Text = document.getElementById(`start${index}`).innerText;
    let date2Text = document.getElementById(`eind${index}`).innerText;
    let date1= convertDate(date1Text);
    let date2= convertDate(date2Text);
    const today = new Date();
    if (date1==""||date2==""){
        console.log("Een van de 2 dates is null")
        document.getElementById(`totaalPrijs${index}`).innerText=`0 €`;
    }
    else{
        winkelmandProducts[index].StartDatum=date1Text;
        winkelmandProducts[index].EindDatum=date2Text;
        setLocalCart();
        let elem = document.getElementById('navwinkelmandcontainer');
        elem.removeChild(elem.lastChild);
        rebuildWinkelmand();

    }

}

function calculateProductPrice(dagPrijs,startDatum,eindDatum){

    let start = convertDate(startDatum);
    let end = convertDate(eindDatum);
    let days = ((end-start)/oneDay)+1;
    return dagPrijs*days;
}

//Convert een DD/MM/YYYY string naar een date object
function convertDate(datum){
    let parts = datum.split("/");
    let day = parseInt(parts[0]);
    let month = parseInt(parts[1]);
    let year = parseInt(parts[2]);
    let date = new Date(year,month,day);
    return date;
}

//maakt een lijn html element die een gestylde lijn toont
function createLine(){
    let lijn =document.createElement("div");
    lijn.classList.add("lijn");
    return lijn;
}

//verwijdert een product uit je winkelmand en rebuild nadien de winkelmandcards
function verwijder(index){
    winkelmandProducts.splice(index, 1);
    setLocalCart();
    let elem = document.getElementById('navwinkelmandcontainer');
    elem.removeChild(elem.lastChild);
    rebuildWinkelmand();
}


function diferrenceInDays(inputStart,inputEind){
    const diffInTime = inputEind.valueAsDate.getTime() - inputStart.valueAsDate.getTime();
    const oneDay = 1000 * 60 * 60 * 24;
    const diffInDays = Math.round(diffInTime / oneDay);
    return diffInDays;


}
function continueShopping(){
    window.location.href = 'ProductcardForRent.html'
}
//herbouwd de winkelmand
function rebuildWinkelmand() {
    totaalBedrag = 0;
    winkelmandje.innerHTML = "";
    for (let i = 0; i < winkelmandProducts.length; i++) {
        createWinkelmandProductCard(winkelmandProducts[i], i);

    }
    document.getElementById("totaalRekeningBedrag").innerText = totaalBedrag + ' €';

    toonAantalProducten();
}

//Checks en processes die uitgevoerd worden als de gebruiker op 'bevesting bestelling' duwt
async function bevestigingClick(){
    if (winkelmandProducts.length===0){
        createModalOk("Je winkelmandje is leeg");
        return;
    }
    if (await loginCheck()===true){
        let email = localStorage.getItem('localemail')??sessionStorage.getItem('sessionemail');
        let token = localStorage.getItem('localtoken')??sessionStorage.getItem('sessiontoken');
        let webtoken = {
            email: email,
            token: token
        }

        let bankGegevens = await myPostRequest("Klanten/getBankGegevens",webtoken);
        if(Object.values(bankGegevens[0]).every(o => o != null)){
            tryOrder(webtoken);

        }
        else createBankModal();

    }
    else{
        showLoginModal();
    }

}
// zoveelste convert om timezones te negeren.
// returned een string om te posten naar backend
function convertDateNr99999(datum){
    let parts = datum.split("/");
    let day = parts[0];
    let month = parts[1];
    let year = parts[2];
    let dateString = `${year}-${month}-${day}T00:00:00`
    return dateString;
}

//alle checks zijn geldig, we proberen de bestelling uit te voeren als de producten nog geldig zijn
async function tryOrder(webtoken){
    let bestellingen = [];
    for (const winkelmandProduct of winkelmandProducts) {
        let bestelling = {
            bestelid:0,
            productid:winkelmandProduct.productId,
            klantid:0,
            startDatum: convertDateNr99999(winkelmandProduct.StartDatum),
            eindDatum: convertDateNr99999(winkelmandProduct.EindDatum)

        }
        bestellingen.push(bestelling);
    };

    let body = {
        webtoken: webtoken,
        bestellingen: bestellingen
    };
    console.log(JSON.stringify(body));
    let response = await myPostRequestJSON("bestellingen/OrderCheck",body);
    if (response.ok===true){
        removeYourWinkelmand();
        createModalOk(await response.text())
    }
    else if(response.status === 403){
        createModalOk(await response.text());
    }
    else if(response.status === 409){
        createModalOk(await response.text());
    }
    else{
        createModalOk(await response.text())
    }
}
function removeYourWinkelmand(){
    let yourEmail = localStorage.getItem('localemail')??sessionStorage.getItem('sessionemail');
    delete winkelmandLijst[yourEmail];
    getLocalCart();
    rebuildWinkelmand();
}

//Toont een modal die zegt dat de gebruiker ingelogd moet zijn, stuurt gebruiker naar de loginpagina
function showLoginModal(){
    createModalOk("Meld je aan voor je producten bestelt.", function (){window.location.href = 'LoginpageForRent.html'});
}

function createBankModal() {
    let bankModal = document.createElement('div');
    bankModal.id="bankModal"
    bankModal.classList.add("modal");

    let modalcontent = document.createElement("div");
    modalcontent.classList.add("modal-content");
    bankModal.appendChild(modalcontent);

    let p = document.createElement("p");
    p.classList.add("bankgegevens-invullen");
    p.id="bankmodalmessage"
    p.innerText = "Gelieve uw bankgegevens in te vullen om een bestelling te kunnen plaatsen";
    modalcontent.appendChild(p);

    let form = document.createElement("form");
    form.action = "#";
    modalcontent.appendChild(form);

    let gegevensContainer1 = document.createElement("div");
    gegevensContainer1.classList.add("gegevens");
    form.appendChild(gegevensContainer1);

    let labelRekeningHouder = document.createElement("label");
    labelRekeningHouder.for = "rekeninghouder";
    labelRekeningHouder.innerText = "Rekeninghouder:";
    gegevensContainer1.appendChild(labelRekeningHouder);
    let inputRekeningHouder = document.createElement("input");
    inputRekeningHouder.type = "text";
    inputRekeningHouder.id = "rekeninghouder";
    inputRekeningHouder.required = true;
    gegevensContainer1.appendChild(inputRekeningHouder);

    let gegevensContainer2 = document.createElement("div");
    gegevensContainer2.classList.add("gegevens");
    form.appendChild(gegevensContainer2);

    let labelIban = document.createElement("label");
    labelIban.for = "iban";
    labelIban.innerText = "Iban:";
    gegevensContainer2.appendChild(labelIban);
    let inputIban = document.createElement("input");
    inputIban.type = "text";
    inputIban.id = "iban";
    inputIban.required = true;
    gegevensContainer2.appendChild(inputIban);

    let gegevensContainer3 = document.createElement("div");
    gegevensContainer3.classList.add("gegevens");
    form.appendChild(gegevensContainer3);

    let labelVervaldatum = document.createElement("label");
    labelVervaldatum.for = "vervaldatum";
    labelVervaldatum.innerText = "Vervaldatum:";
    gegevensContainer3.appendChild(labelVervaldatum);
    let inputVervaldatum = document.createElement("input");
    inputVervaldatum.type = "date";
    inputVervaldatum.id = "vervaldatum";
    inputVervaldatum.required = true;
    gegevensContainer3.appendChild(inputVervaldatum);

    let checkContainer = document.createElement("div");
    checkContainer.classList.add("check");
    form.appendChild(checkContainer);

    let inputCheck = document.createElement("input");
    inputCheck.id = "gegevens-opslaan";
    inputCheck.type = "checkbox";
    inputCheck.checked = true;
    checkContainer.appendChild(inputCheck);
    let labelCheck = document.createElement("label");
    labelCheck.for = "gegevens-opslaan";
    labelCheck.innerText = "Gegegevens opslaan";
    checkContainer.appendChild(labelCheck);

    let verdergaanButton = document.createElement("button");
    verdergaanButton.type = "submit";
    verdergaanButton.onclick = GaVerder;
    verdergaanButton.innerText = "Verdergaan";
    form.appendChild(verdergaanButton);

    let cancelButton = document.createElement("button");
    cancelButton.onclick = closeBankModal;
    cancelButton.innerText = "Annuleren";
    form.appendChild(cancelButton);

    document.body.appendChild(bankModal);
}

function GaVerder() {
    let rekening = document.getElementById("rekeninghouder");
    let iban = document.getElementById("iban");
    let vervaldatum = document.getElementById("vervaldatum");
    let email = localStorage.getItem('localemail')??sessionStorage.getItem('sessionemail');
    let token = localStorage.getItem('localtoken')??sessionStorage.getItem('sessiontoken');
    let webtoken = {
        email: email,
        token: token
    }
    if (rekening.value==""||iban.value==""||vervaldatum.value==""){
        let errorstring = [];
        if(rekening.value==""){
            errorstring.push("rekening");
            rekening.style.border="#ff0000  2px solid";
        }
        else rekening.style.border="##182c4c 2px solid"
        if(iban.value==""){
            errorstring.push("iban");
            iban.style.border="#ff0000  2px solid";
        }
        else iban.style.border="##182c4c  2px solid"
        if(vervaldatum.value==""){
            errorstring.push("vervaldatum");
            vervaldatum.style.border="#ff0000  2px solid";
        }
        else vervaldatum.style.border="##182c4c  2px solid"
        let string = `Gelieve ${errorstring.join()} in te vullen om een bestelling te kunnen plaatsen`
        document.getElementById("bankmodalmessage").innerText=string;
    }
    else if(document.getElementById("gegevens-opslaan").checked){
        updateBankGegevens(rekening.value,iban.value,vervaldatum.value);
        tryOrder(webtoken)
    }
    // Gegevens worden niet opgeslaan door klant, hier zou betaling 1 maal moeten uitgevoerd worden
    else {
        closeBankModal();
        tryOrder(webtoken);
    }
}

async function updateBankGegevens(rekening,iban,vervaldatum){

    let email = localStorage.getItem('localemail')??sessionStorage.getItem('sessionemail');
    let token = localStorage.getItem('localtoken')??sessionStorage.getItem('sessiontoken');
    let webtoken = {
        email: email,
        token: token
    }
    let bankgegevens = {
        rekeningHouder:rekening,
        iban,iban,
        vervaldatum,vervaldatum
    }
    let body= {
        webtoken:webtoken,
        bankgegevens:bankgegevens
    }
    console.log(JSON.stringify(body));
    let response = await myPostRequest("klanten/updateBankGegevens",body);
    closeBankModal();

}
function closeBankModal() {
    document.body.removeChild(document.getElementById("bankModal"));
}




