export default class UserConnectionToggledArgs
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
