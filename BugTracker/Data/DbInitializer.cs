using BugTracker.Models;

namespace BugTracker.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            // Check if there are any bugs already in the database.
            if (context.Bugs.Any())
            {
                return; // Database has been seeded
            }
            var bugs = new Bug[]
            {
                new Bug { Title = "Login page error", Description = "Users cannot log in with valid credentials.", Priority = PriorityEnum.High, Status = StatusEnum.Open, AssignedTo = "Alice", ReportedBy = "Bob" },
                new Bug { Title = "UI glitch on dashboard", Description = "Dashboard widgets overlap on smaller screens.", Priority = PriorityEnum.Medium, Status = StatusEnum.Open, AssignedTo = "Charlie", ReportedBy = "Dave" },
                new Bug { Title = "Data export failure", Description = "Exported CSV files are empty.", Priority = PriorityEnum.High, Status = StatusEnum.Open, AssignedTo = "Eve", ReportedBy = "Frank" }
            };
            foreach (var bug in bugs)
            {
                context.Bugs.Add(bug);
            }
            context.SaveChanges();
        }
    }
}
