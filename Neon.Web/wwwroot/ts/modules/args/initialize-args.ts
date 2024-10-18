import { UserRequestTypeInt } from "../enums/user-request-type";

export default interface InitializeArgs {
    readonly activeUsers: ActiveUserModel[];
    readonly friends: FriendModel[];
    readonly incomingUserRequests: IncomingUserRequestModel[];
    readonly outgoingUserRequests: OtgoingUserRequestModel[];
}

interface ActiveUserModel {
    readonly key: string;
    readonly username: string;
}

interface FriendModel {
    readonly key: string;
    readonly username: string;
}

interface IncomingUserRequestModel {
    readonly type: UserRequestTypeInt;
    readonly requesterKey: string;
    readonly requesterUsername: string;
    readonly createdAt: Date;
}

interface OtgoingUserRequestModel {
    readonly type: UserRequestTypeInt;
    readonly responderKey: string;
    readonly responderUsername: string;
    readonly createdAt: Date;
}