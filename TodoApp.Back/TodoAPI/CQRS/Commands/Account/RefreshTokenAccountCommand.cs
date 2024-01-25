﻿using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.CQRS.Commands.Account
{
    public record RefreshTokenAccountCommand(AuthorizationModel model) : IRequest<IAPIResponse<AuthorizationModel>>;
}
