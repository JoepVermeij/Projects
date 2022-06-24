function toonWachtwoordSignIn() {
    let x = document.getElementById("wachtwoord-sign-in");
    if (x.type === "password") {
        x.type = "text";
    }
    else {
        x.type = "password";
    }
}

function toonWachtwoordSignUp() {
    let x = document.getElementById("wachtwoord-sign-up");
    if (x.type === "password") {
        x.type = "text";
    }
    else {
        x.type = "password";
    }
}