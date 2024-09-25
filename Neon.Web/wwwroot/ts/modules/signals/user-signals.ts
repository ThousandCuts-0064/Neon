import { Accessor, Setter, Signal, createSignal } from "solid-js";
import UserRequestType from "../enums/user-request-type";

class UserSignals {
    public readonly username: Accessor<string>;
    public readonly setUsername: Setter<string>;

    public readonly canSendUserRequest: Record<UserRequestType, Accessor<boolean>>;
    public readonly setCanSendUserRequest: Record<UserRequestType, Setter<boolean>>;

    public constructor(
        username: string,
        canSend: Record<UserRequestType, boolean>) {
        [this.username, this.setUsername] = createSignal(username);

        Object.keys(canSend).forEach((key: UserRequestType) => {
            const [get, set] = createSignal(canSend[key]);

            this.canSendUserRequest[key] = get;
            this.setCanSendUserRequest[key] = set;
        });
    }
};

export default UserSignals;