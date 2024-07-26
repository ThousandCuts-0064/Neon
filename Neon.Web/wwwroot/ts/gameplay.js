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
    $(".neon-opponent-container").append(`<div class="user-${activeConnectionToggle.userName}">
            <button>
                ${activeConnectionToggle.userName}
            </button>
        </div>`);
});
connection.start();
//# sourceMappingURL=gameplay.js.map