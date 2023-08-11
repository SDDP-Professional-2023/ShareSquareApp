var connectionChat = new signalR.HubConnectionBuilder().withUrl("/hubs/messageHub").build();

document.getElementById("sendMessage").disabled = true;

connectionChat.on("MessageReceived", function (user, message) {
    var li = document.createElement("li");

    if (user === document.getElementById("senderEmail").value) {
        li.className = "sent";
    } else {
        li.className = "received";
    }

    li.textContent = `${user} - ${message}`;
    document.getElementById("messagesList").appendChild(li);
});

connectionChat.on("sendNotification", (senderUsername) => {
    toastr.success(`${senderUsername} sent you a new message`)
});

document.getElementById("sendMessage").addEventListener("click", function (event) {
    var sender = document.getElementById("senderEmail").value;
    var messageContent = document.getElementById("chatMessage").innerText;
    var receiver = document.getElementById("receiverEmail").value;

    if (receiver.length > 0) {
        var message = {
            SenderUsername: sender,
            ReceiverUsername: receiver,
            Content: messageContent,
            Timestamp: new Date(),
            Deleted: false
        };

        $.ajax({
            url: '/Message/Send',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(message),
            success: function () {
                connectionChat.send("SendMessageToReceiver", sender, receiver, messageContent);
            },
            error: function (error) {
                console.error("Failed to send message: " + error);
            }
        });
    } else {
        connectionChat.send("SendMessageToAll", sender, message).catch(function (err) {
            return console.error(err.toString());
        });
    }

    document.getElementById("chatMessage").innerText = '';
    event.preventDefault();
})

function loadConversation(sender, receiver) {
    $('#receiverEmail').val(receiver);

    $.ajax({
        url: '/Message/GetHistoricalMessages',
        type: 'GET',
        data: { sender: sender, receiver: receiver },
        success: function (messages) {
            var messageList = $('#messagesList');
            messageList.empty();
            messages.forEach(function (message) {
                var li = document.createElement('li');

                var boldUsername = document.createElement('strong');
                boldUsername.textContent = message.senderUsername;

                if (message.senderUsername === sender) {
                    boldUsername.className = "sent-message";
                } else {
                    boldUsername.className = "received-message";
                }

                li.appendChild(boldUsername);
                li.appendChild(document.createTextNode(`: ${message.content}`));

                messageList.append(li);
            });
        }
    });
}


connectionChat.start().then(function () {
    document.getElementById("sendMessage").disabled = false;
});
