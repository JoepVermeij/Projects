let modal = document.createElement("div");

window.onclick = function(event) {
    if (event.target === modal) {
        document.body.removeChild(document.getElementById("modal"));
    }
}

function createModalOk(message, doThis){
    modal.innerHTML="";
    modal.classList.add("modal");
    modal.id="modal";
    let modalcontent = document.createElement("div");
    modalcontent.classList.add("modal-content");
    let p1 = document.createElement("p");
    p1.innerText= `${message}`;
    let buttoncontainer = document.createElement("div");
    buttoncontainer.classList.add("modal-content-buttons");
    let button = document.createElement("button");
    button.innerText="Ok";
    button.onclick = function(){
        document.body.removeChild(document.getElementById("modal"));
        doThis();
    }
    buttoncontainer.appendChild(button);
    modalcontent.appendChild(p1);
    modalcontent.appendChild(buttoncontainer);
    modal.appendChild(modalcontent);
    document.body.appendChild(modal);
}

function createModalYesNo(message, doThis){
    modal.innerHTML="";
    modal.classList.add("modal");
    modal.id="modal";
    let modalcontent = document.createElement("div");
    modalcontent.classList.add("modal-content");
    let p1 = document.createElement("p");
    p1.innerText= `${message}`;
    let buttoncontainer = document.createElement("div");
    buttoncontainer.classList.add("modal-content-buttons");
    let button1 = document.createElement("button");
    button1.innerText="Ja";
    button1.onclick = function(){
        document.body.removeChild(document.getElementById("modal"));
        doThis();
    };
    let button2 = document.createElement("button");
    button2.innerText="Nee";
    button2.onclick = function(){
        document.body.removeChild(document.getElementById("modal"));
    }
    buttoncontainer.appendChild(button1);
    buttoncontainer.appendChild(button2);
    modalcontent.appendChild(p1);
    modalcontent.appendChild(buttoncontainer);
    modal.appendChild(modalcontent);
    document.body.appendChild(modal);
}