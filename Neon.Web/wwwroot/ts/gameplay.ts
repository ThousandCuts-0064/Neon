import * as signalR from "@microsoft/signalr";
import $ from "jquery";

const connection = new signalR
	.HubConnectionBuilder()
	.withUrl("Gameplay/Hub")
	.build();

connection.on("AlreadyActive", () => {
	console.log("Already active!");
});

connection.on("SendMessage", (message) => {
	console.log(message);
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
		$(".neon-user-opponents").append(
			`<div class="neon-user-${activeConnectionToggle.userName}">
				<button class="neon-btn-special">
				   ${activeConnectionToggle.userName}
				</button>
			</div>`);
	}
});

connection.start();


const neonUserInput = $(".neon-user-input");
const neonUserMessage = $("#neon-user-message");

neonUserInput.on("submit", () => {
	connection.send("HandleInput", neonUserMessage.val());

	neonUserInput.trigger("reset");

	return false;
})

neonUserMessage.on("input", () => {
	if (neonUserMessage.val().toString()[0] === '/')
		neonUserMessage.css("color", "green");
	else
		neonUserMessage.css("color", "black");
})