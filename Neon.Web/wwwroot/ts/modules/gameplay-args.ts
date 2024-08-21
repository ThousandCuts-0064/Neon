export class ActiveConnectionToggleArgs {
    public readonly username: string;
    public readonly isActive: boolean;

    public constructor(
        username: string,
        isActive: boolean
    ) {
        this.username = username;
        this.isActive = isActive;
    }
}

export enum UserRole {
    Guest = 1,
    Standard,
    Admin
}

export abstract class MessageArgs {
    public readonly usernamePrefix: string;
    public readonly username: string;
    public readonly usernameSuffix: string;
    public readonly message: string;

    public constructor(
        usernamePrefix: string,
        username: string,
        usernameSuffix: string,
        message: string
    ) {
        this.usernamePrefix = usernamePrefix;
        this.username = username;
        this.usernameSuffix = usernameSuffix;
        this.message = message;
    }
}

export class UserMessageArgs extends MessageArgs {
    public readonly userRole: UserRole;
    public readonly isImportant: boolean;

    public constructor(
        usernamePrefix: string,
        username: string,
        usernameSuffix: string,
        message: string,
        userRole: UserRole,
        isImportant: boolean
    ) {
        super(usernamePrefix, username, usernameSuffix, message);
        this.userRole = userRole;
        this.isImportant = isImportant;
    }
}

export class CommandMessageArgs extends MessageArgs {
    public constructor(
        usernamePrefix: string,
        username: string,
        usernameSuffix: string,
        message: string
    ) {
        super(usernamePrefix, username, usernameSuffix, message);
    }
}