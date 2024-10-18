import SvgDuel from "../../svg/request-type/duel.svg";
import SvgTrade from "../../svg/request-type/trade.svg";
import SvgFriend from "../../svg/request-type/friend.svg";
import SvgCross from "../../svg/user-action/cross.svg";
import UserRequestType from "enums/user-request-type";
import { Component, JSXElement } from "solid-js";

interface OutgoingUserRequestRowProps {
    readonly createdAt: Date;
    readonly type: UserRequestType;
    readonly key: string;
    readonly username: string;
    readonly onCancelButtonClick: (userRequestType: UserRequestType, responderKey: string) => void;
}

const renderUserRequestTypeSvg = (userRequestType: UserRequestType) => {
    switch (userRequestType) {
        case UserRequestType.Duel: return <SvgDuel />;
        case UserRequestType.Trade: return <SvgTrade />;
        case UserRequestType.Friend: return <SvgFriend />;
    }
};

const OutgoingUserRequestRow: Component<OutgoingUserRequestRowProps> = props => (
    <div class="neon-lobby-user-row">
        <div class="neon-lobby-user-row-info">
            <div class="neon-theme-front-common">
                {renderUserRequestTypeSvg(props.type)}
            </div>
        </div>
        <div class="neon-username">{props.username}</div>
        <div class="neon-lobby-user-row-menu">
            <UserRequestButton
                type={props.type}
                key={props.key}
                onUserRequestButtonClick={props.onCancelButtonClick}>
                <SvgCross />
            </UserRequestButton>
        </div>
    </div>
);

export default OutgoingUserRequestRow;

interface UserRequestButtonProps {
    readonly type: UserRequestType;
    readonly key: string;
    readonly onUserRequestButtonClick: (userRequestType: UserRequestType, requesterKey: string) => void;
    readonly children: JSXElement;
}

const UserRequestButton: Component<UserRequestButtonProps> = props => (
    <button
        class="neon-theme-front-accent"
        onClick={() => props.onUserRequestButtonClick(props.type, props.key)}>
        {props.children}
    </button>
);
