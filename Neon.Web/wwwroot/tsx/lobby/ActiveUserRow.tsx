import SvgDuel from "../../svg/request-type/duel.svg";
import SvgTrade from "../../svg/request-type/trade.svg";
import SvgFriend from "../../svg/request-type/friend.svg";
import UserRequestType from "enums/user-request-type";
import ThemeColor from "enums/theme-color";
import { Component, JSXElement } from "solid-js";

export interface ActiveUserRowProps {
    readonly key: string;
    readonly username: string;
    readonly canReceive: Record<UserRequestType, boolean>;
    readonly onUserRequestButtonClick: (userRequestType: UserRequestType, responderKey: string) => void;
}

const ActiveUserRow: Component<ActiveUserRowProps> = ({
    key,
    username,
    canReceive,
    onUserRequestButtonClick }) => (
    <div class="neon-lobby-user-row">
        <div class="neon-username">{username}</div>
        <div class="neon-lobby-user-row-menu">
            <UserRequestButton
                type={UserRequestType.Duel}
                key={key}
                canReceive={canReceive}
                onUserRequestButtonClick={onUserRequestButtonClick}>
                <SvgDuel />
            </UserRequestButton>
            <UserRequestButton
                type={UserRequestType.Trade}
                key={key}
                canReceive={canReceive}
                onUserRequestButtonClick={onUserRequestButtonClick}>
                <SvgTrade />
            </UserRequestButton>
            <UserRequestButton
                type={UserRequestType.Friend}
                key={key}
                canReceive={canReceive}
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
    readonly canReceive: Record<UserRequestType, boolean>;
    readonly onUserRequestButtonClick: (userRequestType: UserRequestType, responderKey: string) => void;
    readonly children: JSXElement;
}

const getButtonColor = (isEnabled: boolean) => isEnabled ? ThemeColor.Accent : ThemeColor.Common;

const UserRequestButton: Component<UserRequestButtonProps> = ({
    type,
    key,
    canReceive,
    onUserRequestButtonClick,
    children }) => (
    <button
        class={`neon-theme-front-${getButtonColor(canReceive[type])}`}
        onClick={() => onUserRequestButtonClick(type, key)}
        disabled={!canReceive[type]}>
        {children}
    </button>
);
