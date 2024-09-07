import UserRole from "../enums/user-role";

abstract class MessageArgs {
    public constructor(
        public readonly usernamePrefix: string,
        public readonly username: string,
        public readonly usernameSuffix: string,
        public readonly message: string
    ) {}
}

export class UserMessageArgs extends MessageArgs {
    public constructor(
        usernamePrefix: string,
        username: string,
        usernameSuffix: string,
        message: string,
        public readonly userRole: UserRole,
        public readonly isImportant: boolean
    ) {
        super(usernamePrefix, username, usernameSuffix, message);
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