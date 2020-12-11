using SpacedOut.SharedKernel;

namespace SpacedOut.Domain.Cards.Events
{

    public class CardCreatedEvent : BaseDomainEvent
    {
        private CardCreatedEvent() { }
        public CardCreatedEvent(Card card)
        {
            Card = card;
        }

        public Card Card { get; private set; } = null!; // https://docs.microsoft.com/en-us/ef/core/miscellaneous/nullable-reference-types#non-nullable-properties-and-initialization
    }
}