import * as signalR from "@microsoft/signalr";
import $ from "jquery";
import userMessage from "../html/lobby/user-message.html";
import { formatHtml, escapeHtml } from "./modules/html";
import UserRole from "./modules/enums/user-role";
const lobbyActiveUser = $("#neon-lobby-active-user-template").html();
const resource = JSON.parse($(`meta[name="resource"]`).attr("content"));
const userKey = $(`meta[name="user-key"]`).attr("content");
const connection = new signalR
    .HubConnectionBuilder()
    .withUrl("Lobby/Hub")
    .build();
const neonUserMessages = $(".neon-user-messages");
let suppressOnClose = false;
connection.onclose(() => {
    if (suppressOnClose) {
        suppressOnClose = false;
        return;
    }
    neonUserMessages.append(formatHtml(userMessage, {
        "username-class": "",
        "username-prefix": "[",
        "username": resource.systemName,
        "username-suffix": "]",
        "message-class": "neon-theme-front-warning",
        "message": resource.connectionLost
    }));
});
connection.on("ConnectedFromAnotherSource", () => {
    suppressOnClose = true;
    neonUserMessages.append(formatHtml(userMessage, {
        "username-class": "",
        "username-prefix": "[",
        "username": resource.systemName,
        "username-suffix": "]",
        "message-class": "neon-theme-front-warning",
        "message": resource.connectedFromAnotherSource
    }));
});
connection.on("SendMessage", (args) => {
    let usernameClass;
    switch (args.userRole) {
        case UserRole.Guest:
            usernameClass = "neon-theme-front-common";
            break;
        case UserRole.Standard:
            usernameClass = "neon-theme-front-contrast";
            break;
        case UserRole.Admin:
            usernameClass = "neon-theme-front-accent";
            break;
    }
    neonUserMessages.append(formatHtml(userMessage, {
        "username-class": usernameClass,
        "username-prefix": escapeHtml(args.usernamePrefix),
        "username": args.username,
        "username-suffix": escapeHtml(args.usernameSuffix),
        "message-class": args.isImportant ? "neon-theme-front-accent" : "neon-theme-front-common",
        "message": escapeHtml(args.message)
    }));
    neonUserMessages.animate({
        scrollTop: neonUserMessages[0].scrollHeight - neonUserMessages.height()
    }, 250);
});
connection.on("ExecutedCommand", (args) => {
    neonUserMessages.append(formatHtml(userMessage, {
        "username-class": "neon-theme-front-contrast",
        "username-prefix": escapeHtml(args.usernamePrefix),
        "username": args.username,
        "username-suffix": escapeHtml(args.usernameSuffix),
        "message-class": "neon-theme-front-success",
        "message": escapeHtml(args.message)
    }));
    neonUserMessages.animate({
        scrollTop: neonUserMessages[0].scrollHeight - neonUserMessages.height()
    }, 250);
});
connection.on("InvalidCommand", (args) => {
    neonUserMessages.append(formatHtml(userMessage, {
        "username-class": "neon-theme-front-contrast",
        "username-prefix": escapeHtml(args.usernamePrefix),
        "username": args.username,
        "username-suffix": escapeHtml(args.usernameSuffix),
        "message-class": "neon-theme-front-danger",
        "message": escapeHtml(args.message)
    }));
    neonUserMessages.animate({
        scrollTop: neonUserMessages[0].scrollHeight - neonUserMessages.height()
    }, 250);
});
connection.on("UserConnectionToggled", (args) => {
    if (args.key === userKey)
        return;
    if (!args.isActive) {
        $(`.neon-lobby-active-users [data-user-key="${args.key}"]`).remove();
        return;
    }
    $(".neon-lobby-active-users").append(formatHtml(lobbyActiveUser, {
        key: args.key,
        username: args.username
    }));
});
connection.on("DuelRequestSent", (args) => {
});
connection.on("TradeRequestSent", (args) => {
});
connection.on("FriendRequestSent", (args) => {
});
connection.start();
const neonUserForm = $(".neon-user-form");
const neonUserInput = $("#neon-user-input");
$(document).on("keydown", event => {
    switch (event.key) {
        case "Enter": {
            if (event.ctrlKey || event.shiftKey || event.altKey) {
                event.preventDefault();
                return;
            }
            const text = neonUserInput.val();
            if (text !== "")
                return;
            event.preventDefault();
            neonUserInput.trigger(neonUserInput.is(":focus") ? "blur" : "focus");
            break;
        }
        case '/': {
            const text = neonUserInput.val();
            if (text !== "")
                return;
            event.preventDefault();
            neonUserInput
                .trigger("focus")
                .val("/")
                .trigger("input");
            break;
        }
        case '!': {
            const text = neonUserInput.val();
            if (text !== "")
                return;
            event.preventDefault();
            neonUserInput
                .trigger("focus")
                .val("!")
                .trigger("input");
            break;
        }
    }
});
neonUserInput.on("input", () => {
    const messagePrefix = neonUserInput.val().toString()[0];
    switch (messagePrefix) {
        case '/':
            neonUserInput.addClass("neon-theme-front-warning");
            break;
        case '!':
            neonUserInput.addClass("neon-theme-front-accent");
            break;
        case undefined:
            neonUserInput.removeClass("neon-theme-front-warning neon-theme-front-accent");
            break;
    }
});
neonUserForm.on("submit", () => {
    connection.send("HandleInput", neonUserInput.val());
    neonUserForm.trigger("reset");
    neonUserInput.removeClass("neon-theme-front-warning neon-theme-front-accent");
    return false;
});
const activeUsersMenu = $(".neon-lobby-request-type-menu");
$(".neon-lobby-active-users").on("click", ".neon-lobby-user-row-menu button", function () {
    const target = $(this);
    const userRequestType = target.attr("data-request-type");
    target.addClass("neon-theme-front-accent");
    connection.send(`Send${userRequestType}Request`, {
        responderKey: target.closest("[data-user-key]").attr("data-user-key")
    });
});
$(".neon-lobby-incoming-user-requests").on("click", ".neon-lobby-user-row-menu button", function () {
    const target = $(this);
    const row = target.closest(".neon-lobby-user-row");
    const userRequestType = row.find("[data-request-type]").attr("data-request-type");
    const userRequestResponse = target.attr("data-request-response");
    console.log(`${userRequestResponse}${userRequestType}Request`);
    connection.send(`${userRequestResponse}${userRequestType}Request`, {
        requesterKey: target.closest("[data-user-key]").attr("data-user-key")
    });
    row.remove();
});
//# sourceMappingURL=lobby.js.map