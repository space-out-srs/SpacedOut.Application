using Ardalis.SmartEnum;

namespace SpacedOut.Domain.Schedules
{
    public sealed class ScheduleEnum : SmartEnum<ScheduleEnum, string>
    {
        public static readonly ScheduleEnum LEITNER = LeitnerSchedule.Create();

        internal ScheduleEnum(string name, string value) : base(name, value)
        {
        }
    }
}