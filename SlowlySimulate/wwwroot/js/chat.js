"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();


connection.start().then(function () {
    joinCountryGroup();
}).catch(function (err) {
    return console.error(err.toString());
});
connection.on("Send", function (message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    console.log(message);
    li.textContent = `${message}`;
});

function joinCountryGroup() {
    connection.invoke("JoinCountryGroup")
        .catch(err => console.error(err));
}
