﻿using Neon.Infrastructure.Configurations.Bases;

namespace Neon.Infrastructure.Configurations;

public class LastActiveAtUpserterConfiguration : IConfigurationBindable
{
    public static string Key => "LastActiveAtUpserter";

    public required int DeltaSeconds { get; init; }
}