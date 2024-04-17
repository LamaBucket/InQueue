$(".btnLogin").click(function(){ Login() });
$('.forgot-password-button').on('click', function() {
    alert('Contact @proka2 in Telegram.');
});
$('.guest-button').on('click', function() {
    RequestLoginGuest();
});


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