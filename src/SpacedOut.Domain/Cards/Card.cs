using SpacedOut.Domain.Cards.Events;
using SpacedOut.Domain.Schedules;
using SpacedOut.SharedKernel;

namespace SpacedOut.Domain.Cards
{
    public class Card : BaseEntity
    {
        private Card() { }
        public Card(ScheduleEnum schedule)
        {
            Schedule = schedule;

            Events.Add(new CardCreatedEvent(this));
        }

        public ScheduleEnum Schedule { get; private set; } = ScheduleEnum.LEITNER;

        public void DoNothing()
        {

        }
    }
}