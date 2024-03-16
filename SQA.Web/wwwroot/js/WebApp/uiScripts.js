$(document).ready(function() {
  $(".dropdown-main-button").click(function() {
    $(".dropdown-content").toggleClass("show");
  });

  $(".dialog-end-button").click(function () {
    var dialog = $(this).parents(".dialog");  

    closeDialog(dialog);
  });
});

const closeDialog = (dialog) => {
  dialog.addClass("hidden");
};