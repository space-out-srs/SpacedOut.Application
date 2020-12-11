using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpacedOut.Domain.Cards;
using SpacedOut.Domain.Schedules;
using System;
using System.Linq;

namespace SpacedOut.Infrastucture.Domain.Cards
{
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        const int SCHEDULE_MAX_LENGTH = 50;

        public void Configure(EntityTypeBuilder<Card> builder)
        {
            CheckScheduleMaxLength();

            builder
                .Property(p => p.Schedule)
                .HasConversion(
                    p => p.Value,
                    p => ScheduleEnum.FromValue(p)
                )
                .HasMaxLength(SCHEDULE_MAX_LENGTH);
        }

        private static void CheckScheduleMaxLength()
        {
            var maxLength = ScheduleEnum
                .List
                .Select(s => s.Value.Length)
                .DefaultIfEmpty()
                .Max();

            if (maxLength > SCHEDULE_MAX_LENGTH)
            {
                throw new InvalidOperationException($"ScheduleEnum value length must be less than or equal to {SCHEDULE_MAX_LENGTH}");
            }
        }
    }
}