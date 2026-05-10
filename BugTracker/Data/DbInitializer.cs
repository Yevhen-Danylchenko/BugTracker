using BugTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.Migrate();
            // Check if there are any bugs already in the database.
            if (context.Bugs.Any())
            {
                return; // Database has been seeded
            }
            var bugs = new Bug[]
            {
                new Bug { Title = "Login page error", Description = "Users cannot log in with valid credentials.", Priority = PriorityEnum.High, Status = StatusEnum.Open, AssignedTo = "Alice", ReportedBy = "Bob" },
                new Bug { Title = "UI glitch on dashboard", Description = "Dashboard widgets overlap on smaller screens.", Priority = PriorityEnum.Medium, Status = StatusEnum.Open, AssignedTo = "Charlie", ReportedBy = "Dave" },
                new Bug { Title = "Data export failure", Description = "Exported CSV files are empty.", Priority = PriorityEnum.High, Status = StatusEnum.Open, AssignedTo = "Eve", ReportedBy = "Frank" },
                new Bug { Title = "Search not working", Description = "Search results always return empty.", Priority = PriorityEnum.Critical, Status = StatusEnum.Open, AssignedTo = "Grace", ReportedBy = "Heidi" },
                new Bug { Title = "Profile picture upload error", Description = "Images larger than 2MB fail to upload.", Priority = PriorityEnum.Low, Status = StatusEnum.Resolved, AssignedTo = "Ivan", ReportedBy = "Judy" },
                new Bug { Title = "Notification delay", Description = "Users receive notifications several hours late.", Priority = PriorityEnum.Medium, Status = StatusEnum.InProgress, AssignedTo = "Kate", ReportedBy = "Leo" },
                new Bug { Title = "Payment gateway timeout", Description = "Transactions fail intermittently.", Priority = PriorityEnum.Critical, Status = StatusEnum.Open, AssignedTo = "Mike", ReportedBy = "Nina" },
                new Bug { Title = "Broken links", Description = "Several links on the help page are dead.", Priority = PriorityEnum.Low, Status = StatusEnum.Closed, AssignedTo = "Oscar", ReportedBy = "Paul" },
                new Bug { Title = "Slow page load", Description = "Homepage takes more than 10 seconds to load.", Priority = PriorityEnum.High, Status = StatusEnum.Open, AssignedTo = "Quinn", ReportedBy = "Rita" },
                new Bug { Title = "Session expiration issue", Description = "Users are logged out too quickly.", Priority = PriorityEnum.Medium, Status = StatusEnum.InProgress, AssignedTo = "Sam", ReportedBy = "Tina" },
                new Bug { Title = "Email verification not sent", Description = "New users never receive verification emails.", Priority = PriorityEnum.Critical, Status = StatusEnum.Open, AssignedTo = "Uma", ReportedBy = "Victor" },
                new Bug { Title = "Dark mode styling bug", Description = "Text is unreadable in dark mode.", Priority = PriorityEnum.Low, Status = StatusEnum.Resolved, AssignedTo = "Wendy", ReportedBy = "Xavier" },
                new Bug { Title = "File download corrupted", Description = "Downloaded PDFs cannot be opened.", Priority = PriorityEnum.High, Status = StatusEnum.Open, AssignedTo = "Yara", ReportedBy = "Zack" },
                new Bug { Title = "Comment section crash", Description = "Adding a comment causes app crash.", Priority = PriorityEnum.Critical, Status = StatusEnum.Open, AssignedTo = "Alice", ReportedBy = "Bob" }
            };

            foreach (var bug in bugs)
            {
                context.Bugs.Add(bug);
            }
            context.SaveChanges();
        }
    }
}
