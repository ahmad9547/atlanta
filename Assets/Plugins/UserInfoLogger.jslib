mergeInto(LibraryManager.library, {
    GetBrowserDetails: function() {
        var browserDetails = {
            ip: "Fetching IP...",  
            browserType: navigator.userAgent,
            browserVersion: navigator.appVersion
        };

        
        var request = new XMLHttpRequest();
        request.open('GET', 'https://ipinfo.io/json', true);
        request.onload = function() {
            if (request.status >= 200 && request.status < 400) {
                var data = JSON.parse(request.responseText);
                browserDetails.ip = data.ip;
                var jsonBrowserDetails = JSON.stringify(browserDetails);
                SendMessage('UserInfoLogger', 'OnReceiveBrowserDetails', jsonBrowserDetails);
            } else {
                console.error("Failed to fetch IP information.");
            }
        };
        request.onerror = function() {
            console.error("Connection error while fetching IP information.");
        };
        request.send();
    }
});
