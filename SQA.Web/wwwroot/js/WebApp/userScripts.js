$(".btnRename").click(function(){ Rename() });
$(".btnChangePassword").click(function(){ ChangePassword() });
$(".btnDelete").click(function(){ DeleteUser() });
$(".btnCreateUser").click(function(){
    CreateUser();
})


function Rename()
{
    newName = prompt("Enter new Name:");

    RequestRenameUser(newName);
}

function ChangePassword()
{
    oldPassword = prompt("Enter old Password:");
    newPassword = prompt("Enter New Password:");

    RequestChangePassword(oldPassword, newPassword);
}

function DeleteUser()
{
    yousureuwanttodeleteyourprofilemate = confirm("Are you sure?");

    if(yousureuwanttodeleteyourprofilemate)
    {
        RequestDeleteUser();
    }
}

function CreateUser(){
    username = prompt("Enter Username");
    fullName = prompt("Enter full name");
    password = prompt("Enter Password");
    roleId = prompt("Enter role id");

    RequestCreateUser(username, fullName, password, roleId);
}