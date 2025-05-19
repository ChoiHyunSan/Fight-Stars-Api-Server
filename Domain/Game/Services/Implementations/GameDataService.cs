using Microsoft.EntityFrameworkCore;
using System.Text.Json;

public class GameDataService : IGameDataService
{
    private readonly AppDbContext _context;

    public GameDataService(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateGameDataFileAsync()
    {
        // Brawler 목록
        var brawlers = await _context.Brawlers
            .Select(b => new {
                b.Id,
                b.Name,
                b.Description,
                b.goldPrice
            })
            .ToListAsync();

        // Skin 목록
        var skins = await _context.Skins
            .Select(s => new {
                s.Id,
                s.BrawlerId,
                s.Name,
                s.Description,
                s.zemPrice
            })
            .ToListAsync();

        string brawlerJson = JsonSerializer.Serialize(brawlers, new JsonSerializerOptions { WriteIndented = true });
        string skinJson = JsonSerializer.Serialize(skins, new JsonSerializerOptions { WriteIndented = true });

        Console.WriteLine("Brawlers:");
        Console.WriteLine(brawlerJson);

        Console.WriteLine("Skins:");
        Console.WriteLine(skinJson);

        await File.WriteAllTextAsync("brawlers.json", brawlerJson);
        await File.WriteAllTextAsync("skins.json", skinJson);
    }
}