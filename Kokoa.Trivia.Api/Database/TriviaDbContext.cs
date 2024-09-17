using EntityFramework.Exceptions.PostgreSQL;
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
        optionsBuilder.UseExceptionProcessor();
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TriviaAnswer>().HasKey(x => new { x.TriviaOptionId, x.TriviaQuestionId });

        modelBuilder.Entity<TriviaOption>()
            .Property(x => x.Id)
            .UseIdentityAlwaysColumn()
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<TriviaQuestion>()
            .Property(x => x.Id)
            .UseIdentityAlwaysColumn()
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<TriviaQuestion>()
            .HasOne(x => x.TriviaAnswer)
            .WithOne(x => x.TriviaQuestion)
            .HasForeignKey<TriviaQuestion>(x => x.Id)
            .HasPrincipalKey<TriviaAnswer>(x => x.TriviaQuestionId);

        // modelBuilder.Entity<TriviaQuestion>()
        //     .HasMany(x => x.TriviaOptions)
        //     .WithOne()
        //     .HasForeignKey(x => x.TriviaQuestionId)
        //     .HasPrincipalKey(x => x.Id);
    }
}
