export class ActiveConnectionToggleArgs {
    username;
    isActive;
    constructor(username, isActive) {
        this.username = username;
        this.isActive = isActive;
    }
}
export var UserRole;
(function (UserRole) {
    UserRole[UserRole["Guest"] = 1] = "Guest";
    UserRole[UserRole["Standard"] = 2] = "Standard";
    UserRole[UserRole["Admin"] = 3] = "Admin";
})(UserRole || (UserRole = {}));
export class MessageArgs {
    usernamePrefix;
    username;
    usernameSuffix;
    message;
    constructor(usernamePrefix, username, usernameSuffix, message) {
        this.usernamePrefix = usernamePrefix;
        this.username = username;
        this.usernameSuffix = usernameSuffix;
        this.message = message;
    }
}
export class UserMessageArgs extends MessageArgs {
    userRole;
    isImportant;
    constructor(usernamePrefix, username, usernameSuffix, message, userRole, isImportant) {
        super(usernamePrefix, username, usernameSuffix, message);
        this.userRole = userRole;
        this.isImportant = isImportant;
    }
}
export class CommandMessageArgs extends MessageArgs {
    constructor(usernamePrefix, username, usernameSuffix, message) {
        super(usernamePrefix, username, usernameSuffix, message);
    }
}
//# sourceMappingURL=gameplay-args.js.map