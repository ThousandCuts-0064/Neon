import SvgDuel from "../../svg/request-type/duel.svg";
import SvgTrade from "../../svg/request-type/trade.svg";
import SvgFriend from "../../svg/request-type/friend.svg";

import { Component } from "solid-js";

interface UserMessageProps {
    readonly usernameClass: string,
    readonly usernamePrefix: string;
    readonly username: string;
    readonly usernameSuffix: string;
    readonly messageClass: string;
    readonly message: string;
}

const UserMessage: Component<UserMessageProps> = ({
    usernameClass,
    usernamePrefix,
    username,
    usernameSuffix,
    messageClass,
    message }) => (
    <li class="neon-user-message">
        <span class={usernameClass}>{usernamePrefix}</span><span class={usernameClass}>{username}</span><span class={usernameClass}>{usernameSuffix}</span>
        <span class={messageClass}>{message}</span>
    </li>
);

export default UserMessage;