﻿using Neon.Application.Repositories.Bases;
using Neon.Domain.Entities;

namespace Neon.Application.Repositories;

public interface ISystemValueRepository :
    ICreateOnlyRepository<SystemValue, string>,
    IReadOnlyRepository<SystemValue, string>,
    IUpdateOnlyRepository<SystemValue, string>;