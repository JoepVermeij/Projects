let para;
const startDatum = document.getElementById("start");
const eindDatum = document.getElementById("end");
const productCalender = document.getElementById("productcalender");
let occupiedDates =[];
let data;

const searchParams = new URLSearchParams(window.location.search);
let vehicleName = searchParams.get("vehicle");



window.onload = async function(){
    await loginCheck();

    data = await getVehicleData()
    for (let i=0;i<data.dates.length;i++){
        let string = data.dates[i];
        let year = string.substr(0,4);
        let month = string.substr(5,2);
        let day =string.substr(8,2);
        data.dates[i] = new Date(year,month-1,day).toDateString();
    }
    occupiedDates = data.dates;
    createPage(data.vehicle);
    await toonAantalProducten();
    return data;

}

async function getVehicleData(){
    const datesPromise = getOccupiedDates(vehicleName);
    const vehiclePromise = getFirstVehicle(vehicleName);
    const [dates,vehicle]= await Promise.all([datesPromise,vehiclePromise]);
    return{
        dates: dates,
        vehicle:vehicle
    }
}

function  getFirstVehicle(naam){
    const messageHeaders = new Headers({
        'Content-Type': 'application/json'})
    return fetch(apiUrl+`Product/FirstByName?naam=${naam}`,{
        method:'Get',
        headers: messageHeaders
    })
        .then(validateResponse)
        .then(readResponseAsJSON)
        .then(data=> vehicle=data)
        .catch(logError);

}
//getAllOfVehicle(newProduct.naam);
function  getAllOfVehicle(naam){
    const messageHeaders = new Headers({ // (1)
        'Content-Type': 'application/json'})
    fetch(apiUrl+`Product/ByName?naam=${naam}`,{
        method:'Get',
        headers: messageHeaders
    })
        .then(validateResponse)
        .then(readResponseAsText)
        .then(readResponseAsJSON)
        .catch(logError);

}

function createPage(newProduct){
    const productImage = document.getElementById("productimage");
    let img = document.createElement("img");
    img.src = newProduct.imageUrl;
    productImage.appendChild (img);


    const productInfo = document.getElementById("productinfo");
    let h2 = document.getElementById("h2");
    h2.innerHTML = newProduct.naam;



    let productbereik =document.createElement("div");
    productbereik.classList.add("ind-productbereik");
    let actieradius = document.createElement("p");
    actieradius.innerText="Actieradius";
    let actieradiuskm = document.createElement("p");
    actieradiuskm.innerText = `${newProduct.actieRadius} km`;
    productbereik.appendChild(actieradius);
    productbereik.appendChild(actieradiuskm);

    let topsnelheiddiv=document.createElement("div");
    topsnelheiddiv.classList.add("ind-topsnelheid");
    let topsnelheid = document.createElement("p");
    topsnelheid.innerText="Topsnelheid";
    let topsnelheidkm=document.createElement("p");
    topsnelheidkm.innerText=`${newProduct.topSnelheid} km/u`;
    topsnelheiddiv.appendChild(topsnelheid);
    topsnelheiddiv.appendChild(topsnelheidkm);



    let accdiv = document.createElement("div");
    accdiv.classList.add("ind-acceleratie");
    let acc = document.createElement("p");
    acc.innerText="Acceleratie";
    let acckmu = document.createElement("p");
    acckmu.innerText=`${newProduct.accelerate}s (0-100 km/u)`
    accdiv.appendChild(acc);
    accdiv.appendChild(acckmu);


    let productplaatsen = document.createElement("div");
    productplaatsen.classList.add("ind-productplaatsen");
    let zitplaatsen = document.createElement("p");
    zitplaatsen.innerText="Aantal zitplaatsen";
    let aantalzitplaatsen = document.createElement("p");
    aantalzitplaatsen.innerText=newProduct.zitplaatsen;

    productplaatsen.appendChild(zitplaatsen);
    productplaatsen.appendChild(aantalzitplaatsen);

    let productprijs = document.createElement("div");
    productprijs.classList.add("ind-productprijs");

    let prijs = document.createElement("p");
    prijs.innerText="Prijs";
    let prijsperdag = document.createElement("p");
    prijsperdag.innerText = `${newProduct.prijs} €/dag`;

    productprijs.appendChild(prijs);
    productprijs.appendChild(prijsperdag);


    productInfo.appendChild(productbereik);
    productInfo.appendChild(createLine());
    productInfo.appendChild(topsnelheiddiv);
    productInfo.appendChild(createLine());
    productInfo.appendChild(accdiv);
    productInfo.appendChild(createLine());
    productInfo.appendChild(productplaatsen);
    productInfo.appendChild(createLine());
    productInfo.appendChild(productprijs);

    berekenPrijs();
}


function reserveerClick(){

    let startDatum = document.getElementById("start").innerText;
    let eindDatum = document.getElementById("eind").innerText;
    if (startDatum===""||eindDatum===""){
        createModalOk("Vul beide datums in!");
    }
    else{
        let winkelmandLijst = JSON.parse(localStorage.getItem('winkelmandLijst'));
        if (winkelmandLijst==null)winkelmandLijst={};

        let email = localStorage.getItem('localemail');
        //if (email == null)email="null";

        let array = winkelmandLijst[`${email}`];
        if (array == null)array=[];

        let bestelling = data.vehicle;

        bestelling['StartDatum'] = startDatum;
        bestelling['EindDatum'] = eindDatum;
        array.push(bestelling);

        winkelmandLijst[`${email}`] = array;

        winkelmandLijst = JSON.stringify(winkelmandLijst);

        localStorage.setItem('winkelmandLijst',winkelmandLijst);
        window.location.href="WinkelmandPageForRent.html";

    }


}



function berekenPrijs(elem){
    prijs = data.vehicle.prijs;
    let date1Text = document.getElementById(`start`).innerText;
    let date2Text = document.getElementById(`eind`).innerText;
    let date1= convertDate(document.getElementById(`start`).innerText);
    let date2= convertDate(document.getElementById(`eind`).innerText);
    const today = new Date();
    if (date1==""||date2==""){
        console.log("Een van de 2 dates is null")
        document.getElementById(`totaalPrijs`).innerText=`0 €`;
    }
    else{
        let days = ((date2-date1)/oneDay)+1 ;
        console.log(`days:${days}`);
        document.getElementById(`totaalPrijs`).innerText=`${prijs*days} €`;
    }

}


function addParagraaf(divId, stringId){
    para = document.createElement("p");
    para.id = stringId;
    para.appendChild(textNode);
    divId.appendChild(para);
}

function createLine(){
    let lijn =document.createElement("div");
    lijn.classList.add("lijn");
    return lijn;
}