namespace SpacedOut.Domain.Schedules
{
    public static class LeitnerSchedule
    {
        public static ScheduleEnum Create()
        {
            return new ScheduleEnum("Leitner Schedule", "LEITNER");
        }
    }
}