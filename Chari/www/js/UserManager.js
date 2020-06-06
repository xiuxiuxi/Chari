//This file is used to manage token and user information

//**********************************************
//Use following functions when needed
//**********************************************


//Get needed token for api calls that require authentication
//Refreshes token if it will expire within 10 minutes
//Goes to login page if unable to get valid token
function getToken() {
    var date = new Date();
    var token = window.localStorage.getItem("Token");
    var tokenExpir = window.localStorage.getItem("TokenExpiration");
    if (token == null || tokenExpir == null) {
        window.location.href = "index.html";
    }

    checkToken(tokenExpir);

    return window.localStorage.getItem("Token");
}

//Checks if the token is valid
//Returns true/false
//Refreshes token if it will expire within 10 minutes
//Will NOT go to login page if unable to get valid token
function isValidToken() {
    var token = window.localStorage.getItem("Token");
    var tokenExpir = window.localStorage.getItem("TokenExpiration");
    var date = new Date();

    if (token == null || tokenExpir == null)
        return false;
    var tokenExpirDate = new Date(tokenExpir);
    if (tokenExpirDate.getTime() >= date.getTime()) {
        return false;
    }
    tokenExpirDate.setMinutes(tokenExpirDate.getMinutes() + 10);
    if (tokenExpirDate >= date.getTime())
        refreshToken();
    return true;
}

//Gets the First Name of loged in user
//Refreshes token if it will expire within 10 minutes
//Goes to login page if unable to get valid token
function getFirstName() {
    checkToken(window.localStorage.getItem("TokenExpiration"));

    var name = window.localStorage.getItem("FirstName");
    if (name == null) {
        corruptData();
        return window.localStorage.getItem("FirstName");
    }
    return name;
}

//Gets the Last Name of loged in user
//Refreshes token if it will expire within 10 minutes
//Goes to login page if unable to get valid token
function getLastName() {
    checkToken(window.localStorage.getItem("TokenExpiration"));

    var name = window.localStorage.getItem("LastName");
    if (name == null) {
        corruptData();
        return window.localStorage.getItem("LastName");
    }
    return name;
}

//Gets the Email of the loged in user
//Refreshes token if it will expire within 10 minutes
//Goes to login page if unable to get valid token
function getEmail() {
    checkToken(window.localStorage.getItem("TokenExpiration"));

    var email = window.localStorage.getItem("Email");
    if (email == null) {
        corruptData();
        return window.localStorage.getItem("Email");
    }
    return email;
}

//Add user information to local starage
function addToken(firstName,lastName,email, token, tokenExpiration) {
    window.localStorage.setItem("Token", token);
    window.localStorage.setItem("TokenExpiration", tokenExpiration);
    window.localStorage.setItem("FirstName", firstName);
    window.localStorage.setItem("LastName", lastName);
    window.localStorage.setItem("Email", email);
}

//Show user's full name on accStat page
function accStatFullName(){
	document.getElementById("UsersFullName").innerHTML = getFirstName() + ' ' + getLastName();
}

//Show user's email on accStat page
function accStatEmail(){
	document.getElementById("UsersEmail").innerHTML = getEmail();
}



//**********************************************************
//Internal functions, no need to call these
//**********************************************************

function refreshToken() {
    var xhr = new XMLHttpRequest(),
        method = "POST",
        url = "https://chari.azurewebsites.net/api/Users/refreshtoken";

    // Open a new connection, using the POST request on the URL endpoint
    xhr.open(method, url, true);
    xhr.setRequestHeader("Content-type", "application/json");
    xhr.setRequestHeader("Authorization", "Bearer " + getToken());

    // Create a state change callback
    xhr.onreadystatechange = function () {
        if (xhr.readyState === XMLHttpRequest.DONE && xhr.status === 200) {
            //console.log(xhr.responseText);
            jsonRes = JSON.parse(xhr.responseText);
            addToken(jsonRes.firstName,jsonRes.lastName, jsonRes.email, jsonRes.token, jsonRes.tokenExpiration);
        }

        if (xhr.readyState === XMLHttpRequest.DONE && xhr.status === 400) {
            console.log(xhr.responseText);
            alert("Bad user Request");
        }

    };

    // Sending data with the request
    xhr.send();
}

function checkToken(tokenExpir) {
    if (tokenExpir == null)
        window.location.href = "index.html";

    var tokenExpirDate = new Date(tokenExpir);
    var date = new Date();
    if (tokenExpirDate.getTime() <= date.getTime()) {
        window.location.href = "index.html";
    }

    tokenExpirDate.setMinutes(tokenExpirDate.getMinutes() + 10);
    if (tokenExpirDate.getTime() <= date.getTime()) {
        refreshToken();
    }

}

function corruptData() {
    var token = window.localStorage.getItem("Token");
    if (token == null)
        window.location.href = "index.html";
    else
        refreshToken();
}
