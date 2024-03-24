using System.Threading.Tasks;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.DomainEvents.OnUserCreatedDomainEvent;

public class OnUserCreatedDomainEventHandler : INotificationHandler<OnUserCreatedDomainEvent>
{
    private readonly IUserGroupRepositoryAsync _userGroupRepositoryAsync;
        private readonly IGroupRepositoryAsync _groupRepositoryAsync;

    private readonly IAuthenticatedUserService _authenticatedUserService;

    public OnUserCreatedDomainEventHandler(IUserGroupRepositoryAsync userGroupRepositoryAsync,  IAuthenticatedUserService authenticatedUserService, IGroupRepositoryAsync groupRepositoryAsync)
    {
        _userGroupRepositoryAsync = userGroupRepositoryAsync;
        _authenticatedUserService = authenticatedUserService;
        _groupRepositoryAsync = groupRepositoryAsync;

    }

    public async Task Handle(OnUserCreatedDomainEvent request, System.Threading.CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.GroupId))
        {
            var existingUserGroups = await _userGroupRepositoryAsync.GetPagedReponseAsync(0, int.MaxValue, $"UserId:eq:{_authenticatedUserService.UserId}");
            foreach (var group in existingUserGroups.pagedResponse)
            {
                var userGroup = new UserGroup()
                {
                    UserId = request.UserId,
                    GroupId = group.GroupId
                };

                await _userGroupRepositoryAsync.AddAsync(userGroup);
            }
        }
        else
        {
            var existingGroups = await _groupRepositoryAsync.GetPagedReponseAsync(0, int.MaxValue, $"UniqueId:eq:{request.GroupId}");
            
            foreach (var group in existingGroups.pagedResponse)
            {
                var userGroup = new UserGroup()
                {
                    UserId = request.UserId,
                    GroupId = group.Id
                };

                await _userGroupRepositoryAsync.AddAsync(userGroup);
            }
        }

    }
}
