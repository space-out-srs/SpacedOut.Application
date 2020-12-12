using SpacedOut.Api.Helpers;
using SpacedOut.Domain.Cards;
using SpacedOut.Domain.Schedules;
using System.Text.Json.Serialization;

namespace SpacedOut.Api.ViewModels
{
    public class CardModel
    {
        public int Id { get; init; }

        [JsonConverter(typeof(SmartEnumValueConverter<ScheduleEnum, string>))]
        public ScheduleEnum? Schedule { get; init; }

        public static CardModel FromCard(Card card)
        {
            return new CardModel()
            {
                Id = card.Id,
                Schedule = card.Schedule
            };
        }
    }
}