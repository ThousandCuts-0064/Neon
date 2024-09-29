import SvgDuel from "../../svg/request-type/duel.svg";
import SvgTrade from "../../svg/request-type/trade.svg";
import SvgFriend from "../../svg/request-type/friend.svg";
import UserRequestType from "enums/user-request-type";
import ThemeColor from "enums/theme-color";
import { Component, Accessor, JSXElement } from "solid-js";

interface ActiveUserRowProps {
    readonly key: string;
    readonly username: Accessor<string>;
    readonly canReceiveUserRequest: Readonly<Record<UserRequestType, Accessor<boolean>>>;
    readonly onUserRequestButtonClick: (userRequestType: UserRequestType, responderKey: string) => void;
}

const ActiveUserRow: Component<ActiveUserRowProps> = ({
    key,
    username,
    canReceiveUserRequest,
    onUserRequestButtonClick }) => (
    <div class="neon-lobby-user-row">
        <div class="neon-username">{username()}</div>
        <div class="neon-lobby-user-row-menu">
            <UserRequestButton
                type={UserRequestType.Duel}
                key={key}
                canReceiveUserRequest={canReceiveUserRequest}
                onUserRequestButtonClick={onUserRequestButtonClick}>
                <SvgDuel />
            </UserRequestButton>
            <UserRequestButton
                type={UserRequestType.Trade}
                key={key}
                canReceiveUserRequest={canReceiveUserRequest}
                onUserRequestButtonClick={onUserRequestButtonClick}>
                <SvgTrade />
            </UserRequestButton>
            <UserRequestButton
                type={UserRequestType.Friend}
                key={key}
                canReceiveUserRequest={canReceiveUserRequest}
                onUserRequestButtonClick={onUserRequestButtonClick}>
                <SvgFriend />
            </UserRequestButton>
        </div>
    </div>
);

export default ActiveUserRow;

interface UserRequestButtonProps {
    readonly type: UserRequestType;
    readonly key: string;
    readonly canReceiveUserRequest: Record<UserRequestType, Accessor<boolean>>;
    readonly onUserRequestButtonClick: (userRequestType: UserRequestType, responderKey: string) => void;
    readonly children: JSXElement;
}

const getButtonColor = (isEnabled: boolean) => isEnabled ? ThemeColor.Accent : ThemeColor.Common;

const UserRequestButton: Component<UserRequestButtonProps> = ({
    type,
    key,
    canReceiveUserRequest,
    onUserRequestButtonClick,
    children }) => (
    <button
        class={`neon-theme-front-${getButtonColor(canReceiveUserRequest[type]())}`}
        onClick={() => onUserRequestButtonClick(type, key)}
        disabled={!canReceiveUserRequest[type]()}>
        {children}
    </button>
);
