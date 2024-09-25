import SvgDuel from "../../svg/request-type/duel.svg";
import SvgTrade from "../../svg/request-type/trade.svg";
import SvgFriend from "../../svg/request-type/friend.svg";
import UserRequestType from "enums/user-request-type";
import ThemeColor from "enums/theme-color";
import { Component, Accessor, JSXElement } from "solid-js";

interface ActiveUserRowProps {
    readonly key: string;
    readonly username: Accessor<string>;
    readonly canSendUserRequest: Record<UserRequestType, Accessor<boolean>>;
    readonly onUserRequestButtonClick: (userRequestType: UserRequestType, responderKey: string) => void;
}

const ActiveUserRow: Component<ActiveUserRowProps> = ({
    key,
    username,
    canSendUserRequest,
    onUserRequestButtonClick }) => (
    <div class="neon-lobby-user-row" data-user-key={key}>
        <div class="neon-username">{username()}</div>
        <div class="neon-lobby-user-row-menu">
            <UserRequestButton
                type={UserRequestType.Duel}
                key={key}
                canSendUserRequest={canSendUserRequest}
                onUserRequestButtonClick={onUserRequestButtonClick}>
                <SvgDuel />
            </UserRequestButton>
            <UserRequestButton
                type={UserRequestType.Trade}
                key={key}
                canSendUserRequest={canSendUserRequest}
                onUserRequestButtonClick={onUserRequestButtonClick}>
                <SvgTrade />
            </UserRequestButton>
            <UserRequestButton
                type={UserRequestType.Friend}
                key={key}
                canSendUserRequest={canSendUserRequest}
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
    readonly canSendUserRequest: Record<UserRequestType, Accessor<boolean>>;
    readonly onUserRequestButtonClick: (userRequestType: UserRequestType, responderKey: string) => void;
    readonly children: JSXElement;
}

const getButtonColor = (isEnabled: boolean) => isEnabled ? ThemeColor.Accent : ThemeColor.Common;

const UserRequestButton: Component<UserRequestButtonProps> = ({
    type,
    key,
    canSendUserRequest,
    onUserRequestButtonClick,
    children }) => (
    <button
        class={`neon-theme-front-${getButtonColor(canSendUserRequest[type]())}`}
        onClick={() => onUserRequestButtonClick(type, key)}
        disabled={!canSendUserRequest[type]()}>
        {children}
    </button>
);
