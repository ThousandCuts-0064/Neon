import SvgDuel from "../../svg/request-type/duel.svg";
import SvgTrade from "../../svg/request-type/trade.svg";
import SvgFriend from "../../svg/request-type/friend.svg";
import SvgCheck from "../../svg/user-action/check.svg";
import SvgCross from "../../svg/user-action/cross.svg";
import UserRequestType from "enums/user-request-type";
import { Component, Accessor, JSXElement } from "solid-js";

interface IncomingUserRequestRowProps {
    readonly createdAt: Date;
    readonly type: UserRequestType;
    readonly key: string;
    readonly username: Accessor<string>;
    readonly onAcceptButtonClick: (userRequestType: UserRequestType, requesterKey: string) => void;
    readonly onDeclineButtonClick: (userRequestType: UserRequestType, requesterKey: string) => void;
}

const renderUserRequestTypeSvg = (userRequestType: UserRequestType) => {
    switch (userRequestType) {
        case UserRequestType.Duel: return <SvgDuel />;
        case UserRequestType.Trade: return <SvgTrade />;
        case UserRequestType.Friend: return <SvgFriend />;
    }
};

const IncomingUserRequestRow: Component<IncomingUserRequestRowProps> = ({
    createdAt,
    type,
    key,
    username,
    onAcceptButtonClick,
    onDeclineButtonClick }) => (
    <div class="neon-lobby-user-row">
        <div class="neon-lobby-user-row-info">
            <div class="neon-theme-front-common">
                {renderUserRequestTypeSvg(type)}
            </div>
        </div>
        <div class="neon-username">{username()}</div>
        <div class="neon-lobby-user-row-menu">
            <UserResponseButton
                type={type}
                key={key}
                onUserResponseButtonClick={onDeclineButtonClick}>
                <SvgCross />
            </UserResponseButton>
            <UserResponseButton
                type={type}
                key={key}
                onUserResponseButtonClick={onAcceptButtonClick}>
                <SvgCheck />
            </UserResponseButton>
        </div>
    </div>
);

export default IncomingUserRequestRow;

interface UserResponseButtonProps {
    readonly type: UserRequestType;
    readonly key: string;
    readonly onUserResponseButtonClick: (userRequestType: UserRequestType, requesterKey: string) => void;
    readonly children: JSXElement;
}

const UserResponseButton: Component<UserResponseButtonProps> = ({
    type,
    key,
    onUserResponseButtonClick,
    children }) => (
    <button
        class="neon-theme-front-accent"
        onClick={() => onUserResponseButtonClick(type, key)}>
        {children}
    </button>
);
