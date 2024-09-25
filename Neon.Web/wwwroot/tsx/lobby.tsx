import $ from "jquery";
import * as signalR from "@microsoft/signalr";
import Resource from "resources/lobby-resource";
import { escapeHtml } from "html";
import UserRole from "enums/user-role";
import UserRequestType from "enums/user-request-type";
import UserConnectionToggledArgs from "args/user-connection-toggle-args";
import { UserMessageArgs, CommandMessageArgs } from "args/message-args";
import {
    UserRequestSentArgs,
    UserRequestAcceptedArgs,
    UserRequestDeclinedArgs,
    UserRequestCanceledArgs
} from "args/user-request-args";
import { render } from "solid-js/web";
import UserMessage from "./lobby/UserMessage";
import ActiveUserRow from "./lobby/ActiveUserRow";
import UserSignals from "signals/user-signals";

const resource: Resource = JSON.parse($(`meta[name="resource"]`).attr("content"));
const userKey: string = $(`meta[name="user-key"]`).attr("content");

const users = new Map<string, UserSignals>();

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

    neonUserMessages.each((_, x) => {
        render(
            () => <UserMessage
                usernameClass=""
                usernamePrefix="["
                username={resource.systemName}
                usernameSuffix="]"
                messageClass="neon-theme-front-warning"
                message={resource.connectionLost} />,
            x);
    });
});

connection.on("ConnectedFromAnotherSource", () => {
    suppressOnClose = true;

    neonUserMessages.each((_, x) => {
        render(
            () => <UserMessage
                usernameClass=""
                usernamePrefix="["
                username={resource.systemName}
                usernameSuffix="]"
                messageClass="neon-theme-front-warning"
                message={resource.connectedFromAnotherSource} />,
            x);
    });
});

connection.on("SendMessage", (args: UserMessageArgs) => {
    let usernameClass: string;

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

connection.on("ExecutedCommand", (args: CommandMessageArgs) => {
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

connection.on("InvalidCommand", (args: CommandMessageArgs) => {
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

connection.on("UserConnectionToggled", (args: UserConnectionToggledArgs) => {
    if (args.key === userKey)
        return;

    if (!args.isActive) {
        $(`.neon-lobby-active-users [data-user-key="${args.key}"]`).remove();

        return;
    }

    const userSignals = new UserSignals(args.username, {
        Duel: true,
        Trade: true,
        Friend: true,
    });

    users.set(args.key, userSignals);

    const onUserRequestButtonClick = (userRequestType: UserRequestType, responderKey: string) => {
        connection.send(`Send${userRequestType}Request`, {
            responderKey: responderKey
        });

        userSignals.setCanSendUserRequest[userRequestType](false);
    };

    $(".neon-lobby-active-users").each((_, x) => {
        render(
            () => <ActiveUserRow
                key={args.key}
                username={userSignals.username}
                canSendUserRequest={userSignals.canSendUserRequest}
                onUserRequestButtonClick={onUserRequestButtonClick} />,
            x);
    });
});

connection.on("DuelRequestSent", (args: UserRequestSentArgs) => {
});

connection.on("TradeRequestSent", (args: UserRequestSentArgs) => {
});

connection.on("FriendRequestSent", (args: UserRequestSentArgs) => {
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


    connection.send(`${userRequestResponse}${userRequestType}Request`, {
        requesterKey: target.closest("[data-user-key]").attr("data-user-key")
    });

    row.remove();
});