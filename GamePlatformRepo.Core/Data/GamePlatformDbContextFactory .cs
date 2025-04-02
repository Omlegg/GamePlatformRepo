using GamePlatformRepo.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

public class GamePlatformDbContextFactory : IDesignTimeDbContextFactory<GamePlatformDbContext>
{
    public GamePlatformDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<GamePlatformDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=GamePlatformDb;Integrated Security=true;TrustServerCertificate=True;");

        return new GamePlatformDbContext(optionsBuilder.Options);
    }
}
