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
        $(`.neon-opponent-container .user-${activeConnectionToggle.userName}`).remove();
        return;
    }
    for (var i = 0; i < 503; i++) {
        $(".neon-opponent-container").append(`<div class="neon-user-${activeConnectionToggle.userName}">
            <button class="neon-btn-special">
                ${activeConnectionToggle.userName}
            </button>
        </div>`);
    }
});
connection.start();
//# sourceMappingURL=gameplay.js.map