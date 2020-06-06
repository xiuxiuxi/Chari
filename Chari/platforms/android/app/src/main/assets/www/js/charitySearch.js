/*Navigation Drop Menu Things*/
function menu() {
    var m = document.getElementById("menuButton").style.color;
    if (m == "red") {
        hideMenu();
    } else {
        if (document.getElementById("shopButton").style.color == "red")
          hideShop();
        document.getElementById("navig").style.display = "block";
        document.getElementById("menuButton").style.color = "red";
        document.getElementById("box").style.marginLeft = "125px";
    };
}

function hideMenu() {
    document.getElementById("navig").style.display = "none";
    document.getElementById("menuButton").style.color = "black";
    document.getElementById("box").style.marginLeft = "0";
}

function shop() {
    var m = document.getElementById("shopButton").style.color;
    if (m == "red") {
        hideShop();
    } else {
        if (document.getElementById("menuButton").style.color == "red")
          hideMenu();
        document.getElementById("checkout").style.display = "block";
        document.getElementById("shopButton").style.color = "red";
        document.getElementById("box").style.marginLeft = "-125px";
    };
}

function hideShop() {
    document.getElementById("checkout").style.display = "none";
    document.getElementById("shopButton").style.color = "black";
    document.getElementById("box").style.marginLeft = "0";
}

function shwoopsies() {
    console.log("ABOUT page and user details");
    console.log(getToken());
    console.log(getFirstName());
    console.log(getLastName());
}

/*Michelle: The idea here is to create a preformatted html segment
 * that will handle the items from the API GET and will be repeated as necessary*/
function printCharity(ein, charityName, webURL, address, city, stateOrProvince, postalCode) {
  //console.log(ein);
    let divCardCollapsed = document.createElement('div');
    let divCardInner = document.createElement('div');
    let folderLogo = document.createElement('i');

    divCardCollapsed.classList = ' card [ is-collapsed ] ';
    divCardInner.className = 'card__inner [ js-expander ]';
    folderLogo.className = 'fa fa-folder-o';

    divCardInner.innerHTML = charityName;
    divCardInner.appendChild(folderLogo);
    divCardCollapsed.appendChild(divCardInner);

    let divCardExpander = document.createElement('div');
    let folderClose = document.createElement('i');

    divCardExpander.className = 'card__expander';
    folderClose.className = 'fa fa-close [ js-collapser ]';

    divCardExpander.appendChild(folderClose);
    if (city != null)
        divCardExpander.innerHTML += city + ", ";
    divCardExpander.innerHTML += stateOrProvince;
    if (postalCode != null)
        divCardExpander.innerHTML += " " + postalCode;
    if (address != null)
        divCardExpander.innerHTML += "<br/>" + address;
    if (ein != null)
        divCardExpander.innerHTML += "<br/> EIN: " + ein;
    if (webURL != null)
        divCardExpander.innerHTML += "<br/>" + webURL;

    divCardCollapsed.appendChild(divCardExpander);

    document.getElementById("cards").appendChild(divCardCollapsed);
}

function clearCharity() {
    document.getElementById("cards").innerHTML = "";
}

// Test function to output organization search
function readSearchData() {
    var searchTerm = document.getElementById("keyword").value;
    var state = document.getElementById("state").value;
    var city = document.getElementById("city").value;
    orgSearch(searchTerm, state, city);
}

function orgSearch(searchTerm, state, city) {
    var xhr = new XMLHttpRequest(),
        method = "POST",
        url = "https://chari.azurewebsites.net/api/Organization/getorgs";
    xhr.open(method, url, true);
    xhr.setRequestHeader("Content-type", "application/json");

    var obj = {
        "searchTerm": searchTerm,
        "state": state,
        "city": city
    };
    var searchCriJSON = JSON.stringify(obj);

    xhr.send(searchCriJSON);

    xhr.onload = function() {
        // clear charity after every search
        clearCharity();
        var responseText = xhr.responseText;
        let charityArr = [];
        charityArr = parseCharitySearch(xhr.responseText);

        // find the top responses and output it to the page
        for (i in charityArr) {
            let ein = charityArr[i][0];
            let charityName = charityArr[i][1];
            let webURL = charityArr[i][2];
            let address = charityArr[i][3];
            let city = charityArr[i][4];
            let stateOrProvince = charityArr[i][5];
            let postalCode = charityArr[i][6];
            printCharity(ein, charityName, webURL, address, city, stateOrProvince, postalCode);
        }
      var script= document.createElement('script');
      script.src= 'js/cardScript.js';
      document.body.appendChild(script);
    };

    xhr.onerror = function() {
        console.log('Error occurred');
    }

}

//takes in a Json as a string and returns a 2d array with columns in the following order:
//Ein, Charity Name, Website URL, Street Address, City, State, Postal Code
function parseCharitySearch(response) {
    jsonRes = JSON.parse(response);
    var respArr = Array(jsonRes.length);
    for (i in jsonRes) {
        respArr[i] = Array(7);
        respArr[i][0] = jsonRes[i].ein;
        respArr[i][1] = jsonRes[i].charityName;
        respArr[i][2] = jsonRes[i].websiteURL;
        respArr[i][3] = jsonRes[i].mailingAddress.streetAddress1;
        respArr[i][4] = jsonRes[i].mailingAddress.city;
        respArr[i][5] = jsonRes[i].mailingAddress.stateOrProvince;
        respArr[i][6] = jsonRes[i].mailingAddress.postalCode;
    }
    return respArr;
}

function btn() {
  var $btn = document.getElementsByClassName('button');
  var mouseObj = {
    mouseCoords: null,
    mousetThreshold: 0.12,
    manageMouseLeave: function(event) {
      event.currentTarget.style.boxShadow = "0 0 0 rgba(0,0,0,0.2)";
      // update btn gradient
      event.currentTarget.style.background = "red";
    },
    manageMouseMove: function(event) {
      var dot, eventDoc, doc, body, pageX, pageY;

      event = event || window.event; // IE-ism
      target = event.currentTarget;
      // (old IE)
      if (event.pageX == null && event.clientX != null) {
        eventDoc = (event.target && event.target.ownerDocument) || document;
        doc = eventDoc.documentElement;
        body = eventDoc.body;

        event.pageX = event.clientX +
          (doc && doc.scrollLeft || body && body.scrollLeft || 0) -
          (doc && doc.clientLeft || body && body.clientLeft || 0);
        event.pageY = event.clientY +
          (doc && doc.scrollTop || body && body.scrollTop || 0) -
          (doc && doc.clientTop || body && body.clientTop || 0);
      }

    // Use event.pageX / event.pageY here

    //normalize
    //bodyRect = document.body.getBoundingClientRect(),
      var elemRect = target.getBoundingClientRect(),//$btn.getBoundingClientRect(),
          mean = Math.round(elemRect.width / 2);
    //offset   = elemRect.top - bodyRect.top;

    //mouseObj.mouseCoords = {mouse_x: event.pageX - elemRect.left , mouse_y:event.pageY - elemRect.top}
      mouseObj.mouseCoords = {
        mouse_true_x: event.pageX - elemRect.left,
        mouse_x: (event.pageX - elemRect.left - mean) * mouseObj.mousetThreshold,
        mouse_y: event.pageY - elemRect.top
      }
      mouseObj.manageButtonShadow(-1, target);
    },
    manageButtonShadow: function(direction, target) {
      if (mouseObj.mouseCoords) {
        mouseObj.mouseCoords.mouse_x = Math.floor(mouseObj.mouseCoords.mouse_x);
        target.style.boxShadow = direction * mouseObj.mouseCoords.mouse_x + "px 10px 0 grey";

        // update btn gradient
        target.style.background = "radial-gradient(at "+mouseObj.mouseCoords.mouse_true_x+"px, pink 0%, red 80%)";

      }
    }
    }

// init listeners
  for(i=0; i<$btn.length; i++) {
    $btn[i].addEventListener('mousemove', mouseObj.manageMouseMove, false);
    $btn[i].addEventListener('mouseleave', mouseObj.manageMouseLeave, false);
  }
}

//followCharity("Animal Foundation");
//Takes in charity name as string
function followCharity(charityName) {
    var xhr = new XMLHttpRequest(),
        method = "PUT",
        url = "https://chari.azurewebsites.net/api/Account/followcharity?charity=" + charityName;
    xhr.open(method, url, true);
    //xhr.setRequestHeader("Content-type", "application/json");
    xhr.setRequestHeader("Authorization", "Bearer " + getToken());


    xhr.send();

    xhr.onload = function () {                  

        //DISPLAY success message*************************************
        console.log("SUCCESS");
    };

    xhr.onerror = function () {
        console.log('Error occurred');
    }
}

//Gets followed charity
//getFollowCharity();
function getFollowCharity() {
    var xhr = new XMLHttpRequest(),
        method = "GET",
        url = "https://chari.azurewebsites.net/api/Account/currentcharity"
    xhr.open(method, url, true);
    //xhr.setRequestHeader("Content-type", "application/json");
    xhr.setRequestHeader("Authorization", "Bearer " + getToken());
    xhr.send();

    xhr.onload = function () {
        //DISPLAY followed charity*************************************
        console.log(xhr.responseText);
    };

    xhr.onerror = function () {
        console.log('Error occurred');
    }
}

