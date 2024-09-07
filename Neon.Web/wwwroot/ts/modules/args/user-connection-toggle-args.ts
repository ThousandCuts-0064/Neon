export default class UserConnectionToggledArgs
{
    public constructor(
        public readonly key: string,
        public readonly username: string,
        public readonly isActive: boolean
    ) {}
}
