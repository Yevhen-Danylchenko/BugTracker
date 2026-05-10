using BugTracker.Models;

namespace BugTracker.ViewModels
{
    public class DashboardViewModel
    {
        public Dictionary<StatusEnum, int> StatsByStatus { get; set; } = new();
        public int CriticalCount { get; set; }
        public List<Bug> LastChanges { get; set; } = new();
    }
}
