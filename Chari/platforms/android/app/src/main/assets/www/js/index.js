/*<!-- Include the JavaScript file -->
  <script src="index.js"></script> /*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
var app = {
    // Application Constructor
    initialize: function() {
        this.bindEvents();
    },
    // Bind Event Listeners
    //
    // Bind any events that are required on startup. Common events are:
    // 'load', 'deviceready', 'offline', and 'online'.
    bindEvents: function() {
        document.addEventListener('deviceready', this.onDeviceReady, false);
    },
    // deviceready Event Handler
    //
    // The scope of 'this' is the event. In order to call the 'receivedEvent'
    // function, we must explicitly call 'app.receivedEvent(...);'
    onDeviceReady: function() {
        app.receivedEvent('deviceready');
    },
    // Update DOM on a Received Event
    receivedEvent: function(id) {
        var parentElement = document.getElementById(id);
        var listeningElement = parentElement.querySelector('.listening');
        var receivedElement = parentElement.querySelector('.received');

        listeningElement.setAttribute('style', 'display:none;');
        receivedElement.setAttribute('style', 'display:block;');

        console.log('Received Event: ' + id);
    }
};

// Register users and create a JSON object after hitting the "register button"
// Parse data from the HTML document, grab the data from the input, then send the request to the URL
function registerUser(fnameReg, lnameReg, emailReg, passReg) {

    // Create xhr variable and assign a new XMLHttpRequest object to it.
    var xhr = new XMLHttpRequest(),
          method = "POST",
          url = "https://chari.azurewebsites.net/api/Users/createuser";

    // Open a new connection, using the POST request on the URL endpoint
    xhr.open(method, url, true);
    xhr.setRequestHeader("Content-type", "application/json");

    // Create a state change callback
    xhr.onreadystatechange = function () {
        if(xhr.readyState === XMLHttpRequest.DONE && xhr.status === 200) {
            console.log(xhr.responseText);
            jsonRes = JSON.parse(xhr.responseText);
            addToken(jsonRes.firstName, jsonRes.lastName, jsonRes.email, jsonRes.token, jsonRes.tokenExpiration);
            console.log(getToken());
            runPlaid();
            //window.location.ref = "CharitySearch.html";
        }
    };
   // Converting JSON data to string
   var obj = { "firstName": fnameReg, "lastName": lnameReg, "email": emailReg, "password": passReg };
   var userJSON = JSON.stringify(obj);

    // Sending data with the request
    xhr.send(userJSON);
    //alert("User has been created");
	//showLogin();
}

// login the user and connect to Chari
function connectToChari(){
  var user = document.getElementById("signUser").value;
  var pass = document.getElementById("signPass").value;

  // Converting JSON data to string
  var obj = { "email": user, "password": pass };
  var userJSON = JSON.stringify(obj);

  // Create xhr variable and assign a new XMLHttpRequest object to it.
  var xhr = new XMLHttpRequest(),
        method = "POST",
        url = "https://chari.azurewebsites.net/api/Users/login";

  // Open a new connection, using the POST request on the URL endpoint
  xhr.open(method, url, true);
  xhr.setRequestHeader("Content-type", "application/json");

  // Create a state change callback
  xhr.onreadystatechange = function () {
      if(xhr.readyState === XMLHttpRequest.DONE && xhr.status === 200) {
          //console.log(xhr.responseText);
          jsonRes = JSON.parse(xhr.responseText);
          addToken(jsonRes.firstName, jsonRes.lastName, jsonRes.email, jsonRes.token, jsonRes.tokenExpiration);
          console.log("success");
          //getToken();
          window.location.href = "charitySearch.html";
      }
      // go to main page "charitySearch"
   if(xhr.readyState === XMLHttpRequest.DONE && xhr.status === 400){
     console.log(xhr.responseText);
     alert("Bad user Request");
   }

  };

  // Sending data with the request
  xhr.send(userJSON);

  //alert("User is logging");
}

/*Logo Formating*/
var whatPage = "login";
function logoResize() {
    if (whatPage == "login") {
        if (window.innerWidth > window.innerHeight) {
            document.getElementById("logo").style.width = "30%";
        } else {
            document.getElementById("logo").style.width = "60%";
        };
    };
    if (whatPage == "register") {
        if (window.innerWidth > window.innerHeight) {
            document.getElementById("logo").style.width = "20%";
        } else {
            document.getElementById("logo").style.width = "40%";
        };
    };
    if (whatPage == "about") {
        if (window.innerWidth > window.innerHeight) {
            document.getElementById("logo").style.width = "20%";
        } else {
            document.getElementById("logo").style.width = "30%";
        };
    };
}
window.onresize = logoResize;

/* FUNCTIONS THAT "CHANGE THE PAGE" BCUZ WE DID A THREE IN ONE METHOD */

function about() {
    whatPage = "login";
	//document.getElementById("abo").style.display = "block"; // Om: Added About page stuff here as placeholder
	//document.getElementById("login").style.display = "none";
    //whatPage = "about";
    //logoResize();
    window.location.href = "charitySearch.html";
}
function showRegis() {
    document.getElementById("regis").style.display = "block";
    document.getElementById("login").style.display = "none";
    document.getElementById("abo").style.display = "none";
    whatPage = "register";
    logoResize();
}
function showLogin() {
    document.getElementById("regis").style.display = "none";
    document.getElementById("login").style.display = "block";
    document.getElementById("logo").style.width = "25%";
    document.getElementById("abo").style.display = "none";
    whatPage = "login";
    logoResize();
}

/* Checks register data before sending to the Database*/
function reqRegister() {
    var fname = document.getElementById("regFname").value;
    var lname = document.getElementById("regLname").value;
    var email = document.getElementById("regEmail").value;
    var pass = document.getElementById("regPass").value;
    var rpass = document.getElementById("regRePass").value;
    var isok = 0;
    document.getElementById("regiProb").innerHTML = "";

    if (fname == "") {
        document.getElementById("regFname").style.borderBottomColor = "rgb(219,1,31)";
        isok = 1;
        document.getElementById("regiProb").innerHTML += "First Name Field is invalid. <br>";
    } else
        document.getElementById("regFname").style.borderBottomColor = "gray";
    if (lname == "") {
        document.getElementById("regLname").style.borderBottomColor = "rgb(219,1,31)";
        isok = 1;
        document.getElementById("regiProb").innerHTML += "Last Name Field is invalid.<br>";
    } else
        document.getElementById("regLname").style.borderBottomColor = "gray";
    if ((pass != rpass)) {
        document.getElementById("regPass").style.borderBottomColor = "rgb(219,1,31)";
        document.getElementById("regRePass").style.borderBottomColor = "rgb(219,1,31)";
        isok = 1;
        document.getElementById("regiProb").innerHTML += "Password Fields do not match.<br>";
    } else {
        document.getElementById("regPass").style.borderBottomColor = "gray";
        document.getElementById("regRePass").style.borderBottomColor = "gray";
    };

    //Email Check
    var splite = email.split("");
    var atsign = 0;
    for (var k = 0; k < splite.length; k++) {
        if (splite[k] == '@')
            atsign++;
    }
    if (atsign != 1) {
        document.getElementById("regEmail").style.borderBottomColor = "rgb(219,1,31)";
        isok = 1;
        document.getElementById("regiProb").innerHTML += "Email Field is invalid. <br>";
    } else
        document.getElementById("regEmail").style.borderBottomColor = "gray";

    //Password Check
    var nu = 0;
    var lc = 0;
    var uc = 0;
    var splitp = pass.split("");
    for (var k = 0; k<splitp.length; k++) {
        if (splitp[k] >= 'a' && splitp[k] <= 'z')
            lc++;
        if (splitp[k] >= 'A' && splitp[k] <= 'Z')
            uc++;
        if (splitp[k] >= '0' && splitp[k] <= '9')
            nu++;
    }
    if (nu == 0 || lc == 0 || uc == 0 || splitp.length < 8) {
        document.getElementById("regPass").style.borderBottomColor = "rgb(219,1,31)";
        document.getElementById("regRePass").style.borderBottomColor = "rgb(219,1,31)";
        isok = 1;
        document.getElementById("regiProb").innerHTML += "Your password must be at least 8 characters, contain at least 1 number, 1 uppercase letter, and 1 lowercase letter.<br>";
    } else {
        document.getElementById("regPass").style.borderBottomColor = "gray";
        document.getElementById("regRePass").style.borderBottomColor = "gray";
    }
    document.getElementById("regiProb").innerHTML += "<br>"

    //Send to Database or not
    if (isok == 0)
        registerUser(fname, lname, email, pass);
    else
        alert("Invalid Field/s detected.");
}

/*Function that toggles Show Password checkboxes (1st for login, 2nd for register)
I separated class and function because if user accidentally check the box in
login page, and then go to register page, the password and re-password will be shown
even though the Show Password checkbox on register page is unchecked*/
//This is for login page
function togglePWVisibility1()
{
	var x = document.getElementsByClassName("PWToggle1")[0]; //Show Password
	if(x.type === "password")
	{
		x.type = "text";
	}
	else
	{
		x.type = "password";
	}
}

//This is for register page
function togglePWVisibility2()
{
	var x = document.getElementsByClassName("PWToggle2")[0]; //Show Password
	if(x.type === "password")
	{
		x.type = "text";
	}
	else
	{
		x.type = "password";
	}


	var y = document.getElementsByClassName("PWToggle2")[1]; //Show Re-Password
	if(y.type === "password")
	{
		y.type = "text";
	}
	else
	{
		y.type = "password";
	}
}
