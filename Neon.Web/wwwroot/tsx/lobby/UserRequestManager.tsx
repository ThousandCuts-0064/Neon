import { MountableElement, render } from "solid-js/web";
import { Component, Accessor, createSignal, For, Setter } from "solid-js";
import User from "models/user";
import UserRequestType from "enums/user-request-type";
import ActiveUserRow from "./ActiveUserRow";
import IncomingUserRequestRow from "./IncomingUserRequestRow";
import OutgoingUserRequestRow from "./OutgoingUserRequestRow";

export default class UserRequestManager {
    private readonly setActiveUsers: Setter<Map<string, ActiveUser>>;
    private readonly setIncomingUserRequests: Setter<Map<string, IncomingUserRequest>>;
    private readonly setOutgoingUserRequests: Setter<Map<string, OutgoingUserRequest>>;

    public constructor(
        activeUsersParent: MountableElement,
        incomingUserRequestsParent: MountableElement,
        outgoingUserRequestsParent: MountableElement,
        onUserRequestButtonClick: (userRequestType: UserRequestType, responderKey: string) => void,
        onAcceptButtonClick: (userRequestType: UserRequestType, requesterKey: string) => void,
        onDeclineButtonClick: (userRequestType: UserRequestType, requesterKey: string) => void,
        onCancelButtonClick: (userRequestType: UserRequestType, requesterKey: string) => void
    ) {
        const [activeUsers, setActiveUsers] = createSignal(new Map<string, ActiveUser>(), { equals: false });
        const [incomingUserRequests, setIncomingUserRequests] = createSignal(new Map<string, IncomingUserRequest>(), { equals: false });
        const [outgoingUserRequests, setOutgoingUserRequests] = createSignal(new Map<string, OutgoingUserRequest>(), { equals: false });

        this.setActiveUsers = setActiveUsers;
        this.setIncomingUserRequests = setIncomingUserRequests;
        this.setOutgoingUserRequests = setOutgoingUserRequests;

        render(
            () => <ActiveUsers
                activeUsers={() => [...activeUsers().values()]}
                onUserRequestButtonClick={onUserRequestButtonClick} />,
            activeUsersParent);

        render(
            () => <IncomingUserRequests
                incomingUserRequests={() => [...incomingUserRequests().values()]}
                onAcceptButtonClick={onAcceptButtonClick}
                onDeclineButtonClick={onDeclineButtonClick} />,
            incomingUserRequestsParent);

        render(
            () => <OutgoingUserRequests
                outgoingUserRequests={() => [...outgoingUserRequests().values()]}
                onCancelButtonClick={onCancelButtonClick} />,
            outgoingUserRequestsParent);
    }

    public OnUserActivated(activeUser: ActiveUser) {
        this.setActiveUsers(x => x.set(
            activeUser.userSignals.key,
            activeUser));
    }

    public OnUserDeactivated(activeUser: ActiveUser) {
        this.setActiveUsers(x => {
            x.delete(activeUser.userSignals.key);

            return x;
        });
    }

    public OnUserRequestReceived(incomingUserRequest: IncomingUserRequest) {
        this.setIncomingUserRequests(x => x.set(
            `${incomingUserRequest.userSignals.key} ${incomingUserRequest.type}`,
            incomingUserRequest));
    }

    public OnUserRequestResponded(incomingUserRequest: IncomingUserRequest) {
        this.setIncomingUserRequests(x => {
            x.delete(`${incomingUserRequest.userSignals.key} ${incomingUserRequest.type}`);

            return x;
        });
    }

    public OnUserRequestSent(outgoingUserRequest: OutgoingUserRequest) {
        this.setOutgoingUserRequests(x => x.set(
            `${outgoingUserRequest.userSignals.key} ${outgoingUserRequest.type}`,
            outgoingUserRequest));
    }

    public OnUserRequestCanceled(outgoingUserRequest: OutgoingUserRequest) {
        this.setOutgoingUserRequests(x => {
            x.delete(`${outgoingUserRequest.userSignals.key} ${outgoingUserRequest.type}`);

            return x;
        });
    }
}



type ActiveUser = {
    readonly userSignals: User;
};

interface ActiveUsersProps {
    readonly activeUsers: () => ActiveUser[];
    readonly onUserRequestButtonClick: (userRequestType: UserRequestType, responderKey: string) => void;
}

const ActiveUsers: Component<ActiveUsersProps> = ({ activeUsers, onUserRequestButtonClick }) => (
    <For each={activeUsers()}>{x =>
        <ActiveUserRow
            key={x.userSignals.key}
            username={x.userSignals.username}
            canReceiveUserRequest={x.userSignals.canReceiveUserRequest}
            onUserRequestButtonClick={onUserRequestButtonClick} />
    }</For>);



type IncomingUserRequest = {
    readonly createdAt: Date;
    readonly type: UserRequestType;
    readonly userSignals: User;
};

interface IncomingUserRequestsProps {
    readonly incomingUserRequests: () => IncomingUserRequest[];
    readonly onAcceptButtonClick: (userRequestType: UserRequestType, requesterKey: string) => void;
    readonly onDeclineButtonClick: (userRequestType: UserRequestType, requesterKey: string) => void;
}

const IncomingUserRequests: Component<IncomingUserRequestsProps> = ({
    incomingUserRequests,
    onAcceptButtonClick,
    onDeclineButtonClick }) => (
    <For each={incomingUserRequests()}>{x =>
        <IncomingUserRequestRow
            createdAt={x.createdAt}
            type={x.type}
            key={x.userSignals.key}
            username={x.userSignals.username}
            onAcceptButtonClick={onAcceptButtonClick}
            onDeclineButtonClick={onDeclineButtonClick} />
    }</For>);



type OutgoingUserRequest = {
    readonly createdAt: Date;
    readonly type: UserRequestType;
    readonly userSignals: User;
    readonly onCancelButtonClick: (userRequestType: UserRequestType, requesterKey: string) => void;
};

interface OutgoingUserRequestsProps {
    readonly outgoingUserRequests: () => IncomingUserRequest[];
    readonly onCancelButtonClick: (userRequestType: UserRequestType, responderKey: string) => void;
}

const OutgoingUserRequests: Component<OutgoingUserRequestsProps> = ({ outgoingUserRequests, onCancelButtonClick }) => (
    <For each={outgoingUserRequests()}>{x =>
        <OutgoingUserRequestRow
            createdAt={x.createdAt}
            type={x.type}
            key={x.userSignals.key}
            username={x.userSignals.username}
            onCancelButtonClick={onCancelButtonClick} />
    }</For>);