// JavaScript source code
function runPlaid() {
    var handler = Plaid.create({
        clientName: 'Chari',
        // Optional, specify an array of ISO-3166-1 alpha-2 country
        // codes to initialize Link; European countries will have GDPR
        // consent panel
        countryCodes: ['US'],
        env: 'sandbox',
        // Replace with your public_key from the Dashboard
        key: 'd19189b7adfdfd969f31c65c446c65',
        product: ['transactions'],
        // Optional, use webhooks to get transaction and error updates
        webhook: 'https://chari.azurewebsites.net/api/PlaidItem/PlaidWebhook',
        // Optional, specify a language to localize Link
        language: 'en',
        // Optional, specify userLegalName and userEmailAddress to
        // enable all Auth features
        userLegalName: 'John Appleseed',
        userEmailAddress: 'jappleseed@yourapp.com',
        onLoad: function () {
            // Optional, called when Link loads
        },
        onSuccess: function (public_token, metadata) {
            // Send the public_token to your app server.
            // The metadata object contains info about the institution the
            // user selected and the account ID or IDs, if the
            // Select Account view is enabled.
            var xhr = new XMLHttpRequest(),
                method = "POST",
                url = "https://chari.azurewebsites.net/api/PlaidItem/AddBank?public_token=" + public_token;

            // Open a new connection, using the POST request on the URL endpoint
            xhr.open(method, url, true);
            xhr.setRequestHeader("Content-type", "application/json");
            xhr.setRequestHeader("Authorization", "Bearer " + getToken());

            // Create a state change callback 
            xhr.onreadystatechange = function () {
                if (xhr.readyState === XMLHttpRequest.DONE && xhr.status === 200) {
                    window.location.href = "charitySearch.html";
                }
            };


            // Sending data with the request 
            xhr.send();
            //$.post('/get_access_token', {
            //    public_token: public_token,
            //});
        },
        onExit: function (err, metadata) {
            // The user exited the Link flow.
            if (err != null) {
                // The user encountered a Plaid API error prior to exiting.
            }
            // metadata contains information about the institution
            // that the user selected and the most recent API request IDs.
            // Storing this information can be helpful for support.
        },
        onEvent: function (eventName, metadata) {
            // Optionally capture Link flow events, streamed through
            // this callback as your users connect an Item to Plaid.
            // For example:
            // eventName = "TRANSITION_VIEW"
            // metadata  = {
            //   link_session_id: "123-abc",
            //   mfa_type:        "questions",
            //   timestamp:       "2017-09-14T14:42:19.350Z",
            //   view_name:       "MFA",
            // }
        }
    });
    handler.open();
}