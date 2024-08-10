import * as signalR from "@microsoft/signalr";
import $ from "jquery";
import userMessage from "../html/user-message.html";
const connection = new signalR
    .HubConnectionBuilder()
    .withUrl("Gameplay/Hub")
    .build();
connection.on("AlreadyActive", () => {
    console.log("Already active!");
});
const neonUserMessages = $(".neon-user-messages");
connection.on("SendMessage", (username, message) => {
    neonUserMessages.append(userMessage.replace("{{ message }}", `${username}: ${message}`))
        .addClass("newon-user-message");
    neonUserMessages.animate({
        scrollTop: neonUserMessages[0].scrollHeight - neonUserMessages.height()
    }, 250);
});
connection.on("ExecutedCommand", (message) => {
    console.log(message);
});
connection.on("InvalidCommand", (message) => {
    console.log(message);
});
connection.on("ActiveConnectionToggle", (activeConnectionToggle) => {
    if (!activeConnectionToggle.isActive) {
        $(`.neon-user-opponents .user-${activeConnectionToggle.userName}`).remove();
        return;
    }
    for (var i = 0; i < 503; i++) {
        $(".neon-user-opponents").append(`<div class="neon-user-${activeConnectionToggle.userName}">
				<button class="neon-btn-special">
				   ${activeConnectionToggle.userName}
				</button>
			</div>`);
    }
});
connection.start();
const neonUserForm = $(".neon-user-form");
const neonUserInput = $("#neon-user-input");
neonUserForm.on("submit", () => {
    connection.send("HandleInput", neonUserInput.val());
    neonUserForm.trigger("reset");
    return false;
});
neonUserInput.on("input", () => {
    if (neonUserInput.val().toString()[0] === '/')
        neonUserInput.css("color", "green");
    else
        neonUserInput.css("color", "#AAAAAA");
});
//# sourceMappingURL=gameplay.js.map