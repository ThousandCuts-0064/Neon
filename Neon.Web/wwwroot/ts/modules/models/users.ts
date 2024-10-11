import User from "./user";

class UsersSignals
{
    private users: Map<string, User>;

    public getOrSet(key: string , factory: ()=> void): User {
        return this.users.getOrSet(key, factory);
    }
}
