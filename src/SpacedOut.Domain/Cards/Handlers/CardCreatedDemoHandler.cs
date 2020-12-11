using SpacedOut.Domain.Cards.Events;
using SpacedOut.SharedKernal.Interfaces;
using System.Threading.Tasks;

namespace SpacedOut.Domain.Cards.Handlers
{
    public class CardCreatedDemoHandler : IHandle<CardCreatedEvent>
    {
        private readonly ICardCreatedNotification _notification;

        public CardCreatedDemoHandler(ICardCreatedNotification notification)
        {
            _notification = notification;
        }

        public async Task Handle(CardCreatedEvent domainEvent)
        {
            await _notification.Send(domainEvent.Card);
        }
    }

    public interface ICardCreatedNotification
    {
        Task Send(Card card);
    }
}