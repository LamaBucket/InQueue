$(".btnLogin").click(function(){ Login() });

function GetUsername()
{
    return $(".username").val();
}

function GetPassword()
{
    return $(".password").val();
}

function Login()
{
    username = GetUsername();
    password = GetPassword();

    ValidateString(username);
    ValidateString(password);

    RequestLogin(username, password);
}