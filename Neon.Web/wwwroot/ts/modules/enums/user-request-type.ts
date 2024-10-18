enum UserRequestType {
    Duel = "Duel",
    Trade = "Trade",
    Friend = "Friend"
}

export enum UserRequestTypeInt {
    Duel = 1,
    Trade,
    Friend
}

export const parseUserRequestType = (userRequestTypeInt: UserRequestTypeInt) => {
    return UserRequestType[UserRequestTypeInt[userRequestTypeInt] as keyof typeof UserRequestType];
}

export default UserRequestType;
