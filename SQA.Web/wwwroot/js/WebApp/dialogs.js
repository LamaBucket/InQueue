$(".btnAddQueue").click(() => {
    ShowQueueDialog();
});


$(".dialog-create-queue-ok").on("click", () => {
    CreateQueue();
});

$(".dialog-confirm-leave-queue-ok").on("click", () => {
    LeaveQueue();
});


const addQueueDialog = $(".create-queue-dialog");
const confirmLeaveQueueDialog = $(".leave-queue-confirm-dialog");

function ShowQueueDialog(){
    addQueueDialog.removeClass("hidden");
}
  
function ShowConfirmLeaveQueueDialog(){
    confirmLeaveQueueDialog.removeClass("hidden");
}