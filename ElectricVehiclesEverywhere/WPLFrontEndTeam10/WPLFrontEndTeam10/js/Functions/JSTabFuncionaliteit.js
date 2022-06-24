
let signIn = document.getElementById("sign-in-html");
let signUp = document.getElementById("sign-up-html");

function logIn() {
    if (signIn.display === "none") {
        signUp.style.display = "block";
        signUp.style.opacity = "1";
        signIn.style.display = "none";
        signIn.style.opacity = "0";
    } else {
        signIn.style.display = "block";
        signIn.style.opacity = "1";
        signUp.style.display = "none";
        signUp.style.opacity = "0";
}
}

function maakAccount() {
    if (signUp.display === "none") {
        signIn.style.display = "block";
        signIn.style.opacity = "1";
        signUp.style.display = "none";
        signUp.style.opacity = "0";
    } else {
        signUp.style.display = "block";
        signUp.style.opacity = "1";
        signIn.style.display = "none";
        signIn.style.opacity = "0";
    }
}


/*
function logIn() {
    GetSignUpHidden();
    GetSignInDisplayed();
}

function maakAccount() {
    GetSignInHidden();
    GetSignUpDisplayed();
}
*/

/* functions om SignInHTML te displayen */
/*
function GetSignUpHidden() {
    document.querySelector(".sign-up-html").css("opacity", "0").addEventListener('transitionend webkitTransitionEnd oTransitionEnd otransitionend', HideSignUpAfterAnimation);
}

function GetSignInDisplayed() {
    document.querySelector(".sign-in-html").css("display", "block").css("opacity", "1").unbind("transitionend webkitTransitionEnd oTransitionEnd otransitionend");
}

function HideSignUpAfterAnimation() {
    document.querySelector(".sign-up-html").css("display", "none");
}
*/

/* functions om SignUpHTML te displayen */
/*
function GetSignInHidden() {
    document.querySelector(".sign-in-html").css("opacity", "0").addEventListener('transitionend webkitTransitionEnd oTransitionEnd otransitionend', HideSignInAfterAnimation);
}

function GetSignUpDisplayed() {
    document.querySelector(".sign-up-html").css("display", "block").css("opacity", "1").unbind("transitionend webkitTransitionEnd oTransitionEnd otransitionend");
}

function HideSignInAfterAnimation() {
    document.querySelector(".sign-in-html").css("display", "none");
}
*/