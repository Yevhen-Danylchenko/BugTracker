using BugTracker.Data;
using BugTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

public class BugService
{
    private readonly ApplicationDbContext _context;
    private readonly IDistributedCache _cache;

    public BugService(ApplicationDbContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<List<Bug>> GetAllAsync()
    {
        var cacheKey = "bugs:all";
        var cached = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cached))
        {
            Console.WriteLine("Redis дані з кешу");
            return JsonSerializer.Deserialize<List<Bug>>(cached)!;
        }

        Console.WriteLine("SQLite дані з БД");
        var bugs = await _context.Bugs.ToListAsync();

        if (bugs.Any())
        {
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(bugs));
        }

        return bugs;
    }

    public async Task<List<Bug>> GetFilteredAsync(StatusEnum? status, PriorityEnum? priority, string? assignedTo)
    {
        var bugs = await GetAllAsync();

        if (status.HasValue)
            bugs = bugs.Where(b => b.Status == status.Value).ToList();

        if (priority.HasValue)
            bugs = bugs.Where(b => b.Priority == priority.Value).ToList();

        if (!string.IsNullOrEmpty(assignedTo))
            bugs = bugs.Where(b => b.AssignedTo.Contains(assignedTo, StringComparison.OrdinalIgnoreCase)).ToList();

        return bugs;
    }

    public async Task AddAsync(Bug bug)
    {
        _context.Bugs.Add(bug);
        await _context.SaveChangesAsync();
        await InvalidateCache();
    }

    public async Task UpdateAsync(Bug bug)
    {
        _context.Bugs.Update(bug);
        await _context.SaveChangesAsync();
        await InvalidateCache();
    }

    public async Task DeleteAsync(int id)
    {
        var bug = await _context.Bugs.FindAsync(id);
        if (bug != null)
        {
            _context.Bugs.Remove(bug);
            await _context.SaveChangesAsync();
            await InvalidateCache();
        }
    }

    private async Task InvalidateCache()
    {
        Console.WriteLine("Redis інвалідація кешу");
        await _cache.RemoveAsync("bugs:all");
    }
}

