using System.Linq.Expressions;
using System.Text.Json;
using GgdbNet.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GgdbNet.Server.Models;

public class GgdbContext : DbContext
{
    public DbSet<Collection> Collections { get; set; } = null!;
    public DbSet<Game> Games { get; set; } = null!;

    public GgdbContext(DbContextOptions<GgdbContext> opts) : base(opts) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Game>()
            .HasKey(x => new {x.CollectionId, x.GameId});
        
        void JsonListColumn<T>(Expression<Func<Game, IReadOnlyList<T>>> propExpr)
            => model.Entity<Game>()
                .Property(propExpr)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, default(JsonSerializerOptions)),
                    v => JsonSerializer.Deserialize<List<T>>(v, default(JsonSerializerOptions)) ?? new(),
                    JsonListComparer<T>.Instance);

        JsonListColumn(x => x.ReleaseIds);
        JsonListColumn(x => x.AllTitles);
        JsonListColumn(x => x.Genres);
        JsonListColumn(x => x.Themes);
        JsonListColumn(x => x.Platforms);
        JsonListColumn(x => x.Screenshots);

        model.Entity<Collection>()
            .HasIndex(x => x.PublicId)
            .IsUnique();
    }

    private static class JsonListComparer<T>
    {
        public static readonly ValueComparer<IReadOnlyList<T>> Instance = new(
            (a, b) => a!.SequenceEqual(b!),
            l => l.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
            l => l.ToList());
    }
}