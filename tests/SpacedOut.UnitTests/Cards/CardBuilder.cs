using SpacedOut.Domain.Cards;
using SpacedOut.Domain.Schedules;

namespace SpacedOut.UnitTests.Cards
{
    // https://ardalis.com/improve-tests-with-the-builder-pattern-for-test-data
    public class CardBuilder
    {
        private Card _card = null!;

        public CardBuilder WithDefaultValues()
        {
            _card = new Card(ScheduleEnum.LEITNER);

            return this;
        }

        public Card Build() => _card;
    }
}