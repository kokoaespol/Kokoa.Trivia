using Kokoa.Trivia.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Kokoa.Trivia.Api.Database;

public class TriviaDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public TriviaDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<TriviaQuestion> TriviaQuestions { get; set; } = null!;
    public DbSet<TriviaOption> TriviaOptions { get; set; } = null!;
    public DbSet<TriviaAnswer> TriviaAnswers { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
    }
}