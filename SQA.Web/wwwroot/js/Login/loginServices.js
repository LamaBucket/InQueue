_endpoint = "authenticate";

function RequestLogin(username, password) {
  var method = "POST"
  var params = {
    username: username,
    password: password
  }

  SendRequest(_endpoint, method, params);
} 