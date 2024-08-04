import * as signalR from "@microsoft/signalr";
import $ from "jquery";

const connection = new signalR
	.HubConnectionBuilder()
	.withUrl("Gameplay/Hub")
	.build();

connection.on("AlreadyActive", () => {
	console.log("Already active!");
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