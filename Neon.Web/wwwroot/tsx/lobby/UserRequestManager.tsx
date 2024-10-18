import { MountableElement, render } from "solid-js/web";
import { Component, Accessor, For, Setter } from "solid-js";
import { createStore, SetStoreFunction, produce } from "solid-js/store";
import User from "models/user";
import UserRequestType from "enums/user-request-type";
import ActiveUserRow from "./ActiveUserRow";
import IncomingUserRequestRow from "./IncomingUserRequestRow";
import OutgoingUserRequestRow from "./OutgoingUserRequestRow";

export default class UserRequestManager {
    private readonly activeUsers: User[];
    private readonly setActiveUsers: SetStoreFunction<User[]>;

    private readonly incomingUserRequests: IncomingUserRequest[];
    private readonly setIncomingUserRequests: SetStoreFunction<IncomingUserRequest[]>;

    private readonly outgoingUserRequests: OutgoingUserRequest[];
    private readonly setOutgoingUserRequests: SetStoreFunction<OutgoingUserRequest[]>;

    public constructor(
        activeUsersParent: MountableElement,
        incomingUserRequestsParent: MountableElement,
        outgoingUserRequestsParent: MountableElement,
        onUserRequestButtonClick: (userRequestType: UserRequestType, responderKey: string) => void,
        onAcceptButtonClick: (userRequestType: UserRequestType, requesterKey: string) => void,
        onDeclineButtonClick: (userRequestType: UserRequestType, requesterKey: string) => void,
        onCancelButtonClick: (userRequestType: UserRequestType, requesterKey: string) => void
    ) {
        const [activeUsers, setActiveUsers] = createStore<User[]>([]);
        const [incomingUserRequests, setIncomingUserRequests] = createStore<IncomingUserRequest[]>([]);
        const [outgoingUserRequests, setOutgoingUserRequests] = createStore<OutgoingUserRequest[]>([]);

        this.activeUsers = activeUsers;
        this.setActiveUsers = setActiveUsers;

        this.incomingUserRequests = incomingUserRequests;
        this.setIncomingUserRequests = setIncomingUserRequests;

        this.outgoingUserRequests = outgoingUserRequests;
        this.setOutgoingUserRequests = setOutgoingUserRequests;

        render(
            () => <ActiveUsers
                activeUsers={activeUsers}
                onUserRequestButtonClick={onUserRequestButtonClick} />,
            activeUsersParent);

        render(
            () => <IncomingUserRequests
                incomingUserRequests={incomingUserRequests}
                onAcceptButtonClick={onAcceptButtonClick}
                onDeclineButtonClick={onDeclineButtonClick} />,
            incomingUserRequestsParent);

        render(
            () => <OutgoingUserRequests
                outgoingUserRequests={outgoingUserRequests}
                onCancelButtonClick={onCancelButtonClick} />,
            outgoingUserRequestsParent);
    }

    public OnUserActivated(activeUser: User) {
        this.setActiveUsers(produce(x => x.push(activeUser)));
    }

    public OnUserDeactivated(key: string) {
        const index = this.activeUsers.findIndex(x => x.key === key);

        this.setActiveUsers(produce(x => x.splice(index, 1)));
    }

    public OnUserRequestReceived(incomingUserRequest: IncomingUserRequest) {
        this.setIncomingUserRequests(produce(x => x.push(incomingUserRequest)));
    }

    public OnUserRequestResponded(requesterKey: string, userRequestType: UserRequestType) {
        const index = this.incomingUserRequests
            .findIndex(x => x.user.key === requesterKey && x.type === userRequestType);

        this.setIncomingUserRequests(produce(x => x.splice(index, 1)));
    }

    public OnUserRequestSent(outgoingUserRequest: OutgoingUserRequest) {
        this.setOutgoingUserRequests(produce(x => x.push(outgoingUserRequest)));
    }

    public OnUserRequestCanceled(responderKey: string, userRequestType: UserRequestType) {
        const index = this.outgoingUserRequests
            .findIndex(x => x.user.key === responderKey && x.type === userRequestType);

        this.setOutgoingUserRequests(produce(x => x.splice(index, 1)));
    }
}



interface ActiveUsersProps {
    readonly activeUsers: User[];
    readonly onUserRequestButtonClick: (userRequestType: UserRequestType, responderKey: string) => void;
}

const ActiveUsers: Component<ActiveUsersProps> = props => (
    <For each={props.activeUsers}>{x =>
        <ActiveUserRow
            key={x.key}
            username={x.username}
            canReceive={x.canReceive}
            onUserRequestButtonClick={props.onUserRequestButtonClick} />
    }</For>);



type IncomingUserRequest = {
    readonly createdAt: Date;
    readonly type: UserRequestType;
    readonly user: User;
};

interface IncomingUserRequestsProps {
    readonly incomingUserRequests: IncomingUserRequest[];
    readonly onAcceptButtonClick: (userRequestType: UserRequestType, requesterKey: string) => void;
    readonly onDeclineButtonClick: (userRequestType: UserRequestType, requesterKey: string) => void;
}

const IncomingUserRequests: Component<IncomingUserRequestsProps> = props => (
    <For each={props.incomingUserRequests}>{x =>
        <IncomingUserRequestRow
            createdAt={x.createdAt}
            type={x.type}
            key={x.user.key}
            username={x.user.username}
            onAcceptButtonClick={props.onAcceptButtonClick}
            onDeclineButtonClick={props.onDeclineButtonClick} />
    }</For>);



type OutgoingUserRequest = {
    readonly createdAt: Date;
    readonly type: UserRequestType;
    readonly user: User;
};

interface OutgoingUserRequestsProps {
    readonly outgoingUserRequests: IncomingUserRequest[];
    readonly onCancelButtonClick: (userRequestType: UserRequestType, responderKey: string) => void;
}

const OutgoingUserRequests: Component<OutgoingUserRequestsProps> = props => (
    <For each={props.outgoingUserRequests}>{x =>
        <OutgoingUserRequestRow
            createdAt={x.createdAt}
            type={x.type}
            key={x.user.key}
            username={x.user.username}
            onCancelButtonClick={props.onCancelButtonClick} />
    }</For>);