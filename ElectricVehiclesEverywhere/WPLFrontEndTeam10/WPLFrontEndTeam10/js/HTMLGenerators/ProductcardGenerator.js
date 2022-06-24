function createProductCard(vehicle){
    let productcard = document.createElement("div");
    productcard.classList.add("productcard");

    let productcardheader= document.createElement("div");
    productcardheader.classList.add("productcardheader");

    let merkmodel = document.createElement("p");
    merkmodel.classList.add("merkmodel");
    merkmodel.innerText=vehicle.naam;
    let gradienttransition = document.createElement("div");
    gradienttransition.classList.add("gradienttransition");
    let productfoto = document.createElement("img");
    productfoto.classList.add("productfoto");
    productfoto.src=vehicle.imageUrl;
    productfoto.alt=vehicle.naam;

    productcardheader.appendChild(merkmodel);
    productcardheader.appendChild(gradienttransition);
    productcardheader.appendChild(productfoto);

    let productinfo = document.createElement("div");
    productinfo.classList.add("productinfo")

    let productprijs = document.createElement("div");
    productprijs.classList.add("productprijs");

    let prijs = document.createElement("p");
    prijs.innerText="Prijs";
    let prijsperdag = document.createElement("p");
    prijsperdag.innerText = `${vehicle.prijs} €/dag`;

    productprijs.appendChild(prijs);
    productprijs.appendChild(prijsperdag);

    let productbereik =document.createElement("div");
    productbereik.classList.add("productbereik");

    let actieradius = document.createElement("p");
    actieradius.innerText="Actieradius";
    let actieradiuskm = document.createElement("p");
    actieradiuskm.innerText = `${vehicle.actieradius} km`;

    productbereik.appendChild(actieradius);
    productbereik.appendChild(actieradiuskm);

    let productplaatsen = document.createElement("div");
    productplaatsen.classList.add("productplaatsen");

    let zitplaatsen = document.createElement("p");
    zitplaatsen.innerText="Aantal zitplaatsen";
    let aantalzitplaatsen = document.createElement("p");
    aantalzitplaatsen.innerText=vehicle.zitplaatsen;

    productplaatsen.appendChild(zitplaatsen);
    productplaatsen.appendChild(aantalzitplaatsen);

    let meerinfo = document.createElement("div");
    meerinfo.classList.add("meerinfo");

    let meerinfolink = document.createElement("a");
    meerinfolink.innerText="MEER INFO";
    meerinfolink.href=`IndividueleProductPage.html?vehicle=${vehicle.naam}`;

    meerinfo.appendChild(meerinfolink);

    productinfo.appendChild(productprijs);
    productinfo.appendChild(createLine());
    productinfo.appendChild(productbereik);
    productinfo.appendChild(createLine());
    productinfo.appendChild(productplaatsen);
    productinfo.appendChild(meerinfo);

    productcard.appendChild(productcardheader);
    productcard.appendChild(productinfo);

    document.getElementById("productcardscontainer").appendChild(productcard);

}

function createLine(){
    let lijn =document.createElement("div");
    lijn.classList.add("lijn");
    return lijn;
}



