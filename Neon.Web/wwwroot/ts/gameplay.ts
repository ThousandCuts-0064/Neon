import * as signalR from "@microsoft/signalr";
import $ from "jquery";
import userMessage from "../html/user-message.html";
import userOpponent from "../html/user-opponent.html";

const connection = new signalR
	.HubConnectionBuilder()
	.withUrl("Gameplay/Hub")
	.build();

const neonUserMessages = $(".neon-user-messages");

connection.on("AlreadyActive", () => {
	neonUserMessages.append(formatHtml(userMessage, {
		"username-class": "",
		"username-prefix": "",
		"username": "",
		"username-suffix": "",
		"message-class": "neon-theme-front-danger",
		"message": "Already logged in from another tab. This one will be closed in:"
	}));

	let seconds = 5;

	setInterval(() => {
		neonUserMessages.append(formatHtml(userMessage, {
			"username-class": "",
			"username-prefix": "",
			"username": "",
			"username-suffix": "",
			"message-class": "neon-theme-front-danger",
			"message": seconds.toString()
		}));

		if (seconds-- <= 0)
			window.close();
	}, 1000);
});

connection.on("SendMessage", (userRole, usernamePrefix, username, usernameSuffix, message, isImportant) => {
	neonUserMessages.append(formatHtml(userMessage, {
		"username-class": "neon-theme-front-common",
		"username-prefix": escapeHtml(usernamePrefix),
		"username": username,
		"username-suffix": escapeHtml(usernameSuffix),
		"message-class": isImportant ? "neon-theme-front-accent" : "neon-theme-front-common",
		"message": escapeHtml(message)
	}));

	neonUserMessages.animate({
		scrollTop: neonUserMessages[0].scrollHeight - neonUserMessages.height()
	}, 250);
});

connection.on("ExecutedCommand", (usernamePrefix, username, usernameSuffix, message) => {
	neonUserMessages.append(formatHtml(userMessage, {
		"username-class": "neon-theme-front-contrast",
		"username-prefix": escapeHtml(usernamePrefix),
		"username": username,
		"username-suffix": escapeHtml(usernameSuffix),
		"message-class": "neon-theme-front-success",
		"message": escapeHtml(message)
	}));

	neonUserMessages.animate({
		scrollTop: neonUserMessages[0].scrollHeight - neonUserMessages.height()
	}, 250);
});

connection.on("InvalidCommand", (usernamePrefix, username, usernameSuffix, message) => {
	neonUserMessages.append(formatHtml(userMessage, {
		"username-class": "neon-theme-front-contrast",
		"username-prefix": escapeHtml(usernamePrefix),
		"username": username,
		"username-suffix": escapeHtml(usernameSuffix),
		"message-class": "neon-theme-front-danger",
		"message": escapeHtml(message)
	}));

	neonUserMessages.animate({
		scrollTop: neonUserMessages[0].scrollHeight - neonUserMessages.height()
	}, 250);
});

connection.on("ActiveConnectionToggle", (activeConnectionToggle) => {
	if (!activeConnectionToggle.isActive) {
		$(`.neon-user-opponents .user-${activeConnectionToggle.userName}`).remove();

		return;
	}

	for (var i = 0; i < 503; i++) {
		$(".neon-user-opponents").append(userOpponent.replace(/{{username}}/g, activeConnectionToggle.userName));
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

const formatHtml = (html: string, data: Record<string, string>) => {
	return html.replace(/{{(.*?)}}/g, (match, key) => {
		return key in data ? data[key] : match;
	});
};

const escapeHtml = function (unsafe: string) {
	return unsafe.replace(/[&<>"']/g, match => {
		switch (match) {
			case '&': return '&amp;';
			case '<': return '&lt;';
			case '>': return '&gt;';
			case '"': return '&quot;';
			default: return '&#039;';
		}
	});
};