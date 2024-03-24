$(document).ready(function() {
  $(".dropdown-main-button").click(function() {
    $(".dropdown-content").toggleClass("show");
  });

  $(".dialog-end-button").click(function () {
    var dialog = $(this).parents(".dialog");  

    closeDialog(dialog);
  });

  $(".btnDonate").on("click", function(){
    alert("Contact @proka2 in Telegram.");
  })
  
  $(".btnLogout").on("click", function(){
    Logout();
  })
});

const closeDialog = (dialog) => {
  dialog.addClass("hidden");
};