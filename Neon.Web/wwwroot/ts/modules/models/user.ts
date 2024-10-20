import UserRequestType from "../enums/user-request-type";

type User = {
    key: string,
    username: string,
    canReceive: Record<UserRequestType, boolean>;
};

export default User;