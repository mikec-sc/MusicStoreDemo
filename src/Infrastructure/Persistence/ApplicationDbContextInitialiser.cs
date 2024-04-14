using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicStore.Infrastructure.Identity;

namespace MusicStore.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger,
        ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole("Administrator");

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser
            { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }

        // Default data
        // Seed, if necessary
        if (!_context.InventoryItems.Any())
        {
            _context.InventoryItems.Add(new()
            {
                Id = 1, Title = "Abbey Road", Artist = "The Beatles", Year = 1969, Genre = "Rock",
                Price = 12.99m
            });
            _context.InventoryItems.Add(new()
            {
                Id = 2, Title = "Thriller", Artist = "Michael Jackson", Year = 1982, Genre = "Pop",
                Price = 14.99m
            });
            _context.InventoryItems.Add(new()
            {
                Id = 3, Title = "Dark Side of the Moon", Artist = "Pink Floyd", Year = 1973,
                Genre = "Progressive Rock", Price = 11.99m
            });
            _context.InventoryItems.Add(new()
            {
                Id = 4, Title = "Back in Black", Artist = "AC/DC", Year = 1980, Genre = "Rock",
                Price = 10.99m
            });
            _context.InventoryItems.Add(new()
            {
                Id = 5, Title = "Rumours", Artist = "Fleetwood Mac", Year = 1977, Genre = "Rock",
                Price = 13.49m
            });

            await _context.SaveChangesAsync();
        }
    }
}
