import UserRequestType from "../enums/user-request-type";
import User from "./user";
import { createStore, SetStoreFunction, produce, createMutable } from "solid-js/store";

export default class Users {
    private users = new Map<string, User>();
    private usersSet = new Map<string, SetStoreFunction<User>>;

    public get(key: string): User {
        return this.users.get(key)!;
    }

    public getOrSet(key: string, username: string, canReceive: Record<UserRequestType, boolean> = {
        Duel: true,
        Friend: true,
        Trade: true,
    }): User {
        const value = this.users.get(key);

        if (value !== undefined)
            return value;

        const [user, setUser] = createStore(createMutable(new User(key, username, canReceive)));

        this.users.set(key, user);
        this.usersSet.set(key, setUser);

        return user;
    }

    public mutate(key: string, mutation: (user: User) => void): void {
        this.usersSet.get(key)!(produce(mutation));
    }
}
