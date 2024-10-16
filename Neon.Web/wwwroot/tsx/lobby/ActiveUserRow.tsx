﻿import SvgDuel from "../../svg/request-type/duel.svg";
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

const ActiveUserRow: Component<ActiveUserRowProps> = props => (
    <div class="neon-lobby-user-row">
        <div class="neon-username">{props.username}</div>
        <div class="neon-lobby-user-row-menu">
            <UserRequestButton
                type={UserRequestType.Duel}
                key={props.key}
                canReceive={props.canReceive}
                onUserRequestButtonClick={props.onUserRequestButtonClick}>
                <SvgDuel />
            </UserRequestButton>
            <UserRequestButton
                type={UserRequestType.Trade}
                key={props.key}
                canReceive={props.canReceive}
                onUserRequestButtonClick={props.onUserRequestButtonClick}>
                <SvgTrade />
            </UserRequestButton>
            <UserRequestButton
                type={UserRequestType.Friend}
                key={props.key}
                canReceive={props.canReceive}
                onUserRequestButtonClick={props.onUserRequestButtonClick}>
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

const UserRequestButton: Component<UserRequestButtonProps> = props => (
    <button
        class={`neon-theme-front-${getButtonColor(props.canReceive[props.type])}`}
        onClick={() => props.onUserRequestButtonClick(props.type, props.key)}
        disabled={!props.canReceive[props.type]}>
        {props.children}
    </button>
);
