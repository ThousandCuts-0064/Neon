import * as signalR from "@microsoft/signalr";
const connection = new signalR
    .HubConnectionBuilder()
    .withUrl("Gameplay/Hub")
    .build();
connection.start();
//# sourceMappingURL=gameplay.js.map