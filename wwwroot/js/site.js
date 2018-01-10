let connection = new signalR.HubConnection("messengerHub"),
    chatName = "Message",
    username= "";

function sendMessage(){
    const message = $("#message").val();
    if(!message || !username){
        return;
    }
    connection.invoke(chatName,`${username}: ${message}`);
    $("#message").val('');
}

function writeToChat(message){
    const data = $(`<div><p>${message}</p></div>`);
    $("#messages").append(data);
}

function startSignalR(){    
    username= $("#username").val();
    if(!username){
        return;
    }

    connection.on(chatName, (data) => {
        writeToChat(data);
    });
        
    connection.start()
        .then(() => 
            connection.invoke(
                chatName,
                 `${username} dołączył do chatu` 
            )
        );
    changeForm();
}

function changeForm(){
    $("#username").attr("readonly", true);
    $("#join").attr("disabled", true);
}
