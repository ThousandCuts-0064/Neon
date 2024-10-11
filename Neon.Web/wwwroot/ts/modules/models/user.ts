import UserRequestType from "../enums/user-request-type";

class User {
    public constructor(
        public readonly key: string,
        public username: string,
        public canReceive: Record<UserRequestType, boolean> = {
            Duel: true,
            Friend: true,
            Trade: true
        }) { }
};

export default User;