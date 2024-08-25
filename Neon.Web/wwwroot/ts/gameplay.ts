import * as signalR from "@microsoft/signalr";
import $ from "jquery";
import userMessage from "../html/user-message.html";
import userOpponent from "../html/user-opponent.html";
import { formatHtml, escapeHtml } from "./modules/html";
import UserRole from "./modules/enums/user-role";
import ActiveConnectionToggleArgs from "./modules/args/active-connection-toggle-args";
import { UserMessageArgs, CommandMessageArgs } from "./modules/args/message-args";

const connection = new signalR
	.HubConnectionBuilder()
	.withUrl("Gameplay/Hub")
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
		"username-prefix": "",
		"username": "",
		"username-suffix": "",
		"message-class": "neon-theme-front-warning",
		"message": "Connection lost. Refresh the page to reconnect."
	}));
});

connection.on("ConnectedFromAnotherSource", () => {
	suppressOnClose = true;

	neonUserMessages.append(formatHtml(userMessage, {
		"username-class": "",
		"username-prefix": "",
		"username": "",
		"username-suffix": "",
		"message-class": "neon-theme-front-warning",
		"message": "Logged in from another source. Refresh the page to reconnect."
	}));
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

connection.on("ActiveConnectionToggle", (args: ActiveConnectionToggleArgs) => {
	if (!args.isActive) {
		$(`.neon-user-opponents .user-${args.username}`).remove();

		return;
	}

	for (var i = 0; i < 503; i++) {
		$(".neon-user-opponents").append(userOpponent.replace(/{{username}}/g, args.username));
	}
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


$(".neon-user-opponents").on("click", "button", event => {
	$(event.target).removeClass("neon-button-accent").addClass("neon-button-common");
});