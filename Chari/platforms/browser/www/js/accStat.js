/*Navigation Drop Menu Things*/
function menu() {
    var m = document.getElementById("menuButton").style.color;
    if (m == "red") {
        hideMenu();
    } else {
        document.getElementById("navig").style.display = "block";
        document.getElementById("menuButton").style.color = "red";
        document.getElementById("box").style.marginLeft = "135px";
    };
}

function hideMenu() {
    document.getElementById("navig").style.display = "none";
    document.getElementById("menuButton").style.color = "black";
    document.getElementById("box").style.marginLeft = "0";
}

function shwoopsies() {
    alert("ABOUT may need to be on a separate page");
    console.log(getFirstName());
}

function getDonations() {
    var xhr = new XMLHttpRequest(),
        method = "GET",
        url = "https://chari.azurewebsites.net/api/Donations";
    xhr.open(method, url, true);
    xhr.setRequestHeader("Content-type", "application/json");
    xhr.setRequestHeader("Authorization", "Bearer " + getToken());


    xhr.send();

    xhr.onload = function () {
        // clear charity after every search

        var jsonRes = xhr.responseText;
        let donationArr = [];
        donationArr = parseDonation(xhr.responseText);

        //DISPLAY donations
        console.log(donationArr);
    };

    xhr.onerror = function () {
        console.log('Error occurred');
    }
}

//takes in a Json as a string and returns a 2d array with columns in the following order:
//Original transaction ammount, donation ammount, donation date, charity ein, charity name
function parseDonation(response) {
    jsonRes = JSON.parse(response);
    var respArr = Array(jsonRes.length);
    for (i in jsonRes) {
        respArr[i] = Array(5);
        respArr[i][0] = jsonRes[i].transAmount;
        respArr[i][1] = jsonRes[i].donationsAmount;
        respArr[i][2] = jsonRes[i].donationDate;
        respArr[i][3] = jsonRes[i].donationEin;
        respArr[i][3] = jsonRes[i].donationName;
    }
    return respArr;
}

function userDetails(){
  let div = document.createElement('div');
  var name = getFirstName() + " " + getLastName();
  var email = getEmail();
  div.className = "userContainer";
  div.innerHTML += "<div> Name: " + name + "</div><div> Email: " + email + "</div>";

  document.getElementById("profile_info").appendChild(div);
}

function userInfo(){
  userDetails();
  getDonations();
}
