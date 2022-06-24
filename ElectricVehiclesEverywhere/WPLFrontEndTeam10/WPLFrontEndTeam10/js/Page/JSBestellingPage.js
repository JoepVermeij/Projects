const oneDay = 1000 * 60 * 60 * 24;

window.onload = async function(){
    await getprofielinfo();
    let webtoken ={
        email:localStorage.getItem('localemail')??sessionStorage.getItem('sessionemail'),
        token:localStorage.getItem('localtoken')??sessionStorage.getItem('sessiontoken')
    }
    let temp = await myPostRequest("Bestellingen/GetBestellingenFromWebToken",webtoken);
    for (let i=0;i<temp.length;i++){
        temp[i].startdatum = convertToJSDate(temp[i].startdatum);
        temp[i].einddatum = convertToJSDate(temp[i].einddatum);
    }
    createOrder(temp);
    toonAantalProducten();
}

async function  getprofielinfo(){
    let email = localStorage.getItem('localemail')??sessionStorage.getItem('sessionemail');
    let token = localStorage.getItem('localtoken')??sessionStorage.getItem('sessiontoken');

    if (await isLoggedin(email,token)===true){
        return getprofielinfofetch(email,token);

    }
    else{
        window.location.href="../../html/LoginpageForRent.html";
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

let bestelling = {
    merk: 'Tesla',
    model: 'Model 3',
    startDatum: '13/05/2022',
    eindDatum: '15/05/2022',
    prijs: 150
}

let bestellingArray = [];
bestellingArray.push(bestelling);

function createOrder(bestellingArray) {
    const table = document.getElementById('bestellingenTabel');
    bestellingArray.sort(CompareByStartDatumDescending);
    for (i = 0; i < bestellingArray.length; i++) {
        let totaalPrijs = berekenPrijs(bestellingArray[i]);
        const tr = document.createElement("tr");
        tr.id = `bestellingItem${i}`;
        tr.classList.add("bestellingItem");

        let tdMerk = document.createElement("td");
        tdMerk.id = `bestellingMerk${i}`;
        tdMerk.innerHTML = bestellingArray[i].merk;
        let tdModel = document.createElement("td");
        tdModel.id = `bestellingModel${i}`;
        tdModel.innerHTML = bestellingArray[i].model;
        let tdStartdatum = document.createElement("td");
        tdStartdatum.id = `startdatum${i}`;
        tdStartdatum.innerText = bestellingArray[i].startdatum.toLocaleDateString('en-GB');
        let tdEinddatum = document.createElement("td")
        tdEinddatum.id = `einddatum${i}`;
        tdEinddatum.innerText = bestellingArray[i].einddatum.toLocaleDateString('en-GB');
        let tdPrijs = document.createElement("td");
        tdPrijs.id = `bestellingTotaalPrijs${i}`;
        tdPrijs.innerHTML = `${totaalPrijs}€`;

        tr.appendChild(tdMerk);
        tr.appendChild(tdModel);
        tr.appendChild(tdStartdatum);
        tr.appendChild(tdEinddatum);
        tr.appendChild(tdPrijs);

        if(bestellingArray[i].startdatum>new Date()){
            let tdAnnuleerKnop = document.createElement("td");
            tdAnnuleerKnop.id = `annuleerKnop${i}`;
            let annuleerKnop = document.createElement("button");
            annuleerKnop.classList.add("annuleerKnop");
            annuleerKnop.innerHTML = 'Annuleer';
            let bestelid = bestellingArray[i].bestelid
            annuleerKnop.onclick=function(){
                annuleerBestelling(bestelid,this);
            }

            tdAnnuleerKnop.appendChild(annuleerKnop);
            tr.appendChild(tdAnnuleerKnop);
        }





        table.appendChild(tr);
    }
}

async function annuleerBestelling(bestelid,elem) {
    let email = localStorage.getItem('localemail')??sessionStorage.getItem('sessionemail');
    let token = localStorage.getItem('localtoken')??sessionStorage.getItem('sessiontoken');
    let webtoken = {
        email: email,
        token: token
    }
    let body={
        bestelid:bestelid,
        webtoken:webtoken
    }
    let response = await myPostRequestJSON("bestellingen/CancelBestelling",body);
    if(response.status==200){
        let parent = elem.parentElement.parentElement;
        parent.parentElement.removeChild(parent);
        createModalOk(await response.json());
    }
    else createModalOk(await response.json());


}

function berekenPrijs(bestelling){
    let prijs = bestelling.prijs;
    let startDatum = bestelling.startdatum;
    let eindDatum = bestelling.einddatum;

    let totaalPrijs = (((eindDatum - startDatum)/oneDay)+1) * prijs;

    return totaalPrijs;
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

function convertToJSDate(datum){
    let result = datum.substring(0,10);
    let parts = datum.split("-");
    let year = parseInt(parts[0]);
    let month = parseInt(parts[1]);
    let day = parseInt(parts[2]);
    let date = new Date(year,month-1,day);
    return date;

}
function CompareByStartDatumDescending(a,b){
    if (a.startdatum>b.startdatum){
        return -1;
    }
    if (a.startdatum<b.startdatum){
        return 1;
    }
    else return 0;
}
