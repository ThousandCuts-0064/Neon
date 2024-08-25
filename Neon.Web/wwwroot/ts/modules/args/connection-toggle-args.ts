export default class ConnectionToggleArgs
{
    public readonly username: string;
    public readonly isActive: boolean;

    public constructor(username: string,
    isActive: boolean
    ) {
        this.username = username;
        this.isActive = isActive;
    }
}
