using MediatR;

namespace Application.Features.DomainEvents.OnUserCreatedDomainEvent;

public class OnUserCreatedDomainEvent: INotification
{
    public string UserId { get; set; }
    public string GroupId { get; set; } = default;
}
