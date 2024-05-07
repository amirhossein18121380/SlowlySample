"use strict";

// birthdayNotifications.js
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/birthdayHub")
    .build();

connection.start()
    .then(() => console.log("Connected to SignalR hub."))
    .catch(err => console.error(err));


connection.on("SendBirthdayNotification", (message) => {
    const notificationList = document.getElementById("birthdayNotificationList");
    const listItem = document.createElement("li");
    listItem.textContent = message;
    notificationList.appendChild(listItem);
});