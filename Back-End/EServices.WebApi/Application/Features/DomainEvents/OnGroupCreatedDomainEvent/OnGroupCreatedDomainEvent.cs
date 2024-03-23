
using MediatR;

namespace Application.Features.DomainEvents.OnGroupCreatedDomainEvent
{
    public class OnGroupCreatedDomainEvent : INotification
    {
        public int GroupId { get; set; }
    }

}