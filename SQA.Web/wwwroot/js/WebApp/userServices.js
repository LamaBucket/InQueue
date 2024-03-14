const _defaultEndpoint = "user";

function RequestDeleteUser() 
{
    var method = "DELETE"
    SendRequest(this._defaultEndpoint, method)
}


function RequestRenameUser(newName) 
{
    var endpoint = _defaultEndpoint + "/name"
    var method = "PUT"
    var params = {
        fullName: newName
    }
    SendRequest(endpoint, method, params)
}

function RequestChangePassword(oldPassword, newPassword) 
{
    var endpoint = _defaultEndpoint + "/password"
    var method = "PUT"
    var params = {
      oldPassword: oldPassword,
      newPassword: newPassword
    }

    SendRequest(endpoint, method, params)
}