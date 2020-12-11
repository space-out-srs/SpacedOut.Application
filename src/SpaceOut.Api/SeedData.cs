using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpacedOut.Domain.Cards;
using SpacedOut.Domain.Schedules;
using SpacedOut.Infrastucture.Data;
using System;
using System.Linq;

namespace SpaceOut.Api
{
    public static class SeedData
    {
        public static readonly Card Card1 = new Card(ScheduleEnum.LEITNER);
        public static readonly Card Card2 = new Card(ScheduleEnum.LEITNER);
        public static readonly Card Card3 = new Card(ScheduleEnum.LEITNER);

        public static void Initialize(IServiceProvider serviceProvider)
        {
            var dbContextOptions = serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>();
            using (var dbContext = new AppDbContext(dbContextOptions, null))
            {
                if (dbContext.Cards.Any())
                {
                    return; // DB has been seeded
                }

                PopulateTestData(dbContext);
            }
        }

        public static void PopulateTestData(AppDbContext dbContext)
        {
            foreach (var item in dbContext.Cards)
            {
                dbContext.Remove(item);
            }

            dbContext.SaveChanges();

            dbContext.Cards.Add(Card1);
            dbContext.Cards.Add(Card2);
            dbContext.Cards.Add(Card3);

            dbContext.SaveChanges();
        }
    }
}
