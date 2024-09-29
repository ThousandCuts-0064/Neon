import SvgDuel from "../../svg/request-type/duel.svg";
import SvgTrade from "../../svg/request-type/trade.svg";
import SvgFriend from "../../svg/request-type/friend.svg";
import SvgCross from "../../svg/user-action/cross.svg";
import UserRequestType from "enums/user-request-type";
import { Component, Accessor, JSXElement } from "solid-js";

interface OutgoingUserRequestRowProps {
    readonly createdAt: Date;
    readonly type: UserRequestType;
    readonly key: string;
    readonly username: Accessor<string>;
    readonly onCancelButtonClick: (userRequestType: UserRequestType, responderKey: string) => void;
}

const renderUserRequestTypeSvg = (userRequestType: UserRequestType) => {
    switch (userRequestType) {
        case UserRequestType.Duel: return <SvgDuel />;
        case UserRequestType.Trade: return <SvgTrade />;
        case UserRequestType.Friend: return <SvgFriend />;
    }
};

const OutgoingUserRequestRow: Component<OutgoingUserRequestRowProps> = ({
    createdAt,
    type,
    key,
    username,
    onCancelButtonClick }) => (
    <div class="neon-lobby-user-row">
        <div class="neon-lobby-user-row-info">
            <div class="neon-theme-front-common">
                {renderUserRequestTypeSvg(type)}
            </div>
        </div>
        <div class="neon-username">{username()}</div>
        <div class="neon-lobby-user-row-menu">
            <UserRequestButton
                type={type}
                key={key}
                onUserRequestButtonClick={onCancelButtonClick}>
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

const UserRequestButton: Component<UserRequestButtonProps> = ({
    type,
    key,
    onUserRequestButtonClick,
    children }) => (
    <button
        class="neon-theme-front-accent"
        onClick={() => onUserRequestButtonClick(type, key)}>
        {children}
    </button>
);
