let dropdownstate ={
    klasse:false,
    sort:false,
    filterMenu:false
}
let loadState = true;
let filterState = {
    auto:true,
    brommer:true,
    fiets:true,
    step:true
}
//intitieert filterede array
let filteredvehiclearray =[];

//laat onload alle voertuigen zien
window.onload= async function (){
    await loginCheck() ;
    await getProducten();
    filteredvehiclearray = uniqueProductenArray.filter(x => filterState[x.type] === true);
    ShowProductCards(filteredvehiclearray);
    toonAantalProducten();
}
//ShowProductCards(allvehiclesarray);

function ToggleDropDown(dropdown){
    if (dropdownstate[dropdown]===false){
        ShowDropDown(dropdown);
    }
    else HideDropDown(dropdown);
}
function ShowDropDown(dropdown){
    let menu = document.getElementById(`${dropdown}-dropdownbuttoncontainer`);
        menu.classList.add("block-element");
        menu.classList.remove("hide-element");
        let arrow = document.getElementById(`${dropdown}-dropdown-arrow`)
    arrow.classList.remove("standard-arrow");
        arrow.classList.add("rotate-arrow");
    dropdownstate[dropdown] = !dropdownstate[dropdown];
}
function HideDropDown(dropdown){
    let menu = document.getElementById(`${dropdown}-dropdownbuttoncontainer`);
    menu.classList.remove("block-element");
    menu.classList.add("hide-element");
    let arrow = document.getElementById(`${dropdown}-dropdown-arrow`)
    arrow.classList.add("standard-arrow");
    arrow.classList.remove("rotate-arrow");
    dropdownstate[dropdown] = !dropdownstate[dropdown];
}

//Maakt productkaarten uit array aan
function ShowProductCards(array){
    document.getElementById("productcardscontainer").innerHTML="";
    for (const vehicle of array) {
        createProductCard(vehicle);
    }
}

function TogglePopupFilterMenu(){
    if (dropdownstate["filterMenu"]){
        document.getElementById("popup-filtermenu").classList.remove("flex-element");
        document.getElementById("popup-filtermenu").classList.add("hide-element");
    }
    else{
        document.getElementById("popup-filtermenu").classList.add("flex-element");
        document.getElementById("popup-filtermenu").classList.remove("hide-element");
    }
    dropdownstate["filterMenu"]=!dropdownstate["filterMenu"];

}

function mobileFilter(e){
    console.log("hier");
    console.log(productenArray);
    switch (e.id){
        case "ddb-alle":
            filteredvehiclearray = uniqueProductenArray;
            break;
        case"ddb-auto":
            filteredvehiclearray = uniqueProductenArray.filter(x=>x.type=="auto");
            break;
        case"ddb-brommer":
            filteredvehiclearray = uniqueProductenArray.filter(x=>x.type=="brommer");
            break;
        case"ddb-fiets":
            filteredvehiclearray=uniqueProductenArray.filter(x=>x.type=="fiets");
            break;
        case"ddb-step":
            filteredvehiclearray=uniqueProductenArray.filter(x=>x.type=="step");
            break;

    }
    document.getElementById("klasse-dropdown-selected").innerText=e.innerText;
    ShowProductCards(filteredvehiclearray);
}

function desktopFilter(e,element) {
    if(loadState === true){
        filterState = {
            auto:false,
            brommer:false,
            fiets:false,
            step:false
        }
        loadState = false;
    }
    filterState[e] = !filterState[e];
    element.classList.toggle("active-filter")
    filteredvehiclearray = uniqueProductenArray.filter(x => filterState[x.type] === true);
    ShowProductCards(filteredvehiclearray);
}

function SortCards(sortType,element){
    switch (sortType){
        case 0:
            filteredvehiclearray.sort(CompareByPriceAscending)
            document.getElementById("laag-hoog").checked=true;
            break;
        case 1:
            filteredvehiclearray.sort(CompareByPriceDescending)
            document.getElementById("hoog-laag").checked=true;
            break;
        case 2:
            filteredvehiclearray.sort(CompareByNameAscending);
            document.getElementById("A-Z").checked=true;
            break;
        case 3:
            filteredvehiclearray.sort(CompareByNameDescending)
            document.getElementById("Z-A").checked=true;
            break;
        case 4:
            filteredvehiclearray.sort(CompareByActionAscending)
            document.getElementById("actieradius-asc").checked=true;
            break;
        case 5:
            filteredvehiclearray.sort(CompareByActionDescending)
            document.getElementById("actieradius-desc").checked=true;
            break;
    }
    //if else kijkt waar de click vandaan komt, .value en .innertext is nodig afhankelijk van sender
    if (element.nodeName==="INPUT"){
        document.getElementById("sortmenu-label").innerText=element.value;
    }
    else{
        document.getElementById("sortmenu-label").innerText=element.innerText;
    }
    ShowProductCards(filteredvehiclearray);
}

function CompareByNameAscending(a,b){
    if (a.naam>b.naam){
        return 1;
    }
    if (a.naam<b.naam){
        return -1;
    }
    else return 0;
}
function CompareByNameDescending(a,b){
    if (a.naam>b.naam){
        return -1;
    }
    if (a.naam<b.naam){
        return 1;
    }
    else return 0;
}
function CompareByPriceAscending(a,b){
    if (a.prijs>b.prijs){
        return 1;
    }
    if (a.prijs<b.prijs){
        return -1;
    }
    else return 0;
}
function CompareByPriceDescending(a,b){
    if (a.prijs>b.prijs){
        return -1;
    }
    if (a.prijs<b.prijs){
        return 1;
    }
    else return 0;
}
function CompareByActionDescending(a,b){
    if (a.actieradius>b.actieradius){
        return -1;
    }
    if (a.actieradius<b.actieradius){
        return 1;
    }
    else return 0;
}
function CompareByActionAscending(a,b){
    if (a.actieradius>b.actieradius){
        return 1;
    }
    if (a.actieradius<b.actieradius){
        return -1;
    }
    else return 0;
}

