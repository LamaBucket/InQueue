$(document).ready(function() {
  $(".dropdown-main-button").click(function(e) {
    $(".dropdown-content").toggleClass("show");
    e.stopPropagation();
  });
  $(".dropdown-content").click(function(e){
    e.stopPropagation();
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

  $(document).on("click", function(){
    $(".dropdown-content").removeClass("show");
  })
});

const closeDialog = (dialog) => {
  dialog.addClass("hidden");
};