﻿namespace Neon.Domain.Enums;

public enum RegisterResult
{
    Error,
    UsernameTaken,
    UsernameInvalidCharacters,
    WeakPassword,
    Success
}