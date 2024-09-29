export default interface InitializeArgs {
    readonly activeUsers: Array<ActiveUserModel>;
    readonly friends: Array<FriendModel>;
    readonly incomingUserRequests: Array<IncomingUserRequestModel>;
    readonly outgoingUserRequests: Array<OtgoingUserRequestModel>;
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
    readonly type: UserRequestType;
    readonly requesterKey: string;
    readonly requesterUsername: string;
    readonly createdAt: Date;
}

interface OtgoingUserRequestModel {
    readonly type: UserRequestType;
    readonly responderKey: string;
    readonly responderUsername: string;
    readonly createdAt: Date;
}

enum UserRequestType {
    Duel = 1,
    Trade,
    Friend
}