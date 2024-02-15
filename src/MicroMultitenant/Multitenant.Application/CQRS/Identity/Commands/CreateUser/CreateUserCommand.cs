﻿using MediatR;
using Multitenant.Application.CQRS.Identity.Commands.CreateUser.Resources;

namespace Multitenant.Application.CQRS.Identity.Commands.CreateUser
{
    public class CreateUserCommand : CreateUserRequest, IRequest<CreateUserResponse>
    {
        public string Rol { get; set; } = null!;
        public string OrganizationName { get; set; } = null!;
        public string? Token { get; set; }

    }
}