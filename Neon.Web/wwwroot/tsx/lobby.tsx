import $ from "jquery";
import * as signalR from "@microsoft/signalr";
import Resource from "resources/lobby-resource";
import { escapeHtml } from "html";
import UserRole from "enums/user-role";
import UserRequestType from "enums/user-request-type";
import InitializeArgs from "args/initialize-args";
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
import ActiveUser from "signals/online-user";
import { JSX } from "solid-js/jsx-runtime";

const resource: Resource = JSON.parse(document
    .querySelector(`meta[name="resource"]`)!
    .getAttribute("content")!);

const userKey: string = document
    .querySelector(`meta[name="user-key"]`)!
    .getAttribute("content")!;

const activeUsers = new Map<string, ActiveUser>();

const connection = new signalR
    .HubConnectionBuilder()
    .withUrl("Lobby/Hub")
    .build();

const neonUserMessages = () => [...document.getElementsByClassName("neon-user-messages")];
let suppressOnClose = false;

connection.onclose(() => {
    if (suppressOnClose) {
        suppressOnClose = false;

        return;
    }

    neonUserMessages().forEach(x => {
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

connection.on("Initialize", (args: InitializeArgs) => {

})

connection.on("ConnectedFromAnotherSource", () => {
    suppressOnClose = true;

    neonUserMessages().forEach(x => {
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

    neonUserMessages().forEach(x => {
        render(
            () => <UserMessage
                usernameClass={usernameClass}
                usernamePrefix={escapeHtml(args.usernamePrefix)}
                username={args.username}
                usernameSuffix={escapeHtml(args.usernameSuffix)}
                messageClass={args.isImportant ? "neon-theme-front-accent" : "neon-theme-front-common"}
                message={escapeHtml(args.message)} />,
            x);
    });
});

connection.on("ExecutedCommand", (args: CommandMessageArgs) => {
    neonUserMessages().forEach(x => {
        render(
            () => <UserMessage
                usernameClass="neon-theme-front-contrast"
                usernamePrefix={escapeHtml(args.usernamePrefix)}
                username={args.username}
                usernameSuffix={escapeHtml(args.usernameSuffix)}
                messageClass="neon-theme-front-success"
                message={escapeHtml(args.message)} />,
            x);
    });
});

connection.on("InvalidCommand", (args: CommandMessageArgs) => {
    neonUserMessages().forEach(x => {
        render(
            () => <UserMessage
                usernameClass="neon-theme-front-contrast"
                usernamePrefix={escapeHtml(args.usernamePrefix)}
                username={args.username}
                usernameSuffix={escapeHtml(args.usernameSuffix)}
                messageClass="neon-theme-front-danger"
                message={escapeHtml(args.message)} />,
            x);
    });
});

connection.on("UserConnectionToggled", (args: UserConnectionToggledArgs) => {
    if (args.key === userKey)
        return;

    if (!args.isActive) {
        activeUsers.get(args.key)?.[Symbol.dispose]();
        activeUsers.delete(args.key);

        return;
    }

    const onUserRequestButtonClick = (userRequestType: UserRequestType, responderKey: string) => {
        connection.send(`Send${userRequestType}Request`, {
            responderKey: responderKey
        });

        activeUser.setCanReceiveUserRequest[userRequestType](false);
    };

    const activeUser = new ActiveUser(
        args.key,
        args.username,
        {
            Duel: true,
            Trade: true,
            Friend: true,
        },
        [...document.getElementsByClassName(".neon-lobby-active-users")],
        onUserRequestButtonClick
    );

    activeUsers.set(args.key, activeUser);
});

connection.on("DuelRequestSent", (args: UserRequestSentArgs) => {
});

connection.on("TradeRequestSent", (args: UserRequestSentArgs) => {
});

connection.on("FriendRequestSent", (args: UserRequestSentArgs) => {
});

connection.start();


const neonUserForm = document.getElementById("neon-user-form") as HTMLFormElement;
const neonUserInput = document.getElementById("neon-user-input") as HTMLInputElement;

document.addEventListener("keydown", event => {
    switch (event.key) {
        case "Enter": {
            if (event.ctrlKey || event.shiftKey || event.altKey) {
                event.preventDefault();

                return;
            }

            const text = neonUserInput.value;

            if (text !== "")
                return;

            event.preventDefault();

            if (document.activeElement === neonUserInput)
                neonUserInput.blur();
            else
                neonUserInput.focus();

            break;
        }
        case '/': {
            const text = neonUserInput.value;

            if (text !== "")
                return;

            event.preventDefault();

            neonUserInput.focus();
            neonUserInput.value = '/';
            neonUserInput.dispatchEvent(new Event("input"));

            break;
        }
        case '!': {
            const text = neonUserInput.value;

            if (text !== "")
                return;

            event.preventDefault();

            neonUserInput.focus();
            neonUserInput.value = '!';
            neonUserInput.dispatchEvent(new Event("input"));

            break;
        }
    }
});

neonUserInput.addEventListener("input", () => {
    const messagePrefix = neonUserInput.value[0];

    switch (messagePrefix) {
        case '/':
            neonUserInput.classList.add("neon-theme-front-warning");

            break;

        case '!':
            neonUserInput.classList.add("neon-theme-front-accent");

            break;

        case undefined:
            neonUserInput.classList.remove("neon-theme-front-warning", "neon-theme-front-accent");

            break;
    }
});

neonUserForm.addEventListener("submit", () => {
    connection.send("HandleInput", neonUserInput.value);

    neonUserForm.reset();
    neonUserInput.classList.remove("neon-theme-front-warning", "neon-theme-front-accent");

    return false;
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