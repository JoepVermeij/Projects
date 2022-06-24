window.addEventListener("scroll", handleScroll);
let scrollposition =0;

//Functie die uitgevoerd wordt bij klikken op hamburgermenu, houdt huidige scrollposition voor bij HideMenu2
//verbergt alle content behalve nav en laat navbuttons in het groot zien.
//nav krijgt donkerblauwe achtergrond
function ShowMenu2(){

    scrollposition = window.scrollY;
    console.log(scrollposition);
    window.scrollTo(0,0);
    window.removeEventListener("scroll", handleScroll);
    document.getElementById("hamburgermenu").classList.add("hide-element");
    document.getElementById("crossmenu").classList.remove("hide-element");
    document.getElementById("notnav").classList.remove("block-element");
    document.getElementById("notnav").classList.add("hide-element");

    for(let element of document.getElementsByClassName("dropdown-content2"))
    {
        element.classList.add("flex-element");
        element.classList.remove("hide-element");
    }
    $("#subnav").css({"background-color": `rgba(24, 44, 76, 1)`});




}
//Functie die uitgevoerd wordt bij klikken op Crossmenu
//zet pagina terug naar net voor hamburgermenu klik, inculsief scroll height
function HideMenu2(){


    window.addEventListener("scroll", handleScroll);
    document.getElementById("hamburgermenu").classList.remove("hide-element");
    document.getElementById("crossmenu").classList.add("hide-element");
    document.getElementById("notnav").classList.add("block-element");
    document.getElementById("notnav").classList.remove("hide-element");

    for(let element of document.getElementsByClassName("dropdown-content2"))
    {
        element.classList.remove("flex-element");
        element.classList.add("hide-element");
    }

    $("#subnav").css({"background-color": `rgba(24, 44, 76, 0)`});


    console.log(scrollposition);
    window.scrollTo(0,scrollposition);
}


//var checkpoint = document.getElementById("landingpage").clientHeight;
const checkpoint = 350;
var test = window.location;
console.log(test);

function handleScroll() {
    //checkpoint = document.getElementById("landingpage").clientHeight;
    const currentScroll = window.pageYOffset;
    let opacity;
    if (currentScroll <= checkpoint) {
        opacity = currentScroll / checkpoint;
    } else {
        opacity = 1;
    }
    $("#subnav").css({"background-color": `rgba(24, 44, 76, ${opacity})`});
}

//toont aantal producten in winkelmand
function toonAantalProducten(){
    let navWinkelmandIcon = document.getElementById("navwinkelmandcontainer");
    let pAantalProducten = document.createElement("p");
    pAantalProducten.id = "aantalProducten"
    pAantalProducten.innerHTML = aantalProductenInWinkelmand();
    navWinkelmandIcon.append(pAantalProducten);

    if (aantalProductenInWinkelmand() === 0) document.getElementById("aantalProducten").style.display = "none";
}

//berekent het aantal producten in de winkelmand aan de hand van de winkelmandlijst in de localstorage
function aantalProductenInWinkelmand(){
    let email = localStorage.getItem('localemail');
    let winkelmandLijst = JSON.parse(localStorage.getItem('winkelmandLijst'));

    if (Object.keys(winkelmandLijst).length > 0){
        let nullProducts = winkelmandLijst[`null`];
        if (!('null' in winkelmandLijst)) nullProducts = [];
        let winkelmandProducts = winkelmandLijst[`${email}`];
        if (!(`${email}` in winkelmandLijst)) winkelmandProducts = [];

        if (loginCheck()){
            return winkelmandProducts.length + nullProducts.length;
        }
        return nullProducts.length;
    }
    else return 0;
}