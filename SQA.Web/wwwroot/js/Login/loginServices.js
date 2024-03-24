_endpoint = "authenticate";

function RequestLogin(username, password) {
  var method = "POST"
  var params = {
    username: username,
    password: password
  }

  SendRequest(_endpoint, method, params, HandleLoginResponse);
} 

function HandleLoginResponse(response){
  var params = new URLSearchParams(window.location.search);

  var returnUrl = params.get("backTo");

  if(returnUrl == null){
    window.location.href = "/Web";
  }

  window.location.href = returnUrl;
}