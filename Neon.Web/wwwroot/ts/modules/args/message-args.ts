import UserRole from "../enums/user-role";

interface MessageArgs {
	readonly usernamePrefix: string;
	readonly username: string;
	readonly usernameSuffix: string;
	readonly message: string;
}

export interface UserMessageArgs extends MessageArgs {
	readonly userRole: UserRole;
	readonly isImportant: boolean;
}

export interface CommandMessageArgs extends MessageArgs { }