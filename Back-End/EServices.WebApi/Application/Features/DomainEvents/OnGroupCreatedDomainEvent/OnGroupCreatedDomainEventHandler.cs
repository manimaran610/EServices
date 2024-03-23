using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Enums;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.DomainEvents.OnGroupCreatedDomainEvent
{
    public class OnGroupCreatedDomainEventHandler : INotificationHandler<OnGroupCreatedDomainEvent>
    {
        private readonly IUserGroupRepositoryAsync _userGroupRepositoryAsync;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IAuthenticatedUserService _authenticatedUserService;

        public OnGroupCreatedDomainEventHandler(IUserGroupRepositoryAsync userGroupRepositoryAsync, IAuthenticatedUserService authenticatedUserService,  UserManager<ApplicationUser> userManager)
        {
            _userGroupRepositoryAsync = userGroupRepositoryAsync;
            _authenticatedUserService = authenticatedUserService;
            _userManager = userManager;

        }

        public async Task Handle(OnGroupCreatedDomainEvent request, System.Threading.CancellationToken cancellationToken)
        {
            var superAdmins = await _userManager.GetUsersInRoleAsync(Roles.SuperAdmin.ToString());
            foreach (var sa in superAdmins)
            {
               var userGroup = new UserGroup()
                {
                    UserId = sa.Id,
                    GroupId = request.GroupId
                };

                await _userGroupRepositoryAsync.AddAsync(userGroup);   
            }

        }
    }
}