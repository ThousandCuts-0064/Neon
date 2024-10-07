import { Accessor, Setter, createSignal } from "solid-js";
import { render } from "solid-js/web";
import UserRequestType from "../enums/user-request-type";
import ActiveUserRow from "../../../tsx/lobby/ActiveUserRow";

class UserSignals {
    public readonly key: string;
    public readonly username: Accessor<string>;
    public readonly setUsername: Setter<string>;

    public readonly canReceiveUserRequest: Readonly<Record<UserRequestType, Accessor<boolean>>>;
    public readonly setCanReceiveUserRequest: Readonly<Record<UserRequestType, Setter<boolean>>>;

    public constructor(
        key: string,
        username: string,
        canSend: Record<UserRequestType, boolean>) {
        this.key = key;

        [this.username, this.setUsername] = createSignal(username);

        const canReceiveUserRequest = {} as Record<UserRequestType, Accessor<boolean>>;
        const setCanReceiveUserRequest = {} as Record<UserRequestType, Setter<boolean>>;

        Object.entries(canSend).forEach(([key, value]) => {
            const [get, set] = createSignal(value);

            canReceiveUserRequest[key as UserRequestType] = get;
            setCanReceiveUserRequest[key as UserRequestType] = set;
        });

        this.canReceiveUserRequest = canReceiveUserRequest;
        this.setCanReceiveUserRequest = setCanReceiveUserRequest;
    }
};

export default UserSignals;