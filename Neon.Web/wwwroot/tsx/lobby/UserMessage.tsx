import { Component } from "solid-js";

interface UserMessageProps {
    readonly usernameClass: string,
    readonly usernamePrefix: string;
    readonly username: string;
    readonly usernameSuffix: string;
    readonly messageClass: string;
    readonly message: string;
}

const UserMessage: Component<UserMessageProps> = props => (
    <li class="neon-user-message">
        <span class={props.usernameClass}>{props.usernamePrefix}</span>
        <span class={props.usernameClass}>{props.username}</span>
        <span class={props.usernameClass}>{props.usernameSuffix}</span>
        <span> </span>
        <span class={props.messageClass}>{props.message}</span>
    </li>
);

export default UserMessage;