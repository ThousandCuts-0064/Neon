import * as signalR from "@microsoft/signalr";
import "jquery";
const connection = new signalR
    .HubConnectionBuilder()
    .withUrl("Gameplay/Hub")
    .build();
connection.on("AlreadyActive", () => {
    console.log("Already active!");
});
connection.on("ActiveConnectionToggle", (activeConnectionToggle) => {
    $(".neon-opponent-container").add(activeConnectionToggle.userName);
});
connection.start();
//# sourceMappingURL=gameplay.js.map