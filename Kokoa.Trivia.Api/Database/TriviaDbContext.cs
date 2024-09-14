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
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TriviaQuestion>()
            .HasKey(x => x.Id)
            .HasName("TRIVIA_QUESTION_ID");

        modelBuilder.Entity<TriviaQuestion>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<TriviaQuestion>().HasMany(x => x.TriviaOptions)
            .WithOne(x => x.TriviaQuestion)
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.TriviaQuestionId);

        modelBuilder.Entity<TriviaOption>().HasKey(x => x.Id).HasName("TRIVIA_OPTION_ID");
        modelBuilder.Entity<TriviaOption>().Property(x => x.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<TriviaAnswer>().HasKey(x => new { x.TriviaQuestionId, x.TriviaOptionId });

        modelBuilder.Entity<TriviaAnswer>()
            .HasOne(x => x.TriviaQuestion)
            .WithOne()
            .HasForeignKey<TriviaQuestion>(x => x.Id)
            .HasPrincipalKey<TriviaAnswer>(x => x.TriviaQuestionId);

        modelBuilder.Entity<TriviaAnswer>()
            .HasOne(x => x.TriviaOption)
            .WithOne()
            .HasForeignKey<TriviaOption>(x => x.Id)
            .HasPrincipalKey<TriviaAnswer>(x => x.TriviaOptionId);
    }
}