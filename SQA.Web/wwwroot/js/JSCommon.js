function SendRequest(_endpoint, _type, args, handleResponse = null) {
    alert(args);
    var url = new URL(window.location.protocol + "//" + window.location.host + "/" + _endpoint);

    var query = BuildQuery(args);

    url += query;

    var req = new Request(url, { method: _type, });

    fetch(req).then(response => {
        ParseResponseDefault(response, handleResponse)
    });
}

function ParseResponseDefault(response, handleResponse = null) {
    if (response.status != 200) {
        response.json().then(data => {
            if (data != null) {
                alert(data.detail);
            }
            else {
                alert('Some Error Occured.\nTry To Reload The Page Or Contact Server Administrator.')
            }
        })
    }
    else {
        if (response.redirected) {
            window.location = response.url
        }
        else {
            if (handleResponse == null) {
                document.location.reload();
            }
            else {
                handleResponse(response);
            }
        }
    }
}

function BuildQuery(params) {
    var result = new URLSearchParams();
    
    for (var key in params) {
        var value = params[key];

        result.append(key, value);
    }

    return "?" + result.toString();
}





function ValidateString(string)
{
    if(string == "")
    {
        alert("Required string was empty!");
    }

    return string != "";
}

function Reload()
{
    location.reload();
}