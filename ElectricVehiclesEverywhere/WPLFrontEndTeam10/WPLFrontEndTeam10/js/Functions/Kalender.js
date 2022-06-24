let datum = new Date();
let datumPlusOne = new Date();
const oneDay = 1000 * 60 * 60 * 24;
datumPlusOne.setDate(datum.getDate()+1);
let currentYear = datum.getFullYear();
let currentMonth = datum.getMonth();
const monthNames = ["Januari","Februari","Maart","April","Mei","Juni","Juli","Augustus","September","Oktober","November","December"];

const firstDay = new Date(datum.getFullYear(),datum.getMonth(),1);
let kalenderContainer = document.createElement("div");
kalenderContainer.id="kalenderContainer";
let Kalender = document.createElement("table");
Kalender.id="Kalender";



function createKalender(year,month,elem,button,occupiedDates){
    let rect = button.getBoundingClientRect();
    if(rect.top>400){
         kalenderContainer.style.transform="translateY(-215px)";
        kalenderContainer.style.zIndex="5";
     }
    else{
        kalenderContainer.style.transform="translateY(215px)";
        kalenderContainer.style.zIndex="5";

     }
    Kalender.innerHTML="";
    Kalender.appendChild(createKalenderHeader(year,month,elem,button,occupiedDates));
    Kalender.appendChild(CreateKalenderDaysOfWeek());
    CreateDates(year,month,button,occupiedDates);
    kalenderContainer.innerHTML="";
    kalenderContainer.appendChild(Kalender)
    elem.appendChild(kalenderContainer);
    let transform = -Kalender.offsetHeight-50;
}


function createKalenderHeader(year,month,elem,button,occupiedDates){
    let tr = document.createElement("tr");
    let td;


    //Creates Left Arrow
    td = document.createElement("td");
    let img = document.createElement("img");
    img.src = "../assets/icons/ArrowIcon.svg"
    td.appendChild(img);
    td.classList.add("changemonth");
    td.onclick = function(){
        PreviousMonth(year,month,elem,button,occupiedDates);
    }
    tr.appendChild(td);

    //Creates month+year label
    td = document.createElement("td");
    td.colSpan=4;
    td.innerText = `${monthNames[month]} ${year}`
    td.id="calenderlabel";
    tr.appendChild(td);


    //creates Right arrow
    td = document.createElement("td");
    img = document.createElement("img");
    img.src="../assets/icons/ArrowIcon.svg";
    img.style="transform: rotate(180deg);"
    td.appendChild(img);
    td.classList.add("changemonth");
    td.onclick = function(){
        NextMonth(year,month,elem,button,occupiedDates);
    }
    tr.appendChild(td);


    //creates close button
    td = document.createElement("td");
    img = document.createElement("img");
    img.src="../assets/icons/CloseIcon.svg"
    td.appendChild(img);
    td.id = "closeButton";
    /* td.innerText="X" */
    td.onclick=function(){
        closeCalender();
    }
    tr.appendChild(td);
    return tr;
}

function CreateKalenderDaysOfWeek(){
    let tr = document.createElement("tr");
    tr.id = "dagenVanDeWeek"
    let daysOfWeek = ['Zo','Ma','Di','Wo','Do','Vr','Za'];
    for (const day of daysOfWeek) {
        let td = document.createElement("td");
        td.innerText=day;
        tr.appendChild(td);
    }
    return tr;
}

function CreateDates(year,month,button,occupiedDates){
    let daysInMonth = new Date(year,month+1,0).getDate();
    let tr = document.createElement("tr");
    const firstDay = new Date(year,month,1).getDay();
    //outOfMonth voor eerste dag
    for (let i=0; i<firstDay;i++){
        tr.appendChild(createOutOfMonthDateTd(new Date(year,month+1,1+i-firstDay)));
    }
    let dateToFill;
    for (let i=1; i<=daysInMonth;i++){
        dateToFill = new Date(year,month,i);
        tr.appendChild(createInMonthDateTd(dateToFill,occupiedDates,button))
        if (dateToFill.getDay()==6) {
            Kalender.appendChild(tr);
            tr = document.createElement("tr");
        }
    }
    //OutofMonth na laatste dag
    let upperBound = 6-dateToFill.getDay();
    for (let i=1;i<=(6-dateToFill.getDay());i++){
        tr.appendChild(createOutOfMonthDateTd(new Date(year,month+1,i)));
    }
    Kalender.appendChild(tr);
}

function createOutOfMonthDateTd(date){
    let td = document.createElement("td");
    td.innerText=date.getDate();
    td.classList.add("outofmonth");
    td.classList.add("calenderdate-element")
    return td;
}
function createInMonthDateTd(date,occupiedDates,button){
    let td = document.createElement("td");
    td.innerText=date.getDate();
    td.classList.add("inmonth");
    td.classList.add("calenderdate-element")

    if (occupiedDates.includes(date.toDateString())){
        td.classList.add("dateisoccupied");
        td.onclick = function(){
            alert("That date is not available!");
        }
    }
    else{
        td.classList.add("dateisnotoccupied")
        td.onclick = function(){
            updateButton(date,button,occupiedDates);

        }
    }
    return td;
}
function  updateButton(date,button,occupiedDates){
    let index = button.id.charAt(button.id.length - 1);
    if(index=="d"||index=="t"){
        index ="";
    }
    const whichOne = button.id.charAt(0);
    let date1;
    let date2;
    if (whichOne=="s"){
        date1 = date;
        let date2Text = document.getElementById(`eind${index}`).innerText;
        date2= convertDate(date2Text);
        if (date1<datum){
            closeCalender();
            createModalOk("Datum moet na vandaag zijn");
            return;
        }

    }
    else {
        let date1Text = document.getElementById(`start${index}`).innerText;
        date1= convertDate(date1Text);
        date2 = date;
        if (date2<datum){
            closeCalender();
            createModalOk("Datum moet na vandaag zijn");
            return;
        }
    }

    if (date1==""||date2==""){
        button.innerText = date.toLocaleDateString('en-GB');
        closeCalender();
    }
    else if (areDatesValid(date1,date2,occupiedDates)){
        button.innerText = date.toLocaleDateString('en-GB');
        berekenPrijs(button);
        closeCalender();
    }
    else {
        closeCalender();
        createModalOk("Ongeldige keuze voor datum");

    };

}
function areDatesValid(date1,date2,occupiedDates){

    date1 = Date.parse(date1.toDateString());
    date2 = Date.parse(date2.toDateString());

    if (date1>date2){
        return false;
    }
    for (const occupiedDate of occupiedDates) {
        let tempDate = Date.parse(occupiedDate);
        if (tempDate>=date1&&tempDate<=date2){
            return false;
        }
    }
    return true;
}
function PreviousMonth(year,month,elem,button,occupiedDates){
    if (month==0)
    {
        month=11
        year -=1;
    }
    else month -=1;
    createKalender(year,month,elem,button,occupiedDates);
}
function NextMonth(year,month,elem,button,occupiedDates){
    if (month==11)
    {
        month=0
        year +=1;
    }
    else month +=1;
    createKalender(year,month,elem,button,occupiedDates);
}


function DaysInMonth(date){
    let tempDate= new Date(date.getFullYear(), date.getMonth()+1, 0);
    return tempDate.getDate();
}
//ShowCalender voor individuele product page, hier is geen occupiedDates param voor nodig
function ShowCalender(elem){
    let parentElem = elem.parentElement;
    createKalender(currentYear,currentMonth,parentElem,elem,occupiedDates);
}
function ShowCalender(elem,occupiedDatesForThisCar){
    let parentElem = elem.parentElement;
    createKalender(currentYear,currentMonth,parentElem,elem,occupiedDatesForThisCar);
}
function closeCalender(){
    let element = document.getElementById("kalenderContainer");
    element.parentNode.removeChild(element);
}
function getOccupiedDates(naam){
    const messageHeaders = new Headers({
        'Content-Type': 'application/json'})
    return fetch(apiUrl+`Bestellingen/OccupiedDates?naam=${naam}`,{
        method:'Get',
        headers: messageHeaders
    })
        .then(validateResponse)
        .then(readResponseAsJSON)
        .then(data=> vehicle=data)
        .catch(logError);

}
function convertDate(datum){
    if (datum.length!=0){let parts = datum.split("/");
        let day = parseInt(parts[0]);
        let month = parseInt(parts[1])-1;
        let year = parseInt(parts[2]);
        let date = new Date(year,month,day);
        return date;}
    else return datum;

}

