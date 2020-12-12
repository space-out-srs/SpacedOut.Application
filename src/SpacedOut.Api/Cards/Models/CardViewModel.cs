using SpacedOut.Api.Helpers;
using SpacedOut.Domain.Cards;
using SpacedOut.Domain.Schedules;
using System.Text.Json.Serialization;

namespace SpacedOut.Api.Cards.Models
{
    public class CardViewModel
    {
        public int Id { get; init; }

        [JsonConverter(typeof(SmartEnumValueConverter<ScheduleEnum, string>))]
        public ScheduleEnum? Schedule { get; init; }

        public static CardViewModel FromCard(Card card)
        {
            return new CardViewModel()
            {
                Id = card.Id,
                Schedule = card.Schedule
            };
        }
    }
}